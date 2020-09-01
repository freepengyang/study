using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using user;

public class CSPlayerInfo : CSAvatarInfo
{
    //是否能语音聊天
    protected bool canSpeak;
    public virtual bool CanSpeak
    {
        get { return canSpeak; }
        set
        {
            if (canSpeak != value)
            {
                canSpeak = value;
                //EventHandler.SendEvent(CEvent.MainPlayerCanSpeak);
            }
        }
    }

    /// <summary>
    /// 是否拥有护体
    /// </summary>
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

    //private bool oldIsshield = true;
    
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
                //bool isshield = shield > 0;
                //if (oldIsshield != isshield)
                //{
                EventHandler.SendEvent(CEvent.ShieldChange,shield > 0);
                    //oldIsshield = isshield;
                //}
                
            }
        }
    }

    /// <summary>
    /// 当前宠物最大护盾值
    /// </summary>
    private int maxShield;

    public int MaxShield
    {
        get => maxShield;
        set  
        {
            if (value >= 0 && maxShield != value)
            {
                maxShield = value;
                // EventHandler.SendEvent(CEvent.MaxShieldChange);
            }
        }
    }

    /// <summary>
    /// 时装称号Id
    /// </summary>
    private int titleId;
    public int TitleId
    {
        get => titleId;
        set  
        {
            if (value >= 0 && titleId != value)
            {
                titleId = value;
                EventHandler.SendEvent(CEvent.Player_TitleChange, titleId);
            }
        }
    }

    protected long _teamId;
    public virtual long TeamId
    {
        get
        {
            return _teamId;
        }
        set
        {
            if(_teamId == value) return;
            _teamId = value;
            EventHandler.SendEvent(CEvent.Player_TeamIdChange, _teamId);
        }
    }

    protected long _guildCreateId;
    public virtual long GuildCreateId
    {
        get
        {
            return _guildCreateId;
        }

        set
        {
            _guildCreateId = value;
        }
    }

    protected long _guildId;
    public virtual long GuildId
    {
        get
        {
            return _guildId;
        }

        set
        {
            if(_guildId != value)
            {
                _guildId = value;
                EventHandler.SendEvent(CEvent.player_GuildIdChange);
            }
        }
    }

    int _pkValue;
    public virtual int PkValue
    {
        get
        {
            return _pkValue;
        }
        set
        {
            if(_pkValue != value)
            {
                _pkValue = value;
                EventHandler.SendEvent(CEvent.player_pkValueChange);
            }
        }
    }

    bool _greyName;
    public virtual bool GreyName
    {
        get
        {
            return _greyName;
        }
        set
        {
            if (_greyName != value)
            {
                _greyName = value;
                EventHandler.SendEvent(CEvent.player_greyStateChange);
            }
        }
    }

    public override string Name 
    { 
        get
        {
            return name;
        }
        set
        {
            if(name != value)
            {
                name = value;
                EventHandler.SendEvent(CEvent.player_NameChange);
            }
        }
    }

    protected string mGuildName = string.Empty;
    public virtual string GuildName
    {
        set
        {
            if (value == mGuildName) return;
            mGuildName = value;
            EventHandler.SendEvent(CEvent.player_GuildNameChange);
        }
        get
        {
            return mGuildName;
        }
    }

    protected int mGuildPos;
    public virtual int GuildPos
    {
        set
        {
            if (mGuildPos != value)
            {
                mGuildPos = value;
            }
        }
        get
        {
            return mGuildPos;
        }
    }

    protected int _guildLevel;
    public virtual int GuildLevel
    {
        get { return _guildLevel; }

        set
        {
            _guildLevel = value;
        }
    }

    protected int _vipLevel;
    public virtual int VipLevel
    {
        get
        {
            return _vipLevel;
        }
        set
        {
            _vipLevel = value;
        }
    }

    protected int level;
    public override int Level
    {
        get { return level; }
        set
        {
            if (level != value)
            {
                level = value;
                EventHandler.SendEvent(CEvent.player_GuildNameChange);
            }
        }
    }

    protected long _Exp = 0;
    public override long Exp
    {
        get { return _Exp; }
        set
        {
            _Exp = value;
        }
    }

    public override int HP
    {
        get { return base.HP; }
        set
        {
            if (base.HP != value)
            {
                int lastHp = base.HP;
                base.HP = value;
                EventHandler.SendEvent(CEvent.HP_Change);
                if(lastHp == 0 && value > 0)
                {
                    EventHandler.SendEvent(CEvent.Player_Relive);
                }
            }
        }
    }

    public override int MP
    {
        get { return base.MP; }
        set
        {
            if (base.MP != value)
            {
                base.MP = value;
                EventHandler.SendEvent(CEvent.MP_Change);
            }
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

    public override int BodyModel
    {
        get
        {
            return base.BodyModel;
        }
        set
        {
            if(value != base.BodyModel)
            {
                base.BodyModel = value;
                EventHandler.SendEvent(CEvent.Player_ReplaceEquip);
            }
        }
    }

    public override int Weapon
    {
        get
        {
            return base.Weapon;
        }
        set
        {
            if(value != base.Weapon)
            {
                base.Weapon = value;
                EventHandler.SendEvent(CEvent.Player_ReplaceEquip);
            }
        }
    }
    int _fashionCloth;
    public  int FashionCloth
    {
        get
        {
            return _fashionCloth;
        }
        set
        {
            if (value != _fashionCloth)
            {
                _fashionCloth = value;
                EventHandler.SendEvent(CEvent.Player_ReplaceEquip);
            }
        }
    }
    int _fashionWeapon;
    public  int FashionWeapon
    {
        get
        {
            return _fashionWeapon;
        }
        set
        {
            if (value != _fashionWeapon)
            {
                _fashionWeapon = value;
                EventHandler.SendEvent(CEvent.Player_ReplaceEquip);
            }
        }
    }

    private long mWingID;
    public virtual long WingID
    {
        get
        {
            return mWingID;
        }
        set
        {
            if(mWingID != value)
            {
                mWingID = value;
                EventHandler.SendEvent(CEvent.Player_ReplaceEquip);
            }
        }
    }

    public CSPlayerInfo(user.PlayerInfo data, int avatarType = EAvatarType.Player)
    {
        AvatarType = avatarType;
        if (data == null) 
            return;
        BuffInfo.Init(data.buffers);
        UpdateRoleBrief(data.roleBrief);
        CanSpeak = data.canSpeak;
    }

    public CSPlayerInfo(RoleBrief data, int avatarType = EAvatarType.Player)
    {
        AvatarType = avatarType;
        if (data == null) return;
        UpdateRoleBrief(data);
    }

    public void UpdatePlayerInfo(user.PlayerInfo data, int avatarType = EAvatarType.Player)
    {
        UpdateRoleBrief(data.roleBrief);
    }

    public virtual void UpdateRoleBrief(user.RoleBrief data)
    {
        if (data == null) return;
        ID = data.roleId;
        Name = data.roleName;
        Level = data.level;
        Career = data.career;
        Sex = data.sex;
        MapID = data.mapId;
        Line = data.line;
        HP = data.hp;
        RealHP = data.hp;
        MaxHP = data.maxHp;
        MP = data.mp;
        MaxMP = data.maxMp;
        mCoord.x = data.x;
        mCoord.y = data.y;
        Weapon = data.weapon;
        Speed = data.speed;
        BodyModel = (data.armor == 0 ? (ESex.Man == Sex ? 615000 : 625000) : data.armor);
        TeamId = data.teamId;
        GuildId = data.unionId;
        GuildPos = data.unionPos;
        GuildName = data.unionName;
        GuildCreateId = 0;
        VipLevel = data.vipLevel;
        TitleId = data.titleId;
        WingID = data.wingId;
        FashionCloth = data.fashionId;
        FashionWeapon = data.fashionWeaponId;
        PkValue = data.pkValue;
        GreyName = data.greyName == 1;
        Shield = data.shieldAttr;
        MaxShield = data.maxShield;
        HasShield = data.hasShield;
        BuffInfo.Init(data.buffers);
    }

}
