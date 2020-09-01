using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///进入幻境条件满足类型
/// </summary>
public enum ConditionType
{
    Satisfy, //满足条件
    LevelInsufficient, //等级不足
    TimeInsufficient, //幻境时间不足
}

public partial class UIDreamLandPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    /// <summary>
    /// 幻境时间和精力值换算关系配置
    /// </summary>
    TABLE.SUNDRY configRelationEnergyAndTime;

    /// <summary>
    /// 幻境进入条件配置
    /// </summary>
    TABLE.SUNDRY configEntryCondition;

    /// <summary>
    /// 幻境信息
    /// </summary>
    DreamLandData myDreamLandData;

    /// <summary>
    /// 进入幻境条件状态类型
    /// </summary>
    ConditionType conditionType;

    int levelCondition = 0; //我的等级必须满足大于等于封印等级-XX级差的条件
    int timeCondition = 0; //我的幻境时间必须满足大于等于XX分钟的条件
    long openedTime = 0; //幻境已开放小时数

    private long timeLeft = 0; //幻境剩余时间(秒)

    List<UIItemBase> listItemBases = new List<UIItemBase>();

    public override void Init()
    {
        base.Init();
        myDreamLandData = CSDreamLandInfo.Instance.MyDreamLandData;
        mClientEvent.Reg((uint) CEvent.CloseDreamLand, CloseDreamLand);
        mbtn_add.onClick = OnClickAddDreamLandTime;
        mbtn_go.onClick = OnClickGetIntoDreamLand;
        mbtn_help.onClick = OnClickHelp;
    }

    void OnSchedule()
    {
        if (timeLeft <= 0)
        {
            ScriptBinder.StopInvokeRepeating();
            UIManager.Instance.ClosePanel<UISealCombinedPanel>();
        }
        else
        {
            mlb_time.text = CSString.Format(1170, CSServerTime.Instance.FormatLongToTimeStr(timeLeft, 3));
            timeLeft--;
        }
    }

    /// <summary>
    /// 接收关闭幻境广播
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void CloseDreamLand(uint id, object data)
    {
        if (data == null) return;
        UIManager.Instance.ClosePanel<UISealCombinedPanel>();
    }

    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUITexture(mtexbg, "seal_dream");
        if (myDreamLandData == null) return;
        if (!SundryTableManager.Instance.TryGetValue(255, out configRelationEnergyAndTime)) return;
        if (!SundryTableManager.Instance.TryGetValue(256, out configEntryCondition)) return;
        openedTime = (CSServerTime.Instance.TotalMillisecond - myDreamLandData.startTime) / 3600000;
        if (myDreamLandData != null)
        {
            timeLeft = myDreamLandData.endTime / 1000 - CSServerTime.Instance.TotalSeconds;
            if (timeLeft > 0)
            {
                ScriptBinder.InvokeRepeating(0f, 1f, OnSchedule);
            }
        }

        InitData();
    }

    void InitData()
    {
        if (myDreamLandData == null) return;
        myDreamLandData = CSDreamLandInfo.Instance.MyDreamLandData;
        if (myDreamLandData.listMaps == null || myDreamLandData.listMaps.Count <= 0) return;
        int firstMapId = myDreamLandData.listMaps[0];
        CSStringBuilder.Clear();
        mlb_activityEntrance.text = CSStringBuilder.Append(CSString.Format(651),
            MapInfoTableManager.Instance.GetMapInfoDesc(firstMapId)).ToString();
        //精力值和幻境时间转换关系说明
        string[] arrContent;
        arrContent = configRelationEnergyAndTime.effect.Split('#');
        if (arrContent != null && arrContent.Length == 2)
        {
            mlb_tips.text = CSString.Format(641,
                arrContent[0], arrContent[1]);
        }

        //显示当前条件
        //幻境时间
        //条件满足和不满足的情况（显示按钮或者显示提示）
        conditionType = GetIntoDreamLandCondition();
        mlb_condition.text = CSString.Format(1154, levelCondition, timeCondition);
        long sec = myDreamLandData.myTime / 1000;
        switch (conditionType)
        {
            case ConditionType.Satisfy:
                mlb_dreamtime.text = CSServerTime.Instance.FormatLongToTimeStr(sec, 15);
                mslider_time.value = 1f;
                mbtn_go.gameObject.SetActive(true);
                mlb_hint.gameObject.SetActive(false);
                break;
            case ConditionType.LevelInsufficient:
                mlb_dreamtime.text = CSServerTime.Instance.FormatLongToTimeStr(sec, 15);
                mslider_time.value = myDreamLandData.myTime / 60000 > timeCondition
                    ? 1f
                    : (float) myDreamLandData.myTime / 60000 / timeCondition;
                mbtn_go.gameObject.SetActive(false);
                mlb_hint.gameObject.SetActive(true);
                mlb_hint.text = CSString.Format(642);
                break;
            case ConditionType.TimeInsufficient:
                mlb_dreamtime.text = CSServerTime.Instance.FormatLongToTimeStr(sec>0?sec:0, 15);
                mslider_time.value = (float) myDreamLandData.myTime / 60000 / timeCondition;
                mbtn_go.gameObject.SetActive(false);
                mlb_hint.gameObject.SetActive(true);
                mlb_hint.text = CSString.Format(643);
                break;
            default:
                break;
        }
        
        mlb_dreamtime.text = mlb_dreamtime.text.BBCode(myDreamLandData.myTime <= 0 ? ColorType.Red : ColorType.SecondaryText);

        //掉落奖励需要策划新增表
        int instanceId = 0;
        var arr = InstanceTableManager.Instance.array.gItem.handles;
        TABLE.INSTANCE instanceItem;
        for(int i = 0,max =arr.Length;i<max;++i)
        {
            instanceItem = arr[i].Value as TABLE.INSTANCE;
            if (myDreamLandData.listMaps[0] == instanceItem.mapId)
            {
                instanceId = instanceItem.id;
                break;
            }
        }

        int groupId = 0;
        var arr2 = InstanceDropShowTableManager.Instance.array.gItem.handles;
        TABLE.INSTANCEDROPSHOW instanceShowItem;
        for (int i = 0, max = arr2.Length; i < max; ++i)
        {
            instanceShowItem = arr2[i].Value as TABLE.INSTANCEDROPSHOW;
            if (instanceId == instanceShowItem.instanceId &&
                myDreamLandData.sealLeveled >= instanceShowItem.min &&
                myDreamLandData.sealLeveled <= instanceShowItem.max)
            {
                groupId = instanceShowItem.groupId;
                break;
            }
        }

        List<int> DropItemList = new List<int>();
        DropItemList.Clear();
        var arr3 = MDropItemsTableManager.Instance.array.gItem.handles;
        TABLE.MDROPITEMS dropItem;
        for (int i = 0, max = arr3.Length; i < max; ++i)
        {
            dropItem = arr3[i].Value as TABLE.MDROPITEMS;
            if (groupId == dropItem.bigGroupId)
            {
                DropItemList.Add(dropItem.itemId);
            }
        }

        mgrid_reward.MaxCount = DropItemList.Count;
        UIItemBase uiItemBase;
        TABLE.ITEM itemCfg;
        GameObject gp;
        for (int i = 0; i < mgrid_reward.MaxCount; i++)
        {
            gp = mgrid_reward.controlList[i];
            if (listItemBases.Count <= i)
                listItemBases.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, gp.transform, itemSize.Size64));
            uiItemBase = listItemBases[i];
            if (ItemTableManager.Instance.TryGetValue(DropItemList[i], out itemCfg))
                uiItemBase.Refresh(itemCfg);
        }
    }

    /// <summary>
    /// 判断进入幻境条件是否满足
    /// </summary>
    /// <returns></returns>
    ConditionType GetIntoDreamLandCondition()
    {
        //判断时间段
        List<List<int>> listCondition = UtilityMainMath.SplitStringToIntLists(configEntryCondition.effect);
        for (int i = 0; i < listCondition.Count; i++)
        {
            if (i == listCondition.Count - 1)
            {
                if (openedTime >= listCondition[i][0])
                {
                    levelCondition = myDreamLandData.sealLeveled - listCondition[i][1];
                    timeCondition = listCondition[i][2];
                }
            }
            else
            {
                if (openedTime >= listCondition[i][0] && openedTime < listCondition[i + 1][0])
                {
                    levelCondition = myDreamLandData.sealLeveled - listCondition[i][1];
                    timeCondition = listCondition[i][2];
                }
            }
        }

        conditionType = CSMainPlayerInfo.Instance.Level < levelCondition
            ? ConditionType.LevelInsufficient
            : myDreamLandData.myTime / 60000 < timeCondition || myDreamLandData.myTime <= 0
                ? ConditionType.TimeInsufficient
                : ConditionType.Satisfy;

        return conditionType;
    }

    void OnClickAddDreamLandTime(GameObject go)
    {
        if (go == null) return;
        //弹出精力值页面
        Utility.ShowGetWay(50000123);
        // UIManager.Instance.CreatePanel<UIVigorPanel>();
    }

    /// <summary>
    /// 进入幻境首张地图
    /// </summary>
    /// <param name="go"></param>
    void OnClickGetIntoDreamLand(GameObject go)
    {
        if (go == null) return;
        if (myDreamLandData.listMaps == null || myDreamLandData.listMaps[0] <= 0) return;
        Net.ReqEnterInstanceMessage(myDreamLandData.listMaps[0]);
        UIManager.Instance.ClosePanel<UISealCombinedPanel>();
    }

    void OnClickHelp(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.DreamLand);
    }

    public override void OnHide()
    {
        base.OnHide();
        ScriptBinder.StopInvokeRepeating();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mtexbg);
        UIItemManager.Instance.RecycleItemsFormMediator(listItemBases);
        base.OnDestroy();
    }
}