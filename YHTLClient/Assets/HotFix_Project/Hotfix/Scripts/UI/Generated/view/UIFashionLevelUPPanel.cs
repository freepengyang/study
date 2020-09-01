public partial class UIFashionLevelUPPanel : UIBasePanel
{
	protected UILabel mlb_equipname;
	protected UIGridContainer mgrid_starts;
	protected UIGridContainer mgrid_attributes1;
	protected UIGridContainer mgrid_attributes2;
	protected UIEventListener mbtn_upgrade;
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mScrollView_1;
	protected UnityEngine.GameObject mScrollView_2;
	protected UnityEngine.GameObject mbg2;
	protected UnityEngine.GameObject mItemBase;
	protected UIGridContainer mgrid_UIItemBar;
	protected override void _InitScriptBinder()
	{
		mlb_equipname = ScriptBinder.GetObject("lb_equipname") as UILabel;
		mgrid_starts = ScriptBinder.GetObject("grid_starts") as UIGridContainer;
		mgrid_attributes1 = ScriptBinder.GetObject("grid_attributes1") as UIGridContainer;
		mgrid_attributes2 = ScriptBinder.GetObject("grid_attributes2") as UIGridContainer;
		mbtn_upgrade = ScriptBinder.GetObject("btn_upgrade") as UIEventListener;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mScrollView_1 = ScriptBinder.GetObject("ScrollView_1") as UnityEngine.GameObject;
		mScrollView_2 = ScriptBinder.GetObject("ScrollView_2") as UnityEngine.GameObject;
		mbg2 = ScriptBinder.GetObject("bg2") as UnityEngine.GameObject;
		mItemBase = ScriptBinder.GetObject("ItemBase") as UnityEngine.GameObject;
		mgrid_UIItemBar = ScriptBinder.GetObject("grid_UIItemBar") as UIGridContainer;
	}
}
