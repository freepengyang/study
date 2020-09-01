using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSActorHurtEffect: Singleton2<CSActorHurtEffect> 
{
    public static CSBetterList<UIActorHurtEffect> hurtCloneList = new CSBetterList<UIActorHurtEffect>();

    private UIActorHurtEffect GetUnusedHurtObj()
    {
        if (hurtCloneList == null)
        {
            return null;
        }
        for (int i = 0, iMax = hurtCloneList.Count; i < iMax; i++)
        {
            if ((hurtCloneList[i] != null) && (!hurtCloneList[i].IsAvtive()))
            {
                return hurtCloneList[i];
            }
        }
        return null;
    }

    public void PlayHurtEffect(GameObject obj_hurt, GameObject obj_hurtParent, int deltaHp, int flyDirection, float delayTime,int type = ThrowTextType.None)
    {
        UIActorHurtEffect hurtEffect = GetUnusedHurtObj();
        if (hurtEffect == null)
        {
            if (obj_hurt != null)
            {
                GameObject go = GameObject.Instantiate(obj_hurt) as GameObject;
                hurtEffect = go.AddComponent<UIActorHurtEffect>();
                hurtCloneList.Add(hurtEffect);
            }
        }

        if (hurtEffect != null && hurtEffect.hurtLb != null && obj_hurtParent != null)
        {
            //if (!hurtEffect.gameObject.activeSelf)
            //{
            //    hurtEffect.gameObject.SetActive(true);
            //}
            hurtEffect.hurtLb.Atlas = CSGameManager.Instance.GetStaticAtlas("fightnums");
            hurtEffect.hurtLb.TextType = type;
            NGUITools.SetParent(obj_hurtParent.transform, hurtEffect.gameObject);
            hurtEffect.hurtLb.mDamage = deltaHp;
            hurtEffect.Init(flyDirection, delayTime,deltaHp);
        }
    }

}
