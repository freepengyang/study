public partial class UILianTiPanel : UIBasePanel
{
	protected UIEventListener mBtnTeleport;
	protected UIEventListener mBtnLvUp;
	protected UIEventListener mBtnHelp;
	protected UILabel mTitLeLabel;
	protected UIGridContainer mGridLowStageEffect;
	protected UnityEngine.GameObject mStageEffectBg;
	protected UnityEngine.GameObject mStageEffect;
	protected UIGridBinderContainer mGridUpStarCosts;
	protected UnityEngine.GameObject mRedPoint;
	protected UIGridBinderContainer mSixGrid;
	protected UnityEngine.GameObject mLVMaxTip;
	protected UILabel mNeedLVLabel;
	protected UISprite mSp1;
	protected UISprite mSp2;
	protected UIEventListener mIcon1;
	protected UIEventListener mIcon2;
	protected override void _InitScriptBinder()
	{
		mBtnTeleport = ScriptBinder.GetObject("BtnTeleport") as UIEventListener;
		mBtnLvUp = ScriptBinder.GetObject("BtnLvUp") as UIEventListener;
		mBtnHelp = ScriptBinder.GetObject("BtnHelp") as UIEventListener;
		mTitLeLabel = ScriptBinder.GetObject("TitLeLabel") as UILabel;
		mGridLowStageEffect = ScriptBinder.GetObject("GridLowStageEffect") as UIGridContainer;
		mStageEffectBg = ScriptBinder.GetObject("StageEffectBg") as UnityEngine.GameObject;
		mStageEffect = ScriptBinder.GetObject("StageEffect") as UnityEngine.GameObject;
		mGridUpStarCosts = ScriptBinder.GetObject("GridUpStarCosts") as UIGridBinderContainer;
		mRedPoint = ScriptBinder.GetObject("RedPoint") as UnityEngine.GameObject;
		mSixGrid = ScriptBinder.GetObject("SixGrid") as UIGridBinderContainer;
		mLVMaxTip = ScriptBinder.GetObject("LVMaxTip") as UnityEngine.GameObject;
		mNeedLVLabel = ScriptBinder.GetObject("NeedLVLabel") as UILabel;
		mSp1 = ScriptBinder.GetObject("Sp1") as UISprite;
		mSp2 = ScriptBinder.GetObject("Sp2") as UISprite;
		mIcon1 = ScriptBinder.GetObject("Icon1") as UIEventListener;
		mIcon2 = ScriptBinder.GetObject("Icon2") as UIEventListener;
	}
}
