public partial class UICheckDragonPanel : UIBasePanel
{
	protected UILabel mrolename;
	protected UnityEngine.GameObject mwolongEquip;
	protected UILabel mlb_rolename;
	protected UILabel mlb_rolelevel;
	protected UIEventListener mbtn_ToNormal;
	protected UnityEngine.GameObject mRole;
	protected UnityEngine.GameObject mCloth;
	protected UnityEngine.GameObject mWeapon;
	protected UILabel mlb_dragonLevel;
	protected UnityEngine.GameObject mmaxState;
	protected UIEventListener mbtn_help;
	protected UnityEngine.GameObject meffect;
	protected UnityEngine.GameObject matt;
	protected UnityEngine.GameObject mphydef;
	protected UnityEngine.GameObject mmagicdef;
	protected UnityEngine.GameObject mhp;
	protected override void _InitScriptBinder()
	{
		mrolename = ScriptBinder.GetObject("rolename") as UILabel;
		mwolongEquip = ScriptBinder.GetObject("wolongEquip") as UnityEngine.GameObject;
		mlb_rolename = ScriptBinder.GetObject("lb_rolename") as UILabel;
		mlb_rolelevel = ScriptBinder.GetObject("lb_rolelevel") as UILabel;
		mbtn_ToNormal = ScriptBinder.GetObject("btn_ToNormal") as UIEventListener;
		mRole = ScriptBinder.GetObject("Role") as UnityEngine.GameObject;
		mCloth = ScriptBinder.GetObject("Cloth") as UnityEngine.GameObject;
		mWeapon = ScriptBinder.GetObject("Weapon") as UnityEngine.GameObject;
		mlb_dragonLevel = ScriptBinder.GetObject("lb_dragonLevel") as UILabel;
		mmaxState = ScriptBinder.GetObject("maxState") as UnityEngine.GameObject;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		meffect = ScriptBinder.GetObject("effect") as UnityEngine.GameObject;
		matt = ScriptBinder.GetObject("att") as UnityEngine.GameObject;
		mphydef = ScriptBinder.GetObject("phydef") as UnityEngine.GameObject;
		mmagicdef = ScriptBinder.GetObject("magicdef") as UnityEngine.GameObject;
		mhp = ScriptBinder.GetObject("hp") as UnityEngine.GameObject;
	}
}
