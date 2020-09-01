public partial class UIStarSuitTipPanel : UIBasePanel
{
	protected UIEventListener mBtn_close;
	protected UISprite mSp_bg;
	protected UILabel mlb_curMaintitle;
	protected UILabel mlb_curSubtitle;
	protected UIGridContainer mgrid_cur;
	protected UILabel mlb_timeCountDown;
	protected UnityEngine.GameObject mobj_nextInfo;
	protected UILabel mlb_nextMaintitle;
	protected UILabel mlb_nextSubtitle;
	protected UIGridContainer mgrid_next;
	protected UnityEngine.GameObject mobj_curInfo;
	protected UnityEngine.GameObject mobj_line;
	protected UILabel mlb_hint1;
	protected UILabel mlb_hint2;
	protected override void _InitScriptBinder()
	{
		mBtn_close = ScriptBinder.GetObject("Btn_close") as UIEventListener;
		mSp_bg = ScriptBinder.GetObject("Sp_bg") as UISprite;
		mlb_curMaintitle = ScriptBinder.GetObject("lb_curMaintitle") as UILabel;
		mlb_curSubtitle = ScriptBinder.GetObject("lb_curSubtitle") as UILabel;
		mgrid_cur = ScriptBinder.GetObject("grid_cur") as UIGridContainer;
		mlb_timeCountDown = ScriptBinder.GetObject("lb_timeCountDown") as UILabel;
		mobj_nextInfo = ScriptBinder.GetObject("obj_nextInfo") as UnityEngine.GameObject;
		mlb_nextMaintitle = ScriptBinder.GetObject("lb_nextMaintitle") as UILabel;
		mlb_nextSubtitle = ScriptBinder.GetObject("lb_nextSubtitle") as UILabel;
		mgrid_next = ScriptBinder.GetObject("grid_next") as UIGridContainer;
		mobj_curInfo = ScriptBinder.GetObject("obj_curInfo") as UnityEngine.GameObject;
		mobj_line = ScriptBinder.GetObject("obj_line") as UnityEngine.GameObject;
		mlb_hint1 = ScriptBinder.GetObject("lb_hint1") as UILabel;
		mlb_hint2 = ScriptBinder.GetObject("lb_hint2") as UILabel;
	}
}
