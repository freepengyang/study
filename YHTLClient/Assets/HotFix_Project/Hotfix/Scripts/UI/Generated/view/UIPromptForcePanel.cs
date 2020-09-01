public partial class UIPromptForcePanel : UIBasePanel
{
	protected UILabel mlb_Title;
	protected UILabel mlb_Content;
	protected UnityEngine.GameObject mbtn_right;
	protected UILabel mlb_rightLabel;
	protected override void _InitScriptBinder()
	{
		mlb_Title = ScriptBinder.GetObject("lb_Title") as UILabel;
		mlb_Content = ScriptBinder.GetObject("lb_Content") as UILabel;
		mbtn_right = ScriptBinder.GetObject("btn_right") as UnityEngine.GameObject;
		mlb_rightLabel = ScriptBinder.GetObject("lb_rightLabel") as UILabel;
	}
}
