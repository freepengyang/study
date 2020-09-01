using System;
using System.Collections;
using System.Collections.Generic;
using bag;
using Google.Protobuf.Collections;
using treasurehunt;
using UnityEngine;

/// <summary>
/// 寻宝信息
/// </summary>
public class CSSeekTreasureInfo : CSInfo<CSSeekTreasureInfo>
{
    public int seekConfigId = 0;
    public long seekCost = 0;

    public CSSeekTreasureInfo()
    {
        Init();
    }

    void Init()
    {
        string[] strs = UtilityMainMath.StrToStrArr(SundryTableManager.Instance.GetSundryEffect(730));
        if (strs != null && strs.Length == 2)
        {
            seekConfigId = Int32.Parse(strs[0]);
            seekCost = Int64.Parse(strs[1]);
        }
    }

    public override void Dispose()
    {
    }

    /// <summary>
    /// 是否打开气泡
    /// </summary>
    private bool isOpenBubble = true;

    public bool IsOpenBubble
    {
        get => isOpenBubble;
        set => isOpenBubble = value;
    }

    /// <summary>
    /// 宝箱Id
    /// </summary>
    private int treasureChestId = 0;

    public int TreasureChestId
    {
        get { return treasureChestId; }
    }

    /// <summary>
    /// 下次寻宝开始时间
    /// </summary>
    private long starTime = 0;

    public long StarTime => starTime;

    /// <summary>
    /// 寻宝结束时间
    /// </summary>
    private long endTime = 0;

    public long EndTime => endTime;

    /// <summary>
    /// 获取界面宝箱展示列表
    /// </summary>
    /// <returns></returns>
    public List<int> GetTreasureChests()
    {
        string content = TreasureTableManager.Instance.GetTreasureShow(treasureChestId);
        List<int> listTreasureChests = UtilityMainMath.SplitStringToIntList(content);
        return listTreasureChests;
    }

    /// <summary>
    /// 全服寻宝历史记录
    /// </summary>
    private List<string> allSeekWreasureHistory = new List<string>();

    public List<string> AllSeekWreasureHistory
    {
        get { return allSeekWreasureHistory; }
    }

    /// <summary>
    /// 我的寻宝历史记录
    /// </summary>
    private List<string> mySeekWreasureHistory = new List<string>();

    public List<string> MySeekWreasureHistory
    {
        get { return mySeekWreasureHistory; }
    }

    /// <summary>
    /// 兑换积分历史记录
    /// </summary>
    private List<string> integralHistory = new List<string>();

    public List<string> IntegralHistory
    {
        get { return integralHistory; }
    }

    /// <summary>
    /// 寻宝仓库物品信息
    /// </summary>
    private CSBetterLisHot<bag.BagItemInfo> warehouseBagItemInfos = new CSBetterLisHot<BagItemInfo>();

    public CSBetterLisHot<bag.BagItemInfo> WarehouseBagItemInfos
    {
        get { return warehouseBagItemInfos; }
    }

