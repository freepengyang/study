public partial class UIGuildInfoPanel : UIBasePanel
{
	protected UIEventListener mbtn_donate;
	protected UIEventListener mbtn_rename;
	protected UIEventListener mbtn_help;
	protected UILabel mlb_name;
	protected UILabel mlb_level;
	protected UILabel mlb_president;
	protected UILabel mlb_cur_money;
	protected UILabel mlb_member_count;
	protected UILabel mlb_not_enough;
	protected UILabel mlb_unionName;
	protected UISlider mSlider;
	protected UILabel mlb_progress;
	protected UIInput mannouncement;
	protected UIEventListener mbtn_submit;
	protected UIDragScrollView mDragScrollView;
	protected UnityEngine.BoxCollider mcolider;
	protected UIGridContainer mGrid;
	protected UIScrollView mScrollView;
	protected UnityEngine.GameObject mGuildIcon;
	protected UnityEngine.GameObject mtexBg;
	protected UILabel mlb_yuanbao;
	protected UIEventListener mbtn_devide;
	protected override void _InitScriptBinder()
	{
		mbtn_donate = ScriptBinder.GetObject("btn_donate") as UIEventListener;
		mbtn_rename = ScriptBinder.GetObject("btn_rename") as UIEventListener;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mlb_level = ScriptBinder.GetObject("lb_level") as UILabel;
		mlb_president = ScriptBinder.GetObject("lb_president") as UILabel;
		mlb_cur_money = ScriptBinder.GetObject("lb_cur_money") as UILabel;
		mlb_member_count = ScriptBinder.GetObject("lb_member_count") as UILabel;
		mlb_not_enough = ScriptBinder.GetObject("lb_not_enough") as UILabel;
		mlb_unionName = ScriptBinder.GetObject("lb_unionName") as UILabel;
		mSlider = ScriptBinder.GetObject("Slider") as UISlider;
		mlb_progress = ScriptBinder.GetObject("lb_progress") as UILabel;
		mannouncement = ScriptBinder.GetObject("announcement") as UIInput;
		mbtn_submit = ScriptBinder.GetObject("btn_submit") as UIEventListener;
		mDragScrollView = ScriptBinder.GetObject("DragScrollView") as UIDragScrollView;
		mcolider = ScriptBinder.GetObject("colider") as UnityEngine.BoxCollider;
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mGuildIcon = ScriptBinder.GetObject("GuildIcon") as UnityEngine.GameObject;
		mtexBg = ScriptBinder.GetObject("texBg") as UnityEngine.GameObject;
		mlb_yuanbao = ScriptBinder.GetObject("lb_yuanbao") as UILabel;
		mbtn_devide = ScriptBinder.GetObject("btn_devide") as UIEventListener;
	}
}
