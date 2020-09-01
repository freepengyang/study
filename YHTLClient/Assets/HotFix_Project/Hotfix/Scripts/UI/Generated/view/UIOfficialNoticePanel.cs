public partial class UIOfficialNoticePanel : UIBasePanel
{
	protected UILabel mlb_content;
	protected UnityEngine.GameObject mbtn_know;
	protected UnityEngine.GameObject mbtn_close;
	protected override void _InitScriptBinder()
	{
		mlb_content = ScriptBinder.GetObject("lb_content") as UILabel;
		mbtn_know = ScriptBinder.GetObject("btn_know") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
	}
}
