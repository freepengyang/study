using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSceneParticleEffect : EffectBase
{
    private ParticleSystem[] particleSystems;
    private TABLE.EFFECT mTblEffect;

    public void Play(Transform parent, int effectId, int x = 0, int y = 0, System.Action onLoadCallBack = null, int resAssistType = ResourceAssistType.Player)
    {
        InitLocalPosition(x, y);
        Init(parent,effectId, onLoadCallBack);
    }

    protected void Init(Transform parent, int effectId,System.Action onLoadCallBack = null, int resAssistType = ResourceAssistType.Player)
    {
        if (EffectTableManager.Instance.TryGetValue(effectId, out mTblEffect))
        {
            IsActive = true;
            mParent = parent;
            if (mLocalPosition == Vector3.zero)
            {
                if (mTblEffect.offsetX != 0 || mTblEffect.offsetY != 0 || mTblEffect.offsetZ != 0)
                {
                    mLocalPosition = new Vector3(mTblEffect.offsetX, mTblEffect.offsetY, mTblEffect.offsetZ);
                }
            }
            Init(parent, mTblEffect.name, mLocalPosition, mTblEffect.resType, mTblEffect.destroyTime, mTblEffect.resType, onLoadCallBack, resAssistType);

        }
    }

    protected void InitLocalPosition(int coordX, int coordY)
    {
        if (coordX != 0 && coordY != 0)
        {
            CSCell cell = CSMesh.Instance.getCell(coordX, coordY);
            if (cell != null)
            {
                mLocalPosition = cell.LocalPosition2;
            }
        }
    }


    public void Play()
    {
        if(particleSystems != null)
        {
            ParticleSystem tempParticleSystem = null;
            for (int i = 0; i < particleSystems.Length; ++i)
            {
                tempParticleSystem = particleSystems[i];
                if(tempParticleSystem != null)
                {
                    tempParticleSystem.Play(true);
                }
            }
        }
    }

    public void Stop()
    {
        if (particleSystems != null)
        {
            ParticleSystem tempParticleSystem = null;
            for (int i = 0; i < particleSystems.Length; ++i)
            {
                tempParticleSystem = particleSystems[i];
                if (tempParticleSystem != null)
                {
                    tempParticleSystem.Stop(true);
                }
            }
        }
    }

    public override void OnLoaded(CSResource res)
    {
        if (res == null || res.MirrorObj == null)
        {
            return;
        }

        if (this == null && mGoRoot != null)
        {
            Destroy();
            return;
        }
        mGoRoot = res.GetObjInst() as GameObject;
        if(mGoRoot == null)
        {
            return;
        }

        Transform trans = mGoRoot.transform;
        trans.parent = mParent;
        trans.localScale = Vector3.one;
        trans.localPosition = mLocalPosition;
        if (mPoolItem != null)
        {
            mPoolItem.go = mGoRoot;
        }
        if (!IsLoop() && mDestroyTime > 0)
        {
            float destroyTime = mDestroyTime / 1000f;
            mSchedule = Timer.Instance.Invoke(destroyTime, DestroyEffect);
        }
        particleSystems = mGoRoot.GetComponentsInChildren<ParticleSystem>();

        Renderer[] renderers = mGoRoot.GetComponentsInChildren<Renderer>();
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.shader = Shader.Find(renderers[i].material.shader.name);
            }
        }
    }

    public override void Destroy()
    {
        particleSystems = null;
        base.Destroy();
    }
}
