using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSceneFrameEffect : EffectBase
{
    protected TABLE.EFFECT mTblEffect = null;
    public virtual void Play(Transform parent, int effectId, int coordX = 0, int coordY = 0, bool isOffset = false, System.Action onLoadcallBack = null, int resAssistType = ResourceAssistType.Player)
    {
        InitLocalPosition(coordX, coordY);
        Init(parent, effectId, isOffset,onLoadcallBack, resAssistType);
    }

    public virtual void Play(Transform parent, string resName,Vector3 localPosition,int playType,float destroyTime = 0 ,
        int resType = ResourceType.Effect, System.Action onLoadcallBack = null, int resAssistType = ResourceAssistType.Player)
    {
        Init(parent, resName, localPosition, playType, destroyTime, resType, onLoadcallBack,resAssistType);
    }

    public void Play(Transform parent, int effectId, Vector3 localPosition, bool isOffset = false, System.Action onLoadcallBack = null, int resAssistType = ResourceAssistType.Player)
    {
        mLocalPosition = localPosition;
        Init(parent,effectId, isOffset,onLoadcallBack, resAssistType);
    }

    protected void Init(Transform parent, int effectId,bool isOffset = false, System.Action onLoadCallBack = null, int resAssistType = ResourceAssistType.Player)
    {
        if (EffectTableManager.Instance.TryGetValue(effectId, out mTblEffect))
        {
            IsActive = true;
            mParent = parent;
            if (mLocalPosition == Vector3.zero)
            {
                if(mTblEffect.offsetX != 0 || mTblEffect.offsetY != 0 || mTblEffect.offsetZ != 0)
                {
                    mLocalPosition = new Vector3(mTblEffect.offsetX, mTblEffect.offsetY, mTblEffect.offsetZ);
                }
            }
            else if(isOffset)
            {
                mLocalPosition.x = mLocalPosition.x + mTblEffect.offsetX;
                mLocalPosition.y = mLocalPosition.y + mTblEffect.offsetY;
            }
            Init(parent,mTblEffect.name,mLocalPosition, mTblEffect.playType,mTblEffect.destroyTime,mTblEffect.resType, onLoadCallBack, resAssistType);
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
}
