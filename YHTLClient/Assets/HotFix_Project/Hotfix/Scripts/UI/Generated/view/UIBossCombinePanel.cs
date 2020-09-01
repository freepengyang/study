public partial class UIBossCombinePanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mUIPersonal;
	protected UIGrid mgrid_tabsPar;
	protected UnityEngine.GameObject mobj_view;
	protected UnityEngine.GameObject mobj_wildRedpoint;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mUIPersonal = ScriptBinder.GetObject("UIPersonal") as UnityEngine.GameObject;
		mgrid_tabsPar = ScriptBinder.GetObject("grid_tabsPar") as UIGrid;
		mobj_view = ScriptBinder.GetObject("obj_view") as UnityEngine.GameObject;
		mobj_wildRedpoint = ScriptBinder.GetObject("obj_wildRedpoint") as UnityEngine.GameObject;
	}
}
