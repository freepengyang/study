public partial class UICostPromptPanel : UIBasePanel
{
	protected UILabel mlb_Title;
	protected UILabel mlb_Content;
	protected UISprite msp_icon;
	protected UILabel msp_value;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_left;
	protected UIEventListener mbtn_right;
	protected UILabel mlb_leftLabel;
	protected UILabel mlb_rightLabel;
	protected UIEventListener mbtn_add;
	protected UnityEngine.GameObject mItemBarHandle;
	protected UIEventListener mbtn_bg;
	protected UIGrid mgrid;
	protected override void _InitScriptBinder()
	{
		mlb_Title = ScriptBinder.GetObject("lb_Title") as UILabel;
		mlb_Content = ScriptBinder.GetObject("lb_Content") as UILabel;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		msp_value = ScriptBinder.GetObject("sp_value") as UILabel;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_left = ScriptBinder.GetObject("btn_left") as UIEventListener;
		mbtn_right = ScriptBinder.GetObject("btn_right") as UIEventListener;
		mlb_leftLabel = ScriptBinder.GetObject("lb_leftLabel") as UILabel;
		mlb_rightLabel = ScriptBinder.GetObject("lb_rightLabel") as UILabel;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UIEventListener;
		mItemBarHandle = ScriptBinder.GetObject("ItemBarHandle") as UnityEngine.GameObject;
		mbtn_bg = ScriptBinder.GetObject("btn_bg") as UIEventListener;
		mgrid = ScriptBinder.GetObject("grid") as UIGrid;
	}
}
