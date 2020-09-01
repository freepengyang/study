using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine.Networking;

public enum LoginOpenType
{
    None,
    UIRoleList,
    UICreateRole
}
public class UILoginRolePanel : UIBase
{
    public override bool ShowGaussianBlur { get { return false; } }
    #region  forms
    private GameObject _btn_BackLogin;
    private GameObject btn_BackLogin { get { return _btn_BackLogin ?? (_btn_BackLogin = Get("top_left/event/btn_back").gameObject); } }
    private GameObject _btn_Start;
    private GameObject btn_Start { get { return _btn_Start ?? (_btn_Start = Get("bottom_right/event/btn_enter").gameObject); } }
    //角色模型
    private GameObject _tex_roleModel;
    private GameObject tex_roleModel { get { return _tex_roleModel ?? (_tex_roleModel = Get<GameObject>("center/view/roleSprite")); } }
    private GameObject _tex_Career;
    private GameObject tex_Career { get { return _tex_Career ?? (_tex_Career = Get<GameObject>("right/window/tx_career")); } }
    //创角
    private Transform _obj_CreateRole;
    private Transform obj_CreateRole { get { return _obj_CreateRole ?? (_obj_CreateRole = Get("createRoleView")); } }
    private GameObject _obj_CareerGrid;
    private GameObject obj_CareerGrid { get { return _obj_CareerGrid ?? (_obj_CareerGrid = Get("left/career", obj_CreateRole).gameObject); } }
    private UIInput rolename;
    private UIInput RoleName { get { return rolename ?? (rolename = Get<UIInput>("bottom/rolename", obj_CreateRole)); } }
    private GameObject _btn_random;
    private GameObject btn_random { get { return _btn_random ?? (_btn_random = Get("bottom/btn_random", obj_CreateRole).gameObject); } }
    private GameObject _btn_man;
    private GameObject btn_man { get { return _btn_man ?? (_btn_man = Get("right/sex/man", obj_CreateRole).gameObject); } }
    private GameObject _btn_manHl;
    private GameObject btn_manHl { get { return _btn_manHl ?? (_btn_manHl = Get("Checkmark", btn_man.transform).gameObject); } }
    private GameObject _btn_woman;
    private GameObject btn_woman { get { return _btn_woman ?? (_btn_woman = Get("right/sex/woman", obj_CreateRole).gameObject); } }
    private GameObject _btn_womanHl;
    private GameObject btn_womanHl { get { return _btn_womanHl ?? (_btn_womanHl = Get("Checkmark", btn_woman.transform).gameObject); } }

    private GameObject _btn_job1;
    private GameObject btn_job1 { get { return _btn_job1 ?? (_btn_job1 = Get("left/career/1", obj_CreateRole).gameObject); } }
    private GameObject _btn_job1Hl;
    private GameObject btn_job1Hl { get { return _btn_job1Hl ?? (_btn_job1Hl = Get("Checkmark", btn_job1.transform).gameObject); } }
    private GameObject _btn_job2;
    private GameObject btn_job2 { get { return _btn_job2 ?? (_btn_job2 = Get("left/career/2", obj_CreateRole).gameObject); } }
    private GameObject _btn_job2Hl;
    private GameObject btn_job2Hl { get { return _btn_job2Hl ?? (_btn_job2Hl = Get("Checkmark", btn_job2.transform).gameObject); } }
    private GameObject _btn_job3;
    private GameObject btn_job3 { get { return _btn_job3 ?? (_btn_job3 = Get("left/career/3", obj_CreateRole).gameObject); } }
    private GameObject _btn_job3Hl;
    private GameObject btn_job3Hl { get { return _btn_job3Hl ?? (_btn_job3Hl = Get("Checkmark", btn_job3.transform).gameObject); } }

