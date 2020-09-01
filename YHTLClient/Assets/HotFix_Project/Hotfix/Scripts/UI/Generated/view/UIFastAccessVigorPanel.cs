public partial class UIFastAccessVigorPanel : UIBasePanel
{
	protected UnityEngine.Transform mtrans_itemPar;
	protected UnityEngine.GameObject mbtn_exchange;
	protected UnityEngine.GameObject mobj_eff;
	protected UILabel mlb_name;
	protected UILabel mlb_costNum;
	protected UISprite msp_icon;
	protected UILabel mlb_exchangeCount;
	protected UnityEngine.GameObject mbtn_close;
	protected UIGridContainer mgrid_getWayItem;
	protected override void _InitScriptBinder()
	{
		mtrans_itemPar = ScriptBinder.GetObject("trans_itemPar") as UnityEngine.Transform;
		mbtn_exchange = ScriptBinder.GetObject("btn_exchange") as UnityEngine.GameObject;
		mobj_eff = ScriptBinder.GetObject("obj_eff") as UnityEngine.GameObject;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mlb_costNum = ScriptBinder.GetObject("lb_costNum") as UILabel;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mlb_exchangeCount = ScriptBinder.GetObject("lb_exchangeCount") as UILabel;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mgrid_getWayItem = ScriptBinder.GetObject("grid_getWayItem") as UIGridContainer;
	}
}
