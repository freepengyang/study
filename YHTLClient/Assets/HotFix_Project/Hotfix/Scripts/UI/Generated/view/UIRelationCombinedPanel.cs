public partial class UIRelationCombinedPanel
{
	protected UIEventListener mBtnMail;
	protected ScriptBinder mMailPanel;
	protected UnityEngine.GameObject mFriendPanel;
	protected UIEventListener mBtnClose;
	protected UIToggle mTogMail;
	protected UIToggle mTogTeam;
	protected UIToggle mTogFriend;
	protected UIEventListener mBtnFriend;
	protected UIGrid mToggleGroup;
	protected UIEventListener mBtnTeam;
	protected ScriptBinder mTeamPanel;
	protected TweenPosition mMainTween;
	protected UnityEngine.GameObject mFriendRedPoint;
	protected override void _InitScriptBinder()
	{
		mBtnMail = ScriptBinder.GetObject("BtnMail") as UIEventListener;
		mMailPanel = ScriptBinder.GetObject("MailPanel") as ScriptBinder;
		mFriendPanel = ScriptBinder.GetObject("FriendPanel") as UnityEngine.GameObject;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mTogMail = ScriptBinder.GetObject("TogMail") as UIToggle;
		mTogTeam = ScriptBinder.GetObject("TogTeam") as UIToggle;
		mTogFriend = ScriptBinder.GetObject("TogFriend") as UIToggle;
		mBtnFriend = ScriptBinder.GetObject("BtnFriend") as UIEventListener;
		mToggleGroup = ScriptBinder.GetObject("ToggleGroup") as UIGrid;
		mBtnTeam = ScriptBinder.GetObject("BtnTeam") as UIEventListener;
		mTeamPanel = ScriptBinder.GetObject("TeamPanel") as ScriptBinder;
		mMainTween = ScriptBinder.GetObject("MainTween") as TweenPosition;
		mFriendRedPoint = ScriptBinder.GetObject("FriendRedPoint") as UnityEngine.GameObject;
	}
}
