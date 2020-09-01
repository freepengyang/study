using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIWorldInspirePanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    int coinCount;
    int goldCount;
    int coinTotalCount = 0;
    int goldTotalCount = 0;
    worldboss.BlessInfo blessInfo;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint)CEvent.ECM_SCWorldBossBlessInfoMessage, GetBlessChange);
        mClientEvent.Reg((uint)CEvent.SCWorldBossActivityInfoResponseMessage, GetInstanceInfo);
        UIEventListener.Get(mbtn_close).onClick = CloseClick;
        UIEventListener.Get(mbtn_coin, 1).onClick = InspireClick;
        UIEventListener.Get(mbtn_gold, 2).onClick = InspireClick;
        UIEventListener.Get(mbtn_bg).onClick = CloseClick;
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(324), out coinTotalCount);
        int.TryParse(SundryTableManager.Instance.GetSundryEffect(325), out goldTotalCount);
    }
    public void SetData(int coinNum, int goldNum)
    {
        coinCount = coinNum;
        goldCount = goldNum;
        blessInfo = CSInstanceInfo.Instance.GetWorldBossBlessInfo();
        if (blessInfo != null)
        {
            mlb_coinCount.text = $"{coinTotalCount - coinCount}/{coinTotalCount}";
            mlb_goldCount.text = $"{goldTotalCount - goldCount}/{goldTotalCount}";
            mlb_coinCount.color = coinCount >= coinTotalCount ? CSColor.red : CSColor.green;
            mlb_goldCount.color = goldCount >= goldTotalCount ? CSColor.red : CSColor.green;
        }
        else
        {
            mlb_coinCount.text = $"{coinTotalCount}/{coinTotalCount}";
            mlb_goldCount.text = $"{goldTotalCount}/{goldTotalCount}";
            mlb_coinCount.color = CSColor.green;
            mlb_goldCount.color = CSColor.green;
        }
    }

    public override void Show()
    {
        mlb_coinCost.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(656), SundryTableManager.Instance.GetSundryEffect(320));
        mlb_goldCost.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(656), SundryTableManager.Instance.GetSundryEffect(322));
        base.Show();
    }


    protected override void OnDestroy()
    {

        base.OnDestroy();

    }
    void GetBlessChange(uint id, object data)
    {
        if (data == null) { return; }
        worldboss.BlessInfo blessInfo = (worldboss.BlessInfo)data;
        coinCount = blessInfo.goldTimes;
        goldCount = blessInfo.yuanbaoTimes;
        mlb_coinCount.text = $"{coinTotalCount - coinCount}/{coinTotalCount}";
        mlb_goldCount.text = $"{goldTotalCount - goldCount}/{goldTotalCount}";
        mlb_coinCount.color = coinCount >= coinTotalCount ? CSColor.red : CSColor.green;
        mlb_goldCount.color = goldCount >= goldTotalCount ? CSColor.red : CSColor.green;

        mlb_coinCost.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(656), SundryTableManager.Instance.GetSundryEffect(320));
        mlb_goldCost.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(656), SundryTableManager.Instance.GetSundryEffect(322));
    }
    void GetInstanceInfo(uint id,object data)
    {
        blessInfo = CSInstanceInfo.Instance.GetWorldBossBlessInfo();
        if (blessInfo != null)
        {
            mlb_coinCount.text = $"{coinTotalCount - coinCount}/{coinTotalCount}";
            mlb_goldCount.text = $"{goldTotalCount - goldCount}/{goldTotalCount}";
            mlb_coinCount.color = coinCount >= coinTotalCount ? CSColor.red : CSColor.green;
            mlb_goldCount.color = goldCount >= goldTotalCount ? CSColor.red : CSColor.green;
        }
        else
        {
            mlb_coinCount.text = $"{coinTotalCount}/{coinTotalCount}";
            mlb_goldCount.text = $"{goldTotalCount}/{goldTotalCount}";
            mlb_coinCount.color = CSColor.green;
            mlb_goldCount.color = CSColor.green;
        }
    }
    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIWorldInspirePanel>();
    }
    void InspireClick(GameObject _go)
    {
        int type = (int)UIEventListener.Get(_go).parameter;
        int coinEff = int.Parse(SundryTableManager.Instance.GetSundryEffect(320));
        int goldEff = int.Parse(SundryTableManager.Instance.GetSundryEffect(322));
        if (type == 1)
        {
            if (coinCount >= int.Parse(SundryTableManager.Instance.GetSundryEffect(324)))
            {
                UtilityTips.ShowRedTips(652);
                return;
            }
            if (CSItemCountManager.Instance.GetItemCount(1) < coinEff)
            {
                Utility.ShowGetWay(1);
                //UtilityTips.ShowRedTips(654);
                return;
            }
        }
        else if (type == 2)
        {
            if (goldCount >= int.Parse(SundryTableManager.Instance.GetSundryEffect(325)))
            {
                UtilityTips.ShowRedTips(653);
            }
            if (CSItemCountManager.Instance.GetItemCount(3) < goldEff)
            {
                Utility.ShowGetWay(3);
                //UtilityTips.ShowRedTips(655);
                return;
            }
        }
        Net.CSWorldBossBlessMessage(type);
    }
}
