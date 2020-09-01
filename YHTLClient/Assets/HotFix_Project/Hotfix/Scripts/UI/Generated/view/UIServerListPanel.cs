public partial class UIServerListPanel : UIBasePanel
{
	protected UIGridContainer mTableList;
	protected UIGridContainer mServerList;
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbg;
	protected UnityEngine.GameObject mshowHideServer;
	protected UIScrollView msView;
	protected UnityEngine.GameObject mbtn_formal;
	protected UnityEngine.GameObject mbtn_test;
	protected UISprite msp_line;
	protected UIToggle mbtn_formalTo;
	protected UIToggle mbtn_testTo;
	protected override void _InitScriptBinder()
	{
		mTableList = ScriptBinder.GetObject("TableList") as UIGridContainer;
		mServerList = ScriptBinder.GetObject("ServerList") as UIGridContainer;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbg = ScriptBinder.GetObject("bg") as UnityEngine.GameObject;
		mshowHideServer = ScriptBinder.GetObject("showHideServer") as UnityEngine.GameObject;
		msView = ScriptBinder.GetObject("sView") as UIScrollView;
		mbtn_formal = ScriptBinder.GetObject("btn_formal") as UnityEngine.GameObject;
		mbtn_test = ScriptBinder.GetObject("btn_test") as UnityEngine.GameObject;
		msp_line = ScriptBinder.GetObject("sp_line") as UISprite;
		mbtn_formalTo = ScriptBinder.GetObject("btn_formalTo") as UIToggle;
		mbtn_testTo = ScriptBinder.GetObject("btn_testTo") as UIToggle;
	}
}
