using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConfirmTeamInaitation : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    UIGridContainer grid_confirm_inaitation;
    UIEventListener btn_close;
    UIEventListener btn_close1;

    ILBetterList<team.InviteTeamMsg> listPlayersInaitationForMe = CSTeamInfo.Instance.listPlayersInaitationForMe;

    protected override void _InitScriptBinder()
    { 
        grid_confirm_inaitation = ScriptBinder.GetObject("grid_confirm_inaitation") as UIGridContainer;
        btn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
        btn_close1 = ScriptBinder.GetObject("btn_close1") as UIEventListener;

    }

    public override void Init()
    {
        base.Init();
        //mClientEvent.Reg((uint)CEvent.TeamTabMessage, TeamTabMessage);
        //mClientEvent.Reg((uint)CEvent.AddTeamOther, AddTeamOther);
      

        btn_close.onClick = OnClose;
        btn_close1.onClick = OnClose;
    }

    public override void Show()
    {
        base.Show();
        InitData();
    }

    void InitData()
    { 
        ShowPlayers(listPlayersInaitationForMe);
    }



    void ShowPlayers(ILBetterList<team.InviteTeamMsg> listPlayersInaitationForMe)
    {
        GameObject gp = null;
        UISprite sp_role_head = null;
        UILabel lb_role_name = null;
        UILabel lb_role_level = null;
        GameObject btn_agree = null;
        GameObject btn_refuse = null;

        grid_confirm_inaitation.MaxCount = listPlayersInaitationForMe.Count;
        for (int i = 0; i < listPlayersInaitationForMe.Count; i++)
        {
            gp = grid_confirm_inaitation.controlList[i];
            sp_role_head = gp.transform.Find("sp_role_head").gameObject.GetComponent<UISprite>();
            lb_role_name = gp.transform.Find("lb_role_name").gameObject.GetComponent<UILabel>();
            lb_role_level = gp.transform.Find("lb_role_level").gameObject.GetComponent<UILabel>();
            btn_agree = gp.transform.Find("btn_agree").gameObject;
            btn_refuse = gp.transform.Find("btn_refuse").gameObject;

            //头像
            sp_role_head.spriteName = Utility.GetPlayerIcon(listPlayersInaitationForMe[i].sex, listPlayersInaitationForMe[i].career);
            lb_role_name.text = listPlayersInaitationForMe[i].name;
            //等级
            CSStringBuilder.Clear();
            lb_role_level.text = CSStringBuilder.Append(listPlayersInaitationForMe[i].level, lb_role_level.FormatStr).ToString();
            object[] data = new object[2] { listPlayersInaitationForMe[i].teamId, listPlayersInaitationForMe[i].inviter };
            UIEventListener.Get(btn_agree, data).onClick = OnClickAgreeTeamInvitation;
            UIEventListener.Get(btn_refuse, data).onClick = OnClickRefuseTeamInvitation;
        }
    }

    void OnClickAgreeTeamInvitation(GameObject go)
    {
        object[] data = (object[])UIEventListener.Get(go).parameter;
        long teamId = (long)data[0];
        long inviter = (long)data[1];
        Net.ReqConfirmTeamInviteMessage(teamId, ConfirmTeamApplyType.Accept, inviter);
        Close();
    }

    void OnClickRefuseTeamInvitation(GameObject go)
    {
        object[] data = (object[])UIEventListener.Get(go).parameter;
        long teamId = (long)data[0];
        long inviter = (long)data[1];
        Net.ReqConfirmTeamInviteMessage(teamId, ConfirmTeamApplyType.Refuse, inviter);
        Close();
    }

    void OnClose(GameObject go)
    {
        Close();
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}