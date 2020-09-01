public partial class UIWildEscapadePanel : UIBasePanel
{
	protected UnityEngine.GameObject mtex_wildBg;
	protected UnityEngine.GameObject mobj_Empty;
	protected UIEventListener mbtn_getEquip;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_reward;
	protected UILabel mlb_time;
	protected UILabel mlb_earingA;
	protected UILabel mlb_earingB;
	protected UnityEngine.GameObject mobj_Award;
	protected UISprite msp_moneyIcon;
	protected UnityEngine.GameObject mobj_monster;
	protected UnityEngine.GameObject mobj_pet;
	protected UnityEngine.GameObject mitemTemplate;
	protected UnityEngine.GameObject mobj_player;
	protected UnityEngine.GameObject mobj_full;
	protected override void _InitScriptBinder()
	{
		mtex_wildBg = ScriptBinder.GetObject("tex_wildBg") as UnityEngine.GameObject;
		mobj_Empty = ScriptBinder.GetObject("obj_Empty") as UnityEngine.GameObject;
		mbtn_getEquip = ScriptBinder.GetObject("btn_getEquip") as UIEventListener;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_reward = ScriptBinder.GetObject("btn_reward") as UIEventListener;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_earingA = ScriptBinder.GetObject("lb_earingA") as UILabel;
		mlb_earingB = ScriptBinder.GetObject("lb_earingB") as UILabel;
		mobj_Award = ScriptBinder.GetObject("obj_Award") as UnityEngine.GameObject;
		msp_moneyIcon = ScriptBinder.GetObject("sp_moneyIcon") as UISprite;
		mobj_monster = ScriptBinder.GetObject("obj_monster") as UnityEngine.GameObject;
		mobj_pet = ScriptBinder.GetObject("obj_pet") as UnityEngine.GameObject;
		mitemTemplate = ScriptBinder.GetObject("itemTemplate") as UnityEngine.GameObject;
		mobj_player = ScriptBinder.GetObject("obj_player") as UnityEngine.GameObject;
		mobj_full = ScriptBinder.GetObject("obj_full") as UnityEngine.GameObject;
	}
}
