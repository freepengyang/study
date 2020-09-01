public partial class UIActivityHalllTipsPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_mask;
	protected UILabel mlb_des;
	protected UILabel mlb_name;
	protected UILabel mlb_count;
	protected UILabel mlb_active;
	protected UILabel mlb_time;
	protected UILabel mlb_level;
	protected UIGrid mGrid_reward;
	protected UnityEngine.Transform mObj_scrollView;
	protected UISprite mSp_bg;
	protected UnityEngine.Transform mObj_fix;
	protected UIScrollView mScroll_reward;
	protected UnityEngine.Transform mTrans_line;
	protected UISprite mSp_flag;
	protected UISprite mSp_icon;
	protected UIEventListener mBtn_go;
	protected override void _InitScriptBinder()
	{
		mobj_mask = ScriptBinder.GetObject("obj_mask") as UnityEngine.GameObject;
		mlb_des = ScriptBinder.GetObject("lb_des") as UILabel;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mlb_count = ScriptBinder.GetObject("lb_count") as UILabel;
		mlb_active = ScriptBinder.GetObject("lb_active") as UILabel;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_level = ScriptBinder.GetObject("lb_level") as UILabel;
		mGrid_reward = ScriptBinder.GetObject("Grid_reward") as UIGrid;
		mObj_scrollView = ScriptBinder.GetObject("Obj_scrollView") as UnityEngine.Transform;
		mSp_bg = ScriptBinder.GetObject("Sp_bg") as UISprite;
		mObj_fix = ScriptBinder.GetObject("Obj_fix") as UnityEngine.Transform;
		mScroll_reward = ScriptBinder.GetObject("Scroll_reward") as UIScrollView;
		mTrans_line = ScriptBinder.GetObject("Trans_line") as UnityEngine.Transform;
		mSp_flag = ScriptBinder.GetObject("Sp_flag") as UISprite;
		mSp_icon = ScriptBinder.GetObject("Sp_icon") as UISprite;
		mBtn_go = ScriptBinder.GetObject("Btn_go") as UIEventListener;
	}
}
