public partial class UIUltimateChallengCardPanel : UIBasePanel
{
	protected UnityEngine.GameObject mdrawAgain;
	protected UIEventListener mbtn_upgarde;
	protected UILabel mhint1;
	protected UnityEngine.GameObject mhint2;
	protected UIGridContainer mcards;
	protected UnityEngine.GameObject mbtn_cancel;
	protected UnityEngine.GameObject mbtn_config;
	protected UILabel mlb_refreshvalue;
	protected UIEventListener mbtn_refreshadd;
	protected UILabel mlb_openvalue;
	protected UIEventListener mbtn_openadd;
	protected UnityEngine.GameObject mchallenge_bg;
	protected UnityEngine.GameObject mchallenge_card3;
	protected UILabel mlb_hintBeforeSelect;
	protected override void _InitScriptBinder()
	{
		mdrawAgain = ScriptBinder.GetObject("drawAgain") as UnityEngine.GameObject;
		mbtn_upgarde = ScriptBinder.GetObject("btn_upgarde") as UIEventListener;
		mhint1 = ScriptBinder.GetObject("hint1") as UILabel;
		mhint2 = ScriptBinder.GetObject("hint2") as UnityEngine.GameObject;
		mcards = ScriptBinder.GetObject("cards") as UIGridContainer;
		mbtn_cancel = ScriptBinder.GetObject("btn_cancel") as UnityEngine.GameObject;
		mbtn_config = ScriptBinder.GetObject("btn_config") as UnityEngine.GameObject;
		mlb_refreshvalue = ScriptBinder.GetObject("lb_refreshvalue") as UILabel;
		mbtn_refreshadd = ScriptBinder.GetObject("btn_refreshadd") as UIEventListener;
		mlb_openvalue = ScriptBinder.GetObject("lb_openvalue") as UILabel;
		mbtn_openadd = ScriptBinder.GetObject("btn_openadd") as UIEventListener;
		mchallenge_bg = ScriptBinder.GetObject("challenge_bg") as UnityEngine.GameObject;
		mchallenge_card3 = ScriptBinder.GetObject("challenge_card3") as UnityEngine.GameObject;
		mlb_hintBeforeSelect = ScriptBinder.GetObject("lb_hintBeforeSelect") as UILabel;
	}
}
