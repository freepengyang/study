
//-------------------------------------------------------------------------
//CSRestartState
//Author wufuqiang
//Time 2020.07.03
//-------------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class CSRestartState : CSGameState
{
    public override GameState State
    {
        get
        {
            return GameState.RestartScene;
        }
    }
    public override void Initialize()
    {
        
    }

    private void ChangeNextScene() 
    {
        
    }
    public override void Destroy()
    {
    }
}
