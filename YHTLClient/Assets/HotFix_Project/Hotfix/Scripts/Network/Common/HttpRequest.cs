using System.Collections;
using MiniJSON;
using System.Collections.Generic;
using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class ServerListData
{
    /// <summary>
    /// 区服名
    /// </summary>
    public string S_Name;

    /// <summary>
    /// 区服ID
    /// </summary>
    public int S_ID;

    /// <summary>
    /// 域名ID
    /// </summary>
    public string S_DomainID;

    /// <summary>
    /// 唯一服ID
    /// </summary>
    public int S_OnlyID;

    /// <summary>
    /// 区服状态（1新服，2畅通，3爆满，4维护，5隐藏）
    /// </summary>
    public int S_State;

    /// <summary>
    /// 端口号-8000
    /// </summary>
    public int S_Port;

    /// <summary>
    /// 所在大区
    /// </summary>
    public int S_Region;

    /// <summary>
    /// 所属平台id
    /// </summary>
    public int S_Platform;

    /// <summary>
    /// 服务器类型， 0：普通服，!0：先行服，每一个类型就是一个先行服
    /// </summary>
    public string S_Type;

    /// <summary>
    /// 是否开放注册
    /// </summary>
    public bool S_OpenRegis;
}

public class HttpRequest : CSGameMgrBase<HttpRequest>
{
    protected EventHanlderManager mClientEvent;

    public List<object> mdic;

    /// <summary>
    /// 共享服服务器列表
    /// </summary>
    public ILBetterList<ServerListData> mSharedServiceList;

    /// <summary>
    /// 游戏服服务器列表
    /// </summary>
    public ILBetterList<ServerListData> mGameServiceList;

    /// <summary>
    /// 游戏服先行服服务器列表
    /// </summary>
    public ILBetterList<ServerListData> mAdvanceGameServiceList;


    public ILBetterList<int> mRecommendServiceList;

    public ILBetterList<int> mAdvanceRecommendServiceList;
    
    public ILBetterList<string> mIPlist;

    public Dictionary<string, object> mRoleNumDic;

    public int RecommendServerId
    {
        get
        {
            int id = 0;
            if (mdic != null && mdic.Count >= 1)
            {
                int.TryParse(mdic[0].ToString(), out id);
            }

            return id;
        }
    }

    public Dictionary<string, object> RegionDic2
    {
        get
        {
            if (mdic != null && mdic.Count >= 6) return mdic[5] as Dictionary<string, object>;
            return null;
        }
    }

    private string responseText = "";

