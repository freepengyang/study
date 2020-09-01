public partial class UICompleteWayPanel : UIBasePanel
{
	protected UISprite msp_bg;
	protected UIGridContainer mgrid_btn;
	protected UIEventListener mbtn_bg;
	protected UISprite msp_bgButton;
	protected UnityEngine.Transform mtrans_view;
	protected UnityEngine.Transform mtrans_viewParent;
	protected UILabel mlb_title;
	protected override void _InitScriptBinder()
	{
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mgrid_btn = ScriptBinder.GetObject("grid_btn") as UIGridContainer;
		mbtn_bg = ScriptBinder.GetObject("btn_bg") as UIEventListener;
		msp_bgButton = ScriptBinder.GetObject("sp_bgButton") as UISprite;
		mtrans_view = ScriptBinder.GetObject("trans_view") as UnityEngine.Transform;
		mtrans_viewParent = ScriptBinder.GetObject("trans_viewParent") as UnityEngine.Transform;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
	}
}
