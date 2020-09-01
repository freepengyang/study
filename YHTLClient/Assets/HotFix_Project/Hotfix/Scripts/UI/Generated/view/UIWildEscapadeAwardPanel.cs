public partial class UIWildEscapadeAwardPanel : UIBasePanel
{
	protected UILabel mlb_time;
	protected UILabel mlb_exp;
	protected UILabel mlb_money;
	protected UIEventListener mbtn_draw;
	protected UIScrollView mScrollView;
	protected UIGrid mGird;
	protected UISprite msp_moneyIcon;
	protected UILabel mlb_page;
	protected override void _InitScriptBinder()
	{
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_exp = ScriptBinder.GetObject("lb_exp") as UILabel;
		mlb_money = ScriptBinder.GetObject("lb_money") as UILabel;
		mbtn_draw = ScriptBinder.GetObject("btn_draw") as UIEventListener;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mGird = ScriptBinder.GetObject("Gird") as UIGrid;
		msp_moneyIcon = ScriptBinder.GetObject("sp_moneyIcon") as UISprite;
		mlb_page = ScriptBinder.GetObject("lb_page") as UILabel;
	}
}
