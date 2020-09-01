using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIUpdateExitGamePanel : UIBase
{
    private GameObject _btn_config;
    private GameObject bnt_config { get { return _btn_config ?? (_btn_config = Get<GameObject>("events/RightBtn")); } }

    public override UILayerType PanelLayerType
    {
        get { return  UILayerType.Hint; }
    }
   
    public override void Init()
    {
        base.Init();
        UIEventListener.Get(bnt_config).onClick = OnFinishGame;
    }

    private void OnFinishGame(GameObject go)
    {
        QuDaoInterface.Instance.FinishGame();
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
