public partial class UIChatVoiceListPanel : UIBasePanel
{
	protected UILabel mTitle;
	protected UILabel mNoWar;
	protected UIWrapContent mPlayerGrid;
	protected UnityEngine.GameObject mDownArrow;
	protected UIScrollView mScrollView;
	protected UIScrollBar mScrollBar;
	protected override void _InitScriptBinder()
	{
		mTitle = ScriptBinder.GetObject("Title") as UILabel;
		mNoWar = ScriptBinder.GetObject("NoWar") as UILabel;
		mPlayerGrid = ScriptBinder.GetObject("PlayerGrid") as UIWrapContent;
		mDownArrow = ScriptBinder.GetObject("DownArrow") as UnityEngine.GameObject;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mScrollBar = ScriptBinder.GetObject("ScrollBar") as UIScrollBar;
	}
}
