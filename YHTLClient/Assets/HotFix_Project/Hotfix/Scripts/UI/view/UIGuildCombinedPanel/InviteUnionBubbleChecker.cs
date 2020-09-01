public class InviteUnionBubbleChecker : BubbleChecker
{
    public override int ID 
    {
        get
        {
            return (int)FunctionPromptType.InviteUnion;
        }
    }

    protected override void OnCreate()
    {
        mClientEvent.AddEvent(CEvent.OnGuildInviteMessage, OnCheckRedPoint);
    }

    protected void OnCheckRedPoint(uint id,object argv)
    {
        mClientEvent.SendEvent(CEvent.OnFunctionPromptChange, this.ID);
    }

    public override bool OnCheck()
    {
        return null != CSGuildInfo.Instance.inviteUnion;
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnGuildInviteMessage, OnCheckRedPoint);
    }

    public override void OnClick()
    {
        if(null != CSGuildInfo.Instance.inviteUnion)
        {
            var resData = CSGuildInfo.Instance.inviteUnion;
            //不管是同意还是取消清除本次邀请
            CSGuildInfo.Instance.inviteUnion = null;
            UtilityTips.ShowPromptWordTips(49,() =>
             {
                 Net.CSApplyUnionMessage(resData.unionId);
             },resData.roleName, resData.unionName);
        }
    }
}