using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSelectTargetManager : Singleton<CSSelectTargetManager>
{
    private int mLastPkMode = EPkMode.Peace;
    private CSMisc.Dot2 mLastDetectCoord = new CSMisc.Dot2();
    private SelectData mLastSelected = new SelectData();
    private ILBetterList<SelectData> mRoundAvatars = new ILBetterList<SelectData>();

    private MainEventHanlderManager mMainClientEvent;

    public void Init()
    {
        if (mMainClientEvent == null)
        {
            mMainClientEvent = new MainEventHanlderManager(MainEventHanlderManager.MainDispatchType.Event);
        }
        mMainClientEvent.AddEvent(MainEvent.CloseSelectionPanel, OnClearRoundAvaters);
    }


    public void SwitchSelectPlayer(int range)
    {
        int pkMode = (CSMainPlayerInfo.Instance.PkModeMap > 0) ? CSMainPlayerInfo.Instance.PkModeMap : CSMainPlayerInfo.Instance.PkMode;
        if (pkMode == EPkMode.Peace)
        {
            UtilityTips.ShowTips(751);
            return;
        }
        SwithcSelect(EAvatarType.Player, pkMode, range);
    }

    public void SwitchSelectMonster(int range)
    {
        int pkMode = (CSMainPlayerInfo.Instance.PkModeMap > 0) ? CSMainPlayerInfo.Instance.PkModeMap : CSMainPlayerInfo.Instance.PkMode;
        SwithcSelect(EAvatarType.Monster, pkMode, range);
    }

    public void SelectAvatarByID(long id)
    {
        CSAvatar avatar = CSAvatarManager.Instance.GetAvatar(id);
        if(avatar != null)
        {
            SelectAvatar(avatar);
        }
    }

    public void SelectAvatar(CSAvatar avatar)
    {
        if (avatar != null)
        {
            CSCharacter MainPlayer = CSAvatarManager.MainPlayer;
            MainPlayer.ResetTouchData(CSMisc.Dot2.Zero);
            MainPlayer.TouchEvent.SetTarget(avatar);
        }
    }

    private void SwithcSelect(int type, int pkMode, int range)
    {
        SelectData selectData = GetSelectData(pkMode, type);
        mLastPkMode = pkMode;
        if (selectData == null)
        {
            DetectRoundAvatars(type, range);
            if (mRoundAvatars.Count > 0)
            {
                selectData = mRoundAvatars[0];
            }
        }
        SelectAvatar(selectData);
    }

    private void DetectRoundAvatars(int type, int range)
    {
        mLastDetectCoord =CSAvatarManager.MainPlayer.OldCell.Coord;
        mRoundAvatars.Clear();
        float distance = CSCell.width * 2 * range;
        distance *= distance;

        List<CSAvatar> avatars = CSAvatarManager.Instance.GetAvatarList(type);
        if (avatars == null)
        {
            return;
        }
        Vector2 localPosition =CSAvatarManager.MainPlayer.OldCell.LocalPosition2;

        bool isPlayerUndetect = false;
        for (int i = 0; i < avatars.Count; ++i)
        {
            CSAvatar avatar = avatars[i];
            if (avatar == null || !avatar.IsLoad)
            {
                continue;
            }
            if (type == EAvatarType.Player && IsPlayerUndetect(avatar))
            {
                isPlayerUndetect = true;
                continue;
            }

            if (type == EAvatarType.Monster && IsMonsterUndetect(avatar))
            {
                continue;
            }
            float offsetX = avatar.NewCell.LocalPosition2.x - localPosition.x;
            float offsetY = (avatar.NewCell.LocalPosition2.y - localPosition.y) * 1.5f;
            float dis = offsetX * offsetX + offsetY * offsetY;
            if (dis < distance)
            {
                SelectData data = new SelectData(avatar, dis, type);
                int index = GetInsertIndex(data);
                mRoundAvatars.Insert(index, data);
            }
        }
        ResetIndex(type);

        if (type == EAvatarType.Player && isPlayerUndetect && mRoundAvatars.Count <= 0)
        {
            UtilityTips.ShowTips(1788);
        }
        //SortRoundAvatars(type);
    }

    private void SelectAvatar(SelectData selectData)
    {
        if (selectData == null)
        {
            return;
        }
        CSCharacter MainPlayer =CSAvatarManager.MainPlayer;
        if (selectData.Avatar != null)
        {
            MainPlayer.ResetTouchData(CSMisc.Dot2.Zero);
            MainPlayer.TouchEvent.SetTarget(selectData.Avatar);
        }
        else
        {
            MainPlayer.TouchEvent.CancelSelect();
            ClearRoundAvaters();
            HotManager.Instance.MainEventHandler.SendEvent(MainEvent.CloseSelectionPanel);
        }
        mLastSelected = selectData;
    }

    private SelectData GetSelectData(int pkMode, int type)
    {
        SelectData selectData = null;
        if (pkMode == mLastPkMode && mLastSelected.Type == type &&
            mLastDetectCoord.Equal(CSAvatarManager.MainPlayer.OldCell.Coord))
        {
            if (mRoundAvatars.Contains(mLastSelected))
            {
                for (int i = mLastSelected.Index + 1; i < mRoundAvatars.Count; i++)
                {
                    selectData = mRoundAvatars[i];
                    if (selectData.Avatar == null || !selectData.Avatar.IsLoad)
                    {
                        selectData = null;
                        continue;
                    }
                    break;
                }
            }
        }
        return selectData;
    }

    private void SortRoundAvatars(int type)
    {
        if (mRoundAvatars.Count > 0)
        {
            if (type == EAvatarType.Monster)
            {
                //TODO:ddn need sort 
                //mRoundAvatars.Sort(SelectData.CompareMonster);
                ResetIndex(type);
            }
            else if (type == EAvatarType.Player)
            {
                //TODO:ddn need sort 
                //mRoundAvatars.Sort(SelectData.ComparePlayer);
                ResetIndex(type);
            }
        }
    }

    private int GetInsertIndex(SelectData selectData)
    {
        int index = mRoundAvatars.Count;
        for (int i = 0; i < index; ++i)
        {
            SelectData tempData = mRoundAvatars[i];
            if (selectData.Distance < mRoundAvatars[i].Distance)
            {
                return i;
            }
        }
        return index;
    }

    private void ResetIndex(int type)
    {
        if (type == EAvatarType.Monster || type == EAvatarType.Player)
        {
            for (int i = 0; i < mRoundAvatars.Count; i++)
            {
                SelectData p = mRoundAvatars[i];
                p.Index = mRoundAvatars.IndexOf(p);
            }
        }
    }

    public bool IsPlayerUndetect(CSAvatar avatar)
    {
        if (avatar.BaseInfo == null)
        {
            return false;
        }
        if (avatar.BaseInfo != null && avatar.BaseInfo.Level < CSConfigInfo.Instance.AttackPlayerLevel())
        {
            return true;
        }
        return false;
    }

    public bool IsMonsterUndetect(CSAvatar avatar)
    {
        return false;
    }

    private void ClearRoundAvaters()
    {
        if (mRoundAvatars != null)
        {
            mRoundAvatars.Clear();
        }
    }

    public void OnClearRoundAvaters(uint evtId, object obj)
    {
        if(obj == null)
        {
            CSAvatarManager.MainPlayer.TouchEvent.CancelSelect();
        }
        ClearRoundAvaters();
    }

    public void Destroy()
    {
        mLastPkMode = EPkMode.Peace;
        mLastDetectCoord.Clear();
        ClearRoundAvaters();
        if(mMainClientEvent != null)
        {
            mMainClientEvent.UnRegAll();
            mMainClientEvent = null;
        }
    }
}





