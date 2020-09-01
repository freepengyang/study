// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Boss.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace boss {

  #region Messages
  /// <summary>
  ///可挑战boss信息
  /// </summary>
  public sealed class ChallengeBossInfo : pb::IMessage {
    private static readonly pb::MessageParser<ChallengeBossInfo> _parser = new pb::MessageParser<ChallengeBossInfo>(() => new ChallengeBossInfo());
    public static pb::MessageParser<ChallengeBossInfo> Parser { get { return _parser; } }

    private int id_;
    /// <summary>
    ///配置表id
    /// </summary>
    public int id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    private long refreshTime_;
    /// <summary>
    ///刷新时间
    /// </summary>
    public long refreshTime {
      get { return refreshTime_; }
      set {
        refreshTime_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (id != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(id);
      }
      if (refreshTime != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(refreshTime);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(id);
      }
      if (refreshTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(refreshTime);
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
            refreshTime = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///可击败boss列表
  /// </summary>
  public sealed class ChallengeBossInfoResponse : pb::IMessage {
    private static readonly pb::MessageParser<ChallengeBossInfoResponse> _parser = new pb::MessageParser<ChallengeBossInfoResponse>(() => new ChallengeBossInfoResponse());
    public static pb::MessageParser<ChallengeBossInfoResponse> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::boss.ChallengeBossInfo> _repeated_challengeBossInfo_codec
        = pb::FieldCodec.ForMessage(10, global::boss.ChallengeBossInfo.Parser);
    private readonly pbc::RepeatedField<global::boss.ChallengeBossInfo> challengeBossInfo_ = new pbc::RepeatedField<global::boss.ChallengeBossInfo>();
    public pbc::RepeatedField<global::boss.ChallengeBossInfo> challengeBossInfo {
      get { return challengeBossInfo_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      challengeBossInfo_.WriteTo(output, _repeated_challengeBossInfo_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += challengeBossInfo_.CalculateSize(_repeated_challengeBossInfo_codec);
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
            challengeBossInfo_.AddEntriesFrom(input, _repeated_challengeBossInfo_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code