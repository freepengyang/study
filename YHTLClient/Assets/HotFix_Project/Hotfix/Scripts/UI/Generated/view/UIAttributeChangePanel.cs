public partial class UIAttributeChangePanel : UIBasePanel
{
	protected UIGridContainer mchangeValues;
	protected UILabel mrolefight_down;
	protected UIPlayTween mdeltaFight;
	protected UILabel mnum;
	protected UILabel msubnum;
	protected UnityEngine.GameObject mbackground;
	protected UnityEngine.GameObject mobj_fightPowerEff;
	protected UnityEngine.GameObject mfight_part;
	protected override void _InitScriptBinder()
	{
		mchangeValues = ScriptBinder.GetObject("changeValues") as UIGridContainer;
		mrolefight_down = ScriptBinder.GetObject("rolefight_down") as UILabel;
		mdeltaFight = ScriptBinder.GetObject("deltaFight") as UIPlayTween;
		mnum = ScriptBinder.GetObject("num") as UILabel;
		msubnum = ScriptBinder.GetObject("subnum") as UILabel;
		mbackground = ScriptBinder.GetObject("background") as UnityEngine.GameObject;
		mobj_fightPowerEff = ScriptBinder.GetObject("obj_fightPowerEff") as UnityEngine.GameObject;
		mfight_part = ScriptBinder.GetObject("fight_part") as UnityEngine.GameObject;
	}
}
