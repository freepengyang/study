public partial class UIRoleSelectionInfoPanel : UIBasePanel
{
	protected UIEventListener mbtn_detailed_info;
	protected UISprite msp_role_head;
	protected UILabel mlb_role_name;
	protected UILabel mlb_role_level;
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mUISelectionMenuPanel;
	protected UISprite mbginfo;
	protected UILabel mlb_mp;
	protected UILabel mlb_hp;
	protected UISlider mslider_hp;
	protected UISlider mslider_mp;
	protected UnityEngine.GameObject meffectHp;
	protected UnityEngine.GameObject meffectMp;
	protected UIGridContainer mgrid_dropEquip;
	protected UnityEngine.GameObject mUIBuffPanel;
	protected UISprite msp_bg;
	protected UIScrollView mScrollView_buff;
	protected UnityEngine.GameObject mobj_nonbuff;
	protected UIGridContainer mgrid_buff;
	protected UIEventListener mbtn_buff_info;
	protected override void _InitScriptBinder()
	{
		mbtn_detailed_info = ScriptBinder.GetObject("btn_detailed_info") as UIEventListener;
		msp_role_head = ScriptBinder.GetObject("sp_role_head") as UISprite;
		mlb_role_name = ScriptBinder.GetObject("lb_role_name") as UILabel;
		mlb_role_level = ScriptBinder.GetObject("lb_role_level") as UILabel;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mUISelectionMenuPanel = ScriptBinder.GetObject("UISelectionMenuPanel") as UnityEngine.GameObject;
		mbginfo = ScriptBinder.GetObject("bginfo") as UISprite;
		mlb_mp = ScriptBinder.GetObject("lb_mp") as UILabel;
		mlb_hp = ScriptBinder.GetObject("lb_hp") as UILabel;
		mslider_hp = ScriptBinder.GetObject("slider_hp") as UISlider;
		mslider_mp = ScriptBinder.GetObject("slider_mp") as UISlider;
		meffectHp = ScriptBinder.GetObject("effectHp") as UnityEngine.GameObject;
		meffectMp = ScriptBinder.GetObject("effectMp") as UnityEngine.GameObject;
		mgrid_dropEquip = ScriptBinder.GetObject("grid_dropEquip") as UIGridContainer;
		mUIBuffPanel = ScriptBinder.GetObject("UIBuffPanel") as UnityEngine.GameObject;
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mScrollView_buff = ScriptBinder.GetObject("ScrollView_buff") as UIScrollView;
		mobj_nonbuff = ScriptBinder.GetObject("obj_nonbuff") as UnityEngine.GameObject;
		mgrid_buff = ScriptBinder.GetObject("grid_buff") as UIGridContainer;
		mbtn_buff_info = ScriptBinder.GetObject("btn_buff_info") as UIEventListener;
	}
}
