public partial class UIPetTalentPanel : UIBasePanel
{
	protected UIEventListener mbtn_help;
	protected UIEventListener mbtn_talentPreview;
	protected UIScrollView mscroll_right;
	protected UnityEngine.GameObject msp_scrollArrow;
	protected UIGridContainer mGrid_right;
	protected UILabel mlb_title;
	protected UILabel mlb_nextBig;
	protected UILabel mlb_nextBigDes;
	protected UnityEngine.GameObject mobj_abBg1;
	protected UnityEngine.GameObject mobj_abBg2;
	protected UIGridContainer mGrid_left;
	protected UILabel mlb_hint;
	protected UILabel mlb_max;
	protected override void _InitScriptBinder()
	{
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		mbtn_talentPreview = ScriptBinder.GetObject("btn_talentPreview") as UIEventListener;
		mscroll_right = ScriptBinder.GetObject("scroll_right") as UIScrollView;
		msp_scrollArrow = ScriptBinder.GetObject("sp_scrollArrow") as UnityEngine.GameObject;
		mGrid_right = ScriptBinder.GetObject("Grid_right") as UIGridContainer;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mlb_nextBig = ScriptBinder.GetObject("lb_nextBig") as UILabel;
		mlb_nextBigDes = ScriptBinder.GetObject("lb_nextBigDes") as UILabel;
		mobj_abBg1 = ScriptBinder.GetObject("obj_abBg1") as UnityEngine.GameObject;
		mobj_abBg2 = ScriptBinder.GetObject("obj_abBg2") as UnityEngine.GameObject;
		mGrid_left = ScriptBinder.GetObject("Grid_left") as UIGridContainer;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
		mlb_max = ScriptBinder.GetObject("lb_max") as UILabel;
	}
}
