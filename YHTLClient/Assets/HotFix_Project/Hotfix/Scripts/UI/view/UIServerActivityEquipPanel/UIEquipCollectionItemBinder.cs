using System.Collections;
using System.Collections.Generic;
using activity;
using TABLE;
using UnityEngine;

public class UIEquipCollectionItemBinderData
{
    public int goalId = 0; //表头id
    public int count = 0; //当前进度
    public bool reward = false; //false未领奖,true已领奖
    // public int index = 0; //当前索引
}

public class UIEquipCollectionItemBinder : UIBinder
{
    private UILabel lb_name;
    private UIGridContainer grid_itemBase;
    private UIEventListener btn_receive;
    private UIEventListener btn_get;
    private GameObject sp_complete;
    private GameObject effect;

    private UIEquipCollectionItemBinderData itemBinderData;
    
    Dictionary<int, int> rewardDic = new Dictionary<int, int>();
    ILBetterList<ILBetterList<int>> listReward = new ILBetterList<ILBetterList<int>>();
    private List<UIItemBase> listItemBases = new List<UIItemBase>();

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        grid_itemBase = Get<UIGridContainer>("grid_itemBase");
        btn_receive = Get<UIEventListener>("btn_receive");
        btn_get = Get<UIEventListener>("btn_get");
        sp_complete = Get<GameObject>("sp_complete");
        effect = Get<GameObject>("effect17903", btn_receive.transform);

        btn_receive.onClick = OnClickReceive;
        btn_get.onClick = OnClickGet;
        
        CSEffectPlayMgr.Instance.ShowUIEffect(effect, 17903);
    }

    public override void Bind(object data)
    {
        if (data == null) return;
        itemBinderData = data as UIEquipCollectionItemBinderData;
        RefreshUI();
    }

    void RefreshUI()
    {
        List<int> listInt =
            UtilityMainMath.SplitStringToIntList(
                SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardTermNum(itemBinderData.goalId));
        if (listInt.Count != 4) return;

        int maxCount = listInt[0];

        string[] stringsQuality = UtilityMainMath.StrToStrArr(CSString.Format(1104));
        string quality = stringsQuality[listInt[2] - 1];
        
        string color = itemBinderData.count >= maxCount
            ? UtilityColor.GetColorString(ColorType.Green)
            : UtilityColor.GetColorString(ColorType.Red);
        
        lb_name.text = CSString.Format(1103, maxCount, listInt[3], quality, $"{color}({itemBinderData.count}", $"{maxCount})[-]");
        if (itemBinderData.reward) //已领奖
        {
            sp_complete.SetActive(true);
            btn_get.gameObject.SetActive(false);
            btn_receive.gameObject.SetActive(false);
        }
        else
        {
            btn_get.gameObject.SetActive(maxCount > itemBinderData.count);
            btn_receive.gameObject.SetActive(maxCount <= itemBinderData.count);
        }
        
        rewardDic.Clear();
        BoxTableManager.Instance.GetBoxAwardById(
            SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardReward(itemBinderData.goalId), rewardDic);
        
        int index = 0;
        if (rewardDic.Count > 0)
        {
            for (var it = rewardDic.GetEnumerator(); it.MoveNext();)
            {
                if (listReward.Count<=index)
                    listReward.Add(new ILBetterList<int>());
                ILBetterList<int> list = listReward[index];
                list.Clear();
                list.Add(it.Current.Key);
                list.Add(it.Current.Value);
                index++;
            }
        }

        grid_itemBase.MaxCount = listReward.Count;
        GameObject gp;
        for (int i = 0; i < grid_itemBase.MaxCount; i++)
        {
            gp = grid_itemBase.controlList[i];
            if (listItemBases.Count<=i)
                listItemBases.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, gp.transform, itemSize.Size64));
            UIItemBase itemBase = listItemBases[i];
            itemBase.Refresh(listReward[i][0]);
            itemBase.SetCount(listReward[i][1]);
        }
    }

    /// <summary>
    /// 领取
    /// </summary>
    /// <param name="go"></param>
    void OnClickReceive(GameObject go)
    {
        Net.ReqSpecialActivityRewardMessage(10102, itemBinderData.goalId);
    }

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="go"></param>
    void OnClickGet(GameObject go)
    {
        // UtilityPanel.JumpToPanel(10420);
        // UIManager.Instance.ClosePanel<UIServerActivityPanel>();
        UISprite sp_btn = go.GetComponent<UISprite>();
        if (sp_btn!=null)
        {
            string wayStr = SundryTableManager.Instance.GetSundryEffect(1079);
            UtilityPanel.ShowCompleteWayWithSelfAdapt(wayStr, sp_btn);   
        }
    }

    public override void OnDestroy()
    {
        UIItemManager.Instance.RecycleItemsFormMediator(listItemBases);
        CSEffectPlayMgr.Instance.Recycle(effect);
        lb_name = null;
        grid_itemBase = null;
        btn_receive = null;
        btn_get = null;
        sp_complete = null;
        itemBinderData = null;
        rewardDic = null;
        listReward = null;
    }
}