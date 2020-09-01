using System;
using System.Collections.Generic;
using map;
using TABLE;
using UnityEngine;

public class CSMapManager : CSInfo<CSMapManager>
{
    private MAPINFO _tblMapInfo;

    public MAPINFO mTbleMapInfo
    {
        get
        {
            if (_tblMapInfo != null && _tblMapInfo.id == CSScene.GetMapID())
                return _tblMapInfo;
            MapInfoTableManager.Instance.TryGetValue(CSScene.GetMapID(), out _tblMapInfo);
            return _tblMapInfo;
        }
    }

    #region 雷达视野处理

    public Dictionary<long, CSMapAvatarInfo> MapAvatarInfoDic = new Dictionary<long, CSMapAvatarInfo>(128);
    //存储Avatar的key，方便遍历
    public List<long> MapAvatarInfoList = new List<long>(128);

    
    public Dictionary<int, Stack<CSMapAvatarInfo>> MapAvatarPoolListDic =
        new Dictionary<int, Stack<CSMapAvatarInfo>>(8);

    public Dictionary<int, ILBetterList<CSMapAvatarInfo>> MapAvatarInfoListDic =
        new Dictionary<int, ILBetterList<CSMapAvatarInfo>>(8);

    public Queue<CSMapAvatarInfo> WaitLoadList = new Queue<CSMapAvatarInfo>();

  #endregion

    #region 小地图视野处理

    public CSBetterDic<long, CSMapAvatarInfo> BigMapAvatarInfoDic = new CSBetterDic<long, CSMapAvatarInfo>(false, true);
    public Queue<CSMapAvatarInfo> BigMapWaitLoadList = new Queue<CSMapAvatarInfo>();

    #endregion

    public map.UpdateViewResponse initViewData;

    public Dictionary<long, map.SmallViewTeammateNtf> mActMapViewPoint;

    //小地图尺寸，固定此数值
    public Vector3 mMimSize = new Vector3(); //小地图缩放后真实尺寸
    public Vector3 mMapScale = new Vector3(); //小地图缩放比

    public Vector3 mHalfScreenSize;

    //private Vector3 mMapSize = new Vector3(); //大地图尺寸
    public Vector3 mMimMapSize = new Vector3(); //实际加载的地图尺寸
    public static Vector2 mMapOneCellPos = Vector2.zero; //每格坐标大小

    public int recommendLv = 0;

    public void Init()
    {
        mHalfScreenSize = new Vector2(Screen.width / 2, Screen.height / 2);
        mClientEvent.AddEvent(CEvent.Role_ChangeMapId, ChangeMapId);
        ResetMap();
    }

    /// <summary>
    /// 雷达创建晚于该协议刷新，临时存储，UI创建后刷新数据
    /// </summary>
    public void InitViewResponse(map.UpdateViewResponse rsp)
    {
        if (initViewData != null) return;
        initViewData = rsp;
    }

    public void RegisterCEvent()
    {
        mClientEvent.SendEvent(CEvent.InitMiniMapCallBack);
    }

    public void ResetMap()
    {
        ReinitMapInfo();
        ChangeSceneClearPoolObj();
        mClientEvent.SendEvent(CEvent.UpdateMapInfo);
    }

    private void ReinitMapInfo()
    {
        if (mTbleMapInfo == null) return;
        string[] str = mTbleMapInfo.mapSize.Split('#');
        if (str.Length < 4) return;
        mMimMapSize.x = Convert.ToSingle(str[2]);
        mMimMapSize.y = Convert.ToSingle(str[3]);

        string[] str2 = mTbleMapInfo.mapSize1.Split('#');
        if (str2.Length < 2) return;
        mMapScale.x = Convert.ToSingle(str2[0]) / 10000;
        mMapScale.y = Convert.ToSingle(str2[1]) / 10000;

        mMimSize.x = Convert.ToSingle(str[0]) * mMapScale.x;
        mMimSize.y = Convert.ToSingle(str[1]) * mMapScale.y;

        mMapOneCellPos.x = CSCell.Size.x * mMapScale.x;
        mMapOneCellPos.y = CSCell.Size.y * mMapScale.y;
    }

    #region Event

    private void ChangeMapId(uint id, object data)
    {
        ResetMap();
    }

  #endregion

     #region 雷达视野处理

    private CSMapAvatarInfo avatarInfo;

    public void Update()
    {
        if (WaitLoadList.Count > 0)
        {
            avatarInfo = WaitLoadList.Dequeue();
            if (MapAvatarInfoDic.ContainsKey(avatarInfo.ID))
                avatarInfo.Show();
        }

        if (BigMapWaitLoadList.Count > 0)
        {
            CSMapAvatarInfo bigavatarInfo = BigMapWaitLoadList.Dequeue();
            if (BigMapAvatarInfoDic.ContainsKey(bigavatarInfo.ID))
                bigavatarInfo.Show();
        }
    }

    public CSMapAvatarInfo GetPoolItem<T>(MapAvaterType type) where T : CSMapAvatarInfo, new()
    {
        return GetPoolItem<T>((int) type);
    }

