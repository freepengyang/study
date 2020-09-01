public partial class UIPearlEvolutionPanel : UIBasePanel
{
	protected UIEventListener mbtn_rule;
	protected UnityEngine.GameObject mheadbg;
	protected UnityEngine.GameObject memptyHint;
	protected UnityEngine.GameObject mobj_nonEmpty;
	protected UnityEngine.GameObject mItemBase;
	protected UILabel mlb_itemName;
	protected UILabel mlb_title;
	protected UIGridContainer mgrid_effects;
	protected UnityEngine.GameObject mfullHint;
	protected UnityEngine.GameObject mnonfullHint;
	protected UISlider mslider_exp;
	protected UILabel mlb_condition;
	protected UIEventListener mbtn_gotokill  ;
	protected UIEventListener mbtn_evolution;
	protected UIGridContainer mgrid_preal;
	protected UILabel mlb_exp;
	protected UILabel mlb_count;
	protected UIEventListener mlb_hint;
	protected override void _InitScriptBinder()
	{
		mbtn_rule = ScriptBinder.GetObject("btn_rule") as UIEventListener;
		mheadbg = ScriptBinder.GetObject("headbg") as UnityEngine.GameObject;
		memptyHint = ScriptBinder.GetObject("emptyHint") as UnityEngine.GameObject;
		mobj_nonEmpty = ScriptBinder.GetObject("obj_nonEmpty") as UnityEngine.GameObject;
		mItemBase = ScriptBinder.GetObject("ItemBase") as UnityEngine.GameObject;
		mlb_itemName = ScriptBinder.GetObject("lb_itemName") as UILabel;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mgrid_effects = ScriptBinder.GetObject("grid_effects") as UIGridContainer;
		mfullHint = ScriptBinder.GetObject("fullHint") as UnityEngine.GameObject;
		mnonfullHint = ScriptBinder.GetObject("nonfullHint") as UnityEngine.GameObject;
		mslider_exp = ScriptBinder.GetObject("slider_exp") as UISlider;
		mlb_condition = ScriptBinder.GetObject("lb_condition") as UILabel;
		mbtn_gotokill   = ScriptBinder.GetObject("btn_gotokill  ") as UIEventListener;
		mbtn_evolution = ScriptBinder.GetObject("btn_evolution") as UIEventListener;
		mgrid_preal = ScriptBinder.GetObject("grid_preal") as UIGridContainer;
		mlb_exp = ScriptBinder.GetObject("lb_exp") as UILabel;
		mlb_count = ScriptBinder.GetObject("lb_count") as UILabel;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UIEventListener;
	}
}
