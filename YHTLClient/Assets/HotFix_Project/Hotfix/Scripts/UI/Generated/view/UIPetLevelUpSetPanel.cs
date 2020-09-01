public partial class UIPetLevelUpSetPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_benyuanlevel;
	protected UnityEngine.GameObject mobj_benyuanquality;
	protected UnityEngine.GameObject mobj_wolonglevel;
	protected UIToggle mobj_tick_normal1;
	protected UIToggle mobj_tick_normal2;
	protected UIToggle mobj_tick_wolong1;
	protected UIToggle mobj_tick_wolong2;
	protected UIToggle mobj_tick_vip;
	protected UnityEngine.GameObject mbtn_quesition;
	protected override void _InitScriptBinder()
	{
		mobj_benyuanlevel = ScriptBinder.GetObject("obj_benyuanlevel") as UnityEngine.GameObject;
		mobj_benyuanquality = ScriptBinder.GetObject("obj_benyuanquality") as UnityEngine.GameObject;
		mobj_wolonglevel = ScriptBinder.GetObject("obj_wolonglevel") as UnityEngine.GameObject;
		mobj_tick_normal1 = ScriptBinder.GetObject("obj_tick_normal1") as UIToggle;
		mobj_tick_normal2 = ScriptBinder.GetObject("obj_tick_normal2") as UIToggle;
		mobj_tick_wolong1 = ScriptBinder.GetObject("obj_tick_wolong1") as UIToggle;
		mobj_tick_wolong2 = ScriptBinder.GetObject("obj_tick_wolong2") as UIToggle;
		mobj_tick_vip = ScriptBinder.GetObject("obj_tick_vip") as UIToggle;
		mbtn_quesition = ScriptBinder.GetObject("btn_quesition") as UnityEngine.GameObject;
	}
}
