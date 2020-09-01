// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: wrappers.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace google {

  #region Messages
  /// <summary>
  /// Wrapper message for `double`.
  ///
  /// The JSON representation for `DoubleValue` is JSON number.
  /// </summary>
  public sealed class DoubleValue : pb::IMessage {
    private static readonly pb::MessageParser<DoubleValue> _parser = new pb::MessageParser<DoubleValue>(() => new DoubleValue());
    public static pb::MessageParser<DoubleValue> Parser { get { return _parser; } }

    private double value_;
    /// <summary>
    /// The double value.
    /// </summary>
    public double value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (value != 0D) {
        output.WriteRawTag(9);
        output.WriteDouble(value);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (value != 0D) {
        size += 1 + 8;
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
          case 9: {
            value = input.ReadDouble();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Wrapper message for `float`.
  ///
  /// The JSON representation for `FloatValue` is JSON number.
  /// </summary>
  public sealed class FloatValue : pb::IMessage {
    private static readonly pb::MessageParser<FloatValue> _parser = new pb::MessageParser<FloatValue>(() => new FloatValue());
    public static pb::MessageParser<FloatValue> Parser { get { return _parser; } }

    private float value_;
    /// <summary>
    /// The float value.
    /// </summary>
    public float value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (value != 0F) {
        output.WriteRawTag(13);
        output.WriteFloat(value);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (value != 0F) {
        size += 1 + 4;
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
          case 13: {
            value = input.ReadFloat();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Wrapper message for `int64`.
  ///
  /// The JSON representation for `Int64Value` is JSON string.
  /// </summary>
  public sealed class Int64Value : pb::IMessage {
    private static readonly pb::MessageParser<Int64Value> _parser = new pb::MessageParser<Int64Value>(() => new Int64Value());
    public static pb::MessageParser<Int64Value> Parser { get { return _parser; } }

    private long value_;
    /// <summary>
    /// The int64 value.
    /// </summary>
    public long value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (value != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(value);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (value != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(value);
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
            value = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Wrapper message for `uint64`.
  ///
  /// The JSON representation for `UInt64Value` is JSON string.
  /// </summary>
  public sealed class UInt64Value : pb::IMessage {
    private static readonly pb::MessageParser<UInt64Value> _parser = new pb::MessageParser<UInt64Value>(() => new UInt64Value());
    public static pb::MessageParser<UInt64Value> Parser { get { return _parser; } }

    private ulong value_;
    /// <summary>
    /// The uint64 value.
    /// </summary>
    public ulong value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (value != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(value);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (value != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(value);
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
            value = input.ReadUInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Wrapper message for `int32`.
  ///
  /// The JSON representation for `Int32Value` is JSON number.
  /// </summary>
  public sealed class Int32Value : pb::IMessage {
    private static readonly pb::MessageParser<Int32Value> _parser = new pb::MessageParser<Int32Value>(() => new Int32Value());
    public static pb::MessageParser<Int32Value> Parser { get { return _parser; } }

    private int value_;
    /// <summary>
    /// The int32 value.
    /// </summary>
    public int value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (value != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(value);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (value != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(value);
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
            value = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Wrapper message for `uint32`.
  ///
  /// The JSON representation for `UInt32Value` is JSON number.
  /// </summary>
  public sealed class UInt32Value : pb::IMessage {
    private static readonly pb::MessageParser<UInt32Value> _parser = new pb::MessageParser<UInt32Value>(() => new UInt32Value());
    public static pb::MessageParser<UInt32Value> Parser { get { return _parser; } }

    private uint value_;
    /// <summary>
    /// The uint32 value.
    /// </summary>
    public uint value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (value != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(value);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (value != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(value);
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
            value = input.ReadUInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Wrapper message for `bool`.
  ///
  /// The JSON representation for `BoolValue` is JSON `true` and `false`.
  /// </summary>
  public sealed class BoolValue : pb::IMessage {
    private static readonly pb::MessageParser<BoolValue> _parser = new pb::MessageParser<BoolValue>(() => new BoolValue());
    public static pb::MessageParser<BoolValue> Parser { get { return _parser; } }

    private bool value_;
    /// <summary>
    /// The bool value.
    /// </summary>
    public bool value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (value != false) {
        output.WriteRawTag(8);
        output.WriteBool(value);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (value != false) {
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
            value = input.ReadBool();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Wrapper message for `string`.
  ///
  /// The JSON representation for `StringValue` is JSON string.
  /// </summary>
  public sealed class StringValue : pb::IMessage {
    private static readonly pb::MessageParser<StringValue> _parser = new pb::MessageParser<StringValue>(() => new StringValue());
    public static pb::MessageParser<StringValue> Parser { get { return _parser; } }

    private string value_ = "";
    /// <summary>
    /// The string value.
    /// </summary>
    public string value {
      get { return value_; }
      set {
        value_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (value.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(value);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (value.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(value);
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
            value = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Wrapper message for `bytes`.
  ///
  /// The JSON representation for `BytesValue` is JSON string.
  /// </summary>
  public sealed class BytesValue : pb::IMessage {
    private static readonly pb::MessageParser<BytesValue> _parser = new pb::MessageParser<BytesValue>(() => new BytesValue());
    public static pb::MessageParser<BytesValue> Parser { get { return _parser; } }

    private pb::ByteString value_ = pb::ByteString.Empty;
    /// <summary>
    /// The bytes value.
    /// </summary>
    public pb::ByteString value {
      get { return value_; }
      set {
        value_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (value.Length != 0) {
        output.WriteRawTag(10);
        output.WriteBytes(value);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (value.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(value);
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
            value = input.ReadBytes();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code