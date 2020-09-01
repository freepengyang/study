public partial class UIMiniMapPanel
{
	protected UILabel mlb_cityname;
	protected UnityEngine.GameObject mMiniMapTex;
	protected UILabel mlb_coordinate;
	protected UnityEngine.Transform mmainPlayer;
	protected UnityEngine.Transform mNpcPoints;
	protected UnityEngine.Transform mPlayerPoints;
	protected UnityEngine.Transform mMonsterPoints;
	protected UnityEngine.Transform mGravePoints;
	protected UIEventListener mbg;
	protected UnityEngine.GameObject mspr_itemPoint;
	protected override void _InitScriptBinder()
	{
		mlb_cityname = ScriptBinder.GetObject("lb_cityname") as UILabel;
		mMiniMapTex = ScriptBinder.GetObject("MiniMapTex") as UnityEngine.GameObject;
		mlb_coordinate = ScriptBinder.GetObject("lb_coordinate") as UILabel;
		mmainPlayer = ScriptBinder.GetObject("mainPlayer") as UnityEngine.Transform;
		mNpcPoints = ScriptBinder.GetObject("NpcPoints") as UnityEngine.Transform;
		mPlayerPoints = ScriptBinder.GetObject("PlayerPoints") as UnityEngine.Transform;
		mMonsterPoints = ScriptBinder.GetObject("MonsterPoints") as UnityEngine.Transform;
		mGravePoints = ScriptBinder.GetObject("GravePoints") as UnityEngine.Transform;
		mbg = ScriptBinder.GetObject("bg") as UIEventListener;
		mspr_itemPoint = ScriptBinder.GetObject("spr_itemPoint") as UnityEngine.GameObject;
	}
}
