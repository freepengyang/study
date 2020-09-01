using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOpenFrame : ActionBase
{
    public class Factory : ActionFactory<ActionOpenFrame,ActionOpenFrameParam>
    {

    }

    public override int ID 
    {
        get
        {
            return (int)EnumAction.OpenFrame;
        }
    }

    ActionOpenFrameParam frameParam;
    public override void Init(IActionParam argv)
    {
        base.Init(argv);
        frameParam = argv as ActionOpenFrameParam;
    }

    public override bool IsDone()
    {
        Succeed = false;
        if (null != frameParam)
        {
            Succeed = UtilityPanel.JumpToPanel(frameParam.gameModelId);
        }
        return true;
    }

    public override void OnRecycle()
    {
        frameParam = null;
        base.OnRecycle();
    }
}