public partial class UIGuildMemberListPanel : UIBasePanel
{
	protected UnityEngine.Transform mwindow;
	protected UIToggle mHideOffline;
	protected UIEventListener mbtnChat;
	protected UILabel mOnlineCount;
	protected UIEventListener mbtn_cancelguild;
	protected UIEventListener mbtn_accusechief;
	protected UILabel mlb_accusechief;
	protected UIScrollView mScrollView;
	protected UIWrapContent mContainer;
	protected UILabel mlb_cancelguild;
	protected override void _InitScriptBinder()
	{
		mwindow = ScriptBinder.GetObject("window") as UnityEngine.Transform;
		mHideOffline = ScriptBinder.GetObject("HideOffline") as UIToggle;
		mbtnChat = ScriptBinder.GetObject("btnChat") as UIEventListener;
		mOnlineCount = ScriptBinder.GetObject("OnlineCount") as UILabel;
		mbtn_cancelguild = ScriptBinder.GetObject("btn_cancelguild") as UIEventListener;
		mbtn_accusechief = ScriptBinder.GetObject("btn_accusechief") as UIEventListener;
		mlb_accusechief = ScriptBinder.GetObject("lb_accusechief") as UILabel;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mContainer = ScriptBinder.GetObject("Container") as UIWrapContent;
		mlb_cancelguild = ScriptBinder.GetObject("lb_cancelguild") as UILabel;
	}
}