    Stack<CSMapAvatarInfo> infoStack;
    public CSMapAvatarInfo GetPoolItem<T>(int type) where T : CSMapAvatarInfo, new()
    {
        if (MapAvatarPoolListDic.TryGetValue(type, out infoStack))
        {
            if (infoStack.Count > 0)
            {
                return infoStack.Pop();
            }
        }

        return new T();
    }


    public void AddMapAvatar(CSMapAvatarInfo avatar, MapAvaterType type)
    {
        AddMapAvatar(avatar, (int) type);
    }

    public bool AddMapAvatar(CSMapAvatarInfo avatar, int type)
    {
        if (GetMapAvatar(avatar.ID) == null)
        {
            MapAvatarInfoDic.Add(avatar.ID, avatar);
            MapAvatarInfoList.Add(avatar.ID);
            if (!MapAvatarInfoListDic.ContainsKey(type))
                MapAvatarInfoListDic.Add(type, new ILBetterList<CSMapAvatarInfo>(64));
            MapAvatarInfoListDic[type].Add(avatar);
            return true;
        }
        else
            MapAvatarInfoDic[avatar.ID] = avatar;

        return false;
    }

    public CSMapAvatarInfo GetMapAvatar(long id)
    {
        CSMapAvatarInfo info = null;
        if(MapAvatarInfoDic != null && MapAvatarInfoDic.TryGetValue(id, out info))
        {
            return info;
        }
        return null;
    }

    public void AddMapAvatarQueue(CSMapAvatarInfo avatar)
    {
        WaitLoadList.Enqueue(avatar);
    }

    public void AddMapAvatarLoad(CSMapAvatarInfo avatar, MapAvaterType type)
    {
        if (AddMapAvatar(avatar, (int) type))
            AddMapAvatarQueue(avatar);
    }

    public void RemoveAvatar(long id)
    {
        RemoveAvatar(id, GetMapAvatar(id));
    }

    public void RemoveAvatar(long id, CSMapAvatarInfo avatar)
    {
        if (avatar == null) return;
        avatar.Dispose();
        int t = (int) avatar.AvatarType;
        if (MapAvatarInfoListDic.ContainsKey(t))
            MapAvatarInfoListDic[t].Remove(avatar);
        MapAvatarInfoDic.Remove(avatar.ID);
        MapAvatarInfoList.Remove(avatar.ID);
        if (!MapAvatarPoolListDic.ContainsKey(t))
            MapAvatarPoolListDic.Add(t, new Stack<CSMapAvatarInfo>(128));
        MapAvatarPoolListDic[t].Push(avatar);
    }

    public void RemoveAll()
    {
        if(MapAvatarInfoList != null && MapAvatarInfoDic != null)
        {
            CSMapAvatarInfo info;
            for(int i = 0; i < MapAvatarInfoList.Count; i++)
            {
                if(MapAvatarInfoDic.TryGetValue(MapAvatarInfoList[i], out info))
                {
                    info.Destroy();
                }
            }
        }

        ChangeSceneClearPoolObj();

        MapAvatarInfoDic.Clear();
        MapAvatarInfoListDic.Clear();
        MapAvatarPoolListDic.Clear();
        WaitLoadList.Clear();
        MapAvatarInfoList.Clear();
    }

    public ILBetterList<CSMapAvatarInfo> GetAvaterByType(MapAvaterType type)
    {
        ILBetterList<CSMapAvatarInfo> info = null;
        MapAvatarInfoListDic.TryGetValue((int) type, out info);
        return info;
    }

    public void ChangeSceneClearPoolObj()
    {
        if (MapAvatarPoolListDic == null) return;
        for (var dic = MapAvatarPoolListDic.GetEnumerator(); dic.MoveNext();)
        {
            Stack<CSMapAvatarInfo> avatarStack = dic.Current.Value;
            if (avatarStack != null && avatarStack.Count > 0)
            {
                using (var avatar = avatarStack.GetEnumerator())
                {
                    while (avatar.MoveNext())
                        avatar.Current?.Destroy();
                }

                avatarStack.Clear();
            }
        }
    }

    public void ClearPoolObjByType(MapAvaterType type)
    {
        if (MapAvatarPoolListDic == null) return;
        Stack<CSMapAvatarInfo> avatarStack;
        if (!MapAvatarPoolListDic.TryGetValue((int) type, out avatarStack)) return;
        if (avatarStack != null && avatarStack.Count > 0)
        {
            using (var avatar = avatarStack.GetEnumerator())
            {
                while (avatar.MoveNext())
                    avatar.Current?.Destroy();
            }

            avatarStack.Clear();
        }
    }

    public int GetPlayerType(CSMapPlayerInfo playerInfo)
    {
        if (playerInfo == null) return 0;

        if (playerInfo.Teamid != 0 && CSMainPlayerInfo.Instance.TeamId == playerInfo.Teamid)
        {
            return 1;
        }

        return 2;
    }

  #endregion

    #region 小地图视野处理

    public void AddBigMapAvatar(CSMapAvatarInfo avatar, MapAvaterType type)
    {
        AddBigMapAvatar(avatar, (int) type);
    }

