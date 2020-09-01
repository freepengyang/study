using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIConfigPushSetPanel : UIBasePanel
{
	protected UIToggle mtg_banAll;
	protected UIToggle mtg_setAll;
	protected GameObject mbtn_save;
	protected GameObject mbtn_reset;

    private CSConfigInfo mConfigInfo;

    protected override void _InitScriptBinder()
	{
		mtg_banAll = ScriptBinder.GetObject("tg_banAll") as UIToggle;
		mtg_setAll = ScriptBinder.GetObject("tg_setAll") as UIToggle;
		mbtn_save = ScriptBinder.GetObject("btn_save") as GameObject;
		mbtn_reset = ScriptBinder.GetObject("btn_reset") as GameObject;
	}


    public override void Init()
    {
        base.Init();

        mConfigInfo = CSConfigInfo.Instance;

        UIEventListener.Get(mbtn_save).onClick = SaveBtnClick;
        UIEventListener.Get(mbtn_reset).onClick = ResetBtnClick;
    }

    public override void Show()
    {
        base.Show();

        RefreshUI();
    }

    void RefreshUI()
    {

    }

    void SaveBtnClick(GameObject go)
    {

    }

    void ResetBtnClick(GameObject go)
    {

    }
}
