public partial class UICreateGuildPanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_create;
	protected UILabel mlb_name;
	protected UIInput mchatInput;
	protected UILabel mlb_requirements;
	protected UILabel mlb_need_golds;
	protected UIEventListener mbtn_bg;
	protected UISprite mMoneyIcon;
	protected UIEventListener mbtnMoney;
	protected UIEventListener mBtnGetHorn;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_create = ScriptBinder.GetObject("btn_create") as UIEventListener;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mchatInput = ScriptBinder.GetObject("chatInput") as UIInput;
		mlb_requirements = ScriptBinder.GetObject("lb_requirements") as UILabel;
		mlb_need_golds = ScriptBinder.GetObject("lb_need_golds") as UILabel;
		mbtn_bg = ScriptBinder.GetObject("btn_bg") as UIEventListener;
		mMoneyIcon = ScriptBinder.GetObject("MoneyIcon") as UISprite;
		mbtnMoney = ScriptBinder.GetObject("btnMoney") as UIEventListener;
		mBtnGetHorn = ScriptBinder.GetObject("BtnGetHorn") as UIEventListener;
	}
}
