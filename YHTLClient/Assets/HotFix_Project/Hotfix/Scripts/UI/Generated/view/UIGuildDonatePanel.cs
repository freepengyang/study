public partial class UIGuildDonatePanel : UIBasePanel
{
	protected UIEventListener mbtn_Cancel;
	protected UIEventListener mbtn_Submit;
	protected UIEventListener mbtn_Close;
	protected UIEventListener mbtn_Bag;
	protected UILabel mlab_Title;
	protected UILabel mlab_Desc;
	protected UILabel mlab_Desc2;
	protected UILabel mlab_MyCoinNum;
	protected UILabel mlab_MyCoin;
	protected UILabel mlab_Donate;
	protected UILabel mlab_DonateNum;
	protected UIInput mlab_DonateInput;
	protected UIEventListener mbtn_Add;
	protected UIEventListener mbtn_Reduce;
	protected UISprite msp_moneyIcon;
	protected override void _InitScriptBinder()
	{
		mbtn_Cancel = ScriptBinder.GetObject("btn_Cancel") as UIEventListener;
		mbtn_Submit = ScriptBinder.GetObject("btn_Submit") as UIEventListener;
		mbtn_Close = ScriptBinder.GetObject("btn_Close") as UIEventListener;
		mbtn_Bag = ScriptBinder.GetObject("btn_Bag") as UIEventListener;
		mlab_Title = ScriptBinder.GetObject("lab_Title") as UILabel;
		mlab_Desc = ScriptBinder.GetObject("lab_Desc") as UILabel;
		mlab_Desc2 = ScriptBinder.GetObject("lab_Desc2") as UILabel;
		mlab_MyCoinNum = ScriptBinder.GetObject("lab_MyCoinNum") as UILabel;
		mlab_MyCoin = ScriptBinder.GetObject("lab_MyCoin") as UILabel;
		mlab_Donate = ScriptBinder.GetObject("lab_Donate") as UILabel;
		mlab_DonateNum = ScriptBinder.GetObject("lab_DonateNum") as UILabel;
		mlab_DonateInput = ScriptBinder.GetObject("lab_DonateInput") as UIInput;
		mbtn_Add = ScriptBinder.GetObject("btn_Add") as UIEventListener;
		mbtn_Reduce = ScriptBinder.GetObject("btn_Reduce") as UIEventListener;
		msp_moneyIcon = ScriptBinder.GetObject("sp_moneyIcon") as UISprite;
	}
}
