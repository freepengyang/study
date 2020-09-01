public partial class UIRankingPanel : UIBasePanel
{
	protected UIGridContainer mgrid_tab;
	protected UILabel mlb1;
	protected UILabel mlb2;
	protected UILabel mlb3;
	protected UILabel mlb4;
	protected UIGridContainer mgrid_info;
	protected UILabel mlb_myrank;
	protected UIEventListener mbtn_strength;
	protected UITable muitable_tab;
	protected UIEventListener mbtn_upGrade;
	protected UIScrollView mScrollView_Info;
	protected UIWrapContent mwarp_info;
	protected override void _InitScriptBinder()
	{
		mgrid_tab = ScriptBinder.GetObject("grid_tab") as UIGridContainer;
		mlb1 = ScriptBinder.GetObject("lb1") as UILabel;
		mlb2 = ScriptBinder.GetObject("lb2") as UILabel;
		mlb3 = ScriptBinder.GetObject("lb3") as UILabel;
		mlb4 = ScriptBinder.GetObject("lb4") as UILabel;
		mgrid_info = ScriptBinder.GetObject("grid_info") as UIGridContainer;
		mlb_myrank = ScriptBinder.GetObject("lb_myrank") as UILabel;
		mbtn_strength = ScriptBinder.GetObject("btn_strength") as UIEventListener;
		muitable_tab = ScriptBinder.GetObject("uitable_tab") as UITable;
		mbtn_upGrade = ScriptBinder.GetObject("btn_upGrade") as UIEventListener;
		mScrollView_Info = ScriptBinder.GetObject("ScrollView_Info") as UIScrollView;
		mwarp_info = ScriptBinder.GetObject("warp_info") as UIWrapContent;
	}
}
