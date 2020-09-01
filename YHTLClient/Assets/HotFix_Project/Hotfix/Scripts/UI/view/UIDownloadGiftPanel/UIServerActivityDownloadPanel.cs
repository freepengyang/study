using System.Collections;
using System.Collections.Generic;
using Main_Project.Script.Update;
using UnityEngine;


public partial class UIServerActivityDownloadPanel : UIBasePanel
{

    List<UIItemBase> itemList;

    float progressValue = 0f;
    string curPercent = string.Empty;
    string curByteStr = string.Empty;
    string totalByteStr = string.Empty;


    bool isBackDownloadComplete;


    bool DownloadReward
    {
        get
        {
            return CSDownloadGiftInfo.Instance.DownloadReward;
        }
    }


    public override void Init()
    {
        base.Init();

        CSEffectPlayMgr.Instance.ShowUITexture(mbanner17, "banner17");

        //mClientEvent.AddEvent(CEvent.DownloadFinish, OnDownloadComplete);主工程不能向热更工程发事件
        //mClientEvent.AddEvent(CEvent.GotDownloadSouvenir, OnGotReward);

        CSResUpdateManager.Instance.RegUpdateAction(RefreshProgress);
                

        RefreshProgress(CSResUpdateManager.Instance.CurBackDownloadByteNum, CSResUpdateManager.Instance.BackDownloadByteNum);

        mbtn_get.onClick = GetBtnClick;
    }

    public override void Show()
    {
        base.Show();

        ShowRewards();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mbanner17);
        if (itemList != null)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(itemList);
        }
        if (CSResUpdateManager.Instance != null)
            CSResUpdateManager.Instance.UnRegUpdateAction(RefreshProgress);
        base.OnDestroy();
    }


    public override void OnHide()
    {
        if (itemList != null)
        {
            UIItemManager.Instance.RecycleItemsFormMediator(itemList);
        }
        base.OnHide();
    }


    void ShowRewards()
    {
        if (itemList == null) itemList = new List<UIItemBase>();
        string box = SundryTableManager.Instance.GetSundryEffect(669);
        int id = 0;
        if (int.TryParse(box, out id))
        {
            Utility.GetItemByBoxid(id, mGrid_rewards, ref itemList, itemSize.Size64);
        }
        
    }

   

    void RefreshProgress(int curByteNum, int totalByteNum)
    {
        if (totalByteNum != 0 && curByteNum < totalByteNum)
        {
            progressValue = (float)curByteNum / (float)totalByteNum;
            curPercent = (progressValue * 100).ToString("F2");
            curByteStr = ((float)curByteNum / 1024f / 1024f).ToString("F2");
            totalByteStr = ((float)totalByteNum / 1024f / 1024f).ToString("F2");
            mslider_progress.value = progressValue;
            mlb_progress.text = string.Format("{0}%    {1}/{2}Mb", curPercent, curByteStr, totalByteStr);

            isBackDownloadComplete = false;
        }
        else
        {
            mslider_progress.value = 1f;
            mlb_progress.text = "100%";
            isBackDownloadComplete = true;
        }

        mobj_redpoint.SetActive(isBackDownloadComplete && !DownloadReward);
    }
    

    void GetBtnClick(GameObject go)
    {
        if (!DownloadReward && isBackDownloadComplete)
        {
            Net.ReqGetDownloadRewardMessage();
        }
        else
        {
            if (DownloadReward)
            {
                UtilityTips.ShowRedTips("奖励已领取");//奖励已领取
            }
            else
            {
                UtilityTips.ShowRedTips(1315);//下载未完成
            }            
        }
    }


    private void RefreshButton()
    {
        if ((!DownloadReward) && isBackDownloadComplete)
        {
            //sp_getReward.color = Color.white;
            //lab_getReward.color = UtilityColor.GetColor(ColorType.CommonButtonGreen);
            //effect.SetActive(true);
            //FinishDownload();
        }
        else
        {
            //sp_getReward.color = Color.black;
            //lab_getReward.color = UtilityColor.GetColor(ColorType.CommonButtonGrey);
            //effect.SetActive(false);
        }
    }
}
