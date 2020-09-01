public partial class UISkillConfigPanel : UIBasePanel
{
	protected UIEventListener mBtnResetSkill;
	protected override void _InitScriptBinder()
	{
		mBtnResetSkill = ScriptBinder.GetObject("BtnResetSkill") as UIEventListener;
	}
}
