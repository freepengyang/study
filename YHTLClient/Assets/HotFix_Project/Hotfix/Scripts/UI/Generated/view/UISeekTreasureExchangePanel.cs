public partial class UISeekTreasureExchangePanel : UIBasePanel
{
	protected UILabel mlb_integral;
	protected UILabel mlb_instruction;
	protected UIGridContainer mgrid_exchange;
	protected UIGridContainer mgrid_record;
	protected UIEventListener mtab_weapon;
	protected UIEventListener mtab_clothes;
	protected UIEventListener mtab_jewelry;
	protected UIEventListener mtab_other;
	protected UIScrollView mRecordView;
	protected UnityEngine.GameObject mtreasure_line;
	protected UIScrollView mScrollView_exchange;
	protected override void _InitScriptBinder()
	{
		mlb_integral = ScriptBinder.GetObject("lb_integral") as UILabel;
		mlb_instruction = ScriptBinder.GetObject("lb_instruction") as UILabel;
		mgrid_exchange = ScriptBinder.GetObject("grid_exchange") as UIGridContainer;
		mgrid_record = ScriptBinder.GetObject("grid_record") as UIGridContainer;
		mtab_weapon = ScriptBinder.GetObject("tab_weapon") as UIEventListener;
		mtab_clothes = ScriptBinder.GetObject("tab_clothes") as UIEventListener;
		mtab_jewelry = ScriptBinder.GetObject("tab_jewelry") as UIEventListener;
		mtab_other = ScriptBinder.GetObject("tab_other") as UIEventListener;
		mRecordView = ScriptBinder.GetObject("RecordView") as UIScrollView;
		mtreasure_line = ScriptBinder.GetObject("treasure_line") as UnityEngine.GameObject;
		mScrollView_exchange = ScriptBinder.GetObject("ScrollView_exchange") as UIScrollView;
	}
}
