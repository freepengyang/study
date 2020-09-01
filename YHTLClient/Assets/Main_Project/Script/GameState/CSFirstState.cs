
//-------------------------------------------------------------------------
//CSFirstState
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------


using UnityEngine;
using System.Collections;

public class CSFirstState : CSGameState
{
    public override GameState State
    {
        get
        {
            return GameState.FirstScene;
        }
    }
    public override void Initialize()
    {
        //AddBlackCamera();
        ChangeNextScene();
    }

    private void ChangeNextScene() 
    {
        
    }
    public override void Destroy()
    {
    }
}
