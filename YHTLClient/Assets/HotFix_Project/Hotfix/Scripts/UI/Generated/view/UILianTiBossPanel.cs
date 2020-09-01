public partial class UILianTiBossPanel : UIBasePanel
{
	protected UnityEngine.GameObject mtex_bg;
	protected UIScrollView mscr_defeat;
	protected UIGridContainer mgrid_defeat;
	protected UnityEngine.GameObject mbtn_defeat;
	protected UnityEngine.GameObject mbtn_preview;
	protected UILabel mlb_bossName;
	protected UISprite mlb_bossSprite;
	protected UILabel mlb_bossLv;
	protected UILabel mlb_refreshTime;
	protected UIScrollView mscr_rewardScr;
	protected UIGrid mgrid_rewards;
	protected UIScrollView mscr_preview;
	protected UIGridContainer mgrid_preview;
	protected UIScrollView mscr_mapbtn;
	protected UIGridContainer mgrid_mapbtns;
	protected UnityEngine.GameObject mobj_noBoss;
	protected UnityEngine.GameObject mtex_noBossbg;
	protected UnityEngine.GameObject mobj_right;
	protected CSInvoke mCSInvoke;
	protected override void _InitScriptBinder()
	{
		mtex_bg = ScriptBinder.GetObject("tex_bg") as UnityEngine.GameObject;
		mscr_defeat = ScriptBinder.GetObject("scr_defeat") as UIScrollView;
		mgrid_defeat = ScriptBinder.GetObject("grid_defeat") as UIGridContainer;
		mbtn_defeat = ScriptBinder.GetObject("btn_defeat") as UnityEngine.GameObject;
		mbtn_preview = ScriptBinder.GetObject("btn_preview") as UnityEngine.GameObject;
		mlb_bossName = ScriptBinder.GetObject("lb_bossName") as UILabel;
		mlb_bossSprite = ScriptBinder.GetObject("lb_bossSprite") as UISprite;
		mlb_bossLv = ScriptBinder.GetObject("lb_bossLv") as UILabel;
		mlb_refreshTime = ScriptBinder.GetObject("lb_refreshTime") as UILabel;
		mscr_rewardScr = ScriptBinder.GetObject("scr_rewardScr") as UIScrollView;
		mgrid_rewards = ScriptBinder.GetObject("grid_rewards") as UIGrid;
		mscr_preview = ScriptBinder.GetObject("scr_preview") as UIScrollView;
		mgrid_preview = ScriptBinder.GetObject("grid_preview") as UIGridContainer;
		mscr_mapbtn = ScriptBinder.GetObject("scr_mapbtn") as UIScrollView;
		mgrid_mapbtns = ScriptBinder.GetObject("grid_mapbtns") as UIGridContainer;
		mobj_noBoss = ScriptBinder.GetObject("obj_noBoss") as UnityEngine.GameObject;
		mtex_noBossbg = ScriptBinder.GetObject("tex_noBossbg") as UnityEngine.GameObject;
		mobj_right = ScriptBinder.GetObject("obj_right") as UnityEngine.GameObject;
		mCSInvoke = ScriptBinder.GetObject("CSInvoke") as CSInvoke;
	}
}
