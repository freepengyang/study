public partial class UIWelfareMonthMapPanel : UIBasePanel
{
	protected UnityEngine.GameObject mbanner24;
	protected UnityEngine.GameObject mobj_hasA;
	protected UnityEngine.GameObject mobj_notHasA;
	protected UnityEngine.GameObject mobj_hasB;
	protected UnityEngine.GameObject mobj_notHasB;
	protected UIEventListener mbtn_addTicketA;
	protected UISprite msp_ticketA;
	protected UILabel mlb_valueA;
	protected UIEventListener mbtn_addTicketB;
	protected UISprite msp_ticketB;
	protected UILabel mlb_valueB;
	protected UnityEngine.GameObject mbtn_brave_purchased;
	protected UnityEngine.GameObject mbtn_king_purchased;
	protected UnityEngine.GameObject mbtn_brave_not_purchased;
	protected UnityEngine.GameObject mbtn_king_not_purchased;
	protected UILabel mlb_tips;
	protected override void _InitScriptBinder()
	{
		mbanner24 = ScriptBinder.GetObject("banner24") as UnityEngine.GameObject;
		mobj_hasA = ScriptBinder.GetObject("obj_hasA") as UnityEngine.GameObject;
		mobj_notHasA = ScriptBinder.GetObject("obj_notHasA") as UnityEngine.GameObject;
		mobj_hasB = ScriptBinder.GetObject("obj_hasB") as UnityEngine.GameObject;
		mobj_notHasB = ScriptBinder.GetObject("obj_notHasB") as UnityEngine.GameObject;
		mbtn_addTicketA = ScriptBinder.GetObject("btn_addTicketA") as UIEventListener;
		msp_ticketA = ScriptBinder.GetObject("sp_ticketA") as UISprite;
		mlb_valueA = ScriptBinder.GetObject("lb_valueA") as UILabel;
		mbtn_addTicketB = ScriptBinder.GetObject("btn_addTicketB") as UIEventListener;
		msp_ticketB = ScriptBinder.GetObject("sp_ticketB") as UISprite;
		mlb_valueB = ScriptBinder.GetObject("lb_valueB") as UILabel;
		mbtn_brave_purchased = ScriptBinder.GetObject("btn_brave_purchased") as UnityEngine.GameObject;
		mbtn_king_purchased = ScriptBinder.GetObject("btn_king_purchased") as UnityEngine.GameObject;
		mbtn_brave_not_purchased = ScriptBinder.GetObject("btn_brave_not_purchased") as UnityEngine.GameObject;
		mbtn_king_not_purchased = ScriptBinder.GetObject("btn_king_not_purchased") as UnityEngine.GameObject;
		mlb_tips = ScriptBinder.GetObject("lb_tips") as UILabel;
	}

}
