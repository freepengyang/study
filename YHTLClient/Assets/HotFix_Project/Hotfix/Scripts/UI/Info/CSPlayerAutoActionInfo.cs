public enum PlayerAutoAction
{
    None,
    AutoFind,
    AutoFight,
}

public class CSPlayerAutoActionInfo : CSInfo<CSPlayerAutoActionInfo>
{
    private PlayerAutoAction _AutoAction;

    public PlayerAutoAction AutoAction
    {
        get { return _AutoAction; }
        set
        {
            if (_AutoAction == value) return;
            _AutoAction = value;
            mClientEvent.SendEvent(CEvent.PlayerAutoActionChange);
        }
    }

    /// <summary>
    /// 是否自动寻路
    /// </summary>
    public bool IsAutoFind
    {
        get { return AutoAction == PlayerAutoAction.AutoFind; }
    }

    /// <summary>
    /// 是否自动战斗
    /// </summary>
    public bool IsAutoFight
    {
        get { return AutoAction == PlayerAutoAction.AutoFight; }
    }

    public void SetAutoAction()
    {
        if (CSAutoFightManager.Instance.IsAutoFight)
            AutoAction = PlayerAutoAction.AutoFight;
        else if (CSPathFinderManager.IsAutoFinPath)
            AutoAction = PlayerAutoAction.AutoFind;
        else
            AutoAction = PlayerAutoAction.None;
    }


    public override void Dispose()
    {
    }
}