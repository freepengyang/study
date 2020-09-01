public partial class UISpringPaoDianInstacePanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_exit;
	protected UILabel mlb_exp;
	protected UILabel mlb_coin;
	protected UILabel mlb_springQua;
	protected UILabel mlb_time;
	protected UILabel mlb_randomtime;
	protected UILabel mlb_des;
	protected override void _InitScriptBinder()
	{
		mbtn_exit = ScriptBinder.GetObject("btn_exit") as UnityEngine.GameObject;
		mlb_exp = ScriptBinder.GetObject("lb_exp") as UILabel;
		mlb_coin = ScriptBinder.GetObject("lb_coin") as UILabel;
		mlb_springQua = ScriptBinder.GetObject("lb_springQua") as UILabel;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_randomtime = ScriptBinder.GetObject("lb_randomtime") as UILabel;
		mlb_des = ScriptBinder.GetObject("lb_des") as UILabel;
	}
}
