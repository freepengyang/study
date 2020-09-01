public partial class UIRedNameNPCDialogPanel : UIBasePanel
{
	protected UnityEngine.GameObject mobj_buy;
	protected UnityEngine.GameObject mbtn_leave;
	protected UILabel mlb_des;
	protected UISprite msp_btnGo;
	protected UILabel mlb_btnGo;
	protected override void _InitScriptBinder()
	{
		mobj_buy = ScriptBinder.GetObject("obj_buy") as UnityEngine.GameObject;
		mbtn_leave = ScriptBinder.GetObject("btn_leave") as UnityEngine.GameObject;
		mlb_des = ScriptBinder.GetObject("lb_des") as UILabel;
		msp_btnGo = ScriptBinder.GetObject("sp_btnGo") as UISprite;
		mlb_btnGo = ScriptBinder.GetObject("lb_btnGo") as UILabel;
	}
}
