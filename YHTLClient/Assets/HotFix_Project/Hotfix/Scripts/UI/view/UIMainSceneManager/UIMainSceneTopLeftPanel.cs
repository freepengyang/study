using fight;
using UnityEngine;

public class UIMainSceneTopLeftPanel : UIBase
{
    UIGrid _grid_funcs;
    UIGrid grid_func { get { return _grid_funcs ?? (_grid_funcs = Get<UIGrid>("Table")); } }
    private GameObject _btn_mail;
    private GameObject Btn_mail { get { return _btn_mail ?? (_btn_mail = Get<GameObject>("Table/btn_mail")); } }
    private GameObject _sp_mail_red_point;
    private GameObject sp_mail_red_point { get { return _sp_mail_red_point ?? (_sp_mail_red_point = Get<GameObject>("Table/btn_mail/redpoint", UIPrefab.transform)); } }

    private GameObject _btn_relation;
    private GameObject Btn_relation { get { return _btn_relation ?? (_btn_relation = Get<GameObject>("Table/btn_relation")); } }

    private GameObject _sp_relation_red_point;
    private GameObject sp_relation_red_point { get { return _sp_mail_red_point ?? (_sp_mail_red_point = Get<GameObject>("Table/btn_relation/redpoint", UIPrefab.transform)); } }
    private GameObject _btn_friend;
    private GameObject Btn_friend { get { return _btn_friend ?? (_btn_friend = Get<GameObject>("Table/btn_friend")); } }
    private GameObject _btn_transfer;
    private GameObject Btn_transfer { get { return _btn_transfer ?? (_btn_transfer = Get<GameObject>("Table/btn_transfer")); } }



    //战斗力显示
    UILabel _lb_fightPower;

    UILabel lb_fightPower
    {
        get { return _lb_fightPower ?? (_lb_fightPower = Get<UILabel>("powerSpr/lb_power")); }
    }
    

    //buff.
    private GameObject _btn_buff;
    public GameObject btn_buff
    {
        get { return _btn_buff ? _btn_buff : _btn_buff = Get<GameObject>("btn_buff"); }
    }

    private UILabel _lb_buff;

    public UILabel lb_buff
    {
        get { return _lb_buff ? _lb_buff : _lb_buff = Get<UILabel>("btn_buff/Label"); }
    }

    public override void Init()
    {
        base.Init();
        UIEventListener.Get(Btn_mail).onClick = OnClickMail;
        UIEventListener.Get(Btn_relation).onClick = OnClickRelation;
        UIEventListener.Get(Btn_friend).onClick = OnClickFriend;
        UIEventListener.Get(Btn_transfer).onClick = OnClickTransfer;

        mClientEvent.AddEvent(CEvent.MailListChanged, OnMailListChanged);
        mClientEvent.AddEvent(CEvent.OnMailStateChanged, OnMailListChanged);
        mClientEvent.AddEvent(CEvent.FunctionOpenStateChange, FunctionOpenChange);
        mClientEvent.AddEvent(CEvent.UICheckManagerInitCheckComplete, FunctionOpenChange);
        //战斗力变动
        CSMainPlayerInfo.Instance.mClientEvent.AddEvent(CEvent.FightPowerChange, GetFightPowerChange);
        //buff
        mClientEvent.AddEvent(CEvent.Buff_Add, RefreshBuff);
        mClientEvent.AddEvent(CEvent.Buff_Remove, RefreshBuff);
        UIEventListener.Get(btn_buff).onClick = OnClickBuff;
        SetLabelBuff();

        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_Mail, Btn_mail);
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_Team, Btn_relation);
        UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_Friend, Btn_friend);
    }

    public override void Show()
    {
        base.Show();
        CheckMailRedPoint();
        lb_fightPower.text = CSMainPlayerInfo.Instance.fightPower.ToString();
        grid_func.Reposition();
    }

    protected override void OnDestroy()
    {
        CSMainPlayerInfo.Instance.mClientEvent.RemoveEvent(CEvent.FightPowerChange, GetFightPowerChange);

    }
    #region 战斗力变化
    void GetFightPowerChange(uint id, object data)
    {
        lb_fightPower.text = CSMainPlayerInfo.Instance.fightPower.ToString();
    }
    #endregion

    #region  buff
    void RefreshBuff(uint id, object data)
    {
        SetLabelBuff();
    }
    void SetLabelBuff()
    {
        int count = 0;
        ILBetterList<int> buffList = CSMainPlayerInfo.Instance.BuffInfo.buffIdList;
        if(buffList != null)
        {
            for(int i= 0; i < buffList.Count; ++i)
            {
                if (BufferTableManager.Instance.GetBufferShowOrHide(buffList[i]) == 1)
                {
                    count++;
                }
            }
        }
        lb_buff.text = count > 0 ? CSString.Format(1681, count) : CSString.Format(1686);
    }
    private void OnClickBuff(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIBuffPanel>();
    }
    #endregion



    protected void OnClickFriend(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIRelationCombinedPanel>(f =>
        {
            (f as UIRelationCombinedPanel).OpenChildPanel((int)UIRelationCombinedPanel.ChildPanelType.CPT_FRIEND)?.SelectChildPanel((int)FriendType.FT_FRIEND);
        });
    }

    protected void OnClickRelation(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIRelationCombinedPanel>(f =>
        {
            (f as UIRelationCombinedPanel).OpenChildPanel((int)UIRelationCombinedPanel.ChildPanelType.CPT_TEAM);
        });
    }

    protected void OnClickMail(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIRelationCombinedPanel>(f =>
        {
            (f as UIRelationCombinedPanel).OpenChildPanel((int)UIRelationCombinedPanel.ChildPanelType.CPT_MAIL);
        });
    }

    protected void OnClickTransfer(GameObject go)
    {
        UIManager.Instance.CreatePanel<UIMapCombinePanel>(f =>
        {
            (f as UIMapCombinePanel).OpenChildPanel((int)UIMapCombinePanel.MapPanelType.Deliver);
        });
    }

    protected void OnMailListChanged(uint eventId, object args)
    {
        CheckMailRedPoint();
    }
    void FunctionOpenChange(uint eventId, object args)
    {
        grid_func.Reposition();
    } 
    protected void CheckMailRedPoint()
    {
        if (null != sp_mail_red_point)
        {
            bool value = CSMailManager.Instance.HasUnAcquiredMail;
            sp_mail_red_point.gameObject.SetActive(value);
        }
    }

}