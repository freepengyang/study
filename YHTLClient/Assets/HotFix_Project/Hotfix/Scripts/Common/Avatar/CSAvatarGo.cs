using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSAvatarGo : MonoBehaviour 
{
    public CSAvatar Owner;
    public virtual void Init(CSAvatar avatar)
    {
        Owner = avatar;
        //Owner.IsShaderChange = true;
        Owner.Go.SetActive(true);
        if (Platform.IsEditor)
        {
            Owner.Go.name = avatar.GetName();
        }
        InitBuffState();
        //Owner.UpdateShowInMiWu(Owner);
    }

    public void InitBuffState()
    {
        if (Owner.BaseInfo == null)
        {
            return;
        }
        CSBuffInfo buffInfo = Owner.BaseInfo.BuffInfo;
        if (buffInfo == null || buffInfo.buffIdList == null)
        {
            return;
        }
        for (int i = 0; i < buffInfo.buffIdList.Count; ++i)
        {
            CSBuffManager.Instance.AddBuff(Owner, Owner.ID,buffInfo.buffIdList[i]);
        }
    }

    public virtual void OnHit(CSAvatar clicker)
    {
    }

    public virtual void Destroy()
    {

    }
}
