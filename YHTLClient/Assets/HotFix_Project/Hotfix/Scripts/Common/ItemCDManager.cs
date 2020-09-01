using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CDItem
{
    public Dictionary<int, CDItem> CDHandle;
    public int key;
    public float startTime;
    public float cdTime;
    public float endTime;
    public bool reached
    {
        get
        {
            return Time.realtimeSinceStartup >= endTime;
        }
    }
    public float fillAmount
    {
        get
        {
            return (Time.realtimeSinceStartup - startTime) / cdTime;
        }
    }
    public void Reset()
    {
        CDHandle?.Remove(key);
        CDHandle = null;
    }
}
public class ItemCDManager : CSInfo<ItemCDManager>
{
    PoolHandleManager mPoolHandle = new PoolHandleManager();
    FastArrayElementFromPool<CDItem> mTimers;
    Dictionary<int, CDItem> mCDGroup = new Dictionary<int, CDItem>(32);
    Dictionary<int, System.Action<float>> mCDActions = new Dictionary<int, System.Action<float>>(32);
    Dictionary<int, System.Action> mActionStop = new Dictionary<int, System.Action>(32);

    public ItemCDManager()
    {
        mTimers = mPoolHandle.CreateGeneratePool<CDItem>(16);
    }

    public void ResetItemCD(RepeatedField<bag.ItemCd> itemCds)
    {
        for (int i = 0; i < itemCds.Count; ++i)
        {
            EnterGroupCD(itemCds[i].groupId, itemCds[i].useTime);
        }
    }

    public override void Dispose()
    {
        mActionStop.Clear();
        mActionStop = null;
        mCDActions.Clear();
        mCDActions = null;
        mCDGroup.Clear();
        mCDGroup = null;
        for (int i = 0, max = mTimers.Count; i < max; ++i)
        {
            mTimers[i].Reset();
        }
        mTimers.Clear();
        mTimers = null;
    }

    public void EnterGroupCD(int groupId, long startTime)
    {
        //Debug.LogFormat("[ServerNowMs]:{0} [UsedTime]:{1} [PassedTime]:{2}", CSServerTime.ServerNowMsChecked, startTime, CSServerTime.ServerNowMsChecked - startTime);

        if (mCDGroup.ContainsKey(groupId))
        {
            //Debug.LogFormat("[物品使用CD]:已经存在");
            return;
        }

        float cd = ItemTableManager.Instance.GetItemCDTimeByGroupId(groupId) * 0.001f;
        float passedTime = (CSServerTime.ServerNowMsChecked - startTime) * 0.001f;
        if (passedTime >= cd)
            return;
        //Debug.LogFormat("[leavedTime]:{0} seconds cd = {1} seconds", passedTime, cd);
        var cdItem = mTimers.Append();
        cdItem.key = groupId;
        cdItem.cdTime = cd;
        cdItem.startTime = Time.realtimeSinceStartup - passedTime;
        cdItem.endTime = cdItem.startTime + cd;
        cdItem.CDHandle = mCDGroup;
        mCDGroup.Add(groupId, cdItem);
    }

    public bool InGroupCD(int group)
    {
        return mCDGroup.ContainsKey(group);
    }

    public void AddAction(int groupId, System.Action<float> action, System.Action actionStop)
    {
        if (groupId <= 0)
            return;

        if(null != action)
        {
            if (mCDActions.ContainsKey(groupId))
            {
                if (null == mCDActions[groupId])
                    mCDActions[groupId] = action;
                else
                    mCDActions[groupId] = System.Delegate.Combine(mCDActions[groupId], action) as System.Action<float>;
            }
            else
            {
                mCDActions.Add(groupId, action);
            }
        }

        if(null != mActionStop)
        {
            if (mActionStop.ContainsKey(groupId))
            {
                if (null == mActionStop[groupId])
                    mActionStop[groupId] = actionStop;
                else
                    mActionStop[groupId] = System.Delegate.Combine(mActionStop[groupId], actionStop) as System.Action;
            }
            else
            {
                mActionStop.Add(groupId, actionStop);
            }
        }
    }

    public void RemoveAction(int groupId, System.Action<float> action, System.Action actionStop)
    {
        if (groupId <= 0)
            return;
        if (mCDActions.ContainsKey(groupId))
        {
            mCDActions[groupId] = System.Delegate.Remove(mCDActions[groupId], action) as System.Action<float>;
        }
        if (mActionStop.ContainsKey(groupId))
        {
            mActionStop[groupId] = System.Delegate.Remove(mActionStop[groupId], actionStop) as System.Action;
        }
    }

    public void Update()
    {
        for (int i = mTimers.Count - 1; i >= 0; --i)
        {
            var timer = mTimers[i];
            if (timer.reached)
            {
                if (mActionStop.ContainsKey(timer.key))
                {
                    mActionStop[timer.key]?.Invoke();
                }

                timer.Reset();
                mTimers.RemoveAt(i);
            }
            else
            {
                if (mCDActions != null && mCDActions.ContainsKey(timer.key))
                {
                    mCDActions[timer.key]?.Invoke(timer.fillAmount);
                }
            }
        }
    }
}