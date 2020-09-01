public partial class UIServerActivityEquipRewardsPanel : UIBasePanel
{
	protected UnityEngine.GameObject mtex_title;
	protected UIGridContainer mGridItem;
	protected UILabel mLbTime;
	protected UnityEngine.GameObject mSpScroll;
	protected UIScrollBar mBar;
	protected CSInvoke mCSInvoke;
	protected UIWrapContent mWrap;
	protected UIScrollView mitemView;
	protected UIToggle mToggle1;
	protected UIToggle mToggle2;
	protected UILabel mLbText;
	protected UILabel mLbOperator;
	protected UnityEngine.GameObject mRedPoint;
	protected override void _InitScriptBinder()
	{
		mtex_title = ScriptBinder.GetObject("tex_title") as UnityEngine.GameObject;
		mGridItem = ScriptBinder.GetObject("GridItem") as UIGridContainer;
		mLbTime = ScriptBinder.GetObject("LbTime") as UILabel;
		mSpScroll = ScriptBinder.GetObject("SpScroll") as UnityEngine.GameObject;
		mBar = ScriptBinder.GetObject("Bar") as UIScrollBar;
		mCSInvoke = ScriptBinder.GetObject("CSInvoke") as CSInvoke;
		mWrap = ScriptBinder.GetObject("Wrap") as UIWrapContent;
		mitemView = ScriptBinder.GetObject("itemView") as UIScrollView;
		mToggle1 = ScriptBinder.GetObject("Toggle1") as UIToggle;
		mToggle2 = ScriptBinder.GetObject("Toggle2") as UIToggle;
		mLbText = ScriptBinder.GetObject("LbText") as UILabel;
		mLbOperator = ScriptBinder.GetObject("LbOperator") as UILabel;
		mRedPoint = ScriptBinder.GetObject("RedPoint") as UnityEngine.GameObject;
	}
}
