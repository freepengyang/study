//-------------------------------------------------------------------------
//登录
//Author jiabao
//Time 2015.12.15
//-------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using AssetBundles;

public class CSLoginState : CSGameState
{
    public override GameState State
    {
        get { return GameState.LoginScene; }
    }

    public override void Initialize()
    {
        if (CSGame.Sington == null) return;
        //AddBlackCamera();
        if (CSGame.Sington.IsToRoleListPanel)
        {
            CSGame.Sington.LoginStateInit?.Run();
            CSGame.Sington.IsToRoleListPanel = false;
        }
        else
        {
            if (CSGame.Sington.IsFirstTo)
            {
                CheckVersion();
                CSGame.Sington.IsFirstTo = false;
            }
            else
            {
                CSGame.Sington.LoginInit?.Run();
            }
        }
    }

    public void CheckVersion()
    {
        AssetBundleManager.InitializeMaifest();

        CSGame.Sington.CreateDownLoad?.Run((int) DownloadUIType.CheckUpdate);
    }

    public override void Destroy()
    {
    }
}