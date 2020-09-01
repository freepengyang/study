public partial class UISkillCombinedPanel
{
	protected UIEventListener mBtnSkill;
	protected UnityEngine.GameObject mSkillPanel;
	protected UIEventListener mBtnClose;
	protected UIToggle mTogSkill;
	protected UIToggle mTogSkillConfig;
	protected UnityEngine.GameObject mSkillUpgradeRedPoint;
	protected override void _InitScriptBinder()
	{
		mBtnSkill = ScriptBinder.GetObject("BtnSkill") as UIEventListener;
		mSkillPanel = ScriptBinder.GetObject("SkillPanel") as UnityEngine.GameObject;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mTogSkill = ScriptBinder.GetObject("TogSkill") as UIToggle;
		mTogSkillConfig = ScriptBinder.GetObject("TogSkillConfig") as UIToggle;
		mSkillUpgradeRedPoint = ScriptBinder.GetObject("SkillUpgradeRedPoint") as UnityEngine.GameObject;
	}
}
