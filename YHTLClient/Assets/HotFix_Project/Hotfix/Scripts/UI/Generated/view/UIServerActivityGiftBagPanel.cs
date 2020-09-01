public partial class UIServerActivityGiftBagPanel : UIBasePanel
{
	protected UIGridContainer mGrid_A;
	protected UnityEngine.GameObject mbanner8;
	protected UIGridContainer mGrid_B;
	protected UIScrollView mScrollView;
	protected UIWrapContent mWrap;
	protected UICenterOnChild mCenter;
	protected UIGridContainer mgrid_pageDot;
	protected override void _InitScriptBinder()
	{
		mGrid_A = ScriptBinder.GetObject("Grid_A") as UIGridContainer;
		mbanner8 = ScriptBinder.GetObject("banner8") as UnityEngine.GameObject;
		mGrid_B = ScriptBinder.GetObject("Grid_B") as UIGridContainer;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mWrap = ScriptBinder.GetObject("Wrap") as UIWrapContent;
		mCenter = ScriptBinder.GetObject("Center") as UICenterOnChild;
		mgrid_pageDot = ScriptBinder.GetObject("grid_pageDot") as UIGridContainer;
	}
}
