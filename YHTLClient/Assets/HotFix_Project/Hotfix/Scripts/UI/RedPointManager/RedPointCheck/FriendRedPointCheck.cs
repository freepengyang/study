public class FriendRedPointCheck : RedPointCheckBase
{
    public override void Init()
    {
        mClientEvent.AddEvent(CEvent.OnPrivateChatMessageBeRead, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnRecvNewPrivateChatMsg, OnCheckRedPoint);
    }

    public override void LoginOrFuncRedCheck()
    {
        OnCheckRedPoint(0, null);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        RefreshRed(RedPointType.Friend,CSFriendInfo.Instance.HasNewChatMessageToRead());
    }

    public override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnPrivateChatMessageBeRead, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnRecvNewPrivateChatMsg, OnCheckRedPoint);
    }
}