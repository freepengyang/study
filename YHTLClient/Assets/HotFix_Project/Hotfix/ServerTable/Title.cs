// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Title.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace title {

  #region Messages
  /// <summary>
  ///全部称号信息
  /// </summary>
  public sealed class TitleMapInfo : pb::IMessage {
    private static readonly pb::MessageParser<TitleMapInfo> _parser = new pb::MessageParser<TitleMapInfo>(() => new TitleMapInfo());
    public static pb::MessageParser<TitleMapInfo> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::title.TitleInfo> _repeated_info_codec
        = pb::FieldCodec.ForMessage(10, global::title.TitleInfo.Parser);
    private readonly pbc::RepeatedField<global::title.TitleInfo> info_ = new pbc::RepeatedField<global::title.TitleInfo>();
    /// <summary>
    ///称号信息
    /// </summary>
    public pbc::RepeatedField<global::title.TitleInfo> info {
      get { return info_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      info_.WriteTo(output, _repeated_info_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += info_.CalculateSize(_repeated_info_codec);
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
            info_.AddEntriesFrom(input, _repeated_info_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///称号
  /// </summary>
  public sealed class TitleInfo : pb::IMessage {
    private static readonly pb::MessageParser<TitleInfo> _parser = new pb::MessageParser<TitleInfo>(() => new TitleInfo());
    public static pb::MessageParser<TitleInfo> Parser { get { return _parser; } }

    private int titleId_;
    /// <summary>
    ///称号id 
    /// </summary>
    public int TitleId {
      get { return titleId_; }
      set {
        titleId_ = value;
      }
    }

    private long getTime_;
    /// <summary>
    ///获得的时间
    /// </summary>
    public long getTime {
      get { return getTime_; }
      set {
        getTime_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (TitleId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(TitleId);
      }
      if (getTime != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(getTime);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (TitleId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(TitleId);
      }
      if (getTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(getTime);
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
            TitleId = input.ReadInt32();
            break;
          }
          case 16: {
            getTime = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///取消、装备称号
  /// </summary>
  public sealed class ChangeTitle : pb::IMessage {
    private static readonly pb::MessageParser<ChangeTitle> _parser = new pb::MessageParser<ChangeTitle>(() => new ChangeTitle());
    public static pb::MessageParser<ChangeTitle> Parser { get { return _parser; } }

    private int titleId_;
    /// <summary>
    ///称号id  
    /// </summary>
    public int TitleId {
      get { return titleId_; }
      set {
        titleId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (TitleId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(TitleId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (TitleId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(TitleId);
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
            TitleId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code