public partial class UIHandBookSetupPanel : UIBasePanel
{
	protected UIGridContainer mgrid_attributes;
	protected UIEventListener mBtnHelp;
	protected UIGridContainer mgrid_cards;
	protected UIGridContainer mgrid_cardeffects;
	protected UnityEngine.GameObject mfixeHint;
	protected UIEventListener mbtn_mypackage;
	protected override void _InitScriptBinder()
	{
		mgrid_attributes = ScriptBinder.GetObject("grid_attributes") as UIGridContainer;
		mBtnHelp = ScriptBinder.GetObject("BtnHelp") as UIEventListener;
		mgrid_cards = ScriptBinder.GetObject("grid_cards") as UIGridContainer;
		mgrid_cardeffects = ScriptBinder.GetObject("grid_cardeffects") as UIGridContainer;
		mfixeHint = ScriptBinder.GetObject("fixeHint") as UnityEngine.GameObject;
		mbtn_mypackage = ScriptBinder.GetObject("btn_mypackage") as UIEventListener;
	}
}
