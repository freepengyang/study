
//-------------------------------------------------------------------------
//主场景
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class CSMainSceneState : CSGameState
{
    public override GameState State
    {
        get
        {
            return GameState.MainScene;
        }
    }

    public override void Initialize()
    {
    }
    public override void Destroy()
    {
    }
}
