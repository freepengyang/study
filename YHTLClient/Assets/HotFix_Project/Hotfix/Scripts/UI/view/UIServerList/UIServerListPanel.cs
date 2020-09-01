using System.CodeDom.Compiler;
using UnityEngine;
using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.Experimental.AI;
using UnityEngine.Networking;

public partial class UIServerListPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    private bool isWhiteIp;
    private const int size = 100;
    private readonly Vector3 BTN_FORMAL_POSITION = new Vector3(-365, 133, -60);
    private readonly Vector3 BTN_TEST_POSITION = new Vector3(-364, 6, -60);


    private List<int> serverFrom = new List<int>(10);
    ILBetterList<ServerListData> serverList = new ILBetterList<ServerListData>(100);
    ILBetterList<ServerListData> list = new ILBetterList<ServerListData>(100);
    private ILBetterList<UIServerListData> UIServerListDataList = new ILBetterList<UIServerListData>(100);

    //选择的服务器类型
    public static bool ChooseServerFormal = true;

    public override void Init()
    {
        mClientEvent.Reg((uint) CEvent.UI_Login, RefreshServerList);
        mClientEvent.AddEvent(CEvent.UIServerListSelectTable, UIServerListSelectTable);
        mClientEvent.AddEvent(CEvent.RequestRequestRoleNum, RequestRequestRoleNum);

        ShowWaiting();
        base.Init();
        StartCountDown();
        RequestData();
        UIEventListener.Get(mbtn_close).onClick = OnClosePanel;
        UIEventListener.Get(mshowHideServer).onClick = OnShowServerClick;

        UIEventListener.Get(mbtn_formal).onClick = OnOpenFormalPanel;
        UIEventListener.Get(mbtn_test).onClick = OnOpenTestPanel;
    }

    private void ShowWaiting()
    {
        UIManager.Instance.CreatePanel<UIWaiting>(action: (f) =>
        {
            UIWaiting panel = f as UIWaiting;
            panel.Show(false);
        });
    }

    void OnClosePanel(GameObject go)
    {
        UIManager.Instance.ClosePanel<UIServerListPanel>();
    }

    public override void Show()
    {
        base.Show();
    }

    private bool isInit;

    public void InitData()
    {
        bool isShowAdvance = false;
        isWhiteIp = HttpRequest.Instance.IsAdminUser(Constant.mWhiteListIp);
        ILBetterList<ServerListData> serverData = HttpRequest.Instance.mAdvanceGameServiceList;
        if (serverData != null && serverData.Count > 0)
        {
            for (int i = 0, max = serverData.Count; i < max; i++)
            {
                if (serverData[i].S_State == 5 && !isWhiteIp)
                {
                    continue;
                }

                isShowAdvance = true;
                break;
            }
        }

        bool isShowNormal = false;
        ILBetterList<ServerListData> serverNormalData = HttpRequest.Instance.mGameServiceList;
        if (serverNormalData != null && serverNormalData.Count > 0)
        {
            for (int i = 0, max = serverNormalData.Count; i < max; i++)
            {
                if (serverNormalData[i].S_State == 5 && !isWhiteIp)
                {
                    continue;
                }

                isShowNormal = true;
                break;
            }
        }

        if (isShowNormal)
        {
            mbtn_test.transform.localPosition = BTN_TEST_POSITION;
        }
        else
        {
            mbtn_test.transform.localPosition = BTN_FORMAL_POSITION;
        }

        mbtn_formal.SetActive(isShowNormal);
        mbtn_test.SetActive(isShowAdvance);
        msp_line.height = isShowAdvance && isShowNormal ? 320 : 200;

        if (!isInit)
        {
            InitBtn();
            isInit = true;
        }

        CreateNormalTable();
    }
    private void InitBtn()
    {
        if (mbtn_formal.activeSelf)
        {
            ChooseServerFormal = true;
            mbtn_formalTo.value = true;
            mbtn_testTo.value = false;
        }
        else
        {
            ChooseServerFormal = false;
            mbtn_formalTo.value = false;
            mbtn_testTo.value = true;
        }
    }


    private void ShowInfo(ILBetterList<ServerListData> serverlist)
    {
        mServerList.MaxCount = serverlist.Count;
        int count = serverlist.Count - UIServerListDataList.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                UIServerListDataList.Add(new UIServerListData());
            }
        }

        int index = 0;
        for (var i = 0; i < serverlist.Count; i++)
        {
            index = serverlist.Count - 1 - i;
            if (UIServerListDataList[i] == null) UIServerListDataList[i] = new UIServerListData();
            UIServerListData data = UIServerListDataList[i];
            if (!data.isInit)
            {
                data.gameObject = mServerList.controlList[i];
                data.Init();
            }

            data.RefreshUI(serverlist[index]);
        }
    }

    private void CreateNormalTable()
    {
        int serverListCount = 0;
        ILBetterList<ServerListData> mGameServiceList;
        if (ChooseServerFormal)
        {
            mGameServiceList = HttpRequest.Instance.mGameServiceList;
        }
        else
        {
            mGameServiceList = HttpRequest.Instance.mAdvanceGameServiceList;
        }

        if (mGameServiceList != null)
        {
            for (int i = 0; i < mGameServiceList.Count; i++)
            {
                if (mGameServiceList[i].S_State == 5 && !isWhiteIp)
                {
                    continue;
                }

                serverListCount++;
            }
        }

        int total = Mathf.CeilToInt(serverListCount / (float) size);
        //int fixationCount = ChooseServerFormal ? 2 : 1;
        int tableCount = total + 2;

        serverFrom.Clear();
        for (int i = 0; i < tableCount; i++)
        {
            serverFrom.Add(total);
        }

        mTableList.Bind<int, ServerForm>(serverFrom, mPoolHandleManager, mClientEvent);
    }

    private void UIServerListSelectTable(uint id, object data)
    {
        OnToggleClick();
    }

    private void RequestRequestRoleNum(uint id, object data)
    {
        UIManager.Instance.ClosePanel<UIWaiting>();
        InitData();
    }

    public void OnToggleClick()
    {
        if (UIToggle.current == null || !UIToggle.current.value) return;

        serverList?.Clear();
        if (list == null) list = new ILBetterList<ServerListData>(100);
        list.Clear();

        switch (UIToggle.current.name)
        {
            case "tg_myserver":
                serverList = HttpRequest.Instance.GetRoleServerList(serverList);
                if (serverList != null)
                {
                    list.AddRange(serverList);
                }

                break;
            case "tg_recomand":
                if(ChooseServerFormal)
                {
                    serverList = HttpRequest.Instance.GetRecommendList(serverList);
                }else
                {
                    serverList = HttpRequest.Instance.GetAdvanceRecommendList(serverList);

                }
                if (serverList != null)
                {
                    list.AddRange(serverList);
                }

                break;
            default:
                if (!int.TryParse(UIToggle.current.name, out int index)) return;
                if (serverList == null) serverList = new ILBetterList<ServerListData>(100);

                if (index <= 0) index = 1;
                int startcount = 0;
                int endcount = 0;
                int total = 0;

                ILBetterList<ServerListData> mGameServiceList;
                if (ChooseServerFormal)
                {
                    mGameServiceList = HttpRequest.Instance.mGameServiceList;
                }
                else
                {
                    mGameServiceList = HttpRequest.Instance.mAdvanceGameServiceList;
                }

                if (mGameServiceList != null)
                {
                    for (int i = 0, max = mGameServiceList.Count; i < max; i++)
                    {
                        if (mGameServiceList[i].S_State == 5 && !isWhiteIp)
                        {
                            continue;
                        }

                        serverList.Add(mGameServiceList[i]);
                    }
                }

                total = Mathf.CeilToInt(serverList.Count / (float) size);

                if (index < total)
                {
                    startcount = (index - 1) * size;

                    list = serverList.GetRange(startcount, size, list);
                }
                else
                {
                    startcount = (index - 1) * size;
                    endcount = serverList.Count - startcount;
                    list = serverList.GetRange(startcount, endcount, list);
                } /*
                else if (index > total) //先行服
                {
                    list.AddRange(HttpRequest.Instance.mAdvanceGameServiceList);
                }*/

                break;
        }

        ShowInfo(list);

        if (msView != null)
        {
            msView.SetDragAmount(0.0f, 0.0f, false);
        }
    }

    public void RefreshServerList(uint id, object data)
    {
        InitData();
    }

    private void TimeLimit()
    {
        HttpRequest.Instance.StartGetServerList();
    }

    private void StartCountDown()
    {
        ScriptBinder.InvokeRepeating(15, 15, TimeLimit);
    }

    protected override void OnDestroy()
    {
        mTableList.UnBind<ServerForm>();
        UIServerListDataList?.Clear();
        base.OnDestroy();
    }


    private void RequestData()
    {
        if (QuDaoConstant.isEditorMode())
        {
            UIManager.Instance.ClosePanel<UIWaiting>();
            InitData();
        }
        else
        {
            HttpRequest.Instance.RequestRoleNum();
        }
    }

    private void RequestIP()
    {
        CoroutineManager.DoCoroutine(IP());
    }

    private IEnumerator IP()
    {
        UnityWebRequest www = UnityWebRequest.Get(AppUrl.blackUrl);

        yield return www.SendWebRequest();

        try
        {
            if (string.IsNullOrEmpty(www.error))
            {
                Constant.mWhiteListIp = www.downloadHandler.text;
                InitData();
            }

            else
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError(www.error);
#endif
            }
        }
        catch
        {
        }

        www.Dispose();
    }


    private int clickIndex = 0;

    //显示隐藏服
    private void OnShowServerClick(GameObject go)
    {
        clickIndex++;
        if (clickIndex >= 5 && clickIndex < 6)
        {
            if (string.IsNullOrEmpty(Constant.mWhiteListIp))
            {
                RequestIP();
            }
        }
    }

    private void OnOpenFormalPanel(GameObject go)
    {
        ChooseServerFormal = true;
        mClientEvent.SendEvent(CEvent.ResetServerListToggle);
        CreateNormalTable();
    }

    private void OnOpenTestPanel(GameObject go)
    {
        ChooseServerFormal = false;
        mClientEvent.SendEvent(CEvent.ResetServerListToggle);
        CreateNormalTable();
    }
}

