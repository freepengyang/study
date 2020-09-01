using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CSServerTime : Singleton<CSServerTime>
{
    bool isInitServerTime = false;
    //bool isNeedCheck = false;
    long clientBeginTime = 0;
    long curClientTime = 0;
    long serverBeginTime = 0;
    long curServerTime = 0;
    long lastNow = 0;//DateTime.UtcNow.Ticks
    long lastN = 0;//realtimeSinceStartup
    long delta = 0;

    //DateTime.UtcNow.Ticks 单位是100纳秒(10^-7 秒)  转换为毫秒（10^-3 秒） 要除以10000
    public static long BaseTimePoke = 621355968000000000;//System.DateTime(1970, 1, 1, 0, 0, 0)
    public static long ZoneTime = 28800;//东8区,+8小时
    public static int timepart = 8;//所在时区 默认东8区

    public static long ServernowTime;
    public static long ServerNowMs;//服务器当前时间毫秒
    public static float lastCheckTime;//最后校正时间

    public int ServerMergeCount = 0;
    public void Reset()
    {
        clientBeginTime = 0;
        curClientTime = 0;
        serverBeginTime = 0;
        curServerTime = 0;
        //isNeedCheck = false;
        isInitServerTime = false;
    }

    /// <summary>
    /// 服务器当前时间 和时区无关
    /// </summary>
    public static long ServerNowMsChecked
    {
        get
        {
            //Debug.LogErrorFormat("[passed]:{0} SyncTime:{1}", Time.realtimeSinceStartup - lastCheckTime, ServerNowMs);
            return ServerNowMs + (long)((Time.realtimeSinceStartup - lastCheckTime) * 1000);
        }
    }

    public void refreshTime(heart.Heartbeat rsp)
    {
        if (null != rsp) // 注意：这里假设服务器位于0时区
        {
            ServerNowMs = rsp.nowTime;
            lastCheckTime = Time.realtimeSinceStartup;
            //Debug.LogErrorFormat("[checkTime]:{0} ServerNowMs:{1}", lastCheckTime, ServerNowMs);
            ServernowTime = rsp.nowTime / 1000;
            // 获取unix time stamp
            long now = (DateTime.UtcNow.Ticks - BaseTimePoke) / 10000;
            long n = (long)(Time.realtimeSinceStartup * 1000);
            delta = now - rsp.nowTime;
            if (!isInitServerTime)
            {
                if (n - rsp.clientTime < 1000)//如果网络延迟超过1秒忽略这个初始化值，等下一个包
                {
                    lastNow = now;
                    lastN = n;
                    isInitServerTime = true;
                    clientBeginTime = rsp.clientTime;
                    serverBeginTime = rsp.nowTime;
                }
                else
                {
                    return;
                }
            }

            long clientD = (now - lastNow) - (n - lastN);
            lastNow = now;
            lastN = n;
            if (Math.Abs(clientD) > 2000)//防止进入游戏clientBeginTime和serverBeginTime已经成为定值时，改大了本地时间
            {
                Reset();
                return;
            }
            long delay = n - rsp.clientTime;//网络延迟+客户端卡顿
            curClientTime = rsp.clientTime;
            curServerTime = rsp.nowTime + delay;//排除网络延迟+客户端卡顿的影响
            //isNeedCheck = true;
        }
    }

    public void refreshTime(long serverTime)
    {
        if (serverTime == 0) return;
        long now = (DateTime.UtcNow.Ticks - BaseTimePoke) / 10000;
        delta = now - serverTime;
    }

    public void Update()
    {
    }

    /************************ Func ************************/
    /// <summary>
    /// 根据时区矫正本地时间
    /// </summary>
    public DateTime ServerNows
    {
        get
        {
            return new DateTime((TotalSeconds + ZoneTime) * 10000000 + BaseTimePoke);
        }
    }

    public DateTime ServerNowsMilli
    {
        get
        {
            return new DateTime((TotalMillisecond + ZoneTime * 1000) * 10000 + BaseTimePoke);
        }
    }

    /// <summary>
    /// 本地描述
    /// </summary>
    public long LocalSeconds
    {
        get
        {
            return TotalSeconds + ZoneTime;
        }
    }

    /// <summary>
    /// 距离下一天的秒数
    /// </summary>
    public long SecondsLeaveNextDay
    {
        get
        {
            return 86400 - LocalSeconds % 86400;
        }
    }

    /// <summary>
    /// 注意：始终记住这里假设服务器位于0时区,而服务器的所谓gmt time，也并非真正的gmt time，因为服务器把自己作为世界的中心了，好吧，就让他这么干吧！
    /// </summary>
    public long TotalSeconds
    {
        get { return ((DateTime.UtcNow.Ticks - BaseTimePoke) / 10000 - delta) / 1000; }
    }

    /// <summary>
    /// 毫秒 // 注意：始终记住这里假设服务器位于0时区,而服务器的所谓gmt time，也并非真正的gmt time，因为服务器把自己作为世界的中心了，好吧，就让他这么干吧！
    /// </summary>
    public long TotalMillisecond
    {
        get
        {
            return (DateTime.UtcNow.Ticks - BaseTimePoke) / 10000 - delta;
        }
    }

    public long LocalMillisecond
    {
        get
        {
            return (DateTime.UtcNow.Ticks - BaseTimePoke) / 10000 - delta + (ZoneTime * 3600 * 1000);
        }
    }

    public string GetNowTimeToTimeString()
    {
        DateTime weeHour = new DateTime(ServerNows.Year, ServerNows.Month, ServerNows.Day);
        long l = TotalSeconds - DateTimeToStamp(weeHour);
        return FormatLongToTimeStrHour(l);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="l">毫秒</param>
    /// <returns></returns>
    public string FormatLongToTimeStrMinusCurTime(long l)
    {
        l = l / 1000;
        return FormatLongToTimeStr(l - TotalSeconds);
    }

    public string FormatLongToTimeStrHour(long l)
    {
        long hour = 0;
        long minute = 0;
        long second = 0;
        second = l;

        if (second > 60)
        {
            minute = second / 60;
            second = second % 60;
        }
        if (minute > 60)
        {
            hour = minute / 60;
            minute = minute % 60;
        }
        return (hour.ToString("D2") + ":" + minute.ToString("D2"));
    }

    public string FormatLongToTimeStr(long l)
    {
        //string str = "";
        long hour = 0;
        long minute = 0;
        long second = 0;
        second = l;

        if (second > 60)
        {
            minute = second / 60;
            second = second % 60;
        }
        if (minute > 60)
        {
            hour = minute / 60;
            minute = minute % 60;
        }
        return (hour.ToString("D2") + ":" + minute.ToString("D2") + ":"
            + second.ToString("D2"));
    }

    public string FormatLongToTimeStrMinute(long l)
    {
        long minute = 0;
        long second = 0;
        second = l;

        if (second > 60)
        {
            minute = second / 60;
            second = second % 60;
        }
        if (minute > 60)
        {
            minute = minute % 60;
        }
        return (minute.ToString("D2") + ":" + second.ToString("D2"));
    }

    public string FormatLongToTimeStrHourOrMinute(long totalSecond)
    {
        if (totalSecond > 3600)
        {
            return (Mathf.CeilToInt(totalSecond / 3600f) + CSString.Format(419)).ToString();
        }
        else
        {
            return (Mathf.CeilToInt(totalSecond / 60f) + CSString.Format(420)).ToString();
        }
    }

    public string FormatLongToTimeStrMin(long l)
    {
        long minute = 0;
        long second = l;

        minute = second / 60;
        second = second % 60;

        return (minute.ToString("D2") + ":" + second.ToString("D2"));
    }

    public string FormatLongToTimeStr(long l, int mode)
    {
        string str = "";
        long days = 0;
        long hour = 0;
        long minute = 0;
        long second = 0;
        second = l;

        if (second >= 60)
        {
            minute = second / 60;
            second = second % 60;
        }
        if (minute >= 60)
        {
            hour = minute / 60;
            minute = minute % 60;
        }
        if (hour >= 24)
        {
            days = hour / 24;
            hour = hour % 24;
        }

        switch (mode)
        {
            case 1://格式：X天X小时X分X秒
                {
                    CSStringBuilder.Clear();
                    CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                    CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                    CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                    CSStringBuilder.Append(second.ToString("D2"), CSString.Format(423));
                    return CSStringBuilder.ToString();
                }
            case 2:
                {
                    CSStringBuilder.Clear();
                    if (days > 0) CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                    CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                    CSStringBuilder.Append(minute.ToString(), CSString.Format(424));
                    CSStringBuilder.Append(second.ToString(), CSString.Format(423));
                    return CSStringBuilder.ToString();
                }
            case 3:
                {
                    CSStringBuilder.Clear();
                    if (days > 0) CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                    if (hour > 0) CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                    if (minute > 0) CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                    if (second > 0) CSStringBuilder.Append(second.ToString(), CSString.Format(423));
                    return CSStringBuilder.ToString();
                }
            case 4:
                {
                    CSStringBuilder.Clear();
                    CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                    CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                    CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                    return CSStringBuilder.ToString();
                }
            case 5:                 //限时寻宝特殊要求 当小于一小时时  显示秒数
                {
                    CSStringBuilder.Clear();
                    CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                    CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                    CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                    if (hour <= 1)
                    {
                        CSStringBuilder.Append(second.ToString(), CSString.Format(423));
                    }
                    return CSStringBuilder.ToString();
                }
            case 6:
                {
                    CSStringBuilder.Clear();
                    if (days > 0) CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                    if (hour > 0) CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                    return CSStringBuilder.ToString();
                }
            case 7:
                {
                    CSStringBuilder.Clear();
                    if (minute > 0) CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                    if (second > 0) CSStringBuilder.Append(second.ToString(), CSString.Format(423));
                    return CSStringBuilder.ToString();
                }
            case 8://每日礼品
                {
                    CSStringBuilder.Clear();
                    if (days > 0) CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                    if (hour > 0) CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                    if (minute > 0) CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                    if (hour <= 1)
                    {
                        CSStringBuilder.Append(second.ToString(), CSString.Format(423));
                    }
                    return CSStringBuilder.ToString();
                }
            case 9:
                {
                    CSStringBuilder.Clear();
                    CSStringBuilder.Append(days > 0 ? days.ToString() : "", days > 0 ? CSString.Format(421) : "");
                    CSStringBuilder.Append(hour <= 0 && days <= 0 ? "" : hour.ToString(), hour <= 0 && days <= 0 ? "" : CSString.Format(419));
                    CSStringBuilder.Append(hour <= 0 && days <= 0 && minute <= 0 ? "" : minute.ToString(), hour <= 0 && days <= 0 && minute <= 0 ? "" : CSString.Format(422));
                    if (days <= 0)
                    {
                        CSStringBuilder.Append(second.ToString(), CSString.Format(423));
                    }
                    return CSStringBuilder.ToString();
                }
            case 10:
                {
                    CSStringBuilder.Clear();
                    if (days > 0)
                    {
                        CSStringBuilder.Append(days, CSString.Format(421), hour, CSString.Format(419));
                    }
                    else if (days <= 0 && hour > 0)
                    {
                        CSStringBuilder.Append(hour, CSString.Format(419), minute, CSString.Format(422));
                    }
                    else if (hour <= 0 && minute > 0)
                    {
                        CSStringBuilder.Append(minute, CSString.Format(422), second, CSString.Format(423));
                    }
                    else
                    {
                        CSStringBuilder.Append(second.ToString("D2"), CSString.Format(423));
                    }
                    return CSStringBuilder.ToString();

                }
            case 11:
                {
                    CSStringBuilder.Clear();
                    if (days > 0)
                    {
                        CSStringBuilder.Append(days, CSString.Format(421));
                    }
                    else if (days <= 0 && hour > 0)
                    {
                        CSStringBuilder.Append(hour, CSString.Format(419));
                    }
                    else if (days <= 0 && hour <= 0 && minute > 0)
                    {
                        CSStringBuilder.Append(minute, CSString.Format(422));
                    }
                    else if (days <= 0 && hour <= 0 && minute <= 0 && second > 0)
                    {
                        CSStringBuilder.Append(second, CSString.Format(423));
                    }
                    return CSStringBuilder.ToString();
                }
            case 12:
                {
                    CSStringBuilder.Clear();
                    if (days > 0)
                    {
                        CSStringBuilder.Append(days, CSString.Format(421), hour, CSString.Format(419));
                    }
                    else if (days <= 0 && hour > 0)
                    {
                        CSStringBuilder.Append(hour, CSString.Format(419), minute, CSString.Format(422));
                    }
                    else if (hour <= 0 && minute > 0)
                    {
                        CSStringBuilder.Append(minute, CSString.Format(422), second > 0 ? second.ToString() : "", second > 0 ? CSString.Format(423) : "");
                    }
                    else
                    {
                        CSStringBuilder.Append(second, CSString.Format(423));
                    }
                    return CSStringBuilder.ToString();
                }
            case 13:
                CSStringBuilder.Clear();
                break;
            case 14:
                CSStringBuilder.Clear();
                if (days > 0)
                {
                    CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                    CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                    CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                }
                else
                {
                    CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                    CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                    CSStringBuilder.Append(second.ToString(), CSString.Format(423));
                }
                return CSStringBuilder.ToString();
            case 15:
                {
                    CSStringBuilder.Clear();
                    if (days > 0) CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                    if (hour > 0) CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                    if (minute > 0) CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                    if (second >= 0) CSStringBuilder.Append(second.ToString(), CSString.Format(423));
                    return CSStringBuilder.ToString();
                }
            case 16:
                CSStringBuilder.Clear();
                if (days > 0) CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                if (hour > 0) CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                if (minute > 0) CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                // if (second > 0) CSStringBuilder.Append(second.ToString(), CSString.Format(423));
                return CSStringBuilder.ToString();
            case 17:
                CSStringBuilder.Clear();
                if (days > 0) CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                if (hour > 0) CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                if (minute > 0) CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                if (second >= 0) CSStringBuilder.Append(second.ToString("D2"), CSString.Format(423));
                return CSStringBuilder.ToString();
            case 18:
                CSStringBuilder.Clear();
                if (second>=0 && days==0 && hour==0 && minute==0)
                {
                    CSStringBuilder.Append(second.ToString(), CSString.Format(423));
                    return CSStringBuilder.ToString();
                }
                if (days > 0) CSStringBuilder.Append(days.ToString(), CSString.Format(421));
                if (hour > 0) CSStringBuilder.Append(hour.ToString(), CSString.Format(419));
                if (minute > 0) CSStringBuilder.Append(minute.ToString(), CSString.Format(422));
                // if (second > 0) CSStringBuilder.Append(second.ToString(), CSString.Format(423));
                return CSStringBuilder.ToString();
			case 19:
				CSStringBuilder.Clear();
				if(days > 0) CSStringBuilder.Append(days.ToString("D2"), ":");
				CSStringBuilder.Append(hour.ToString("D2"), ":");
				CSStringBuilder.Append(minute.ToString("D2"), ":");
				CSStringBuilder.Append(second.ToString("D2"));
				return CSStringBuilder.ToString();
		}
        return str;
    }

    /// <summary>
    /// 时间戳转为C#格式时间
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime StampToDateTime(double timeStamp)
    {
        return new System.DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(timeStamp + ZoneTime * 1000);
    }

    public static DateTime StampToDateTimeForSecond(long timeStamp)
    {
        return new System.DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timeStamp / 1000 + ZoneTime);
    }
    static System.DateTime zeroClock;
    public static long GetZeroClockGapSeconds()
    {
        zeroClock = new System.DateTime(Now.Year, Now.Month, Now.Day, 0, 0, 0);
        long gap = 24 * 60 * 60 - (CSServerTime.Instance.TotalSeconds - DateTimeToStamp(zeroClock));
        return gap;
    }
    /// <summary>
    /// DateTime时间格式转换为Unix时间戳格式
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static long DateTimeToStamp(System.DateTime time)
    {
        return (long)(time - new System.DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds - ZoneTime;
    }

    public static long DateTimeToStampForMilli(System.DateTime time)
    {
        return (long)(time - new System.DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds - ZoneTime * 1000;
    }

    public TimeSpan GetSurplusTime(DateTime nowTime, DateTime endTime)
    {
        TimeSpan ts1 = new TimeSpan(nowTime.Ticks);
        TimeSpan ts2 = new TimeSpan(endTime.Ticks);
        return ts2 - ts1;
    }
    /// <summary>
    /// 得到还需要多少时间
    /// </summary>
    /// <param name="cd"></param>
    /// <returns></returns>
    public TimeSpan GetLastTimeSeconds(long cd)
    {
        DateTime start = ServerNows;

        DateTime last = new DateTime((cd + ZoneTime) * 10000000 + BaseTimePoke);

        TimeSpan ts = last - start;

        return ts;
    }

    public static int DayOfWeek { get { return (int)Now.DayOfWeek; } }

    public static int CurTime { get { return Now.Hour * 100 + Now.Minute; } }

    public static DateTime Now { get { return Instance.ServerNows; } }

    public static string GetWeekDayForString(int weekDay)
    {
        string str = "";
        switch (weekDay)
        {
            case 1: str = CSString.Format(425); break;
            case 2: str = CSString.Format(426); break;
            case 3: str = CSString.Format(427); break;
            case 4: str = CSString.Format(428); break;
            case 5: str = CSString.Format(429); break;
            case 6: str = CSString.Format(430); break;
            case 7: str = CSString.Format(431); break;
        }
        return str;
    }

    //两个周天之间相差秒数
    public static long GetWeekDayDiffeSeconnds(int weekDay)
    {
        int d = GetWeekDiffeDay(weekDay);
        return (long)(86400 * (d) - Now.TimeOfDay.TotalSeconds);
    }

    /// <summary>
    /// 两个周天之间相差天数
    /// </summary>
    /// <param name="weekDay"></param>
    /// <returns></returns>
    public static int GetWeekDiffeDay(int weekDay)
    {
        int w = DayOfWeek == 0 ? 7 : DayOfWeek;

        if (weekDay + 1 - w <= 0)
        {
            return 1;
        }
        return weekDay + 1 - w;
    }

    public static bool IsTodayActivityForWeek(string dayOfWeek) { return dayOfWeek.Contains(DayOfWeek.ToString()); }

    public static bool IsTodayActivityByBanList(string unActivityDayOfWeeok) { return !unActivityDayOfWeeok.Contains(DayOfWeek.ToString()); }

    public static bool IsOpening(string dayOfWeek, string openTime, uint lastTime)
    {
        string[] startTime = openTime.Split('#');
        int h = int.Parse(startTime[0]);
        int m = int.Parse(startTime[1]);
        int startSecond = (h * 60 + m) * 60;
        int now = (Now.Hour * 60 + Now.Minute) * 60 + Now.Second;
        int endSecond = startSecond + (int)lastTime;
        return IsTodayActivityForWeek(dayOfWeek) && now >= startSecond && now < endSecond;
    }

    public static bool IsOpeningByTime(string openTime, uint lastTime)
    {
        string[] startTime = openTime.Split('#');
        int h = int.Parse(startTime[0]);
        int m = int.Parse(startTime[1]);
        int startSecond = (h * 60 + m) * 60;
        int now = (Now.Hour * 60 + Now.Minute) * 60 + Now.Second;
        int endSecond = startSecond + (int)lastTime;
        return now >= startSecond && now < endSecond;
    }

    /// <summary>
    /// 返回当前时间与传入时间相差的天数
    /// </summary>
    /// <param name="time">需要计算时间 </param>
    /// <returns></returns>
    public int GetDayByMinusCurTime(long time)
    {
        DateTime date = StampToDateTime(time); 
        DateTime nextdate = StampToDateTime(TotalMillisecond);
        
         if (date.Year == nextdate.Year)
         {
             return nextdate.DayOfYear - date.DayOfYear;
         }
         else
         {
             if (date.Year % 100 == 0 && date.Year % 400 != 0 )
             {
                 return nextdate.DayOfYear- date.DayOfYear + 365;
             }
             
             if (date.Year % 4 != 0 )
             {
                 return nextdate.DayOfYear- date.DayOfYear + 365;
             }
             else
             {
                 return nextdate.DayOfYear - date.DayOfYear + 366;
             }
         }
    }

    
    /// <summary>
    /// 判断时间是否是本周内
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool IsWeekByMinusCurTime(long time)
    {
        DateTime date = StampToDateTime(time); 
        DateTime serverDt = StampToDateTime(TotalMillisecond);
        int day = Math.Abs(GetDayByMinusCurTime(time));
        int dateWeek = date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek;
        int serverWeek = serverDt.DayOfWeek == 0 ? 7 : (int)serverDt.DayOfWeek;

        if (day >= 7)
            return false;
        else
        {
            if (date > serverDt)
            {
                return dateWeek >= serverWeek;
            }
            else
            {
                return dateWeek <= serverWeek;
            }

            
        }
    }


}
