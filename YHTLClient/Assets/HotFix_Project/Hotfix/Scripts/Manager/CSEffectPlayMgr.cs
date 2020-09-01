using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSEffectPlayMgr : Singleton<CSEffectPlayMgr>
{
    readonly PoolHandleManager _manager = new PoolHandleManager();

    private readonly Dictionary<int, UIEffectPlayBase> _uiEffectPlays = new Dictionary<int, UIEffectPlayBase>();

    public void ShowEffectPlay(GameObject go, int effectId, int _speed = 10, bool delete = true)
    {
        if (go == null) return;

        TABLE.EFFECT effect;
        if (EffectTableManager.Instance.TryGetValue(effectId, out effect))
        {
            //UIParticle
            if (effect.effecttype == 1)
            {
                ShowParticleEffect(go, effect.name);
            }
            //UIEffect
            else if (effect.resType == 17)
            {
                ShowUIEffect(go, effect.name, _speed, delete);
            }
            //UITexture
            else if (effect.resType == 23)
            {
                ShowUITexture(go, effect.name, delete);
            }

        }
    }

    /// <summary>
    /// 根据 Effect 表显示特效(是否循环走配表)
    /// </summary>
    public void ShowUIEffect(GameObject go, int effectId, int _speed = 10, bool delete = true)
    {
        if (go == null) return;

        TABLE.EFFECT effect;
        if (EffectTableManager.Instance.TryGetValue(effectId, out effect))
        {
            ShowUIEffect(go, effect.name, _speed, effect.destroyTime == 0, delete);
        }
    }

    public void ShowUIEffect(GameObject go, string effectId, int type)
    {
        ShowUIEffect(go, effectId, type,10,true,true,null);
    }

    /// <summary>
    /// 根据 字符串 显示特效
    /// </summary>
    public void ShowUIEffect(GameObject go, string effecName, int _speed = 10, bool _loop = true, bool delete = true)
    {
        if (go == null) return;
        UIEffectPlay effectPlay = GetEffectPlay<UIEffectPlay>(go.GetHashCode());
        effectPlay.ShowUIEffect(go, effecName, _speed, _loop, ResourceType.UIEffect, delete);
    }

    public void ShowUIEffect2(GameObject go, string effecName,int _loadType, int _speed = 10, bool _loop = true, bool delete = true)
    {
        if (go == null) return;
        UIEffectPlay effectPlay = GetEffectPlay<UIEffectPlay>(go.GetHashCode());
        effectPlay.ShowUIEffect(go, effecName, _speed, _loop, ResourceType.UIEffect, delete, null, _loadType);
    }

    public void ShowUIEffect(GameObject go, string effecName, int type, int _speed = 10, bool _loop = true,
        bool delete = true, System.Action action = null)
    {
        if (go == null) return;
        UIEffectPlay effectPlay = GetEffectPlay<UIEffectPlay>(go.GetHashCode());
        effectPlay.ShowUIEffect(go, effecName, _speed, _loop, type, delete, action);
    }

    public void ShowUITexture(GameObject go, string effecName, bool isPerfect = true,
        System.Action action = null)
    {
        if (go == null) return;
        UITexturePlay effectPlay = GetEffectPlay<UITexturePlay>(go.GetHashCode());
        effectPlay.ShowTexture(go, effecName, ResourceType.UITexture, isPerfect, action);
    }

    public void ShowUITexture(GameObject go, string effecName, int type, bool isPerfect = true,
        System.Action action = null, bool isCanBeDelete = true)
    {
        if (go == null) return;
        UITexturePlay effectPlay = GetEffectPlay<UITexturePlay>(go.GetHashCode());
        effectPlay.ShowTexture(go, effecName, type, isPerfect, action, isCanBeDelete);
    }

    /// <summary>
    /// 显示粒子特效
    /// </summary>
    public void ShowParticleEffect(GameObject go, int effectId, int mParticlePlayTime = 0, bool delete = true,
        int objScale = 1, bool IsDestroyParent = false, Vector3? position = null, int sortingStage = 300,
        bool isrepeat = false, System.Action action = null)
    {
        if (go == null) return;

        TABLE.EFFECT effect;
        if (EffectTableManager.Instance.TryGetValue(effectId, out effect))
        {
            ShowParticleEffect(go, effect.name, mParticlePlayTime, delete, objScale, IsDestroyParent, position,
                sortingStage, isrepeat, action);
        }
    }

    public void ShowParticleEffect(GameObject go, string effecName, int mParticlePlayTime = 0, bool delete = true,
        int objScale = 1, bool IsDestroyParent = false, Vector3? position = null, int sortingStage = 300,
        bool isrepeat = false, System.Action action = null)
    {
        if (go == null) return;

        UIParticlePlay effectPlay = GetEffectPlay<UIParticlePlay>(go.GetHashCode());
        effectPlay.ShowParticleEffect(go, effecName, mParticlePlayTime, ResourceType.Effect, delete, objScale,
            IsDestroyParent, position, sortingStage, isrepeat, action);
    }


    public void Recycle(GameObject go)
    {
        if (go == null) return;
        UIEffectPlayBase effectPlay;
        var hashValue = go.GetHashCode();
        if (_uiEffectPlays.ContainsKey(hashValue))
        {
            effectPlay = _uiEffectPlays[hashValue];
            _uiEffectPlays.Remove(hashValue);
            _manager.Recycle(effectPlay);
        }
    }

    private T GetEffectPlay<T>(int id) where T : UIEffectPlayBase, new()
    {
        UIEffectPlayBase effectPlay;
        if (!_uiEffectPlays.ContainsKey(id))
        {
            effectPlay = _manager.GetCustomClass<T>();
            _uiEffectPlays.Add(id, effectPlay);
        }
        else
        {
            effectPlay = _uiEffectPlays[id];
        }

        return effectPlay as T;
    }

    public bool IsContains(int id)
    {
        return _uiEffectPlays.ContainsKey(id);
    }
}