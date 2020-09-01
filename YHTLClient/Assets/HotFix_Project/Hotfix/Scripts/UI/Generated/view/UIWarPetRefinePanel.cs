public partial class UIWarPetRefinePanel : UIBasePanel
{
	protected UIGridContainer mgrid_skill;
	protected UIEventListener mbtn_rule;
	protected UIEventListener mbtn_preview;
	protected UnityEngine.GameObject mcolRefresh;
	protected UnityEngine.GameObject mcolInjection;
	protected UILabel mlb_content1;
	protected UILabel mlb_skillCD1;
	protected UnityEngine.GameObject mbgtex1;
	protected UnityEngine.GameObject mbgtex2;
	protected UILabel mlb_content2;
	protected UILabel mlb_skillCD2;
	protected UnityEngine.GameObject mlb_hint;
	protected UnityEngine.GameObject mgrid_btns;
	protected UIEventListener mbtn_reinject;
	protected UIEventListener mbtn_replace;
	protected UIEventListener mbtn_refresh;
	protected UnityEngine.GameObject mbgtex0;
	protected UIEventListener mbtn_inject;
	protected UnityEngine.GameObject mUIItemBarPrefab1;
	protected UnityEngine.GameObject mUIItemBarPrefab2;
	protected UIGrid mgrid_UIItemBarPrefab;
	protected UISprite msp_icon1;
	protected UILabel mlb_value1;
	protected UIEventListener mbtn_add1;
	protected UISprite msp_icon2;
	protected UILabel mlb_value2;
	protected UIEventListener mbtn_add2;
	protected UISprite msp_skill_icon1;
	protected UILabel mlb_skill_name1;
	protected UISprite msp_skill_icon2;
	protected UILabel mlb_skill_name2;
	protected UIScrollView mScrollView1;
	protected UIScrollView mScrollView2;
	protected UnityEngine.GameObject mredpoint_inject;
	protected UnityEngine.GameObject meffect;
	protected UIScrollView mScrollViewLeft;
	protected TweenPosition mTween_curSkill;
	protected UISprite minjection_sp_skill_icon;
	protected UILabel minjection_lb_skill_name;
	protected UnityEngine.GameObject minjection_ScrollView;
	protected UILabel minjection_lb_content;
	protected UILabel minjection_lb_skillCD;
	protected UILabel minjection_lb_nonSkill;
	protected UIScrollView mScrollView_colRefresh1;
	protected UIScrollView mScrollView_colRefresh2;
	protected UIEventListener mbtn_sp1;
	protected UIEventListener mbtn_sp2;
	protected override void _InitScriptBinder()
	{
		mgrid_skill = ScriptBinder.GetObject("grid_skill") as UIGridContainer;
		mbtn_rule = ScriptBinder.GetObject("btn_rule") as UIEventListener;
		mbtn_preview = ScriptBinder.GetObject("btn_preview") as UIEventListener;
		mcolRefresh = ScriptBinder.GetObject("colRefresh") as UnityEngine.GameObject;
		mcolInjection = ScriptBinder.GetObject("colInjection") as UnityEngine.GameObject;
		mlb_content1 = ScriptBinder.GetObject("lb_content1") as UILabel;
		mlb_skillCD1 = ScriptBinder.GetObject("lb_skillCD1") as UILabel;
		mbgtex1 = ScriptBinder.GetObject("bgtex1") as UnityEngine.GameObject;
		mbgtex2 = ScriptBinder.GetObject("bgtex2") as UnityEngine.GameObject;
		mlb_content2 = ScriptBinder.GetObject("lb_content2") as UILabel;
		mlb_skillCD2 = ScriptBinder.GetObject("lb_skillCD2") as UILabel;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UnityEngine.GameObject;
		mgrid_btns = ScriptBinder.GetObject("grid_btns") as UnityEngine.GameObject;
		mbtn_reinject = ScriptBinder.GetObject("btn_reinject") as UIEventListener;
		mbtn_replace = ScriptBinder.GetObject("btn_replace") as UIEventListener;
		mbtn_refresh = ScriptBinder.GetObject("btn_refresh") as UIEventListener;
		mbgtex0 = ScriptBinder.GetObject("bgtex0") as UnityEngine.GameObject;
		mbtn_inject = ScriptBinder.GetObject("btn_inject") as UIEventListener;
		mUIItemBarPrefab1 = ScriptBinder.GetObject("UIItemBarPrefab1") as UnityEngine.GameObject;
		mUIItemBarPrefab2 = ScriptBinder.GetObject("UIItemBarPrefab2") as UnityEngine.GameObject;
		mgrid_UIItemBarPrefab = ScriptBinder.GetObject("grid_UIItemBarPrefab") as UIGrid;
		msp_icon1 = ScriptBinder.GetObject("sp_icon1") as UISprite;
		mlb_value1 = ScriptBinder.GetObject("lb_value1") as UILabel;
		mbtn_add1 = ScriptBinder.GetObject("btn_add1") as UIEventListener;
		msp_icon2 = ScriptBinder.GetObject("sp_icon2") as UISprite;
		mlb_value2 = ScriptBinder.GetObject("lb_value2") as UILabel;
		mbtn_add2 = ScriptBinder.GetObject("btn_add2") as UIEventListener;
		msp_skill_icon1 = ScriptBinder.GetObject("sp_skill_icon1") as UISprite;
		mlb_skill_name1 = ScriptBinder.GetObject("lb_skill_name1") as UILabel;
		msp_skill_icon2 = ScriptBinder.GetObject("sp_skill_icon2") as UISprite;
		mlb_skill_name2 = ScriptBinder.GetObject("lb_skill_name2") as UILabel;
		mScrollView1 = ScriptBinder.GetObject("ScrollView1") as UIScrollView;
		mScrollView2 = ScriptBinder.GetObject("ScrollView2") as UIScrollView;
		mredpoint_inject = ScriptBinder.GetObject("redpoint_inject") as UnityEngine.GameObject;
		meffect = ScriptBinder.GetObject("effect") as UnityEngine.GameObject;
		mScrollViewLeft = ScriptBinder.GetObject("ScrollViewLeft") as UIScrollView;
		mTween_curSkill = ScriptBinder.GetObject("Tween_curSkill") as TweenPosition;
		minjection_sp_skill_icon = ScriptBinder.GetObject("injection_sp_skill_icon") as UISprite;
		minjection_lb_skill_name = ScriptBinder.GetObject("injection_lb_skill_name") as UILabel;
		minjection_ScrollView = ScriptBinder.GetObject("injection_ScrollView") as UnityEngine.GameObject;
		minjection_lb_content = ScriptBinder.GetObject("injection_lb_content") as UILabel;
		minjection_lb_skillCD = ScriptBinder.GetObject("injection_lb_skillCD") as UILabel;
		minjection_lb_nonSkill = ScriptBinder.GetObject("injection_lb_nonSkill") as UILabel;
		mScrollView_colRefresh1 = ScriptBinder.GetObject("ScrollView_colRefresh1") as UIScrollView;
		mScrollView_colRefresh2 = ScriptBinder.GetObject("ScrollView_colRefresh2") as UIScrollView;
		mbtn_sp1 = ScriptBinder.GetObject("btn_sp1") as UIEventListener;
		mbtn_sp2 = ScriptBinder.GetObject("btn_sp2") as UIEventListener;
	}
}
