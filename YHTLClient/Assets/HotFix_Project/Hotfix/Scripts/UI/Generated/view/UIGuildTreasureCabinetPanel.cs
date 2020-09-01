public partial class UIGuildTreasureCabinetPanel : UIBasePanel
{
	protected UILabel menterHint;
	protected UIEventListener mbtn_go;
	protected UIGridContainer mgrid_costs;
	protected UnityEngine.GameObject msand_bg;
	protected UnityEngine.GameObject msand;
	protected UnityEngine.GameObject msand_line;
	protected override void _InitScriptBinder()
	{
		menterHint = ScriptBinder.GetObject("enterHint") as UILabel;
		mbtn_go = ScriptBinder.GetObject("btn_go") as UIEventListener;
		mgrid_costs = ScriptBinder.GetObject("grid_costs") as UIGridContainer;
		msand_bg = ScriptBinder.GetObject("sand_bg") as UnityEngine.GameObject;
		msand = ScriptBinder.GetObject("sand") as UnityEngine.GameObject;
		msand_line = ScriptBinder.GetObject("sand_line") as UnityEngine.GameObject;
	}
}
