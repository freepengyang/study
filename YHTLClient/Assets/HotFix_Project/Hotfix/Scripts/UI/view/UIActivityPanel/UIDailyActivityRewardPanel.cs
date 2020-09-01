using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIDailyActivityRewardPanel : UIBasePanel
{
    private Dictionary<int, int> BoxReward;
    public override void Init()
    {
        base.Init();
        AddCollider();
        BoxReward = mPoolHandleManager.GetSystemClass<Dictionary<int, int>>();
    }

    List<UIItemBase> uiItemlist = null;
    public void OpenPanel(int boxid) {
        UIEventListener.Get(mbtn_ok).onClick = SureClick;
        if (uiItemlist != null)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(uiItemlist);
            uiItemlist.Clear();
        }
        BoxReward.Clear();
        BoxTableManager.Instance.GetBoxAwardById(boxid, BoxReward);
        Utility.GetItemByBoxid(BoxReward, mgrid_reward,ref uiItemlist);
        //Map<int, int> BoxReward = new Map<int, int>();
        //BoxTableManager.Instance.GetBoxAwardById(boxid, BoxReward);
        ////Debug.Log("BoxReward.Count" + BoxReward.Count);
        //if (BoxReward.Count > 0)
        //{
        //    mgrid_reward.MaxCount = BoxReward.Count;
        //    int i = 0;
        //    for (BoxReward.Begin(); BoxReward.Next();)
        //    {

        //        if (ItemTableManager.Instance.TryGetValue(BoxReward.Key, out TABLE.ITEM boxItem))
        //        {
        //            UIItemBase uiItemBase = new UIItemBase(mgrid_reward.controlList[i], PropItemType.Normal);
        //            uiItemBase.Refresh(boxItem, Utility.ItemClick);
        //            UILabel lb_count = UtilityObj.Get<UILabel>(mgrid_reward.controlList[i].transform, "lb_count");
        //            lb_count.text = BoxReward.Value.ToString();
        //        }
        //        i++;
        //    }
        //}
    }

    private void SureClick(GameObject obj)
    {
        Close();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (uiItemlist != null)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(uiItemlist);
            uiItemlist.Clear();
        }
    }
}
