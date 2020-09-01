public class TimeLimitActivityBubbleChecker : BubbleChecker
{
    public override int ID
    {
        get
        {
            if (config != null)
            {
                return (int)FunctionPromptType.TimeLimitActivity + config.id;
            }
            return (int)FunctionPromptType.TimeLimitActivity;
        }
    }


    public override int IconID
    {
        get
        {
            if (config != null)
            {                
                return (int)FunctionPromptType.TimeLimitActivity + actIcon;
            }
            return (int)FunctionPromptType.TimeLimitActivity;
        }
    }


    public TABLE.ACTIVE config;

    int actIcon;


    bool state;


    protected override void OnCreate()
    {
        mClientEvent.AddEvent(CEvent.ActivityBubbleAdded, Open);
        mClientEvent.AddEvent(CEvent.ActivityBubbleRemoved, Close);

        if (config != null)
        {
            int.TryParse(config.icon, out actIcon);
        }
    }

    protected void Open(uint id, object argv)
    {
        if (config == null) return;
        if (!state)
        {
            int actId = System.Convert.ToInt32(argv);
            if (actId != config.id) return;
            state = true;
            mClientEvent.SendEvent(CEvent.OnFunctionPromptChange, this.ID);
        }
        
    }

    protected void Close(uint id, object argv)
    {
        if (config == null) return;
        if (state)
        {
            int actId = System.Convert.ToInt32(argv);
            if (actId != config.id) return;
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
        mClientEvent.RemoveEvent(CEvent.ActivityBubbleAdded, Open);
        mClientEvent.RemoveEvent(CEvent.ActivityBubbleRemoved, Close);
    }

    public override void OnClick()
    {
        if (config == null) return;
        UtilityTips.ShowPromptWordTips(13, JoinActivity, config.name.BBCode(ColorType.Green));
    }


    void JoinActivity()
    {
        if (config == null) return;

        if (config.deliver != 0)
        {
            UtilityPath.FindWithDeliverId(config.deliver);
        }
        else if (config.uiModel != 0)
        {
            UtilityPanel.JumpToPanel(config.uiModel);
        }
    }

}