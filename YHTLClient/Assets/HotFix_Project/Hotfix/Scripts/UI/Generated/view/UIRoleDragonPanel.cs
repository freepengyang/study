public partial class UIRoleDragonPanel : UIBasePanel
{
	protected UnityEngine.GameObject mtex_showTex;
	protected UILabel mlb_level;
	protected UnityEngine.GameObject mobj_maxState;
	protected UnityEngine.GameObject mbtn_help;
	protected UnityEngine.GameObject mbtn_upgrade;
	protected UnityEngine.GameObject mobj_att;
	protected UnityEngine.GameObject mobj_phydef;
	protected UnityEngine.GameObject mobj_magicdef;
	protected UnityEngine.GameObject mobj_hp;
	protected UnityEngine.GameObject mobj_itembar;
	protected UnityEngine.GameObject mequipShow;
	protected UnityEngine.GameObject mbtn_add;
	protected UISprite msp_icon;
	protected UnityEngine.GameObject mobj_levelEff;
	protected UnityEngine.GameObject mobj_btnRedPoint;
	protected UISprite msp_itembar;
	protected override void _InitScriptBinder()
	{
		mtex_showTex = ScriptBinder.GetObject("tex_showTex") as UnityEngine.GameObject;
		mlb_level = ScriptBinder.GetObject("lb_level") as UILabel;
		mobj_maxState = ScriptBinder.GetObject("obj_maxState") as UnityEngine.GameObject;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		mbtn_upgrade = ScriptBinder.GetObject("btn_upgrade") as UnityEngine.GameObject;
		mobj_att = ScriptBinder.GetObject("obj_att") as UnityEngine.GameObject;
		mobj_phydef = ScriptBinder.GetObject("obj_phydef") as UnityEngine.GameObject;
		mobj_magicdef = ScriptBinder.GetObject("obj_magicdef") as UnityEngine.GameObject;
		mobj_hp = ScriptBinder.GetObject("obj_hp") as UnityEngine.GameObject;
		mobj_itembar = ScriptBinder.GetObject("obj_itembar") as UnityEngine.GameObject;
		mequipShow = ScriptBinder.GetObject("equipShow") as UnityEngine.GameObject;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UnityEngine.GameObject;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mobj_levelEff = ScriptBinder.GetObject("obj_levelEff") as UnityEngine.GameObject;
		mobj_btnRedPoint = ScriptBinder.GetObject("obj_btnRedPoint") as UnityEngine.GameObject;
		msp_itembar = ScriptBinder.GetObject("sp_itembar") as UISprite;
	}
}
