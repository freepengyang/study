public partial class UISummonPanel : UIBasePanel
{
	protected UILabel mDesc;
	protected UISprite mBG;
	protected UIEventListener mBtnCancel;
	protected UIEventListener mBtnOK;
	protected UISlider mSlider;
	protected UnityEngine.Transform mSliderBg;
	protected override void _InitScriptBinder()
	{
		mDesc = ScriptBinder.GetObject("Desc") as UILabel;
		mBG = ScriptBinder.GetObject("BG") as UISprite;
		mBtnCancel = ScriptBinder.GetObject("BtnCancel") as UIEventListener;
		mBtnOK = ScriptBinder.GetObject("BtnOK") as UIEventListener;
		mSlider = ScriptBinder.GetObject("Slider") as UISlider;
		mSliderBg = ScriptBinder.GetObject("SliderBg") as UnityEngine.Transform;
	}
}
