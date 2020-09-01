using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIGuildVoicePlayerBinder : UIBinder
{
    protected UILabel name;
    protected UILabel level;
    protected UISprite state;
    protected UILabel permission;
    public override void Init(UIEventListener handle)
    {
        name = Get<UILabel>("name");
        level = Get<UILabel>("level");
        state = Get<UISprite>("state");
        permission = Get<UILabel>("permission");
        handle.onClick = this.OnMemberClick;
    }

    private void OnMemberClick(GameObject go)
    {
        if (null == this.data || null == this.data.member)
            return;
        var member = this.data.member;
        long roleId = member.roleId;
        if (roleId == CSMainPlayerInfo.Instance.ID)
        {
            UtilityTips.ShowRedTips(813);
            return;
        }
        MenuInfo data = new MenuInfo();
        bool canSpeak = member.canSpeak;
        bool isBaby = false;//info.isBaby;
        data.SetFamilyTips(member.roleId,member.name, member.teamId, member.position, 
            member.contribute, member.isOnline, canSpeak, isBaby, member.sex, 0, member.level, member.career);
        data.sundryId = (int)PanelSelcetType.GuildPanel;
        data.myPosition = CSMainPlayerInfo.Instance.GuildPos;
        UIManager.Instance.CreatePanel<UIRoleSelectionMenuPanel>((f) =>
        {
            (f as UIRoleSelectionMenuPanel).ShowSelectData(data);
        });
    }

    GuildVoicePlayerData data;
    public override void Bind(object data)
    {
        this.data = data as GuildVoicePlayerData;
        if(null != this.data)
        {
            bool upMic = this.data.openVoiceSpeak;
            bool limited = !this.data.member.canSpeak;
            bool isOnline = this.data.member.isOnline;
            if (null != name)
                name.text = this.data.member.name.BBCode(isOnline ? ColorType.MainText : ColorType.WeakText);
            if (null != level)
                level.text = this.data.member.level.ToString().BBCode(isOnline ? ColorType.MainText : ColorType.WeakText);
            if (null != permission)
            {
                if(!isOnline)
                    permission.text = CSString.Format(1827).BBCode(ColorType.WeakText);
                else
                {
                    if (limited)
                    {
                        permission.text = CSString.Format(1827).BBCode(ColorType.Red);
                    }
                    else
                    {
                        permission.text = CSString.Format(1826).BBCode(ColorType.MainText);
                    }
                }
            }
            state.CustomActive(isOnline && !limited);
            if (null != this.state)
            {
                state.spriteName = this.data.openVoiceSpeak ? "chat_bi6" : "chat_bi8";
            }
        }
    }

    public override void OnDestroy()
    {
        data = null;
    }
}
public class GuildVoicePlayerData
{
    public bool openVoiceSpeak;
    public union.UnionMember member;
    public int index;
}
public partial class UIChatVoiceListPanel : UIBasePanel 
{
    public override void Init()
    {
        base.Init();

        AddCollider();
        mClientEvent.AddEvent(CEvent.OnGuildTabDataChanged, OnGuildTabDataChanged);
        mClientEvent.AddEvent(CEvent.OnRoleDetailNtfMessage, OnGuildTabDataChanged);
        mClientEvent.AddEvent(CEvent.YvVoiceUpmicPlayerChanged,OnGuildTabDataChanged);
        if (null != mScrollBar)
            EventDelegate.Add(mScrollBar.onChange, InitArrow);
    }

    protected void InitArrow()
    {
        mDownArrow.CustomActive(mScrollBar.value < 1.0f && mScrollView.shouldMoveVertically);
    }

    int GuildVoicePlayerComparer(GuildVoicePlayerData lmember, GuildVoicePlayerData rmember)
    {
        union.UnionMember l = lmember.member;
        union.UnionMember r = rmember.member;
        //有权限
        //if (lmember.limit != rmember.limit)
        //    return lmember.limit ? 1 : -1;

        //已经上麦
        if (lmember.openVoiceSpeak != rmember.openVoiceSpeak)
            return lmember.openVoiceSpeak ? -1 : 1;

        //在线排前面
        if (l.isOnline != r.isOnline)
            return l.isOnline ? -1 : 1;

        //职位大的排前面
        if (l.position != r.position)
            return l.position - r.position;

        //等级从大到小排
        if (l.level != r.level)
            return r.level - l.level;

        //战力从大到小排
        if (l.fighting != r.fighting)
            return r.fighting - l.fighting;

        //安GUID排序
        return l.roleId < r.roleId ? -1 : l.roleId == r.roleId ? 0 : 1;
    }

