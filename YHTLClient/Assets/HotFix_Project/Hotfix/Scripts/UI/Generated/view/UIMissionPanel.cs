public partial class UIMissionPanel : UIBasePanel
{
	protected UnityEngine.GameObject mTips;
	protected UIGridContainer mButtonGroup;
	protected UISprite mbg2;
	protected UIScrollView mlistScrollView;
	protected UnityEngine.GameObject mSpecialTipGroup;
	protected UnityEngine.GameObject mMsiionList;
	protected UnityEngine.GameObject mrecommend;
	protected UILabel mlb_title;
	protected UISprite mspr_icon;
	protected UILabel mlb_level;
	protected UIPanel mlistScrollViewPanel;
	protected UnityEngine.GameObject mmission;
	protected UnityEngine.GameObject mmissionEffect;
	protected UIEventListener mbtn_recommand;
	protected UISprite mtipBg;
	protected TweenAlpha mTweenAlpha;
	protected override void _InitScriptBinder()
	{
		mTips = ScriptBinder.GetObject("Tips") as UnityEngine.GameObject;
		mButtonGroup = ScriptBinder.GetObject("ButtonGroup") as UIGridContainer;
		mbg2 = ScriptBinder.GetObject("bg2") as UISprite;
		mlistScrollView = ScriptBinder.GetObject("listScrollView") as UIScrollView;
		mSpecialTipGroup = ScriptBinder.GetObject("SpecialTipGroup") as UnityEngine.GameObject;
		mMsiionList = ScriptBinder.GetObject("MsiionList") as UnityEngine.GameObject;
		mrecommend = ScriptBinder.GetObject("recommend") as UnityEngine.GameObject;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mspr_icon = ScriptBinder.GetObject("spr_icon") as UISprite;
		mlb_level = ScriptBinder.GetObject("lb_level") as UILabel;
		mlistScrollViewPanel = ScriptBinder.GetObject("listScrollViewPanel") as UIPanel;
		mmission = ScriptBinder.GetObject("mission") as UnityEngine.GameObject;
		mmissionEffect = ScriptBinder.GetObject("missionEffect") as UnityEngine.GameObject;
		mbtn_recommand = ScriptBinder.GetObject("btn_recommand") as UIEventListener;
		mtipBg = ScriptBinder.GetObject("tipBg") as UISprite;
		mTweenAlpha = ScriptBinder.GetObject("TweenAlpha") as TweenAlpha;
	}
}
