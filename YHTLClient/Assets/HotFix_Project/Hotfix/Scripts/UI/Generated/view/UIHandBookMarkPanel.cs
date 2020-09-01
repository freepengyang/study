public partial class UIHandBookMarkPanel : UIBasePanel
{
	protected UIGridContainer mgrid_cards;
	protected UnityEngine.GameObject mbtn_map_filter;
	protected UnityEngine.GameObject mbtn_camp_filter;
	protected UnityEngine.GameObject mbtn_position_filter;
	protected override void _InitScriptBinder()
	{
		mgrid_cards = ScriptBinder.GetObject("grid_cards") as UIGridContainer;
		mbtn_map_filter = ScriptBinder.GetObject("btn_map_filter") as UnityEngine.GameObject;
		mbtn_camp_filter = ScriptBinder.GetObject("btn_camp_filter") as UnityEngine.GameObject;
		mbtn_position_filter = ScriptBinder.GetObject("btn_position_filter") as UnityEngine.GameObject;
	}
}
