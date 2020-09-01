using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSceneEffectMgr : Singleton<CSSceneEffectMgr>
{
    private const float createEffectInterval = 0.1f;
    private Dictionary<long, CSSceneEffect> mSceneEffectDic = new Dictionary<long, CSSceneEffect>(256);
    private Dictionary<long, CSSceneParticleEffect> mParticleEffectDic = new Dictionary<long, CSSceneParticleEffect>(8);

    
    public CSSceneParticleEffect PlayParticleEffect(Transform anchor, int effectConfigId)
    {
        TABLE.EFFECT tblEffect = null;
        if (EffectTableManager.Instance.TryGetValue(effectConfigId, out tblEffect))
        {
            CSObjectPoolItem poolItem = null;
            CSSceneParticleEffect effect = AddPoolItem<CSSceneParticleEffect>(ref poolItem);
            if (effect != null)
            {
                effect.PoolItem = poolItem;
                effect.Play(anchor, effectConfigId);
                if (tblEffect.playType == 0)
                {
                    effect.SetDestroyCallBack(OnDestroyCallBack);
                }
                int hashCode = effect.GetHashCode();
                if (!mParticleEffectDic.ContainsKey(hashCode))
                {
                    mParticleEffectDic.Add(hashCode, effect);
                }
            }
            return effect;
        }
        return null;
    }


    /// <summary>
    ///一般用于播一次就销毁的特效
    /// </summary>
    /// <param name="anchor"></param>
    /// <param name="effectConfigId"></param>
    public void PlayEffect(Transform anchor, int effectConfigId)
    {
        TABLE.EFFECT tblEffect = null;
        if(EffectTableManager.Instance.TryGetValue(effectConfigId, out tblEffect))
        {
            CSObjectPoolItem poolItem = null;
            CSSceneEffect effect = AddPoolItem<CSSceneEffect>(ref poolItem);
            if (effect != null)
            {
                effect.PoolItem = poolItem;
                effect.Play(anchor, effectConfigId);
                if(tblEffect.playType == 0)
                {
                    effect.SetDestroyCallBack(OnDestroyCallBack);
                }
                int hashCode = effect.GetHashCode();
                if (!mSceneEffectDic.ContainsKey(hashCode))
                {
                    mSceneEffectDic.Add(hashCode, effect);
                }
            }
        }
    }

    private void OnDestroyCallBack(int key)
    {
        if (mSceneEffectDic.ContainsKey(key))
        {
            mSceneEffectDic.Remove(key);
        }
    }

    /// <summary>
    /// 等待创建场景特效
    /// </summary>
    /// <param name="anchor">特效锚点</param>
    /// <param name="id">唯一id</param>
    /// <param name="info">数据信息</param>
    /// <param name="onLoad">创建完成回调</param>
    /// <param name="param">额外参数</param>
    public void PlayEffectWaitDeal(Transform anchor, long id, object info, Func<object, object, bool> onLoad, object param = null)
    {
        if(anchor == null)
        {
            return;
        }
        if (mSceneEffectDic.ContainsKey(id))
        {
            onLoad?.Invoke(info, param);
            return;
        }
        CSObjectPoolItem poolItem = null;
        CSSceneEffect effect = AddPoolItem<CSSceneEffect>(ref poolItem);
        if (effect != null && poolItem != null)
        {
            effect.PoolItem = poolItem;
            mSceneEffectDic.Add(id, effect);
            AddEffectWaitDeal(anchor, info, onLoad, createEffectInterval, param);
        }
    }

    public void AddEffectWaitDeal(Transform anchor, object info, Func<object, object, bool> onLoad, float waitFrame, object param = null)
    {
        GameObject go = anchor.gameObject;
        var waitDealDic = CSWaitFrameDealManager.Instance.AnchorToWaitDealDic;
        if (!waitDealDic.ContainsKey(go))
        {
            CSWaitFrameDeal deal = go.AddComponent<CSWaitFrameDeal>();
            waitDealDic.Add(go, deal);
            deal.Add(info, onLoad, waitFrame, param);
        }
        else
        {
            waitDealDic[go].Add(info, onLoad, waitFrame, param);
        }
    }

    /// <summary>
    ///播放有唯一id的特效，一般销毁对应Remove(id)
    /// </summary>
    /// <param name="anchor">特效锚点</param>
    /// <param name="id">特效唯一id</param>
    /// <param name="effectConfigId">Effect表id</param>
    /// <param name="x">x格子坐标</param>
    /// <param name="y">y格子坐标</param>
    ///  <param name="isOffset">effect坐标偏移是否有效</param>
    public void PlayEffect(Transform anchor, long id, int effectConfigId, int x, int y, bool isOffset = false)
    {
        if (mSceneEffectDic.ContainsKey(id))
        {
            mSceneEffectDic[id].Play(anchor,effectConfigId, x, y, isOffset);
        }
        else
        {
            CSObjectPoolItem poolItem = null;
            CSSceneEffect effect = AddPoolItem<CSSceneEffect>(ref poolItem);
            if (effect != null)
            {
                effect.PoolItem = poolItem;
                effect.Play(anchor, effectConfigId, x, y, isOffset);
                mSceneEffectDic.Add(id, effect);
            }
        }
    }

    /// <summary>
    ///  用于循环播放特效或者播完不销毁的特效，需要自己销毁，隐藏实体调用Release(...),销毁实体调用Destroy(...)
    ///  再次播放原来的特效可调用 CSSceneEffect.cs中的 Replay();
    /// </summary>
    /// <param name="anchor"></param>
    /// <param name="effectConfigId"></param>
    /// <param name="localPosition"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public CSSceneEffect Create(Transform anchor, int effectConfigId, Vector3 localPosition,System.Action action = null)
    {
        CSObjectPoolItem poolItem = null;
        CSSceneEffect effect = AddPoolItem<CSSceneEffect>(ref poolItem);
        if (effect != null && poolItem != null)
        {
            effect.PoolItem = poolItem;
            effect.Play(anchor, effectConfigId, localPosition, false,action);
            int hashCode = effect.GetHashCode();
            if (!mSceneEffectDic.ContainsKey(hashCode))
            {
                mSceneEffectDic.Add(hashCode, effect);
            }
        }
        return effect;
    }

    public static T AddPoolItem<T>(ref CSObjectPoolItem poolItem) where T : EffectBase
    {
        if (CSObjectPoolMgr.Instance == null)
        {
            return null;
        }
        Type type = typeof(T);
        poolItem = Utility.GetAndAddPoolItem_Class(type.ToString(), type.ToString(), null, type, null);
        return poolItem.objParam as T;
    }

    public CSSceneEffect GetEffect(long id)
    {
        if (mSceneEffectDic.ContainsKey(id))
        {
            return mSceneEffectDic[id];
        }
        return null;
    }

    public bool Remove(long id)
    {
        if (mSceneEffectDic.ContainsKey(id))
        {
            CSSceneEffect effect = mSceneEffectDic[id];
            if (effect != null)
            {
                effect.Release();
            }
            mSceneEffectDic.Remove(id);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 释放特效资源：回收到缓存池，隐藏实体
    /// </summary>
    /// <param name="effect"></param>
    public void Release(ref CSSceneEffect effect)
    {
        if (effect != null)
        {
            int hashCode = effect.GetHashCode();
            if (mSceneEffectDic.ContainsKey(hashCode))
            {
                mSceneEffectDic.Remove(hashCode);
            }
            effect.Release();
            effect = null;
        }
    }

    /// <summary>
    /// 释放特效资源：回收到缓存池，销毁实体
    /// </summary>
    /// <param name="effect"></param>
    public void Destroy(CSSceneEffect effect)
    {
        if (effect != null)
        {
            int hashCode = effect.GetHashCode();
            if (mSceneEffectDic.ContainsKey(hashCode))
            {
                mSceneEffectDic.Remove(hashCode);
            }
            effect.Destroy();
        }
    }

    public void Destroy(CSSceneParticleEffect effect)
    {
        if (effect != null)
        {
            int hashCode = effect.GetHashCode();
            if (mParticleEffectDic.ContainsKey(hashCode))
            {
                mParticleEffectDic.Remove(hashCode);
            }
            effect.Destroy();
        }
    }

    public void Destroy()
    {
        DestroyFrameEffect();
        DestroyParticleEffect();
    }

    private void DestroyFrameEffect()
    {
        var dic = mSceneEffectDic.GetEnumerator();
        while (dic.MoveNext())
        {
            CSSceneEffect effect = dic.Current.Value;
            if (effect != null)
            {
                effect.Destroy();
            }
        }
        mSceneEffectDic.Clear();
    }

    private void DestroyParticleEffect()
    {
        var dic = mParticleEffectDic.GetEnumerator();
        while(dic.MoveNext())
        {
            CSSceneParticleEffect effect = dic.Current.Value;
            if(effect != null)
            {
                effect.Destroy();
            }
        }
        mParticleEffectDic.Clear();
    }
}
