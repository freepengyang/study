public partial class UIGemStarSuitTipPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_curInfo;
	protected UnityEngine.GameObject mobj_nextInfo;
	protected UISprite mspr_outframe;
	protected UnityEngine.GameObject mobj_line;
	protected override void _InitScriptBinder()
	{
		mobj_curInfo = ScriptBinder.GetObject("obj_curInfo") as UnityEngine.GameObject;
		mobj_nextInfo = ScriptBinder.GetObject("obj_nextInfo") as UnityEngine.GameObject;
		mspr_outframe = ScriptBinder.GetObject("spr_outframe") as UISprite;
		mobj_line = ScriptBinder.GetObject("obj_line") as UnityEngine.GameObject;
	}
}
