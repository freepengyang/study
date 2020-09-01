using activity;
using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

using lifelongfund;
using UnityEngine;

public class CSlifeTimeFundInfo : CSInfo<CSlifeTimeFundInfo>
{
    private LifelongFundInfo _lifelongFundInfo;
    private PoolHandleManager _poolHandleManager = new PoolHandleManager();
    private List<rewardClassData> rewardidList = new List<rewardClassData>();
    public LifelongFundInfo LifelongFundInfo
    {
        get
        {
            return _lifelongFundInfo;
        }
    
        set
        {
            _lifelongFundInfo = value;
            mClientEvent.SendEvent(CEvent.LifeTimeFundChange);
        }
    }

    public bool isFirstTabClick = true;
    
    public bool isFirstBtnClick = true;

    private int curScoreIndex = 0;

    /// <summary>
    /// 当前积分的索引值
    /// </summary>
    public int CurScoreIndex
    {
        get => curScoreIndex;
        set => curScoreIndex = value;
    }

    /// <summary>
    /// 获得第一个奖励,如果没有那么返回最后一个
    /// </summary>
    /// <returns></returns>
    public int GetFirstReward()
    {
        var list = LifelongFundInfo.unreceivedRewards;
        //CSBetterLisHot<int> hotlist

        if (LifelongFundInfo.isBuy == false)
        {
            return 0;
        }
        
        if (list.Count > 0)
        {
            for (int i = 0; i < rewardidList.Count; i++)
            {
                if (rewardidList[i].score == list[0])
                {
                    return i;
                }
                
            }

            return 0;
        }
        else
        {

            for (int i = 0; i < rewardidList.Count; i++)
            {
                if (rewardidList[i].score == rewardidList[i].GetScore )
                {
                    return i + 1;
                }
            }
            
            return 0;
        }

    }
    public void SetTaskData(RepeatedField<FundTaskInfo> info)
    {
        //这里终身基金修改传列表
        if (LifelongFundInfo !=null)
        {
            //var funtaskinfo =  LifelongFundInfo.fundTaskInfos.FirstOrNull(x => x.taskId == info.taskId);
            //funtaskinfo.curProgress = info.curProgress;
            //funtaskinfo.taskState = info.taskState;
            mClientEvent.SendEvent(CEvent.LifeTimeFundChange); 
        }
    }

    public void SetRewardData(RepeatedField<int> unreceivedRewards)
    {
        _lifelongFundInfo.unreceivedRewards.Clear();
        for (int i = 0; i < unreceivedRewards.Count; i++)
        {
            _lifelongFundInfo.unreceivedRewards.Add(unreceivedRewards[i]);
        }
        mClientEvent.SendEvent(CEvent.LifeTimeFundRewardChange);
    }
    
    /// <summary>
    /// 获取 显示的奖励列表
    /// </summary>
    /// <returns></returns>
    public  List<rewardClassData> GetRewardList()
    {
        
        for (int i = 0; i < rewardidList.Count; i++)
        {
            _poolHandleManager.Recycle(rewardidList[i]);
        }
        
        rewardidList.Clear();
        
        int point = LifelongFundInfo.curPoint;
        //int loopid = JijinRewardTableManager.Instance.GetLoopId();
        //int downid = JijinRewardTableManager.Instance.GetDownId();
        int beforeNum = int.Parse(SundryTableManager.Instance.GetSundryEffect(623));
        int nextNum = int.Parse(SundryTableManager.Instance.GetSundryEffect(624));
        int needIngegral = int.Parse(SundryTableManager.Instance.GetSundryEffect(629));
        
        //初始显示的积分值 将玩家的积分值减去余数  再减去追溯的总积分 如果小于表中第一个积分,那么显示第一个积分, 
        int curscore = point - (point % needIngegral); 
        int tempPoint = curscore - (beforeNum * needIngegral);
        tempPoint = tempPoint > needIngegral ? tempPoint : needIngegral;  
        int tempId = JijinRewardTableManager.Instance.GetIdByScore(tempPoint);

        List<int> loopList = JijinRewardTableManager.Instance.GetLoopList();
        int max = loopList[loopList.Count - 1];
        for (int i = 0; i < beforeNum + nextNum; i++)
        {
            rewardClassData rewardclass;
            
            //缓存当前积分索引
            if (tempPoint == curscore)
            {
                curScoreIndex = i;
            }
            rewardclass = _poolHandleManager.GetCustomClass<rewardClassData>(); 
            
            rewardclass.id = tempId;
            rewardclass.score = tempPoint;
            rewardclass.GetScore = curscore;
            rewardidList.Add(rewardclass);
            
            tempId++;
            //如果id > down的id 返回loop标志位 
            if (tempId > max)
            {
                tempId = loopList[0];
            }
            tempPoint += needIngegral;
            if (tempPoint > point + nextNum*needIngegral)
            {
                break;
            }
        }
        return rewardidList;
        
    }

    public bool RedCheck()
    {
        if (LifelongFundInfo == null)
        {
            return false;
        }
        if (!LifelongFundInfo.isBuy)
        {
            return isFirstTabClick; 
        }

        for (int i = 0; i < LifelongFundInfo.fundTaskInfos.Count; i++)
        {
            if (LifelongFundInfo.fundTaskInfos[i].taskState == 1)
            {
                return true;
            }
        }


        if (LifelongFundInfo.unreceivedRewards.Count>0)
        {
            return true;
        }
        
        return false;
    }


    public override void Dispose()
    {
        _lifelongFundInfo = null;
        rewardidList = null;
        _poolHandleManager.RecycleAll();
    }
}

public class rewardClassData : IDispose
{
    public int id; //奖励对应的表id
    public int score; //积分
    //public bool isCurScore; //是否是当前积分达到的奖励
    public int GetScore;//玩家达到的最高积分
    public bool isMax = false;

    public void CopyData(rewardClassData data)
    {
        id = data.id;
        score = data.score;
        GetScore = data.GetScore;
    }

    public void Dispose()
    {
      
    }
}
