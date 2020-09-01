using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TABLE;
using activity;

public class CSSevenDayManager : CSPool.IPoolItem
{
    UISprite msp_lock;
    UISprite msp_complete;
    UILabel mlb_name;
    public void OnTabBar(Transform trans, NEWBIEACTIVITY newbie) {
        

        //if (msp_lock == null)
        msp_lock = UtilityObj.Get<UISprite>(trans, "sp_lock");
        //if (msp_complete == null)
        msp_complete = UtilityObj.Get<UISprite>(trans, "sp_complete");
        //if (mlb_name == null)
        mlb_name = UtilityObj.Get<UILabel>(trans, "lb_name");
        
        msp_lock.gameObject.SetActive(CSActivityInfo.Instance.GetSevenDayTime() >= newbie.group);//判断该tab任务是否开启
        msp_complete.gameObject.SetActive(CSActivityInfo.Instance.IsFinishByType(newbie.group));//判断该分页任务是否已完成
        mlb_name.text = newbie.desc1;
    }

    UILabel mlb_point;
    Transform mtrans_check;
    Transform mtrans_select;

    public void OnGridBar(Transform trans, SevenDayData sevenDayData,int index)
    {
        var dic = NewbieActivityScheduleTableManager.Instance.array.gItem.id2offset;

        //if (!mlb_point)
            mlb_point = UtilityObj.Get<UILabel>(trans, "lb_point");
        //if (!mtrans_check)
            mtrans_check = UtilityObj.Get<Transform>(trans, "check");
        //if (!mtrans_select)
            mtrans_select = UtilityObj.Get<Transform>(trans, "select");

        //添加事件

        TABLE.NEWBIEACTIVITYSCHEDULE item = dic[index + 1].Value as TABLE.NEWBIEACTIVITYSCHEDULE;
        bool isFinish = sevenDayData.score >= item.requiresSore;
        bool isReceive = sevenDayData.scoreRewards.Contains(index + 1);
        mtrans_check.gameObject.SetActive(isFinish && isReceive);
        mtrans_select.gameObject.SetActive(isFinish && !isReceive);
        mlb_point.text = item.requiresSore.ToString();
    }


    public void OnRecycle()
    {
        throw new System.NotImplementedException();
    }
}
