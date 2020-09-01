public partial class UIGuildListPanel : UIBasePanel
{
	protected UIEventListener mbtnCreate;
	protected UIEventListener mbtn_help;
	protected UIToggle mtg_select;
	protected UILabel mlb_timecount;
	protected UIScrollView mScrollView;
	protected UIWrapContent mContainer;
	protected UILabel mlb_create;
	protected UIEventListener mbtnOneKeyApply;
	protected UnityEngine.GameObject mEmptyDesc;
	protected UnityEngine.GameObject mpattern;
	protected override void _InitScriptBinder()
	{
		mbtnCreate = ScriptBinder.GetObject("btnCreate") as UIEventListener;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		mtg_select = ScriptBinder.GetObject("tg_select") as UIToggle;
		mlb_timecount = ScriptBinder.GetObject("lb_timecount") as UILabel;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mContainer = ScriptBinder.GetObject("Container") as UIWrapContent;
		mlb_create = ScriptBinder.GetObject("lb_create") as UILabel;
		mbtnOneKeyApply = ScriptBinder.GetObject("btnOneKeyApply") as UIEventListener;
		mEmptyDesc = ScriptBinder.GetObject("EmptyDesc") as UnityEngine.GameObject;
		mpattern = ScriptBinder.GetObject("pattern") as UnityEngine.GameObject;
	}
}
