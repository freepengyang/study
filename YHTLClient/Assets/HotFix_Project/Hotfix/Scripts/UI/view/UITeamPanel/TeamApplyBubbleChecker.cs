public class TeamApplyBubbleChecker : BubbleChecker
{
    public override int ID
    {
        get
        {
            return (int)FunctionPromptType.TeamInaitation;
        }
    }
    bool state;
    protected override void OnCreate()
    {
        mClientEvent.AddEvent(CEvent.ResInviteTeam, Open);
        mClientEvent.AddEvent(CEvent.HandledInviteTeam, Close);
    }

    protected void Open(uint id, object argv)
    {
        state = true;
        mClientEvent.SendEvent(CEvent.OnFunctionPromptChange, this.ID);
    }

    protected void Close(uint id, object argv)
    {
        state = false;
        mClientEvent.SendEvent(CEvent.OnFunctionPromptChange, this.ID);
    }

    public override bool OnCheck()
    {
        return state;
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.ResInviteTeam, Open);
        mClientEvent.RemoveEvent(CEvent.HandledInviteTeam, Close);
    }

    public override void OnClick()
    {
        UIManager.Instance.CreatePanel<UIConfirmTeamInaitation>();
    }
}