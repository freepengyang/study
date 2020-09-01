// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Rank.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace rank {

  #region Messages
  public sealed class RankInfo : pb::IMessage {
    private static readonly pb::MessageParser<RankInfo> _parser = new pb::MessageParser<RankInfo>(() => new RankInfo());
    public static pb::MessageParser<RankInfo> Parser { get { return _parser; } }

    private int type_;
    /// <summary>
    ///排行榜类型
    /// </summary>
    public int type {
      get { return type_; }
      set {
        type_ = value;
      }
    }

    private static readonly pb::FieldCodec<global::rank.RankData> _repeated_ranks_codec
        = pb::FieldCodec.ForMessage(18, global::rank.RankData.Parser);
    private readonly pbc::RepeatedField<global::rank.RankData> ranks_ = new pbc::RepeatedField<global::rank.RankData>();
    /// <summary>
    ///排行榜列表
    /// </summary>
    public pbc::RepeatedField<global::rank.RankData> ranks {
      get { return ranks_; }
    }

    private int myRank_;
    /// <summary>
    ///我的排名 排名从0开始  -1为未上榜
    /// </summary>
    public int myRank {
      get { return myRank_; }
      set {
        myRank_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (type != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(type);
      }
      ranks_.WriteTo(output, _repeated_ranks_codec);
      if (myRank != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(myRank);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (type != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(type);
      }
      size += ranks_.CalculateSize(_repeated_ranks_codec);
      if (myRank != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(myRank);
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
          case 18: {
            ranks_.AddEntriesFrom(input, _repeated_ranks_codec);
            break;
          }
          case 24: {
            myRank = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class RankData : pb::IMessage {
    private static readonly pb::MessageParser<RankData> _parser = new pb::MessageParser<RankData>(() => new RankData());
    public static pb::MessageParser<RankData> Parser { get { return _parser; } }

    private long rid_;
    /// <summary>
    ///玩家id
    /// </summary>
    public long rid {
      get { return rid_; }
      set {
        rid_ = value;
      }
    }

    private string name_ = "";
    /// <summary>
    ///玩家名字
    /// </summary>
    public string name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    private int value_;
    /// <summary>
    ///数值
    /// </summary>
    public int value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    private static readonly pb::FieldCodec<int> _repeated_paramList_codec
        = pb::FieldCodec.ForInt32(34);
    private readonly pbc::RepeatedField<int> paramList_ = new pbc::RepeatedField<int>();
    /// <summary>
    /// 其他参数
    /// 类型102，封印心法榜参数：衣服装备Id，武器装备Id，时装衣服Id，时装武器Id，时装称号Id，套装Id，性别
    /// </summary>
    public pbc::RepeatedField<int> paramList {
      get { return paramList_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (rid != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(rid);
      }
      if (name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(name);
      }
      if (value != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(value);
      }
      paramList_.WriteTo(output, _repeated_paramList_codec);
    }

    public int CalculateSize() {
      int size = 0;
      if (rid != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(rid);
      }
      if (name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(name);
      }
      if (value != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(value);
      }
      size += paramList_.CalculateSize(_repeated_paramList_codec);
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
            rid = input.ReadInt64();
            break;
          }
          case 18: {
            name = input.ReadString();
            break;
          }
          case 24: {
            value = input.ReadInt32();
            break;
          }
          case 34:
          case 32: {
            paramList_.AddEntriesFrom(input, _repeated_paramList_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class CSRankInfo : pb::IMessage {
    private static readonly pb::MessageParser<CSRankInfo> _parser = new pb::MessageParser<CSRankInfo>(() => new CSRankInfo());
    public static pb::MessageParser<CSRankInfo> Parser { get { return _parser; } }

    private int type_;
    /// <summary>
    ///排行榜类型
    /// </summary>
    public int type {
      get { return type_; }
      set {
        type_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (type != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(type);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (type != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(type);
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
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
