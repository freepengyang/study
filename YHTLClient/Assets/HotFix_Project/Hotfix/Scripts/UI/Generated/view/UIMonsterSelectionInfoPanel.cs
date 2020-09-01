public partial class UIMonsterSelectionInfoPanel : UIBasePanel
{
	protected UISprite msp_monster_head;
	protected UILabel mlb_type_ascription;
	protected UILabel mlb_monster_level;
	protected UILabel mlb_monster_name;
	protected UILabel mlb_role_name_ascription;
	protected UISlider mslider_hp;
	protected UILabel mlb_hp;
	protected UIEventListener mbtn_detailed_info;
	protected UIEventListener mbtn_close;
	protected UITable mtable;
	protected UIGridContainer mgrid_dropEquip;
	protected UnityEngine.GameObject meffectHp;
	protected override void _InitScriptBinder()
	{
		msp_monster_head = ScriptBinder.GetObject("sp_monster_head") as UISprite;
		mlb_type_ascription = ScriptBinder.GetObject("lb_type_ascription") as UILabel;
		mlb_monster_level = ScriptBinder.GetObject("lb_monster_level") as UILabel;
		mlb_monster_name = ScriptBinder.GetObject("lb_monster_name") as UILabel;
		mlb_role_name_ascription = ScriptBinder.GetObject("lb_role_name_ascription") as UILabel;
		mslider_hp = ScriptBinder.GetObject("slider_hp") as UISlider;
		mlb_hp = ScriptBinder.GetObject("lb_hp") as UILabel;
		mbtn_detailed_info = ScriptBinder.GetObject("btn_detailed_info") as UIEventListener;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mtable = ScriptBinder.GetObject("table") as UITable;
		mgrid_dropEquip = ScriptBinder.GetObject("grid_dropEquip") as UIGridContainer;
		meffectHp = ScriptBinder.GetObject("effectHp") as UnityEngine.GameObject;
	}
}
