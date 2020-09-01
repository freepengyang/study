public partial class UIServerActivityEquipPanel : UIBasePanel
{
	protected UILabel mlb_time;
	protected UIGridContainer mgrid_equipCollection;
	protected UIEventListener mbtn_rule;
	protected UnityEngine.GameObject mbanner6;
	protected UIScrollView mScrollView;
	protected override void _InitScriptBinder()
	{
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mgrid_equipCollection = ScriptBinder.GetObject("grid_equipCollection") as UIGridContainer;
		mbtn_rule = ScriptBinder.GetObject("btn_rule") as UIEventListener;
		mbanner6 = ScriptBinder.GetObject("banner6") as UnityEngine.GameObject;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
	}
}
