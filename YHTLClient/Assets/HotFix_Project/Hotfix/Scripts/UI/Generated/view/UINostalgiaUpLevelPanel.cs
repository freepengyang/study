public partial class UINostalgiaUpLevelPanel : UIBasePanel
{
	protected UIScrollView mBagEquip;
	protected UnityEngine.GameObject mbtn_equip;
	protected UnityEngine.GameObject mbtn_bag;
	protected UnityEngine.GameObject mItemList;
	protected UnityEngine.GameObject mbtn_up;
	protected UILabel mlb_hint;
	protected UnityEngine.GameObject mmoreEquip;
	protected UIGridContainer mwrap_page;
	protected UILabel mlb_name;
	protected UnityEngine.GameObject mstar;
	protected UISprite msp_suit;
	protected UnityEngine.GameObject mlb_max;
	protected UnityEngine.GameObject mlb_empty;
	protected UnityEngine.GameObject mnostalgia_bg;
	protected UIGridContainer mwrap_equip;
	protected UIScrollView mEquipList;
	protected UnityEngine.GameObject meffect;
	protected UIScrollBar mscollbar;
	protected override void _InitScriptBinder()
	{
		mBagEquip = ScriptBinder.GetObject("BagEquip") as UIScrollView;
		mbtn_equip = ScriptBinder.GetObject("btn_equip") as UnityEngine.GameObject;
		mbtn_bag = ScriptBinder.GetObject("btn_bag") as UnityEngine.GameObject;
		mItemList = ScriptBinder.GetObject("ItemList") as UnityEngine.GameObject;
		mbtn_up = ScriptBinder.GetObject("btn_up") as UnityEngine.GameObject;
		mlb_hint = ScriptBinder.GetObject("lb_hint") as UILabel;
		mmoreEquip = ScriptBinder.GetObject("moreEquip") as UnityEngine.GameObject;
		mwrap_page = ScriptBinder.GetObject("wrap_page") as UIGridContainer;
		mlb_name = ScriptBinder.GetObject("lb_name") as UILabel;
		mstar = ScriptBinder.GetObject("star") as UnityEngine.GameObject;
		msp_suit = ScriptBinder.GetObject("sp_suit") as UISprite;
		mlb_max = ScriptBinder.GetObject("lb_max") as UnityEngine.GameObject;
		mlb_empty = ScriptBinder.GetObject("lb_empty") as UnityEngine.GameObject;
		mnostalgia_bg = ScriptBinder.GetObject("nostalgia_bg") as UnityEngine.GameObject;
		mwrap_equip = ScriptBinder.GetObject("wrap_equip") as UIGridContainer;
		mEquipList = ScriptBinder.GetObject("EquipList") as UIScrollView;
		meffect = ScriptBinder.GetObject("effect") as UnityEngine.GameObject;
		mscollbar = ScriptBinder.GetObject("scollbar") as UIScrollBar;
	}
}
