public partial class UIUltimateChallengeAttributePanel
{
	protected UIGridContainer mAttrbuteGrid;
	protected UnityEngine.GameObject mbtn_select;
	protected override void _InitScriptBinder()
	{
		mAttrbuteGrid = ScriptBinder.GetObject("AttrbuteGrid") as UIGridContainer;
		mbtn_select = ScriptBinder.GetObject("btn_select") as UnityEngine.GameObject;
	}
}
