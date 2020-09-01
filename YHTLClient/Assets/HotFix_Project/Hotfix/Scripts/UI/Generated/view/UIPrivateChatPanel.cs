public partial class UIPrivateChatPanel : UIBasePanel
{
	protected UIEventListener mBtnSendMessage;
	protected UIEventListener mBtnVoice;
	protected UIEventListener mBtnAdd;
	protected UIInput mInput;
	protected TweenPosition mTweenPos;
	protected UIScrollViewChat mchatScrollView;
	protected UIScrollBar mchatScrollBar;
	protected UIChatSpringPanel mchatSpring;
	protected UnityEngine.GameObject mGpHuatong;
	protected UnityEngine.GameObject mNoReadTips;
	protected UnityEngine.GameObject mbtn_voice_huatong;
	protected UnityEngine.GameObject mbtn_voice_quxiao;
	protected UILabel mbtn_voice_text;
	protected UnityEngine.GameObject mContainer;
	protected UIEventListener mAddFriendBtn;
	protected UIEventListener mDeleteFriendBtn;
	protected UILabel mchatName;
	protected UnityEngine.GameObject mLChatTemplate;
	protected UnityEngine.GameObject mRChatTemplate;
	protected UISprite mFace;
	protected UnityEngine.GameObject mChatListView;
	protected override void _InitScriptBinder()
	{
		mBtnSendMessage = ScriptBinder.GetObject("BtnSendMessage") as UIEventListener;
		mBtnVoice = ScriptBinder.GetObject("BtnVoice") as UIEventListener;
		mBtnAdd = ScriptBinder.GetObject("BtnAdd") as UIEventListener;
		mInput = ScriptBinder.GetObject("Input") as UIInput;
		mTweenPos = ScriptBinder.GetObject("TweenPos") as TweenPosition;
		mchatScrollView = ScriptBinder.GetObject("chatScrollView") as UIScrollViewChat;
		mchatScrollBar = ScriptBinder.GetObject("chatScrollBar") as UIScrollBar;
		mchatSpring = ScriptBinder.GetObject("chatSpring") as UIChatSpringPanel;
		mGpHuatong = ScriptBinder.GetObject("GpHuatong") as UnityEngine.GameObject;
		mNoReadTips = ScriptBinder.GetObject("NoReadTips") as UnityEngine.GameObject;
		mbtn_voice_huatong = ScriptBinder.GetObject("btn_voice_huatong") as UnityEngine.GameObject;
		mbtn_voice_quxiao = ScriptBinder.GetObject("btn_voice_quxiao") as UnityEngine.GameObject;
		mbtn_voice_text = ScriptBinder.GetObject("btn_voice_text") as UILabel;
		mContainer = ScriptBinder.GetObject("Container") as UnityEngine.GameObject;
		mAddFriendBtn = ScriptBinder.GetObject("AddFriendBtn") as UIEventListener;
		mDeleteFriendBtn = ScriptBinder.GetObject("DeleteFriendBtn") as UIEventListener;
		mchatName = ScriptBinder.GetObject("chatName") as UILabel;
		mLChatTemplate = ScriptBinder.GetObject("LChatTemplate") as UnityEngine.GameObject;
		mRChatTemplate = ScriptBinder.GetObject("RChatTemplate") as UnityEngine.GameObject;
		mFace = ScriptBinder.GetObject("Face") as UISprite;
		mChatListView = ScriptBinder.GetObject("ChatListView") as UnityEngine.GameObject;
	}
}
