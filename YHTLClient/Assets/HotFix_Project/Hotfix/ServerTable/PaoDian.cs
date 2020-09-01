// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: PaoDian.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace paodian {

  #region Messages
  public sealed class UpgradePaoDian : pb::IMessage {
    private static readonly pb::MessageParser<UpgradePaoDian> _parser = new pb::MessageParser<UpgradePaoDian>(() => new UpgradePaoDian());
    public static pb::MessageParser<UpgradePaoDian> Parser { get { return _parser; } }

    public void WriteTo(pb::CodedOutputStream output) {
    }

    public int CalculateSize() {
      int size = 0;
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
        }
      }
    }

  }

  public sealed class PaoDianChange : pb::IMessage {
    private static readonly pb::MessageParser<PaoDianChange> _parser = new pb::MessageParser<PaoDianChange>(() => new PaoDianChange());
    public static pb::MessageParser<PaoDianChange> Parser { get { return _parser; } }

    private int rank_;
    /// <summary>
    ///阶
    /// </summary>
    public int rank {
      get { return rank_; }
      set {
        rank_ = value;
      }
    }

    private int star_;
    /// <summary>
    ///星
    /// </summary>
    public int star {
      get { return star_; }
      set {
        star_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (rank != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(rank);
      }
      if (star != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(star);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (rank != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(rank);
      }
      if (star != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(star);
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
            rank = input.ReadInt32();
            break;
          }
          case 16: {
            star = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class RandomPaoDian : pb::IMessage {
    private static readonly pb::MessageParser<RandomPaoDian> _parser = new pb::MessageParser<RandomPaoDian>(() => new RandomPaoDian());
    public static pb::MessageParser<RandomPaoDian> Parser { get { return _parser; } }

    private long nextRefreshTime_;
    /// <summary>
    ///下次刷新时间戳
    /// </summary>
    public long nextRefreshTime {
      get { return nextRefreshTime_; }
      set {
        nextRefreshTime_ = value;
      }
    }

    private static readonly pb::FieldCodec<global::paodian.RandomPaoDianData> _repeated_paoDianPoints_codec
        = pb::FieldCodec.ForMessage(18, global::paodian.RandomPaoDianData.Parser);
    private readonly pbc::RepeatedField<global::paodian.RandomPaoDianData> paoDianPoints_ = new pbc::RepeatedField<global::paodian.RandomPaoDianData>();
    /// <summary>
    ///泡点位置列表
    /// </summary>
    public pbc::RepeatedField<global::paodian.RandomPaoDianData> paoDianPoints {
      get { return paoDianPoints_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (nextRefreshTime != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(nextRefreshTime);
      }
      paoDianPoints_.WriteTo(output, _repeated_paoDianPoints_codec);
    }

    public int CalculateSize() {
      int size = 0;
      if (nextRefreshTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(nextRefreshTime);
      }
      size += paoDianPoints_.CalculateSize(_repeated_paoDianPoints_codec);
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
            nextRefreshTime = input.ReadInt64();
            break;
          }
          case 18: {
            paoDianPoints_.AddEntriesFrom(input, _repeated_paoDianPoints_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class RandomPaoDianData : pb::IMessage {
    private static readonly pb::MessageParser<RandomPaoDianData> _parser = new pb::MessageParser<RandomPaoDianData>(() => new RandomPaoDianData());
    public static pb::MessageParser<RandomPaoDianData> Parser { get { return _parser; } }

    private int x_;
    /// <summary>
    ///x
    /// </summary>
    public int x {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    private int y_;
    /// <summary>
    ///y
    /// </summary>
    public int y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    private int configId_;
    /// <summary>
    ///泡点表id
    /// </summary>
    public int configId {
      get { return configId_; }
      set {
        configId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (x != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(x);
      }
      if (y != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(y);
      }
      if (configId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(configId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (x != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(x);
      }
      if (y != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(y);
      }
      if (configId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(configId);
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
            x = input.ReadInt32();
            break;
          }
          case 16: {
            y = input.ReadInt32();
            break;
          }
          case 24: {
            configId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class PaoDianExp : pb::IMessage {
    private static readonly pb::MessageParser<PaoDianExp> _parser = new pb::MessageParser<PaoDianExp>(() => new PaoDianExp());
    public static pb::MessageParser<PaoDianExp> Parser { get { return _parser; } }

    private long exp_;
    /// <summary>
    ///经验
    /// </summary>
    public long exp {
      get { return exp_; }
      set {
        exp_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (exp != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(exp);
      }
    }

    public int CalculateSize() {
      int size = 0;
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
            exp = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
