public partial class UICompoundPanel : UIBasePanel
{
	protected UILabel mlb_before;
	protected UILabel mlb_after;
	protected UnityEngine.GameObject mcompound_bg1;
	protected UnityEngine.GameObject mItemBase1;
	protected UnityEngine.GameObject mcompound_bg2;
	protected UnityEngine.GameObject mItemBase2;
	protected UISprite msp_icon;
	protected UILabel mlb_value;
	protected UIEventListener mbtn_add;
	protected UIEventListener mbtn_help;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_compound;
	protected UIGridContainer mgrid_table;
	protected UIScrollView mScrollView_table;
	protected UnityEngine.GameObject mAdditionalNeed;
	protected UnityEngine.GameObject mUIItemBarPrefab;
	protected UnityEngine.GameObject mItemBaseNeed;
	protected UnityEngine.GameObject mresidentEffect;
	protected UIGridContainer mgrid_sub;
	protected UIEventListener mbtn_sp;
	protected override void _InitScriptBinder()
	{
		mlb_before = ScriptBinder.GetObject("lb_before") as UILabel;
		mlb_after = ScriptBinder.GetObject("lb_after") as UILabel;
		mcompound_bg1 = ScriptBinder.GetObject("compound_bg1") as UnityEngine.GameObject;
		mItemBase1 = ScriptBinder.GetObject("ItemBase1") as UnityEngine.GameObject;
		mcompound_bg2 = ScriptBinder.GetObject("compound_bg2") as UnityEngine.GameObject;
		mItemBase2 = ScriptBinder.GetObject("ItemBase2") as UnityEngine.GameObject;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mlb_value = ScriptBinder.GetObject("lb_value") as UILabel;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UIEventListener;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_compound = ScriptBinder.GetObject("btn_compound") as UIEventListener;
		mgrid_table = ScriptBinder.GetObject("grid_table") as UIGridContainer;
		mScrollView_table = ScriptBinder.GetObject("ScrollView_table") as UIScrollView;
		mAdditionalNeed = ScriptBinder.GetObject("AdditionalNeed") as UnityEngine.GameObject;
		mUIItemBarPrefab = ScriptBinder.GetObject("UIItemBarPrefab") as UnityEngine.GameObject;
		mItemBaseNeed = ScriptBinder.GetObject("ItemBaseNeed") as UnityEngine.GameObject;
		mresidentEffect = ScriptBinder.GetObject("residentEffect") as UnityEngine.GameObject;
		mgrid_sub = ScriptBinder.GetObject("grid_sub") as UIGridContainer;
		mbtn_sp = ScriptBinder.GetObject("btn_sp") as UIEventListener;
	}
}
