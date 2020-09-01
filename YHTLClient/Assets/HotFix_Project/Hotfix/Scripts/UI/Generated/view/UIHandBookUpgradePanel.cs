public partial class UIHandBookUpgradePanel : UIBasePanel
{
	protected UIEventListener mBtnHelp;
	protected UIEventListener mBtnUpgarde;
	protected UIGridContainer mgrid_attributes;
	protected UnityEngine.GameObject mChoicedCard;
	protected UnityEngine.GameObject mbg;
	protected UIEventListener mbtn_help;
	protected UnityEngine.GameObject mfixeHint;
	protected UIGridContainer mcost_items;
	protected UnityEngine.GameObject mlevelFull;
	protected UnityEngine.GameObject mredPoint;
	protected UILabel mquality_hint;
	protected override void _InitScriptBinder()
	{
		mBtnHelp = ScriptBinder.GetObject("BtnHelp") as UIEventListener;
		mBtnUpgarde = ScriptBinder.GetObject("BtnUpgarde") as UIEventListener;
		mgrid_attributes = ScriptBinder.GetObject("grid_attributes") as UIGridContainer;
		mChoicedCard = ScriptBinder.GetObject("ChoicedCard") as UnityEngine.GameObject;
		mbg = ScriptBinder.GetObject("bg") as UnityEngine.GameObject;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		mfixeHint = ScriptBinder.GetObject("fixeHint") as UnityEngine.GameObject;
		mcost_items = ScriptBinder.GetObject("cost_items") as UIGridContainer;
		mlevelFull = ScriptBinder.GetObject("levelFull") as UnityEngine.GameObject;
		mredPoint = ScriptBinder.GetObject("redPoint") as UnityEngine.GameObject;
		mquality_hint = ScriptBinder.GetObject("quality_hint") as UILabel;
	}
}
