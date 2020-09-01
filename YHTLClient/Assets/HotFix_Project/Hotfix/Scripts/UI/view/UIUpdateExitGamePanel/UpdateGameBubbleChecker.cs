public class UpdateGameBubbleChecker : BubbleChecker
{
    public override int ID
    {
        get { return (int) FunctionPromptType.UpdateGame; }
    }

    private bool isUpdate;

    protected override void OnCreate()
    {
        mClientEvent.AddEvent(CEvent.UpdateGame, OnCheckRedPoint);
    }

    protected void OnCheckRedPoint(uint id, object argv)
    {
        isUpdate = true;
        mClientEvent.SendEvent(CEvent.OnFunctionPromptChange, this.ID);
    }

    public override bool OnCheck()
    {
        return isUpdate;
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.UpdateGame, OnCheckRedPoint);
    }

    public override void OnClick()
    {
        UtilityTips.ShowPromptWordTips(74, () => { QuDaoInterface.Instance.FinishGame(); });
    }
}