public partial class UIRandomBtnPanel : UIBasePanel
{
	protected UIEventListener mBtnRandom;
	protected TweenPosition mRootTween;
	protected UISprite mItemIcon;
	protected UISprite mCdMask;
	protected UILabel mLbName;
	protected UILabel mLbNum;
	protected UISprite mCdMaskBlack;
	protected override void _InitScriptBinder()
	{
		mBtnRandom = ScriptBinder.GetObject("BtnRandom") as UIEventListener;
		mRootTween = ScriptBinder.GetObject("RootTween") as TweenPosition;
		mItemIcon = ScriptBinder.GetObject("ItemIcon") as UISprite;
		mCdMask = ScriptBinder.GetObject("CdMask") as UISprite;
		mLbName = ScriptBinder.GetObject("LbName") as UILabel;
		mLbNum = ScriptBinder.GetObject("LbNum") as UILabel;
		mCdMaskBlack = ScriptBinder.GetObject("CdMaskBlack") as UISprite;
	}
}
