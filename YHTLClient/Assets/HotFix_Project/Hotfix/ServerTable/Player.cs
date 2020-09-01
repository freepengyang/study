// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Player.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace player {

  #region Messages
  /// <summary>
  ///经验值变化
  /// </summary>
  public sealed class RoleExpUpdated : pb::IMessage {
    private static readonly pb::MessageParser<RoleExpUpdated> _parser = new pb::MessageParser<RoleExpUpdated>(() => new RoleExpUpdated());
    public static pb::MessageParser<RoleExpUpdated> Parser { get { return _parser; } }

    private long exp_;
    public long exp {
      get { return exp_; }
      set {
        exp_ = value;
      }
    }

    private long energyExp_;
    /// <summary>
    ///精力值经验
    /// </summary>
    public long energyExp {
      get { return energyExp_; }
      set {
        energyExp_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (exp != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(exp);
      }
      if (energyExp != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(energyExp);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (exp != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(exp);
      }
      if (energyExp != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(energyExp);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            exp = input.ReadInt64();
            break;
          }
          case 16: {
            energyExp = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///玩家升级
  /// </summary>
  public sealed class RoleUpgrade : pb::IMessage {
    private static readonly pb::MessageParser<RoleUpgrade> _parser = new pb::MessageParser<RoleUpgrade>(() => new RoleUpgrade());
    public static pb::MessageParser<RoleUpgrade> Parser { get { return _parser; } }

    private long roleId_;
    public long roleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    private int oldLevel_;
    public int oldLevel {
      get { return oldLevel_; }
      set {
        oldLevel_ = value;
      }
    }

    private int newLevel_;
    public int newLevel {
      get { return newLevel_; }
      set {
        newLevel_ = value;
      }
    }

    private long exp_;
    public long exp {
      get { return exp_; }
      set {
        exp_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (roleId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(roleId);
      }
      if (oldLevel != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(oldLevel);
      }
      if (newLevel != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(newLevel);
      }
      if (exp != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(exp);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (roleId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(roleId);
      }
      if (oldLevel != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(oldLevel);
      }
      if (newLevel != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(newLevel);
      }
      if (exp != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(exp);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            roleId = input.ReadInt64();
            break;
          }
          case 16: {
            oldLevel = input.ReadInt32();
            break;
          }
          case 24: {
            newLevel = input.ReadInt32();
            break;
          }
          case 32: {
            exp = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///角色额外数据
  /// </summary>
  public sealed class RoleExtraValues : pb::IMessage {
    private static readonly pb::MessageParser<RoleExtraValues> _parser = new pb::MessageParser<RoleExtraValues>(() => new RoleExtraValues());
    public static pb::MessageParser<RoleExtraValues> Parser { get { return _parser; } }

    private int todayCharge_;
    /// <summary>
    ///今日充值元宝
    /// </summary>
    public int todayCharge {
      get { return todayCharge_; }
      set {
        todayCharge_ = value;
      }
    }

    private int todayTimes_;
    /// <summary>
    ///今日充值次数
    /// </summary>
    public int todayTimes {
      get { return todayTimes_; }
      set {
        todayTimes_ = value;
      }
    }

    private int totalLoginDays_;
    /// <summary>
    ///累计登录天数
    /// </summary>
    public int totalLoginDays {
      get { return totalLoginDays_; }
      set {
        totalLoginDays_ = value;
      }
    }

    private int openServerDays_;
    /// <summary>
    ///开服天数
    /// </summary>
    public int openServerDays {
      get { return openServerDays_; }
      set {
        openServerDays_ = value;
      }
    }

    private int levelPacks_;
    /// <summary>
    ///等级奖励领取等级
    /// </summary>
    public int levelPacks {
      get { return levelPacks_; }
      set {
        levelPacks_ = value;
      }
    }

    private bool drawFirstSouvenir_;
    /// <summary>
    ///是否已领取首冲奖励
    /// </summary>
    public bool drawFirstSouvenir {
      get { return drawFirstSouvenir_; }
      set {
        drawFirstSouvenir_ = value;
      }
    }

    private int vipExp_;
    /// <summary>
    ///vip经验（充值量）
    /// </summary>
    public int vipExp {
      get { return vipExp_; }
      set {
        vipExp_ = value;
      }
    }

    private bool downloadReward_;
    /// <summary>
    ///下载奖励是否已领
    /// </summary>
    public bool downloadReward {
      get { return downloadReward_; }
      set {
        downloadReward_ = value;
      }
    }

    private bool isBindPhoneRewardGet_;
    /// <summary>
    ///绑定手机号奖励是否已领
    /// </summary>
    public bool isBindPhoneRewardGet {
      get { return isBindPhoneRewardGet_; }
      set {
        isBindPhoneRewardGet_ = value;
      }
    }

    private long updateLevelTime_;
    /// <summary>
    ///上次升级时间
    /// </summary>
    public long updateLevelTime {
      get { return updateLevelTime_; }
      set {
        updateLevelTime_ = value;
      }
    }

    private bool gm_;
    /// <summary>
    ///裁判者视角
    /// </summary>
    public bool gm {
      get { return gm_; }
      set {
        gm_ = value;
      }
    }

    private long lastDieTime_;
    /// <summary>
    ///上次死亡时间戳
    /// </summary>
    public long lastDieTime {
      get { return lastDieTime_; }
      set {
        lastDieTime_ = value;
      }
    }

    private bool isIdCardNumberEntered_;
    /// <summary>
    ///是否输入过实名信息
    /// </summary>
    public bool isIdCardNumberEntered {
      get { return isIdCardNumberEntered_; }
      set {
        isIdCardNumberEntered_ = value;
      }
    }

    private bool isOver18_;
    /// <summary>
    ///是否大于18岁
    /// </summary>
    public bool isOver18 {
      get { return isOver18_; }
      set {
        isOver18_ = value;
      }
    }

    private int onlineTime_;
    /// <summary>
    ///在线时间, 秒
    /// </summary>
    public int onlineTime {
      get { return onlineTime_; }
      set {
        onlineTime_ = value;
      }
    }

    private long createTime_;
    /// <summary>
    ///创建角色时间
    /// </summary>
    public long createTime {
      get { return createTime_; }
      set {
        createTime_ = value;
      }
    }

    private int buQianCount_;
    /// <summary>
    ///补签次数
    /// </summary>
    public int buQianCount {
      get { return buQianCount_; }
      set {
        buQianCount_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (todayCharge != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(todayCharge);
      }
      if (todayTimes != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(todayTimes);
      }
      if (totalLoginDays != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(totalLoginDays);
      }
      if (openServerDays != 0) {
        output.WriteRawTag(200, 1);
        output.WriteInt32(openServerDays);
      }
      if (levelPacks != 0) {
        output.WriteRawTag(232, 1);
        output.WriteInt32(levelPacks);
      }
      if (drawFirstSouvenir != false) {
        output.WriteRawTag(240, 1);
        output.WriteBool(drawFirstSouvenir);
      }
      if (vipExp != 0) {
        output.WriteRawTag(248, 1);
        output.WriteInt32(vipExp);
      }
      if (downloadReward != false) {
        output.WriteRawTag(160, 2);
        output.WriteBool(downloadReward);
      }
      if (isBindPhoneRewardGet != false) {
        output.WriteRawTag(216, 2);
        output.WriteBool(isBindPhoneRewardGet);
      }
      if (updateLevelTime != 0L) {
        output.WriteRawTag(128, 3);
        output.WriteInt64(updateLevelTime);
      }
      if (gm != false) {
        output.WriteRawTag(160, 4);
        output.WriteBool(gm);
      }
      if (lastDieTime != 0L) {
        output.WriteRawTag(216, 4);
        output.WriteInt64(lastDieTime);
      }
      if (isIdCardNumberEntered != false) {
        output.WriteRawTag(224, 4);
        output.WriteBool(isIdCardNumberEntered);
      }
      if (isOver18 != false) {
        output.WriteRawTag(232, 4);
        output.WriteBool(isOver18);
      }
      if (onlineTime != 0) {
        output.WriteRawTag(240, 4);
        output.WriteInt32(onlineTime);
      }
      if (createTime != 0L) {
        output.WriteRawTag(208, 6);
        output.WriteInt64(createTime);
      }
      if (buQianCount != 0) {
        output.WriteRawTag(216, 6);
        output.WriteInt32(buQianCount);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (todayCharge != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(todayCharge);
      }
      if (todayTimes != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(todayTimes);
      }
      if (totalLoginDays != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(totalLoginDays);
      }
      if (openServerDays != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(openServerDays);
      }
      if (levelPacks != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(levelPacks);
      }
      if (drawFirstSouvenir != false) {
        size += 2 + 1;
      }
      if (vipExp != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(vipExp);
      }
      if (downloadReward != false) {
        size += 2 + 1;
      }
      if (isBindPhoneRewardGet != false) {
        size += 2 + 1;
      }
      if (updateLevelTime != 0L) {
        size += 2 + pb::CodedOutputStream.ComputeInt64Size(updateLevelTime);
      }
      if (gm != false) {
        size += 2 + 1;
      }
      if (lastDieTime != 0L) {
        size += 2 + pb::CodedOutputStream.ComputeInt64Size(lastDieTime);
      }
      if (isIdCardNumberEntered != false) {
        size += 2 + 1;
      }
      if (isOver18 != false) {
        size += 2 + 1;
      }
      if (onlineTime != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(onlineTime);
      }
      if (createTime != 0L) {
        size += 2 + pb::CodedOutputStream.ComputeInt64Size(createTime);
      }
      if (buQianCount != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(buQianCount);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            todayCharge = input.ReadInt32();
            break;
          }
          case 16: {
            todayTimes = input.ReadInt32();
            break;
          }
          case 40: {
            totalLoginDays = input.ReadInt32();
            break;
          }
          case 200: {
            openServerDays = input.ReadInt32();
            break;
          }
          case 232: {
            levelPacks = input.ReadInt32();
            break;
          }
          case 240: {
            drawFirstSouvenir = input.ReadBool();
            break;
          }
          case 248: {
            vipExp = input.ReadInt32();
            break;
          }
          case 288: {
            downloadReward = input.ReadBool();
            break;
          }
          case 344: {
            isBindPhoneRewardGet = input.ReadBool();
            break;
          }
          case 384: {
            updateLevelTime = input.ReadInt64();
            break;
          }
          case 544: {
            gm = input.ReadBool();
            break;
          }
          case 600: {
            lastDieTime = input.ReadInt64();
            break;
          }
          case 608: {
            isIdCardNumberEntered = input.ReadBool();
            break;
          }
          case 616: {
            isOver18 = input.ReadBool();
            break;
          }
          case 624: {
            onlineTime = input.ReadInt32();
            break;
          }
          case 848: {
            createTime = input.ReadInt64();
            break;
          }
          case 856: {
            buQianCount = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///每日数据更新
  /// </summary>
  public sealed class DayPassed : pb::IMessage {
    private static readonly pb::MessageParser<DayPassed> _parser = new pb::MessageParser<DayPassed>(() => new DayPassed());
    public static pb::MessageParser<DayPassed> Parser { get { return _parser; } }

    private global::player.RoleExtraValues roleExtraValues_;
    public global::player.RoleExtraValues roleExtraValues {
      get { return roleExtraValues_; }
      set {
        roleExtraValues_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (roleExtraValues_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(roleExtraValues);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (roleExtraValues_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(roleExtraValues);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            if (roleExtraValues_ == null) {
              roleExtraValues_ = new global::player.RoleExtraValues();
            }
            input.ReadMessage(roleExtraValues_);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///保存玩家设置请求
  /// </summary>
  public sealed class SaveRoleSettingsMsg : pb::IMessage {
    private static readonly pb::MessageParser<SaveRoleSettingsMsg> _parser = new pb::MessageParser<SaveRoleSettingsMsg>(() => new SaveRoleSettingsMsg());
    public static pb::MessageParser<SaveRoleSettingsMsg> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<long> _repeated_roleSettings_codec
        = pb::FieldCodec.ForInt64(10);
    private readonly pbc::RepeatedField<long> roleSettings_ = new pbc::RepeatedField<long>();
    public pbc::RepeatedField<long> roleSettings {
      get { return roleSettings_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      roleSettings_.WriteTo(output, _repeated_roleSettings_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += roleSettings_.CalculateSize(_repeated_roleSettings_codec);
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10:
          case 8: {
            roleSettings_.AddEntriesFrom(input, _repeated_roleSettings_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///保存新手引导步骤请求
  /// </summary>
  public sealed class SaveNewbieGuideRequest : pb::IMessage {
    private static readonly pb::MessageParser<SaveNewbieGuideRequest> _parser = new pb::MessageParser<SaveNewbieGuideRequest>(() => new SaveNewbieGuideRequest());
    public static pb::MessageParser<SaveNewbieGuideRequest> Parser { get { return _parser; } }

    private int groupId_;
    public int groupId {
      get { return groupId_; }
      set {
        groupId_ = value;
      }
    }

    private int step_;
    public int step {
      get { return step_; }
      set {
        step_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (groupId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(groupId);
      }
      if (step != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(step);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (groupId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(groupId);
      }
      if (step != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(step);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            groupId = input.ReadInt32();
            break;
          }
          case 16: {
            step = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///通用消息
  /// </summary>
  public sealed class CommonInfo : pb::IMessage {
    private static readonly pb::MessageParser<CommonInfo> _parser = new pb::MessageParser<CommonInfo>(() => new CommonInfo());
    public static pb::MessageParser<CommonInfo> Parser { get { return _parser; } }

    private int type_;
    /// <summary>
    ///s->c 1:表示玩家不满足召唤条件 2:能否进苍月伏魔 3:验证码不正确 4:共享服的配置变了 5:结盟公会增加 6:结盟公会解除
    ///c->s 1:结盟解除 2:强求查看别人玩家的基本信息
    /// </summary>
    public int type {
      get { return type_; }
      set {
        type_ = value;
      }
    }

    private int data_;
    public int data {
      get { return data_; }
      set {
        data_ = value;
      }
    }

    private string str_ = "";
    public string str {
      get { return str_; }
      set {
        str_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    private long data64_;
    public long data64 {
      get { return data64_; }
      set {
        data64_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (type != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(type);
      }
      if (data != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(data);
      }
      if (str.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(str);
      }
      if (data64 != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(data64);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (type != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(type);
      }
      if (data != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(data);
      }
      if (str.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(str);
      }
      if (data64 != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(data64);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            type = input.ReadInt32();
            break;
          }
          case 16: {
            data = input.ReadInt32();
            break;
          }
          case 26: {
            str = input.ReadString();
            break;
          }
          case 32: {
            data64 = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  public sealed class I32I64Pair : pb::IMessage {
    private static readonly pb::MessageParser<I32I64Pair> _parser = new pb::MessageParser<I32I64Pair>(() => new I32I64Pair());
    public static pb::MessageParser<I32I64Pair> Parser { get { return _parser; } }

    private int k_;
    public int k {
      get { return k_; }
      set {
        k_ = value;
      }
    }

    private long v_;
    public long v {
      get { return v_; }
      set {
        v_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (k != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(k);
      }
      if (v != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(v);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (k != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(k);
      }
      if (v != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(v);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            k = input.ReadInt32();
            break;
          }
          case 16: {
            v = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///角色属性变更通知
  /// </summary>
  public sealed class RoleAttrNtf : pb::IMessage {
    private static readonly pb::MessageParser<RoleAttrNtf> _parser = new pb::MessageParser<RoleAttrNtf>(() => new RoleAttrNtf());
    public static pb::MessageParser<RoleAttrNtf> Parser { get { return _parser; } }

    private long roleId_;
    public long roleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    private static readonly pb::FieldCodec<global::player.I32I64Pair> _repeated_pairs_codec
        = pb::FieldCodec.ForMessage(18, global::player.I32I64Pair.Parser);
    private readonly pbc::RepeatedField<global::player.I32I64Pair> pairs_ = new pbc::RepeatedField<global::player.I32I64Pair>();
    public pbc::RepeatedField<global::player.I32I64Pair> pairs {
      get { return pairs_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (roleId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(roleId);
      }
      pairs_.WriteTo(output, _repeated_pairs_codec);
    }

    public int CalculateSize() {
      int size = 0;
      if (roleId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(roleId);
      }
      size += pairs_.CalculateSize(_repeated_pairs_codec);
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            roleId = input.ReadInt64();
            break;
          }
          case 18: {
            pairs_.AddEntriesFrom(input, _repeated_pairs_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class ShopBuytItem : pb::IMessage {
    private static readonly pb::MessageParser<ShopBuytItem> _parser = new pb::MessageParser<ShopBuytItem>(() => new ShopBuytItem());
    public static pb::MessageParser<ShopBuytItem> Parser { get { return _parser; } }

    private int shopId_;
    public int shopId {
      get { return shopId_; }
      set {
        shopId_ = value;
      }
    }

    private int count_;
    public int count {
      get { return count_; }
      set {
        count_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (shopId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(shopId);
      }
      if (count != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(count);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (shopId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(shopId);
      }
      if (count != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(count);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            shopId = input.ReadInt32();
            break;
          }
          case 16: {
            count = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class PlayerDie : pb::IMessage {
    private static readonly pb::MessageParser<PlayerDie> _parser = new pb::MessageParser<PlayerDie>(() => new PlayerDie());
    public static pb::MessageParser<PlayerDie> Parser { get { return _parser; } }

    private int killlerType_;
    /// <summary>
    /// 1玩家，2怪物
    /// </summary>
    public int killlerType {
      get { return killlerType_; }
      set {
        killlerType_ = value;
      }
    }

    private string killerName_ = "";
    /// <summary>
    /// 击杀者名字
    /// </summary>
    public string killerName {
      get { return killerName_; }
      set {
        killerName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    private int reliveCount_;
    /// <summary>
    /// 今日复活次数
    /// </summary>
    public int reliveCount {
      get { return reliveCount_; }
      set {
        reliveCount_ = value;
      }
    }

    private long dieTime_;
    /// <summary>
    /// 死亡时间
    /// </summary>
    public long dieTime {
      get { return dieTime_; }
      set {
        dieTime_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (killlerType != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(killlerType);
      }
      if (killerName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(killerName);
      }
      if (reliveCount != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(reliveCount);
      }
      if (dieTime != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(dieTime);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (killlerType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(killlerType);
      }
      if (killerName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(killerName);
      }
      if (reliveCount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(reliveCount);
      }
      if (dieTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(dieTime);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            killlerType = input.ReadInt32();
            break;
          }
          case 18: {
            killerName = input.ReadString();
            break;
          }
          case 24: {
            reliveCount = input.ReadInt32();
            break;
          }
          case 32: {
            dieTime = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///pk值变化
  /// </summary>
  public sealed class PkValueUpdate : pb::IMessage {
    private static readonly pb::MessageParser<PkValueUpdate> _parser = new pb::MessageParser<PkValueUpdate>(() => new PkValueUpdate());
    public static pb::MessageParser<PkValueUpdate> Parser { get { return _parser; } }

    private int pkValue_;
    public int pkValue {
      get { return pkValue_; }
      set {
        pkValue_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (pkValue != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(pkValue);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (pkValue != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(pkValue);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            pkValue = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///请求
  /// </summary>
  public sealed class ReqUpdateName : pb::IMessage {
    private static readonly pb::MessageParser<ReqUpdateName> _parser = new pb::MessageParser<ReqUpdateName>(() => new ReqUpdateName());
    public static pb::MessageParser<ReqUpdateName> Parser { get { return _parser; } }

    private string newName_ = "";
    /// <summary>
    ///用户更改姓名
    /// </summary>
    public string newName {
      get { return newName_; }
      set {
        newName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (newName.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(newName);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (newName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(newName);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            newName = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///响应
  /// </summary>
  public sealed class ResRoleBrief : pb::IMessage {
    private static readonly pb::MessageParser<ResRoleBrief> _parser = new pb::MessageParser<ResRoleBrief>(() => new ResRoleBrief());
    public static pb::MessageParser<ResRoleBrief> Parser { get { return _parser; } }

    private bool updateFlag_;
    /// <summary>
    ///成功传true
    /// </summary>
    public bool updateFlag {
      get { return updateFlag_; }
      set {
        updateFlag_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (updateFlag != false) {
        output.WriteRawTag(8);
        output.WriteBool(updateFlag);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (updateFlag != false) {
        size += 1 + 1;
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            updateFlag = input.ReadBool();
            break;
          }
        }
      }
    }

  }

  public sealed class PkValueChange : pb::IMessage {
    private static readonly pb::MessageParser<PkValueChange> _parser = new pb::MessageParser<PkValueChange>(() => new PkValueChange());
    public static pb::MessageParser<PkValueChange> Parser { get { return _parser; } }

    private long roleId_;
    public long roleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    private int pkValue_;
    /// <summary>
    ///pk值
    /// </summary>
    public int pkValue {
      get { return pkValue_; }
      set {
        pkValue_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (roleId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(roleId);
      }
      if (pkValue != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(pkValue);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (roleId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(roleId);
      }
      if (pkValue != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(pkValue);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            roleId = input.ReadInt64();
            break;
          }
          case 16: {
            pkValue = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class PkGreyNameState : pb::IMessage {
    private static readonly pb::MessageParser<PkGreyNameState> _parser = new pb::MessageParser<PkGreyNameState>(() => new PkGreyNameState());
    public static pb::MessageParser<PkGreyNameState> Parser { get { return _parser; } }

    private long roleId_;
    public long roleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    private int state_;
    /// <summary>
    ///1进入，2退出
    /// </summary>
    public int state {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (roleId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(roleId);
      }
      if (state != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(state);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (roleId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(roleId);
      }
      if (state != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(state);
      }
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            roleId = input.ReadInt64();
            break;
          }
          case 16: {
            state = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
