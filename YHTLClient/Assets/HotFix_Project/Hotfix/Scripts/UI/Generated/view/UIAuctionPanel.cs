public partial class UIAuctionPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_buy;
	protected UnityEngine.GameObject mbtn_sell;
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mobj_tabs;
	protected UnityEngine.GameObject mobj_views;
	protected UnityEngine.GameObject mred_sellRed;
	protected override void _InitScriptBinder()
	{
		mbtn_buy = ScriptBinder.GetObject("btn_buy") as UnityEngine.GameObject;
		mbtn_sell = ScriptBinder.GetObject("btn_sell") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mobj_tabs = ScriptBinder.GetObject("obj_tabs") as UnityEngine.GameObject;
		mobj_views = ScriptBinder.GetObject("obj_views") as UnityEngine.GameObject;
		mred_sellRed = ScriptBinder.GetObject("red_sellRed") as UnityEngine.GameObject;
	}
}
