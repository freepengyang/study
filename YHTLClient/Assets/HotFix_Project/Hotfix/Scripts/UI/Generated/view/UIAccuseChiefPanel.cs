public partial class UIAccuseChiefPanel : UIBasePanel
{
	protected UILabel mlb_time;
	protected UILabel mlb_desc;
	protected UILabel mlb_agree_count;
	protected UILabel mlb_disagree_count;
	protected UILabel mlb_have_voted;
	protected UISlider msp_agree_graphic;
	protected UIEventListener mbtn_Yes;
	protected UIEventListener mbtn_No;
	protected UnityEngine.GameObject mhvnt_vote;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_description;
	protected UIEventListener mbtn_bg;
	protected UISlider msp_disagree_graphic;
	protected override void _InitScriptBinder()
	{
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_desc = ScriptBinder.GetObject("lb_desc") as UILabel;
		mlb_agree_count = ScriptBinder.GetObject("lb_agree_count") as UILabel;
		mlb_disagree_count = ScriptBinder.GetObject("lb_disagree_count") as UILabel;
		mlb_have_voted = ScriptBinder.GetObject("lb_have_voted") as UILabel;
		msp_agree_graphic = ScriptBinder.GetObject("sp_agree_graphic") as UISlider;
		mbtn_Yes = ScriptBinder.GetObject("btn_Yes") as UIEventListener;
		mbtn_No = ScriptBinder.GetObject("btn_No") as UIEventListener;
		mhvnt_vote = ScriptBinder.GetObject("hvnt_vote") as UnityEngine.GameObject;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_description = ScriptBinder.GetObject("btn_description") as UIEventListener;
		mbtn_bg = ScriptBinder.GetObject("btn_bg") as UIEventListener;
		msp_disagree_graphic = ScriptBinder.GetObject("sp_disagree_graphic") as UISlider;
	}
}
