// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Social.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace social {

  #region Messages
  public sealed class FriendInfo : pb::IMessage {
    private static readonly pb::MessageParser<FriendInfo> _parser = new pb::MessageParser<FriendInfo>(() => new FriendInfo());
    public static pb::MessageParser<FriendInfo> Parser { get { return _parser; } }

    private long roleId_;
    public long roleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    private string name_ = "";
    public string name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    private int sex_;
    public int sex {
      get { return sex_; }
      set {
        sex_ = value;
      }
    }

    private int career_;
    public int career {
      get { return career_; }
      set {
        career_ = value;
      }
    }

    private int level_;
    public int level {
      get { return level_; }
      set {
        level_ = value;
      }
    }

    private bool isOnline_;
    public bool isOnline {
      get { return isOnline_; }
      set {
        isOnline_ = value;
      }
    }

    private int relation_;
    /// <summary>
    ///关系:1好友,2仇人,3黑名单 
    /// </summary>
    public int relation {
      get { return relation_; }
      set {
        relation_ = value;
      }
    }

    private int curServerId_;
    /// <summary>
    ///当前所在服务器Id
    /// </summary>
    public int curServerId {
      get { return curServerId_; }
      set {
        curServerId_ = value;
      }
    }

    private int curServerType_;
    /// <summary>
    ///当前所在服务器类型
    /// </summary>
    public int curServerType {
      get { return curServerType_; }
      set {
        curServerType_ = value;
      }
    }

    private int enemy_;
    /// <summary>
    ///仇恨值
    /// </summary>
    public int enemy {
      get { return enemy_; }
      set {
        enemy_ = value;
      }
    }

    private int friendLove_;
    /// <summary>
    ///好友亲密度
    /// </summary>
    public int friendLove {
      get { return friendLove_; }
      set {
        friendLove_ = value;
      }
    }

    private string union_ = "";
    /// <summary>
    ///家族
    /// </summary>
    public string union {
      get { return union_; }
      set {
        union_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
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
      if (sex != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(sex);
      }
      if (career != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(career);
      }
      if (level != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(level);
      }
      if (isOnline != false) {
        output.WriteRawTag(48);
        output.WriteBool(isOnline);
      }
      if (relation != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(relation);
      }
      if (curServerId != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(curServerId);
      }
      if (curServerType != 0) {
        output.WriteRawTag(72);
        output.WriteInt32(curServerType);
      }
      if (enemy != 0) {
        output.WriteRawTag(96);
        output.WriteInt32(enemy);
      }
      if (friendLove != 0) {
        output.WriteRawTag(104);
        output.WriteInt32(friendLove);
      }
      if (union.Length != 0) {
        output.WriteRawTag(114);
        output.WriteString(union);
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
      if (sex != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(sex);
      }
      if (career != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(career);
      }
      if (level != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(level);
      }
      if (isOnline != false) {
        size += 1 + 1;
      }
      if (relation != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(relation);
      }
      if (curServerId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(curServerId);
      }
      if (curServerType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(curServerType);
      }
      if (enemy != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(enemy);
      }
      if (friendLove != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(friendLove);
      }
      if (union.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(union);
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
          case 24: {
            sex = input.ReadInt32();
            break;
          }
          case 32: {
            career = input.ReadInt32();
            break;
          }
          case 40: {
            level = input.ReadInt32();
            break;
          }
          case 48: {
            isOnline = input.ReadBool();
            break;
          }
          case 56: {
            relation = input.ReadInt32();
            break;
          }
          case 64: {
            curServerId = input.ReadInt32();
            break;
          }
          case 72: {
            curServerType = input.ReadInt32();
            break;
          }
          case 96: {
            enemy = input.ReadInt32();
            break;
          }
          case 104: {
            friendLove = input.ReadInt32();
            break;
          }
          case 114: {
            union = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed class SocialInfo : pb::IMessage {
    private static readonly pb::MessageParser<SocialInfo> _parser = new pb::MessageParser<SocialInfo>(() => new SocialInfo());
    public static pb::MessageParser<SocialInfo> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::social.FriendInfo> _repeated_relations_codec
        = pb::FieldCodec.ForMessage(10, global::social.FriendInfo.Parser);
    private readonly pbc::RepeatedField<global::social.FriendInfo> relations_ = new pbc::RepeatedField<global::social.FriendInfo>();
    public pbc::RepeatedField<global::social.FriendInfo> relations {
      get { return relations_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      relations_.WriteTo(output, _repeated_relations_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += relations_.CalculateSize(_repeated_relations_codec);
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
            relations_.AddEntriesFrom(input, _repeated_relations_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///添加关系请求
  /// </summary>
  public sealed class AddRelationRequest : pb::IMessage {
    private static readonly pb::MessageParser<AddRelationRequest> _parser = new pb::MessageParser<AddRelationRequest>(() => new AddRelationRequest());
    public static pb::MessageParser<AddRelationRequest> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<long> _repeated_roleId_codec
        = pb::FieldCodec.ForInt64(10);
    private readonly pbc::RepeatedField<long> roleId_ = new pbc::RepeatedField<long>();
    public pbc::RepeatedField<long> roleId {
      get { return roleId_; }
    }

    private int relation_;
    /// <summary>
    ///关系:1好友,2仇人,3黑名单 
    /// </summary>
    public int relation {
      get { return relation_; }
      set {
        relation_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      roleId_.WriteTo(output, _repeated_roleId_codec);
      if (relation != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(relation);
      }
    }

    public int CalculateSize() {
      int size = 0;
      size += roleId_.CalculateSize(_repeated_roleId_codec);
      if (relation != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(relation);
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
          case 10:
          case 8: {
            roleId_.AddEntriesFrom(input, _repeated_roleId_codec);
            break;
          }
          case 16: {
            relation = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///通过名字添加好友
  /// </summary>
  public sealed class AddFriendByNameRequest : pb::IMessage {
    private static readonly pb::MessageParser<AddFriendByNameRequest> _parser = new pb::MessageParser<AddFriendByNameRequest>(() => new AddFriendByNameRequest());
    public static pb::MessageParser<AddFriendByNameRequest> Parser { get { return _parser; } }

    private string name_ = "";
    public string name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (name.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(name);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(name);
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
            name = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///添加关系响应
  /// </summary>
  public sealed class AddRelationResponse : pb::IMessage {
    private static readonly pb::MessageParser<AddRelationResponse> _parser = new pb::MessageParser<AddRelationResponse>(() => new AddRelationResponse());
    public static pb::MessageParser<AddRelationResponse> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::social.FriendInfo> _repeated_added_codec
        = pb::FieldCodec.ForMessage(10, global::social.FriendInfo.Parser);
    private readonly pbc::RepeatedField<global::social.FriendInfo> added_ = new pbc::RepeatedField<global::social.FriendInfo>();
    public pbc::RepeatedField<global::social.FriendInfo> added {
      get { return added_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      added_.WriteTo(output, _repeated_added_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += added_.CalculateSize(_repeated_added_codec);
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
            added_.AddEntriesFrom(input, _repeated_added_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///通过名字查找请求
  /// </summary>
  public sealed class FindPlayerByNameRequest : pb::IMessage {
    private static readonly pb::MessageParser<FindPlayerByNameRequest> _parser = new pb::MessageParser<FindPlayerByNameRequest>(() => new FindPlayerByNameRequest());
    public static pb::MessageParser<FindPlayerByNameRequest> Parser { get { return _parser; } }

    private string name_ = "";
    public string name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (name.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(name);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(name);
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
            name = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///通过名字查找响应
  /// </summary>
  public sealed class FindPlayerByNameResponse : pb::IMessage {
    private static readonly pb::MessageParser<FindPlayerByNameResponse> _parser = new pb::MessageParser<FindPlayerByNameResponse>(() => new FindPlayerByNameResponse());
    public static pb::MessageParser<FindPlayerByNameResponse> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::social.FriendInfo> _repeated_players_codec
        = pb::FieldCodec.ForMessage(10, global::social.FriendInfo.Parser);
    private readonly pbc::RepeatedField<global::social.FriendInfo> players_ = new pbc::RepeatedField<global::social.FriendInfo>();
    public pbc::RepeatedField<global::social.FriendInfo> players {
      get { return players_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      players_.WriteTo(output, _repeated_players_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += players_.CalculateSize(_repeated_players_codec);
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
            players_.AddEntriesFrom(input, _repeated_players_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///删除好友请求
  /// </summary>
  public sealed class DeleteRelationRequest : pb::IMessage {
    private static readonly pb::MessageParser<DeleteRelationRequest> _parser = new pb::MessageParser<DeleteRelationRequest>(() => new DeleteRelationRequest());
    public static pb::MessageParser<DeleteRelationRequest> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<long> _repeated_roleId_codec
        = pb::FieldCodec.ForInt64(10);
    private readonly pbc::RepeatedField<long> roleId_ = new pbc::RepeatedField<long>();
    public pbc::RepeatedField<long> roleId {
      get { return roleId_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      roleId_.WriteTo(output, _repeated_roleId_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += roleId_.CalculateSize(_repeated_roleId_codec);
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
            roleId_.AddEntriesFrom(input, _repeated_roleId_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///申请加好友的列表
  /// </summary>
  public sealed class ApplyFriendList : pb::IMessage {
    private static readonly pb::MessageParser<ApplyFriendList> _parser = new pb::MessageParser<ApplyFriendList>(() => new ApplyFriendList());
    public static pb::MessageParser<ApplyFriendList> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::social.FriendInfo> _repeated_applys_codec
        = pb::FieldCodec.ForMessage(10, global::social.FriendInfo.Parser);
    private readonly pbc::RepeatedField<global::social.FriendInfo> applys_ = new pbc::RepeatedField<global::social.FriendInfo>();
    public pbc::RepeatedField<global::social.FriendInfo> applys {
      get { return applys_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      applys_.WriteTo(output, _repeated_applys_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += applys_.CalculateSize(_repeated_applys_codec);
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
            applys_.AddEntriesFrom(input, _repeated_applys_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///关系信息
  /// </summary>
  public sealed class RelationInfo : pb::IMessage {
    private static readonly pb::MessageParser<RelationInfo> _parser = new pb::MessageParser<RelationInfo>(() => new RelationInfo());
    public static pb::MessageParser<RelationInfo> Parser { get { return _parser; } }

    private long roleId_;
    public long roleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    private int relationType_;
    /// <summary>
    ///1:好友 2：仇人 3:黑名单 4:师傅 5:徒弟 6:兄弟
    /// </summary>
    public int relationType {
      get { return relationType_; }
      set {
        relationType_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (roleId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(roleId);
      }
      if (relationType != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(relationType);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (roleId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(roleId);
      }
      if (relationType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(relationType);
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
          case 16: {
            relationType = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///所有的关系信息
  /// </summary>
  public sealed class RelationAllResponse : pb::IMessage {
    private static readonly pb::MessageParser<RelationAllResponse> _parser = new pb::MessageParser<RelationAllResponse>(() => new RelationAllResponse());
    public static pb::MessageParser<RelationAllResponse> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::social.RelationInfo> _repeated_relationInfo_codec
        = pb::FieldCodec.ForMessage(10, global::social.RelationInfo.Parser);
    private readonly pbc::RepeatedField<global::social.RelationInfo> relationInfo_ = new pbc::RepeatedField<global::social.RelationInfo>();
    public pbc::RepeatedField<global::social.RelationInfo> relationInfo {
      get { return relationInfo_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      relationInfo_.WriteTo(output, _repeated_relationInfo_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += relationInfo_.CalculateSize(_repeated_relationInfo_codec);
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
            relationInfo_.AddEntriesFrom(input, _repeated_relationInfo_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///拒绝单个人请求
  /// </summary>
  public sealed class RejectSingleReq : pb::IMessage {
    private static readonly pb::MessageParser<RejectSingleReq> _parser = new pb::MessageParser<RejectSingleReq>(() => new RejectSingleReq());
    public static pb::MessageParser<RejectSingleReq> Parser { get { return _parser; } }

    private long roleId_;
    public long roleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (roleId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(roleId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (roleId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(roleId);
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
        }
      }
    }

  }

  /// <summary>
  ///拒绝单个人响应
  /// </summary>
  public sealed class RejectSingleAck : pb::IMessage {
    private static readonly pb::MessageParser<RejectSingleAck> _parser = new pb::MessageParser<RejectSingleAck>(() => new RejectSingleAck());
    public static pb::MessageParser<RejectSingleAck> Parser { get { return _parser; } }

    private long roleId_;
    public long roleId {
      get { return roleId_; }
      set {
        roleId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (roleId != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(roleId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (roleId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(roleId);
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
        }
      }
    }

  }

  /// <summary>
  ///查询最近联系人请求
  /// </summary>
  public sealed class QueryLatelyTouchReq : pb::IMessage {
    private static readonly pb::MessageParser<QueryLatelyTouchReq> _parser = new pb::MessageParser<QueryLatelyTouchReq>(() => new QueryLatelyTouchReq());
    public static pb::MessageParser<QueryLatelyTouchReq> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<long> _repeated_roleId_codec
        = pb::FieldCodec.ForInt64(10);
    private readonly pbc::RepeatedField<long> roleId_ = new pbc::RepeatedField<long>();
    public pbc::RepeatedField<long> roleId {
      get { return roleId_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      roleId_.WriteTo(output, _repeated_roleId_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += roleId_.CalculateSize(_repeated_roleId_codec);
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
            roleId_.AddEntriesFrom(input, _repeated_roleId_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///查询最近联系人响应
  /// </summary>
  public sealed class QueryLatelyTouchAck : pb::IMessage {
    private static readonly pb::MessageParser<QueryLatelyTouchAck> _parser = new pb::MessageParser<QueryLatelyTouchAck>(() => new QueryLatelyTouchAck());
    public static pb::MessageParser<QueryLatelyTouchAck> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::social.FriendInfo> _repeated_touchList_codec
        = pb::FieldCodec.ForMessage(10, global::social.FriendInfo.Parser);
    private readonly pbc::RepeatedField<global::social.FriendInfo> touchList_ = new pbc::RepeatedField<global::social.FriendInfo>();
    public pbc::RepeatedField<global::social.FriendInfo> touchList {
      get { return touchList_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      touchList_.WriteTo(output, _repeated_touchList_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += touchList_.CalculateSize(_repeated_touchList_codec);
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
            touchList_.AddEntriesFrom(input, _repeated_touchList_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///设置里的拒绝好友请求按钮
  /// </summary>
  public sealed class ReqSettingSocial : pb::IMessage {
    private static readonly pb::MessageParser<ReqSettingSocial> _parser = new pb::MessageParser<ReqSettingSocial>(() => new ReqSettingSocial());
    public static pb::MessageParser<ReqSettingSocial> Parser { get { return _parser; } }

    private int socialFlag_;
    /// <summary>
    /// 1：关闭 2：开启
    /// </summary>
    public int socialFlag {
      get { return socialFlag_; }
      set {
        socialFlag_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (socialFlag != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(socialFlag);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (socialFlag != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(socialFlag);
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
            socialFlag = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///设置里的拒绝陌生人消息的按钮
  /// </summary>
  public sealed class ReqSettingStrangerInfo : pb::IMessage {
    private static readonly pb::MessageParser<ReqSettingStrangerInfo> _parser = new pb::MessageParser<ReqSettingStrangerInfo>(() => new ReqSettingStrangerInfo());
    public static pb::MessageParser<ReqSettingStrangerInfo> Parser { get { return _parser; } }

    private int strangerFlag_;
    /// <summary>
    /// 1：关闭 2：开启
    /// </summary>
    public int strangerFlag {
      get { return strangerFlag_; }
      set {
        strangerFlag_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (strangerFlag != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(strangerFlag);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (strangerFlag != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(strangerFlag);
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
            strangerFlag = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///登录响应，返回设置里两个按钮的状态
  /// </summary>
  public sealed class ResSetting : pb::IMessage {
    private static readonly pb::MessageParser<ResSetting> _parser = new pb::MessageParser<ResSetting>(() => new ResSetting());
    public static pb::MessageParser<ResSetting> Parser { get { return _parser; } }

    private int socialFlag_;
    /// <summary>
    /// 1：关闭 2：开启
    /// </summary>
    public int socialFlag {
      get { return socialFlag_; }
      set {
        socialFlag_ = value;
      }
    }

    private int strangerFlag_;
    /// <summary>
    /// 1：关闭 2：开启
    /// </summary>
    public int strangerFlag {
      get { return strangerFlag_; }
      set {
        strangerFlag_ = value;
      }
    }

    private int guildFlag_;
    /// <summary>
    /// 1：关闭 2：开启
    /// </summary>
    public int guildFlag {
      get { return guildFlag_; }
      set {
        guildFlag_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (socialFlag != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(socialFlag);
      }
      if (strangerFlag != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(strangerFlag);
      }
      if (guildFlag != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(guildFlag);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (socialFlag != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(socialFlag);
      }
      if (strangerFlag != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(strangerFlag);
      }
      if (guildFlag != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(guildFlag);
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
            socialFlag = input.ReadInt32();
            break;
          }
          case 16: {
            strangerFlag = input.ReadInt32();
            break;
          }
          case 24: {
            guildFlag = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///设置里的拒绝行会的按钮
  /// </summary>
  public sealed class ReqSettingGuild : pb::IMessage {
    private static readonly pb::MessageParser<ReqSettingGuild> _parser = new pb::MessageParser<ReqSettingGuild>(() => new ReqSettingGuild());
    public static pb::MessageParser<ReqSettingGuild> Parser { get { return _parser; } }

    private int guildFlag_;
    /// <summary>
    /// 1：关闭 2：开启
    /// </summary>
    public int guildFlag {
      get { return guildFlag_; }
      set {
        guildFlag_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (guildFlag != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(guildFlag);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (guildFlag != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(guildFlag);
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
            guildFlag = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code