using instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using TABLE;
using vip;

public enum VipTasteCard
{
    start, //体验开始
    end, //体验结束
}

public class CSVipInfo : CSInfo<CSVipInfo>
{
    FirstRechargeInfoResponse _firstRechargeInfo = null; //首次充值数据

    VipInfo vipInfo = null; //VIP数据
    ExperienceCardInfo _experienceCardInfo;
    private bool isRechargeFirstClick = true;// 是否首次点击首充按钮
    public FirstRechargeInfoResponse FirstRechargeInfo
    {
        get => _firstRechargeInfo;

        set
        {
            _firstRechargeInfo = value;
            mClientEvent.SendEvent(CEvent.FirstRechargeInfoChange);
        }
    }
    public VipInfo VipInfo
    {
        get => vipInfo != null ? vipInfo : new VipInfo();
        set
        {
            vipInfo = value;
            mClientEvent.SendEvent(CEvent.VipInfoChange);
        }
    }

    public ExperienceCardInfo ExperienceCardInfo
    {
        get => _experienceCardInfo;
        set
        {
            _experienceCardInfo = value;
            mClientEvent.SendEvent(CEvent.VipInfoChange);
            //mClientEvent.SendEvent(CEvent.VipExperienceInfoChange);
        }
    }

    #region 首次充值

    /// <summary>
    /// 是否进行过首充
    /// </summary>
    /// <returns></returns>
    public bool IsFirstRecharge()
    {
        if (FirstRechargeInfo == null)
            return false;
        else
            return true;
    }

    /// <summary>
    /// 判断首充是否开始
    /// </summary>
    public bool IsFirstRechargeFinish()
    {
        if (FirstRechargeInfo == null)
            return true;
        //Debug.Log("ffff" + _info.firstRechargeInfo.receiveDay);
        if (GetRechargeDay() >= 3 && FirstRechargeInfo.firstRechargeInfo.receiveDay == 7) //二进制111 = 7 表示奖励都被领取
            return false;
        return true;
    }

    /// <summary>
    /// 返回首充gameModel
    /// </summary>
    /// <returns></returns>
    public int GetFirstRechargeGameModel()
    {
        return 21000;
    }


    /// <summary>
    /// 可领取的奖励的天数
    /// </summary>
    /// <returns></returns>
    private int rewardDay = -1;

    /// <summary>
    /// 获取可以领取奖励的天数
    /// </summary>
    /// <returns></returns>
    public int GetRechargeDay()
    {
        if (FirstRechargeInfo == null)
            return 0;


        rewardDay = CSServerTime.Instance.GetDayByMinusCurTime(FirstRechargeInfo.firstRechargeInfo.firstRechargeTime) + 1;
        // if (rewardDay != -1)
        // {
        //     return rewardDay;
        // }
        // else
        // {
        //     
        // }

        return rewardDay;
    }


    /// <summary>
    /// 判断是否已领取充值
    /// </summary>
    /// <param name="day">第几天</param>
    /// <returns></returns>
    public bool IsRecharge(int day)
    {
        if (day == 0 || FirstRechargeInfo == null)
            return false;
        if (((FirstRechargeInfo.firstRechargeInfo.receiveDay >> (day - 1)) & 1) == 0)
            return false;
        else
            return true;
    }

    /// <summary>
    /// 判断是否完成奖励
    /// </summary>
    /// <returns></returns>
    public bool IsFinishRechargeFirst()
    {
        if (FirstRechargeInfo == null || FirstRechargeInfo.firstRechargeInfo == null)
        {
            return false;
        }

        if (FirstRechargeInfo.firstRechargeInfo.receiveDay == 7)
        {
            return true;
        }
        return false;
    }


