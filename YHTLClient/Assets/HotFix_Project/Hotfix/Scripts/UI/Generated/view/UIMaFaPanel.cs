public partial class UIMaFaPanel : UIBasePanel
{
	protected UIEventListener mCloseBtn;
	protected UIGridContainer mRewardsGrid;
	protected UIScrollView mRewardsView;
	protected CSInvoke mCSInvoke;
	protected UILabel mLbTime;
	protected UISlider mSliderExp;
	protected UILabel mLbRate;
	protected UILabel mLbLayer;
	protected UIEventListener mBtnWant;
	protected UnityEngine.GameObject mBoxEffectObj;
	protected UIEventListener mBtnOpen;
	protected UILabel mBoxTip;
	protected UISprite mKeyIcon;
	protected UILabel mKeyValue;
	protected UIEventListener mBtnOneKey;
	protected UILabel mLbBoxName;
	protected UnityEngine.GameObject mObjLock;
	protected UIEventListener mBtnBuy;
	protected UIEventListener mBtnHelp;
	protected UnityEngine.GameObject mMaFaTitleBg;
	protected UnityEngine.GameObject mBoxRedPoint;
	protected UnityEngine.GameObject mOneKeyRedPoint;
	protected UIEventListener mAdvancedBox;
	protected UIEventListener mBtnAdd;
	protected UIWrapContent mGridWrap;
	protected override void _InitScriptBinder()
	{
		mCloseBtn = ScriptBinder.GetObject("CloseBtn") as UIEventListener;
		mRewardsGrid = ScriptBinder.GetObject("RewardsGrid") as UIGridContainer;
		mRewardsView = ScriptBinder.GetObject("RewardsView") as UIScrollView;
		mCSInvoke = ScriptBinder.GetObject("CSInvoke") as CSInvoke;
		mLbTime = ScriptBinder.GetObject("LbTime") as UILabel;
		mSliderExp = ScriptBinder.GetObject("SliderExp") as UISlider;
		mLbRate = ScriptBinder.GetObject("LbRate") as UILabel;
		mLbLayer = ScriptBinder.GetObject("LbLayer") as UILabel;
		mBtnWant = ScriptBinder.GetObject("BtnWant") as UIEventListener;
		mBoxEffectObj = ScriptBinder.GetObject("BoxEffectObj") as UnityEngine.GameObject;
		mBtnOpen = ScriptBinder.GetObject("BtnOpen") as UIEventListener;
		mBoxTip = ScriptBinder.GetObject("BoxTip") as UILabel;
		mKeyIcon = ScriptBinder.GetObject("KeyIcon") as UISprite;
		mKeyValue = ScriptBinder.GetObject("KeyValue") as UILabel;
		mBtnOneKey = ScriptBinder.GetObject("BtnOneKey") as UIEventListener;
		mLbBoxName = ScriptBinder.GetObject("LbBoxName") as UILabel;
		mObjLock = ScriptBinder.GetObject("ObjLock") as UnityEngine.GameObject;
		mBtnBuy = ScriptBinder.GetObject("BtnBuy") as UIEventListener;
		mBtnHelp = ScriptBinder.GetObject("BtnHelp") as UIEventListener;
		mMaFaTitleBg = ScriptBinder.GetObject("MaFaTitleBg") as UnityEngine.GameObject;
		mBoxRedPoint = ScriptBinder.GetObject("BoxRedPoint") as UnityEngine.GameObject;
		mOneKeyRedPoint = ScriptBinder.GetObject("OneKeyRedPoint") as UnityEngine.GameObject;
		mAdvancedBox = ScriptBinder.GetObject("AdvancedBox") as UIEventListener;
		mBtnAdd = ScriptBinder.GetObject("BtnAdd") as UIEventListener;
		mGridWrap = ScriptBinder.GetObject("GridWrap") as UIWrapContent;
	}
}
