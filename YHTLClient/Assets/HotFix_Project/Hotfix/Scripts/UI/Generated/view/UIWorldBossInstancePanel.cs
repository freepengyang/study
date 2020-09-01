public partial class UIWorldBossInstancePanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_exit;
	protected UILabel mlb_lefttime;
	protected UILabel mlb_bossName;
	protected UISlider msli_bossHp;
	protected UILabel mlb_hpPro;
	protected UnityEngine.GameObject mobj_buffs;
	protected UnityEngine.GameObject mobj_rankPart;
	protected UIGridContainer mgrid_rankItems;
	protected UILabel mlb_selfNum;
	protected UnityEngine.GameObject mbtn_inspire;
	protected UILabel mlb_inspire;
	protected UnityEngine.GameObject mobj_bossInfo;
	protected UnityEngine.GameObject mobj_died;
	protected UnityEngine.GameObject mobj_inspireEff;
	protected UnityEngine.GameObject mobj_hint;
	protected override void _InitScriptBinder()
	{
		mbtn_exit = ScriptBinder.GetObject("btn_exit") as UnityEngine.GameObject;
		mlb_lefttime = ScriptBinder.GetObject("lb_lefttime") as UILabel;
		mlb_bossName = ScriptBinder.GetObject("lb_bossName") as UILabel;
		msli_bossHp = ScriptBinder.GetObject("sli_bossHp") as UISlider;
		mlb_hpPro = ScriptBinder.GetObject("lb_hpPro") as UILabel;
		mobj_buffs = ScriptBinder.GetObject("obj_buffs") as UnityEngine.GameObject;
		mobj_rankPart = ScriptBinder.GetObject("obj_rankPart") as UnityEngine.GameObject;
		mgrid_rankItems = ScriptBinder.GetObject("grid_rankItems") as UIGridContainer;
		mlb_selfNum = ScriptBinder.GetObject("lb_selfNum") as UILabel;
		mbtn_inspire = ScriptBinder.GetObject("btn_inspire") as UnityEngine.GameObject;
		mlb_inspire = ScriptBinder.GetObject("lb_inspire") as UILabel;
		mobj_bossInfo = ScriptBinder.GetObject("obj_bossInfo") as UnityEngine.GameObject;
		mobj_died = ScriptBinder.GetObject("obj_died") as UnityEngine.GameObject;
		mobj_inspireEff = ScriptBinder.GetObject("obj_inspireEff") as UnityEngine.GameObject;
		mobj_hint = ScriptBinder.GetObject("obj_hint") as UnityEngine.GameObject;
	}
}
