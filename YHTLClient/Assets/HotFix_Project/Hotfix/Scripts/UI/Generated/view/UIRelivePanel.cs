public partial class UIRelivePanel : UIBasePanel
{
	protected UIEventListener mbtn_close;
	protected UILabel mlb_title;
	protected UIEventListener mbtn_inplace;
	protected UIEventListener mbtn_bornpoint;
	protected UILabel mlb_inplace;
	protected UnityEngine.GameObject mobj_yuanbao;
	protected UILabel mlb_num_yuanbao;
	protected UILabel mlb_bornpoint;
	protected UnityEngine.GameObject mobj_mask;
	protected UnityEngine.GameObject mobj_auto_relive;
	protected UILabel mlb_time;
	protected UILabel mlb_place_relive;
	protected UnityEngine.GameObject mgridbtns;
	protected UnityEngine.GameObject mbgtitle;
	protected UIEventListener mbtn_instance;
	protected UILabel mlb_instance;
	protected UIEventListener mbtn_redName;
	protected UILabel mlb_redName;
	protected override void _InitScriptBinder()
	{
		mbtn_close = ScriptBinder.GetObject("btn_close") as UIEventListener;
		mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
		mbtn_inplace = ScriptBinder.GetObject("btn_inplace") as UIEventListener;
		mbtn_bornpoint = ScriptBinder.GetObject("btn_bornpoint") as UIEventListener;
		mlb_inplace = ScriptBinder.GetObject("lb_inplace") as UILabel;
		mobj_yuanbao = ScriptBinder.GetObject("obj_yuanbao") as UnityEngine.GameObject;
		mlb_num_yuanbao = ScriptBinder.GetObject("lb_num_yuanbao") as UILabel;
		mlb_bornpoint = ScriptBinder.GetObject("lb_bornpoint") as UILabel;
		mobj_mask = ScriptBinder.GetObject("obj_mask") as UnityEngine.GameObject;
		mobj_auto_relive = ScriptBinder.GetObject("obj_auto_relive") as UnityEngine.GameObject;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlb_place_relive = ScriptBinder.GetObject("lb_place_relive") as UILabel;
		mgridbtns = ScriptBinder.GetObject("gridbtns") as UnityEngine.GameObject;
		mbgtitle = ScriptBinder.GetObject("bgtitle") as UnityEngine.GameObject;
		mbtn_instance = ScriptBinder.GetObject("btn_instance") as UIEventListener;
		mlb_instance = ScriptBinder.GetObject("lb_instance") as UILabel;
		mbtn_redName = ScriptBinder.GetObject("btn_redName") as UIEventListener;
		mlb_redName = ScriptBinder.GetObject("lb_redName") as UILabel;
	}
}
