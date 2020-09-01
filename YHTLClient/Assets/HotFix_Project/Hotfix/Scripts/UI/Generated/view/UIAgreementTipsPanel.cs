public partial class UIAgreementTipsPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_knew;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_knew = ScriptBinder.GetObject("btn_knew") as UIEventListener;
	}
}
