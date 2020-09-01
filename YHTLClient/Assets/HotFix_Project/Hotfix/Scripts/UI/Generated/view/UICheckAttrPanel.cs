public partial class UICheckAttrPanel : UIBasePanel
{
	protected UILabel mrolename;
	protected UnityEngine.GameObject mequips;
	protected UnityEngine.GameObject mRole;
	protected UnityEngine.GameObject mCloth;
	protected UnityEngine.GameObject mWeapon;
	protected UILabel mlb_rolename;
	protected UILabel mlb_rolelevel;
	protected UIEventListener mbtn_ToWoLong;
	protected UISlider mslier_hp;
	protected UILabel mlb_hp;
	protected UISlider mslier_mp;
	protected UILabel mlb_mp;
	protected UIScrollBar mbar;
	protected UIGridContainer mgrid_attr;
	protected UnityEngine.GameObject marrow;
	protected UILabel mlab_fight;
	protected override void _InitScriptBinder()
	{
		mrolename = ScriptBinder.GetObject("rolename") as UILabel;
		mequips = ScriptBinder.GetObject("equips") as UnityEngine.GameObject;
		mRole = ScriptBinder.GetObject("Role") as UnityEngine.GameObject;
		mCloth = ScriptBinder.GetObject("Cloth") as UnityEngine.GameObject;
		mWeapon = ScriptBinder.GetObject("Weapon") as UnityEngine.GameObject;
		mlb_rolename = ScriptBinder.GetObject("lb_rolename") as UILabel;
		mlb_rolelevel = ScriptBinder.GetObject("lb_rolelevel") as UILabel;
		mbtn_ToWoLong = ScriptBinder.GetObject("btn_ToWoLong") as UIEventListener;
		mslier_hp = ScriptBinder.GetObject("slier_hp") as UISlider;
		mlb_hp = ScriptBinder.GetObject("lb_hp") as UILabel;
		mslier_mp = ScriptBinder.GetObject("slier_mp") as UISlider;
		mlb_mp = ScriptBinder.GetObject("lb_mp") as UILabel;
		mbar = ScriptBinder.GetObject("bar") as UIScrollBar;
		mgrid_attr = ScriptBinder.GetObject("grid_attr") as UIGridContainer;
		marrow = ScriptBinder.GetObject("arrow") as UnityEngine.GameObject;
		mlab_fight = ScriptBinder.GetObject("lab_fight") as UILabel;
	}
}
