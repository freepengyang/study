using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSAvatarBuffEffect
{
    /// <summary>
    /// <effectId, CSBuffEffect>
    /// </summary>
    public Dictionary<int, CSBuffEffect> buffEffectDic = new Dictionary<int, CSBuffEffect>(32);

    public void Add(Transform anchor,int effectId)
    {
        CSBuffEffect effect = null;
        if (!buffEffectDic.TryGetValue(effectId, out effect))
        {
            CSObjectPoolItem poolItem = null;
            effect = CSSceneEffectMgr.AddPoolItem<CSBuffEffect>(ref poolItem);
            if (effect != null && poolItem != null)
            {
                effect.PoolItem = poolItem;
                effect.Play(anchor, effectId);
                buffEffectDic.Add(effectId, effect);
            }
        }
        else
        {
            effect.SetAvtive(true);
            effect.Replay();
        }
    }

    public void Remove(int effectId)
    {
        CSBuffEffect effect = null;
        if (buffEffectDic.TryGetValue(effectId, out effect))
        {
            if (effect.IsDestroy())
            {
                buffEffectDic.Remove(effectId);
            }
            effect.SetAvtive(false);
        }
    }

    public void Destroy()
    {
        var dic = buffEffectDic.GetEnumerator();
        while(dic.MoveNext())
        {
            CSBuffEffect effect = dic.Current.Value;
            if(effect != null)
            {
                effect.Destroy();
            }
        }
        buffEffectDic.Clear();
    }

}
