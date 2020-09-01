using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSHead : MonoBehaviour
{
    private GameObject _obj_hp;
    private GameObject obj_hp { get { return _obj_hp ?? (_obj_hp = UtilityObj.GetObject<GameObject>(transform, "root/obj_hp")); } }

    protected CSLabel lb_actorName = null;
    protected CSFillSprite spr_hp = null;
    protected GameObject obj_hurt = null;
    protected GameObject obj_hurtParent = null;
    protected CSAvatar mAvatar;
    protected float mHpFillAmount = 0f;
    public bool isHideModel;
    public bool isHideAllName;

    private static float mTempFillAmount = 0;
    private static Vector3 _vector3 = new Vector3(0, 102, -100000);
    public static ILBetterList<UIActorHurtEffect> hurtCloneList = new ILBetterList<UIActorHurtEffect>(32);

    public void InitCompoment()
    {
        if(spr_hp == null)
        {
            spr_hp = UtilityObj.GetOrAdd<CSFillSprite>(transform, "root/obj_hp/spr_hp");
        }
        if(obj_hurtParent == null)
        {
            obj_hurtParent = UtilityObj.GetObject<GameObject>(transform, "root/obj_hurtParent");
        }
        if(obj_hurt == null)
        {
            obj_hurt = UtilityObj.GetObject<GameObject>(transform, "root/obj_hurt");
        }
        if(lb_actorName == null)
        {
            lb_actorName = UtilityObj.GetOrAdd<CSLabel>(transform, "root/lb_name");
        }
    }

    public virtual void Init(CSAvatar avatar, bool isVisible = true,bool isHideModel = false, bool isHideAllName = false)
    {
        mAvatar = avatar;
        transform.parent = avatar.ModelModule.Top.transform;
        transform.localScale = Vector3.one;
        transform.localPosition = _vector3;
        InitCompoment();
        this.isHideModel = isHideModel;
        this.isHideAllName = isHideAllName;
        if (lb_actorName != null) lb_actorName.fontSize = 16;
        isVisible = (isHideModel) ? true : (isVisible && mAvatar.BaseInfo.HP > 0);
        SetActive(isVisible);
        SetHeadActive(!isHideAllName);
        SetActorHpActive(true);
        SetNameColor();
        SetName();
        SetHpFillAmount();
        InitEvent();
        SetHeadPosition();
    }

    protected virtual void SetName()
    {
        if(null != lb_actorName)
            lb_actorName.text = mAvatar.GetName();
    }

    public virtual void InitEvent()
    {
        mAvatar.BaseInfo.EventHandler.AddEvent(CEvent.HP_Change, OnHeadHpChange);
    }

    public List<CSLabel> list_lbs;
    
    /// <summary>
    /// 将cslabel加入列表,并设置size
    /// </summary>
    /// <param name="Lb"></param>
    /// <param name="size"></param>
    protected void SetListV(CSLabel Lb)
    {
        if (null != Lb)
        {
            //Lb.fontSize = size;
            if (list_lbs != null && !list_lbs.Contains(Lb))
                list_lbs.Add(Lb);
        }
    }

    /// <summary>
    /// 设置对象的名称是否显示隐藏 
    /// </summary>
    /// <param name="active"></param>
    public virtual void SetHeadActive(bool active)
    {
        SetActorNameActive(active);
    }

    public virtual void Update()
    {
        if(spr_hp != null)
        {
            mTempFillAmount = spr_hp.fillAmount;
            if (spr_hp != null && mHpFillAmount != mTempFillAmount)
            {
                mTempFillAmount = Mathf.Lerp(mTempFillAmount, mHpFillAmount, Time.deltaTime * 10);
                if (Mathf.Abs(mHpFillAmount - mTempFillAmount) < 0.01f)
                {
                    mTempFillAmount = mHpFillAmount;
                }
                spr_hp.fillAmount = mTempFillAmount;
                if (mTempFillAmount <= 0.0001f && (mAvatar != null) && (mAvatar.IsServerDead))
                {
                    SetHeadActive(false);
                    SetActorHpActive(false);
                }
            }
        }
    }

    public virtual void SetNameColor()
    {
    }

    public virtual void SetHeadPosition()
    {

    }

    public virtual void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public virtual void Show()
    {
        SetActive(true);
        SetHeadActive(!isHideAllName);
        SetActorHpActive(true);
        SetHpFillAmount();
        SetNameColor();
        SetName();
    }

    public void SetActorNameActive(bool isActive)
    {
        if ((lb_actorName != null) && (lb_actorName.gameObject.activeSelf != isActive))
        {
            lb_actorName.gameObject.SetActive(isActive);
        }
    }

    public void SetActorHpActive(bool isActive)
    {
        if ((obj_hp != null )&& (obj_hp.activeSelf != isActive))
        {
            obj_hp.gameObject.SetActive(isActive);
        }
    }

    private void SetHpFillAmount()
    {
        mHpFillAmount = mAvatar.BaseInfo.HP * 1.0f / mAvatar.BaseInfo.MaxHP;
        if(spr_hp != null)
        {
            spr_hp.fillAmount = mHpFillAmount;
        }
    }

    public void PlayHurtEffect(int deltaHp, int flyDirection,float delayTime,int type = ThrowTextType.None)
    {
        if (obj_hurt != null && obj_hurtParent != null)
        {
            CSActorHurtEffect.Instance.PlayHurtEffect(obj_hurt, obj_hurtParent, deltaHp, flyDirection, delayTime,type);
        }
    }

    private void OnHeadHpChange(uint evtId,object obj)
    {
        if (this == null || mAvatar == null || mAvatar.BaseInfo.MaxHP == 0)
        {
            return;
        }
        mHpFillAmount = mAvatar.BaseInfo.HP * 1.0f / mAvatar.BaseInfo.MaxHP;
    }

    private UIActorHurtEffect GetUnusedHurtObj()
    {
        if(hurtCloneList == null)
        {
            return null;
        }
        for(int i = 0, iMax = hurtCloneList.Count; i < iMax; i++)
        {
            if((hurtCloneList[i] != null) && (!hurtCloneList[i].IsAvtive()))
            {
                return hurtCloneList[i];
            }
        }
        return null;
    }

    public virtual void Destroy()
    {
        gameObject.SetActive(false);
    }

    public virtual void Release()
    {
        Destroy();
    }

}
