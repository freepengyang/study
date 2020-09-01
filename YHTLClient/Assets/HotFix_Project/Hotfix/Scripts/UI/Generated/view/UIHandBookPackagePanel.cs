public partial class UIHandBookPackagePanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_map_filter;
	protected UnityEngine.GameObject mbtn_camp_filter;
	protected UnityEngine.GameObject mbtn_position_filter;
	protected UIGridContainer mgrid_cards;
	protected UnityEngine.GameObject mfixeHint;
	protected UIEventListener mbtn_quality;
	protected UnityEngine.Transform mbtn_quality_transform;
	protected UIEventListener mbtn_close;
	protected override void _InitScriptBinder()
	{
		mbtn_map_filter = ScriptBinder.GetObject("btn_map_filter") as UnityEngine.GameObject;
		mbtn_camp_filter = ScriptBinder.GetObject("btn_camp_filter") as UnityEngine.GameObject;
		mbtn_position_filter = ScriptBinder.GetObject("btn_position_filter") as UnityEngine.GameObject;
		mgrid_cards = ScriptBinder.GetObject("grid_cards") as UIGridContainer;
		mfixeHint = ScriptBinder.GetObject("fixeHint") as UnityEngine.GameObject;
		mbtn_quality = ScriptBinder.GetObject("btn_quality") as UIEventListener;
		mbtn_quality_transform = ScriptBinder.GetObject("btn_quality_transform") as UnityEngine.Transform;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
	}
}
