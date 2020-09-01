public partial class UIMaFaPromptPanel : UIBasePanel
{
	protected UIGridContainer mGrid;
	protected UISprite mTitle;
	protected UILabel mLbTitle;
	protected UnityEngine.Transform mHintTrs;
	protected UnityEngine.Transform mBtnBuy;
	protected UnityEngine.GameObject mTexBG;
	protected UILabel mLbBtnBuy;
	protected override void _InitScriptBinder()
	{
		mGrid = ScriptBinder.GetObject("Grid") as UIGridContainer;
		mTitle = ScriptBinder.GetObject("Title") as UISprite;
		mLbTitle = ScriptBinder.GetObject("LbTitle") as UILabel;
		mHintTrs = ScriptBinder.GetObject("HintTrs") as UnityEngine.Transform;
		mBtnBuy = ScriptBinder.GetObject("BtnBuy") as UnityEngine.Transform;
		mTexBG = ScriptBinder.GetObject("TexBG") as UnityEngine.GameObject;
		mLbBtnBuy = ScriptBinder.GetObject("LbBtnBuy") as UILabel;
	}
}
