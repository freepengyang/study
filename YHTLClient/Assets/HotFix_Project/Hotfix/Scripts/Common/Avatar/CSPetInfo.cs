using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSPetInfo : CSAvatarInfo
{
    private long mMasterID;
    public long MasterID
    {
        get
        {
            return mMasterID;
        }
        set
        {
            mMasterID = value;
        }
    }
    public override CSBuffInfo BuffInfo
    {
        get
        {
            if (base.BuffInfo == null)
            {
                base.BuffInfo = new CSBuffInfo();
            }
            return base.BuffInfo;
        }
        set
        {
            base.BuffInfo = value;
        }
    }

    public override int HP
    {
        get
        {
            return base.HP;
        }
        set
        {
            if (base.HP != value)
            {
                base.HP = value;
                EventHandler.SendEvent(CEvent.HP_Change);
            }
        }
    }

    private int mState;
    public int State
    {
        get
        {
            return mState;
        }
        set
        {
            if(mState != value)
            {
                mState = value;
                EventHandler.SendEvent(CEvent.Pet_StateChange);
            }
        }
    }

    private bool hasShield;

    public bool HasShield
    {
        get => hasShield;
        set
        {
            if (hasShield != value && value)
            {
                hasShield = value;
                EventHandler.SendEvent(CEvent.ActiveShield);
            }
        }
    }

    /// <summary>
    /// 当前宠物护盾值
    /// </summary>
    private int shield;

    public int Shield
    {
        get => shield;
        set
        {
            if (value >= 0 && shield != value)
            {

                shield = value;
                EventHandler.SendEvent(CEvent.ShieldChange,shield > 0);
                
            }
        }
    }

    private bool awaked;
    public bool Awaked
    {
        get => awaked;
        set
        {
            awaked = value;
            EventHandler.SendEvent(CEvent.pet_awaked, awaked);
        }
    }
    
    public void Init(map.RoundPet info)
    {
        if(info == null)
        {
            return;
        }
        ID = info.petId;
        ConfigId = info.monsterConfigId;
        Name = info.petName;
        Level = info.level;
        HP = info.hp;
        RealHP = info.hp;
        MaxHP = info.maxHp;
        Coord = new CSMisc.Dot2(info.x, info.y);
        Speed = info.speed;
        BodyModel = (int)ConfigId;
        AvatarType = EAvatarType.Pet;
        MasterID = info.masterId;
        Quality = 0;
        hasShield = info.hasShield;
        Shield = info.shieldAttr;
        State = info.state;
        Awaked = info.activePvp;
        BuffInfo.Init(info.buffers);
    }
}