    public bool AddBigMapAvatar(CSMapAvatarInfo avatar, int type)
    {
        if (GetBigMapAvatar(avatar.ID) == null)
        {
            BigMapAvatarInfoDic.Add(avatar.ID, avatar);
            /*if (!MapAvatarInfoListDic.ContainsKey(type))
                MapAvatarInfoListDic.Add(type, new ILBetterList<CSMapAvatarInfo>(64));
            MapAvatarInfoListDic[type].Add(avatar);*/
            return true;
        }

        //BigMapAvatarInfoDic[avatar.ID] = avatar;

        return false;
    }

    public CSMapAvatarInfo GetBigMapAvatar(long id)
    {
        if (BigMapAvatarInfoDic != null && BigMapAvatarInfoDic.ContainsKey(id))
            return BigMapAvatarInfoDic[id];
        return null;
    }

    public void AddBigMapAvatarQueue(CSMapAvatarInfo avatar)
    {
        BigMapWaitLoadList.Enqueue(avatar);
    }

    public void AddBigMapAvatarLoad(CSMapAvatarInfo avatar, MapAvaterType type)
    {
        if (AddBigMapAvatar(avatar, (int) type))
            AddBigMapAvatarQueue(avatar);
    }

    public void RemoveBigAvatar(long id)
    {
        RemoveBigAvatar(id, GetBigMapAvatar(id));
    }

    public void RemoveBigAvatar(long id, CSMapAvatarInfo avatar)
    {
        if (avatar == null) return;
        avatar.Dispose();
        int t = (int) avatar.AvatarType;
        /*if (MapAvatarInfoListDic.ContainsKey(t))
            MapAvatarInfoListDic[t].Remove(avatar);*/
        BigMapAvatarInfoDic.Remove(avatar.ID);
        if (!MapAvatarPoolListDic.ContainsKey(t))
            MapAvatarPoolListDic.Add(t, new Stack<CSMapAvatarInfo>(128));
        MapAvatarPoolListDic[t].Push(avatar);
    }

    public void RemoveBigMapAll()
    {
        if(BigMapAvatarInfoDic != null)
        {
            for (var i = 0; i < BigMapAvatarInfoDic.Count; i++)
            {
                BigMapAvatarInfoDic.GetValue(i).Destroy();
            }
            BigMapAvatarInfoDic.Clear();
        }

        ClearPoolObjByType(MapAvaterType.MapMonster);
        BigMapWaitLoadList?.Clear();
    }

    #endregion

    private int _direction;
    Quaternion vectorForward = new Quaternion();

    public Quaternion GetPlayerDirection()
    {
        if (_direction == CSAvatarManager.MainPlayer.GetDirection())
            return vectorForward;
        _direction = CSAvatarManager.MainPlayer.GetDirection();
        GetDirectionAngle(_direction);

        return vectorForward;
    }

    private void GetDirectionAngle(int dir)
    {
        switch (dir)
        {
            case CSDirection.Up:
                vectorForward.Set(0, 0, 0, 1); //0;
                break;
            case CSDirection.Right_Up:
                vectorForward.Set(0, 0, 0.4f, -0.9f); //315;
                break;
            case CSDirection.Right:
                vectorForward.Set(0, 0, 0.7f, -0.7f); //270;
                break;
            case CSDirection.Right_Down:
                vectorForward.Set(0, 0, 0.9f, -0.4f); //225;
                break;
            case CSDirection.Down:
                vectorForward.Set(0, 0, 1, 0); // 180;
                break;
            case CSDirection.Left_Down:
                vectorForward.Set(0, 0, 0.9f, 0.4f); //135;
                break;
            case CSDirection.Left:
                vectorForward.Set(0, 0, 0.7f, 0.7f); // 90;
                break;
            case CSDirection.Left_Up:
                vectorForward.Set(0, 0, 0.4f, 0.9f); // 45;
                break;
        }
    }

    #region 大地图动态点    队伍

    public void UpdateTeamViewPosition(map.SmallViewTeammateNtf teamData)
    {
        if (mActMapViewPoint == null) mActMapViewPoint = new Dictionary<long, SmallViewTeammateNtf>();
        //自己切换地图服务器不会移除周围所有人的信息，，现在加上这个判断，切地图时，服务器给我下发空数据，我清空
        if (teamData.x == 0 && teamData.y == 0 && teamData.roleId == CSMainPlayerInfo.Instance.ID)
        {
            mActMapViewPoint.Clear();
        }

        if (mActMapViewPoint.ContainsKey(teamData.roleId))
        {
            if (teamData.x == 0 && teamData.y == 0)
            {
                mActMapViewPoint.Remove(teamData.roleId);
            }
            else
            {
                mActMapViewPoint[teamData.roleId] = teamData;
            }
        }
        else
        {
            if (teamData.x != 0 || teamData.y != 0)
            {
                mActMapViewPoint.Add(teamData.roleId, teamData);
            }
        }

        mClientEvent.SendEvent(CEvent.UpdateMapSpecialPlayer);
    }

    #endregion

    public override void Dispose()
    {
        initViewData = null;
        RemoveAll();
        RemoveBigMapAll();
    }
}