    public override void Awake()
    {
        base.Awake();
        mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);
    }

    public void StartGetServerList()
    {
        StopAllCoroutines();
        StartCoroutine(StartRequest());
    }


    public IEnumerator StartRequest()
    {
        string mUrl = string.Empty;
        //在非编辑器上运行时候,会从CDN的配置文件中获取平台名称,任何获取对应的服务器列表,如果没有配置,直接不显示服务器列表,直观告诉测试,配置文件出错
        if (QuDaoConstant.GetPlatformData() != null)
        {
            mUrl = AppUrlMain.serverListUrl();
        }

        UnityWebRequest webRequest = UnityWebRequest.Get(mUrl);
        yield return webRequest.SendWebRequest();

        if (string.IsNullOrEmpty(webRequest.error) && webRequest.responseCode == 200)
        {
            responseText = Encoding.UTF8.GetString(webRequest.downloadHandler.data);
            string s = Encryption.ReplaceValue(responseText);
            object obj = Json.Deserialize(s);

            if (obj != null)
            {
                mdic = obj as List<object>;

                if (mdic != null)
                {
                    if (mdic.Count > 0) ParseRecommendServiceList(mdic[0] as List<object>);

                    if (mdic.Count >= 2) ParseGameServiceList(mdic[1] as List<object>);

                    if (mdic.Count >= 3) ParseSharedServiceList(mdic[2] as List<object>);

                    if (mdic.Count >= 4) ParseIPList(mdic[3] as List<object>);

                    if (mdic.Count >= 5) ParsePlantformList(mdic[4] as List<object>);

                    if (mdic.Count >= 6) ParseOtherParameterList(mdic[5] as Dictionary<string, object>); //解析后台开关参数

                    //if (mdic.Count >= 7) ParseTongServiceList(mdic[6] as Dictionary<string, object>); //解析通服列表  预留

                    //if (mdic.Count >= 8) ParseTongRecommendServiceList(mdic[7] as Dictionary<string, object>); //解析通服推荐服  预留

                    if (mdic.Count >= 9) ParseAdvanceGameServerceList(mdic[8] as List<object>); // 解析 先行服 参数
                    
                    if (mdic.Count >= 10) ParseAdvanceRecommendServiceList(mdic[9] as List<object>); // 解析 先行服 推荐服

                    
                }
                mClientEvent.SendEvent(CEvent.UI_Login);
            }
        }
        else
        {
            FNDebug.LogErrorFormat("HttpRequest StartRequest error  {0}  code: {1}    url:{2}", webRequest.error,
                webRequest.responseCode, webRequest.uri);
            ;
            BuglyAgent.ReportException("HttpRequest StartRequest", webRequest.error, "");
        }

        webRequest.Dispose();
    }

    /// <summary>
    /// 解析游戏服列表
    /// </summary>
    /// <param name="list"></param>
    private void ParseGameServiceList(List<object> list)
    {
        if (list == null) return;
        if (mGameServiceList == null) mGameServiceList = new ILBetterList<ServerListData>(list.Count);
        else mGameServiceList.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            List<object> l = list[i] as List<object>;

            if (l == null) continue;
            ServerListData data = new ServerListData();
            data.S_Name = l[0].ToString();
            data.S_ID = Convert.ToInt32(l[1]);
            data.S_DomainID = l[2].ToString();
            data.S_OnlyID = Convert.ToInt32(l[3]);
            data.S_State = Convert.ToInt32(l[4]);
            data.S_Port = Convert.ToInt32(l[5]);
            data.S_Type = "0"; //正常服务器列表中只存普通服
            data.S_OpenRegis = true; //普通服不关闭注册

            if (l.Count > 6)
            {
                data.S_Region = Convert.ToInt32(l[6]);
                data.S_Platform = Convert.ToInt32(l[7]);
            }

            mGameServiceList.Add(data);
        }
    }

    /// <summary>
    ///解析先行服
    /// </summary>
    /// <param name="list"></param>
    private void ParseAdvanceGameServerceList(List<object> list)
    {
        if (list == null) return;
        if (mAdvanceGameServiceList == null)
        {
            mAdvanceGameServiceList = new ILBetterList<ServerListData>(list.Count);
        }
        else
        {
            mAdvanceGameServiceList.Clear();
        }

        for (int i = 0; i < list.Count; i++)
        {
            List<object> l = list[i] as List<object>;

            if (l == null) continue;
            ServerListData data = new ServerListData();
            data.S_Name = l[0].ToString();
            data.S_ID = Convert.ToInt32(l[1]);
            data.S_DomainID = l[2].ToString();
            data.S_OnlyID = Convert.ToInt32(l[3]);
            data.S_State = Convert.ToInt32(l[4]);
            data.S_Port = Convert.ToInt32(l[5]);
            data.S_Region = Convert.ToInt32(l[6]);
            data.S_Platform = Convert.ToInt32(l[7]);
            data.S_Type = l[8].ToString();
            if (l.Count > 9) data.S_OpenRegis = l[9].ToString().Equals("1");

            mAdvanceGameServiceList.Add(data);
        }
    }

    /// <summary>
    ///解析推荐服
    /// </summary>
    /// <param name="list"></param>
    private void ParseRecommendServiceList(List<object> list)
    {
        if (list == null) return;
        if (mRecommendServiceList == null)
        {
            mRecommendServiceList = new ILBetterList<int>(list.Count);
        }
        else
        {
            mRecommendServiceList.Clear();
        }

        for (int i = 0; i < list.Count; i++)
        {
            int index = Convert.ToInt32(list[i]);
            if (!mRecommendServiceList.Contains(index))
                mRecommendServiceList.Add(index);
        }
    }
    
    /// <summary>
    ///解析先行服推荐服
    /// </summary>
    /// <param name="list"></param>
    private void ParseAdvanceRecommendServiceList(List<object> list)
    {
        if (list == null) return;
        if (mAdvanceRecommendServiceList == null)
        {
            mAdvanceRecommendServiceList = new ILBetterList<int>(list.Count);
        }
        else
        {
            mAdvanceRecommendServiceList.Clear();
        }

        for (int i = 0; i < list.Count; i++)
        {
            int index = Convert.ToInt32(list[i]);
            if (!mAdvanceRecommendServiceList.Contains(index))
                mAdvanceRecommendServiceList.Add(index);
        }
    }

    /// <summary>
    /// 解析IP列表
    /// </summary>
    /// <param name="list"></param>
    private void ParseIPList(List<object> list)
    {
        if (list == null) return;
        if (mIPlist == null)
        {
            mIPlist = new ILBetterList<string>(8);
        }
        else
        {
            mIPlist.Clear();
        }


        for (int i = 0; i < list.Count; i++)
        {
            string ip = list[i].ToString();
            if (!mIPlist.Contains(ip))
                mIPlist.Add(ip);
        }
    }

    //获取当前包 平台id
    private void ParsePlantformList(List<object> list)
    {
        if (list == null || list.Count <= 0) return;

        int.TryParse(list[0].ToString(), out CSConstant.plantformBigId);
    }

    private void ParseOtherParameterList(Dictionary<string, object> jsonDic)
    {
        if (jsonDic == null) return;

        if (jsonDic.ContainsKey("openFeedBack"))
        {
            int value = Convert.ToInt32(jsonDic["openFeedBack"]);
            QuDaoConstant.OpenFeedBack = value == 1;
        }

        if (jsonDic.ContainsKey("openVoice"))
        {
            int value = Convert.ToInt32(jsonDic["openVoice"]);
            QuDaoConstant.OpenVoice = value == 1;
        }

        if (jsonDic.ContainsKey("openRecharge"))
        {
            int value = Convert.ToInt32(jsonDic["openRecharge"]);
            QuDaoConstant.OpenRecharge = value == 1;
        }

        if (jsonDic.ContainsKey("openTranslate"))
        {
            int value = Convert.ToInt32(jsonDic["openTranslate"]);
            QuDaoConstant.OpenTranslate = value == 1;
        }

        if (jsonDic.ContainsKey("openPush"))
        {
            int value = Convert.ToInt32(jsonDic["openPush"]);
            QuDaoConstant.OpenPush = value == 1;
        }
        
        if (jsonDic.ContainsKey("channelCloseRegister"))
        {
            int value = Convert.ToInt32(jsonDic["channelCloseRegister"]);
            QuDaoConstant.ChannelCloseRegister = value == 1;
        }
    }

    /// <summary>key
    /// 解析共享服列表
    /// </summary>
    /// <param name="list"></param>
    private void ParseSharedServiceList(List<object> list)
    {
        if (list == null) return;
        if (mSharedServiceList == null)
        {
            mSharedServiceList = new ILBetterList<ServerListData>(list.Count);
        }
        else
        {
            mSharedServiceList.Clear();
        }

        for (int i = 0; i < list.Count; i++)
        {
            List<object> l = list[i] as List<object>;

            if (l == null) continue;
            ServerListData data = new ServerListData();
            data.S_OnlyID = Convert.ToInt32(l[0]);
            data.S_Port = Convert.ToInt32(l[1]);
            data.S_DomainID = l[2].ToString();
            mSharedServiceList.Add(data);
        }
    }

    public string GetAreaName(int serverId)
    {
        string result = "";

        ServerListData S_data = GetGameServerFormOnlyId(serverId);

        if (S_data == null) return result;

        result = "S" + Regex.Replace(S_data.S_Name, @"[^0-9]+", "");

        return result;
    }

    public ILBetterList<ServerListData> GetRoleServerList(ILBetterList<ServerListData> l)
    {
        if (mRoleNumDic != null && mRoleNumDic.Count > 0)
        {
            if(l == null) l = new ILBetterList<ServerListData>();
            else l.Clear();
            var v = mRoleNumDic.GetEnumerator();
            while (v.MoveNext())
            {
                string str = v.Current.Key;

                if (string.IsNullOrEmpty(str)) continue;

                int index = 0;

                Int32.TryParse(str, out index);

                ServerListData data = CurGameService(index);
                if(data != null)
                {
                    l.Add(data);
                }
            }
            v.Dispose();

            return l;
        }

        return null;
    }

    /// <summary>
    /// 得到游戏服
    /// </summary>
    /// <param name="serverId"></param>
    /// <returns></returns>
    public ServerListData CurGameService(int serverId)
    {
        if (mGameServiceList == null) return null;

        for (int i = 0; i < mGameServiceList.Count; i++)
        {
            if (serverId == mGameServiceList[i].S_ID)
            {
                return mGameServiceList[i];
            }
        }

        if (mAdvanceGameServiceList == null) return null;
        for (var i = 0; i < mAdvanceGameServiceList.Count; i++)
        {
            if (serverId == mAdvanceGameServiceList[i].S_ID)
            {
                return mAdvanceGameServiceList[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 通过区服唯一ID获得区服数据
    /// </summary>
    /// <returns></returns>
    public ServerListData GetGameServerFormOnlyId(int onlyServerId)
    {
        if (mGameServiceList == null) return null;

        for (int i = 0; i < mGameServiceList.Count; i++)
        {
            if (onlyServerId == mGameServiceList[i].S_OnlyID)
            {
                return mGameServiceList[i];
            }
        }
        if (mAdvanceGameServiceList == null) return null;

        for (int i = 0; i < mAdvanceGameServiceList.Count; i++)
        {
            if (onlyServerId == mAdvanceGameServiceList[i].S_OnlyID)
            {
                return mAdvanceGameServiceList[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 得到共享服列表
    /// </summary>
    /// <param name="serverId"></param>
    /// <returns></returns>
    public ServerListData CurSharedService(int serverId)
    {
        if (mSharedServiceList == null) return null;

        for (int i = 0; i < mSharedServiceList.Count; i++)
        {
            if (serverId == mSharedServiceList[i].S_OnlyID)
            {
                return mSharedServiceList[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 得到推荐服列表
    /// </summary>
    /// <returns></returns>
    public ILBetterList<ServerListData> GetRecommendList(ILBetterList<ServerListData> l)
    {
        if(l == null) l = new ILBetterList<ServerListData>();
        else l.Clear();
        for (int i = 0; i < mRecommendServiceList.Count; i++)
        {
            int id = mRecommendServiceList[i];

            ServerListData s_data = CurGameService(id);
            if (s_data != null)
            {
                l.Add(s_data);
            }
        }

        return l;
    }
    
    /// <summary>
    /// 得到先行推荐服列表
    /// </summary>
    /// <returns></returns>
    public ILBetterList<ServerListData> GetAdvanceRecommendList(ILBetterList<ServerListData> l)
    {
        if(l == null) l = new ILBetterList<ServerListData>();
        else l.Clear();
        if (mAdvanceRecommendServiceList == null || mAdvanceGameServiceList == null) return l;
        for (int i = 0; i < mAdvanceRecommendServiceList.Count; i++)
        {
            int id = mAdvanceRecommendServiceList[i];

            for (var j = 0; j < mAdvanceGameServiceList.Count; j++)
            {
                if (id == mAdvanceGameServiceList[j].S_ID)
                {
                    l.Add(mAdvanceGameServiceList[j]);
                }
            }
        }

        return l;
    }

    /// <summary>
    /// 得到我的服务器列表
    /// </summary>
    /// <returns></returns>
    public ILBetterList<ServerListData> GetMySevrverList()
    {
        ILBetterList<ServerListData> l = new ILBetterList<ServerListData>();
        if(mRoleNumDic != null)
        {
            for (int i = 0; i < mRoleNumDic.Count; i++)
            {
                int id = mRecommendServiceList[i];

                ServerListData s_data = CurGameService(id);
                if (s_data != null)
                {
                    l.Add(s_data);
                }
            }
        }

        return l;
    }

    /// <summary>
    /// 得到一个推荐服
    /// </summary>
    public int GetRecommendServer
    {
        get
        {
            if (mRecommendServiceList.Count > 0) return mRecommendServiceList[0];
            if (mAdvanceRecommendServiceList.Count > 0) return mAdvanceRecommendServiceList[0];
            return 0;
        }
    }

    public bool IsAdminUser(string ip)
    {
        if (string.IsNullOrEmpty(ip) || mIPlist == null) return false;

        for (int i = 0; i < mIPlist.Count; i++)
        {
            string str_ip = mIPlist[i].ToString();
            if (str_ip == ip) return true;
        }

        return false;
    }
    
    //请求角色数量
    public void RequestRoleNum()
    {
        StartCoroutine(RequestRoleNumIEnume());
    }
    
    private IEnumerator RequestRoleNumIEnume()
    {
        WWWForm from = CSSubmitDataManager.GetQueryloginnameForm();
        UnityWebRequest  www = UnityWebRequest.Post(AppUrlMain.centerUrl, from);
        yield return www.SendWebRequest();
        if (string.IsNullOrEmpty(www.error) && www.responseCode == 200)
        {
            HttpRequest.Instance.mRoleNumDic =
                Json.Deserialize(www.downloadHandler.text) as Dictionary<string, object>;
            
        }else
        {
            FNDebug.LogErrorFormat("HttpRequest StartRequest error  {0}  code: {1}    url:{2}", www.error,
                www.responseCode, www.uri);
        }
    
        www.Dispose();
        www = null;
        if(mClientEvent != null)
        {
            mClientEvent.SendEvent(CEvent.RequestRequestRoleNum);
        }
    }


    public override void OnDestroy()
    {
        mClientEvent?.UnRegAll();
        mClientEvent = null;
        mdic = null;
        mSharedServiceList = null;
        mGameServiceList = null;
        mAdvanceGameServiceList = null;
        mRecommendServiceList = null;
        mIPlist = null;
        mRoleNumDic = null;
        base.Destroy();
    }
}