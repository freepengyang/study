public partial class UILifeTimeFundPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_help;
	protected UnityEngine.GameObject mbtn_buy;
	protected UIGridContainer mgrid_task;
	protected UILabel mlb_point;
	protected UIWrapContent mwrap_items;
	protected UIScrollView mscrollview_item;
	protected UIGridContainer mgrid_reward;
	protected UILabel mlb_money;
	protected UISprite mSprite;
	protected UnityEngine.GameObject mbtn_scrollv;
	protected UnityEngine.GameObject mbanner2;
	protected UnityEngine.GameObject mbtn_scrollh;
	protected UIScrollView mscrollview_task;
	protected UnityEngine.GameObject mbtnbuy_red;
	protected UIScrollView mscrollview_reward;
	protected UISlider mslider_exp;
	protected UISprite mobj_buyeffect;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UnityEngine.GameObject;
		mbtn_buy = ScriptBinder.GetObject("btn_buy") as UnityEngine.GameObject;
		mgrid_task = ScriptBinder.GetObject("grid_task") as UIGridContainer;
		mlb_point = ScriptBinder.GetObject("lb_point") as UILabel;
		mwrap_items = ScriptBinder.GetObject("wrap_items") as UIWrapContent;
		mscrollview_item = ScriptBinder.GetObject("scrollview_item") as UIScrollView;
		mgrid_reward = ScriptBinder.GetObject("grid_reward") as UIGridContainer;
		mlb_money = ScriptBinder.GetObject("lb_money") as UILabel;
		mSprite = ScriptBinder.GetObject("Sprite") as UISprite;
		mbtn_scrollv = ScriptBinder.GetObject("btn_scrollv") as UnityEngine.GameObject;
		mbanner2 = ScriptBinder.GetObject("banner2") as UnityEngine.GameObject;
		mbtn_scrollh = ScriptBinder.GetObject("btn_scrollh") as UnityEngine.GameObject;
		mscrollview_task = ScriptBinder.GetObject("scrollview_task") as UIScrollView;
		mbtnbuy_red = ScriptBinder.GetObject("btnbuy_red") as UnityEngine.GameObject;
		mscrollview_reward = ScriptBinder.GetObject("scrollview_reward") as UIScrollView;
		mslider_exp = ScriptBinder.GetObject("slider_exp") as UISlider;
		mobj_buyeffect = ScriptBinder.GetObject("obj_buyeffect") as UISprite;
	}
}
