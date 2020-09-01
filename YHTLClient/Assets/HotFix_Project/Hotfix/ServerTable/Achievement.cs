// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Achievement.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace achievement {

  #region Messages
  /// <summary>
  ///成就信息
  /// </summary>
  public sealed class AchievementInfo : pb::IMessage {
    private static readonly pb::MessageParser<AchievementInfo> _parser = new pb::MessageParser<AchievementInfo>(() => new AchievementInfo());
    public static pb::MessageParser<AchievementInfo> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<long> _repeated_achievementData_codec
        = pb::FieldCodec.ForInt64(10);
    private readonly pbc::RepeatedField<long> achievementData_ = new pbc::RepeatedField<long>();
    /// <summary>
    ///高16位为成就id，在16位是是否领取 后32为是进度
    /// </summary>
    public pbc::RepeatedField<long> achievementData {
      get { return achievementData_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      achievementData_.WriteTo(output, _repeated_achievementData_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += achievementData_.CalculateSize(_repeated_achievementData_codec);
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
            achievementData_.AddEntriesFrom(input, _repeated_achievementData_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///领取成就奖励请求
  /// </summary>
  public sealed class GetAchievementRewardRequest : pb::IMessage {
    private static readonly pb::MessageParser<GetAchievementRewardRequest> _parser = new pb::MessageParser<GetAchievementRewardRequest>(() => new GetAchievementRewardRequest());
    public static pb::MessageParser<GetAchievementRewardRequest> Parser { get { return _parser; } }

    private int achievementId_;
    /// <summary>
    ///成就的id
    /// </summary>
    public int achievementId {
      get { return achievementId_; }
      set {
        achievementId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (achievementId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(achievementId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (achievementId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(achievementId);
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
            achievementId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///一键领取成就奖励请求
  /// </summary>
  public sealed class GetAutoAchievementRewardRequest : pb::IMessage {
    private static readonly pb::MessageParser<GetAutoAchievementRewardRequest> _parser = new pb::MessageParser<GetAutoAchievementRewardRequest>(() => new GetAutoAchievementRewardRequest());
    public static pb::MessageParser<GetAutoAchievementRewardRequest> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<int> _repeated_achievementIds_codec
        = pb::FieldCodec.ForInt32(18);
    private readonly pbc::RepeatedField<int> achievementIds_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///成就id组
    /// </summary>
    public pbc::RepeatedField<int> achievementIds {
      get { return achievementIds_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      achievementIds_.WriteTo(output, _repeated_achievementIds_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += achievementIds_.CalculateSize(_repeated_achievementIds_codec);
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 18:
          case 16: {
            achievementIds_.AddEntriesFrom(input, _repeated_achievementIds_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///领取成就奖励响应
  /// </summary>
  public sealed class GetAchievementRewardResponse : pb::IMessage {
    private static readonly pb::MessageParser<GetAchievementRewardResponse> _parser = new pb::MessageParser<GetAchievementRewardResponse>(() => new GetAchievementRewardResponse());
    public static pb::MessageParser<GetAchievementRewardResponse> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::bag.BagItemInfo> _repeated_rewardItem_codec
        = pb::FieldCodec.ForMessage(10, global::bag.BagItemInfo.Parser);
    private readonly pbc::RepeatedField<global::bag.BagItemInfo> rewardItem_ = new pbc::RepeatedField<global::bag.BagItemInfo>();
    /// <summary>
    ///物品信息
    /// </summary>
    public pbc::RepeatedField<global::bag.BagItemInfo> rewardItem {
      get { return rewardItem_; }
    }

    private int achievementNumber_;
    /// <summary>
    ///成就点数
    /// </summary>
    public int achievementNumber {
      get { return achievementNumber_; }
      set {
        achievementNumber_ = value;
      }
    }

    private static readonly pb::FieldCodec<int> _repeated_achievementId_codec
        = pb::FieldCodec.ForInt32(26);
    private readonly pbc::RepeatedField<int> achievementId_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///成就的id
    /// </summary>
    public pbc::RepeatedField<int> achievementId {
      get { return achievementId_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      rewardItem_.WriteTo(output, _repeated_rewardItem_codec);
      if (achievementNumber != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(achievementNumber);
      }
      achievementId_.WriteTo(output, _repeated_achievementId_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += rewardItem_.CalculateSize(_repeated_rewardItem_codec);
      if (achievementNumber != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(achievementNumber);
      }
      size += achievementId_.CalculateSize(_repeated_achievementId_codec);
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
            rewardItem_.AddEntriesFrom(input, _repeated_rewardItem_codec);
            break;
          }
          case 16: {
            achievementNumber = input.ReadInt32();
            break;
          }
          case 26:
          case 24: {
            achievementId_.AddEntriesFrom(input, _repeated_achievementId_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///勋章升级请求
  /// </summary>
  public sealed class MedalUpgradeRequest : pb::IMessage {
    private static readonly pb::MessageParser<MedalUpgradeRequest> _parser = new pb::MessageParser<MedalUpgradeRequest>(() => new MedalUpgradeRequest());
    public static pb::MessageParser<MedalUpgradeRequest> Parser { get { return _parser; } }

    private int currentId_;
    /// <summary>
    ///当前的configId
    /// </summary>
    public int currentId {
      get { return currentId_; }
      set {
        currentId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (currentId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(currentId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (currentId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(currentId);
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
            currentId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///勋章升级响应
  /// </summary>
  public sealed class MedalUpgradeResponse : pb::IMessage {
    private static readonly pb::MessageParser<MedalUpgradeResponse> _parser = new pb::MessageParser<MedalUpgradeResponse>(() => new MedalUpgradeResponse());
    public static pb::MessageParser<MedalUpgradeResponse> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::bag.BagItemInfo> _repeated_changedItems_codec
        = pb::FieldCodec.ForMessage(10, global::bag.BagItemInfo.Parser);
    private readonly pbc::RepeatedField<global::bag.BagItemInfo> changedItems_ = new pbc::RepeatedField<global::bag.BagItemInfo>();
    public pbc::RepeatedField<global::bag.BagItemInfo> changedItems {
      get { return changedItems_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      changedItems_.WriteTo(output, _repeated_changedItems_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += changedItems_.CalculateSize(_repeated_changedItems_codec);
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
            changedItems_.AddEntriesFrom(input, _repeated_changedItems_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code