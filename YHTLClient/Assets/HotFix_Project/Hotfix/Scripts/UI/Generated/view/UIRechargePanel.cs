public partial class UIRechargePanel : UIBasePanel
{
	protected UnityEngine.GameObject mtexbg;
	protected UILabel mlb_yuanbao;
	protected UIGridContainer mgrid;
	protected override void _InitScriptBinder()
	{
		mtexbg = ScriptBinder.GetObject("texbg") as UnityEngine.GameObject;
		mlb_yuanbao = ScriptBinder.GetObject("lb_yuanbao") as UILabel;
		mgrid = ScriptBinder.GetObject("grid") as UIGridContainer;
	}
}
