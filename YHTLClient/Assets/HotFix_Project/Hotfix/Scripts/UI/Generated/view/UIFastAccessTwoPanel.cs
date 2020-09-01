public partial class UIFastAccessTwoPanel : UIBasePanel
{
	protected UIGridContainer mItemGrid;
	protected UnityEngine.GameObject mGetWay;
	protected UnityEngine.GameObject mBGSprite;
	protected UIGridContainer mTextGrid;
	protected UIEventListener mBtnClose;
	protected UnityEngine.GameObject mSpScroll;
	protected UIScrollView mItemView;
	protected override void _InitScriptBinder()
	{
		mItemGrid = ScriptBinder.GetObject("ItemGrid") as UIGridContainer;
		mGetWay = ScriptBinder.GetObject("GetWay") as UnityEngine.GameObject;
		mBGSprite = ScriptBinder.GetObject("BGSprite") as UnityEngine.GameObject;
		mTextGrid = ScriptBinder.GetObject("TextGrid") as UIGridContainer;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mSpScroll = ScriptBinder.GetObject("SpScroll") as UnityEngine.GameObject;
		mItemView = ScriptBinder.GetObject("ItemView") as UIScrollView;
	}
}
