using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Resources;

public class UIFeatureTurnTable : UIBase
{
    private GameObject _btn_function;

    private GameObject btn_function
    {
        get { return _btn_function ?? (_btn_function = Get("btn_function").gameObject); }
    }


    private GameObject _redpoint;

    private GameObject redpoint
    {
        get { return _redpoint ?? (_redpoint = Get("btn_function/redpoint").gameObject); }
    }

    public override void Init()
    {
        base.Init();
        UIEventListener.Get(btn_function).onClick = OnOpenFunctionClick;
    }

    public override void Show()
    {
        base.Show();
        RegisterRes();
    }

    private void RegisterRes()
    {
        Map<FuntionMuneType, RedPointType[]> redPointTypeses = CSFunPanelRedPointInfo.Instance.GetFunRed();
        if (redPointTypeses != null)
        {
            for (redPointTypeses.Begin(); redPointTypeses.Next();)
            {
                //普通装备锻造   卧龙装备锻造红点不在主界面显示 只在轮盘界面显示
                if (redPointTypeses.Key != FuntionMuneType.RongLian && redPointTypeses.Key != FuntionMuneType.ForgePanel)
                {
                    RegisterRedList(redpoint, redPointTypeses.Value);
                }
            }
        }
    }

    private void RegisterRedList(GameObject go, params RedPointType[] pointType)
    {
        CSRedPointManager.Instance.RegisterRedPoint(go, pointType);
    }

    private void OnOpenFunctionClick(GameObject go)
    {
        UIManager.Instance.CloseAllPanelByFunc();
        UIManager.Instance.CreatePanel<UIFunctionPanel>();
    }

    protected override void OnDestroy()
    {
        CSRedPointManager.Instance.Recycle(redpoint);
        base.OnDestroy();
    }
}