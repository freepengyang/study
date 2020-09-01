using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIUndergroundTreasureInstancePanel : UIBasePanel
{
    //Schedule randomTimer;
    long nextRefreshTime = 0;
    instance.UndergroundTreasureInstanceInfo Info;
    public override void Init()
    {

        base.Init();
        if (CSInstanceInfo.Instance.UndergroundInfo != null)
        {
            
            Info = CSInstanceInfo.Instance.UndergroundInfo;
            //Debug.Log(Info.state);
            CSInstanceInfo.Instance.UndergroundInfo = null;
            ShowPanelInfo();
        }
        mClientEvent.Reg((uint)CEvent.UndergroundTreasure, ShowPanelInfo);//显示波数信息
    }

    private void ShowPanelInfo(uint uiEvtID, object data)
    {
        Info = data as instance.UndergroundTreasureInstanceInfo;
        ShowPanelInfo();
    }

    private void ShowPanelInfo() {

        if (Info.state == 1)
        {
            ScriptBinder.InvokeRepeating(0, 1f, RandomTimeDown);
            //randomTimer = Timer.Instance.InvokeRepeating(0, 1f, RandomTimeDown);
        }
        else
        {
            //Debug.Log("ShowPanelInfo");
            //显示副本完成
            mobj_Finish.SetActive(true);
            mobj_time.SetActive(false);
            mobj_protectTime.SetActive(false);
        }
    }

    int i = 0;
    void RandomTimeDown()
    {
        if (Info != null)
        {
            if (true)
            {

            }
            if (Info.nextFreshTime == 0)
                return;
            int refreshtime = int.Parse(SundryTableManager.Instance.GetSundryEffect(443)); //刷新时间间隔
            int totaltime = int.Parse(SundryTableManager.Instance.GetSundryEffect(517));
            nextRefreshTime = ((Info.nextFreshTime - CSServerTime.DateTimeToStampForMilli(CSServerTime.Now)) / 1000) + refreshtime * i;
            //Debug.Log("Info.nextFreshTime :" + Info.nextFreshTime);
            //Debug.Log("Info.nexe :" + CSServerTime.DateTimeToStampForMilli(CSServerTime.Now));
            mlb_time.text = CSServerTime.Instance.FormatLongToTimeStr(nextRefreshTime, 3);
            if (nextRefreshTime > (refreshtime - totaltime))
            {
                mobj_protectTime.SetActive(true);
                mlb_protectTime.text = CSServerTime.Instance.FormatLongToTimeStr(nextRefreshTime - (refreshtime - totaltime), 3);
                mlb_hint.SetActive(false);
            }
            else {
                mobj_protectTime.SetActive(false);
                mlb_protectTime.text = "";
                mlb_hint.SetActive(true);   
            }

            //计算掉落时间

        }
        if (nextRefreshTime <= 0)
        {
            i++;
        }
        
    }
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    
    public override UILayerType PanelLayerType
    {
        get
        {
            return UILayerType.Resident;
        }
    }
    

    protected override void OnDestroy()
    {
        // if (Timer.Instance.IsInvoking(randomTimer))
        //     Timer.Instance.CancelInvoke(randomTimer);
        //randomTimer = null;
        base.OnDestroy();
    }
}
