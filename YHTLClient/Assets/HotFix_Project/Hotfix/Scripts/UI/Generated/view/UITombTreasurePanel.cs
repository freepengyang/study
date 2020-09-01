public partial class UITombTreasurePanel : UIBasePanel
{
	protected UIEventListener mCloseBtn;
	protected UIGrid mGrid;
	protected UnityEngine.GameObject mView;
	protected UnityEngine.GameObject mTexBG;
	protected UILabel mFloorName;
	protected UnityEngine.GameObject mUIItem;
	protected UIGridContainer mStonesGrid;
	protected UIGridContainer mRareGrid;
	protected UIGridContainer mSpecialGrid;
	protected UnityEngine.GameObject mSp_hint;
	protected UnityEngine.GameObject mSpecialLayer;
	protected UIScrollBar mRareScrollBar;
	protected UnityEngine.GameObject mRareDirection;
	protected UIEventListener mLbCheckAll;
	protected UIScrollBar mSpecialScrollBar;
	protected UnityEngine.GameObject mSpecialDirection;
	protected UnityEngine.GameObject mTombredpoint;
	protected UILabel mLbFreeNum;
	protected UIScrollView mRareScrollView;
	protected UIScrollView mSpecialScrollView;
	protected UIEventListener mIconTips;
	protected UISprite mspIcon;
	protected UILabel mLbValue;
	protected UIEventListener mBtnAdd;
	protected override void _InitScriptBinder()
	{
		mCloseBtn = ScriptBinder.GetObject("CloseBtn") as UIEventListener;
		mGrid = ScriptBinder.GetObject("Grid") as UIGrid;
		mView = ScriptBinder.GetObject("View") as UnityEngine.GameObject;
		mTexBG = ScriptBinder.GetObject("TexBG") as UnityEngine.GameObject;
		mFloorName = ScriptBinder.GetObject("FloorName") as UILabel;
		mUIItem = ScriptBinder.GetObject("UIItem") as UnityEngine.GameObject;
		mStonesGrid = ScriptBinder.GetObject("StonesGrid") as UIGridContainer;
		mRareGrid = ScriptBinder.GetObject("RareGrid") as UIGridContainer;
		mSpecialGrid = ScriptBinder.GetObject("SpecialGrid") as UIGridContainer;
		mSp_hint = ScriptBinder.GetObject("Sp_hint") as UnityEngine.GameObject;
		mSpecialLayer = ScriptBinder.GetObject("SpecialLayer") as UnityEngine.GameObject;
		mRareScrollBar = ScriptBinder.GetObject("RareScrollBar") as UIScrollBar;
		mRareDirection = ScriptBinder.GetObject("RareDirection") as UnityEngine.GameObject;
		mLbCheckAll = ScriptBinder.GetObject("LbCheckAll") as UIEventListener;
		mSpecialScrollBar = ScriptBinder.GetObject("SpecialScrollBar") as UIScrollBar;
		mSpecialDirection = ScriptBinder.GetObject("SpecialDirection") as UnityEngine.GameObject;
		mTombredpoint = ScriptBinder.GetObject("Tombredpoint") as UnityEngine.GameObject;
		mLbFreeNum = ScriptBinder.GetObject("LbFreeNum") as UILabel;
		mRareScrollView = ScriptBinder.GetObject("RareScrollView") as UIScrollView;
		mSpecialScrollView = ScriptBinder.GetObject("SpecialScrollView") as UIScrollView;
		mIconTips = ScriptBinder.GetObject("IconTips") as UIEventListener;
		mspIcon = ScriptBinder.GetObject("spIcon") as UISprite;
		mLbValue = ScriptBinder.GetObject("LbValue") as UILabel;
		mBtnAdd = ScriptBinder.GetObject("BtnAdd") as UIEventListener;
	}
}
