public partial class UIHandBookCardSelectPanel : UIBasePanel
{
	protected UIEventListener mbtn_confirmed;
	protected UIGridContainer mgrid_cardeffects;
	protected UIGridContainer mgrid_cards;
	protected UnityEngine.GameObject mbtn_position_filter;
	protected UnityEngine.GameObject mbtn_camp_filter;
	protected UnityEngine.GameObject mbtn_map_filter;
	protected UILabel mlb_equiped_hint;
	protected UIEventListener mbtn_card_group;
	protected UIEventListener mbtn_attr_effect;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtnCardLink;
	protected UIScrollView mCardScrollView;
	protected override void _InitScriptBinder()
	{
		mbtn_confirmed = ScriptBinder.GetObject("btn_confirmed") as UIEventListener;
		mgrid_cardeffects = ScriptBinder.GetObject("grid_cardeffects") as UIGridContainer;
		mgrid_cards = ScriptBinder.GetObject("grid_cards") as UIGridContainer;
		mbtn_position_filter = ScriptBinder.GetObject("btn_position_filter") as UnityEngine.GameObject;
		mbtn_camp_filter = ScriptBinder.GetObject("btn_camp_filter") as UnityEngine.GameObject;
		mbtn_map_filter = ScriptBinder.GetObject("btn_map_filter") as UnityEngine.GameObject;
		mlb_equiped_hint = ScriptBinder.GetObject("lb_equiped_hint") as UILabel;
		mbtn_card_group = ScriptBinder.GetObject("btn_card_group") as UIEventListener;
		mbtn_attr_effect = ScriptBinder.GetObject("btn_attr_effect") as UIEventListener;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtnCardLink = ScriptBinder.GetObject("btnCardLink") as UIEventListener;
		mCardScrollView = ScriptBinder.GetObject("CardScrollView") as UIScrollView;
	}
}
