public partial class UIColoursWorldPanel : UIBasePanel
{
	protected UIInput mInput;
	protected UILabel mIngot;
	protected UIEventListener mBtnClose;
	protected UIEventListener mBtnVoice;
	protected UIEventListener mBtnSend;
	protected UnityEngine.GameObject mMic;
	protected UIEventListener mBtnMic;
	protected UIEventListener mBtnVoiceCancel;
	protected UILabel mVoiceText;
	protected override void _InitScriptBinder()
	{
		mInput = ScriptBinder.GetObject("Input") as UIInput;
		mIngot = ScriptBinder.GetObject("Ingot") as UILabel;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mBtnVoice = ScriptBinder.GetObject("BtnVoiceWorld") as UIEventListener;
		mBtnSend = ScriptBinder.GetObject("BtnSend") as UIEventListener;
		mMic = ScriptBinder.GetObject("Mic") as UnityEngine.GameObject;
		mBtnMic = ScriptBinder.GetObject("BtnMic") as UIEventListener;
		mBtnVoiceCancel = ScriptBinder.GetObject("BtnVoiceCancel") as UIEventListener;
		mVoiceText = ScriptBinder.GetObject("VoiceText") as UILabel;
	}
}
