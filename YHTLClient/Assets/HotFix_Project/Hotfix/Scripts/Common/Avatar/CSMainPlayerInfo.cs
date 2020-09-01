using fight;
using Google.Protobuf.Collections;
using Main_Project.Script.Update;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using user;

public class CSMainPlayerInfo : CSPlayerInfo
{
    private static CSMainPlayerInfo m_instance;

    public static CSMainPlayerInfo Instance
    {
        get
        {
            if (null == m_instance)
            {
                m_instance = new CSMainPlayerInfo(null);
            }

            return m_instance;
        }

        private set { m_instance = value; }
    }

    public static void CreateInstance(user.PlayerInfo data)
    {
        if (m_instance != null)
        {
            if (m_instance.ID == data.roleBrief.roleId)
            {
                m_instance.UpdatePlayerInfo(data);
            }
            else
            {
                m_instance.Release();
                m_instance = new CSMainPlayerInfo(data);
            }
        }
        else
        {
            m_instance = new CSMainPlayerInfo(data);
        }
    }

    public override string Name
    {
        get { return name; }
        set
        {
            base.Name = value;
            CSConstant.MainPlayerName = name;
            BuglyAgent.SetUserId(value);
        }
    }

    //全局事件，用于主角的消息发送
    public EventHanlderManager mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);

    #region RoleBrief（自定义字段不要写这里）

    //1 角色ID  (avatar中定义中定义)
    //2 名称    (avatar中定义中定义)
    //3 等级 (avatar中定义中定义)
    public override int Level
    {
        set
        {
            if (level != value)
            {
                level = value;
                mClientEvent.SendEvent(CEvent.MainPlayer_LevelChange);
            }
        }
    }

    //4 职业 (avatar中定义中定义)
    //5 性别 (avatar中定义中定义)
    //6 所在地图 (avatar中定义中定义)
    public override int MapID
    {
        get { return base.MapID; }
        set
        {
            if (value != base.MapID)
            {
                mLastMapId = base.MapID;
                base.MapID = value;
                mClientEvent.SendEvent(CEvent.Role_ChangeMapId);

                if (MapInfoTableManager.Instance.TryGetValue(value, out TABLE.MAPINFO mapinfo))
                {
                    CSMainParameterManager.tableMapInfo.SetMapInfo(mapinfo.id, mapinfo.mapSize, mapinfo.img);
                }
            }
        }
    }

    public override int GuildLevel
    {
        get { return _guildLevel; }

        set
        {
            _guildLevel = value;
            mClientEvent.SendEvent(CEvent.OnMainPlayerGuildLvChanged);
        }
    }

    public override long GuildCreateId
    {
        get { return _guildCreateId; }

        set { _guildCreateId = value; }
    }

    //7 所在分线 (avatar中定义中定义)
    //8 血量 (avatar中定义中定义)
    //9 最大血量 (avatar中定义中定义)
    //10 x (avatar中定义中定义)
    //11 y (avatar中定义中定义)
    //12 武器 (avatar中定义中定义)
    //13 衣服 (avatar中定义中定义)
    //14 身上的buff    (avatar中定义中定义)
    //15 魔法值 (avatar中定义中定义)
    //16 最大魔法值 (avatar中定义中定义)
    //19 帮会名称
    public override string GuildName
    {
        set
        {
            if (value == mGuildName)
                return;
            base.GuildName = value;
            mClientEvent.SendEvent(CEvent.OnMainPlayerGuildNameChanged);
        }
        get { return mGuildName; }
    }

    //20 帮会职位
    public override int GuildPos
    {
        set
        {
            if (mGuildPos != value)
            {
                mGuildPos = value;
                EventHandler.SendEvent(CEvent.OnMainPlayerGuildPosChanged);
            }
        }
        get { return mGuildPos; }
    }

    //21 坐骑
    //22 是否骑乘状态
    //23 称号
    //24 帮会ID（CSPlayerInfo定义）
    public override long GuildId
    {
        get { return _guildId; }

        set
        {
            if (_guildId != value)
            {
                base.GuildId = value;
                mClientEvent.SendEvent(CEvent.OnMainPlayerGuildIdChanged);
            }
        }
    }

    //29 队伍ID（CSPlayerInfo定义）
    public override long TeamId
    {
        get { return _teamId; }

        set
        {
            if (_teamId != value)
            {
                _teamId = value;
                mClientEvent.SendEvent(CEvent.OnMainPlayerTeamIdChanged);
            }
        }
    }

    //30 帮会等级
    //32 显示图标
    //33 角色以前的服务器ID
    //34 角色当前的服务器ID
    //35 当前服务器类型
    //39 头像
    //40 速度(avatar中定义)
    //41 时装衣服
    //43 激活的卧龙套装
    //44 激活的卧龙战魂id
    int _wolongPetId;

    public int WolongPetId
    {
        get { return _wolongPetId; }

        protected set { _wolongPetId = value; }
    }

    //45 翅膀id

    #endregion

    #region PlayerInfo

    public override long Exp
    {
        set
        {
            _Exp = value;
            mClientEvent.SendEvent(CEvent.GetExp);
        }
    }

    public override int PkValue
    {
        get { return base.PkValue; }
        set
        {
            base.PkValue = value;
            SetPkNameMode();
            mClientEvent.SendEvent(CEvent.PkValueUpdate);
        }
    }

    int _PkMode;

    public int PkMode
    {
        get { return _PkMode; }
        set
        {
            _PkMode = value;
            mClientEvent.SendEvent(CEvent.PkModeChangedNtf);
        }
    }
    int _PkModeMap;

    public int PkModeMap
    {
        get { return _PkModeMap; }
        set
        {
            _PkModeMap = value;
            mClientEvent.SendEvent(CEvent.PkModeChangedNtf);
        }
    }

    private int mTeamMode;

    public int TeamMode
    {
        get { return mTeamMode; }
        set
        {
            if (mTeamMode != value)
            {
                mTeamMode = value;
            }
        }
    }

    int _oldfightPower;

    public int OldFightPower
    {
        get { return _oldfightPower; }
        set { _oldfightPower = value; }
    }

    int _fightPower;

    public int fightPower
    {
        get { return _fightPower; }
        set { _fightPower = value; }
    }

    long _serverOpenDay;

    public long ServerOpenDay
    {
        get { return _serverOpenDay; }
        set { _serverOpenDay = value; }
    }

    long _serverOpenTime;

    public long ServerOpenTime
    {
        get { return _serverOpenTime; }
        set { _serverOpenTime = value; }
    }

    #endregion

    #region 自定义字段
    public override bool CanSpeak
    {
        get { return canSpeak; }
        set
        {
            if (canSpeak != value)
            {
                base.canSpeak = value;
                mClientEvent.SendEvent(CEvent.MainPlayerCanSpeak);
            }
        }
    }

    //会员等级
    public override int VipLevel
    {
        get { return _vipLevel; }
        set
        {
            _vipLevel = value;
            mClientEvent.SendEvent(CEvent.OnMainPlayerVipLevelChanged);
        }
    }

    private int mPosChangeReason;

    public int PosChangeReason
    {
        get { return mPosChangeReason; }
        set { mPosChangeReason = value; }
    }

    #region 任务传送EventId

    private RepeatedField<int> mMainTaskEvevtIds;

    public RepeatedField<int> MainTaskEventIds
    {
        get { return mMainTaskEvevtIds; }
        set { mMainTaskEvevtIds = value; }
    }

    public void AddEventId(RepeatedField<int> eventIds)
    {
        if (mMainTaskEvevtIds == null)
        {
            mMainTaskEvevtIds = new RepeatedField<int>();
        }

        for (int i = 0; i < eventIds.Count; ++i)
        {
            if (!mMainTaskEvevtIds.Contains(eventIds[i]))
            {
                mMainTaskEvevtIds.Add(eventIds[i]);
            }
        }
    }

    public void RemoveEventId(RepeatedField<int> eventIds)
    {
        if (mMainTaskEvevtIds == null)
        {
            return;
        }

        for (int i = 0; i < eventIds.Count; ++i)
        {
            if (mMainTaskEvevtIds.Contains(eventIds[i]))
            {
                mMainTaskEvevtIds.Remove(eventIds[i]);
            }
        }
    }

    #endregion

    int _pkNameMode;

    public int pkNameMode
    {
        get { return _pkNameMode; }
        set { _pkNameMode = value; }
    }

    private int mPlayerFsmState;

    public int PlayerFsmState
    {
        get { return mPlayerFsmState; }
        set
        {
            if (mPlayerFsmState != value)
            {
                mPlayerFsmState = value;
            }
        }
    }
    public bool IsFirstGetZhenQiCurrentDay = true;

    #endregion


    /// <summary>玩家额外数据</summary>
    private player.RoleExtraValues mRoleExtraValues;

    /// <summary>玩家额外数据</summary>
    public player.RoleExtraValues RoleExtraValues
    {
        get { return mRoleExtraValues; }
        set { mRoleExtraValues = value; }
    }

    user.PlayerInfo mInfo;

    public CSMainPlayerInfo(user.PlayerInfo data, int avatarType = EAvatarType.MainPlayer)
        : base(data, avatarType)
    {
        if (data == null) return;
        mInfo = data;
        PkMode = data.pkMode;
        PkValue = data.pkValue;
        Exp = data.exp;
        SetAttrData();
        TeamMode = data.teamMode;
        WolongPetId = data.roleBrief.wolongPetId;
        fightPower = data.attribute.nbValue;
        ServerOpenDay = data.roleExtraValues.openServerDays;
        ServerOpenTime = data.serverOpenTime;
        RoleExtraValues = data.roleExtraValues;
    }


    private int mLastMapId;

    public int LastMapID
    {
        get { return mLastMapId; }
        set
        {
            if (value != mLastMapId)
            {
                mLastMapId = value;
            }
        }
    }

    public user.PlayerInfo GetMyInfo()
    {
        return mInfo;
    }

    Dictionary<int, TupleProperty> attrDic = new Dictionary<int, TupleProperty>(124);
    //玩家属性值变化展示
    RepeatedField<int> keyRepreated = new RepeatedField<int>(124);
    RepeatedField<int> valueRepreated = new RepeatedField<int>(124);

    public Dictionary<int, TupleProperty> GetMyAttr()
    {
        return attrDic;
    }
    public RepeatedField<int> GetChangeAttrKey()
    {
        return keyRepreated;
    }
    public RepeatedField<int> GetChangeAttrValue()
    {
        return valueRepreated;
    }
    public int GetMyAttrById(int _id)
    {
        TupleProperty pro;
        if (attrDic.TryGetValue(_id, out pro))
        {
            return pro.value;
        }

        return 0;
    }

    void SetAttrData()
    {
        attrDic.Clear();
        for (int i = 0; i < mInfo.attribute.attrs.Count; i++)
        {
            attrDic.Add(mInfo.attribute.attrs[i].type, mInfo.attribute.attrs[i]);
            if (mInfo.attribute.attrs[i].type == 11)
            {
                //Debug.Log("mainPlayerinfo   hp   " + mInfo.attribute.attrs[i].value);
                MaxHP = mInfo.attribute.attrs[i].value;
                mClientEvent.SendEvent(CEvent.HP_Change);
            }
            else if (mInfo.attribute.attrs[i].type == 12)
            {
                //Debug.Log("mainPlayerinfo   mp   " + mInfo.attribute.attrs[i].value);
                MaxMP = mInfo.attribute.attrs[i].value;
                mClientEvent.SendEvent(CEvent.MP_Change);
            }
        }
    }

    void AttrChange(user.PlayerAttribute _info)
    {
        keyRepreated.Clear();
        valueRepreated.Clear();
        user.TupleProperty curProperty;
        for (int i = 0, max = _info.attrs.Count; i < max; i++)
        {
            curProperty = _info.attrs[i];
            if (attrDic.ContainsKey(curProperty.type))
            {
                if (curProperty.value - attrDic[curProperty.type].value > 0)
                {
                    keyRepreated.Add(curProperty.type);
                    valueRepreated.Add(curProperty.value - attrDic[curProperty.type].value);
                }
                attrDic[curProperty.type] = curProperty;
            }
            else
            {
                attrDic.Add(curProperty.type, curProperty);
                keyRepreated.Add(curProperty.type);
                valueRepreated.Add(curProperty.value);
            }

            if (_info.attrs[i].type == 11)
            {
                MaxHP = _info.attrs[i].value;
                mClientEvent.SendEvent(CEvent.HP_Change);
            }
            else if (_info.attrs[i].type == 12)
            {
                MaxMP = _info.attrs[i].value;
                mClientEvent.SendEvent(CEvent.MP_Change);
            }
        }
    }

    public void ResPlayAttributeChangedMessage(user.PlayerAttribute _info)
    {
        AttrChange(_info);
        //Debug.Log(fightPower + "   战斗力变化  " + _info.nbValue);
        OldFightPower = fightPower;
        fightPower = _info.nbValue;
        mClientEvent.SendEvent(CEvent.FightPowerChange, OldFightPower);
    }

    public void GetLevelChangedMes(player.RoleUpgrade _msg)
    {
        Exp = _msg.exp;
        Level = _msg.newLevel;
    }

    void SetPkNameMode()
    {
        if (redNameParam == null)
        {
            redNameParam = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetDes(107));
        }

        if (PkValue <= 0)
        {
            pkNameMode = PlayerPkMode.White;
        }
        else if (0 < PkValue && PkValue <= redNameParam[0])
        {
            pkNameMode = PlayerPkMode.Yellow;
        }
        else
        {
            pkNameMode = PlayerPkMode.Red;
        }
    }

    List<int> redNameParam;

    public bool IsRedNameResurrection()
    {
        if (redNameParam == null)
        {
            redNameParam = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetDes(107));
        }

        return PkValue >= redNameParam[0];
    }

    public long GetCurLevelMaxExp()
    {
        return LevelTableManager.Instance.GetExpByLevel(Level);
    }

    public void ResetWhenChangeMap(int x, int y, int mapId, int reason)
    {
        Coord = new CSMisc.Dot2(x, y);
        MapID = mapId;
        PosChangeReason = reason;
        //if (MapInfoTableManager.Instance.GetMapInfoMapPKMode(MapID) != 0)
        //{
        PkModeMap = MapInfoTableManager.Instance.GetMapInfoMapPKMode(MapID);
        //}

    }

    /// <summary>
    /// 每日更新角色额外数据
    /// </summary>
    /// <param name="data"></param>
    public void DayPassedRoleExtraValues(player.DayPassed data)
    {
        if (data == null) return;
        PlayerRoleExtraValues(data.roleExtraValues);
        IsFirstGetZhenQiCurrentDay = true;
    }

    /// <summary>
    /// 玩家额外数据更新
    /// </summary>
    /// <param name="data"></param>
    public void PlayerRoleExtraValues(player.RoleExtraValues data)
    {
        RoleExtraValues = data;
        Constant.isIdCardNumberEntered = data.isIdCardNumberEntered;
        Constant.isOver18 = data.isOver18;
    }

    public override void Release()
    {
        base.Release();
        RoleExtraValues = null;
        mClientEvent?.UnRegAll();
        mClientEvent = null;
        Instance = null;
    }
}