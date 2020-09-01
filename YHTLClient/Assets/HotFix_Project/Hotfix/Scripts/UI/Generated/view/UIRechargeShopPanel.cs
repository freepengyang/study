public partial class UIRechargeShopPanel : UIBasePanel
{
	protected UIGridContainer mgrid_Item;
	protected UnityEngine.GameObject mitemBar;
	protected UIDragScrollView mdrag;
	protected UIGridContainer mgrid_pageDot;
	protected UIScrollBar mscrollBar;
	protected UIGridContainer mToggle;
	protected UIScrollView mscroll;
	protected UISprite msp_icon;
	protected UnityEngine.GameObject mbtn_add;
	protected UILabel mlb_value;
	protected UILabel mlb_DayNum;
	protected UnityEngine.GameObject mlb_hint;
	protected override void _InitScriptBinder()
	{
		mgrid_Item = ScriptBinder.GetObject("grid_Item") as UIGridContainer;
		mitemBar = ScriptBinder.GetObject("itemBar") as UnityEngine.GameObject;
		mdrag = ScriptBinder.GetObject("drag") as UIDragScrollView;
		mgrid_pageDot = ScriptBinder.GetObject("grid_pageDot") as UIGridContainer;
		mscrollBar = ScriptBinder.GetObject("scrollBar") as UIScrollBar;
		mToggle = ScriptBinder.GetObject("Toggle") as UIGridContainer;
		mscroll = ScriptBinder.GetObject("scroll") as UIScrollView;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UnityEngine.GameObject;
		mlb_value = ScriptBinder.GetObject("lb_value") as UILabel;
		mlb_DayNum = ScriptBinder.GetObject("lb_DayNum") as UILabel;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UnityEngine.GameObject;
	}
}
