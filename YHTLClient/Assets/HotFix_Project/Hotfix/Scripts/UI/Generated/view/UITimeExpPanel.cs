public partial class UITimeExpPanel : UIBasePanel
{
	protected UILabel mStage;
	protected UIEventListener mBtnUpStar;
	protected UIGridContainer mGridStars;
	protected UIGridContainer mGridLowStageEffects;
	protected UILabel mStageUpEffect;
	protected UnityEngine.GameObject mRedPoint;
	protected UITable mGridUpStarCosts;
	protected UIEventListener mBtnLeft;
	protected UIEventListener mBtnRight;
	protected UnityEngine.GameObject mStageEffect;
	protected UnityEngine.GameObject mStageEffectBg;
	protected UILabel mStartUpDesc;
	protected UIEventListener mBtnStageUpEffect;
	protected UILabel mL;
	protected UIEventListener mBtnHelp;
	protected UISpriteAnimation mEffectBaoDian;
	protected UILabel mgoMax;
	protected UnityEngine.GameObject mlb_stage_up_effect2;
	protected UIGridContainer mattributeContainer;
	protected UILabel mR;
	protected UITable mmTimeExpTable;
	protected UISprite mline;
	protected UILabel mTemplate;
	protected UILabel mlb_getWay;
	protected override void _InitScriptBinder()
	{
		mStage = ScriptBinder.GetObject("Stage") as UILabel;
		mBtnUpStar = ScriptBinder.GetObject("BtnUpStar") as UIEventListener;
		mGridStars = ScriptBinder.GetObject("GridStars") as UIGridContainer;
		mGridLowStageEffects = ScriptBinder.GetObject("GridLowStageEffects") as UIGridContainer;
		mStageUpEffect = ScriptBinder.GetObject("StageUpEffect") as UILabel;
		mRedPoint = ScriptBinder.GetObject("RedPoint") as UnityEngine.GameObject;
		mGridUpStarCosts = ScriptBinder.GetObject("GridUpStarCosts") as UITable;
		mBtnLeft = ScriptBinder.GetObject("BtnLeft") as UIEventListener;
		mBtnRight = ScriptBinder.GetObject("BtnRight") as UIEventListener;
		mStageEffect = ScriptBinder.GetObject("StageEffect") as UnityEngine.GameObject;
		mStageEffectBg = ScriptBinder.GetObject("StageEffectBg") as UnityEngine.GameObject;
		mStartUpDesc = ScriptBinder.GetObject("StartUpDesc") as UILabel;
		mBtnStageUpEffect = ScriptBinder.GetObject("BtnStageUpEffect") as UIEventListener;
		mL = ScriptBinder.GetObject("L") as UILabel;
		mBtnHelp = ScriptBinder.GetObject("BtnHelp") as UIEventListener;
		mEffectBaoDian = ScriptBinder.GetObject("EffectBaoDian") as UISpriteAnimation;
		mgoMax = ScriptBinder.GetObject("goMax") as UILabel;
		mlb_stage_up_effect2 = ScriptBinder.GetObject("lb_stage_up_effect2") as UnityEngine.GameObject;
		mattributeContainer = ScriptBinder.GetObject("attributeContainer") as UIGridContainer;
		mR = ScriptBinder.GetObject("R") as UILabel;
		mmTimeExpTable = ScriptBinder.GetObject("mTimeExpTable") as UITable;
		mline = ScriptBinder.GetObject("line") as UISprite;
		mTemplate = ScriptBinder.GetObject("Template") as UILabel;
		mlb_getWay = ScriptBinder.GetObject("lb_getWay") as UILabel;
	}
}
