using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Collections;
using System;

public class UIFirstChargeReward : UIBasePanel
{
    protected UIGridContainer mgrid_day;
    protected UIGridContainer mgrid_itemList;

    List<RewardInfo> rewardInfoList = new List<RewardInfo>();
    public override void Init() {

        base.Init();
        string requiredInfo = SundryTableManager.Instance.GetSundryEffect(420);
        string[] infos = requiredInfo.Split('#');

        mgrid_day.MaxCount = infos.Length;
        for (int i = 0; i < infos.Length; i++)
        {
            int boxid = int.Parse(infos[i]);
            //string boxnuminfo = BoxTableManager.Instance.GetBoxNum(boxid);
            rewardInfoList.Add(new RewardInfo(boxid));
            //循环给tabitem添加点击事件 调用RewardInfo中的showItemList方法
            UIEventListener.Get(mgrid_day.controlList[i], rewardInfoList[i]).onClick = tabClick;
            
        }
        
    }

    private void tabClick(GameObject obj)
    {
        RewardInfo info = (RewardInfo)UIEventListener.Get(obj).parameter;
        showItemList();
        //这里根据info生成itemList
        //判断玩家是否首充
        //判断该天数玩家是否已领取

    }

    public void showItemList()
    {
        //根据_itemInfoList 生成item列表
        //mgrid_itemList.controlList[i].getchild
        //给item添加点击事件
        //UIEventListener.Get(mbtn_buy).onClick = BuyClick;
    }
}

public class RewardInfo {

    public int _boxid;
    public List<itemInfo> _itemInfoList = new List<itemInfo>();

    public struct itemInfo {
       public int itemID;
       public int num;
    }

    public RewardInfo(int boxid) {
        _boxid = boxid;
        ////解析info
        string boxItemInfo = BoxTableManager.Instance.GetBoxItem(boxid);
        string[] boxItems = boxItemInfo.Split('#');
        string boxNumInfo = BoxTableManager.Instance.GetBoxNum(boxid);
        //if (boxItems.Length != boxNumInfo.Count)
        FNDebug.LogError("解析box表错误item数量与num数量不一致，请检查  boxid: " + _boxid);
        //mgrid_itemList.MaxCount = boxItems.Length;
        for (int i = 0; i < boxItems.Length; i++)
        {
            itemInfo ItemInfoData = new itemInfo();
            ItemInfoData.itemID = int.Parse(boxItems[i]);
            ItemInfoData.num = boxNumInfo[i];
            _itemInfoList.Add(ItemInfoData);
        }

        //Map<int, int> BoxReward = new Map<int, int>();
        //BoxTableManager.Instance.GetBoxAwardById(para.reward, BoxReward);
        //if (BoxReward.Count > 0)
        //{
        //    for (BoxReward.Begin(); BoxReward.Next();)
        //    {
        //        if (ItemTableManager.Instance.TryGetValue(BoxReward.Key, out TABLE.ITEM boxItem))
        //        {

        //            //_taskRewards.Add(new NPCTaskReward()
        //            //{ TableItem = boxItem, Count = BoxReward.Value });
        //        }
        //    }
        //}


    }

    

}