    public void ShowItemTip(int itemId)
    {
        TABLE.ITEM tblItem;
        if (ItemTableManager.Instance.TryGetValue(itemId, out tblItem))
        {
            if (UIManager.Instance.IsPanel<UIEquipTipPanel>())
            {
                UIManager.Instance.ClosePanel<UIEquipTipPanel>();
            }

            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, tblItem);

            // UIManager.Instance.CreatePanel<UIEquipTipPanel>(action: (f) =>
            // {
            //     CSEquipTipsData equipData = UtilityEquip.GetDiyuanDuiHuanTipsDataByItem(tblItem, ItemTipsType.BagShow);
            //     (f as UIEquipTipPanel).Show(equipData);
            // });
        }
    }

    /// <summary>
    /// 获取可回收地元装备列表（根据杂项表配置剔除由服务端做）
    /// </summary>
    /// <returns></returns>
    public RepeatedField<int> GetRecyclableWoLongItems()
    {
        RepeatedField<int> list = new RepeatedField<int>();
        list.Clear();
        for (int i = 0; i < warehouseBagItemInfos.Count; i++)
        {
            TABLE.ITEM cfg = null;
            if (ItemTableManager.Instance.TryGetValue(warehouseBagItemInfos[i].configId, out cfg))
            {
                if (CSBagInfo.Instance.IsWoLongEquip(cfg))
                {
                    list.Add(warehouseBagItemInfos[i].bagIndex);
                }
            }
        }

        return list;
    }

    /// <summary>
    /// 获取仓库可用地元丹列表
    /// </summary>
    /// <returns></returns>
    public RepeatedField<int> GetWoLongTans()
    {
        RepeatedField<int> list = new RepeatedField<int>();
        list.Clear();
        //只能使用这些配置列表中的地元丹
        List<int> enableUseItems =
            UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(586));

        for (int i = 0; i < warehouseBagItemInfos.Count; i++)
        {
            TABLE.ITEM cfg = null;
            if (ItemTableManager.Instance.TryGetValue(warehouseBagItemInfos[i].configId, out cfg))
            {
                if (CSBagInfo.Instance.IsWoLongDan(cfg) && enableUseItems.Contains(cfg.id))
                {
                    list.Add(warehouseBagItemInfos[i].bagIndex);
                }
            }
        }

        return list;
    }

    /// <summary>
    /// 获取不同类型兑换页签的Points信息
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, List<TABLE.POINTS>> GetDicFilterByType()
    {
        var arr = PointsTableManager.Instance.array.gItem.handles;
        Dictionary<int, List<TABLE.POINTS>> d = new Dictionary<int, List<TABLE.POINTS>>();
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            TABLE.POINTS tbl = arr[k].Value as TABLE.POINTS;
            if (d.ContainsKey((int) tbl.type))
            {
                if (CheckIsNeeded(tbl))
                    d[(int) tbl.type].Add(tbl);
            }
            else
            {
                if (CheckIsNeeded(tbl))
                {
                    List<TABLE.POINTS> list = new List<TABLE.POINTS>();
                    list.Add(tbl);
                    d.Add((int) tbl.type, list);
                }
            }
        }

        return d;
    }

    private bool CheckIsNeeded(TABLE.POINTS point)
    {
        if (point == null) return false;
        TABLE.ITEM item;
        CSMainPlayerInfo playerInfo = CSMainPlayerInfo.Instance;
        List<int> limits = UtilityMainMath.SplitStringToIntList(point.showlevel, '_');
        if (limits.Count != 2)
        {
            // Debug.Log("-------------------ponits表限制条件配置错误@吕惠铭");
        }

        if (playerInfo.ServerOpenDay >= point.showDay)
        {
            if (playerInfo.Level >= limits[0] &&
                CSWoLongInfo.Instance.ReturnWoLongInfo().wolongLevel >= limits[1]) //等级和地元等级控制显示
            {
                if (ItemTableManager.Instance.TryGetValue((int) point.itemId, out item))
                {
                    if (item.career == playerInfo.Career || item.career == 0 &&
                        (item.sex == playerInfo.Sex || item.sex == 2))
                        return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 寻宝仓库是否有可提取道具
    /// </summary>
    /// <returns></returns>
    public bool HasExtractEquip()
    {
        return warehouseBagItemInfos.Count > 0;
    }


    /// <summary>
    /// 获取主界面寻宝活动按钮倒计时显示
    /// </summary>
    /// <returns></returns>
    public string GetTreasureEndTime()
    {
        if (endTime > 0)
        {
            long sec = endTime / 1000 - CSServerTime.Instance.TotalSeconds;
            if (sec>0)
                return CSString.Format(2012, CSServerTime.Instance.FormatLongToTimeStr(sec, 11));
        }
        
        return String.Empty;
    }

    #region 网络响应处理函数

    /// <summary>
    /// 处理历史信息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleServerHistory(treasurehunt.ServerHistory msg)
    {
        if (msg == null) return;
        //1为寻宝记录，2为积分兑换
        if (msg.type == 1)
        {
            //全服寻宝记录
            allSeekWreasureHistory.Clear();
            for (int i = 0; i < msg.serverHistory.Count; i++)
            {
                allSeekWreasureHistory.Add(msg.serverHistory[i]);
            }

            if (allSeekWreasureHistory.Count > 50)
            {
                allSeekWreasureHistory.RemoveRange(0, allSeekWreasureHistory.Count - 50);
            }

            //我的个人寻宝纪录
            // mySeekWreasureHistory.Clear();
            for (int i = 0; i < msg.history.Count; i++)
            {
                mySeekWreasureHistory.Add(msg.history[i]);
            }

            if (mySeekWreasureHistory.Count > 50)
            {
                mySeekWreasureHistory.RemoveRange(0, mySeekWreasureHistory.Count - 50);
            }
        }
        else if (msg.type == 2)
        {
            integralHistory.Clear();
            for (int i = 0; i < msg.serverHistory.Count; i++)
            {
                integralHistory.Add(msg.serverHistory[i]);
            }

            if (integralHistory.Count > 50)
            {
                integralHistory.RemoveRange(0, integralHistory.Count - 50);
            }
        }
    }

    /// <summary>
    /// 处理获取仓库物品信息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleStorehouse(treasurehunt.TreasureHuntInfo msg)
    {
        if (msg == null) return;
        warehouseBagItemInfos.Clear();
        for (int i = 0; i < msg.itemInfo.Count; i++)
        {
            warehouseBagItemInfos.Add(msg.itemInfo[i]);
        }

        warehouseBagItemInfos.Sort((x, y) => { return x.bagIndex.CompareTo(y.bagIndex); });
    }

    /// <summary>
    /// 处理仓库物品变动
    /// </summary>
    /// <param name="msg"></param>
    public void HandleItemChanged(treasurehunt.TreasureItemChangeList msg)
    {
        if (msg == null) return;
        for (int i = 0; i < msg.changeList.Count; i++)
        {
            bag.BagItemInfo info = msg.changeList[i];
            bool isExist = false;
            for (int j = 0; j < warehouseBagItemInfos.Count; j++)
            {
                if (warehouseBagItemInfos[j].id == info.id)
                {
                    isExist = true;
                    if (info.count <= 0)
                        warehouseBagItemInfos.RemoveAt(j);
                    else
                        warehouseBagItemInfos[j] = info;
                }
            }

            if (!isExist && info.count > 0)
            {
                warehouseBagItemInfos.Add(info);
            }
        }

        warehouseBagItemInfos.Sort((x, y) => { return x.bagIndex.CompareTo(y.bagIndex); });
    }

    /// <summary>
    /// 处理寻宝结束信息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleEnd(treasurehunt.TreasureEndResponse msg)
    {
        if (msg == null) return;
    }

    /// <summary>
    /// 处理使用经验丹信息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleUseTreasureExp(treasurehunt.ExpUseRequest msg)
    {
        if (msg == null) return;
    }

    /// <summary>
    /// 处理寻宝界面宝箱响应
    /// </summary>
    /// <param name="msg"></param>
    public void HandleTreasureBox(treasurehunt.TreasureIdResponse msg)
    {
        if (msg == null) return;
        treasureChestId = msg.id;
        starTime = msg.startTime;
        endTime = msg.endTime;
    }

    /// <summary>
    /// 处理寻宝仓库中回收装备响应
    /// </summary>
    /// <param name="msg"></param>
    public void HandleHuntCallBack(treasurehunt.HuntCallbackResponse msg)
    {
        if (msg == null) return;
    }

    #endregion
}