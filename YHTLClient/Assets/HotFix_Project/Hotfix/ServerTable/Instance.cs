// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Instance.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace instance {

  #region Messages
  public sealed class InstanceIdMsg : pb::IMessage {
    private static readonly pb::MessageParser<InstanceIdMsg> _parser = new pb::MessageParser<InstanceIdMsg>(() => new InstanceIdMsg());
    public static pb::MessageParser<InstanceIdMsg> Parser { get { return _parser; } }

    private int instanceId_;
    public int instanceId {
      get { return instanceId_; }
      set {
        instanceId_ = value;
      }
    }

    private bool enter_;
    /// <summary>
    ///帮会禁地是否进入, true为进入，false为创建
    /// </summary>
    public bool enter {
      get { return enter_; }
      set {
        enter_ = value;
      }
    }

    private long ownerId_;
    /// <summary>
    ///帮会禁地副本主人ID
    /// </summary>
    public long ownerId {
      get { return ownerId_; }
      set {
        ownerId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (instanceId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(instanceId);
      }
      if (enter != false) {
        output.WriteRawTag(16);
        output.WriteBool(enter);
      }
      if (ownerId != 0L) {
        output.WriteRawTag(24);
        output.WriteInt64(ownerId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (instanceId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(instanceId);
      }
      if (enter != false) {
        size += 1 + 1;
      }
      if (ownerId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(ownerId);
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
            instanceId = input.ReadInt32();
            break;
          }
          case 16: {
            enter = input.ReadBool();
            break;
          }
          case 24: {
            ownerId = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///领取副本奖励
  /// </summary>
  public sealed class GetInstanceRewardRequest : pb::IMessage {
    private static readonly pb::MessageParser<GetInstanceRewardRequest> _parser = new pb::MessageParser<GetInstanceRewardRequest>(() => new GetInstanceRewardRequest());
    public static pb::MessageParser<GetInstanceRewardRequest> Parser { get { return _parser; } }

    private int count_;
    /// <summary>
    ///领取倍数
    /// </summary>
    public int count {
      get { return count_; }
      set {
        count_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (count != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(count);
      }
    }

    public int CalculateSize() {
      int size = 0;
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
            count = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class QuickInstanceRewardMsg : pb::IMessage {
    private static readonly pb::MessageParser<QuickInstanceRewardMsg> _parser = new pb::MessageParser<QuickInstanceRewardMsg>(() => new QuickInstanceRewardMsg());
    public static pb::MessageParser<QuickInstanceRewardMsg> Parser { get { return _parser; } }

    private int instanceId_;
    public int instanceId {
      get { return instanceId_; }
      set {
        instanceId_ = value;
      }
    }

    private int subType_;
    public int subType {
      get { return subType_; }
      set {
        subType_ = value;
      }
    }

    private int count_;
    /// <summary>
    ///领取倍数
    /// </summary>
    public int count {
      get { return count_; }
      set {
        count_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (instanceId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(instanceId);
      }
      if (subType != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(subType);
      }
      if (count != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(count);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (instanceId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(instanceId);
      }
      if (subType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(subType);
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
            instanceId = input.ReadInt32();
            break;
          }
          case 16: {
            subType = input.ReadInt32();
            break;
          }
          case 24: {
            count = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class QuickInstanceInfoMsg : pb::IMessage {
    private static readonly pb::MessageParser<QuickInstanceInfoMsg> _parser = new pb::MessageParser<QuickInstanceInfoMsg>(() => new QuickInstanceInfoMsg());
    public static pb::MessageParser<QuickInstanceInfoMsg> Parser { get { return _parser; } }

    private int instanceId_;
    public int instanceId {
      get { return instanceId_; }
      set {
        instanceId_ = value;
      }
    }

    private int subType_;
    public int subType {
      get { return subType_; }
      set {
        subType_ = value;
      }
    }

    private bool success_;
    public bool success {
      get { return success_; }
      set {
        success_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (instanceId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(instanceId);
      }
      if (subType != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(subType);
      }
      if (success != false) {
        output.WriteRawTag(24);
        output.WriteBool(success);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (instanceId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(instanceId);
      }
      if (subType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(subType);
      }
      if (success != false) {
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
            instanceId = input.ReadInt32();
            break;
          }
          case 16: {
            subType = input.ReadInt32();
            break;
          }
          case 24: {
            success = input.ReadBool();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///离开副本请求
  /// </summary>
  public sealed class LeaveInstanceRequest : pb::IMessage {
    private static readonly pb::MessageParser<LeaveInstanceRequest> _parser = new pb::MessageParser<LeaveInstanceRequest>(() => new LeaveInstanceRequest());
    public static pb::MessageParser<LeaveInstanceRequest> Parser { get { return _parser; } }

    private bool isBackCity_;
    /// <summary>
    ///是否回城,选false默认按配置里的操作进行
    /// </summary>
    public bool isBackCity {
      get { return isBackCity_; }
      set {
        isBackCity_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (isBackCity != false) {
        output.WriteRawTag(8);
        output.WriteBool(isBackCity);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (isBackCity != false) {
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
            isBackCity = input.ReadBool();
            break;
          }
        }
      }
    }

  }

  public sealed class InstanceInfo : pb::IMessage {
    private static readonly pb::MessageParser<InstanceInfo> _parser = new pb::MessageParser<InstanceInfo>(() => new InstanceInfo());
    public static pb::MessageParser<InstanceInfo> Parser { get { return _parser; } }

    private long startTime_;
    /// <summary>
    ///副本开始时间
    /// </summary>
    public long startTime {
      get { return startTime_; }
      set {
        startTime_ = value;
      }
    }

    private int killedBoss_;
    /// <summary>
    ///杀boss数
    /// </summary>
    public int killedBoss {
      get { return killedBoss_; }
      set {
        killedBoss_ = value;
      }
    }

    private int killedMonsters_;
    /// <summary>
    ///杀怪数
    /// </summary>
    public int killedMonsters {
      get { return killedMonsters_; }
      set {
        killedMonsters_ = value;
      }
    }

    private int finishCount_;
    /// <summary>
    ///大于0表示副本关闭倒计时
    /// </summary>
    public int finishCount {
      get { return finishCount_; }
      set {
        finishCount_ = value;
      }
    }

    private bool rewarded_;
    /// <summary>
    ///奖励是否已领
    /// </summary>
    public bool rewarded {
      get { return rewarded_; }
      set {
        rewarded_ = value;
      }
    }

    private bool success_;
    /// <summary>
    ///挑战是否成功
    /// </summary>
    public bool success {
      get { return success_; }
      set {
        success_ = value;
      }
    }

    private long usedTime_;
    /// <summary>
    ///副本已经过去的时间;
    /// </summary>
    public long usedTime {
      get { return usedTime_; }
      set {
        usedTime_ = value;
      }
    }

    private int instanceId_;
    public int instanceId {
      get { return instanceId_; }
      set {
        instanceId_ = value;
      }
    }

    private int state_;
    /// <summary>
    ///副本状态;0:create;1:wait;2:started;3:finished;4:closed;
    /// </summary>
    public int state {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    private long endTime_;
    /// <summary>
    ///结束时间戳;
    /// </summary>
    public long endTime {
      get { return endTime_; }
      set {
        endTime_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (startTime != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(startTime);
      }
      if (killedBoss != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(killedBoss);
      }
      if (killedMonsters != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(killedMonsters);
      }
      if (finishCount != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(finishCount);
      }
      if (rewarded != false) {
        output.WriteRawTag(40);
        output.WriteBool(rewarded);
      }
      if (success != false) {
        output.WriteRawTag(48);
        output.WriteBool(success);
      }
      if (usedTime != 0L) {
        output.WriteRawTag(56);
        output.WriteInt64(usedTime);
      }
      if (instanceId != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(instanceId);
      }
      if (state != 0) {
        output.WriteRawTag(72);
        output.WriteInt32(state);
      }
      if (endTime != 0L) {
        output.WriteRawTag(80);
        output.WriteInt64(endTime);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (startTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(startTime);
      }
      if (killedBoss != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(killedBoss);
      }
      if (killedMonsters != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(killedMonsters);
      }
      if (finishCount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(finishCount);
      }
      if (rewarded != false) {
        size += 1 + 1;
      }
      if (success != false) {
        size += 1 + 1;
      }
      if (usedTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(usedTime);
      }
      if (instanceId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(instanceId);
      }
      if (state != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(state);
      }
      if (endTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(endTime);
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
            startTime = input.ReadInt64();
            break;
          }
          case 16: {
            killedBoss = input.ReadInt32();
            break;
          }
          case 24: {
            killedMonsters = input.ReadInt32();
            break;
          }
          case 32: {
            finishCount = input.ReadInt32();
            break;
          }
          case 40: {
            rewarded = input.ReadBool();
            break;
          }
          case 48: {
            success = input.ReadBool();
            break;
          }
          case 56: {
            usedTime = input.ReadInt64();
            break;
          }
          case 64: {
            instanceId = input.ReadInt32();
            break;
          }
          case 72: {
            state = input.ReadInt32();
            break;
          }
          case 80: {
            endTime = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  public sealed class OneInstanceCount : pb::IMessage {
    private static readonly pb::MessageParser<OneInstanceCount> _parser = new pb::MessageParser<OneInstanceCount>(() => new OneInstanceCount());
    public static pb::MessageParser<OneInstanceCount> Parser { get { return _parser; } }

    private int groupId_;
    /// <summary>
    ///副本类型
    /// </summary>
    public int groupId {
      get { return groupId_; }
      set {
        groupId_ = value;
      }
    }

    private int leftCount_;
    /// <summary>
    ///今日剩余挑战次数
    /// </summary>
    public int leftCount {
      get { return leftCount_; }
      set {
        leftCount_ = value;
      }
    }

    private int totalTimes_;
    /// <summary>
    ///总挑战次数
    /// </summary>
    public int totalTimes {
      get { return totalTimes_; }
      set {
        totalTimes_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (groupId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(groupId);
      }
      if (leftCount != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(leftCount);
      }
      if (totalTimes != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(totalTimes);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (groupId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(groupId);
      }
      if (leftCount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(leftCount);
      }
      if (totalTimes != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(totalTimes);
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
            leftCount = input.ReadInt32();
            break;
          }
          case 24: {
            totalTimes = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class InstanceCount : pb::IMessage {
    private static readonly pb::MessageParser<InstanceCount> _parser = new pb::MessageParser<InstanceCount>(() => new InstanceCount());
    public static pb::MessageParser<InstanceCount> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::instance.OneInstanceCount> _repeated_countList_codec
        = pb::FieldCodec.ForMessage(10, global::instance.OneInstanceCount.Parser);
    private readonly pbc::RepeatedField<global::instance.OneInstanceCount> countList_ = new pbc::RepeatedField<global::instance.OneInstanceCount>();
    /// <summary>
    ///副本数量列表
    /// </summary>
    public pbc::RepeatedField<global::instance.OneInstanceCount> countList {
      get { return countList_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      countList_.WriteTo(output, _repeated_countList_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += countList_.CalculateSize(_repeated_countList_codec);
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
            countList_.AddEntriesFrom(input, _repeated_countList_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class DiLaoInfo : pb::IMessage {
    private static readonly pb::MessageParser<DiLaoInfo> _parser = new pb::MessageParser<DiLaoInfo>(() => new DiLaoInfo());
    public static pb::MessageParser<DiLaoInfo> Parser { get { return _parser; } }

    private int wave_;
    /// <summary>
    ///波数
    /// </summary>
    public int wave {
      get { return wave_; }
      set {
        wave_ = value;
      }
    }

    private int score_;
    /// <summary>
    ///积分
    /// </summary>
    public int score {
      get { return score_; }
      set {
        score_ = value;
      }
    }

    private int skillId_;
    /// <summary>
    ///鼓舞技能
    /// </summary>
    public int skillId {
      get { return skillId_; }
      set {
        skillId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (wave != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(wave);
      }
      if (score != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(score);
      }
      if (skillId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(skillId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (wave != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(wave);
      }
      if (score != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(score);
      }
      if (skillId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(skillId);
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
            wave = input.ReadInt32();
            break;
          }
          case 16: {
            score = input.ReadInt32();
            break;
          }
          case 24: {
            skillId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class UndergroundTreasureInstanceInfo : pb::IMessage {
    private static readonly pb::MessageParser<UndergroundTreasureInstanceInfo> _parser = new pb::MessageParser<UndergroundTreasureInstanceInfo>(() => new UndergroundTreasureInstanceInfo());
    public static pb::MessageParser<UndergroundTreasureInstanceInfo> Parser { get { return _parser; } }

    private long nextFreshTime_;
    /// <summary>
    ///下次刷新时间戳;
    /// </summary>
    public long nextFreshTime {
      get { return nextFreshTime_; }
      set {
        nextFreshTime_ = value;
      }
    }

    private int state_;
    /// <summary>
    ///1 正在运行，2：结束;
    /// </summary>
    public int state {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (nextFreshTime != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(nextFreshTime);
      }
      if (state != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(state);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (nextFreshTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(nextFreshTime);
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
            nextFreshTime = input.ReadInt64();
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

  public sealed class BossChallengeInfo : pb::IMessage {
    private static readonly pb::MessageParser<BossChallengeInfo> _parser = new pb::MessageParser<BossChallengeInfo>(() => new BossChallengeInfo());
    public static pb::MessageParser<BossChallengeInfo> Parser { get { return _parser; } }

    private int group_;
    /// <summary>
    ///1为新手组2为进阶组3为大师组
    /// </summary>
    public int group {
      get { return group_; }
      set {
        group_ = value;
      }
    }

    private int pass_;
    /// <summary>
    ///已通关的关卡
    /// </summary>
    public int pass {
      get { return pass_; }
      set {
        pass_ = value;
      }
    }

    private long surplusTime_;
    /// <summary>
    ///赛季剩余时间
    /// </summary>
    public long surplusTime {
      get { return surplusTime_; }
      set {
        surplusTime_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (group != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(group);
      }
      if (pass != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(pass);
      }
      if (surplusTime != 0L) {
        output.WriteRawTag(24);
        output.WriteInt64(surplusTime);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (group != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(group);
      }
      if (pass != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(pass);
      }
      if (surplusTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(surplusTime);
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
            group = input.ReadInt32();
            break;
          }
          case 16: {
            pass = input.ReadInt32();
            break;
          }
          case 24: {
            surplusTime = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///每日地图、小月卡地图、大月卡地图击杀怪物限制，响应
  /// </summary>
  public sealed class ResDropLimit : pb::IMessage {
    private static readonly pb::MessageParser<ResDropLimit> _parser = new pb::MessageParser<ResDropLimit>(() => new ResDropLimit());
    public static pb::MessageParser<ResDropLimit> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::instance.MonsterNumInfo> _repeated_monsterNumInfo_codec
        = pb::FieldCodec.ForMessage(10, global::instance.MonsterNumInfo.Parser);
    private readonly pbc::RepeatedField<global::instance.MonsterNumInfo> monsterNumInfo_ = new pbc::RepeatedField<global::instance.MonsterNumInfo>();
    /// <summary>
    ///size = 1为boss数量，size = 2为小怪数量
    /// </summary>
    public pbc::RepeatedField<global::instance.MonsterNumInfo> monsterNumInfo {
      get { return monsterNumInfo_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      monsterNumInfo_.WriteTo(output, _repeated_monsterNumInfo_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += monsterNumInfo_.CalculateSize(_repeated_monsterNumInfo_codec);
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
            monsterNumInfo_.AddEntriesFrom(input, _repeated_monsterNumInfo_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class MonsterNumInfo : pb::IMessage {
    private static readonly pb::MessageParser<MonsterNumInfo> _parser = new pb::MessageParser<MonsterNumInfo>(() => new MonsterNumInfo());
    public static pb::MessageParser<MonsterNumInfo> Parser { get { return _parser; } }

    private int monsterNum_;
    /// <summary>
    /// 击杀的怪物的数量
    /// </summary>
    public int monsterNum {
      get { return monsterNum_; }
      set {
        monsterNum_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (monsterNum != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(monsterNum);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (monsterNum != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(monsterNum);
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
            monsterNum = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
