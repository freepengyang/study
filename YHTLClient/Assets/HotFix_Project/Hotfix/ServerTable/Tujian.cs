// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Tujian.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace tujian {

  #region Messages
  /// <summary>
  ///图鉴信息
  /// </summary>
  public sealed class TujianInfo : pb::IMessage {
    private static readonly pb::MessageParser<TujianInfo> _parser = new pb::MessageParser<TujianInfo>(() => new TujianInfo());
    public static pb::MessageParser<TujianInfo> Parser { get { return _parser; } }

    private long id_;
    /// <summary>
    ///唯一ID
    /// </summary>
    public long id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    private int slotId_;
    /// <summary>
    ///槽位（0为未镶嵌）
    /// </summary>
    public int slotId {
      get { return slotId_; }
      set {
        slotId_ = value;
      }
    }

    private int handBookId_;
    /// <summary>
    ///图鉴配置id
    /// </summary>
    public int handBookId {
      get { return handBookId_; }
      set {
        handBookId_ = value;
      }
    }

    private int bind_;
    /// <summary>
    ///是否绑定
    /// </summary>
    public int bind {
      get { return bind_; }
      set {
        bind_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (id != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(id);
      }
      if (slotId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(slotId);
      }
      if (handBookId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(handBookId);
      }
      if (bind != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(bind);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (id != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(id);
      }
      if (slotId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(slotId);
      }
      if (handBookId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(handBookId);
      }
      if (bind != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(bind);
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
            id = input.ReadInt64();
            break;
          }
          case 16: {
            slotId = input.ReadInt32();
            break;
          }
          case 24: {
            handBookId = input.ReadInt32();
            break;
          }
          case 32: {
            bind = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///推送给客户端的图鉴系统数据
  /// </summary>
  public sealed class TujianInfoResponse : pb::IMessage {
    private static readonly pb::MessageParser<TujianInfoResponse> _parser = new pb::MessageParser<TujianInfoResponse>(() => new TujianInfoResponse());
    public static pb::MessageParser<TujianInfoResponse> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::tujian.TujianInfo> _repeated_tujianInfos_codec
        = pb::FieldCodec.ForMessage(10, global::tujian.TujianInfo.Parser);
    private readonly pbc::RepeatedField<global::tujian.TujianInfo> tujianInfos_ = new pbc::RepeatedField<global::tujian.TujianInfo>();
    /// <summary>
    ///图鉴信息
    /// </summary>
    public pbc::RepeatedField<global::tujian.TujianInfo> tujianInfos {
      get { return tujianInfos_; }
    }

    private long tujianSlot_;
    /// <summary>
    ///已激活的槽位
    /// </summary>
    public long tujianSlot {
      get { return tujianSlot_; }
      set {
        tujianSlot_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      tujianInfos_.WriteTo(output, _repeated_tujianInfos_codec);
      if (tujianSlot != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(tujianSlot);
      }
    }

    public int CalculateSize() {
      int size = 0;
      size += tujianInfos_.CalculateSize(_repeated_tujianInfos_codec);
      if (tujianSlot != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(tujianSlot);
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
            tujianInfos_.AddEntriesFrom(input, _repeated_tujianInfos_codec);
            break;
          }
          case 16: {
            tujianSlot = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///图鉴升级请求
  /// </summary>
  public sealed class TujianUpLevelRequest : pb::IMessage {
    private static readonly pb::MessageParser<TujianUpLevelRequest> _parser = new pb::MessageParser<TujianUpLevelRequest>(() => new TujianUpLevelRequest());
    public static pb::MessageParser<TujianUpLevelRequest> Parser { get { return _parser; } }

    private long id_;
    /// <summary>
    ///图鉴唯一ID
    /// </summary>
    public long id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (id != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(id);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (id != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(id);
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
            id = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///图鉴升级返回
  /// </summary>
  public sealed class TujianUpLevelResponse : pb::IMessage {
    private static readonly pb::MessageParser<TujianUpLevelResponse> _parser = new pb::MessageParser<TujianUpLevelResponse>(() => new TujianUpLevelResponse());
    public static pb::MessageParser<TujianUpLevelResponse> Parser { get { return _parser; } }

    private global::tujian.TujianInfo tujianInfo_;
    /// <summary>
    ///图鉴信息
    /// </summary>
    public global::tujian.TujianInfo tujianInfo {
      get { return tujianInfo_; }
      set {
        tujianInfo_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (tujianInfo_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(tujianInfo);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (tujianInfo_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(tujianInfo);
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
            if (tujianInfo_ == null) {
              tujianInfo_ = new global::tujian.TujianInfo();
            }
            input.ReadMessage(tujianInfo_);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///图鉴升品请求
  /// </summary>
  public sealed class TujianUpQualityRequest : pb::IMessage {
    private static readonly pb::MessageParser<TujianUpQualityRequest> _parser = new pb::MessageParser<TujianUpQualityRequest>(() => new TujianUpQualityRequest());
    public static pb::MessageParser<TujianUpQualityRequest> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<long> _repeated_ids_codec
        = pb::FieldCodec.ForInt64(10);
    private readonly pbc::RepeatedField<long> ids_ = new pbc::RepeatedField<long>();
    /// <summary>
    ///用来升品的图鉴id列表
    /// </summary>
    public pbc::RepeatedField<long> ids {
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

  /// <summary>
  ///图鉴升品返回
  /// </summary>
  public sealed class TujianUpQualityResponse : pb::IMessage {
    private static readonly pb::MessageParser<TujianUpQualityResponse> _parser = new pb::MessageParser<TujianUpQualityResponse>(() => new TujianUpQualityResponse());
    public static pb::MessageParser<TujianUpQualityResponse> Parser { get { return _parser; } }

    private global::tujian.TujianInfo tujianInfo_;
    /// <summary>
    ///图鉴信息
    /// </summary>
    public global::tujian.TujianInfo tujianInfo {
      get { return tujianInfo_; }
      set {
        tujianInfo_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (tujianInfo_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(tujianInfo);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (tujianInfo_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(tujianInfo);
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
            if (tujianInfo_ == null) {
              tujianInfo_ = new global::tujian.TujianInfo();
            }
            input.ReadMessage(tujianInfo_);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///图鉴镶嵌请求
  /// </summary>
  public sealed class TujianInlayRequest : pb::IMessage {
    private static readonly pb::MessageParser<TujianInlayRequest> _parser = new pb::MessageParser<TujianInlayRequest>(() => new TujianInlayRequest());
    public static pb::MessageParser<TujianInlayRequest> Parser { get { return _parser; } }

    private long id_;
    /// <summary>
    ///图鉴唯一ID
    /// </summary>
    public long id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    private int slotId_;
    /// <summary>
    ///槽位
    /// </summary>
    public int slotId {
      get { return slotId_; }
      set {
        slotId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (id != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(id);
      }
      if (slotId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(slotId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (id != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(id);
      }
      if (slotId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(slotId);
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
            id = input.ReadInt64();
            break;
          }
          case 16: {
            slotId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///图鉴镶嵌返回
  /// </summary>
  public sealed class TujianInlayResponse : pb::IMessage {
    private static readonly pb::MessageParser<TujianInlayResponse> _parser = new pb::MessageParser<TujianInlayResponse>(() => new TujianInlayResponse());
    public static pb::MessageParser<TujianInlayResponse> Parser { get { return _parser; } }

    private global::tujian.TujianInfo tujianInfo_;
    /// <summary>
    ///图鉴信息
    /// </summary>
    public global::tujian.TujianInfo tujianInfo {
      get { return tujianInfo_; }
      set {
        tujianInfo_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (tujianInfo_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(tujianInfo);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (tujianInfo_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(tujianInfo);
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
            if (tujianInfo_ == null) {
              tujianInfo_ = new global::tujian.TujianInfo();
            }
            input.ReadMessage(tujianInfo_);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///激活槽位请求
  /// </summary>
  public sealed class ActivateSlotRequest : pb::IMessage {
    private static readonly pb::MessageParser<ActivateSlotRequest> _parser = new pb::MessageParser<ActivateSlotRequest>(() => new ActivateSlotRequest());
    public static pb::MessageParser<ActivateSlotRequest> Parser { get { return _parser; } }

    private int slotId_;
    /// <summary>
    ///槽位
    /// </summary>
    public int slotId {
      get { return slotId_; }
      set {
        slotId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (slotId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(slotId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (slotId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(slotId);
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
            slotId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///激活槽位返回
  /// </summary>
  public sealed class ActivateSlotResponse : pb::IMessage {
    private static readonly pb::MessageParser<ActivateSlotResponse> _parser = new pb::MessageParser<ActivateSlotResponse>(() => new ActivateSlotResponse());
    public static pb::MessageParser<ActivateSlotResponse> Parser { get { return _parser; } }

    private long tujianSlot_;
    /// <summary>
    ///已激活的槽位
    /// </summary>
    public long tujianSlot {
      get { return tujianSlot_; }
      set {
        tujianSlot_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (tujianSlot != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(tujianSlot);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (tujianSlot != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(tujianSlot);
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
          case 16: {
            tujianSlot = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///图鉴增加推送
  /// </summary>
  public sealed class TujianAddResponse : pb::IMessage {
    private static readonly pb::MessageParser<TujianAddResponse> _parser = new pb::MessageParser<TujianAddResponse>(() => new TujianAddResponse());
    public static pb::MessageParser<TujianAddResponse> Parser { get { return _parser; } }

    private global::tujian.TujianInfo tujianInfo_;
    /// <summary>
    ///图鉴信息
    /// </summary>
    public global::tujian.TujianInfo tujianInfo {
      get { return tujianInfo_; }
      set {
        tujianInfo_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (tujianInfo_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(tujianInfo);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (tujianInfo_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(tujianInfo);
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
            if (tujianInfo_ == null) {
              tujianInfo_ = new global::tujian.TujianInfo();
            }
            input.ReadMessage(tujianInfo_);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///图鉴移除推送
  /// </summary>
  public sealed class TujianRemoveResponse : pb::IMessage {
    private static readonly pb::MessageParser<TujianRemoveResponse> _parser = new pb::MessageParser<TujianRemoveResponse>(() => new TujianRemoveResponse());
    public static pb::MessageParser<TujianRemoveResponse> Parser { get { return _parser; } }

    private global::tujian.TujianInfo tujianInfo_;
    /// <summary>
    ///图鉴信息
    /// </summary>
    public global::tujian.TujianInfo tujianInfo {
      get { return tujianInfo_; }
      set {
        tujianInfo_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (tujianInfo_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(tujianInfo);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (tujianInfo_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(tujianInfo);
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
            if (tujianInfo_ == null) {
              tujianInfo_ = new global::tujian.TujianInfo();
            }
            input.ReadMessage(tujianInfo_);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code