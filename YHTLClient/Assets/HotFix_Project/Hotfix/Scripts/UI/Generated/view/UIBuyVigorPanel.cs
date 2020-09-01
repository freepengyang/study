public partial class UIBuyVigorPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_buy;
	protected UnityEngine.Transform mtran_itemPar;
	protected UILabel mlb_name;
	protected UILabel mlb_time;
	protected UILabel mlb_des2;
	protected UnityEngine.GameObject mbtn_reduce;
	protected UnityEngine.GameObject mbtn_add;
	protected UIInput minput_num;
	protected UISprite msp_costIcon;
	protected UILabel mlb_costNum;
	protected UnityEngine.GameObject mbtn_costAdd;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_buy = ScriptBinder.GetObject("btn_buy") as UnityEngine.GameObject;
		mtran_itemPar = ScriptBinder.GetObject("tran_itemPar") as UnityEngine.Transform;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_des2 = ScriptBinder.GetObject("lb_des2") as UILabel;
		mbtn_reduce = ScriptBinder.GetObject("btn_reduce") as UnityEngine.GameObject;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UnityEngine.GameObject;
		minput_num = ScriptBinder.GetObject("input_num") as UIInput;
		msp_costIcon = ScriptBinder.GetObject("sp_costIcon") as UISprite;
		mlb_costNum = ScriptBinder.GetObject("lb_costNum") as UILabel;
		mbtn_costAdd = ScriptBinder.GetObject("btn_costAdd") as UnityEngine.GameObject;
	}
}
