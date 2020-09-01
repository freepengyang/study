public partial class UITimeExpStageUpPanel : UIDailyHideBasePanel
{
	protected UIGridContainer mGridExtraAddEffects;
	protected UILabel mExtraAddEffects;
	protected UILabel mExtraAddEffectsContent;
	protected UISprite mExtraAddEffectsLine;
	protected UnityEngine.GameObject mgo_effect;
	protected UILabel mlb_nextName;
	protected UILabel mlb_name;
	protected override void _InitScriptBinder()
	{
		mGridExtraAddEffects = ScriptBinder.GetObject("GridExtraAddEffects") as UIGridContainer;
		mExtraAddEffects = ScriptBinder.GetObject("ExtraAddEffects") as UILabel;
		mExtraAddEffectsContent = ScriptBinder.GetObject("ExtraAddEffectsContent") as UILabel;
		mExtraAddEffectsLine = ScriptBinder.GetObject("ExtraAddEffectsLine") as UISprite;
		mgo_effect = ScriptBinder.GetObject("go_effect") as UnityEngine.GameObject;
		mlb_nextName = ScriptBinder.GetObject("lb_nextName") as UILabel;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
	}
}
