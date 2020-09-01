public partial class UIPetTalentTipsPanel : UIBasePanel
{
	protected UnityEngine.Transform mtrans_view;
	protected UILabel mlb_text;
	protected UILabel mlb_name;
	protected override void _InitScriptBinder()
	{
		mtrans_view = ScriptBinder.GetObject("trans_view") as UnityEngine.Transform;
		mlb_text = ScriptBinder.GetObject("lb_text") as UILabel;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
	}
}
