public class FriendApplyBubbleChecker : BubbleChecker
{
    public override int ID 
    {
        get
        {
            return (int)FunctionPromptType.FriendRequst;
        }
    }

    protected override void OnCreate()
    {
        mClientEvent.AddEvent(CEvent.OnApplyListChanged, OnCheckRedPoint);
    }

    protected void OnCheckRedPoint(uint id,object argv)
    {
        mClientEvent.SendEvent(CEvent.OnFunctionPromptChange, this.ID);
    }

    public override bool OnCheck()
    {
        return !CSFriendInfo.Instance.IsApplyListEmpty();
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnApplyListChanged, OnCheckRedPoint);
    }

    public override void OnClick()
    {
        UIManager.Instance.CreatePanel<UIFriendResponseTipsPanel>();
    }
}