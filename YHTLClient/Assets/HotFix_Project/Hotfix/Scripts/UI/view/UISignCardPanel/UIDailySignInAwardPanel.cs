using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIDailySignInAwardPanel : UIBasePanel
{

    List<UIItemBase> rewardItemList = new List<UIItemBase>();

    public override void Init()
    {
        base.Init();
        AddCollider();
    }

    public override void Show()
    {
        base.Show();
    }


    public void OpenPanel(string name, Map<int,int> rewards, int slot = 0)
    {
        mlb_name.text = name;
        rewardItemList = UIItemManager.Instance.GetUIItems(rewards.Count, PropItemType.Normal, mGrid_reward.transform, itemSize.Size60);
        if (rewardItemList != null)
        {
            int i = 0;
            for (rewards.Begin(); rewards.Next();)
            {
                rewardItemList[i].Refresh(rewards.Key, ItemClick);
                var count = rewards.Value;
                string numStr = count < 10000 ? count.ToString() : UtilityMath.GetDecimalValue(count, "F1");
                rewardItemList[i].SetCount(numStr, CSColor.white);
                i++;
            }
            mGrid_reward.Reposition();
            mscroll_reward.ResetPosition();
        }

        mlb_cardSlot.gameObject.SetActive(slot > 0);
        mobj_arrow.SetActive(rewards.Count > 8);

        mtrans_scrollView.localPosition = slot > 0 ? new Vector2(-104, 57) : new Vector2(-104, 77);
        mobj_arrow.transform.localPosition = slot > 0 ? new Vector2(0, -61) : new Vector2(0, -41);

        msp_bg.height = slot > 0 ? 229 : 209;

        if (slot > 0)
        {
            mlb_cardSlot.text = CSString.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1071), slot);
        }
    }


    void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }


    protected override void OnDestroy()
    {
        UIItemManager.Instance.RecycleItemsFormMediator(rewardItemList);
        base.OnDestroy();
    }
}
