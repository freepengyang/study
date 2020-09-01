public partial class UIWingSoulPromotePanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_upgrade;
	protected UnityEngine.GameObject meffect;
	protected UnityEngine.GameObject mitem72A;
	protected UILabel mlb_nameA;
	protected UnityEngine.GameObject mitem72B;
	protected UILabel mlb_nameB;
	protected UILabel mlb_title;
	protected UIGridContainer mgrid_cost;
	protected UnityEngine.GameObject mlb_no;
	protected UIEventListener mbtn_no;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_upgrade = ScriptBinder.GetObject("btn_upgrade") as UIEventListener;
		meffect = ScriptBinder.GetObject("effect") as UnityEngine.GameObject;
		mitem72A = ScriptBinder.GetObject("item72A") as UnityEngine.GameObject;
		mlb_nameA = ScriptBinder.GetObject("lb_nameA") as UILabel;
		mitem72B = ScriptBinder.GetObject("item72B") as UnityEngine.GameObject;
		mlb_nameB = ScriptBinder.GetObject("lb_nameB") as UILabel;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mgrid_cost = ScriptBinder.GetObject("grid_cost") as UIGridContainer;
		mlb_no = ScriptBinder.GetObject("lb_no") as UnityEngine.GameObject;
		mbtn_no = ScriptBinder.GetObject("btn_no") as UIEventListener;
	}
}