class ServerForm : UIBinder
{
    private UILabel lb_name;
    private UILabel checkName;
    private UIToggle toggle;
    private UISprite sp_check;
    private const int size = 100;
    private GameObject gp;
    private bool isSet;

    private int total;

    public override void Init(UIEventListener handle)
    {
        gp = handle.gameObject;
        lb_name = Get<UILabel>("name");
        checkName = Get<UILabel>("check/name");
        toggle = gp.GetComponent<UIToggle>();
        sp_check = Get<UISprite>("check");
        toggle.onChange.Clear();
        //UIEventListener.Get(gp.gameObject).onClick = OnToggleClick;
        EventDelegate.Add(toggle.onChange, OnToggleClick);
        EventHandle.AddEvent(CEvent.ResetServerListToggle, ResetServerListToggle);
    }


    public override void Bind(object data)
    {
        total = (int) data;
        RefreshName();
    }

    private void RefreshName()
    {
        if (index == 0)
        {
            lb_name.text = checkName.text = CSStringTip.SERVER_MY;
            gp.name = "tg_myserver";
            return;
        }
        if (index == 1)
        {
            lb_name.text = checkName.text = CSStringTip.SERVER_RECOMMEND;
            gp.name = "tg_recomand";
            if (!isSet)
            {
                isSet = true;
                toggle.value = true;
            }
        }else
        {
            int i = total + 1 - index;
            lb_name.text = checkName.text = string.Format(CSStringTip.SERVER_FORMAT, GetAreaName(i));
            gp.name = (i + 1).ToString();
            
        }
    }

