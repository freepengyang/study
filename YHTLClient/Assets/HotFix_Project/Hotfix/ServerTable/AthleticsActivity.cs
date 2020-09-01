// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: AthleticsActivity.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace athleticsactivity {

  #region Messages
  public sealed class AthleticsActivityInfoResponse : pb::IMessage {
    private static readonly pb::MessageParser<AthleticsActivityInfoResponse> _parser = new pb::MessageParser<AthleticsActivityInfoResponse>(() => new AthleticsActivityInfoResponse());
    public static pb::MessageParser<AthleticsActivityInfoResponse> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::athleticsactivity.AthleticsActivityInfo> _repeated_athleticsActivityInfos_codec
        = pb::FieldCodec.ForMessage(10, global::athleticsactivity.AthleticsActivityInfo.Parser);
    private readonly pbc::RepeatedField<global::athleticsactivity.AthleticsActivityInfo> athleticsActivityInfos_ = new pbc::RepeatedField<global::athleticsactivity.AthleticsActivityInfo>();
    public pbc::RepeatedField<global::athleticsactivity.AthleticsActivityInfo> athleticsActivityInfos {
      get { return athleticsActivityInfos_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      athleticsActivityInfos_.WriteTo(output, _repeated_athleticsActivityInfos_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += athleticsActivityInfos_.CalculateSize(_repeated_athleticsActivityInfos_codec);
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
            athleticsActivityInfos_.AddEntriesFrom(input, _repeated_athleticsActivityInfos_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///竞技活动信息
  /// </summary>
  public sealed class AthleticsActivityInfo : pb::IMessage {
    private static readonly pb::MessageParser<AthleticsActivityInfo> _parser = new pb::MessageParser<AthleticsActivityInfo>(() => new AthleticsActivityInfo());
    public static pb::MessageParser<AthleticsActivityInfo> Parser { get { return _parser; } }

    private int id_;
    /// <summary>
    ///活动id
    /// </summary>
    public int id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    private long endTime_;
    /// <summary>
    ///结束时间
    /// </summary>
    public long endTime {
      get { return endTime_; }
      set {
        endTime_ = value;
      }
    }

    private static readonly pb::FieldCodec<global::athleticsactivity.ActivityRewardInfo> _repeated_activityRewardInfos_codec
        = pb::FieldCodec.ForMessage(26, global::athleticsactivity.ActivityRewardInfo.Parser);
    private readonly pbc::RepeatedField<global::athleticsactivity.ActivityRewardInfo> activityRewardInfos_ = new pbc::RepeatedField<global::athleticsactivity.ActivityRewardInfo>();
    public pbc::RepeatedField<global::athleticsactivity.ActivityRewardInfo> activityRewardInfos {
      get { return activityRewardInfos_; }
    }

    private int state_;
    /// <summary>
    ///状态：0进行中，1未开启，2已结束
    /// </summary>
    public int state {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (id != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(id);
      }
      if (endTime != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(endTime);
      }
      activityRewardInfos_.WriteTo(output, _repeated_activityRewardInfos_codec);
      if (state != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(state);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(id);
      }
      if (endTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(endTime);
      }
      size += activityRewardInfos_.CalculateSize(_repeated_activityRewardInfos_codec);
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
            id = input.ReadInt32();
            break;
          }
          case 16: {
            endTime = input.ReadInt64();
            break;
          }
          case 26: {
            activityRewardInfos_.AddEntriesFrom(input, _repeated_activityRewardInfos_codec);
            break;
          }
          case 32: {
            state = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///竞技活动奖励信息
  /// </summary>
  public sealed class ActivityRewardInfo : pb::IMessage {
    private static readonly pb::MessageParser<ActivityRewardInfo> _parser = new pb::MessageParser<ActivityRewardInfo>(() => new ActivityRewardInfo());
    public static pb::MessageParser<ActivityRewardInfo> Parser { get { return _parser; } }

    private int id_;
    /// <summary>
    ///活动奖励表id
    /// </summary>
    public int id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    private int curProcess_;
    /// <summary>
    ///当前进度
    /// </summary>
    public int curProcess {
      get { return curProcess_; }
      set {
        curProcess_ = value;
      }
    }

    private int surplusNum_;
    /// <summary>
    ///剩余数量
    /// </summary>
    public int surplusNum {
      get { return surplusNum_; }
      set {
        surplusNum_ = value;
      }
    }

    private int state_;
    /// <summary>
    ///状态：0进行中，1可领取，2已完成
    /// </summary>
    public int state {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (id != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(id);
      }
      if (curProcess != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(curProcess);
      }
      if (surplusNum != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(surplusNum);
      }
      if (state != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(state);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(id);
      }
      if (curProcess != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(curProcess);
      }
      if (surplusNum != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(surplusNum);
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
            id = input.ReadInt32();
            break;
          }
          case 16: {
            curProcess = input.ReadInt32();
            break;
          }
          case 24: {
            surplusNum = input.ReadInt32();
            break;
          }
          case 32: {
            state = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///领取奖励
  /// </summary>
  public sealed class ReceiveAthleticsActivityRewardRequest : pb::IMessage {
    private static readonly pb::MessageParser<ReceiveAthleticsActivityRewardRequest> _parser = new pb::MessageParser<ReceiveAthleticsActivityRewardRequest>(() => new ReceiveAthleticsActivityRewardRequest());
    public static pb::MessageParser<ReceiveAthleticsActivityRewardRequest> Parser { get { return _parser; } }

    private int id_;
    /// <summary>
    ///活动奖励表id
    /// </summary>
    public int id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (id != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(id);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(id);
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
            id = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class ActivityRewardInfoChange : pb::IMessage {
    private static readonly pb::MessageParser<ActivityRewardInfoChange> _parser = new pb::MessageParser<ActivityRewardInfoChange>(() => new ActivityRewardInfoChange());
    public static pb::MessageParser<ActivityRewardInfoChange> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::athleticsactivity.ActivityRewardInfo> _repeated_activityRewardInfos_codec
        = pb::FieldCodec.ForMessage(10, global::athleticsactivity.ActivityRewardInfo.Parser);
    private readonly pbc::RepeatedField<global::athleticsactivity.ActivityRewardInfo> activityRewardInfos_ = new pbc::RepeatedField<global::athleticsactivity.ActivityRewardInfo>();
    public pbc::RepeatedField<global::athleticsactivity.ActivityRewardInfo> activityRewardInfos {
      get { return activityRewardInfos_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      activityRewardInfos_.WriteTo(output, _repeated_activityRewardInfos_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += activityRewardInfos_.CalculateSize(_repeated_activityRewardInfos_codec);
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
            activityRewardInfos_.AddEntriesFrom(input, _repeated_activityRewardInfos_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code