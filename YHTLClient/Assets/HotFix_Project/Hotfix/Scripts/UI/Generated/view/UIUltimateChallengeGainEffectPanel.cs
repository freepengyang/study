public partial class UIUltimateChallengeGainEffectPanel : UIBasePanel
{
	protected UIScrollView mScrollView;
	protected UIGridContainer mgrid;
	protected UIGridContainer mpagePointGrid;
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mnoneAttrbute;
	protected UnityEngine.GameObject mscrolviewConti;
	protected UnityEngine.GameObject mpattern;
	protected override void _InitScriptBinder()
	{
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mgrid = ScriptBinder.GetObject("grid") as UIGridContainer;
		mpagePointGrid = ScriptBinder.GetObject("pagePointGrid") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mnoneAttrbute = ScriptBinder.GetObject("noneAttrbute") as UnityEngine.GameObject;
		mscrolviewConti = ScriptBinder.GetObject("scrolviewConti") as UnityEngine.GameObject;
		mpattern = ScriptBinder.GetObject("pattern") as UnityEngine.GameObject;
	}
}
