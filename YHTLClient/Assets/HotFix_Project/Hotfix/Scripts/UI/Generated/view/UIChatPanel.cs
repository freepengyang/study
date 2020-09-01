public partial class UIChatPanel : UIBasePanel
{
	protected UIEventListener mBtnClose;
	protected UIGridContainer mChatGroup;
	protected UnityEngine.GameObject mChannelTemplate;
	protected TweenPosition mPanelTween;
	protected UIEventListener mBtnSend;
	protected UIInput mInput;
	protected UnityEngine.GameObject mChatListView;
	protected UnityEngine.GameObject mLChatTemplate;
	protected UnityEngine.GameObject mRChatTemplate;
	protected UIEventListener mBtnAdd;
	protected UIEventListener mBtnMaskLayer;
	protected UIEventListener mBtnVoice;
	protected UILabel mVoiceText;
	protected UnityEngine.GameObject mMic;
	protected UnityEngine.GameObject mMicUndo;
	protected UILabel mNoChat;
	protected UnityEngine.GameObject mUnReadTips;
	protected UIScrollViewChat mScrollViewChat;
	protected UnityEngine.GameObject mVoicePanelHandle;
	protected UIPanel mChatViewPanel;
	protected UIChatSpringPanel mChatSpring;
	protected UIScrollBar mChatScrollBar;
	protected UnityEngine.GameObject mMicHandle;
	protected UnityEngine.GameObject mReplaceHandle;
	protected UIEventListener mBtnSetting;
	protected UIPanel mChatSettingPanel;
	protected override void _InitScriptBinder()
	{
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mChatGroup = ScriptBinder.GetObject("ChatGroup") as UIGridContainer;
		mChannelTemplate = ScriptBinder.GetObject("ChannelTemplate") as UnityEngine.GameObject;
		mPanelTween = ScriptBinder.GetObject("PanelTween") as TweenPosition;
		mBtnSend = ScriptBinder.GetObject("BtnSend") as UIEventListener;
		mInput = ScriptBinder.GetObject("Input") as UIInput;
		mChatListView = ScriptBinder.GetObject("ChatListView") as UnityEngine.GameObject;
		mLChatTemplate = ScriptBinder.GetObject("LChatTemplate") as UnityEngine.GameObject;
		mRChatTemplate = ScriptBinder.GetObject("RChatTemplate") as UnityEngine.GameObject;
		mBtnAdd = ScriptBinder.GetObject("BtnAdd") as UIEventListener;
		mBtnMaskLayer = ScriptBinder.GetObject("BtnMaskLayer") as UIEventListener;
		mBtnVoice = ScriptBinder.GetObject("BtnVoice") as UIEventListener;
		mVoiceText = ScriptBinder.GetObject("VoiceText") as UILabel;
		mMic = ScriptBinder.GetObject("Mic") as UnityEngine.GameObject;
		mMicUndo = ScriptBinder.GetObject("MicUndo") as UnityEngine.GameObject;
		mNoChat = ScriptBinder.GetObject("NoChat") as UILabel;
		mUnReadTips = ScriptBinder.GetObject("UnReadTips") as UnityEngine.GameObject;
		mScrollViewChat = ScriptBinder.GetObject("ScrollViewChat") as UIScrollViewChat;
		mVoicePanelHandle = ScriptBinder.GetObject("VoicePanelHandle") as UnityEngine.GameObject;
		mChatViewPanel = ScriptBinder.GetObject("ChatViewPanel") as UIPanel;
		mChatSpring = ScriptBinder.GetObject("ChatSpring") as UIChatSpringPanel;
		mChatScrollBar = ScriptBinder.GetObject("ChatScrollBar") as UIScrollBar;
		mMicHandle = ScriptBinder.GetObject("MicHandle") as UnityEngine.GameObject;
		mReplaceHandle = ScriptBinder.GetObject("ReplaceHandle") as UnityEngine.GameObject;
		mBtnSetting = ScriptBinder.GetObject("BtnSetting") as UIEventListener;
		mChatSettingPanel = ScriptBinder.GetObject("ChatSettingPanel") as UIPanel;
	}
}
