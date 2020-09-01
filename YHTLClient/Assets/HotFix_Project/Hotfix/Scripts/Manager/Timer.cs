using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Schedule
{
    float mRepeatRatio = 0f;
    float mTime = 0.0001f;
    float mLastTime = 0f;
    System.Action<Schedule> mDelegate;
    bool mCanceled = false;
    bool mUseWorldTimeScale = false;

    public bool Canceled
    {
        get
        {
            return mCanceled;
        }
    }

    public float Time
    {
        get { return mTime; }
    }

    public float LastTime
    {
        get { return mLastTime; }
        set { mLastTime = value; }
    }

    public float RepeatRatio
    {
        get { return mRepeatRatio; }
    }

    public bool UsingWorldTimeScale
    {
        get { return mUseWorldTimeScale; }
    }

    public System.Action<Schedule> Callback
    {
        get { return mDelegate; }
    }

    public Schedule() { }

    public Schedule(float t, System.Action<Schedule> callback)
    {
        mTime = t;
        mLastTime = t;
        mDelegate = callback;
    }

    public Schedule(float t, bool worldTimeScale, System.Action<Schedule> callback)
    {
        mTime = t;
        mLastTime = t;
        mUseWorldTimeScale = worldTimeScale;
        mDelegate = callback;
    }

    public Schedule(float time, float repeatRatio, System.Action<Schedule> callback)
    {
        mTime = time;
        mLastTime = time;
        mRepeatRatio = repeatRatio;
        mDelegate = callback;
    }

    public void Cancel()
    {
        mCanceled = true;
    }

    public void ResetTime(float t)
    {
        mLastTime = t;
    }
}

public class Timer
{
    float InGameTimeScale = 1.0f; //时间缩放比
    float deltaTime; //每帧时间间隔
    float tmpTime;

    public static Timer Instance
    {
        get { return msSingleton; }
    }

    static readonly Timer msSingleton = new Timer();

    public Schedule SetSchedule(float time, System.Action<Schedule> func)
    {
        Schedule sc = new Schedule(time, func);
        mFront.Add(sc);
        return sc;
    }

    public Schedule SetSchedule(float time, bool usingWorldTimeScale, System.Action<Schedule> func)
    {
        Schedule sc = new Schedule(time, usingWorldTimeScale, func);
        mFront.Add(sc);
        return sc;
    }

    /// <summary>
    /// 延迟time后执行回调
    /// </summary>
    /// <param name="time"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public Schedule Invoke(float time, System.Action<Schedule> func)
    {
        return SetSchedule(time, func);
    }

    /// <summary>
    /// 延迟time后执行回调，此后每repeatRatio执行一次回调
    /// 注意关闭回收
    /// </summary>
    /// <param name="time"></param>
    /// <param name="repeatRatio"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public Schedule InvokeRepeating(float time, float repeatRatio, System.Action<Schedule> func)
    {
        Schedule sc = new Schedule(time, repeatRatio, func);
        mFront.Add(sc);
        return sc;
    }

    public void RemoveSchedule(Schedule sc)
    {
        if (sc == null)
        {
            return;
        }

        mFront.Remove(sc);
        mBack.Remove(sc);
    }

    public void SetSchedule(Schedule sc)
    {
        if (mBack.Contains(sc))
        {
            return;
        }

        if (!mFront.Contains(sc))
        {
            mFront.Add(sc);
        }
    }

    public void Update()
    {
        if (mFront.Count > 0)
        {
            mBack.AddRange(mFront);
            mFront.Clear();
        }

        deltaTime = Time.deltaTime;

        for (int i = 0; i < mBack.Count; i++)
        {

            if (mBack[i].Canceled)
            {
                mGarbrage.Add(mBack[i]);
                continue;
            }
            Schedule sc = mBack[i];

            tmpTime = sc.LastTime;
            // TODO:  if game is pause,InGameTimeScale value is 0.0f;
            tmpTime -= sc.UsingWorldTimeScale ? (deltaTime * InGameTimeScale) : deltaTime;
            //tmpTime -= dt;
            sc.ResetTime(tmpTime);
            if (tmpTime <= 0)
            {
                if (sc.Callback != null)
                {
                    sc.Callback(sc);
                }

                if (sc.RepeatRatio > 0)
                {
                    sc.ResetTime(sc.RepeatRatio);
                }
                else
                {
                    // 如果重置了时间，不要放进去
                    if (sc.LastTime <= 0)
                    {
                        mGarbrage.Add(sc);
                    }
                }
            }
        }

        for (int i = 0; i < mGarbrage.Count; i++)
        {
            if (mBack.Contains(mGarbrage[i]))
            {
                mBack.Remove(mGarbrage[i]);
            }
        }
        mGarbrage.Clear();
    }

    /// <summary>
    /// 是否在执行
    /// </summary>
    /// <param name="schedule"></param>
    /// <returns></returns>
    public bool IsInvoking(Schedule schedule)
    {
        return schedule != null && !schedule.Canceled;
    }

    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="schedule"></param>
    public void CancelInvoke(Schedule schedule)
    {
        if (schedule == null)
            return;
        schedule.Cancel();
        RemoveSchedule(schedule);
        schedule = null;
    }

    // 双缓冲，防止计时器回调时更改计时器队列
    List<Schedule> mFront = new List<Schedule>();
    List<Schedule> mBack = new List<Schedule>();
    List<Schedule> mGarbrage = new List<Schedule>();
}
