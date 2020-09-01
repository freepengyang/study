public partial class UIPetLevelUpPromptPanel : UIBasePanel
{
	protected UIGridContainer mgrid_mustItems;
	protected UIGrid mgrid_luckItems;
	protected UISlider mslider_level;
	protected UnityEngine.GameObject mluckItem;
	protected UIWidget mType2;
	protected UITexture mTexbg;
	protected UILabel mlb_exp;
	protected UILabel mlb_lv;
	protected override void _InitScriptBinder()
	{
		mgrid_mustItems = ScriptBinder.GetObject("grid_mustItems") as UIGridContainer;
		mgrid_luckItems = ScriptBinder.GetObject("grid_luckItems") as UIGrid;
		mslider_level = ScriptBinder.GetObject("slider_level") as UISlider;
		mluckItem = ScriptBinder.GetObject("luckItem") as UnityEngine.GameObject;
		mType2 = ScriptBinder.GetObject("Type2") as UIWidget;
		mTexbg = ScriptBinder.GetObject("Texbg") as UITexture;
		mlb_exp = ScriptBinder.GetObject("lb_exp") as UILabel;
		mlb_lv = ScriptBinder.GetObject("lb_lv") as UILabel;
	}
}
