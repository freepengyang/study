public partial class UIRecoverDialogPanel : UIBasePanel
{
	protected UIEventListener mbtn_recover;
	protected UIEventListener mbtn_icon;
	protected UILabel mlb_value;
	protected UIEventListener mbtn_add;
	protected override void _InitScriptBinder()
	{
		mbtn_recover = ScriptBinder.GetObject("btn_recover") as UIEventListener;
		mbtn_icon = ScriptBinder.GetObject("btn_icon") as UIEventListener;
		mlb_value = ScriptBinder.GetObject("lb_value") as UILabel;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UIEventListener;
	}
}
