public class GuildCombatBubbleChecker : BubbleChecker
{
    public override int ID
    {
        get
        {
            return (int)FunctionPromptType.GuildCombat;
        }
    }
    

    bool state;

    TABLE.ACTIVE config;


    protected override void OnCreate()
    {
        mClientEvent.AddEvent(CEvent.GuildCombatOpen, Open);
        mClientEvent.AddEvent(CEvent.GuildCombatClose, Close);

        ActiveTableManager.Instance.TryGetValue(42, out config);
    }


    void CloseOrOpen(uint id, object argv)
    {

    }


    protected void Open(uint id, object argv)
    {
        if (!state)
        {            
            state = true;
            mClientEvent.SendEvent(CEvent.OnFunctionPromptChange, this.ID);
        }
        
    }

    protected void Close(uint id, object argv)
    {
        if (state)
        {           
            state = false;
            mClientEvent.SendEvent(CEvent.OnFunctionPromptChange, this.ID);
        }       
    }

    public override bool OnCheck()
    {
        return state;
    }

    protected override void OnDestroy()
    {
        config = null;
        mClientEvent.RemoveEvent(CEvent.GuildCombatOpen, Open);
        mClientEvent.RemoveEvent(CEvent.GuildCombatClose, Close);
    }

    public override void OnClick()
    {
        if (config == null) return;
        UtilityTips.ShowPromptWordTips(13, JoinActivity, config.name.BBCode(ColorType.Green));
    }


    void JoinActivity()
    {
        if (config == null) return;
        UtilityPanel.JumpToPanel(config.uiModel);
    }

}