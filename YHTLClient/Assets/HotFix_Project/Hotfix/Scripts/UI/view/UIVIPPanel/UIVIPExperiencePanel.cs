using System;
using UnityEngine;
using vip;

public partial class UIVIPExperiencePanel : UIBasePanel
{
    ExperienceCardInfo experienceCardInfo;
    private Schedule VipExperienceTimer;
    private long randomTime = 0;
    private TABLE.VIP viptable;
    int second;
    public override void Init()
	{
		base.Init();
        AddCollider();
        UIEventListener.Get(mbtn_close).onClick = Close;
        UIEventListener.Get(mbtn_go).onClick = Close;
        UIEventListener.Get(mbtn_end).onClick = OnEndClick;
        experienceCardInfo = CSVipInfo.Instance.ExperienceCardInfo;
        bool isVipExpired = CSVipInfo.Instance.IsVipTasteExpired();
        msp_end.SetActive(isVipExpired);
        msp_state.SetActive(!isVipExpired);
        if (isVipExpired)
        {
            mbtn_go.SetActive(false);
            mbtn_end.SetActive(true);
            //mlb_time.gameObject.SetActive(false);
            //msp_end.SetActive(true);
        }
        else {
            mbtn_go.SetActive(true);
            mbtn_end.SetActive(false);
            //mlb_time.gameObject.SetActive(true);
            //msp_state.SetActive(true);
            //记时
            //VipExperienceTimer = Timer.Instance.InvokeRepeating(0, 1f, VipExperienceTimeDown);
        }

        int viplevel = experienceCardInfo.vipLevel;
        if (VIPTableManager.Instance.TryGetValue(viplevel,out viptable))
        {
            //mlb_item.text = viptable.tips;
            CSStringBuilder.Clear();
            mlb_item.text = CSStringBuilder.Append(UtilityColor.ImportantText,viptable.tips).ToString(); 
        }
        mClientEvent.SendEvent(CEvent.HideChatPanel);

    }
    
    private void VipExperienceTimeDown(Schedule obj)
    {
        
        if (experienceCardInfo != null)
        {
            randomTime = (experienceCardInfo.endTime - CSServerTime.DateTimeToStampForMilli(CSServerTime.Now)) / 1000;
            if (viptable != null)
            {
                string time = CSServerTime.Instance.FormatLongToTimeStr(randomTime, 3);
                mlb_time.text = CSString.Format(1297, viptable.id, time);
            }
            
        }
        if (randomTime <= 0)
        {
            mbtn_go.SetActive(true);
            mlb_time.gameObject.SetActive(false);
            randomTime = 0;
            mlb_time.text = "";
            if (Timer.Instance.IsInvoking(VipExperienceTimer))
                Timer.Instance.CancelInvoke(VipExperienceTimer);
        }
        if (second >= int.Parse(SundryTableManager.Instance.GetSundryEffect(606)))
            Close(); 
        second++;
    }

    private void OnEndClick(GameObject obj)
    {
        UIManager.Instance.CreatePanel<UIVIPPanel>();
        Close();
    }

    protected override void OnDestroy()
	{
        base.OnDestroy();
        if (Timer.Instance.IsInvoking(VipExperienceTimer))
            Timer.Instance.CancelInvoke(VipExperienceTimer);
        VipExperienceTimer = null;
        experienceCardInfo = null;
       
        viptable = null;
    }

    // public override bool ShowGaussianBlur
    // {
    //     get { return false; }
    // }
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }

    
}