    public bool RechargeFirstRedCheck()
    {
        if (!IsFirstRecharge())
        {
            if (isRechargeFirstClick)
            {
                isRechargeFirstClick = false;
                return true;
            }
            else
                return false;
        }
        else
        {
            int rewardDay = GetRechargeDay();

            for (int i = 1; i <= rewardDay; i++)
            {
                if (!IsRecharge(i))
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion

    #region vip信息
    Dictionary<int, VIP> vipList = new Dictionary<int, VIP>();
    private int oldviplv;
    /// <summary>
    /// 返回 从第一级到当前等级 +3级的vip信息列表，如果vip等级+3级超过最大等级，那么读取最大等级
    /// </summary>
    /// <param name="lv">当前等级</param>
    public Dictionary<int, VIP> GetvipListInfo()
    {


        vipList.Clear();
        int curviplv = CSMainPlayerInfo.Instance.VipLevel;
        var arr = VIPTableManager.Instance.array.gItem.handles;
        int num = curviplv + 3 < arr.Length ? curviplv + 3 : arr.Length;

        int i = 1;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            if ((arr[k].Value as VIP).id == 0)
                continue;

            if (i <= num)
                vipList.Add(i, arr[k].Value as VIP);
            else
                break;
            i++;
        }
        return vipList;

    }

    public void GetVipTasteCardInfo(ExperienceCardInfo info)
    {

        ExperienceCardInfo = info;

        UIManager.Instance.CreatePanel<UIVIPExperiencePanel>();

        //UIManager.Instance.CreatePanel<UIVIPPanel>(VipTasteCard.start);
    }

    /// <summary>
    /// 判断体验卡是否过期
    /// </summary>
    /// <returns></returns>
    public bool IsVipTasteExpired()
    {
        //如果 数据为空，则认为没有体验卡
        if (ExperienceCardInfo == null)
            return true;

        if (CSServerTime.Instance.TotalMillisecond - ExperienceCardInfo.endTime >= 0)
            return true;
        return false;
    }


    public bool VipInfoRedPointChenk()
    {
        int viplv = CSMainPlayerInfo.Instance.VipLevel;
        var iter = vipRwardInfos.GetEnumerator();
        while (iter.MoveNext())
        {
            // if (iter.Current.Key == 0)
            // {
            //     continue;
            // }

            if (iter.Current.Key > vipInfo.vipLevel)
            {
                continue;
            }
            if (iter.Current.Value)
            {
                return true;
            }

        }
        return false;
    }

    public Dictionary<int, bool> vipRwardInfos;
    int vipExp = 0;
    public void SetRewardInfos(VipInfo info)
    {
        if (vipRwardInfos == null)
        {
            vipRwardInfos = new Dictionary<int, bool>();
        }
        else
        {
            vipRwardInfos.Clear();
        }
        vipExp = info.exp;
        int realLv = info.vipLevel;


        for (int i = 0; i < info.reward.Count; i++)
        {
            int temp = UtilityMath.GetProPertyValue(info.reward[i]);//获取低32
            int lv = UtilityMath.GetProPerty(info.reward[i]); //获取高32
            bool isreward = false;
            if (lv == 0)
            {
                continue;
            }
            if (lv > realLv)
            {
                isreward = false;
            }
            else
            {
                isreward = temp == 0 ? true : false;
            }

            vipRwardInfos.Add(lv, isreward);
        }

    }





    /// <summary>
    /// 获得第一个没有领取的奖励
    /// </summary>
    /// <returns></returns>
    public int GetFirstRwardLv()
    {
        if (vipRwardInfos == null)
        {
            return 0;
        }

        var iter = vipRwardInfos.GetEnumerator();
        int i = 0;
        while (iter.MoveNext())
        {
            if (iter.Current.Key > vipInfo.vipLevel)
            {
                continue;
            }

            i++;

            if (iter.Current.Value)
            {
                return iter.Current.Key;
            }
        }
        return i;
    }


    #endregion

    #region 累计充值

    private AccumulatedRechargeInfo _accumulatedRechargeInfo;


    public void GetAccumulateInfo(AccumulatedRechargeInfo info)
    {
        _accumulatedRechargeInfo = info;
        HotManager.Instance.EventHandler.SendEvent(CEvent.AddUpInfoChange);
    }

    private int oldloop;
    private ILBetterList<RECHARGEREWARD> listrewards;

    public ILBetterList<RECHARGEREWARD> GetRewardInfoList()
    {
        if (_accumulatedRechargeInfo == null)
            return null;
        int loop = _accumulatedRechargeInfo.loop;
        if (listrewards == null)
        {
            listrewards = new ILBetterList<RECHARGEREWARD>(10);
        }
        else
        {
            if (loop == oldloop)
            {
                return listrewards;
            }
            else
            {
                listrewards.Clear();
            }
        }

        var arr = RechargeRewardTableManager.Instance.array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var v = arr[k].Value as RECHARGEREWARD;
            if (v.loop == loop && v.type == 1)
            {
                listrewards.Add(v);
            }
        }

        oldloop = loop;
        return listrewards;
    }

    public int GetAddUpDay()
    {
        if (_accumulatedRechargeInfo == null)
        {
            return 0;
        }

        return _accumulatedRechargeInfo.accumulatedRechargeDay;
    }

    /// <summary>
    /// 获得第一个可领取未领取的奖励
    /// </summary>
    /// <returns></returns>
    public int GetFirstReward()
    {
        if (_accumulatedRechargeInfo == null || listrewards == null)
        {
            return 0;
        }

        int day = _accumulatedRechargeInfo.accumulatedRechargeDay;
        for (int i = 0; i < listrewards.Count; i++)
        {
            int id = listrewards[i].id;
            if (day >= listrewards[i].need && !_accumulatedRechargeInfo.receiveIds.Contains(id))
            {
                return i;
            }

        }

        return 0;
    }

    public RepeatedField<int> GetReceiveIds()
    {
        if (_accumulatedRechargeInfo == null)
        {
            return null;
        }

        return _accumulatedRechargeInfo.receiveIds;
    }

    public bool AddUpReChargeRedCheck()
    {
        if (_accumulatedRechargeInfo == null)
        {
            return false;
        }

        var list = GetRewardInfoList();
        for (int i = 0; i < list.Count; i++)
        {
            if (_accumulatedRechargeInfo.accumulatedRechargeDay >= list[i].need
                && !_accumulatedRechargeInfo.receiveIds.Contains(list[i].id))
                return true;
        }

        return false;
    }

    #endregion

    public override void Dispose()
    {

    }


}
