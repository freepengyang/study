public class FriendPrivateChatBubbleChecker : BubbleChecker
{
    public override int ID
    {
        get
        {
            return (int)FunctionPromptType.FriendPrivateChat;
        }
    }

    protected override void OnCreate()
    {
        mClientEvent.AddEvent(CEvent.OnPrivateChatMessageBeRead, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnRecvNewPrivateChatMsg, OnCheckRedPoint);
    }

    protected void OnCheckRedPoint(uint id,object argv)
    {
        mClientEvent.SendEvent(CEvent.OnFunctionPromptChange, this.ID);
    }

    public override bool OnCheck()
    {
        return CSFriendInfo.Instance.HasNewChatMessageToRead();
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnPrivateChatMessageBeRead, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnRecvNewPrivateChatMsg, OnCheckRedPoint);
    }

    public override void OnClick()
    {
        var privateChatList = CSFriendInfo.Instance.GetPrivateChatList();
        if(privateChatList.Count > 0 && null != privateChatList[0].Info)
        {
            UIManager.Instance.CreatePanel<UIRelationCombinedPanel>(f =>
            {
                (f as UIRelationCombinedPanel).OpenChildPanel((int)UIRelationCombinedPanel.ChildPanelType.CPT_FRIEND)?.RefreshData((int)FriendType.FT_NONE, privateChatList[0].Info.roleId);
            });
        }
    }
}