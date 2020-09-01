public partial class UISkillPanel : UIBasePanel
{
	protected UIScrollView mSkillScrollView;
	protected UIGridContainer mGridSkills;
	protected UILabel mSkillUnLearned;
	protected UILabel mSkillName;
	protected UILabel mCurLevel;
	protected UILabel mCurDesc;
	protected UILabel mNextLevel;
	protected UnityEngine.GameObject mQualityFly;
	protected UILabel mNextDesc;
	protected UILabel mHighEffectHint;
	protected UILabel mUpgradeNeedPlayerLevel;
	protected UIGridContainer mUpgradeCosts;
	protected UIEventListener mBtnUpGrade;
	protected UIEventListener mBtnHighSkillEffect;
	protected UIToggle mToggleSwitch;
	protected UnityEngine.GameObject mMicroScope;
	protected UnityEngine.GameObject mSkillConfigHandle;
	protected UnityEngine.GameObject mSkillRedPoint;
	protected UITexture mBGL;
	protected UITexture mBGR;
	protected UIWidget mCurSkillHead;
	protected UIWidget mNextSkillHead;
	protected UIWidget mQualityFlyFrame;
	protected UIWidget mBottom;
	protected UnityEngine.GameObject mQualityFlyFull;
	protected UIEventListener mBtnLookUp;
	protected UIWidget mNextHead;
	protected UIEventListener mbtn_help;
	protected UIScrollView mScrollView;
	protected override void _InitScriptBinder()
	{
        UnityEngine.Profiling.Profiler.BeginSample("Test");
        Test();
        UnityEngine.Profiling.Profiler.EndSample();
    }

    public void Test()
    {
        mSkillScrollView = ScriptBinder.GetObject("SkillScrollView") as UIScrollView;
        mGridSkills = ScriptBinder.GetObject("GridSkills") as UIGridContainer;
        mSkillUnLearned = ScriptBinder.GetObject("SkillUnLearned") as UILabel;
        mSkillName = ScriptBinder.GetObject("SkillName") as UILabel;
        mCurLevel = ScriptBinder.GetObject("CurLevel") as UILabel;
        mCurDesc = ScriptBinder.GetObject("CurDesc") as UILabel;
        mNextLevel = ScriptBinder.GetObject("NextLevel") as UILabel;
        mQualityFly = ScriptBinder.GetObject("QualityFly") as UnityEngine.GameObject;
        mNextDesc = ScriptBinder.GetObject("NextDesc") as UILabel;
        mHighEffectHint = ScriptBinder.GetObject("HighEffectHint") as UILabel;
        mUpgradeNeedPlayerLevel = ScriptBinder.GetObject("UpgradeNeedPlayerLevel") as UILabel;
        mUpgradeCosts = ScriptBinder.GetObject("UpgradeCosts") as UIGridContainer;
        mBtnUpGrade = ScriptBinder.GetObject("BtnUpGrade") as UIEventListener;
        mBtnHighSkillEffect = ScriptBinder.GetObject("BtnHighSkillEffect") as UIEventListener;
        mToggleSwitch = ScriptBinder.GetObject("ToggleSwitch") as UIToggle;
        mMicroScope = ScriptBinder.GetObject("MicroScope") as UnityEngine.GameObject;
        mSkillConfigHandle = ScriptBinder.GetObject("SkillConfigHandle") as UnityEngine.GameObject;
        mSkillRedPoint = ScriptBinder.GetObject("SkillRedPoint") as UnityEngine.GameObject;
        mBGL = ScriptBinder.GetObject("BGL") as UITexture;
        mBGR = ScriptBinder.GetObject("BGR") as UITexture;
        mCurSkillHead = ScriptBinder.GetObject("CurSkillHead") as UIWidget;
        mNextSkillHead = ScriptBinder.GetObject("NextSkillHead") as UIWidget;
        mQualityFlyFrame = ScriptBinder.GetObject("QualityFlyFrame") as UIWidget;
        mBottom = ScriptBinder.GetObject("Bottom") as UIWidget;
        mQualityFlyFull = ScriptBinder.GetObject("QualityFlyFull") as UnityEngine.GameObject;
        mBtnLookUp = ScriptBinder.GetObject("BtnLookUp") as UIEventListener;
        mNextHead = ScriptBinder.GetObject("NextHead") as UIWidget;
        mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
        mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;

    }
}
