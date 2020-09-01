public partial class UIMapPanel
{
	protected UILabel mcityName;
	protected UnityEngine.GameObject mTexture;
	protected UnityEngine.Transform mmainPlayer;
	protected UnityEngine.GameObject mendPoint;
	protected UnityEngine.GameObject mPathPoint;
	protected UnityEngine.Transform mActMonsterPoints;
	protected UnityEngine.Transform mStaticPoints;
	protected UnityEngine.Transform mStaticBossPoints;
	protected UnityEngine.Transform mActPlayerPoints;
	protected UnityEngine.Transform mSpecialPlayerPoints;
	protected UnityEngine.GameObject mitemPoint;
	protected UnityEngine.GameObject mpathPoint;
	protected UILabel mposLabel;
	protected UILabel mendLabel;
	protected UIScrollView mScrollView;
	protected UITable mTable;
	protected UnityEngine.GameObject mmainTemp;
	protected UnityEngine.GameObject msubTemp;
	protected UnityEngine.GameObject msubCheckmark;
	protected TweenPosition mright;
	protected UIEventListener mbtn_random;
	protected UIEventListener mbtn_back;
	protected TweenRotation mbtn_show;
	protected UILabel mlbRandom;
	protected UILabel mlbBack;
	protected override void _InitScriptBinder()
	{
		mcityName = ScriptBinder.GetObject("cityName") as UILabel;
		mTexture = ScriptBinder.GetObject("Texture") as UnityEngine.GameObject;
		mmainPlayer = ScriptBinder.GetObject("mainPlayer") as UnityEngine.Transform;
		mendPoint = ScriptBinder.GetObject("endPoint") as UnityEngine.GameObject;
		mPathPoint = ScriptBinder.GetObject("PathPoint") as UnityEngine.GameObject;
		mActMonsterPoints = ScriptBinder.GetObject("ActMonsterPoints") as UnityEngine.Transform;
		mStaticPoints = ScriptBinder.GetObject("StaticPoints") as UnityEngine.Transform;
		mStaticBossPoints = ScriptBinder.GetObject("StaticBossPoints") as UnityEngine.Transform;
		mActPlayerPoints = ScriptBinder.GetObject("ActPlayerPoints") as UnityEngine.Transform;
		mSpecialPlayerPoints = ScriptBinder.GetObject("SpecialPlayerPoints") as UnityEngine.Transform;
		mitemPoint = ScriptBinder.GetObject("itemPoint") as UnityEngine.GameObject;
		mpathPoint = ScriptBinder.GetObject("pathPoint") as UnityEngine.GameObject;
		mposLabel = ScriptBinder.GetObject("posLabel") as UILabel;
		mendLabel = ScriptBinder.GetObject("endLabel") as UILabel;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mTable = ScriptBinder.GetObject("Table") as UITable;
		mmainTemp = ScriptBinder.GetObject("mainTemp") as UnityEngine.GameObject;
		msubTemp = ScriptBinder.GetObject("subTemp") as UnityEngine.GameObject;
		msubCheckmark = ScriptBinder.GetObject("subCheckmark") as UnityEngine.GameObject;
		mright = ScriptBinder.GetObject("right") as TweenPosition;
		mbtn_random = ScriptBinder.GetObject("btn_random") as UIEventListener;
		mbtn_back = ScriptBinder.GetObject("btn_back") as UIEventListener;
		mbtn_show = ScriptBinder.GetObject("btn_show") as TweenRotation;
		mlbRandom = ScriptBinder.GetObject("lbRandom") as UILabel;
		mlbBack = ScriptBinder.GetObject("lbBack") as UILabel;
	}
}
