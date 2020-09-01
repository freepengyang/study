public partial class UIPetTalentSkillPreviewPanel : UIBasePanel
{
	protected UISprite msp_rightBg;
	protected UITable mTable_right;
	protected UnityEngine.GameObject mtableChild;
	protected override void _InitScriptBinder()
	{
		msp_rightBg = ScriptBinder.GetObject("sp_rightBg") as UISprite;
		mTable_right = ScriptBinder.GetObject("Table_right") as UITable;
		mtableChild = ScriptBinder.GetObject("tableChild") as UnityEngine.GameObject;
	}
}
