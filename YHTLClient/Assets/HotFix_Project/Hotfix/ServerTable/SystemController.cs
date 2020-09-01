// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: SystemController.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace systemcontroller {

  #region Messages
  /// <summary>
  ///系统控制功能状态
  /// </summary>
  public sealed class SystemFunctionStateNtf : pb::IMessage {
    private static readonly pb::MessageParser<SystemFunctionStateNtf> _parser = new pb::MessageParser<SystemFunctionStateNtf>(() => new SystemFunctionStateNtf());
    public static pb::MessageParser<SystemFunctionStateNtf> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::systemcontroller.FunctionInfo> _repeated_info_codec
        = pb::FieldCodec.ForMessage(10, global::systemcontroller.FunctionInfo.Parser);
    private readonly pbc::RepeatedField<global::systemcontroller.FunctionInfo> info_ = new pbc::RepeatedField<global::systemcontroller.FunctionInfo>();
    public pbc::RepeatedField<global::systemcontroller.FunctionInfo> info {
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

  public sealed class FunctionInfo : pb::IMessage {
    private static readonly pb::MessageParser<FunctionInfo> _parser = new pb::MessageParser<FunctionInfo>(() => new FunctionInfo());
    public static pb::MessageParser<FunctionInfo> Parser { get { return _parser; } }

    private int functionId_;
    /// <summary>
    ///功能id
    /// </summary>
    public int functionId {
      get { return functionId_; }
      set {
        functionId_ = value;
      }
    }

    private int state_;
    /// <summary>
    ///功能状态 1 开启 0 关闭
    /// </summary>
    public int state {
      get { return state_; }
      set {
        state_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (functionId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(functionId);
      }
      if (state != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(state);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (functionId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(functionId);
      }
      if (state != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(state);
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
            functionId = input.ReadInt32();
            break;
          }
          case 16: {
            state = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///玩家功能开启状态;
  /// </summary>
  public sealed class RoleFunctionData : pb::IMessage {
    private static readonly pb::MessageParser<RoleFunctionData> _parser = new pb::MessageParser<RoleFunctionData>(() => new RoleFunctionData());
    public static pb::MessageParser<RoleFunctionData> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::systemcontroller.FunctionInfo> _repeated_info_codec
        = pb::FieldCodec.ForMessage(10, global::systemcontroller.FunctionInfo.Parser);
    private readonly pbc::RepeatedField<global::systemcontroller.FunctionInfo> info_ = new pbc::RepeatedField<global::systemcontroller.FunctionInfo>();
    public pbc::RepeatedField<global::systemcontroller.FunctionInfo> info {
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

  #endregion

}

#endregion Designer generated code
