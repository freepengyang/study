public partial class UIMailPanel : UIBasePanel
{
	protected UIEventListener mBtnOneKeyDelete;
	protected UIEventListener mBtnOneKeyAcquire;
	protected UIEventListener mBtnDelete;
	protected UIEventListener mBtnAcquire;
	protected UIGridBinderContainer mGridMailContainer;
	protected UIEventListener mBtnRecreate;
	protected UILabel mMailCount;
	protected UILabel mMailContent;
	protected UILabel mMailTheme;
	protected UnityEngine.GameObject mAllMailRedPoint;
	protected UnityEngine.GameObject mOneMailRedPoint;
	protected UIGrid mAwardParent;
	protected UIEventListener mBtnSendMail;
	protected UIScrollView mMailScrollView;
	protected UnityEngine.GameObject mAwardStatus;
	protected UnityEngine.GameObject mRightDesc;
	protected UnityEngine.GameObject mTexture;
	protected UnityEngine.GameObject mlArrow;
	protected UnityEngine.GameObject mrArrow;
	protected UIScrollView mScrollView;
	protected UIGrid mgrid_award;
	protected UIScrollView mContentScrollView;
	protected override void _InitScriptBinder()
	{
		mBtnOneKeyDelete = ScriptBinder.GetObject("BtnOneKeyDelete") as UIEventListener;
		mBtnOneKeyAcquire = ScriptBinder.GetObject("BtnOneKeyAcquire") as UIEventListener;
		mBtnDelete = ScriptBinder.GetObject("BtnDelete") as UIEventListener;
		mBtnAcquire = ScriptBinder.GetObject("BtnAcquire") as UIEventListener;
		mGridMailContainer = ScriptBinder.GetObject("GridMailContainer") as UIGridBinderContainer;
		mBtnRecreate = ScriptBinder.GetObject("BtnRecreate") as UIEventListener;
		mMailCount = ScriptBinder.GetObject("MailCount") as UILabel;
		mMailContent = ScriptBinder.GetObject("MailContent") as UILabel;
		mMailTheme = ScriptBinder.GetObject("MailTheme") as UILabel;
		mAllMailRedPoint = ScriptBinder.GetObject("AllMailRedPoint") as UnityEngine.GameObject;
		mOneMailRedPoint = ScriptBinder.GetObject("OneMailRedPoint") as UnityEngine.GameObject;
		mAwardParent = ScriptBinder.GetObject("AwardParent") as UIGrid;
		mBtnSendMail = ScriptBinder.GetObject("BtnSendMail") as UIEventListener;
		mMailScrollView = ScriptBinder.GetObject("MailScrollView") as UIScrollView;
		mAwardStatus = ScriptBinder.GetObject("AwardStatus") as UnityEngine.GameObject;
		mRightDesc = ScriptBinder.GetObject("RightDesc") as UnityEngine.GameObject;
		mTexture = ScriptBinder.GetObject("Texture") as UnityEngine.GameObject;
		mlArrow = ScriptBinder.GetObject("lArrow") as UnityEngine.GameObject;
		mrArrow = ScriptBinder.GetObject("rArrow") as UnityEngine.GameObject;
		mScrollView = ScriptBinder.GetObject("ScrollView") as UIScrollView;
		mgrid_award = ScriptBinder.GetObject("grid_award") as UIGrid;
		mContentScrollView = ScriptBinder.GetObject("ContentScrollView") as UIScrollView;
	}
}