    EndLessList<UIGuildVoicePlayerBinder, GuildVoicePlayerData> mMemberList;
    public void RefreshMemberList()
    {
        if (null == mMemberList)
        {
            mMemberList = new EndLessList<UIGuildVoicePlayerBinder, GuildVoicePlayerData>(SortType.Vertical, mPlayerGrid, mPoolHandleManager, 12, ScriptBinder);
        }

        int upMicCnt = YvVoiceMgr.Instance.mSpeakingMembers.Count;
        int max = 5;
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(1001), out max);
        mMemberList.Clear();
        var tabInfo = CSGuildInfo.Instance.GetTabInfo(UnionTab.UnionMemberInfo);
        if (tabInfo == null || null == tabInfo.unionInfo)
        {
            mNoWar.CustomActive(true);
        }
        else
        {
            mNoWar.CustomActive(false);
            var guildInfo = tabInfo.unionInfo;
            for (int i = 0; i < guildInfo.members.Count; i++)
            {
                var member = guildInfo.members[i];
                var itemData = mMemberList.Append();
                itemData.member = member;
                itemData.index = mMemberList.Count - 1;
                //itemData.openVoiceSpeak = CSGuildInfo.Instance.IsPlayerUpMic(member.roleId);
                itemData.openVoiceSpeak = YvVoiceMgr.Instance.mSpeakingMembers.Contains(member.roleId);
                //if (itemData.openVoiceSpeak)
                //    ++upMicCnt;
            }
        }

//#if ENABLE_VOICE_DEBUG
//        upMicCnt = 0;
//        mMemberList.Clear();
//        for (int i = 0; i < 30; ++i)
//        {
//            var itemData = mMemberList.Append();
//            itemData.member = new union.UnionMember();
//            itemData.member.roleId = 12434 + i;
//            itemData.member.name = $"肃就了_{15000+i}";
//            itemData.member.level = UnityEngine.Random.Range(45, 250);
//            itemData.limit = i % 4 == 0;
//            itemData.openVoiceSpeak = i % 2 == 0;
//            if (itemData.openVoiceSpeak)
//                ++upMicCnt;
//        }
//#endif

        mMemberList.Sort(GuildVoicePlayerComparer);
        var elements = mMemberList.Elements();
        for (int i = 0; i < elements.Count; ++i)
        {
            elements[i].index = i;
        }
        mMemberList.Bind();
        InitTitleForGuild(upMicCnt, max);
    }

    protected void OnGuildTabDataChanged(uint id, object argv)
    {
        RefreshMemberList();
    }

    public override void Show()
    {
        base.Show();

        //if(YvVoiceMgr.Instance.mSpeakingMembers.Count == 0)
        //{
        //    mPlayerGrid.MaxCount = 0;
        //    mNoWar.gameObject.SetActive(true);
        //}
        //else 
        //{
        //    Net.ChatGetUpMicrPlayer(YvVoiceMgr.Instance.mSpeakingMembers);
        //}
        RefreshMemberList();
        Net.ChatGetUpMicrPlayer(YvVoiceMgr.Instance.mSpeakingMembers);
        Net.CSGetUnionTabMessage((int)UnionTab.UnionMemberInfo);
    }

    //private void OnRecvRoleDetailNtfMessage(uint id, object argv)
    //{
    //    chat.RoleDetailNtf mTeamInfos = argv as chat.RoleDetailNtf;
    //    if (null == mTeamInfos)
    //    {
    //        return;
    //    }

    //    var info = mTeamInfos.infos;
    //    if (mTeamInfos.infos != null && info.Count > 0)
    //    {
    //        mPlayerGrid.MaxCount = info.Count;
    //        for (int i = 0; i < info.Count; i++)
    //        {
    //            Transform gb = mPlayerGrid.controlList[i].transform;
    //            gb.Find("name").GetComponent<UILabel>().text = info[i].name;
    //            gb.Find("level").GetComponent<UILabel>().text = info[i].level.ToString();
    //        }
    //        mNoWar.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        mPlayerGrid.MaxCount = 0;
    //        mNoWar.gameObject.SetActive(true);
    //    }
    //}
    
    protected void InitTitleForGuild(int current,int max)
    {
        mTitle.text = CSString.Format(1824, current,max);
    }

    public void ShowMsg(ChatType channelId)
    {
        switch (channelId)
        {
            //case ChatType.CT_GUILD:
            //    mTitle.text = CSString.Format(1824,5,20);
            //    break;
            case ChatType.CT_TEAM:
                mTitle.text = CSString.Format(1825); 
                break;
        }
    }

}
