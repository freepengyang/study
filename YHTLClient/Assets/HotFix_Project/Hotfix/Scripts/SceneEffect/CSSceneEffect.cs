using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSceneEffect : CSSceneFrameEffect
{
    public override void Play(Transform parent, int effectId, int x = 0, int y = 0,bool isOffset = false, System.Action onLoadcallBack = null, int resAssistType = ResourceAssistType.Player)
    {
        base.Play(parent,effectId, x, y, isOffset, onLoadcallBack,resAssistType);
    }

    public virtual void UpdatePosition(int coordX, int coordY)
    {
        InitLocalPosition(coordX, coordY);
        ResetPosition();
    }

    protected void ResetPosition()
    {
        if (mGoRoot != null)
        {
            Transform trans = mGoRoot.transform;
            trans.localPosition = mLocalPosition;
        }
    }
}
