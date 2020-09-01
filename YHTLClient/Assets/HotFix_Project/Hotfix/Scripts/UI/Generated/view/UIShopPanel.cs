public partial class UIShopPanel : UIBasePanel
{
	protected UIToggle mtg_hot;
	protected UIToggle mtg_enhance;
	protected UIToggle mtg_common;
	protected UIToggle mtg_limitation;
	protected UIGridContainer mgrid_Page;
	protected UISprite msp_moneyicon1;
	protected UISprite msp_moneyicon2;
	protected UISprite msp_moneyicon3;
	protected UILabel mlb_moneyvalue1;
	protected UILabel mlb_moneyvalue2;
	protected UILabel mlb_moneyvalue3;
	protected UnityEngine.GameObject mbtn_moneyadd1;
	protected UnityEngine.GameObject mbtn_moneyadd2;
	protected UnityEngine.GameObject mbtn_moneyadd3;
	protected UIGridContainer mgrid_pageDot;
	protected UIScrollView mscroll;
	protected UIScrollBar mscrollBar;
	protected override void _InitScriptBinder()
	{
		mtg_hot = ScriptBinder.GetObject("tg_hot") as UIToggle;
		mtg_enhance = ScriptBinder.GetObject("tg_enhance") as UIToggle;
		mtg_common = ScriptBinder.GetObject("tg_common") as UIToggle;
		mtg_limitation = ScriptBinder.GetObject("tg_limitation") as UIToggle;
		mgrid_Page = ScriptBinder.GetObject("grid_Page") as UIGridContainer;
		msp_moneyicon1 = ScriptBinder.GetObject("sp_moneyicon1") as UISprite;
		msp_moneyicon2 = ScriptBinder.GetObject("sp_moneyicon2") as UISprite;
		msp_moneyicon3 = ScriptBinder.GetObject("sp_moneyicon3") as UISprite;
		mlb_moneyvalue1 = ScriptBinder.GetObject("lb_moneyvalue1") as UILabel;
		mlb_moneyvalue2 = ScriptBinder.GetObject("lb_moneyvalue2") as UILabel;
		mlb_moneyvalue3 = ScriptBinder.GetObject("lb_moneyvalue3") as UILabel;
		mbtn_moneyadd1 = ScriptBinder.GetObject("btn_moneyadd1") as UnityEngine.GameObject;
		mbtn_moneyadd2 = ScriptBinder.GetObject("btn_moneyadd2") as UnityEngine.GameObject;
		mbtn_moneyadd3 = ScriptBinder.GetObject("btn_moneyadd3") as UnityEngine.GameObject;
		mgrid_pageDot = ScriptBinder.GetObject("grid_pageDot") as UIGridContainer;
		mscroll = ScriptBinder.GetObject("scroll") as UIScrollView;
		mscrollBar = ScriptBinder.GetObject("scrollBar") as UIScrollBar;
	}
}
