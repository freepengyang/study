public partial class UIChallengePanel : UIBasePanel
{
	protected UIEventListener mbtn_challenge;
	protected UIEventListener mbtn_left;
	protected UIEventListener mbtn_right;
	protected UIEventListener mbtn_buffAdd;
	protected UIEventListener mbtn_help;
	protected UIEventListener mbtn_ranking;
	protected UIEventListener mbtn_reset;
	protected UILabel mlbLevelValue;
	protected UISprite mspLevelValue;
	protected UILabel mlbBloodValue;
	protected UIScrollView mScrollView;
	protected UITexture msp_bgMap;
	protected UIGridContainer mgrid_dian;
	protected UIGridContainer mgrid_frag;
	protected UISprite mspBloodValue;
	protected UILabel mresurgenceCount;
	protected UnityEngine.GameObject mbloodIcon;
	protected UILabel mlb_time;
	protected UnityEngine.GameObject mlevel;
	protected UnityEngine.GameObject mblood;
	protected UnityEngine.GameObject mresurgence;
	protected UnityEngine.GameObject mSprite;
	protected UnityEngine.GameObject mtimebg;
	protected UILabel mlb_resetBtn;
	protected UISprite msp_resetBtn;
	protected UISprite msp_challengeBtn;
	protected UILabel mlb_challengeBtn;
	protected override void _InitScriptBinder()
	{
		mbtn_challenge = ScriptBinder.GetObject("btn_challenge") as UIEventListener;
		mbtn_left = ScriptBinder.GetObject("btn_left") as UIEventListener;
		mbtn_right = ScriptBinder.GetObject("btn_right") as UIEventListener;
		mbtn_buffAdd = ScriptBinder.GetObject("btn_buffAdd") as UIEventListener;
		mbtn_help = ScriptBinder.GetObject("btn_help") as UIEventListener;
		mbtn_ranking = ScriptBinder.GetObject("btn_ranking") as UIEventListener;
		mbtn_reset = ScriptBinder.GetObject("btn_reset") as UIEventListener;
		mlbLevelValue = ScriptBinder.GetObject("lbLevelValue") as UILabel;
		mspLevelValue = ScriptBinder.GetObject("spLevelValue") as UISprite;
		mlbBloodValue = ScriptBinder.GetObject("lbBloodValue") as UILabel;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		msp_bgMap = ScriptBinder.GetObject("sp_bgMap") as UITexture;
		mgrid_dian = ScriptBinder.GetObject("grid_dian") as UIGridContainer;
		mgrid_frag = ScriptBinder.GetObject("grid_frag") as UIGridContainer;
		mspBloodValue = ScriptBinder.GetObject("spBloodValue") as UISprite;
		mresurgenceCount = ScriptBinder.GetObject("resurgenceCount") as UILabel;
		mbloodIcon = ScriptBinder.GetObject("bloodIcon") as UnityEngine.GameObject;
		mlb_time = ScriptBinder.GetObject("lb_time") as UILabel;
		mlevel = ScriptBinder.GetObject("level") as UnityEngine.GameObject;
		mblood = ScriptBinder.GetObject("blood") as UnityEngine.GameObject;
		mresurgence = ScriptBinder.GetObject("resurgence") as UnityEngine.GameObject;
		mSprite = ScriptBinder.GetObject("Sprite") as UnityEngine.GameObject;
		mtimebg = ScriptBinder.GetObject("timebg") as UnityEngine.GameObject;
		mlb_resetBtn = ScriptBinder.GetObject("lb_resetBtn") as UILabel;
		msp_resetBtn = ScriptBinder.GetObject("sp_resetBtn") as UISprite;
		msp_challengeBtn = ScriptBinder.GetObject("sp_challengeBtn") as UISprite;
		mlb_challengeBtn = ScriptBinder.GetObject("lb_challengeBtn") as UILabel;
	}
}
