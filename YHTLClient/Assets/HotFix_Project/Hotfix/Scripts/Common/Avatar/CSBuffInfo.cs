using fight;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSBuffInfo
{
    //private System.Action<BufferInfo> onUpdateBuff;
    public ILBetterList<int> buffIdList = new ILBetterList<int>();
    private Dictionary<int, BufferInfo> mBuffInfoDic = new Dictionary<int, BufferInfo>();
    public static CSAvatarState avatarState = new CSAvatarState();

    public void Init(RepeatedField<BufferInfo> list)
    {
        if(buffIdList == null)
        {
            buffIdList = new ILBetterList<int>();
        }
        buffIdList.Clear();
        mBuffInfoDic.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            BufferInfo info = list[i];
            UpdateBuff(info);
        }
        //onUpdateBuff?.Invoke(null);
    }

    public void RegUpdateBuff(System.Action<BufferInfo> callBack)
    {
        UnRegUpdateBuff(callBack);
        //onUpdateBuff += callBack;
    }

    public void UnRegUpdateBuff(System.Action<BufferInfo> callBack)
    {
        //onUpdateBuff -= callBack;
    }

    public void RemoveBuff(int buffId)
    {
        if(mBuffInfoDic.ContainsKey(buffId))
        {
            mBuffInfoDic.Remove(buffId);
            buffIdList.Remove(buffId);
        }
    }

    public void UpdateBuff(BufferInfo info)
    {
        if (mBuffInfoDic.ContainsKey(info.bufferId))
        {
            mBuffInfoDic[info.bufferId] = info;
        }
        else
        {
            buffIdList.Add(info.bufferId);
            mBuffInfoDic.Add(info.bufferId, info);
        }
        //onUpdateBuff?.Invoke(info);
    }

    public BufferInfo GetBuff(int buffId)
    {
        if(mBuffInfoDic.ContainsKey(buffId))
        {
            return mBuffInfoDic[buffId];
        }
        return null;
    }

    public BufferInfo GetBuffByType(int type)
    {
        BufferInfo buffInfo = null;
        int buffType = type;
        for (int i = 0; i < buffIdList.Count; ++i)
        {
            TABLE.BUFFER tblBuff = null;
            if (BufferTableManager.Instance.TryGetValue(buffIdList[i], out tblBuff))
            {
                if (tblBuff.type == buffType)
                {
                    buffInfo = GetBuff(buffIdList[i]);
                    return buffInfo;
                }
            }
        }
        return buffInfo;
    }

    public bool IsHasBuff(int buffId)
    {
        return (GetBuff(buffId) != null);
    }

    public bool IsHasSpecialBuff(int buffId)
    {
        return (GetBuff(buffId) != null);
    }

    public bool IsHasBuffByType(int type)
    {
        BufferInfo info = GetBuffByType(type);
        return (info != null);
    }

    public void UpdateState(CSAvatarState actState)
    {
        avatarState.Copy(actState);
        actState.Reset();
        for (int i = 0; i < buffIdList.Count; ++i)
        {
            TABLE.BUFFER tblBuff = null;
            if (BufferTableManager.Instance.TryGetValue(buffIdList[i], out tblBuff))
            {
                switch ((tblBuff.type))
                {
                    case EBuffType.Vertigo:
                        {
                            actState.IsVertigo = true;
                        }
                        break;
                    case EBuffType.Hiding:
                        {
                            actState.IsHiding = true;
                        }
                        break;
                    case EBuffType.Hold:
                        {
                            actState.IsHold = true;
                        }
                        break;
                }
                if (tblBuff.effectId > 0 && tblBuff.effectId <= CSBuffManager.EFFECT_COLOR_MAX)
                {
                    if (actState.ColorType < tblBuff.effectId)
                    {
                        actState.ColorType = tblBuff.effectId;
                    }
                }
            }
        }
    }


    public void Dispose()
    {
        if(buffIdList != null)
        {
            buffIdList.Clear();
        }
        if(mBuffInfoDic != null)
        {
            mBuffInfoDic.Clear();
        }
        //onUpdateBuff = null;
    }

}
