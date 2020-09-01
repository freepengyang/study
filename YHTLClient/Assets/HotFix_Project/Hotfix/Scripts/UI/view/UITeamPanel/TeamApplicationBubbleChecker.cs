public class TeamApplicationBubbleChecker : BubbleChecker
{
    public override int ID
    {
        get { return (int) FunctionPromptType.EnrollmentApplication; }
    }

    bool state;

    protected override void OnCreate()
    {
        mClientEvent.AddEvent(CEvent.EnrollmentApplication, Open);
        mClientEvent.AddEvent(CEvent.HandleEnrollmentApplication, Close);
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
        mClientEvent.RemoveEvent(CEvent.HandleEnrollmentApplication, Close);
    }

    public override void OnClick()
    {
        UIManager.Instance.CreatePanel<UIRelationCombinedPanel>(
            p => (p as UIRelationCombinedPanel).OpenChildPanel((int) UIRelationCombinedPanel.ChildPanelType.CPT_TEAM)
                .SelectChildPanel(2));
    }
}