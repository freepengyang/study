public partial class UIUltimateInstacePanel : UIBasePanel
{
	protected UILabel mlb_killMonster;
	protected UILabel mlb_level;
	protected UIEventListener mbtn_addattr;
	protected UISlider mslider;
	protected UILabel mlabel;
	protected UnityEngine.GameObject mChallengeResult;
	protected UnityEngine.GameObject mbgtitle;
	protected UILabel mlb_title;
	protected override void _InitScriptBinder()
	{
		mlb_killMonster = ScriptBinder.GetObject("lb_killMonster") as UILabel;
		mlb_level = ScriptBinder.GetObject("lb_level") as UILabel;
		mbtn_addattr = ScriptBinder.GetObject("btn_addattr") as UIEventListener;
		mslider = ScriptBinder.GetObject("slider") as UISlider;
		mlabel = ScriptBinder.GetObject("label") as UILabel;
		mChallengeResult = ScriptBinder.GetObject("ChallengeResult") as UnityEngine.GameObject;
		mbgtitle = ScriptBinder.GetObject("bgtitle") as UnityEngine.GameObject;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
	}
}
