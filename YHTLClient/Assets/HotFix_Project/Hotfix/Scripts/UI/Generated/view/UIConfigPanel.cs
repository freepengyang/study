public partial class UIConfigPanel 
{
	protected UIEventListener mbtn_close;
	protected UnityEngine.GameObject mbtn_base;
	protected UnityEngine.GameObject mobj_baseMark;
	protected UnityEngine.GameObject mbtn_hangUp;
	protected UnityEngine.GameObject mobj_hangUpMark;
	protected UnityEngine.GameObject mbtn_graphic;
	protected UnityEngine.GameObject mobj_graphicMark;
	protected UnityEngine.GameObject mbtn_feedBack;
	protected UnityEngine.GameObject mobj_feedBack;
	protected UnityEngine.GameObject mbtn_push;
	protected UnityEngine.GameObject mobj_push;
	protected UnityEngine.GameObject mobj_basePanel;
	protected UnityEngine.GameObject mobj_hangUpPanel;
	protected UnityEngine.GameObject mobj_graphicPanel;
	protected UnityEngine.GameObject mobj_feedbackPanel;
	protected UnityEngine.GameObject mobj_pushSetPanel;
	protected UIToggle mtg_base;
	protected UIToggle mtg_hangup;
	protected UIToggle mtg_graphics;
	protected UIToggle mtg_feedback;
	protected UIToggle mtg_push;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_base = ScriptBinder.GetObject("btn_base") as UnityEngine.GameObject;
		mobj_baseMark = ScriptBinder.GetObject("obj_baseMark") as UnityEngine.GameObject;
		mbtn_hangUp = ScriptBinder.GetObject("btn_hangUp") as UnityEngine.GameObject;
		mobj_hangUpMark = ScriptBinder.GetObject("obj_hangUpMark") as UnityEngine.GameObject;
		mbtn_graphic = ScriptBinder.GetObject("btn_graphic") as UnityEngine.GameObject;
		mobj_graphicMark = ScriptBinder.GetObject("obj_graphicMark") as UnityEngine.GameObject;
		mbtn_feedBack = ScriptBinder.GetObject("btn_feedBack") as UnityEngine.GameObject;
		mobj_feedBack = ScriptBinder.GetObject("obj_feedBack") as UnityEngine.GameObject;
		mbtn_push = ScriptBinder.GetObject("btn_push") as UnityEngine.GameObject;
		mobj_push = ScriptBinder.GetObject("obj_push") as UnityEngine.GameObject;
		mobj_basePanel = ScriptBinder.GetObject("obj_basePanel") as UnityEngine.GameObject;
		mobj_hangUpPanel = ScriptBinder.GetObject("obj_hangUpPanel") as UnityEngine.GameObject;
		mobj_graphicPanel = ScriptBinder.GetObject("obj_graphicPanel") as UnityEngine.GameObject;
		mobj_feedbackPanel = ScriptBinder.GetObject("obj_feedbackPanel") as UnityEngine.GameObject;
		mobj_pushSetPanel = ScriptBinder.GetObject("obj_pushSetPanel") as UnityEngine.GameObject;
		mtg_base = ScriptBinder.GetObject("tg_base") as UIToggle;
		mtg_hangup = ScriptBinder.GetObject("tg_hangup") as UIToggle;
		mtg_graphics = ScriptBinder.GetObject("tg_graphics") as UIToggle;
		mtg_feedback = ScriptBinder.GetObject("tg_feedback") as UIToggle;
		mtg_push = ScriptBinder.GetObject("tg_push") as UIToggle;
	}
}
