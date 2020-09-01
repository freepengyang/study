using System;
using System.Collections.Generic;
using UnityEngine;

public class UIRankingTabBinder : UIBinder
{
    public bool isSelect;
    public bool isOpen;
    public RankingType type;
    //当前子页签(只有选中时才有用)
    public RankingSubType curSubType;
    public Action<RankingType, bool> actionTab;
    public Action<RankingSubType> actionItem;
    private GameObject checkmark;
    private UIEventListener mainTempListener;
    private UIGridContainer grid_sub;
    private UILabel lb_name;
    private UILabel checkmarkName;
    private List<RankingType> rankingTypes;
    private Map<RankingType, List<RankingSubType>> mapRankingSubTypes;
    

    public override void Init(UIEventListener handle)
    {
        mainTempListener = handle.GetComponent<UIEventListener>();
        lb_name = Get<UILabel>("name");
        checkmark = Get<GameObject>("checkmark");
        checkmarkName = Get<UILabel>("name", checkmark.transform);
        grid_sub = Get<UIGridContainer>("grid_sub");
        
        mainTempListener.onClick = OnClickTab;
    }

    void OnClickTab(GameObject go)
    {
        actionTab?.Invoke(type, isOpen);
    }

    public override void Bind(object data)
    {
        rankingTypes = CSRankingInfo.Instance.RankingTypes;
        mapRankingSubTypes = CSRankingInfo.Instance.MapRankingSubTypes;
        RefreshUI();
    }

    
    // private List<UIRankingItemBinder> listRankingItemBinder;
    void RefreshUI()
    {
        checkmark.SetActive(isOpen);
        string[] stringsTabName = UtilityMainMath.StrToStrArr(CSString.Format(1156));
        if (stringsTabName.Length<4)
        {
            // Debug.Log("------------------排行榜大页签名字数量配置错误@刘轶");
        }

        for (int i = 0; i < rankingTypes.Count; i++)
        {
            if (rankingTypes[i]==type)
            {
                lb_name.text = stringsTabName[i];
                checkmarkName.text = stringsTabName[i];
                break;
            }
        }
        
        if (!isSelect||(isSelect&&!isOpen))
        {
            grid_sub.MaxCount = 0;
        }
        else
        {
            // if (listRankingItemBinder==null)
            // {
            //     listRankingItemBinder = new List<UIRankingItemBinder>();
            // }
            grid_sub.MaxCount = mapRankingSubTypes[type].Count;
            GameObject gp;
            for (int i = 0; i < grid_sub.MaxCount; i++)
            {
                gp = grid_sub.controlList[i];
                var eventHandle = UIEventListener.Get(gp);
                UIRankingItemBinder Binder;
                if(eventHandle.parameter == null)
                {
                    Binder = new UIRankingItemBinder();
                    Binder.Setup(eventHandle);
                }
                else
                {
                    Binder = eventHandle.parameter as UIRankingItemBinder;
                }
                // if (listRankingItemBinder.Count==i)
                // {
                //     UIRankingItemBinder binder = new UIRankingItemBinder();
                //     listRankingItemBinder.Add(binder);
                // }
                // var eventHandle = UIEventListener.Get(gp);
                // UIRankingItemBinder Binder = listRankingItemBinder[i]; 
                // Binder.Setup(eventHandle);
                Binder.type = type;
                Binder.subType = mapRankingSubTypes[type][i];
                Binder.isSelect = curSubType == Binder.subType;
                Binder.actionItem = actionItem;
                Binder.Bind(null);
            }
        }
    }

    public override void OnDestroy()
    {
        actionTab = null;
        actionItem = null;
        checkmark=null;
        mainTempListener = null;
        lb_name = null;
        checkmarkName = null;
        // listRankingItemBinder = null;
        grid_sub.UnBind<UIRankingItemBinder>();
        grid_sub = null;
        rankingTypes = null;
        mapRankingSubTypes = null;
    }
}
