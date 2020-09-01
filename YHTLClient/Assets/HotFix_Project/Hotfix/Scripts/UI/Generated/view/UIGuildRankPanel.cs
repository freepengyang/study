public partial class UIGuildRankPanel : UIBasePanel
{
	protected UnityEngine.GameObject mrole1;
	protected UnityEngine.GameObject mrole2;
	protected UnityEngine.GameObject mrole3;
	protected UIGridContainer mContainer;
	protected UIEventListener mbtn_help;
	protected UnityEngine.GameObject mtex_bg;
	protected UnityEngine.GameObject mDownArrow;
	protected UnityEngine.GameObject mUpArrow;
	protected UIScrollBar mScrollBar;
	protected UIScrollView mScrollView;
	protected override void _InitScriptBinder()
	{
		mrole1 = ScriptBinder.GetObject("role1") as UnityEngine.GameObject;
		mrole2 = ScriptBinder.GetObject("role2") as UnityEngine.GameObject;
		mrole3 = ScriptBinder.GetObject("role3") as UnityEngine.GameObject;
		mContainer = ScriptBinder.GetObject("Container") as UIGridContainer;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UnityEngine.GameObject;
		mDownArrow = ScriptBinder.GetObject("DownArrow") as UnityEngine.GameObject;
		mUpArrow = ScriptBinder.GetObject("UpArrow") as UnityEngine.GameObject;
		mScrollBar = ScriptBinder.GetObject("ScrollBar") as UIScrollBar;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
	}
}
