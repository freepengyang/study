public partial class UIMainSkillPanel : UIBasePanel
{
	protected UIEventListener mBtnCommonAtk;
	protected UnityEngine.GameObject mSkillShortParent;
	protected UIEventListener mBtnAutoFight;
	protected UnityEngine.GameObject mAutoFightEffect;
	protected UIEventListener mBtnSelectPlayer;
	protected UIEventListener mBtnSelectMonster;
	protected UnityEngine.GameObject mcombinedSkill;
	protected override void _InitScriptBinder()
	{
		mBtnCommonAtk = ScriptBinder.GetObject("BtnCommonAtk") as UIEventListener;
		mSkillShortParent = ScriptBinder.GetObject("SkillShortParent") as UnityEngine.GameObject;
		mBtnAutoFight = ScriptBinder.GetObject("BtnAutoFight") as UIEventListener;
		mAutoFightEffect = ScriptBinder.GetObject("AutoFightEffect") as UnityEngine.GameObject;
		mBtnSelectPlayer = ScriptBinder.GetObject("BtnSelectPlayer") as UIEventListener;
		mBtnSelectMonster = ScriptBinder.GetObject("BtnSelectMonster") as UIEventListener;
		mcombinedSkill = ScriptBinder.GetObject("combinedSkill") as UnityEngine.GameObject;
	}
}