    private void OnToggleClick()
    {
        EventHandle.SendEvent(CEvent.UIServerListSelectTable);
    }

    private void ResetServerListToggle(uint id, object data)
    {
        isSet = false;
        toggle.optionCanBeNone = true;
        toggle.value = false;
        toggle.optionCanBeNone = false;
        sp_check.alpha = 0;
    }

    private string GetAreaName(int i)
    {
        int curNum = (i + 1) * size;

        return i * size + 1 + "-" + curNum;
    }

    public override void OnDestroy()
    {
        lb_name = null;
        checkName = null;
        toggle = null;
        isSet = false;
    }
}

class UIServerListData : GridContainerBase
{
    private UILabel lb_name;
    private UISprite sp_tex;
    private UISprite point;
    private Transform mNum;
    private Transform sp_head;

    private ServerListData S_data;

    public bool isInit;

    public override void Init()
    {
        isInit = true;
        lb_name = Get<UILabel>("name");
        sp_head = Get<Transform>("sp_head");
        sp_tex = Get<UISprite>("tex");
        point = Get<UISprite>("point");
        mNum = Get<Transform>("num");

        UIEventListener.Get(gameObject).onClick = OnAddressClick;
    }

    public void RefreshUI(ServerListData data)
    {
        S_data = data;
        if (S_data == null) return;

        int state = S_data.S_State;
        lb_name.text = S_data.S_Name;
        point.spriteName = GetPoint(state);

        if (mNum != null) mNum.gameObject.SetActive(false);

        if (sp_head != null)
        {
            sp_head.gameObject.SetActive(int.TryParse(GetNum(S_data.S_ID.ToString()), out int redpoint) &&
                                         redpoint > 0); //去除小红点上角色数的赋值------jlx  2017/05/23
        }

        sp_tex.gameObject.SetActive(state == 1 || state == 3);
        sp_tex.spriteName = state == 3 ? "hot" : "new";
    }

    private string GetNum(string serverid)
    {
        if (HttpRequest.Instance.mRoleNumDic != null && HttpRequest.Instance.mRoleNumDic.Count >= 0)
        {
            if (HttpRequest.Instance.mRoleNumDic.ContainsKey(serverid))
                return HttpRequest.Instance.mRoleNumDic[serverid].ToString();
        }

        return "";
    }

    private string GetPoint(int num)
    {
        switch (num)
        {
            case 1:
                return "11"; //新服
            case 2:
                return "11"; //畅通
            case 3:
                return "14"; //爆满
            default:
                return "12"; //维护
        }
    }

    public void OnAddressClick(GameObject gp)
    {
        if (S_data == null) return;

        HotManager.Instance.EventHandler.SendEvent(CEvent.Updae_ServerId, S_data);

        UIManager.Instance.ClosePanel<UIServerListPanel>();
    }


    public override void Dispose()
    {
        isInit = false;
    }
}