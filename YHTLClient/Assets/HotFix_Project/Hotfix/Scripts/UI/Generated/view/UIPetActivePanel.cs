public partial class UIPetActivePanel : UIBasePanel
{
	protected UnityEngine.GameObject mBGTexture;
	protected UILabel mPetName;
	protected UnityEngine.GameObject mClothModel;
	protected UIGridContainer mBasicGrid;
	protected UnityEngine.GameObject mDownIcon;
	protected UnityEngine.GameObject mWeaponModel;
	protected UnityEngine.GameObject mEffect;
	protected UIScrollView mScrAttView;
	protected CSInvoke mCSInvoke;
	protected UnityEngine.Transform mStartPos;
	protected override void _InitScriptBinder()
	{
		mBGTexture = ScriptBinder.GetObject("BGTexture") as UnityEngine.GameObject;
		mPetName = ScriptBinder.GetObject("PetName") as UILabel;
		mClothModel = ScriptBinder.GetObject("ClothModel") as UnityEngine.GameObject;
		mBasicGrid = ScriptBinder.GetObject("BasicGrid") as UIGridContainer;
		mDownIcon = ScriptBinder.GetObject("DownIcon") as UnityEngine.GameObject;
		mWeaponModel = ScriptBinder.GetObject("WeaponModel") as UnityEngine.GameObject;
		mEffect = ScriptBinder.GetObject("Effect") as UnityEngine.GameObject;
		mScrAttView = ScriptBinder.GetObject("ScrAttView") as UIScrollView;
		mCSInvoke = ScriptBinder.GetObject("CSInvoke") as CSInvoke;
		mStartPos = ScriptBinder.GetObject("StartPos") as UnityEngine.Transform;
	}
}
