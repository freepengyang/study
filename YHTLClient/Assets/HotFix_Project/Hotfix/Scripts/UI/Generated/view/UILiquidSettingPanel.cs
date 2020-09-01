public partial class UILiquidSettingPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_buy;
	protected UnityEngine.GameObject mshowitem;
	protected UIGrid mgrid1;
	protected UIGrid mgrid2;
	protected UIGrid mgrid3;
	protected UISprite mbtn_autobuy;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_buy = ScriptBinder.GetObject("btn_buy") as UnityEngine.GameObject;
		mshowitem = ScriptBinder.GetObject("showitem") as UnityEngine.GameObject;
		mgrid1 = ScriptBinder.GetObject("grid1") as UIGrid;
		mgrid2 = ScriptBinder.GetObject("grid2") as UIGrid;
		mgrid3 = ScriptBinder.GetObject("grid3") as UIGrid;
		mbtn_autobuy = ScriptBinder.GetObject("btn_autobuy") as UISprite;
	}
}
