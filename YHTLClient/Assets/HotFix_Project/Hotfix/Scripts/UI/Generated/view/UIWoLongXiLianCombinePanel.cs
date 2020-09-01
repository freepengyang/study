public partial class UIWoLongXiLianCombinePanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mobj_view;
	protected UIGrid mgrid_tabsPar;
	protected UnityEngine.GameObject mlongJi_red;
	protected UnityEngine.GameObject mlongli_red;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mobj_view = ScriptBinder.GetObject("obj_view") as UnityEngine.GameObject;
		mgrid_tabsPar = ScriptBinder.GetObject("grid_tabsPar") as UIGrid;
		mlongJi_red = ScriptBinder.GetObject("longJi_red") as UnityEngine.GameObject;
		mlongli_red = ScriptBinder.GetObject("longli_red") as UnityEngine.GameObject;
	}
}
