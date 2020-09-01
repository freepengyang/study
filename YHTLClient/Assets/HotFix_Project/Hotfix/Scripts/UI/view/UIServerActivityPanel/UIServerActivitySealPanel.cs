using activity;
using System.Collections.Generic;
using UnityEngine;

public partial class UIServerActivitySealPanel : UIBasePanel
{
    ResFengYinData dengyinData;
    SpecialActivityData rewardmes;
    int sealAcId = 10103;
    List<UIItemBase> allReward = new List<UIItemBase>();
    FastArrayElementFromPool<OpenAcSealItem> showItems;
    long leftTime = 0;
    Schedule schedule;
    int allReId = 0;
    int allboxId = 0;
    int needLv = 0;
    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.ResSpecialActivityDataMessage, RewardChangeBack);
        int rankStartId = SpecialActiveRewardTableManager.Instance.GetSealTypeStartId(1);
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_title, "banner1");
        showItems = mPoolHandleManager.CreateGeneratePool<OpenAcSealItem>();
        showItems.Count = 3;
        for (int i = 0; i < showItems.Count; i++)
        {
            showItems[i].SetGo(mgrid_rank.GetChild(i).gameObject, rankStartId + i);
        }
        UIEventListener.Get(mbtn_getReward.gameObject).onClick = GetBtnClick;

        //全服
        allReId = SpecialActiveRewardTableManager.Instance.GetSealTypeStartId(2);
        allboxId = SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardReward(allReId);
        needLv = int.Parse(SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardTermNum(allReId));
        mlb_lvLimit.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1060), needLv);

        List<int> allRewardIds = UtilityMainMath.SplitStringToIntList(BoxTableManager.Instance.GetBoxItem(allboxId), '&');
        List<int> allRewardNums = UtilityMainMath.SplitStringToIntList(BoxTableManager.Instance.GetBoxNum(allboxId), '&');
        allReward = UIItemManager.Instance.GetUIItems(allRewardIds.Count, PropItemType.Normal, mgrid_allreward.transform, itemSize.Size54);
        for (int i = 0; i < allReward.Count; i++)
        {
            allReward[i].Refresh(allRewardIds[i]);
            allReward[i].SetCount(allRewardNums[i]);
        }
        mgrid_allreward.Reposition();
        leftTime = UIServerActivityPanel.GetEndTime(sealAcId);
        if (!Timer.Instance.IsInvoking(schedule))
        {
            schedule = Timer.Instance.InvokeRepeating(0f, 1f, CountDown);
        }
    }

    public override void Show()
    {
        base.Show();
    }
    public override void OnShow(int typeId = 0)
    {
        base.OnShow(typeId);
        leftTime = UIServerActivityPanel.GetEndTime(sealAcId);
        if (!Timer.Instance.IsInvoking(schedule))
        {
            schedule = Timer.Instance.InvokeRepeating(0f, 1f, CountDown);
        }
    }
    public override void OnHide()
    {
        base.OnHide();
        Timer.Instance.CancelInvoke(schedule);
    }

    protected override void OnDestroy()
    {
        Timer.Instance.CancelInvoke(schedule);
        for (int i = 0; i < showItems.Count; i++)
        {
            showItems[i].Recycle();
        }
        UIItemManager.Instance.RecycleItemsFormMediator(allReward);
        base.OnDestroy();
    }
    void RewardChangeBack(uint id, object data)
    {
        SpecialActivityData mes = (SpecialActivityData)data;
        if (mes.activityId != sealAcId)
        {
            return;
        }
        rewardmes = mes;
        dengyinData = CSOpenServerACInfo.Instance.GetFengYinData();
        Refresh();
    }
    int getState = 0;
    void CountDown(Schedule _schedule)
    {
        mlb_countdown.text = CSString.Format(1108, CSServerTime.Instance.FormatLongToTimeStr(leftTime, 1));;
        leftTime--;
    }
    void Refresh()
    {
        if (rewardmes.finishGoals.Contains(allReId))//已完成可领
        {
            getState = 1;
            mlb_getReward.text = "领取";

            mbtn_getReward.gameObject.SetActive(true);
            mobj_allRewardGet.SetActive(false);
            mbtn_getReward.spriteName = "btn_samll1";
        }
        else if (rewardmes.rewardGoals.Contains(allReId))//已领取
        {
            getState = 2;
            mlb_getReward.text = "已领取";
            mbtn_getReward.gameObject.SetActive(false);
            mobj_allRewardGet.SetActive(true);
            mbtn_getReward.spriteName = "btn_samll3";
        }
        else//不可领取
        {
            getState = 3;
            mlb_getReward.text = "前往升级";
            mbtn_getReward.gameObject.SetActive(true);
            mobj_allRewardGet.SetActive(false);
            mbtn_getReward.spriteName = "btn_samll1";
        }

        //排名
        for (int i = 0; i < showItems.Count; i++)
        {
            if (dengyinData.datas.Count <= i)
            {
                showItems[i].ReFreshName(rewardmes);
            }
            else
            {
                showItems[i].ReFreshName(rewardmes, dengyinData.datas[i]);
            }
        }
    }
    void GetBtnClick(GameObject _go)
    {
        if (getState == 1)
        {
            Net.ReqSpecialActivityRewardMessage(10103, allReId);
        }
        else if (getState == 2)
        {
            UtilityTips.ShowRedTips(1061);
        }
        else
        {
            Utility.ShowGetWay(5);
        }
    }
}
public class OpenAcSealItem
{
    public GameObject go;
    public UILabel IsNil;
    public UISprite btn_get;
    public GameObject sp_got;
    public UILabel lb_btntext;
    public UIGrid grid;
    List<UIItemBase> allReward = new List<UIItemBase>();
    int index; //reward里边GoalId
    int getState = 0;
    public OpenAcSealItem()
    {

    }
    public void SetGo(GameObject _go, int _index)
    {
        go = _go;
        index = _index;
        IsNil = go.transform.Find("lb_level").GetComponent<UILabel>();
        btn_get = go.transform.Find("btn_get").GetComponent<UISprite>();
        sp_got = go.transform.Find("sp_get").gameObject;
        lb_btntext = go.transform.Find("btn_get/Label").GetComponent<UILabel>();
        grid = go.transform.Find("Grid").GetComponent<UIGrid>();
        int allboxId = SpecialActiveRewardTableManager.Instance.GetSpecialActiveRewardReward(_index);
        List<int> allRewardIds = UtilityMainMath.SplitStringToIntList(BoxTableManager.Instance.GetBoxItem(allboxId), '&');
        List<int> allRewardNums = UtilityMainMath.SplitStringToIntList(BoxTableManager.Instance.GetBoxNum(allboxId), '&');
        allReward = UIItemManager.Instance.GetUIItems(allRewardIds.Count, PropItemType.Normal, grid.transform, itemSize.Size54);
        for (int i = 0; i < allReward.Count; i++)
        {
            allReward[i].Refresh(allRewardIds[i]);
            allReward[i].SetCount(allRewardNums[i]);
        }
        UIEventListener.Get(btn_get.gameObject).onClick = GetBtnClick;
    }
    public void ReFreshName(SpecialActivityData rewardDat, string _name = "")
    {
        if (string.IsNullOrEmpty(_name))
        {
            IsNil.text = "虚位以待";
            IsNil.color = UtilityColor.HexToColor("#ff0000");
            btn_get.gameObject.SetActive(false);
        }
        else
        {
            if (_name == CSMainPlayerInfo.Instance.Name)
            {
                IsNil.text = "";
                btn_get.gameObject.SetActive(true);
                if (rewardDat.finishGoals.Contains(index))//已完成可领
                {
                    lb_btntext.text = "领取";
                    btn_get.spriteName = "btn_samll1";
                    btn_get.gameObject.SetActive(true);
                    sp_got.SetActive(false);
                    getState = 1;
                }
                else if (rewardDat.rewardGoals.Contains(index))//已领取
                {
                    getState = 2;
                    lb_btntext.text = "已领取";
                    sp_got.SetActive(true);
                    btn_get.gameObject.SetActive(false);
                    btn_get.spriteName = "btn_samll3";
                }
                else//不可领取
                {
                    btn_get.gameObject.SetActive(true);
                    sp_got.SetActive(false);
                    getState = 3;
                }
            }
            else
            {
                btn_get.gameObject.SetActive(false);
                IsNil.text = _name;
                IsNil.color = UtilityColor.HexToColor("#3d1400");
            }

        }
    }
    void GetBtnClick(GameObject _go)
    {
        if (getState == 1)
        {
            Net.ReqSpecialActivityRewardMessage(10103, index);
        }
        else if (getState == 2)
        {
            UtilityTips.ShowRedTips(1061);
        }
    }
    public void Recycle()
    {
        UIItemManager.Instance.RecycleItemsFormMediator(allReward);
    }
}
