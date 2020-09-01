// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Yuanbao.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace yuanbao {

  #region Messages
  public sealed class YuanBaoInfo : pb::IMessage {
    private static readonly pb::MessageParser<YuanBaoInfo> _parser = new pb::MessageParser<YuanBaoInfo>(() => new YuanBaoInfo());
    public static pb::MessageParser<YuanBaoInfo> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::yuanbao.InstanceGet> _repeated_instances_codec
        = pb::FieldCodec.ForMessage(10, global::yuanbao.InstanceGet.Parser);
    private readonly pbc::RepeatedField<global::yuanbao.InstanceGet> instances_ = new pbc::RepeatedField<global::yuanbao.InstanceGet>();
    /// <summary>
    ///副本途径
    /// </summary>
    public pbc::RepeatedField<global::yuanbao.InstanceGet> instances {
      get { return instances_; }
    }

    private int recycleGet_;
    /// <summary>
    ///回收获得
    /// </summary>
    public int recycleGet {
      get { return recycleGet_; }
      set {
        recycleGet_ = value;
      }
    }

    private int yewaiGet_;
    /// <summary>
    ///野外首领
    /// </summary>
    public int yewaiGet {
      get { return yewaiGet_; }
      set {
        yewaiGet_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      instances_.WriteTo(output, _repeated_instances_codec);
      if (recycleGet != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(recycleGet);
      }
      if (yewaiGet != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(yewaiGet);
      }
    }

    public int CalculateSize() {
      int size = 0;
      size += instances_.CalculateSize(_repeated_instances_codec);
      if (recycleGet != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(recycleGet);
      }
      if (yewaiGet != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(yewaiGet);
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
            instances_.AddEntriesFrom(input, _repeated_instances_codec);
            break;
          }
          case 16: {
            recycleGet = input.ReadInt32();
            break;
          }
          case 24: {
            yewaiGet = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class InstanceGet : pb::IMessage {
    private static readonly pb::MessageParser<InstanceGet> _parser = new pb::MessageParser<InstanceGet>(() => new InstanceGet());
    public static pb::MessageParser<InstanceGet> Parser { get { return _parser; } }

    private int instanceType_;
    /// <summary>
    ///副本类型
    /// </summary>
    public int instanceType {
      get { return instanceType_; }
      set {
        instanceType_ = value;
      }
    }

    private int count_;
    /// <summary>
    ///数量
    /// </summary>
    public int count {
      get { return count_; }
      set {
        count_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (instanceType != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(instanceType);
      }
      if (count != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(count);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (instanceType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(instanceType);
      }
      if (count != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(count);
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
            instanceType = input.ReadInt32();
            break;
          }
          case 16: {
            count = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
