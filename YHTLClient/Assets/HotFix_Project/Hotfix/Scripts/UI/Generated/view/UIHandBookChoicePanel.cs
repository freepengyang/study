public partial class UIHandBookChoicePanel : UIBasePanel
{
	protected UIGridContainer mgrid_cards;
	protected UnityEngine.GameObject mbtn_position_filter;
	protected UnityEngine.GameObject mbtn_camp_filter;
	protected UnityEngine.GameObject mbtn_map_filter;
	protected UIEventListener mbtn_close;
	protected UILabel mlb_hint;
	protected UIEventListener mbtnCardLink;
	protected override void _InitScriptBinder()
	{
		mgrid_cards = ScriptBinder.GetObject("grid_cards") as UIGridContainer;
		mbtn_position_filter = ScriptBinder.GetObject("btn_position_filter") as UnityEngine.GameObject;
		mbtn_camp_filter = ScriptBinder.GetObject("btn_camp_filter") as UnityEngine.GameObject;
		mbtn_map_filter = ScriptBinder.GetObject("btn_map_filter") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
		mbtnCardLink = ScriptBinder.GetObject("btnCardLink") as UIEventListener;
	}
}
