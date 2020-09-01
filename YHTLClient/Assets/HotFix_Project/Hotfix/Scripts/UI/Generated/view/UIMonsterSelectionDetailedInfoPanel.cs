public partial class UIMonsterSelectionDetailedInfoPanel : UIBasePanel
{
	protected UISprite msp_monster_head;
	protected UILabel mlb_monster_name;
	protected UILabel mlb_monster_level;
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mtx_montser;
	protected UIGridContainer mgrid_dropEquip;
	protected override void _InitScriptBinder()
	{
		msp_monster_head = ScriptBinder.GetObject("sp_monster_head") as UISprite;
		mlb_monster_name = ScriptBinder.GetObject("lb_monster_name") as UILabel;
		mlb_monster_level = ScriptBinder.GetObject("lb_monster_level") as UILabel;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mtx_montser = ScriptBinder.GetObject("tx_montser") as UnityEngine.GameObject;
		mgrid_dropEquip = ScriptBinder.GetObject("grid_dropEquip") as UIGridContainer;
	}
}
