public partial class UIWarPetCombinedPanel : UIBasePanel
{
	protected UIEventListener mBtnSkill;
	protected UnityEngine.GameObject mWarPetSkillPanel;
	protected UIEventListener mBtnClose;
	protected UIToggle mTogSkill;
	protected UnityEngine.GameObject mSkillRedPoint;
	protected UIToggle mTogWarsoul;
	protected UnityEngine.GameObject mUIWarSoulPanel;
	protected UIToggle mTogRefine;
	protected UnityEngine.GameObject mUIWarPetRefinePanel;
	protected UIToggle mTogTalent;
	protected UnityEngine.GameObject mUIPetTalentPanel;
	protected UnityEngine.GameObject mTalentRedPoint;
	protected UnityEngine.GameObject mRefineRedpoint;
	protected UnityEngine.GameObject mbtn_skill;
	protected UnityEngine.GameObject mbtn_refine;
	protected UIGrid mgrid_Group;
	protected override void _InitScriptBinder()
	{
		mBtnSkill = ScriptBinder.GetObject("BtnSkill") as UIEventListener;
		mWarPetSkillPanel = ScriptBinder.GetObject("WarPetSkillPanel") as UnityEngine.GameObject;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mTogSkill = ScriptBinder.GetObject("TogSkill") as UIToggle;
		mSkillRedPoint = ScriptBinder.GetObject("SkillRedPoint") as UnityEngine.GameObject;
		mTogWarsoul = ScriptBinder.GetObject("TogWarsoul") as UIToggle;
		mUIWarSoulPanel = ScriptBinder.GetObject("UIWarSoulPanel") as UnityEngine.GameObject;
		mTogRefine = ScriptBinder.GetObject("TogRefine") as UIToggle;
		mUIWarPetRefinePanel = ScriptBinder.GetObject("UIWarPetRefinePanel") as UnityEngine.GameObject;
		mTogTalent = ScriptBinder.GetObject("TogTalent") as UIToggle;
		mUIPetTalentPanel = ScriptBinder.GetObject("UIPetTalentPanel") as UnityEngine.GameObject;
		mTalentRedPoint = ScriptBinder.GetObject("TalentRedPoint") as UnityEngine.GameObject;
		mRefineRedpoint = ScriptBinder.GetObject("RefineRedpoint") as UnityEngine.GameObject;
		mbtn_skill = ScriptBinder.GetObject("btn_skill") as UnityEngine.GameObject;
		mbtn_refine = ScriptBinder.GetObject("btn_refine") as UnityEngine.GameObject;
		mgrid_Group = ScriptBinder.GetObject("grid_Group") as UIGrid;
	}
}
