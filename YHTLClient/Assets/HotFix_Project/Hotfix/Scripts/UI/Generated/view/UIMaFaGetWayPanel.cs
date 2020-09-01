public partial class UIMaFaGetWayPanel : UIBasePanel
{
	protected UnityEngine.GameObject mBG;
	protected UIGridContainer mBtnGrid;
	protected UnityEngine.GameObject mButtonBg;
	protected UnityEngine.Transform mViewTrs;
	protected override void _InitScriptBinder()
	{
		mBG = ScriptBinder.GetObject("BG") as UnityEngine.GameObject;
		mBtnGrid = ScriptBinder.GetObject("BtnGrid") as UIGridContainer;
		mButtonBg = ScriptBinder.GetObject("ButtonBg") as UnityEngine.GameObject;
		mViewTrs = ScriptBinder.GetObject("ViewTrs") as UnityEngine.Transform;
	}
}
