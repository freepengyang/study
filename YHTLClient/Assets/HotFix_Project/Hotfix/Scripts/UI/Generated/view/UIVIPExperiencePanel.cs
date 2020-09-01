public partial class UIVIPExperiencePanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_go;
	protected UILabel mlb_time;
	protected UILabel mlb_item;
	protected UnityEngine.GameObject msp_state;
	protected UnityEngine.GameObject msp_end;
	protected UnityEngine.GameObject mbtn_end;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_go = ScriptBinder.GetObject("btn_go") as UnityEngine.GameObject;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_item = ScriptBinder.GetObject("lb_item") as UILabel;
		msp_state = ScriptBinder.GetObject("sp_state") as UnityEngine.GameObject;
		msp_end = ScriptBinder.GetObject("sp_end") as UnityEngine.GameObject;
		mbtn_end = ScriptBinder.GetObject("btn_end") as UnityEngine.GameObject;
	}
}
