public partial class UIPearlSkillslotPanel : UIBasePanel
{
	protected UIGridContainer mgrid_skillslot;
	protected UIEventListener mbtn_rule;
	protected UIEventListener mbtn_preview;
	protected UnityEngine.GameObject mcolRefresh;
	protected UnityEngine.GameObject mcolInjection;
	protected UISprite msp_itemicon1;
	protected UILabel mlb_itemName1;
	protected UILabel mlb_content1;
	protected UILabel mlb_skillCD1;
	protected UISprite msp_itemicon2;
	protected UILabel mlb_itemName2;
	protected UnityEngine.GameObject mbg1;
	protected UnityEngine.GameObject mbg2;
	protected UILabel mlb_content2;
	protected UILabel mlb_skillCD2;
	protected UnityEngine.GameObject mlb_hint2;
	protected UnityEngine.GameObject mgrid_btns;
	protected UIEventListener mbtn_reinject;
	protected UIEventListener mbtn_replace;
	protected UIEventListener mbtn_refresh;
	protected UnityEngine.GameObject mbg0;
	protected UIEventListener mbtn_inject;
	protected UILabel mlb_value;
	protected UIEventListener mbtn_add;
	protected UnityEngine.GameObject mUIItemBarPrefab;
	protected UnityEngine.GameObject mUIItemBarPrefab1;
	protected UISprite msp_icon1;
	protected UISprite msp_icon;
	protected UILabel mlb_value1;
	protected UIEventListener mbtn_add1;
	protected UIGrid mgrid_UIItemBarPrefab;
	protected override void _InitScriptBinder()
	{
		mgrid_skillslot = ScriptBinder.GetObject("grid_skillslot") as UIGridContainer;
		mbtn_rule = ScriptBinder.GetObject("btn_rule") as UIEventListener;
		mbtn_preview = ScriptBinder.GetObject("btn_preview") as UIEventListener;
		mcolRefresh = ScriptBinder.GetObject("colRefresh") as UnityEngine.GameObject;
		mcolInjection = ScriptBinder.GetObject("colInjection") as UnityEngine.GameObject;
		msp_itemicon1 = ScriptBinder.GetObject("sp_itemicon1") as UISprite;
		mlb_itemName1 = ScriptBinder.GetObject("lb_itemName1") as UILabel;
		mlb_content1 = ScriptBinder.GetObject("lb_content1") as UILabel;
		mlb_skillCD1 = ScriptBinder.GetObject("lb_skillCD1") as UILabel;
		msp_itemicon2 = ScriptBinder.GetObject("sp_itemicon2") as UISprite;
		mlb_itemName2 = ScriptBinder.GetObject("lb_itemName2") as UILabel;
		mbg1 = ScriptBinder.GetObject("bg1") as UnityEngine.GameObject;
		mbg2 = ScriptBinder.GetObject("bg2") as UnityEngine.GameObject;
		mlb_content2 = ScriptBinder.GetObject("lb_content2") as UILabel;
		mlb_skillCD2 = ScriptBinder.GetObject("lb_skillCD2") as UILabel;
		mlb_hint2 = ScriptBinder.GetObject("lb_hint2") as UnityEngine.GameObject;
		mgrid_btns = ScriptBinder.GetObject("grid_btns") as UnityEngine.GameObject;
		mbtn_reinject = ScriptBinder.GetObject("btn_reinject") as UIEventListener;
		mbtn_replace = ScriptBinder.GetObject("btn_replace") as UIEventListener;
		mbtn_refresh = ScriptBinder.GetObject("btn_refresh") as UIEventListener;
		mbg0 = ScriptBinder.GetObject("bg0") as UnityEngine.GameObject;
		mbtn_inject = ScriptBinder.GetObject("btn_inject") as UIEventListener;
		mlb_value = ScriptBinder.GetObject("lb_value") as UILabel;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UIEventListener;
		mUIItemBarPrefab = ScriptBinder.GetObject("UIItemBarPrefab") as UnityEngine.GameObject;
		mUIItemBarPrefab1 = ScriptBinder.GetObject("UIItemBarPrefab1") as UnityEngine.GameObject;
		msp_icon1 = ScriptBinder.GetObject("sp_icon1") as UISprite;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mlb_value1 = ScriptBinder.GetObject("lb_value1") as UILabel;
		mbtn_add1 = ScriptBinder.GetObject("btn_add1") as UIEventListener;
		mgrid_UIItemBarPrefab = ScriptBinder.GetObject("grid_UIItemBarPrefab") as UIGrid;
	}
}
