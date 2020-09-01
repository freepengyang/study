public partial class UILianTiLandPanel : UIBasePanel
{
	protected UIEventListener mCloseBtn;
	protected UIEventListener mTeleportBtn;
	protected UIGridContainer mMapGrid;
	protected UIGrid mTipsGrid;
	protected UILabel mLBLevel;
	protected UIScrollView mMapsScrollView;
	protected UIScrollBar mScrollBar;
	protected UnityEngine.GameObject mLeftObj;
	protected UnityEngine.GameObject mRightObj;
	protected UISprite mTeleportBtnBG;
	protected UnityEngine.GameObject mBG;
	protected UISprite mSpIcon;
	protected UILabel mLbValue;
	protected UnityEngine.GameObject mBtnAdd;
	protected UIEventListener mIcon;
	protected override void _InitScriptBinder()
	{
		mCloseBtn = ScriptBinder.GetObject("CloseBtn") as UIEventListener;
		mTeleportBtn = ScriptBinder.GetObject("TeleportBtn") as UIEventListener;
		mMapGrid = ScriptBinder.GetObject("MapGrid") as UIGridContainer;
		mTipsGrid = ScriptBinder.GetObject("TipsGrid") as UIGrid;
		mLBLevel = ScriptBinder.GetObject("LBLevel") as UILabel;
		mMapsScrollView = ScriptBinder.GetObject("MapsScrollView") as UIScrollView;
		mScrollBar = ScriptBinder.GetObject("ScrollBar") as UIScrollBar;
		mLeftObj = ScriptBinder.GetObject("LeftObj") as UnityEngine.GameObject;
		mRightObj = ScriptBinder.GetObject("RightObj") as UnityEngine.GameObject;
		mTeleportBtnBG = ScriptBinder.GetObject("TeleportBtnBG") as UISprite;
		mBG = ScriptBinder.GetObject("BG") as UnityEngine.GameObject;
		mSpIcon = ScriptBinder.GetObject("SpIcon") as UISprite;
		mLbValue = ScriptBinder.GetObject("LbValue") as UILabel;
		mBtnAdd = ScriptBinder.GetObject("BtnAdd") as UnityEngine.GameObject;
		mIcon = ScriptBinder.GetObject("Icon") as UIEventListener;
	}
}
