public partial class UIPetLevelUpPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_recycle;
	protected UnityEngine.GameObject mbtn_quesition;
	protected UnityEngine.GameObject mbtn_config;
	protected UnityEngine.GameObject mbtn_benyuan;
	protected UnityEngine.GameObject mbtn_wolong;
	protected UISlider mslider_level;
	protected UISlider mslider_sublevel;
	protected UIGridContainer mgrild_mastitems;
	protected UnityEngine.GameObject msp_scrolldowm;
	protected UIGridContainer mgrid_luckItems;
	protected UIWrapContent mwrap_page;
	protected UnityEngine.GameObject msp_benyuansel;
	protected UnityEngine.GameObject msp_wolongsel;
	protected UIScrollView mScrollView_ConsumableEquip;
	protected UILabel mlb_page;
	protected UILabel mlb_level;
	protected UILabel mlb_exp;
	protected UnityEngine.GameObject mred_normal;
	protected UnityEngine.GameObject mred_wolong;
	protected UnityEngine.GameObject mred_recyle;
	protected UnityEngine.GameObject memptyHint;
	protected UIGridContainer mgrid_getway;
	protected UILabel mlb_hint;
	protected UIScrollView mscrollview_luckItems;
	protected UIScrollBar mscollbar;
	protected UnityEngine.GameObject mlb_explanation;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_recycle = ScriptBinder.GetObject("btn_recycle") as UnityEngine.GameObject;
		mbtn_quesition = ScriptBinder.GetObject("btn_quesition") as UnityEngine.GameObject;
		mbtn_config = ScriptBinder.GetObject("btn_config") as UnityEngine.GameObject;
		mbtn_benyuan = ScriptBinder.GetObject("btn_benyuan") as UnityEngine.GameObject;
		mbtn_wolong = ScriptBinder.GetObject("btn_wolong") as UnityEngine.GameObject;
		mslider_level = ScriptBinder.GetObject("slider_level") as UISlider;
		mslider_sublevel = ScriptBinder.GetObject("slider_sublevel") as UISlider;
		mgrild_mastitems = ScriptBinder.GetObject("grild_mastitems") as UIGridContainer;
		msp_scrolldowm = ScriptBinder.GetObject("sp_scrolldowm") as UnityEngine.GameObject;
		mgrid_luckItems = ScriptBinder.GetObject("grid_luckItems") as UIGridContainer;
		mwrap_page = ScriptBinder.GetObject("wrap_page") as UIWrapContent;
		msp_benyuansel = ScriptBinder.GetObject("sp_benyuansel") as UnityEngine.GameObject;
		msp_wolongsel = ScriptBinder.GetObject("sp_wolongsel") as UnityEngine.GameObject;
		mScrollView_ConsumableEquip = ScriptBinder.GetObject("ScrollView_ConsumableEquip") as UIScrollView;
		mlb_page = ScriptBinder.GetObject("lb_page") as UILabel;
		mlb_level = ScriptBinder.GetObject("lb_level") as UILabel;
		mlb_exp = ScriptBinder.GetObject("lb_exp") as UILabel;
		mred_normal = ScriptBinder.GetObject("red_normal") as UnityEngine.GameObject;
		mred_wolong = ScriptBinder.GetObject("red_wolong") as UnityEngine.GameObject;
		mred_recyle = ScriptBinder.GetObject("red_recyle") as UnityEngine.GameObject;
		memptyHint = ScriptBinder.GetObject("emptyHint") as UnityEngine.GameObject;
		mgrid_getway = ScriptBinder.GetObject("grid_getway") as UIGridContainer;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
		mscrollview_luckItems = ScriptBinder.GetObject("scrollview_luckItems") as UIScrollView;
		mscollbar = ScriptBinder.GetObject("scollbar") as UIScrollBar;
		mlb_explanation = ScriptBinder.GetObject("lb_explanation") as UnityEngine.GameObject;
	}
}
