// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Heart.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace heart {

  #region Messages
  public sealed class Heartbeat : pb::IMessage {
    private static readonly pb::MessageParser<Heartbeat> _parser = new pb::MessageParser<Heartbeat>(() => new Heartbeat());
    public static pb::MessageParser<Heartbeat> Parser { get { return _parser; } }

    private long nowTime_;
    /// <summary>
    ///当前时间
    /// </summary>
    public long nowTime {
      get { return nowTime_; }
      set {
        nowTime_ = value;
      }
    }

    private long clientTime_;
    public long clientTime {
      get { return clientTime_; }
      set {
        clientTime_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (nowTime != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(nowTime);
      }
      if (clientTime != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(clientTime);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (nowTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(nowTime);
      }
      if (clientTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(clientTime);
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
            nowTime = input.ReadInt64();
            break;
          }
          case 16: {
            clientTime = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code