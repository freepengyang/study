public class MailBubbleChecker : BubbleChecker
{
    public override int ID 
    {
        get
        {
            return (int)FunctionPromptType.HasUnreadedMail;
        }
    }

    protected override void OnCreate()
    {
        mClientEvent.AddEvent(CEvent.OnMailStateChanged, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.MailListChanged, OnCheckRedPoint);
        mClientEvent.AddEvent(CEvent.OnMailRead, OnCheckRedPoint);
    }

    protected void OnCheckRedPoint(uint id,object argv)
    {
        mClientEvent.SendEvent(CEvent.OnFunctionPromptChange, this.ID);
    }

    public override bool OnCheck()
    {
        return CSMailManager.Instance.HasUnReadMail;
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnMailStateChanged, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.MailListChanged, OnCheckRedPoint);
        mClientEvent.RemoveEvent(CEvent.OnMailRead, OnCheckRedPoint);
    }

    public override void OnClick()
    {
        UIManager.Instance.CreatePanel<UIRelationCombinedPanel>(f =>
        {
            (f as UIRelationCombinedPanel).OpenChildPanel((int)UIRelationCombinedPanel.ChildPanelType.CPT_MAIL);
        });
    }
}