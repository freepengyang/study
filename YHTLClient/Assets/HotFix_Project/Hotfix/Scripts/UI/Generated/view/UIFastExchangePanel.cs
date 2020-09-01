public partial class UIFastExchangePanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UILabel mlb_itemNum;
	protected UILabel mlb_itemName;
	protected UIInput minput_num;
	protected UILabel mlb_hint1;
	protected UILabel mlb_hint2;
	protected UIEventListener mbtn_add;
	protected UIEventListener mbtn_sub;
	protected UIEventListener mbtn_exchange;
	protected UIGridContainer mgrid_bottom;
	protected UISprite msp_bg;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mlb_itemNum = ScriptBinder.GetObject("lb_itemNum") as UILabel;
		mlb_itemName = ScriptBinder.GetObject("lb_itemName") as UILabel;
		minput_num = ScriptBinder.GetObject("input_num") as UIInput;
		mlb_hint1 = ScriptBinder.GetObject("lb_hint1") as UILabel;
		mlb_hint2 = ScriptBinder.GetObject("lb_hint2") as UILabel;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UIEventListener;
		mbtn_sub = ScriptBinder.GetObject("btn_sub") as UIEventListener;
		mbtn_exchange = ScriptBinder.GetObject("btn_exchange") as UIEventListener;
		mgrid_bottom = ScriptBinder.GetObject("grid_bottom") as UIGridContainer;
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
	}
}
