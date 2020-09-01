public partial class UIGuildFightStatusPanel : UIBasePanel
{
	protected UILabel mendTime;
	protected UILabel mguildName;
	protected UIEventListener mbtnRank;
	protected UILabel mstatus_desc;
	protected UISlider mSlider;
	protected UILabel mSliderPercent;
	protected UISprite mStageImage;
	protected UIEventListener mbtnGoFight;
	protected UILabel mHint;
	protected UIEventListener mbtnDoorLink;
	protected override void _InitScriptBinder()
	{
		mendTime = ScriptBinder.GetObject("endTime") as UILabel;
		mguildName = ScriptBinder.GetObject("guildName") as UILabel;
		mbtnRank = ScriptBinder.GetObject("btnRank") as UIEventListener;
		mstatus_desc = ScriptBinder.GetObject("status_desc") as UILabel;
		mSlider = ScriptBinder.GetObject("Slider") as UISlider;
		mSliderPercent = ScriptBinder.GetObject("SliderPercent") as UILabel;
		mStageImage = ScriptBinder.GetObject("StageImage") as UISprite;
		mbtnGoFight = ScriptBinder.GetObject("btnGoFight") as UIEventListener;
		mHint = ScriptBinder.GetObject("Hint") as UILabel;
		mbtnDoorLink = ScriptBinder.GetObject("btnDoorLink") as UIEventListener;
	}
}
