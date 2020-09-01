public partial class UIWoLongRefineResultPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_refine;
	protected UnityEngine.GameObject mbtn_confirm;
	protected UnityEngine.GameObject mbtn_close;
	protected UIGridContainer mgrid_oldInten;
	protected UIGridContainer mgrid_oldBase;
	protected UIGridContainer mgrid_NewInten;
	protected UIGridContainer mgrid_NewBase;
	protected UILabel mlb_moneyNum;
	protected UISprite msp_moneyIcon;
	protected UnityEngine.GameObject mbtn_moneyBuy;
	protected UILabel mlb_goodsNum;
	protected UISprite msp_goodsIcon;
	protected UnityEngine.GameObject mbtn_goodsBuy;
	protected override void _InitScriptBinder()
	{
		mbtn_refine = ScriptBinder.GetObject("btn_refine") as UnityEngine.GameObject;
		mbtn_confirm = ScriptBinder.GetObject("btn_confirm") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mgrid_oldInten = ScriptBinder.GetObject("grid_oldInten") as UIGridContainer;
		mgrid_oldBase = ScriptBinder.GetObject("grid_oldBase") as UIGridContainer;
		mgrid_NewInten = ScriptBinder.GetObject("grid_NewInten") as UIGridContainer;
		mgrid_NewBase = ScriptBinder.GetObject("grid_NewBase") as UIGridContainer;
		mlb_moneyNum = ScriptBinder.GetObject("lb_moneyNum") as UILabel;
		msp_moneyIcon = ScriptBinder.GetObject("sp_moneyIcon") as UISprite;
		mbtn_moneyBuy = ScriptBinder.GetObject("btn_moneyBuy") as UnityEngine.GameObject;
		mlb_goodsNum = ScriptBinder.GetObject("lb_goodsNum") as UILabel;
		msp_goodsIcon = ScriptBinder.GetObject("sp_goodsIcon") as UISprite;
		mbtn_goodsBuy = ScriptBinder.GetObject("btn_goodsBuy") as UnityEngine.GameObject;
	}
}
