// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Memory.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace memory {

  #region Messages
  public sealed class MemoryInstanceInfo : pb::IMessage {
    private static readonly pb::MessageParser<MemoryInstanceInfo> _parser = new pb::MessageParser<MemoryInstanceInfo>(() => new MemoryInstanceInfo());
    public static pb::MessageParser<MemoryInstanceInfo> Parser { get { return _parser; } }

    private int freeInstanceId_;
    /// <summary>
    ///免费副本
    /// </summary>
    public int freeInstanceId {
      get { return freeInstanceId_; }
      set {
        freeInstanceId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (freeInstanceId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(freeInstanceId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (freeInstanceId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(freeInstanceId);
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
            freeInstanceId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class MemoryBag : pb::IMessage {
    private static readonly pb::MessageParser<MemoryBag> _parser = new pb::MessageParser<MemoryBag>(() => new MemoryBag());
    public static pb::MessageParser<MemoryBag> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::bag.BagItemInfo> _repeated_bagItems_codec
        = pb::FieldCodec.ForMessage(10, global::bag.BagItemInfo.Parser);
    private readonly pbc::RepeatedField<global::bag.BagItemInfo> bagItems_ = new pbc::RepeatedField<global::bag.BagItemInfo>();
    /// <summary>
    ///背包里的物品
    /// </summary>
    public pbc::RepeatedField<global::bag.BagItemInfo> bagItems {
      get { return bagItems_; }
    }

    private int gezi_;
    /// <summary>
    ///格子
    /// </summary>
    public int gezi {
      get { return gezi_; }
      set {
        gezi_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      bagItems_.WriteTo(output, _repeated_bagItems_codec);
      if (gezi != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(gezi);
      }
    }

    public int CalculateSize() {
      int size = 0;
      size += bagItems_.CalculateSize(_repeated_bagItems_codec);
      if (gezi != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(gezi);
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
            bagItems_.AddEntriesFrom(input, _repeated_bagItems_codec);
            break;
          }
          case 16: {
            gezi = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class MemoryEquipInfo : pb::IMessage {
    private static readonly pb::MessageParser<MemoryEquipInfo> _parser = new pb::MessageParser<MemoryEquipInfo>(() => new MemoryEquipInfo());
    public static pb::MessageParser<MemoryEquipInfo> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::bag.BagItemInfo> _repeated_equips_codec
        = pb::FieldCodec.ForMessage(10, global::bag.BagItemInfo.Parser);
    private readonly pbc::RepeatedField<global::bag.BagItemInfo> equips_ = new pbc::RepeatedField<global::bag.BagItemInfo>();
    /// <summary>
    ///身上的装备
    /// </summary>
    public pbc::RepeatedField<global::bag.BagItemInfo> equips {
      get { return equips_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      equips_.WriteTo(output, _repeated_equips_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += equips_.CalculateSize(_repeated_equips_codec);
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
            equips_.AddEntriesFrom(input, _repeated_equips_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class MemoryAdd : pb::IMessage {
    private static readonly pb::MessageParser<MemoryAdd> _parser = new pb::MessageParser<MemoryAdd>(() => new MemoryAdd());
    public static pb::MessageParser<MemoryAdd> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::bag.BagItemInfo> _repeated_changeList_codec
        = pb::FieldCodec.ForMessage(10, global::bag.BagItemInfo.Parser);
    private readonly pbc::RepeatedField<global::bag.BagItemInfo> changeList_ = new pbc::RepeatedField<global::bag.BagItemInfo>();
    public pbc::RepeatedField<global::bag.BagItemInfo> changeList {
      get { return changeList_; }
    }

    private int logType_;
    /// <summary>
    ///变动原因
    /// </summary>
    public int logType {
      get { return logType_; }
      set {
        logType_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      changeList_.WriteTo(output, _repeated_changeList_codec);
      if (logType != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(logType);
      }
    }

    public int CalculateSize() {
      int size = 0;
      size += changeList_.CalculateSize(_repeated_changeList_codec);
      if (logType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(logType);
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
            changeList_.AddEntriesFrom(input, _repeated_changeList_codec);
            break;
          }
          case 16: {
            logType = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class MemoryRemove : pb::IMessage {
    private static readonly pb::MessageParser<MemoryRemove> _parser = new pb::MessageParser<MemoryRemove>(() => new MemoryRemove());
    public static pb::MessageParser<MemoryRemove> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<long> _repeated_changeList_codec
        = pb::FieldCodec.ForInt64(10);
    private readonly pbc::RepeatedField<long> changeList_ = new pbc::RepeatedField<long>();
    public pbc::RepeatedField<long> changeList {
      get { return changeList_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      changeList_.WriteTo(output, _repeated_changeList_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += changeList_.CalculateSize(_repeated_changeList_codec);
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
            changeList_.AddEntriesFrom(input, _repeated_changeList_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class MemoryEquipChange : pb::IMessage {
    private static readonly pb::MessageParser<MemoryEquipChange> _parser = new pb::MessageParser<MemoryEquipChange>(() => new MemoryEquipChange());
    public static pb::MessageParser<MemoryEquipChange> Parser { get { return _parser; } }

    private global::bag.BagItemInfo equips_;
    /// <summary>
    ///身上的装备
    /// </summary>
    public global::bag.BagItemInfo equips {
      get { return equips_; }
      set {
        equips_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (equips_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(equips);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (equips_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(equips);
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
            if (equips_ == null) {
              equips_ = new global::bag.BagItemInfo();
            }
            input.ReadMessage(equips_);
            break;
          }
        }
      }
    }

  }

  public sealed class MemoryEquipSuit : pb::IMessage {
    private static readonly pb::MessageParser<MemoryEquipSuit> _parser = new pb::MessageParser<MemoryEquipSuit>(() => new MemoryEquipSuit());
    public static pb::MessageParser<MemoryEquipSuit> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<int> _repeated_suits_codec
        = pb::FieldCodec.ForInt32(10);
    private readonly pbc::RepeatedField<int> suits_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///套装信息
    /// </summary>
    public pbc::RepeatedField<int> suits {
      get { return suits_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      suits_.WriteTo(output, _repeated_suits_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += suits_.CalculateSize(_repeated_suits_codec);
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
            suits_.AddEntriesFrom(input, _repeated_suits_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class PutOnMemoryEquip : pb::IMessage {
    private static readonly pb::MessageParser<PutOnMemoryEquip> _parser = new pb::MessageParser<PutOnMemoryEquip>(() => new PutOnMemoryEquip());
    public static pb::MessageParser<PutOnMemoryEquip> Parser { get { return _parser; } }

    private long lid_;
    /// <summary>
    /// 唯一ID
    /// </summary>
    public long lid {
      get { return lid_; }
      set {
        lid_ = value;
      }
    }

    private long oldLid_;
    /// <summary>
    /// 替换的唯一ID
    /// </summary>
    public long oldLid {
      get { return oldLid_; }
      set {
        oldLid_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (lid != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(lid);
      }
      if (oldLid != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(oldLid);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (lid != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(lid);
      }
      if (oldLid != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(oldLid);
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
            lid = input.ReadInt64();
            break;
          }
          case 16: {
            oldLid = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  public sealed class ReqMemoryEquipLevelUp : pb::IMessage {
    private static readonly pb::MessageParser<ReqMemoryEquipLevelUp> _parser = new pb::MessageParser<ReqMemoryEquipLevelUp>(() => new ReqMemoryEquipLevelUp());
    public static pb::MessageParser<ReqMemoryEquipLevelUp> Parser { get { return _parser; } }

    private long lid_;
    /// <summary>
    /// 唯一ID
    /// </summary>
    public long lid {
      get { return lid_; }
      set {
        lid_ = value;
      }
    }

    private static readonly pb::FieldCodec<long> _repeated_eatIds_codec
        = pb::FieldCodec.ForInt64(26);
    private readonly pbc::RepeatedField<long> eatIds_ = new pbc::RepeatedField<long>();
    /// <summary>
    ///吃掉的装备
    /// </summary>
    public pbc::RepeatedField<long> eatIds {
      get { return eatIds_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (lid != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(lid);
      }
      eatIds_.WriteTo(output, _repeated_eatIds_codec);
    }

    public int CalculateSize() {
      int size = 0;
      if (lid != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(lid);
      }
      size += eatIds_.CalculateSize(_repeated_eatIds_codec);
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
            lid = input.ReadInt64();
            break;
          }
          case 26:
          case 24: {
            eatIds_.AddEntriesFrom(input, _repeated_eatIds_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class UnlockMemoryEquipGezi : pb::IMessage {
    private static readonly pb::MessageParser<UnlockMemoryEquipGezi> _parser = new pb::MessageParser<UnlockMemoryEquipGezi>(() => new UnlockMemoryEquipGezi());
    public static pb::MessageParser<UnlockMemoryEquipGezi> Parser { get { return _parser; } }

    public void WriteTo(pb::CodedOutputStream output) {
    }

    public int CalculateSize() {
      int size = 0;
      return size;
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
        }
      }
    }

  }

  public sealed class MemoryEquipGeziChange : pb::IMessage {
    private static readonly pb::MessageParser<MemoryEquipGeziChange> _parser = new pb::MessageParser<MemoryEquipGeziChange>(() => new MemoryEquipGeziChange());
    public static pb::MessageParser<MemoryEquipGeziChange> Parser { get { return _parser; } }

    private int gezi_;
    /// <summary>
    ///格子
    /// </summary>
    public int gezi {
      get { return gezi_; }
      set {
        gezi_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (gezi != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(gezi);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (gezi != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(gezi);
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
            gezi = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class MemoryEquipId : pb::IMessage {
    private static readonly pb::MessageParser<MemoryEquipId> _parser = new pb::MessageParser<MemoryEquipId>(() => new MemoryEquipId());
    public static pb::MessageParser<MemoryEquipId> Parser { get { return _parser; } }

    private long lid_;
    /// <summary>
    /// 唯一ID
    /// </summary>
    public long lid {
      get { return lid_; }
      set {
        lid_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (lid != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(lid);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (lid != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(lid);
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
            lid = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  public sealed class MemorySummonTeam : pb::IMessage {
    private static readonly pb::MessageParser<MemorySummonTeam> _parser = new pb::MessageParser<MemorySummonTeam>(() => new MemorySummonTeam());
    public static pb::MessageParser<MemorySummonTeam> Parser { get { return _parser; } }

    private long rid_;
    /// <summary>
    /// 召唤者ID
    /// </summary>
    public long rid {
      get { return rid_; }
      set {
        rid_ = value;
      }
    }

    private string name_ = "";
    /// <summary>
    /// 召唤者名字
    /// </summary>
    public string name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    private int mapId_;
    /// <summary>
    /// 地图表配置ID
    /// </summary>
    public int mapId {
      get { return mapId_; }
      set {
        mapId_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (rid != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(rid);
      }
      if (name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(name);
      }
      if (mapId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(mapId);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (rid != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(rid);
      }
      if (name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(name);
      }
      if (mapId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(mapId);
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
            rid = input.ReadInt64();
            break;
          }
          case 18: {
            name = input.ReadString();
            break;
          }
          case 24: {
            mapId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class MemoryGotoSummon : pb::IMessage {
    private static readonly pb::MessageParser<MemoryGotoSummon> _parser = new pb::MessageParser<MemoryGotoSummon>(() => new MemoryGotoSummon());
    public static pb::MessageParser<MemoryGotoSummon> Parser { get { return _parser; } }

    private long rid_;
    /// <summary>
    /// 召唤者ID
    /// </summary>
    public long rid {
      get { return rid_; }
      set {
        rid_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (rid != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(rid);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (rid != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(rid);
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
            rid = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  public sealed class MemorySummonTeamCd : pb::IMessage {
    private static readonly pb::MessageParser<MemorySummonTeamCd> _parser = new pb::MessageParser<MemorySummonTeamCd>(() => new MemorySummonTeamCd());
    public static pb::MessageParser<MemorySummonTeamCd> Parser { get { return _parser; } }

    private long nextSummonTime_;
    /// <summary>
    /// 下次可召唤时间
    /// </summary>
    public long nextSummonTime {
      get { return nextSummonTime_; }
      set {
        nextSummonTime_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (nextSummonTime != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(nextSummonTime);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (nextSummonTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(nextSummonTime);
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
            nextSummonTime = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code