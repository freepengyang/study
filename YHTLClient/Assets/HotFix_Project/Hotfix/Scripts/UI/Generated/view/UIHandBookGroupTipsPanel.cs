public partial class UIHandBookGroupTipsPanel : UIBasePanel
{
	protected UISprite msp_quality;
	protected UISprite msp_icon;
	protected UILabel mlb_name;
	protected UILabel mlb_collection_desc;
	protected UITable mcurrent;
	protected UITable mnext;
	protected UnityEngine.GameObject mUpArrow;
	protected UnityEngine.GameObject mDownArrow;
	protected UIScrollBar mScrollBar;
	protected UnityEngine.GameObject mButton;
	protected UIGrid mBtnList;
	protected UITable mgroups;
	protected UIEventListener mBG;
	protected UnityEngine.BoxCollider mBoxColider;
	protected UIWidget mWidge;
	protected UnityEngine.GameObject mBtnTemp;
	protected UIScrollView mScrollView;
	protected UISprite mView;
	protected override void _InitScriptBinder()
	{
		msp_quality = ScriptBinder.GetObject("sp_quality") as UISprite;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mlb_collection_desc = ScriptBinder.GetObject("lb_collection_desc") as UILabel;
		mcurrent = ScriptBinder.GetObject("current") as UITable;
		mnext = ScriptBinder.GetObject("next") as UITable;
		mUpArrow = ScriptBinder.GetObject("UpArrow") as UnityEngine.GameObject;
		mDownArrow = ScriptBinder.GetObject("DownArrow") as UnityEngine.GameObject;
		mScrollBar = ScriptBinder.GetObject("ScrollBar") as UIScrollBar;
		mButton = ScriptBinder.GetObject("Button") as UnityEngine.GameObject;
		mBtnList = ScriptBinder.GetObject("BtnList") as UIGrid;
		mgroups = ScriptBinder.GetObject("groups") as UITable;
		mBG = ScriptBinder.GetObject("BG") as UIEventListener;
		mBoxColider = ScriptBinder.GetObject("BoxColider") as UnityEngine.BoxCollider;
		mWidge = ScriptBinder.GetObject("Widge") as UIWidget;
		mBtnTemp = ScriptBinder.GetObject("BtnTemp") as UnityEngine.GameObject;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mView = ScriptBinder.GetObject("View") as UISprite;
	}
}
