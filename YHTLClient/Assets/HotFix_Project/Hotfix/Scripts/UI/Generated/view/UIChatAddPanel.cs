public partial class UIChatAddPanel : UIBasePanel
{
	protected TweenPosition mWindowPositoinTween;
	protected UIToggle mBagToggle;
	protected UIToggle mWearedNormalEquipToggle;
	protected UIToggle mWearedNeigongEquipToggle;
	protected UIToggle mEmotionToggle;
	protected UIToggle mLocationToggle;
	protected UnityEngine.GameObject mToggleGroup;
	protected UIGrid mEmotionGrids;
	protected UnityEngine.GameObject mEmotionPanel;
	protected UIScrollView mBagScrollview;
	protected UILabel mLocalPosition;
	protected UISprite mBg;
	protected UIGridContainer mGridPoints;
	protected UIGrid mGridItems;
	protected UIDragScrollView mBagDragScrollView;
	protected UIGrid mGridItemsL;
	protected UIGrid mGridItemsR;
	protected UIPanel mLPanel;
	protected UIPanel mRPanel;
	protected override void _InitScriptBinder()
	{
		mWindowPositoinTween = ScriptBinder.GetObject("WindowPositoinTween") as TweenPosition;
		mBagToggle = ScriptBinder.GetObject("BagToggle") as UIToggle;
		mWearedNormalEquipToggle = ScriptBinder.GetObject("WearedNormalEquipToggle") as UIToggle;
		mWearedNeigongEquipToggle = ScriptBinder.GetObject("WearedNeigongEquipToggle") as UIToggle;
		mEmotionToggle = ScriptBinder.GetObject("EmotionToggle") as UIToggle;
		mLocationToggle = ScriptBinder.GetObject("LocationToggle") as UIToggle;
		mToggleGroup = ScriptBinder.GetObject("ToggleGroup") as UnityEngine.GameObject;
		mEmotionGrids = ScriptBinder.GetObject("EmotionGrids") as UIGrid;
		mEmotionPanel = ScriptBinder.GetObject("EmotionPanel") as UnityEngine.GameObject;
		mBagScrollview = ScriptBinder.GetObject("BagScrollview") as UIScrollView;
		mLocalPosition = ScriptBinder.GetObject("LocalPosition") as UILabel;
		mBg = ScriptBinder.GetObject("Bg") as UISprite;
		mGridPoints = ScriptBinder.GetObject("GridPoints") as UIGridContainer;
		mGridItems = ScriptBinder.GetObject("GridItems") as UIGrid;
		mBagDragScrollView = ScriptBinder.GetObject("BagDragScrollView") as UIDragScrollView;
		mGridItemsL = ScriptBinder.GetObject("GridItemsL") as UIGrid;
		mGridItemsR = ScriptBinder.GetObject("GridItemsR") as UIGrid;
		mLPanel = ScriptBinder.GetObject("LPanel") as UIPanel;
		mRPanel = ScriptBinder.GetObject("RPanel") as UIPanel;
	}
}
