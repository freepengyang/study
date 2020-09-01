public partial class UIWorldInspirePanel : UIBasePanel
{
	protected UILabel mlb_coinAdd;
	protected UnityEngine.GameObject mbtn_coin;
	protected UILabel mlb_coinCost;
	protected UILabel mlb_goldAdd;
	protected UnityEngine.GameObject mbtn_gold;
	protected UILabel mlb_goldCost;
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_bg;
	protected UILabel mlb_coinCount;
	protected UILabel mlb_goldCount;
	protected override void _InitScriptBinder()
	{
		mlb_coinAdd = ScriptBinder.GetObject("lb_coinAdd") as UILabel;
		mbtn_coin = ScriptBinder.GetObject("btn_coin") as UnityEngine.GameObject;
		mlb_coinCost = ScriptBinder.GetObject("lb_coinCost") as UILabel;
		mlb_goldAdd = ScriptBinder.GetObject("lb_goldAdd") as UILabel;
		mbtn_gold = ScriptBinder.GetObject("btn_gold") as UnityEngine.GameObject;
		mlb_goldCost = ScriptBinder.GetObject("lb_goldCost") as UILabel;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_bg = ScriptBinder.GetObject("btn_bg") as UnityEngine.GameObject;
		mlb_coinCount = ScriptBinder.GetObject("lb_coinCount") as UILabel;
		mlb_goldCount = ScriptBinder.GetObject("lb_goldCount") as UILabel;
	}
}
