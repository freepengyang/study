public partial class UISpecialShopCombinePanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIToggle mbtn_rechargeShop;
	protected UIToggle mbtn_exchangeShop;
	protected UnityEngine.GameObject mURechargeShopPanel;
	protected UnityEngine.GameObject mUIExchangeShopPanel;
	protected UIToggle mbtn_recharge;
	protected UnityEngine.GameObject mUIRechargePanel;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_rechargeShop = ScriptBinder.GetObject("btn_rechargeShop") as UIToggle;
		mbtn_exchangeShop = ScriptBinder.GetObject("btn_exchangeShop") as UIToggle;
		mURechargeShopPanel = ScriptBinder.GetObject("URechargeShopPanel") as UnityEngine.GameObject;
		mUIExchangeShopPanel = ScriptBinder.GetObject("UIExchangeShopPanel") as UnityEngine.GameObject;
		mbtn_recharge = ScriptBinder.GetObject("btn_recharge") as UIToggle;
		mUIRechargePanel = ScriptBinder.GetObject("UIRechargePanel") as UnityEngine.GameObject;
	}
}
