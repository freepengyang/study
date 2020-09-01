public partial class UIServerActivitySlayPanel : UIBasePanel
{
	protected UILabel mlb_time;
	protected UIWrapContent mGrid;
	protected UnityEngine.GameObject mbanner12;
	protected UnityEngine.GameObject msp_scroll;
	protected UIScrollView mscroll;
	protected override void _InitScriptBinder()
	{
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mGrid = ScriptBinder.GetObject("Grid") as UIWrapContent;
		mbanner12 = ScriptBinder.GetObject("banner12") as UnityEngine.GameObject;
		msp_scroll = ScriptBinder.GetObject("sp_scroll") as UnityEngine.GameObject;
		mscroll = ScriptBinder.GetObject("scroll") as UIScrollView;
	}
}
