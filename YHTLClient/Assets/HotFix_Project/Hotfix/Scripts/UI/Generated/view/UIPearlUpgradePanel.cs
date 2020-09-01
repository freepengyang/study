public partial class UIPearlUpgradePanel : UIBasePanel
{
	protected UIGridContainer mgrid_preal;
	protected UnityEngine.GameObject mItemBase;
	protected UILabel mlb_count;
	protected UILabel mlb_itemName;
	protected UIEventListener mbtn_rule;
	protected UnityEngine.GameObject mScrollView_effects;
	protected UIGridContainer mgrid_effects;
	protected UISlider mslider_exp;
	protected UISlider mslider_exp_add;
	protected UIEventListener mbtn_select;
	protected UIEventListener mbtn_upgrade;
	protected UILabel mlb_exp;
	protected UnityEngine.GameObject mfullHint;
	protected UnityEngine.GameObject memptyHint;
	protected UIEventListener mlb_hint;
	protected UnityEngine.GameObject mobj_nonEmpty;
	protected UnityEngine.GameObject mnonfullHint;
	protected UILabel mlb_curLevel;
	protected UILabel mlb_nextLevel;
	protected UnityEngine.GameObject mheadbg;
	protected UIScrollView mScrollView_ConsumableEquip;
	protected UIGridContainer mGrid0;
	protected UIGridContainer mGrid1;
	protected UIGridContainer mgrid_dot;
	protected UIWrapContent mwrap_page;
	protected UIEventListener mlb_hintMax;
	protected UIEventListener mlb_hintNoDevours;
	protected override void _InitScriptBinder()
	{
		mgrid_preal = ScriptBinder.GetObject("grid_preal") as UIGridContainer;
		mItemBase = ScriptBinder.GetObject("ItemBase") as UnityEngine.GameObject;
		mlb_count = ScriptBinder.GetObject("lb_count") as UILabel;
		mlb_itemName = ScriptBinder.GetObject("lb_itemName") as UILabel;
		mbtn_rule = ScriptBinder.GetObject("btn_rule") as UIEventListener;
		mScrollView_effects = ScriptBinder.GetObject("ScrollView_effects") as UnityEngine.GameObject;
		mgrid_effects = ScriptBinder.GetObject("grid_effects") as UIGridContainer;
		mslider_exp = ScriptBinder.GetObject("slider_exp") as UISlider;
		mslider_exp_add = ScriptBinder.GetObject("slider_exp_add") as UISlider;
		mbtn_select = ScriptBinder.GetObject("btn_select") as UIEventListener;
		mbtn_upgrade = ScriptBinder.GetObject("btn_upgrade") as UIEventListener;
		mlb_exp = ScriptBinder.GetObject("lb_exp") as UILabel;
		mfullHint = ScriptBinder.GetObject("fullHint") as UnityEngine.GameObject;
		memptyHint = ScriptBinder.GetObject("emptyHint") as UnityEngine.GameObject;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UIEventListener;
		mobj_nonEmpty = ScriptBinder.GetObject("obj_nonEmpty") as UnityEngine.GameObject;
		mnonfullHint = ScriptBinder.GetObject("nonfullHint") as UnityEngine.GameObject;
		mlb_curLevel = ScriptBinder.GetObject("lb_curLevel") as UILabel;
		mlb_nextLevel = ScriptBinder.GetObject("lb_nextLevel") as UILabel;
		mheadbg = ScriptBinder.GetObject("headbg") as UnityEngine.GameObject;
		mScrollView_ConsumableEquip = ScriptBinder.GetObject("ScrollView_ConsumableEquip") as UIScrollView;
		mGrid0 = ScriptBinder.GetObject("Grid0") as UIGridContainer;
		mGrid1 = ScriptBinder.GetObject("Grid1") as UIGridContainer;
		mgrid_dot = ScriptBinder.GetObject("grid_dot") as UIGridContainer;
		mwrap_page = ScriptBinder.GetObject("wrap_page") as UIWrapContent;
		mlb_hintMax = ScriptBinder.GetObject("lb_hintMax") as UIEventListener;
		mlb_hintNoDevours = ScriptBinder.GetObject("lb_hintNoDevours") as UIEventListener;
	}
}
