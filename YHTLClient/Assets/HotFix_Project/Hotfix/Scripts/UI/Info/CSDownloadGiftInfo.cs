using Main_Project.Script.Update;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSDownloadGiftInfo : CSInfo<CSDownloadGiftInfo>
{
    public bool DownloadReward
    {
        get
        {
            if (CSMainPlayerInfo.Instance.RoleExtraValues == null) return false;
            return CSMainPlayerInfo.Instance.RoleExtraValues.downloadReward;
        }
    }

    CSResUpdateManager resMgr;


    public override void Dispose()
    {
        if (resMgr != null)
            resMgr.UnRegUpdateAction(DownloadAction);
        resMgr = null;


    }

    public void Initialize()
    {
        resMgr = CSResUpdateManager.Instance;
        if (resMgr != null)
        {
            resMgr.RegUpdateAction(DownloadAction);
            DownloadAction(resMgr.CurBackDownloadByteNum, resMgr.BackDownloadByteNum);
        }
        
    }


    void DownloadAction(int curByteNum, int totalByteNum)
    {
        if (totalByteNum == 0 || (totalByteNum > 0 && curByteNum >= totalByteNum))
        {
            //下载已完成
            if (resMgr != null)
            {
                resMgr.UnRegUpdateAction(DownloadAction);
            }            
            mClientEvent.SendEvent(CEvent.DownloadResComplete);
        }
    }

    public bool CheckDownloadRewardRedpoint()
    {
        if (resMgr == null) return false;
        var curNum = resMgr.CurBackDownloadByteNum;
        var totalNum = resMgr.BackDownloadByteNum;
        if (totalNum == 0 || (totalNum > 0 && curNum >= totalNum))
        {
            return !DownloadReward;
        }

        return false;
    }

    public bool CanShowDownloadRewardPanel()
    {
        return !DownloadReward;
    }
}
