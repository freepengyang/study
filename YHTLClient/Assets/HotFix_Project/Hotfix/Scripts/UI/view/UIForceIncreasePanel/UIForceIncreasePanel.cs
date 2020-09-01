using UnityEngine;

public class UIForceIncreasePanel : UIBasePanel
{
    private UIGridContainer m_grid;
    private UIGridContainer grid { get { return m_grid ? m_grid : (m_grid = Get<UIGridContainer>("content/buttonScrollView/despGrid")); } }
    
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    
    public override void Init()
    {
        base.Init();
        AddCollider();
    }
    
    public override void Show()
    {
        base.Show();
        grid.Bind<int,FroceIncreaseGrid>(CSForceIncreaseInfo.Instance.OpenFuncList, mPoolHandleManager);
    }

    protected override void OnDestroy()
    {
        grid.UnBind<FroceIncreaseGrid>();
        base.OnDestroy();
    }

}

public class FroceIncreaseGrid : UIBinder
{
    private UILabel value;
    private GameObject icon;
    private int gameModelId;
    private int tipsId;
    
    public override void Init(UIEventListener handle)
    {
        value = Get<UILabel>("value");
        icon = Get<GameObject>("icon");
        
        if(index == 0)
            CSEffectPlayMgr.Instance.ShowUIEffect(icon, "");
    }

    ForceIncreaseRedData increaseData;
    public override void Bind(object data)
    {
        increaseData = null;
        if (CSForceIncreaseInfo.Instance.ForceIncreaseDic.TryGetValue((int) data, out increaseData))
        {
            tipsId = increaseData.ClientTipsId;
            value.text = CSString.Format(tipsId);
            gameModelId = increaseData.GameModel;
            Handle.onClick = OnOpenPanel;
        }
    }

    private void OnOpenPanel(GameObject go)
    {
        if (tipsId == 1094)
        {
            UIManager.Instance.CreatePanel<UIBagPanel>(p=> 
            {
                (p as UIBagPanel).SelectWeaponZhuFu();
            });
        }
        else if (tipsId == 1838)
        {
            UIManager.Instance.CreatePanel<UIBagPanel>(p =>
            {
                (p as UIBagPanel).ReplaceBetterEquip();
            });
        }
        else
        {
            UtilityPanel.JumpToPanel(gameModelId,increaseData.actionCreate);
        }
        UIManager.Instance.ClosePanel<UIForceIncreasePanel>();
    }

    public override void OnDestroy()
    {
        if(icon != null)
            CSEffectPlayMgr.Instance.Recycle(icon);
    }
}