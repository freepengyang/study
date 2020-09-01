using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSAvatarEffectManager : CSInfo<CSAvatarEffectManager>
{
    private Dictionary<long, CSSceneEffect> mEffectDic = new Dictionary<long, CSSceneEffect>();

    public void Show(Transform anchor, long id,int effectConfigId)
    {
        if (effectConfigId > 0)
        {
            CSSceneEffect effect = null;
            if(mEffectDic.TryGetValue(id, out effect))
            {
                if (effect != null)
                {
                    effect.Play(anchor, effectConfigId);
                }
                return;
            }
            AddEffect(anchor, id, effectConfigId);
        }
    }

    private void AddEffect(Transform anchor, long id, int effectConfigId)
    {
        CSObjectPoolItem poolItem = null;
        CSSceneEffect effect = CSSceneEffectMgr.AddPoolItem<CSSceneEffect>(ref poolItem);
        if (effect != null && poolItem != null)
        {
            effect.PoolItem = poolItem;
            effect.Play(anchor, effectConfigId);
            mEffectDic.Add(id, effect);
        }
    }

    public void Remove(long id)
    {
        if (mEffectDic.ContainsKey(id))
        {
            CSSceneEffect effect = mEffectDic[id];
            if (effect != null)
            {
                effect.Release();
            }
            mEffectDic.Remove(id);
            return;
        }
    }

    public override void Dispose()
    {
        var dic = mEffectDic.GetEnumerator();
        while (dic.MoveNext())
        {
            CSSceneEffect effect = dic.Current.Value;
            if (effect != null)
            {
                effect.Destroy();
            }
        }
        mEffectDic.Clear();
    }
}
