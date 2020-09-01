public partial class UIRenameGuildPanel : UIBasePanel
{
	protected UIInput mrename;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_confirm;
	protected UIEventListener mbtn_bg;
	protected UIGridContainer mgrid;
	protected override void _InitScriptBinder()
	{
		mrename = ScriptBinder.GetObject("rename") as UIInput;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_confirm = ScriptBinder.GetObject("btn_confirm") as UIEventListener;
		mbtn_bg = ScriptBinder.GetObject("btn_bg") as UIEventListener;
		mgrid = ScriptBinder.GetObject("grid") as UIGridContainer;
	}
}
