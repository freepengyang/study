// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: c_table_randomgain.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace TABLE {

  #region Messages
  public partial class RANDOMGAIN : pb::IMessage {
    private static readonly pb::MessageParser<RANDOMGAIN> _parser = new pb::MessageParser<RANDOMGAIN>(() => new RANDOMGAIN());
    public static pb::MessageParser<RANDOMGAIN> Parser { get { return _parser; } }

    private int id_;
    public int id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    private int gainID_;
    public int gainID {
      get { return gainID_; }
      set {
        gainID_ = value;
      }
    }

    private int type_;
    /// <summary>
    /// sint32 career = 3;
    /// </summary>
    public int Type {
      get { return type_; }
      set {
        type_ = value;
      }
    }

    private int typeRate_;
    public int typeRate {
      get { return typeRate_; }
      set {
        typeRate_ = value;
      }
    }

    private int parameter_;
    public int parameter {
      get { return parameter_; }
      set {
        parameter_ = value;
      }
    }

    private int paraRate_;
    public int paraRate {
      get { return paraRate_; }
      set {
        paraRate_ = value;
      }
    }

    private int max_;
    public int max {
      get { return max_; }
      set {
        max_ = value;
      }
    }

    private string icon_ = "";
    public string Icon {
      get { return icon_; }
      set {
        icon_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    private int factor_;
    /// <summary>
    /// sint32 factorRate = 11;
    /// sint32 SortID = 12;
    /// sint32 tip = 13;
    /// </summary>
    public int factor {
      get { return factor_; }
      set {
        factor_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (id != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(id);
      }
      if (gainID != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(gainID);
      }
      if (Type != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(Type);
      }
      if (typeRate != 0) {
        output.WriteRawTag(40);
        output.WriteSInt32(typeRate);
      }
      if (parameter != 0) {
        output.WriteRawTag(48);
        output.WriteSInt32(parameter);
      }
      if (paraRate != 0) {
        output.WriteRawTag(56);
        output.WriteSInt32(paraRate);
      }
      if (max != 0) {
        output.WriteRawTag(64);
        output.WriteSInt32(max);
      }
      if (Icon.Length != 0) {
        output.WriteRawTag(74);
        output.WriteString(Icon);
      }
      if (factor != 0) {
        output.WriteRawTag(80);
        output.WriteSInt32(factor);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(id);
      }
      if (gainID != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(gainID);
      }
      if (Type != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Type);
      }
      if (typeRate != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(typeRate);
      }
      if (parameter != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(parameter);
      }
      if (paraRate != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(paraRate);
      }
      if (max != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(max);
      }
      if (Icon.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Icon);
      }
      if (factor != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(factor);
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
            id = input.ReadSInt32();
            break;
          }
          case 16: {
            gainID = input.ReadSInt32();
            break;
          }
          case 32: {
            Type = input.ReadSInt32();
            break;
          }
          case 40: {
            typeRate = input.ReadSInt32();
            break;
          }
          case 48: {
            parameter = input.ReadSInt32();
            break;
          }
          case 56: {
            paraRate = input.ReadSInt32();
            break;
          }
          case 64: {
            max = input.ReadSInt32();
            break;
          }
          case 74: {
            Icon = input.ReadString();
            break;
          }
          case 80: {
            factor = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  public partial class RANDOMGAINARRAY : pb::IMessage {
    private static readonly pb::MessageParser<RANDOMGAINARRAY> _parser = new pb::MessageParser<RANDOMGAINARRAY>(() => new RANDOMGAINARRAY());
    public static pb::MessageParser<RANDOMGAINARRAY> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::TABLE.RANDOMGAIN> _repeated_rows_codec
        = pb::FieldCodec.ForMessage(10, global::TABLE.RANDOMGAIN.Parser);
    private readonly pbc::RepeatedField<global::TABLE.RANDOMGAIN> rows_ = new pbc::RepeatedField<global::TABLE.RANDOMGAIN>();
    public pbc::RepeatedField<global::TABLE.RANDOMGAIN> rows {
      get { return rows_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      rows_.WriteTo(output, _repeated_rows_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += rows_.CalculateSize(_repeated_rows_codec);
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
            rows_.AddEntriesFrom(input, _repeated_rows_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code