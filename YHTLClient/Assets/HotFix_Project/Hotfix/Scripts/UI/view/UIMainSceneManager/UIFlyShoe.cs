using UnityEngine;

public class UIFlyShoe : UIBase
{
    private GameObject _autoFind;

    public GameObject AutoFind
    {
        get { return _autoFind ? _autoFind : _autoFind = Get<GameObject>("autoFind"); }
    }
    
    private GameObject _autoFight;

    public GameObject AutoFight
    {
        get { return _autoFight ? _autoFight : _autoFight = Get<GameObject>("autoFight"); }
    }
    
    private GameObject _autoEscort;

    public GameObject AutoEscort
    {
        get { return _autoEscort ? _autoEscort : _autoEscort = Get<GameObject>("autoEscort"); }
    }
    
    private GameObject _flyShoes;

    public GameObject FlyShoes
    {
        get { return _flyShoes ? _flyShoes : _flyShoes = Get<GameObject>("flyShoes"); }
    }

    
    
    
    public override void Init()
    {
        base.Init();
        UIEventListener.Get(FlyShoes).onClick = OnUseFlyShoesClick;
        
        mClientEvent.AddEvent(CEvent.PlayerAutoActionChange, UpdateFindState);
    }



    private void CancelPathFind(uint id, object data)
    {
        if(AutoFind.activeSelf) AutoFind.SetActive(false);
        if(AutoFight.activeSelf) AutoFight.SetActive(false);
        //if(AutoEscort.activeSelf) AutoEscort.SetActive(false);
        if(FlyShoes.activeSelf) FlyShoes.SetActive(false);
    }
    
    private void OnUseFlyShoesClick(GameObject go)
    {
        if(CSResourceManager.Instance.IsChangingScene) return;
        if(!FlyShoes.activeSelf) return;
        
    }

    private void UpdateFindState(uint id, object data)
    {
        CancelPathFind(0, null);
        
        switch (CSPlayerAutoActionInfo.Instance.AutoAction)
        {
            case PlayerAutoAction.AutoFight:
                AutoFight.SetActive(true);
                break;
            case PlayerAutoAction.AutoFind:
                AutoFind.SetActive(true);
                break;
        }
    }
}