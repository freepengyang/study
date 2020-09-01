// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: RankAloneTable.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace rankalonetable {

  #region Messages
  public sealed class RoleRankInfo : pb::IMessage {
    private static readonly pb::MessageParser<RoleRankInfo> _parser = new pb::MessageParser<RoleRankInfo>(() => new RoleRankInfo());
    public static pb::MessageParser<RoleRankInfo> Parser { get { return _parser; } }

    private long roleId_;
    /// <summary>
    ///角色id
    /// </summary>
    public long roleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    private string name_ = "";
    /// <summary>
    ///角色名字
    /// </summary>
    public string name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    private string unionName_ = "";
    /// <summary>
    ///公会名称
    /// </summary>
    public string unionName {
      get { return unionName_; }
      set {
        unionName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    private int sex_;
    /// <summary>
    ///性别
    /// </summary>
    public int sex {
      get { return sex_; }
      set {
        sex_ = value;
      }
    }

    private int career_;
    /// <summary>
    ///职业
    /// </summary>
    public int career {
      get { return career_; }
      set {
        career_ = value;
      }
    }

    private string value_ = "";
    /// <summary>
    ///排行数据
    /// </summary>
    public string value {
      get { return value_; }
      set {
        value_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    private int rank_;
    /// <summary>
    ///排名 1开始
    /// </summary>
    public int rank {
      get { return rank_; }
      set {
        rank_ = value;
      }
    }

    private int level_;
    /// <summary>
    ///角色等级
    /// </summary>
    public int level {
      get { return level_; }
      set {
        level_ = value;
      }
    }

    private long unionId_;
    /// <summary>
    ///公会id
    /// </summary>
    public long unionId {
      get { return unionId_; }
      set {
        unionId_ = value;
      }
    }

    private int position_;
    /// <summary>
    ///公会职位
    /// </summary>
    public int position {
      get { return position_; }
      set {
        position_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (roleId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(roleId);
      }
      if (name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(name);
      }
      if (unionName.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(unionName);
      }
      if (sex != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(sex);
      }
      if (career != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(career);
      }
      if (value.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(value);
      }
      if (rank != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(rank);
      }
      if (level != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(level);
      }
      if (unionId != 0L) {
        output.WriteRawTag(72);
        output.WriteInt64(unionId);
      }
      if (position != 0) {
        output.WriteRawTag(80);
        output.WriteInt32(position);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (roleId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(roleId);
      }
      if (name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(name);
      }
      if (unionName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(unionName);
      }
      if (sex != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(sex);
      }
      if (career != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(career);
      }
      if (value.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(value);
      }
      if (rank != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(rank);
      }
      if (level != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(level);
      }
      if (unionId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(unionId);
      }
      if (position != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(position);
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
            roleId = input.ReadInt64();
            break;
          }
          case 18: {
            name = input.ReadString();
            break;
          }
          case 26: {
            unionName = input.ReadString();
            break;
          }
          case 32: {
            sex = input.ReadInt32();
            break;
          }
          case 40: {
            career = input.ReadInt32();
            break;
          }
          case 50: {
            value = input.ReadString();
            break;
          }
          case 56: {
            rank = input.ReadInt32();
            break;
          }
          case 64: {
            level = input.ReadInt32();
            break;
          }
          case 72: {
            unionId = input.ReadInt64();
            break;
          }
          case 80: {
            position = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class RoleRankInfoResponse : pb::IMessage {
    private static readonly pb::MessageParser<RoleRankInfoResponse> _parser = new pb::MessageParser<RoleRankInfoResponse>(() => new RoleRankInfoResponse());
    public static pb::MessageParser<RoleRankInfoResponse> Parser { get { return _parser; } }

    private int type_;
    /// <summary>
    ///101 
    /// </summary>
    public int type {
      get { return type_; }
      set {
        type_ = value;
      }
    }

    private static readonly pb::FieldCodec<global::rankalonetable.RoleRankInfo> _repeated_roleRankInfo_codec
        = pb::FieldCodec.ForMessage(18, global::rankalonetable.RoleRankInfo.Parser);
    private readonly pbc::RepeatedField<global::rankalonetable.RoleRankInfo> roleRankInfo_ = new pbc::RepeatedField<global::rankalonetable.RoleRankInfo>();
    public pbc::RepeatedField<global::rankalonetable.RoleRankInfo> roleRankInfo {
      get { return roleRankInfo_; }
    }

    private int myRank_;
    public int myRank {
      get { return myRank_; }
      set {
        myRank_ = value;
      }
    }

    private int subType_;
    public int subType {
      get { return subType_; }
      set {
        subType_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (type != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(type);
      }
      roleRankInfo_.WriteTo(output, _repeated_roleRankInfo_codec);
      if (myRank != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(myRank);
      }
      if (subType != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(subType);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (type != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(type);
      }
      size += roleRankInfo_.CalculateSize(_repeated_roleRankInfo_codec);
      if (myRank != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(myRank);
      }
      if (subType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(subType);
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
            roleRankInfo_.AddEntriesFrom(input, _repeated_roleRankInfo_codec);
            break;
          }
          case 24: {
            myRank = input.ReadInt32();
            break;
          }
          case 32: {
            subType = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class UnionRankInfo : pb::IMessage {
    private static readonly pb::MessageParser<UnionRankInfo> _parser = new pb::MessageParser<UnionRankInfo>(() => new UnionRankInfo());
    public static pb::MessageParser<UnionRankInfo> Parser { get { return _parser; } }

    private long unionId_;
    /// <summary>
    ///公会id
    /// </summary>
    public long unionId {
      get { return unionId_; }
      set {
        unionId_ = value;
      }
    }

    private string unionName_ = "";
    /// <summary>
    ///公会名称
    /// </summary>
    public string unionName {
      get { return unionName_; }
      set {
        unionName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    private int level_;
    /// <summary>
    ///公会等级
    /// </summary>
    public int level {
      get { return level_; }
      set {
        level_ = value;
      }
    }

    private long leaderId_;
    /// <summary>
    ///会长id
    /// </summary>
    public long leaderId {
      get { return leaderId_; }
      set {
        leaderId_ = value;
      }
    }

    private string name_ = "";
    /// <summary>
    ///会长名字
    /// </summary>
    public string name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    private int sex_;
    /// <summary>
    ///会长性别
    /// </summary>
    public int sex {
      get { return sex_; }
      set {
        sex_ = value;
      }
    }

    private int career_;
    /// <summary>
    ///会长职业
    /// </summary>
    public int career {
      get { return career_; }
      set {
        career_ = value;
      }
    }

    private int rank_;
    /// <summary>
    ///排名 1开始
    /// </summary>
    public int rank {
      get { return rank_; }
      set {
        rank_ = value;
      }
    }

    private int leaderLevel_;
    /// <summary>
    ///角色等级
    /// </summary>
    public int leaderLevel {
      get { return leaderLevel_; }
      set {
        leaderLevel_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (unionId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(unionId);
      }
      if (unionName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(unionName);
      }
      if (level != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(level);
      }
      if (leaderId != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(leaderId);
      }
      if (name.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(name);
      }
      if (sex != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(sex);
      }
      if (career != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(career);
      }
      if (rank != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(rank);
      }
      if (leaderLevel != 0) {
        output.WriteRawTag(72);
        output.WriteInt32(leaderLevel);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (unionId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(unionId);
      }
      if (unionName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(unionName);
      }
      if (level != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(level);
      }
      if (leaderId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(leaderId);
      }
      if (name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(name);
      }
      if (sex != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(sex);
      }
      if (career != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(career);
      }
      if (rank != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(rank);
      }
      if (leaderLevel != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(leaderLevel);
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
            unionId = input.ReadInt64();
            break;
          }
          case 18: {
            unionName = input.ReadString();
            break;
          }
          case 24: {
            level = input.ReadInt32();
            break;
          }
          case 32: {
            leaderId = input.ReadInt64();
            break;
          }
          case 42: {
            name = input.ReadString();
            break;
          }
          case 48: {
            sex = input.ReadInt32();
            break;
          }
          case 56: {
            career = input.ReadInt32();
            break;
          }
          case 64: {
            rank = input.ReadInt32();
            break;
          }
          case 72: {
            leaderLevel = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class UnionRankInfoResponse : pb::IMessage {
    private static readonly pb::MessageParser<UnionRankInfoResponse> _parser = new pb::MessageParser<UnionRankInfoResponse>(() => new UnionRankInfoResponse());
    public static pb::MessageParser<UnionRankInfoResponse> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::rankalonetable.UnionRankInfo> _repeated_unionRankInfo_codec
        = pb::FieldCodec.ForMessage(10, global::rankalonetable.UnionRankInfo.Parser);
    private readonly pbc::RepeatedField<global::rankalonetable.UnionRankInfo> unionRankInfo_ = new pbc::RepeatedField<global::rankalonetable.UnionRankInfo>();
    public pbc::RepeatedField<global::rankalonetable.UnionRankInfo> unionRankInfo {
      get { return unionRankInfo_; }
    }

    private int myRank_;
    public int myRank {
      get { return myRank_; }
      set {
        myRank_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      unionRankInfo_.WriteTo(output, _repeated_unionRankInfo_codec);
      if (myRank != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(myRank);
      }
    }

    public int CalculateSize() {
      int size = 0;
      size += unionRankInfo_.CalculateSize(_repeated_unionRankInfo_codec);
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
          case 10: {
            unionRankInfo_.AddEntriesFrom(input, _repeated_unionRankInfo_codec);
            break;
          }
          case 16: {
            myRank = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class RankInfoRequrst : pb::IMessage {
    private static readonly pb::MessageParser<RankInfoRequrst> _parser = new pb::MessageParser<RankInfoRequrst>(() => new RankInfoRequrst());
    public static pb::MessageParser<RankInfoRequrst> Parser { get { return _parser; } }

    private int type_;
    public int type {
      get { return type_; }
      set {
        type_ = value;
      }
    }

    private int subType_;
    public int subType {
      get { return subType_; }
      set {
        subType_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (type != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(type);
      }
      if (subType != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(subType);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (type != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(type);
      }
      if (subType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(subType);
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
          case 16: {
            subType = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code