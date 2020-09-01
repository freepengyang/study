public partial class UINostalgiaTipPanel : UITipsBase
{
	protected UIGridContainer mdespGrid;
	protected UnityEngine.GameObject msuitTitle;
	protected UnityEngine.GameObject msubTable;
	protected UnityEngine.GameObject mtitlePart;
	protected UIGridContainer mSubBtns;
	protected UILabel mlb_title;
	protected UILabel mlb_fightPower;
	protected UILabel mlb_lv;
	protected UILabel mlb_job;
	protected UnityEngine.GameObject mitem;
	protected UISprite msp_use;
	protected UITable mTable;
	protected UISprite mbg;
	protected override void _InitScriptBinder()
	{
		mdespGrid = ScriptBinder.GetObject("despGrid") as UIGridContainer;
		msuitTitle = ScriptBinder.GetObject("suitTitle") as UnityEngine.GameObject;
		msubTable = ScriptBinder.GetObject("subTable") as UnityEngine.GameObject;
		mtitlePart = ScriptBinder.GetObject("titlePart") as UnityEngine.GameObject;
		mSubBtns = ScriptBinder.GetObject("SubBtns") as UIGridContainer;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mlb_fightPower = ScriptBinder.GetObject("lb_fightPower") as UILabel;
		mlb_lv = ScriptBinder.GetObject("lb_lv") as UILabel;
		mlb_job = ScriptBinder.GetObject("lb_job") as UILabel;
		mitem = ScriptBinder.GetObject("item") as UnityEngine.GameObject;
		msp_use = ScriptBinder.GetObject("sp_use") as UISprite;
		mTable = ScriptBinder.GetObject("Table") as UITable;
		mbg = ScriptBinder.GetObject("bg") as UISprite;
	}
}
