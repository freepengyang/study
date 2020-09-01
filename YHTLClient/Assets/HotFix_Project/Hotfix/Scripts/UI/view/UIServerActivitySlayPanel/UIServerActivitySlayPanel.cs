using System;
using System.Collections.Generic;
using UnityEngine;


public partial class UIServerActivitySlayPanel : UIBasePanel
{
    const int actId = 10121;

    long leftTime = 0;


    EndLessList<UIMonsterSlayItem, MonsterSlayData> endLessList;


    public override void Init()
    {
        base.Init();

        CSEffectPlayMgr.Instance.ShowUITexture(mbanner12, "banner12");

        mClientEvent.Reg((uint)CEvent.MonsterSlayInfoChange, MonsterSlayInfoChange);

        //mscroll.SetDynamicArrowVertical(msp_scroll);
        var list = CSSpecialActivityMonsterSlayInfo.Instance.AllDatas;
        int maxLength = mGrid.itemSize * (list != null ? list.Count : 0);
        if (maxLength > 0)
        {
            mscroll.SetDynamicArrowVerticalWithWrap(maxLength, msp_scroll);
        }
        else mscroll.SetDynamicArrowVertical(msp_scroll);
    }

    public override void Show()
    {
        base.Show();

        SetCountDownTimer();
        RefreshUI();
    }

    public override void OnHide()
    {
        base.OnHide();
        ScriptBinder.StopInvokeRepeating();
        //mGrid.UnBind<UIMonsterSlayItem>();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mbanner12);
        //mGrid.UnBind<UIMonsterSlayItem>();
        endLessList?.Destroy();
        endLessList = null;

        base.OnDestroy();
    }


    void RefreshUI()
    {
        var list = CSSpecialActivityMonsterSlayInfo.Instance.AllDatas;

        //mGrid.Bind<MonsterSlayData, UIMonsterSlayItem>(list, mPoolHandleManager);
        mscroll.CustomActive(false);

        if (endLessList == null)
        {
            endLessList = new EndLessList<UIMonsterSlayItem, MonsterSlayData>(SortType.Vertical, mGrid, mPoolHandleManager, 5, ScriptBinder);
        }
        endLessList.Clear();

        if (list == null) return;

        list.Sort((a, b) =>
        {
            if (a.StateInt == b.StateInt) return a.id - b.id;
            else return a.StateInt - b.StateInt;
        });

        for (int i = 0; i < list.Count; i++)
        {
            var data = endLessList.Append();
            data.CopyData(list[i]);
        }

        endLessList.Bind();

        mscroll.CustomActive(true);
    }


    void SetCountDownTimer()
    {
        leftTime = UIServerActivityPanel.GetEndTime(actId);
        ScriptBinder.InvokeRepeating(0f, 1f, CountDown);
    }

    void CountDown()
    {
        if (leftTime < 0)
        {
            ScriptBinder.StopInvokeRepeating();
            return;
        }
        mlb_time.text = $"剩余时间：{CSServerTime.Instance.FormatLongToTimeStr(leftTime, 1)}";
        leftTime--;
    }


    void MonsterSlayInfoChange(uint id, object data)
    {
        RefreshUI();
    }

}



public class UIMonsterSlayItem : UIBinder
{
    UILabel lb_name;

    UIEventListener btn_receive;
    GameObject obj_blueEffect;
    UIEventListener btn_kill;
    UISprite sp_kill;
    GameObject obj_received;
    UIGridContainer grid_rewards;

    MonsterSlayData mData;


    List<UIItemBase> itemList;


    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        btn_receive = Get<UIEventListener>("btn_receive");
        obj_blueEffect = Get<GameObject>("btn_receive/effect");
        btn_kill = Get<UIEventListener>("btn_kill");
        sp_kill = Get<UISprite>("btn_kill");
        obj_received = Get<GameObject>("sp_complete");
        grid_rewards = Get<UIGridContainer>("Grid");

        CSEffectPlayMgr.Instance.ShowUIEffect(obj_blueEffect, 17903);
    }

    public override void Bind(object data)
    {
        mData = data as MonsterSlayData;
        if (mData == null) return;

        string[] quaStr = SundryTableManager.Instance.GetSundryEffect(33).Split('#');
        //Debug.LogError("@@@@@quaStr:" + quaStr.Length + ", mData.quality:" + mData.quality + ", id:" + mData.id);
        string desc1 = CSString.Format(1144, mData.goalNum, quaStr[mData.quality - 1], mData.targetLv);
        string desc2 = $"({mData.curNum}/{mData.goalNum})".BBCode(mData.curNum >= mData.goalNum ? ColorType.Green : ColorType.Red);
        lb_name.text = $"{desc1}{desc2}";
        btn_receive.gameObject.SetActive(mData.curNum >= mData.goalNum && !mData.rewardReceived);
        btn_kill.gameObject.SetActive(mData.curNum < mData.goalNum);
        obj_received.SetActive(mData.curNum >= mData.goalNum && mData.rewardReceived);

        if (itemList == null) itemList = new List<UIItemBase>();
        //else UIItemManager.Instance.RecycleItemsFormMediator(itemList);
        int boxId = SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardReward(mData.id);
        Utility.GetItemByBoxid(boxId, grid_rewards, ref itemList, itemSize.Size64);

        btn_receive.onClick = ReceiveRewardsCilck;
        btn_kill.onClick = GoToSlayClick;

    }


    void ReceiveRewardsCilck(GameObject go)
    {
        if (mData == null) return;
        CSSpecialActivityMonsterSlayInfo.Instance.TryToReceiveRewards(mData.id);
    }


    void GoToSlayClick(GameObject go)
    {
        //if (mData == null) return;
        //UtilityPanel.JumpToPanel(10420);
        //UIManager.Instance.ClosePanel<UIServerActivityPanel>();

        string wayStr = SundryTableManager.Instance.GetSundryEffect(1080);
        UtilityPanel.ShowCompleteWayWithSelfAdapt(wayStr, sp_kill, AnchorType.TopCenter);
    }


    public override void OnDestroy()
    {
        UIItemManager.Instance.RecycleItemsFormMediator(itemList);
        CSEffectPlayMgr.Instance.Recycle(obj_blueEffect);
        lb_name = null;
        btn_receive = null;
        obj_blueEffect = null;
        btn_kill = null;
        sp_kill = null;
        obj_received = null;
        grid_rewards = null;
    }
}