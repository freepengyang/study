public partial class UIHonorChanllengeCombinePanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UIGrid mgrid_tabsPar;
	protected UnityEngine.GameObject mobj_view;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mgrid_tabsPar = ScriptBinder.GetObject("grid_tabsPar") as UIGrid;
		mobj_view = ScriptBinder.GetObject("obj_view") as UnityEngine.GameObject;
	}
}
