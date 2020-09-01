public partial class UINPCDialogPanel 
{
	protected UILabel mlb_title;
	protected UIScrollView mdescriptionScrollView;
	protected UILabel mlb_say;
	protected UILabel mlb_limited;
	protected UILabel mlb_taskName;
	protected UILabel mlb_taskDesp;
	protected UnityEngine.GameObject mexpContainer;
	protected UILabel mlb_expkey;
	protected UILabel mlb_expvalue;
	protected UnityEngine.GameObject msilverContainer;
	protected UILabel mlb_silverkey;
	protected UILabel mlb_silvervalue;
	protected UIScrollView msrc_AwardList;
	protected UIGrid mgrid_itemGrid;
	protected UIScrollView mbuttonScrollView;
	protected UIGridContainer mgrid_desp;
	protected UnityEngine.GameObject mbtnClose;
	protected UnityEngine.GameObject mbtntask;
	protected UILabel mlb_taskValue;
	protected UnityEngine.GameObject mdesTask;
	protected override void _InitScriptBinder()
	{
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mdescriptionScrollView = ScriptBinder.GetObject("descriptionScrollView") as UIScrollView;
		mlb_say = ScriptBinder.GetObject("lb_say") as UILabel;
		mlb_limited = ScriptBinder.GetObject("lb_limited") as UILabel;
		mlb_taskName = ScriptBinder.GetObject("lb_taskName") as UILabel;
		mlb_taskDesp = ScriptBinder.GetObject("lb_taskDesp") as UILabel;
		mexpContainer = ScriptBinder.GetObject("expContainer") as UnityEngine.GameObject;
		mlb_expkey = ScriptBinder.GetObject("lb_expkey") as UILabel;
		mlb_expvalue = ScriptBinder.GetObject("lb_expvalue") as UILabel;
		msilverContainer = ScriptBinder.GetObject("silverContainer") as UnityEngine.GameObject;
		mlb_silverkey = ScriptBinder.GetObject("lb_silverkey") as UILabel;
		mlb_silvervalue = ScriptBinder.GetObject("lb_silvervalue") as UILabel;
		msrc_AwardList = ScriptBinder.GetObject("src_AwardList") as UIScrollView;
		mgrid_itemGrid = ScriptBinder.GetObject("grid_itemGrid") as UIGrid;
		mbuttonScrollView = ScriptBinder.GetObject("buttonScrollView") as UIScrollView;
		mgrid_desp = ScriptBinder.GetObject("grid_desp") as UIGridContainer;
		mbtnClose = ScriptBinder.GetObject("btnClose") as UnityEngine.GameObject;
		mbtntask = ScriptBinder.GetObject("btntask") as UnityEngine.GameObject;
		mlb_taskValue = ScriptBinder.GetObject("lb_taskValue") as UILabel;
		mdesTask = ScriptBinder.GetObject("desTask") as UnityEngine.GameObject;
	}
}
