public partial class UIShopCombinePanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIToggle mbtn_shop;
	protected UIToggle mbtn_recharge;
	protected UnityEngine.GameObject mobj_shopPanel;
	protected UnityEngine.GameObject mobj_rechargePanel;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_shop = ScriptBinder.GetObject("btn_shop") as UIToggle;
		mbtn_recharge = ScriptBinder.GetObject("btn_recharge") as UIToggle;
		mobj_shopPanel = ScriptBinder.GetObject("obj_shopPanel") as UnityEngine.GameObject;
		mobj_rechargePanel = ScriptBinder.GetObject("obj_rechargePanel") as UnityEngine.GameObject;
	}
}
