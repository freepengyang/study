public partial class UIHandBookCombinedPanel : UIBasePanel
{
	protected UnityEngine.GameObject mHandBookSetupHandle;
	protected UIEventListener mBtnClose;
	protected UIToggle mTogSetup;
	protected UIGrid mToggleGroup;
	protected UnityEngine.GameObject mGridAttributes;
	protected UIGridContainer mGridCards;
	protected UIToggle mTogPackage;
	protected UnityEngine.GameObject mHandBookPackagePanel;
	protected UnityEngine.GameObject mHandBookUpgradePanel;
	protected UIToggle mTogUpgrade;
	protected UnityEngine.GameObject mHandBookMergePanel;
	protected UIToggle mTogMerge;
	protected UnityEngine.GameObject mUpgradeQualityRedPoint;
	protected UnityEngine.GameObject mUpgradeLevelRedPoint;
	protected UnityEngine.GameObject mSetupRedPoint;
	protected UnityEngine.GameObject mCardPackageRedPoint;
	protected UIToggle mTogHandBook;
	protected UnityEngine.GameObject mUIHandBookMarkPanel;
	protected override void _InitScriptBinder()
	{
		mHandBookSetupHandle = ScriptBinder.GetObject("HandBookSetupHandle") as UnityEngine.GameObject;
		mBtnClose = ScriptBinder.GetObject("BtnClose") as UIEventListener;
		mTogSetup = ScriptBinder.GetObject("TogSetup") as UIToggle;
		mToggleGroup = ScriptBinder.GetObject("ToggleGroup") as UIGrid;
		mGridAttributes = ScriptBinder.GetObject("GridAttributes") as UnityEngine.GameObject;
		mGridCards = ScriptBinder.GetObject("GridCards") as UIGridContainer;
		mTogPackage = ScriptBinder.GetObject("TogPackage") as UIToggle;
		mHandBookPackagePanel = ScriptBinder.GetObject("HandBookPackagePanel") as UnityEngine.GameObject;
		mHandBookUpgradePanel = ScriptBinder.GetObject("HandBookUpgradePanel") as UnityEngine.GameObject;
		mTogUpgrade = ScriptBinder.GetObject("TogUpgrade") as UIToggle;
		mHandBookMergePanel = ScriptBinder.GetObject("HandBookMergePanel") as UnityEngine.GameObject;
		mTogMerge = ScriptBinder.GetObject("TogMerge") as UIToggle;
		mUpgradeQualityRedPoint = ScriptBinder.GetObject("UpgradeQualityRedPoint") as UnityEngine.GameObject;
		mUpgradeLevelRedPoint = ScriptBinder.GetObject("UpgradeLevelRedPoint") as UnityEngine.GameObject;
		mSetupRedPoint = ScriptBinder.GetObject("SetupRedPoint") as UnityEngine.GameObject;
		mCardPackageRedPoint = ScriptBinder.GetObject("CardPackageRedPoint") as UnityEngine.GameObject;
		mTogHandBook = ScriptBinder.GetObject("TogHandBook") as UIToggle;
		mUIHandBookMarkPanel = ScriptBinder.GetObject("UIHandBookMarkPanel") as UnityEngine.GameObject;
	}
}
