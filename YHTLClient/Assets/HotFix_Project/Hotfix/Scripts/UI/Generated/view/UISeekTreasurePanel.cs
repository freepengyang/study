public partial class UISeekTreasurePanel : UIBasePanel
{
	protected UILabel mlb_props;
	protected UILabel mlb_integral;
	protected UIGridContainer mgrid_awardList1;
	protected UIGridContainer mgrid_awardList2;
	protected UnityEngine.GameObject mawardMiddle;
	protected UIGridContainer mgrid_myrecord;
	protected UIGridContainer mgrid_allrecord;
	protected UnityEngine.GameObject meffect_normal;
	protected UnityEngine.GameObject meffect_explore;
	protected UnityEngine.GameObject mtexture2;
	protected UIEventListener mbtn_seek1;
	protected UIEventListener mbtn_seek10;
	protected UIEventListener mbtn_seek20;
	protected UIEventListener mbtn_all;
	protected UIEventListener mbtn_me;
	protected UIScrollView mScrollViewMyrecord;
	protected UIScrollView mScrollViewAllrecord;
	protected UnityEngine.GameObject mtreasure_bg2;
	protected UnityEngine.GameObject mtreasure_line;
	protected UnityEngine.GameObject meffect_seek_treasure_idle_add;
	protected UnityEngine.GameObject meffect_seek_treasure_point_add;
	protected UISprite msp_propsIcon;
	protected UIEventListener mbtn_gift;
	protected UIEventListener mbtn_propsIcon;
	protected UIEventListener mbtn_add_cost;
	protected UnityEngine.GameObject mButtons;
	protected UnityEngine.GameObject mHints;
	protected UILabel mlb_starTime;
	protected UnityEngine.GameObject mGift;
	protected UIEventListener mbtn_close_bubble;
	protected UnityEngine.GameObject mbubble;
	protected override void _InitScriptBinder()
	{
		mlb_props = ScriptBinder.GetObject("lb_props") as UILabel;
		mlb_integral = ScriptBinder.GetObject("lb_integral") as UILabel;
		mgrid_awardList1 = ScriptBinder.GetObject("grid_awardList1") as UIGridContainer;
		mgrid_awardList2 = ScriptBinder.GetObject("grid_awardList2") as UIGridContainer;
		mawardMiddle = ScriptBinder.GetObject("awardMiddle") as UnityEngine.GameObject;
		mgrid_myrecord = ScriptBinder.GetObject("grid_myrecord") as UIGridContainer;
		mgrid_allrecord = ScriptBinder.GetObject("grid_allrecord") as UIGridContainer;
		meffect_normal = ScriptBinder.GetObject("effect_normal") as UnityEngine.GameObject;
		meffect_explore = ScriptBinder.GetObject("effect_explore") as UnityEngine.GameObject;
		mtexture2 = ScriptBinder.GetObject("texture2") as UnityEngine.GameObject;
		mbtn_seek1 = ScriptBinder.GetObject("btn_seek1") as UIEventListener;
		mbtn_seek10 = ScriptBinder.GetObject("btn_seek10") as UIEventListener;
		mbtn_seek20 = ScriptBinder.GetObject("btn_seek20") as UIEventListener;
		mbtn_all = ScriptBinder.GetObject("btn_all") as UIEventListener;
		mbtn_me = ScriptBinder.GetObject("btn_me") as UIEventListener;
		mScrollViewMyrecord = ScriptBinder.GetObject("ScrollViewMyrecord") as UIScrollView;
		mScrollViewAllrecord = ScriptBinder.GetObject("ScrollViewAllrecord") as UIScrollView;
		mtreasure_bg2 = ScriptBinder.GetObject("treasure_bg2") as UnityEngine.GameObject;
		mtreasure_line = ScriptBinder.GetObject("treasure_line") as UnityEngine.GameObject;
		meffect_seek_treasure_idle_add = ScriptBinder.GetObject("effect_seek_treasure_idle_add") as UnityEngine.GameObject;
		meffect_seek_treasure_point_add = ScriptBinder.GetObject("effect_seek_treasure_point_add") as UnityEngine.GameObject;
		msp_propsIcon = ScriptBinder.GetObject("sp_propsIcon") as UISprite;
		mbtn_gift = ScriptBinder.GetObject("btn_gift") as UIEventListener;
		mbtn_propsIcon = ScriptBinder.GetObject("btn_propsIcon") as UIEventListener;
		mbtn_add_cost = ScriptBinder.GetObject("btn_add_cost") as UIEventListener;
		mButtons = ScriptBinder.GetObject("Buttons") as UnityEngine.GameObject;
		mHints = ScriptBinder.GetObject("Hints") as UnityEngine.GameObject;
		mlb_starTime = ScriptBinder.GetObject("lb_starTime") as UILabel;
		mGift = ScriptBinder.GetObject("Gift") as UnityEngine.GameObject;
		mbtn_close_bubble = ScriptBinder.GetObject("btn_close_bubble") as UIEventListener;
		mbubble = ScriptBinder.GetObject("bubble") as UnityEngine.GameObject;
	}
}
