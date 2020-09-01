public partial class UIHandBookMergePanel : UIBasePanel
{
	protected UIEventListener mBtnUpgarde;
	protected UIGridContainer mGridAttributes;
	protected UnityEngine.GameObject mChoicedCard;
	protected UnityEngine.GameObject mSlotA;
	protected UnityEngine.GameObject mSlotC;
	protected UnityEngine.GameObject mTargetHint;
	protected UnityEngine.GameObject mbg;
	protected UIEventListener mbtn_help;
	protected UnityEngine.GameObject mqualityFull;
	protected UnityEngine.GameObject mredpoint;
	protected UnityEngine.GameObject mfixeHint;
	protected UIGridContainer mcost_items;
	protected override void _InitScriptBinder()
	{
		mBtnUpgarde = ScriptBinder.GetObject("BtnUpgarde") as UIEventListener;
		mGridAttributes = ScriptBinder.GetObject("GridAttributes") as UIGridContainer;
		mChoicedCard = ScriptBinder.GetObject("ChoicedCard") as UnityEngine.GameObject;
		mSlotA = ScriptBinder.GetObject("SlotA") as UnityEngine.GameObject;
		mSlotC = ScriptBinder.GetObject("SlotC") as UnityEngine.GameObject;
		mTargetHint = ScriptBinder.GetObject("TargetHint") as UnityEngine.GameObject;
		mbg = ScriptBinder.GetObject("bg") as UnityEngine.GameObject;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		mqualityFull = ScriptBinder.GetObject("qualityFull") as UnityEngine.GameObject;
		mredpoint = ScriptBinder.GetObject("redpoint") as UnityEngine.GameObject;
		mfixeHint = ScriptBinder.GetObject("fixeHint") as UnityEngine.GameObject;
		mcost_items = ScriptBinder.GetObject("cost_items") as UIGridContainer;
	}
}
