public partial class UIEquipZhuFuPanel : UIBasePanel
{
	protected UnityEngine.GameObject mitemBase;
	protected UnityEngine.GameObject mbtn_zhufu;
	protected UnityEngine.GameObject mcostItemBase;
	protected UILabel mlb_equip_name;
	protected UILabel mlb_equip_lucky;
	protected UITexture mbtn_bg_close;
	protected UILabel mlb_cost_num;
	protected UITexture mobj_bg;
	protected UnityEngine.GameObject mbtn_help;
	protected UnityEngine.GameObject meffect;
	protected override void _InitScriptBinder()
	{
		mitemBase = ScriptBinder.GetObject("itemBase") as UnityEngine.GameObject;
		mbtn_zhufu = ScriptBinder.GetObject("btn_zhufu") as UnityEngine.GameObject;
		mcostItemBase = ScriptBinder.GetObject("costItemBase") as UnityEngine.GameObject;
		mlb_equip_name = ScriptBinder.GetObject("lb_equip_name") as UILabel;
		mlb_equip_lucky = ScriptBinder.GetObject("lb_equip_lucky") as UILabel;
		mbtn_bg_close = ScriptBinder.GetObject("btn_bg_close") as UITexture;
		mlb_cost_num = ScriptBinder.GetObject("lb_cost_num") as UILabel;
		mobj_bg = ScriptBinder.GetObject("obj_bg") as UITexture;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		meffect = ScriptBinder.GetObject("effect") as UnityEngine.GameObject;
	}
}
