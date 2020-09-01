public partial class UIFriendPanel : UIBasePanel
{
	protected UIToggle mToggleBlackList;
	protected UIToggle mToggleDebt;
	protected UIToggle mToggleFriend;
	protected UIToggle mToggleRelation;
	protected UIEventListener mBtnAddFriend;
	protected UIEventListener mBtnHelper;
	protected UIGridContainer mGridList;
	protected UIScrollView mFriendListScroll;
	protected UIScrollView mFriendScrollView;
	protected UILabel mCount;
	protected UILabel mFriendCount;
	protected UILabel mLeftDesc;
	protected UILabel mRightDesc;
	protected UISprite mRelationRedPoint;
	protected UIWidget mContainer;
	protected UnityEngine.GameObject mPrivateChatHandle;
	protected UnityEngine.GameObject mAddFriendHandle;
	protected UnityEngine.GameObject mTouchListRedPoint;
	protected UIEventListener mBtnClearList;
	protected UnityEngine.GameObject mPattern;
	protected override void _InitScriptBinder()
	{
		mToggleBlackList = ScriptBinder.GetObject("ToggleBlackList") as UIToggle;
		mToggleDebt = ScriptBinder.GetObject("ToggleDebt") as UIToggle;
		mToggleFriend = ScriptBinder.GetObject("ToggleFriend") as UIToggle;
		mToggleRelation = ScriptBinder.GetObject("ToggleRelation") as UIToggle;
		mBtnAddFriend = ScriptBinder.GetObject("BtnAddFriend") as UIEventListener;
		mBtnHelper = ScriptBinder.GetObject("BtnHelper") as UIEventListener;
		mGridList = ScriptBinder.GetObject("GridList") as UIGridContainer;
		mFriendListScroll = ScriptBinder.GetObject("FriendListScroll") as UIScrollView;
		mFriendScrollView = ScriptBinder.GetObject("FriendScrollView") as UIScrollView;
		mCount = ScriptBinder.GetObject("Count") as UILabel;
		mFriendCount = ScriptBinder.GetObject("FriendCount") as UILabel;
		mLeftDesc = ScriptBinder.GetObject("LeftDesc") as UILabel;
		mRightDesc = ScriptBinder.GetObject("RightDesc") as UILabel;
		mRelationRedPoint = ScriptBinder.GetObject("RelationRedPoint") as UISprite;
		mContainer = ScriptBinder.GetObject("Container") as UIWidget;
		mPrivateChatHandle = ScriptBinder.GetObject("PrivateChatHandle") as UnityEngine.GameObject;
		mAddFriendHandle = ScriptBinder.GetObject("AddFriendHandle") as UnityEngine.GameObject;
		mTouchListRedPoint = ScriptBinder.GetObject("TouchListRedPoint") as UnityEngine.GameObject;
		mBtnClearList = ScriptBinder.GetObject("BtnClearList") as UIEventListener;
		mPattern = ScriptBinder.GetObject("Pattern") as UnityEngine.GameObject;
	}
}
