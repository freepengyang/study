public partial class UISeekTreasureCombinedPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_bg;
	protected UIToggle mbtn_seektreasure;
	protected UIToggle mbtn_stroehouse;
	protected UIToggle mbtn_exchange;
	protected UnityEngine.GameObject mUISeekTreasurePanel;
	protected UnityEngine.GameObject mUISeekTreasureWarehousePanel;
	protected UnityEngine.GameObject mUISeekTreasureExchangePanel;
	protected UnityEngine.GameObject mredpoint_stroehouse;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_bg = ScriptBinder.GetObject("btn_bg") as UIEventListener;
		mbtn_seektreasure = ScriptBinder.GetObject("btn_seektreasure") as UIToggle;
		mbtn_stroehouse = ScriptBinder.GetObject("btn_stroehouse") as UIToggle;
		mbtn_exchange = ScriptBinder.GetObject("btn_exchange") as UIToggle;
		mUISeekTreasurePanel = ScriptBinder.GetObject("UISeekTreasurePanel") as UnityEngine.GameObject;
		mUISeekTreasureWarehousePanel = ScriptBinder.GetObject("UISeekTreasureWarehousePanel") as UnityEngine.GameObject;
		mUISeekTreasureExchangePanel = ScriptBinder.GetObject("UISeekTreasureExchangePanel") as UnityEngine.GameObject;
		mredpoint_stroehouse = ScriptBinder.GetObject("redpoint_stroehouse") as UnityEngine.GameObject;
	}
}