public class SelectData : IComparable<SelectData>
{
    public CSAvatar Avatar { get; set; }
    public long ID { get; set; }
    public int Index { get; set; }
    public float Distance { get; set; }
    public int Type = EAvatarType.None;

    public SelectData() { }

    public SelectData(CSAvatar avatar, float distance, int type)
    {
        if (avatar != null)
        {
            Avatar = avatar;
            ID = avatar.ID;
        }
        Distance = distance;
        Type = type;
    }

    public int CompareTo(SelectData other)
    {
        return Distance.CompareTo(other.Distance);
    }

    public static int CompareMonster(SelectData x, SelectData y)
    {
        //if (!MonsterInfoClientTableManager.Instance.dic.ContainsKey((uint)x.Avater.BaseInfo.TemplateID) ||
        //    !MonsterInfoClientTableManager.Instance.dic.ContainsKey((uint)y.Avater.BaseInfo.TemplateID))
        //{
        //    return 0;
        //}
        //uint xType = MonsterInfoClientTableManager.Instance.dic[(uint)x.Avater.BaseInfo.TemplateID].type;
        //uint yType = MonsterInfoClientTableManager.Instance.dic[(uint)y.Avater.BaseInfo.TemplateID].type;
        uint xType = 0;
        uint yType = 0;
        if (xType == yType)
        {
            return x.Distance.CompareTo(y.Distance);
        }
        else
        {
            if (xType == 39) return -1;
            if (yType == 39) return 1;

            if (xType == 19 || yType == 19 || xType == 20 || yType == 20)
                return x.Distance.CompareTo(y.Distance);
            return yType.CompareTo(xType);
        }
    }

    public static int ComparePlayer(SelectData x, SelectData y)
    {
        return x.Distance.CompareTo(y.Distance);
    }
}

