public partial class UIGuildScoreListPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UILabel mmyrank;
	protected UILabel mmyscore;
	protected UIGridContainer mGrildList;
	protected UnityEngine.GameObject marrow;
	protected UIScrollBar mScrollBar;
	protected UIScrollView mScrollView;
	protected UIEventListener mbtnBG;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mmyrank = ScriptBinder.GetObject("myrank") as UILabel;
		mmyscore = ScriptBinder.GetObject("myscore") as UILabel;
		mGrildList = ScriptBinder.GetObject("GrildList") as UIGridContainer;
		marrow = ScriptBinder.GetObject("arrow") as UnityEngine.GameObject;
		mScrollBar = ScriptBinder.GetObject("ScrollBar") as UIScrollBar;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mbtnBG = ScriptBinder.GetObject("btnBG") as UIEventListener;
	}
}
