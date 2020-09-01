public partial class UIMoneyTipsPanel : UIBasePanel
{
	protected UISprite msp_bg;
	protected UILabel mlb_title;
	protected UnityEngine.GameObject mobj_line;
	protected UnityEngine.GameObject mtable_item;
	protected UnityEngine.Transform mtrans_par;
	protected UnityEngine.Transform mtrans_view;
	protected UILabel mitem1Down;
	protected override void _InitScriptBinder()
	{
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mobj_line = ScriptBinder.GetObject("obj_line") as UnityEngine.GameObject;
		mtable_item = ScriptBinder.GetObject("table_item") as UnityEngine.GameObject;
		mtrans_par = ScriptBinder.GetObject("trans_par") as UnityEngine.Transform;
		mtrans_view = ScriptBinder.GetObject("trans_view") as UnityEngine.Transform;
		mitem1Down = ScriptBinder.GetObject("item1Down") as UILabel;
	}
}
