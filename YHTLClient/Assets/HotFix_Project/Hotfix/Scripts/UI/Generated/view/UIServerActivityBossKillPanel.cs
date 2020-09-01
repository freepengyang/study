public partial class UIServerActivityBossKillPanel : UIBasePanel
{
	protected UILabel mlb_countdown;
	protected UnityEngine.GameObject mobj_leftArrow;
	protected UnityEngine.GameObject mobj_rightArrow;
	protected UIScrollView mscr_bossItem;
	protected UIGridContainer mgrid_bossItem;
	protected UnityEngine.GameObject mscr_serverReward;
	protected UIGrid mgrid_serverReward;
	protected UnityEngine.GameObject mscr_personalReward;
	protected UIGrid mgrid_personalReward;
	protected UISprite mbtn_server;
	protected UILabel mlb_server;
	protected UISprite mbtn_personal;
	protected UILabel mlb_personal;
	protected UnityEngine.GameObject mtex_bgTex;
	protected UILabel mlb_serverDes;
	protected UILabel mlb_personalDes;
	protected UnityEngine.GameObject mobj_personalLingqu;
	protected override void _InitScriptBinder()
	{
		mlb_countdown = ScriptBinder.GetObject("lb_countdown") as UILabel;
		mobj_leftArrow = ScriptBinder.GetObject("obj_leftArrow") as UnityEngine.GameObject;
		mobj_rightArrow = ScriptBinder.GetObject("obj_rightArrow") as UnityEngine.GameObject;
		mscr_bossItem = ScriptBinder.GetObject("scr_bossItem") as UIScrollView;
		mgrid_bossItem = ScriptBinder.GetObject("grid_bossItem") as UIGridContainer;
		mscr_serverReward = ScriptBinder.GetObject("scr_serverReward") as UnityEngine.GameObject;
		mgrid_serverReward = ScriptBinder.GetObject("grid_serverReward") as UIGrid;
		mscr_personalReward = ScriptBinder.GetObject("scr_personalReward") as UnityEngine.GameObject;
		mgrid_personalReward = ScriptBinder.GetObject("grid_personalReward") as UIGrid;
		mbtn_server = ScriptBinder.GetObject("btn_server") as UISprite;
		mlb_server = ScriptBinder.GetObject("lb_server") as UILabel;
		mbtn_personal = ScriptBinder.GetObject("btn_personal") as UISprite;
		mlb_personal = ScriptBinder.GetObject("lb_personal") as UILabel;
		mtex_bgTex = ScriptBinder.GetObject("tex_bgTex") as UnityEngine.GameObject;
		mlb_serverDes = ScriptBinder.GetObject("lb_serverDes") as UILabel;
		mlb_personalDes = ScriptBinder.GetObject("lb_personalDes") as UILabel;
		mobj_personalLingqu = ScriptBinder.GetObject("obj_personalLingqu") as UnityEngine.GameObject;
	}
}
