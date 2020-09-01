// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Fashion.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace fashion {

  #region Messages
  public sealed class FashionId : pb::IMessage {
    private static readonly pb::MessageParser<FashionId> _parser = new pb::MessageParser<FashionId>(() => new FashionId());
    public static pb::MessageParser<FashionId> Parser { get { return _parser; } }

    private int id_;
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

  /// <summary>
  ///时装信息;
  /// </summary>
  public sealed class FashionInfo : pb::IMessage {
    private static readonly pb::MessageParser<FashionInfo> _parser = new pb::MessageParser<FashionInfo>(() => new FashionInfo());
    public static pb::MessageParser<FashionInfo> Parser { get { return _parser; } }

    private int fashionId_;
    public int fashionId {
      get { return fashionId_; }
      set {
        fashionId_ = value;
      }
    }

    private long timeLimit_;
    /// <summary>
    ///生效时间，0为永久;
    /// </summary>
    public long timeLimit {
      get { return timeLimit_; }
      set {
        timeLimit_ = value;
      }
    }

    private int star_;
    public int star {
      get { return star_; }
      set {
        star_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (fashionId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(fashionId);
      }
      if (timeLimit != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(timeLimit);
      }
      if (star != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(star);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (fashionId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(fashionId);
      }
      if (timeLimit != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(timeLimit);
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
            fashionId = input.ReadInt32();
            break;
          }
          case 16: {
            timeLimit = input.ReadInt64();
            break;
          }
          case 24: {
            star = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class AllFashionInfo : pb::IMessage {
    private static readonly pb::MessageParser<AllFashionInfo> _parser = new pb::MessageParser<AllFashionInfo>(() => new AllFashionInfo());
    public static pb::MessageParser<AllFashionInfo> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::fashion.FashionInfo> _repeated_fashions_codec
        = pb::FieldCodec.ForMessage(10, global::fashion.FashionInfo.Parser);
    private readonly pbc::RepeatedField<global::fashion.FashionInfo> fashions_ = new pbc::RepeatedField<global::fashion.FashionInfo>();
    public pbc::RepeatedField<global::fashion.FashionInfo> fashions {
      get { return fashions_; }
    }

    private static readonly pb::FieldCodec<int> _repeated_fashionIds_codec
        = pb::FieldCodec.ForInt32(18);
    private readonly pbc::RepeatedField<int> fashionIds_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///装备的时装;
    /// </summary>
    public pbc::RepeatedField<int> fashionIds {
      get { return fashionIds_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      fashions_.WriteTo(output, _repeated_fashions_codec);
      fashionIds_.WriteTo(output, _repeated_fashionIds_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += fashions_.CalculateSize(_repeated_fashions_codec);
      size += fashionIds_.CalculateSize(_repeated_fashionIds_codec);
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
            fashions_.AddEntriesFrom(input, _repeated_fashions_codec);
            break;
          }
          case 18:
          case 16: {
            fashionIds_.AddEntriesFrom(input, _repeated_fashionIds_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class FashionIdList : pb::IMessage {
    private static readonly pb::MessageParser<FashionIdList> _parser = new pb::MessageParser<FashionIdList>(() => new FashionIdList());
    public static pb::MessageParser<FashionIdList> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<int> _repeated_ids_codec
        = pb::FieldCodec.ForInt32(10);
    private readonly pbc::RepeatedField<int> ids_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///时装id列表;
    /// </summary>
    public pbc::RepeatedField<int> ids {
      get { return ids_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      ids_.WriteTo(output, _repeated_ids_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += ids_.CalculateSize(_repeated_ids_codec);
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
            ids_.AddEntriesFrom(input, _repeated_ids_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class ActiveFashion : pb::IMessage {
    private static readonly pb::MessageParser<ActiveFashion> _parser = new pb::MessageParser<ActiveFashion>(() => new ActiveFashion());
    public static pb::MessageParser<ActiveFashion> Parser { get { return _parser; } }

    private int chipItemId_;
    /// <summary>
    ///时装碎片道具id
    /// </summary>
    public int chipItemId {
      get { return chipItemId_; }
      set {
        chipItemId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (chipItemId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(chipItemId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (chipItemId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(chipItemId);
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
            chipItemId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
