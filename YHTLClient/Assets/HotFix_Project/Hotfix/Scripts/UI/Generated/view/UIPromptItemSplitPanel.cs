public partial class UIPromptItemSplitPanel : UIBasePanel
{
	protected UILabel mlb_Title;
	protected UnityEngine.GameObject mbtn_close;
	protected UnityEngine.GameObject mbtn_left;
	protected UnityEngine.GameObject mbtn_right;
	protected UnityEngine.GameObject mbtn_shield;
	protected UnityEngine.GameObject mbtn_minus;
	protected UnityEngine.GameObject mbtn_add;
	protected UnityEngine.GameObject mobj_itemPar;
	protected UIInput minput_Num;
	protected override void _InitScriptBinder()
	{
		mlb_Title = ScriptBinder.GetObject("lb_Title") as UILabel;
		mbtn_close = ScriptBinder.GetObject("btn_close") as UnityEngine.GameObject;
		mbtn_left = ScriptBinder.GetObject("btn_left") as UnityEngine.GameObject;
		mbtn_right = ScriptBinder.GetObject("btn_right") as UnityEngine.GameObject;
		mbtn_shield = ScriptBinder.GetObject("btn_shield") as UnityEngine.GameObject;
		mbtn_minus = ScriptBinder.GetObject("btn_minus") as UnityEngine.GameObject;
		mbtn_add = ScriptBinder.GetObject("btn_add") as UnityEngine.GameObject;
		mobj_itemPar = ScriptBinder.GetObject("obj_itemPar") as UnityEngine.GameObject;
		minput_Num = ScriptBinder.GetObject("input_Num") as UIInput;
	}
}
