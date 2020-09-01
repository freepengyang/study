public partial class UIRechargeFirstPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_ToggleGroup;
	protected UIGridContainer mgrid_items;
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_recharge;
	protected UnityEngine.GameObject mbtn_draw;
	protected UISprite msp_icon;
	protected UnityEngine.GameObject mobj_Received;
	protected UnityEngine.GameObject mrecharge_label_bg;
	protected UILabel mlb_Day;
	protected UnityEngine.GameObject meffect;
	protected UnityEngine.GameObject meffectbtn;
	protected override void _InitScriptBinder()
	{
		mobj_ToggleGroup = ScriptBinder.GetObject("obj_ToggleGroup") as UnityEngine.GameObject;
		mgrid_items = ScriptBinder.GetObject("grid_items") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_recharge = ScriptBinder.GetObject("btn_recharge") as UnityEngine.GameObject;
		mbtn_draw = ScriptBinder.GetObject("btn_draw") as UnityEngine.GameObject;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mobj_Received = ScriptBinder.GetObject("obj_Received") as UnityEngine.GameObject;
		mrecharge_label_bg = ScriptBinder.GetObject("recharge_label_bg") as UnityEngine.GameObject;
		mlb_Day = ScriptBinder.GetObject("lb_Day") as UILabel;
		meffect = ScriptBinder.GetObject("effect") as UnityEngine.GameObject;
		meffectbtn = ScriptBinder.GetObject("effectbtn") as UnityEngine.GameObject;
	}
}
