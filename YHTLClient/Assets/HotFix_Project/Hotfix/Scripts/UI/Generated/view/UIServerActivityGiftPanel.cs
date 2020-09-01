public partial class UIServerActivityGiftPanel : UIBasePanel
{
	protected UITexture mtex_banner3;
	protected UIGridBinderContainer mgrid_tab;
	protected UIGridContainer mgrid_item;
	protected UnityEngine.GameObject mbtn_get;
	protected UnityEngine.GameObject mbtn_close;
	protected UILabel mlb_money;
	protected UnityEngine.GameObject mlb_Receive;
	protected UISprite meffect;
	protected UnityEngine.GameObject mobj_select;
	protected override void _InitScriptBinder()
	{
		mtex_banner3 = ScriptBinder.GetObject("tex_banner3") as UITexture;
		mgrid_tab = ScriptBinder.GetObject("grid_tab") as UIGridBinderContainer;
		mgrid_item = ScriptBinder.GetObject("grid_item") as UIGridContainer;
		mbtn_get = ScriptBinder.GetObject("btn_get") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mlb_money = ScriptBinder.GetObject("lb_money") as UILabel;
		mlb_Receive = ScriptBinder.GetObject("lb_Receive") as UnityEngine.GameObject;
		meffect = ScriptBinder.GetObject("effect") as UISprite;
		mobj_select = ScriptBinder.GetObject("obj_select") as UnityEngine.GameObject;
	}
}
