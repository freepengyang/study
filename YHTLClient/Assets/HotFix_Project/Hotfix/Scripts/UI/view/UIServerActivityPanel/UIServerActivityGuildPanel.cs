using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIServerActivityGuildPanel : UIBasePanel
{
    //const int actId = 10122;
    public override void Init()
	{
		base.Init();

        CSEffectPlayMgr.Instance.ShowUITexture(mobj_banner16, "banner16");

        mbtn_go.onClick = GoClick;
    }
	
	public override void Show()
	{
		base.Show();
	}
	
	protected override void OnDestroy()
	{
        CSEffectPlayMgr.Instance.Recycle(mobj_banner16);
        base.OnDestroy();
	}


    void GoClick(GameObject go)
    {
        //UIManager.Instance.CreatePanel<UIGuildFightCombinedPanel>();
        UtilityPanel.JumpToPanel(12501);
        UIManager.Instance.ClosePanel<UIServerActivityPanel>();
    }
}