    //选角
    private Transform _obj_ChooseRole;
    private Transform obj_ChooseRole { get { return _obj_ChooseRole ?? (_obj_ChooseRole = Get("roleListView")); } }
    private GameObject _btn_Delect;
    private GameObject btn_Delect { get { return _btn_Delect ?? (_btn_Delect = Get("bottom_left/btn_del", obj_ChooseRole).gameObject); } }
    private GameObject _obj_RoleListgrid;
    private GameObject obj_RoleListgrid { get { return _obj_RoleListgrid ?? (_obj_RoleListgrid = Get("left/view/grid", obj_ChooseRole).gameObject); } }
    //waiting
    private GameObject waiting;
    private GameObject mWaiting { get { return waiting ?? (waiting = Get<GameObject>("waiting")); } }
    #endregion
    public void InitComponents()
    {
        RoleInfoCtrlLIst.Add(new RoleInfoCtrl(Get("roleListView/left/view/grid/1").gameObject));
        RoleInfoCtrlLIst.Add(new RoleInfoCtrl(Get("roleListView/left/view/grid/2").gameObject));
    }

    protected EventHanlderManager mSocketEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Socket);

    List<RoleInfoCtrl> RoleInfoCtrlLIst;
    //string careerTexPath = string.Empty;
    LoginOpenType mCurType = LoginOpenType.None;
    int time = 150;
    Schedule schedule;
    //TABLE.SUNDRY tbl_sundry = null;
    user.RoleBrief mCurRoleinfo = null;
    public user.RoleBrief CurRoleinfo
    {
        get { return mCurRoleinfo; }
        set
        {
            mCurRoleinfo = value;
        }
    }
    RoleInfoCtrl mCurInfoCtrl = null;
    public int mSex = 1;   //性别
    public int mCareer = 1;   //职业
    #region 
    GameObject curSex;
    GameObject curJob;
    #endregion
    List<user.RoleBrief> Rolelist = new List<user.RoleBrief>();
    private int ConnectNum = 0;
    public void Awake()
    {
        mCareer = 1;   //职业
        mSex = 1;   //性别
        Rolelist = new List<user.RoleBrief>();
        RoleInfoCtrlLIst = new List<RoleInfoCtrl>();
    }

    public override void Init()
    {
        base.Init();
        Awake();
        InitComponents();
        UIEventListener.Get(btn_random).onClick = p => { Net.ReqRandomRoleName(mSex); };
        UIEventListener.Get(btn_BackLogin).onClick = OnBackClick;
        UIEventListener.Get(btn_Start).onClick = OnButtonClick;
        UIEventListener.Get(btn_Delect).onClick = OnRemoveRoleClick;
        mClientEvent.AddEvent(CEvent.ResPlayerInfoMessage, OnReceiveChooseRoleMessage);
        mClientEvent.AddEvent(CEvent.ResDeleteRoleMessage, OnReceiveRemoveRoleMessage);
        mClientEvent.AddEvent(CEvent.ResRandomRoleNameMessage, OnReceiveRandomRoleNameMessage);
        mClientEvent.AddEvent(CEvent.CreateRoleNtfMessage, OnCreateRoleMessage);
        mClientEvent.AddEvent(CEvent.Connect, OnConnectSucceed);
        mClientEvent.AddEvent(CEvent.Disconnect, OnConnectFailure);
        mClientEvent.AddEvent(CEvent.ConnectFail, OnConnectFailure);
        mClientEvent.AddEvent(CEvent.ResLoginMessage, OnReconnectReceivedLoginData);

    }

    public override void Show()
    {
        base.Show();

        mCurType = UserManager.Instance.RoleCount <= 0 ? LoginOpenType.UICreateRole : LoginOpenType.UIRoleList;
        Show(mCurType);

        CSSubmitDataManager.Instance.SendSubmitData(SubmitDataType.SUBMIT_9);
    }

    public void Show(LoginOpenType type)
    {
        mCurType = type;

        if (type == LoginOpenType.UICreateRole)
        {
            mCareer = 1;
            mSex = 1;
            InitCareerGrid();
            InitSex();
            if (obj_ChooseRole.gameObject.activeSelf) { obj_ChooseRole.gameObject.SetActive(false); }
            if (!obj_CreateRole.gameObject.activeSelf) { obj_CreateRole.gameObject.SetActive(true); }
            if (schedule != null && Timer.Instance.IsInvoking(schedule)) { Timer.Instance.CancelInvoke(schedule); schedule = null; }
            obj_CareerGrid.transform.GetComponent<TweenAlpha>().PlayTween();
            obj_CareerGrid.transform.GetComponent<TweenPosition>().PlayTween();
        }
        else if (type == LoginOpenType.UIRoleList)
        {
            if (!obj_ChooseRole.gameObject.activeSelf) { obj_ChooseRole.gameObject.SetActive(true); }
            if (obj_CreateRole.gameObject.activeSelf) { obj_CreateRole.gameObject.SetActive(false); }
            ShowRoleList();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        CSEffectPlayMgr.Instance.Recycle(tex_Career);
        CSEffectPlayMgr.Instance.Recycle(tex_roleModel);
        mSocketEvent?.UnRegAll();
        mSocketEvent = null;
    }
    private void InitCareerGrid()
    {
        UIEventListener.Get(btn_job1, 1).onClick = OnJobClick;
        UIEventListener.Get(btn_job2, 2).onClick = OnJobClick;
        UIEventListener.Get(btn_job3, 3).onClick = OnJobClick;
        OnJobClick(btn_job1);
    }
    private void OnJobClick(GameObject go)
    {
        mCareer = (int)UIEventListener.Get(go).parameter;
        if (curJob != null) { curJob.SetActive(false); }
        switch (mCareer)
        {
            case 1:
                btn_job1Hl.SetActive(true);
                curJob = btn_job1Hl;
                break;
            case 2:
                btn_job2Hl.SetActive(true);
                curJob = btn_job2Hl;
                break;
            case 3:
                btn_job3Hl.SetActive(true);
                curJob = btn_job3Hl;
                break;
        }
        ChangeRoleModel();
        ChangeCareerTexture();
    }
    private void InitSex()
    {
        UIEventListener.Get(btn_man, 1).onClick = SexBtnClick;
        UIEventListener.Get(btn_woman, 0).onClick = SexBtnClick;
        SexBtnClick(btn_man);
    }
    private void SexBtnClick(GameObject _go)
    {
        mSex = (int)UIEventListener.Get(_go).parameter;
        if (curSex != null) { curSex.SetActive(false); }
        switch (mSex)
        {
            case 1:
                btn_manHl.SetActive(true);
                curSex = btn_manHl;
                break;
            case 0:
                btn_womanHl.SetActive(true);
                curSex = btn_womanHl;
                break;
        }
        Net.ReqRandomRoleName(mSex);
        ChangeRoleModel();
    }
    private void ChangeCareerTexture()
    {
        CSStringBuilder.Clear();
        string str = CSStringBuilder.Append("dlxjjs", mCareer.ToString(), "_cq_1").ToString();
        CSEffectPlayMgr.Instance.ShowUITexture(tex_Career, str);
    }
    private void ChangeRoleModel()
    {
        CSStringBuilder.Clear();
        string str = CSStringBuilder.Append("modelcj", mCareer.ToString(), mSex.ToString()).ToString();
        CSEffectPlayMgr.Instance.ShowUIEffect2(tex_roleModel, str, ResourceAssistType.ForceLoad);
    }
    private void OnBackClick(GameObject gp)
    {
        if (mCurType == LoginOpenType.UICreateRole && UserManager.Instance.RoleCount > 0)
        {
            Show(LoginOpenType.UIRoleList);
        }
        else
        {
            switch (Platform.mPlatformType)
            {
                case PlatformType.EDITOR:
                    BackToLogin();
                    break;
                case PlatformType.ANDROID:
                    if (QuDaoConstant.GetPlatformData() != null && QuDaoConstant.GetPlatformData().exitAccount)
                    {
                        QuDaoInterface.Instance.Logout();
                    }
                    else
                    {
                        BackToLogin();
                    }
                    break;
                case PlatformType.IOS:
                    QuDaoInterface.Instance.Logout();
                    BackToLogin();
                    break;
            }
        }
    }

    public void BackToLogin()
    {
        UIManager.Instance.ClosePanel<UILoginRolePanel>();
        UIManager.Instance.CreatePanel<UILogin>(null);
        CSNetwork.Instance.CloseThread();
        CSNetwork.Instance.Close();
    }

    private void OnButtonClick(GameObject gp)
    {
        CSSubmitDataManager.Instance.SendSubmitData(SubmitDataType.SUBMIT_10);
        if (mCurType == LoginOpenType.UICreateRole)
        {
            if (string.IsNullOrEmpty(RoleName.value))
            {
                UtilityTips.ShowTips(1306);
                return;
            }
            if (!CSNetwork.Instance.IsConnect)
            {
                UtilityTips.ShowTips(1308);
                return;
            }
            Net.ReqCreateRoleMessage(RoleName.value, mSex, mCareer, QuDaoInterface.Instance.getSystemModel());
        }
        else if (mCurType == LoginOpenType.UIRoleList)
        {
            EnterGame();
        }
    }


    private bool IsEnteringGame = false;
    //private string ipFromClient;
    //private int portFromClient = 0;
    //private int curServerId = 0;
    private bool isKuaFu = false;
    private void EnterGame()
    {
        if (!CSNetwork.Instance.IsConnect)
        {
            FNDebug.Log("CSNetwork.Instance.IsConnect == false");
            return;
        }
        if (mCurType == LoginOpenType.UIRoleList)
        {
            Constant.mCurServer = ServerType.GameServer;
            Net.ReqChooseRole(CurRoleinfo.roleId);
            Constant.mRoleId = CurRoleinfo.roleId;
            isKuaFu = false;
            if (!IsEnteringGame)
            {
                EnterGameTips(CSString.Format(418));
                //CoroutineManager.StopCoroutine("EnterFailTips");
                //CoroutineManager.DoCoroutine("EnterFailTips", CSString.Format(102332));
                IsEnteringGame = true;
            }
        }
        else
        {
            if (!CSNetwork.Instance.IsConnect)
            {
                FNDebug.Log("CSNetwork.Instance.IsConnect == false");
                return;
            }
            Constant.mCurServer = ServerType.GameServer;
            Net.ReqCreateRoleMessage(RoleName.value, mSex, mCareer, QuDaoInterface.Instance.getSystemModel());
        }
    }

    IEnumerator EnterFailTips(string tips)
    {
        yield return new WaitForSeconds(5f);
        FNDebug.Log("连接失败");

        IsEnteringGame = false;

        if (mCurRoleinfo != null)
        {
            string s = CSString.Format(isKuaFu, 400, 401);
            //  UnityEngine.Debug.LogError(mCurRoleinfo.roleName + " " + tips + " 登入不了服务器 " + s + " Server= " + " ipFromClient = " + ipFromClient + " portFromClient=" + portFromClient + " curServerId=" + curServerId
            // + " Constant.mCurServer = " + Constant.mCurServer);
        }

        ReconnectGameServer();
        yield return new WaitForSeconds(5f);
        OnBackClick(null);
    }

    void ReconnectGameServer()
    {
        if (isKuaFu && mCurRoleinfo != null)
        {
            if (CSNetwork.Instance != null)
            {
                if (CSNetwork.Instance.Host != ServerViewCtrl.host || CSNetwork.Instance.port != ServerViewCtrl.gamePort)
                {
                    if (Constant.mCurServer == ServerType.SharedService || Constant.mCurServer == ServerType.SldgService)
                    {
                        // UnityEngine.Debug.LogError(mCurRoleinfo.roleName + " 登入不了服务器重连进服务器  Server= " + " ipFromClient = " + ipFromClient + " portFromClient=" + portFromClient + " curServerId=" + curServerId + " Constant.mCurServer = " + Constant.mCurServer);
                        UIManager.Instance.ClosePanel<UIWaiting>();
                        mSocketEvent.UnReg((uint)ECM.Connect, OnReconnectGameServer);
                        mSocketEvent.Reg((uint)ECM.Connect, OnReconnectGameServer);
                        CSNetwork.Instance.Connect(ServerViewCtrl.host, ServerViewCtrl.gamePort);
                    }
                }
            }
        }
    }

    public void OnReconnectGameServer(uint uiEvtID, object data)
    {
        if (CSNetwork.Instance != null)
        {
            string ip = string.Empty;
            int port = 0;
            CSNetwork.Instance.GetCurClientIpAndPort(ref ip, ref port);
            if (!string.IsNullOrEmpty(ip) && port != 0)
            {
                if (ip != ServerViewCtrl.host || port != ServerViewCtrl.gamePort)
                {
                    //UnityEngine.Debug.LogError("不是本服ip地址的回包");
                    // UnityEngine.Debug.LogError(mCurRoleinfo.roleName + " 登入不了服务器重连进服务器OnReconnectGameServer  不是本服ip地址的回包 Server= " + " ipFromClient = " + ipFromClient + " portFromClient=" + portFromClient + " curServerId=" + curServerId
                    //   + " Constant.mCurServer = " + Constant.mCurServer);
                    return;
                }
            }
        }
        if (mCurRoleinfo != null)
        {
            string str = PlayerPrefs.GetString("userName");
            if (string.IsNullOrEmpty(str))
            {
                UtilityTips.ShowTips(100076);
                // UnityEngine.Debug.LogError(mCurRoleinfo.roleName + " 登入不了服务器重连进服务器OnReconnectGameServer 账号异常  Server= " + " ipFromClient = " + ipFromClient + " portFromClient=" + portFromClient + " curServerId=" + curServerId
                //       + " Constant.mCurServer = " + Constant.mCurServer);
                return;
            }

            if (Platform.IsEditor)
            {
                str = str.Replace(Environment.NewLine, "");
            }
            else
            {
                if (string.IsNullOrEmpty(str) && QuDaoConstant.GetPlatformData() != null)
                    str = CSConstant.loginName;
            }

            UIManager.Instance.ClosePanel<UIWaiting>();
            //mSocketEvent.UnReg((uint)ECM.ResLoginMessage);
            //mSocketEvent.Reg((uint)ECM.ResLoginMessage, OnReconnectReceivedLoginData);
            Net.ReqLoginMessage(str, Constant.platformid, CSConstant.mOnlyServerId, Constant.sign, Constant.time, QuDaoInterface.Instance.getSystemModel());


            //UIManager.Instance.ClosePanel<UIWaiting>();
            //Net.ReqBackServerMessage(15, mCurRoleinfo.roleId);
        }
    }

    private void OnReconnectReceivedLoginData(uint id, object data)
    {
        if (data == null)
        {
            // UnityEngine.Debug.LogError(mCurRoleinfo.roleName + " 登入不了服务器 数据为空 OnReconnectReceivedLoginData Server= " + " ipFromClient = " + ipFromClient + " portFromClient=" + portFromClient + " curServerId=" + curServerId
            //           + " Constant.mCurServer = " + Constant.mCurServer);
            return;
        }

        UIManager.Instance.ClosePanel<UIWaiting>();
        Net.ReqBackServerMessage(15, mCurRoleinfo.roleId);
    }

    void EnterGameTips(string tips)
    {
        UtilityTips.ShowTips(tips, 1.5f, ColorType.Green);
    }

    private void TimeLimit(Schedule sch)
    {
        time--;
        if (time <= 0)
        {
            Timer.Instance.CancelInvoke(schedule);
            schedule = null;
            OnButtonClick(null);
        }
    }

    public void ShowRoleList()
    {
        CreateRoleTemp(RefreshRoleList(UserManager.Instance.RoleBriefList));
    }

    public void OnCreateRoleClick(GameObject obj)
    {
        Show(LoginOpenType.UICreateRole);
    }

    public void OnRemoveRoleClick(GameObject obj)
    {
        if (obj == null) return;
        if (CurRoleinfo.roleId == 0)
        {
            UtilityTips.ShowTips(100154); return;
        }
        else
        {
            UtilityTips.ShowPromptWordTips(2, DeleteRole);
        }
    }

    private void DeleteRole()
    {
        Net.ReqRemoveRole(CurRoleinfo.roleId);
    }

    private List<user.RoleBrief> RefreshRoleList(List<user.RoleBrief> _rolelist)
    {
        Rolelist.Clear();
        //读取上次登录角色列表,把上次登陆的角色放在第一个
        if (PlayerPrefs.GetString("RoleID") != null)
        {
            for (int i = 0; i < _rolelist.Count; i++)
            {
                if (_rolelist[i].roleId.ToString() == PlayerPrefs.GetString("RoleID"))
                    Rolelist.Add(_rolelist[i]);
            }
        }
        for (int i = 0; i < _rolelist.Count; i++)
        {
            if (_rolelist[i].roleId.ToString() != PlayerPrefs.GetString("RoleID"))
                Rolelist.Add(_rolelist[i]);
        }
        return Rolelist;
    }

    private void CreateRoleTemp(List<user.RoleBrief> c_RoleList)
    {
        if (obj_RoleListgrid == null) return;

        for (int i = 0; i < RoleInfoCtrlLIst.Count; i++)
        {
            GameObject temp = obj_RoleListgrid.transform.GetChild(i).gameObject;

            temp.GetComponent<TweenAlpha>().PlayTween();
            temp.GetComponent<TweenPosition>().PlayTween();

            RoleInfoCtrl infoctrl = RoleInfoCtrlLIst[i];

            if (infoctrl == null) continue;

            if (i < c_RoleList.Count && c_RoleList[i] != null)  //如果是存在的角色
            {
                user.RoleBrief roledata = c_RoleList[i];
                infoctrl.obj_ChooseRole.gameObject.SetActive(true);
                infoctrl.obj_CreateRole.gameObject.SetActive(false);
                infoctrl.name.text = roledata.roleName;
                infoctrl.level.text = roledata.level + "级";
                CSStringBuilder.Clear();
                infoctrl.icon.spriteName = CSStringBuilder.Append("crzhiye", roledata.career.ToString()).ToString();
                CSStringBuilder.Clear();
                infoctrl.iconHL.spriteName = CSStringBuilder.Append("crzhiye", roledata.career.ToString(), "2").ToString();
                UIEventListener.Get(temp, roledata).onClick = OnChooseRoleClick;
            }
            else
            {
                infoctrl.obj_ChooseRole.gameObject.SetActive(false);
                infoctrl.obj_CreateRole.gameObject.SetActive(true);
                UIEventListener.Get(temp).onClick = OnCreateRoleClick;
            }
        }
        OnChooseRoleClick(obj_RoleListgrid.transform.GetChild(0).gameObject);
    }

    /// <summary>选中当前角色</summary>
    private void OnChooseRoleClick(GameObject obj)
    {
        if (obj == null) return;
        if (mCurInfoCtrl != null)
        {
            mCurInfoCtrl.obj_highlight.gameObject.SetActive(false);
        }
        CurRoleinfo = UIEventListener.Get(obj).parameter as user.RoleBrief;
        if (CurRoleinfo == null) return;
        for (int i = 0; i <= RoleInfoCtrlLIst.Count; i++)
        {
            if (RoleInfoCtrlLIst[i].obj.name == obj.name)
            {
                mCurInfoCtrl = RoleInfoCtrlLIst[i];
                break;
            }
        }
        mCurInfoCtrl.obj_highlight.gameObject.SetActive(true);
        mCareer = CurRoleinfo.career;
        mSex = CurRoleinfo.sex;


        ChangeCareerTexture();
        ChangeRoleModel();
    }
    private void OnReceiveChooseRoleMessage(uint id, object data)
    {
        if (data == null)
        {
            if (CurRoleinfo != null)
            {
                UnityEngine.Debug.LogError(CurRoleinfo.roleName + " 获得数据为0");
            }
            else
            {
                UnityEngine.Debug.LogError("CurRoleInfo == null_0");
            }
            return;
        }
        try
        {
            user.PlayerInfo s_RoleInfo = Network.Deserialize<user.PlayerInfo>(data);
            Constant.mRoleId = s_RoleInfo.roleBrief.roleId;
            UserManager.Instance.UpdateRoleList(s_RoleInfo.roleBrief);
            Constant.ShowTipsOnceList.Clear();
            Constant.IsWeddingCeremonyTips = false;
            Constant.IsWeddingStarTips = false;
            HotManager.Instance.ChangeState("MainScene", false, true);
            CSSubmitDataManager.Instance.StartWrite();
            CSConfigInfo.Instance.Init();
            submitGameData();//登入的时候上传数据
        }
        catch (System.Exception ex)
        {
            string roleName = CurRoleinfo != null ? CurRoleinfo.roleName : "CurRoleInfo == null_1";
            string err = ex.ToString();
            if (Platform.mPlatformType == PlatformType.EDITOR)
            {
                UnityEngine.Debug.LogError(roleName + " 异常 = " + err);
            }
            else
            {
                CoroutineManager.DoCoroutine(WriteToLog(roleName, err));
            }
        }
    }

    private IEnumerator WriteToLog(string _name, string ex)
    {
        WWWForm sum = new WWWForm();
        sum.AddField("roleName == ", _name);
        sum.AddField("platformid == ", Constant.platformid);
        sum.AddField("logString == ", ex);
        UnityWebRequest ww2 = UnityWebRequest.Post(AppUrlMain.errorUrl2, sum);
        yield return ww2.SendWebRequest();
    }

    public void submitGameData()
    {
        if (QuDaoConstant.GetPlatformData() != null && QuDaoConstant.GetPlatformData().submitData)
        {
            try
            {
                SDKUtility.SubmitGameData(1);//上传服务器数据
                if (mCurType == LoginOpenType.UICreateRole)
                {
                    SDKUtility.SubmitGameData(2);//创建角色
                }
                //上报登录服务器信息
                SDKUtility.SubmitGameData(3);//进入游戏
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("------------------------------------------" + ex);
            }
        }
    }

    private void OnReceiveRemoveRoleMessage(uint id, object data)
    {
        if (data == null) return;
        user.RoleIdMsg s_roleId = Network.Deserialize<user.RoleIdMsg>(data);
        UserManager.Instance.UpdateRemoveRoleList(s_roleId.roleId);
        if (UserManager.Instance.RoleCount <= 0)
        {
            Show(LoginOpenType.UICreateRole);
        }
        else
        {
            CreateRoleTemp(RefreshRoleList(UserManager.Instance.RoleBriefList));  //创建角色的列表
        }
        CSSubmitDataManager.Instance.StartWrite();
    }

    private void OnConnectSucceed(uint id, object data)
    {
        FNDebug.Log("收到登陆成功");
        if (CurRoleinfo == null) return;
        CoroutineManager.StopCoroutine("EnterFailTips");
        EnterGameTips(CSString.Format(432));
        CoroutineManager.DoCoroutine("EnterFailTips", CSString.Format(433));
        if (Constant.mCurServer == ServerType.GameServer)
        {
            if (CurRoleinfo != null)
            {
                Constant.mRoleId = CurRoleinfo.roleId;
                CSHotNetWork.Instance.ReqReconnect();
            }
            else
            {
                Net.ReqLoginMessage();
            }
        }
        UIManager.Instance.ClosePanel<UIWaiting>();
    }

    private void OnConnectFailure(uint id, object data)
    {
        if (!Constant.isAccountException)
        {
            if (Constant.mCurServer != ServerType.SharedService)
            {
                if (Constant.mCurServer == ServerType.CrossService)
                {
                    ConnectNum++;
                    if (ConnectNum >= 3)
                    {
                        Constant.mCurServer = ServerType.GameServer;
                        Constant.IsChangeLine = true;
                        ConnectNum = 0;
                        CSHotNetWork.Instance.ReqBackGame();
                    }
                }
                UIManager.Instance.OpenPanel<UIWaiting>().Show(true);

            }
        }
    }

    private void OnReceiveRandomRoleNameMessage(uint id, object data)
    {
        if (data == null) return;
        user.RandomRoleNameResponse m_random = (user.RandomRoleNameResponse)(data);
        if (RoleName != null) RoleName.value = m_random.name;
    }

    private void OnCreateRoleMessage(uint id, object data)
    {

    }
}
