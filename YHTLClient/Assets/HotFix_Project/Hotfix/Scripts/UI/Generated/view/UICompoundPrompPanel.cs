public partial class UICompoundPrompPanel : UIBasePanel
{
	protected UnityEngine.GameObject mItemBase1;
	protected UnityEngine.GameObject mItemBase2;
	protected UIInput mlb_inputvalue;
	protected UIEventListener mbtn_minus;
	protected UIEventListener mbtn_add;
	protected UILabel mlb_value;
	protected UIEventListener mbtn_addMoney;
	protected UISprite msp_icon;
	protected UIEventListener mbtn_compound;
	protected UIEventListener mbtn_close;
	protected UIEventListener mbtn_bg;
	protected UnityEngine.GameObject mAdditionalNeed;
	protected UnityEngine.GameObject mUIItemBarPrefab;
	protected UnityEngine.GameObject mItemBaseNeed;
	protected UISprite msp_bg;
	protected UIEventListener mbtn_sp;
	protected override void _InitScriptBinder()
	{
		mItemBase1 = ScriptBinder.GetObject("ItemBase1") as UnityEngine.GameObject;
		mItemBase2 = ScriptBinder.GetObject("ItemBase2") as UnityEngine.GameObject;
		mlb_inputvalue = ScriptBinder.GetObject("lb_inputvalue") as UIInput;
		mbtn_minus = ScriptBinder.GetObject("btn_minus") as UIEventListener;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UIEventListener;
		mlb_value = ScriptBinder.GetObject("lb_value") as UILabel;
		mbtn_addMoney = ScriptBinder.GetObject("btn_addMoney") as UIEventListener;
		msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
		mbtn_compound = ScriptBinder.GetObject("btn_compound") as UIEventListener;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mbtn_bg = ScriptBinder.GetObject("btn_bg") as UIEventListener;
		mAdditionalNeed = ScriptBinder.GetObject("AdditionalNeed") as UnityEngine.GameObject;
		mUIItemBarPrefab = ScriptBinder.GetObject("UIItemBarPrefab") as UnityEngine.GameObject;
		mItemBaseNeed = ScriptBinder.GetObject("ItemBaseNeed") as UnityEngine.GameObject;
		msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
		mbtn_sp = ScriptBinder.GetObject("btn_sp") as UIEventListener;
	}
}
