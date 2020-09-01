public partial class UIGuildWealPanel : UIBasePanel
{
	protected UIInput minput_gold_num;
	protected UIInput minput_package_num;
	protected UIEventListener mBtnAddGold;
	protected UIEventListener mBtnReduceGold;
	protected UIEventListener mBtnAddNum;
	protected UIEventListener mBtnReduceNum;
	protected UIEventListener mBtnSendRedPacket;
	protected UIEventListener mBtnMakeSureSendRedPacket;
	protected UnityEngine.GameObject mGoSendRedPacketPanel;
	protected UIEventListener mBtnCloseSendRedPacket;
	protected UISprite mGoSendRedPacketBg;
	protected UIGridContainer mGridRedPacket;
	protected UnityEngine.GameObject mGoOpenRedPacketPanel;
	protected UIEventListener mBtnOpenRedPacketClose;
	protected UISprite mGoOpenRedPacketBg;
	protected UILabel mlb_residue;
	protected UILabel mlb_time;
	protected UILabel mlb_Igot;
	protected UnityEngine.GameObject mNoRedPackets;
	protected UnityEngine.GameObject mGoRedPacketAnim;
	protected TweenScale mTweenScale;
	protected TweenRotation mTweenRotation;
	protected UnityEngine.GameObject mOpenEffect;
	protected UILabel mMoneyName;
	protected UISprite mInputContent;
	protected UnityEngine.GameObject mGoRedPackAni;
	protected UnityEngine.GameObject mTexture;
	protected UISprite mMoneyIcon;
	protected override void _InitScriptBinder()
	{
		minput_gold_num = ScriptBinder.GetObject("input_gold_num") as UIInput;
		minput_package_num = ScriptBinder.GetObject("input_package_num") as UIInput;
		mBtnAddGold = ScriptBinder.GetObject("BtnAddGold") as UIEventListener;
		mBtnReduceGold = ScriptBinder.GetObject("BtnReduceGold") as UIEventListener;
		mBtnAddNum = ScriptBinder.GetObject("BtnAddNum") as UIEventListener;
		mBtnReduceNum = ScriptBinder.GetObject("BtnReduceNum") as UIEventListener;
		mBtnSendRedPacket = ScriptBinder.GetObject("BtnSendRedPacket") as UIEventListener;
		mBtnMakeSureSendRedPacket = ScriptBinder.GetObject("BtnMakeSureSendRedPacket") as UIEventListener;
		mGoSendRedPacketPanel = ScriptBinder.GetObject("GoSendRedPacketPanel") as UnityEngine.GameObject;
		mBtnCloseSendRedPacket = ScriptBinder.GetObject("BtnCloseSendRedPacket") as UIEventListener;
		mGoSendRedPacketBg = ScriptBinder.GetObject("GoSendRedPacketBg") as UISprite;
		mGridRedPacket = ScriptBinder.GetObject("GridRedPacket") as UIGridContainer;
		mGoOpenRedPacketPanel = ScriptBinder.GetObject("GoOpenRedPacketPanel") as UnityEngine.GameObject;
		mBtnOpenRedPacketClose = ScriptBinder.GetObject("BtnOpenRedPacketClose") as UIEventListener;
		mGoOpenRedPacketBg = ScriptBinder.GetObject("GoOpenRedPacketBg") as UISprite;
		mlb_residue = ScriptBinder.GetObject("lb_residue") as UILabel;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_Igot = ScriptBinder.GetObject("lb_Igot") as UILabel;
		mNoRedPackets = ScriptBinder.GetObject("NoRedPackets") as UnityEngine.GameObject;
		mGoRedPacketAnim = ScriptBinder.GetObject("GoRedPacketAnim") as UnityEngine.GameObject;
		mTweenScale = ScriptBinder.GetObject("TweenScale") as TweenScale;
		mTweenRotation = ScriptBinder.GetObject("TweenRotation") as TweenRotation;
		mOpenEffect = ScriptBinder.GetObject("OpenEffect") as UnityEngine.GameObject;
		mMoneyName = ScriptBinder.GetObject("MoneyName") as UILabel;
		mInputContent = ScriptBinder.GetObject("InputContent") as UISprite;
		mGoRedPackAni = ScriptBinder.GetObject("GoRedPackAni") as UnityEngine.GameObject;
		mTexture = ScriptBinder.GetObject("Texture") as UnityEngine.GameObject;
		mMoneyIcon = ScriptBinder.GetObject("MoneyIcon") as UISprite;
	}
}
