// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Auction.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using scg = global::System.Collections.Generic;
namespace auction {

  #region Messages
  public sealed class AllAuctionItems : pb::IMessage {
    private static readonly pb::MessageParser<AllAuctionItems> _parser = new pb::MessageParser<AllAuctionItems>(() => new AllAuctionItems());
    public static pb::MessageParser<AllAuctionItems> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<global::auction.AuctionItemInfo> _repeated_items_codec
        = pb::FieldCodec.ForMessage(10, global::auction.AuctionItemInfo.Parser);
    private readonly pbc::RepeatedField<global::auction.AuctionItemInfo> items_ = new pbc::RepeatedField<global::auction.AuctionItemInfo>();
    public pbc::RepeatedField<global::auction.AuctionItemInfo> items {
      get { return items_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      items_.WriteTo(output, _repeated_items_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += items_.CalculateSize(_repeated_items_codec);
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
            items_.AddEntriesFrom(input, _repeated_items_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class SelfAuctionItems : pb::IMessage {
    private static readonly pb::MessageParser<SelfAuctionItems> _parser = new pb::MessageParser<SelfAuctionItems>(() => new SelfAuctionItems());
    public static pb::MessageParser<SelfAuctionItems> Parser { get { return _parser; } }

    private int shelve_;
    /// <summary>
    /// 货架数量
    /// </summary>
    public int shelve {
      get { return shelve_; }
      set {
        shelve_ = value;
      }
    }

    private static readonly pb::FieldCodec<global::auction.AuctionItemInfo> _repeated_items_codec
        = pb::FieldCodec.ForMessage(18, global::auction.AuctionItemInfo.Parser);
    private readonly pbc::RepeatedField<global::auction.AuctionItemInfo> items_ = new pbc::RepeatedField<global::auction.AuctionItemInfo>();
    public pbc::RepeatedField<global::auction.AuctionItemInfo> items {
      get { return items_; }
    }

    private int taxRate_;
    /// <summary>
    /// 税率
    /// </summary>
    public int taxRate {
      get { return taxRate_; }
      set {
        taxRate_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (shelve != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(shelve);
      }
      items_.WriteTo(output, _repeated_items_codec);
      if (taxRate != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(taxRate);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (shelve != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(shelve);
      }
      size += items_.CalculateSize(_repeated_items_codec);
      if (taxRate != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(taxRate);
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
            shelve = input.ReadInt32();
            break;
          }
          case 18: {
            items_.AddEntriesFrom(input, _repeated_items_codec);
            break;
          }
          case 24: {
            taxRate = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class AuctionItemInfo : pb::IMessage {
    private static readonly pb::MessageParser<AuctionItemInfo> _parser = new pb::MessageParser<AuctionItemInfo>(() => new AuctionItemInfo());
    public static pb::MessageParser<AuctionItemInfo> Parser { get { return _parser; } }

    private long lid_;
    /// <summary>
    /// 交易物品唯一ID
    /// </summary>
    public long lid {
      get { return lid_; }
      set {
        lid_ = value;
      }
    }

    private long ownerId_;
    /// <summary>
    /// 上架者
    /// </summary>
    public long ownerId {
      get { return ownerId_; }
      set {
        ownerId_ = value;
      }
    }

    private global::bag.BagItemInfo item_;
    public global::bag.BagItemInfo item {
      get { return item_; }
      set {
        item_ = value;
      }
    }

    private int price_;
    public int price {
      get { return price_; }
      set {
        price_ = value;
      }
    }

    private long addTime_;
    /// <summary>
    ///上架时间
    /// </summary>
    public long addTime {
      get { return addTime_; }
      set {
        addTime_ = value;
      }
    }

    private long showTime_;
    /// <summary>
    ///展示时间
    /// </summary>
    public long showTime {
      get { return showTime_; }
      set {
        showTime_ = value;
      }
    }

    private int priceType_;
    /// <summary>
    /// 1：银子 2：金子
    /// </summary>
    public int priceType {
      get { return priceType_; }
      set {
        priceType_ = value;
      }
    }

    private global::tujian.TujianInfo tujianItem_;
    /// <summary>
    /// 图鉴
    /// </summary>
    public global::tujian.TujianInfo tujianItem {
      get { return tujianItem_; }
      set {
        tujianItem_ = value;
      }
    }

    private int itemType_;
    /// <summary>
    /// 1背包，2图鉴
    /// </summary>
    public int itemType {
      get { return itemType_; }
      set {
        itemType_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (lid != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(lid);
      }
      if (ownerId != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(ownerId);
      }
      if (item_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(item);
      }
      if (price != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(price);
      }
      if (addTime != 0L) {
        output.WriteRawTag(40);
        output.WriteInt64(addTime);
      }
      if (showTime != 0L) {
        output.WriteRawTag(48);
        output.WriteInt64(showTime);
      }
      if (priceType != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(priceType);
      }
      if (tujianItem_ != null) {
        output.WriteRawTag(66);
        output.WriteMessage(tujianItem);
      }
      if (itemType != 0) {
        output.WriteRawTag(72);
        output.WriteInt32(itemType);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (lid != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(lid);
      }
      if (ownerId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(ownerId);
      }
      if (item_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(item);
      }
      if (price != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(price);
      }
      if (addTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(addTime);
      }
      if (showTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(showTime);
      }
      if (priceType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(priceType);
      }
      if (tujianItem_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(tujianItem);
      }
      if (itemType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(itemType);
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
            ownerId = input.ReadInt64();
            break;
          }
          case 26: {
            if (item_ == null) {
              item_ = new global::bag.BagItemInfo();
            }
            input.ReadMessage(item_);
            break;
          }
          case 32: {
            price = input.ReadInt32();
            break;
          }
          case 40: {
            addTime = input.ReadInt64();
            break;
          }
          case 48: {
            showTime = input.ReadInt64();
            break;
          }
          case 56: {
            priceType = input.ReadInt32();
            break;
          }
          case 66: {
            if (tujianItem_ == null) {
              tujianItem_ = new global::tujian.TujianInfo();
            }
            input.ReadMessage(tujianItem_);
            break;
          }
          case 72: {
            itemType = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///上架请求
  /// </summary>
  public sealed class AddToShelfRequest : pb::IMessage {
    private static readonly pb::MessageParser<AddToShelfRequest> _parser = new pb::MessageParser<AddToShelfRequest>(() => new AddToShelfRequest());
    public static pb::MessageParser<AddToShelfRequest> Parser { get { return _parser; } }

    private int bagIndex_;
    public int bagIndex {
      get { return bagIndex_; }
      set {
        bagIndex_ = value;
      }
    }

    private int count_;
    public int count {
      get { return count_; }
      set {
        count_ = value;
      }
    }

    private int price_;
    public int price {
      get { return price_; }
      set {
        price_ = value;
      }
    }

    private long tujianId_;
    /// <summary>
    ///图鉴ID
    /// </summary>
    public long tujianId {
      get { return tujianId_; }
      set {
        tujianId_ = value;
      }
    }

    private int itemType_;
    /// <summary>
    /// 1背包，2图鉴
    /// </summary>
    public int itemType {
      get { return itemType_; }
      set {
        itemType_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (bagIndex != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(bagIndex);
      }
      if (count != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(count);
      }
      if (price != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(price);
      }
      if (tujianId != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(tujianId);
      }
      if (itemType != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(itemType);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (bagIndex != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(bagIndex);
      }
      if (count != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(count);
      }
      if (price != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(price);
      }
      if (tujianId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(tujianId);
      }
      if (itemType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(itemType);
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
            bagIndex = input.ReadInt32();
            break;
          }
          case 16: {
            count = input.ReadInt32();
            break;
          }
          case 24: {
            price = input.ReadInt32();
            break;
          }
          case 32: {
            tujianId = input.ReadInt64();
            break;
          }
          case 40: {
            itemType = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///上架响应
  /// </summary>
  public sealed class AddToShelfResponse : pb::IMessage {
    private static readonly pb::MessageParser<AddToShelfResponse> _parser = new pb::MessageParser<AddToShelfResponse>(() => new AddToShelfResponse());
    public static pb::MessageParser<AddToShelfResponse> Parser { get { return _parser; } }

    private int bagIndex_;
    public int bagIndex {
      get { return bagIndex_; }
      set {
        bagIndex_ = value;
      }
    }

    private global::auction.AuctionItemInfo item_;
    public global::auction.AuctionItemInfo item {
      get { return item_; }
      set {
        item_ = value;
      }
    }

    private long tujianId_;
    /// <summary>
    ///图鉴ID
    /// </summary>
    public long tujianId {
      get { return tujianId_; }
      set {
        tujianId_ = value;
      }
    }

    private int itemType_;
    /// <summary>
    /// 1背包，2图鉴
    /// </summary>
    public int itemType {
      get { return itemType_; }
      set {
        itemType_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (bagIndex != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(bagIndex);
      }
      if (item_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(item);
      }
      if (tujianId != 0L) {
        output.WriteRawTag(24);
        output.WriteInt64(tujianId);
      }
      if (itemType != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(itemType);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (bagIndex != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(bagIndex);
      }
      if (item_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(item);
      }
      if (tujianId != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(tujianId);
      }
      if (itemType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(itemType);
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
            bagIndex = input.ReadInt32();
            break;
          }
          case 18: {
            if (item_ == null) {
              item_ = new global::auction.AuctionItemInfo();
            }
            input.ReadMessage(item_);
            break;
          }
          case 24: {
            tujianId = input.ReadInt64();
            break;
          }
          case 32: {
            itemType = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///重新上架请求
  /// </summary>
  public sealed class ReqReAddToShelf : pb::IMessage {
    private static readonly pb::MessageParser<ReqReAddToShelf> _parser = new pb::MessageParser<ReqReAddToShelf>(() => new ReqReAddToShelf());
    public static pb::MessageParser<ReqReAddToShelf> Parser { get { return _parser; } }

    private long lid_;
    /// <summary>
    /// 交易物品唯一ID
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

  /// <summary>
  ///重新上架返回
  /// </summary>
  public sealed class ResReAddToShelf : pb::IMessage {
    private static readonly pb::MessageParser<ResReAddToShelf> _parser = new pb::MessageParser<ResReAddToShelf>(() => new ResReAddToShelf());
    public static pb::MessageParser<ResReAddToShelf> Parser { get { return _parser; } }

    private long lid_;
    /// <summary>
    /// 交易物品唯一ID
    /// </summary>
    public long lid {
      get { return lid_; }
      set {
        lid_ = value;
      }
    }

    private global::auction.AuctionItemInfo item_;
    public global::auction.AuctionItemInfo item {
      get { return item_; }
      set {
        item_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (lid != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(lid);
      }
      if (item_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(item);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (lid != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(lid);
      }
      if (item_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(item);
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
          case 18: {
            if (item_ == null) {
              item_ = new global::auction.AuctionItemInfo();
            }
            input.ReadMessage(item_);
            break;
          }
        }
      }
    }

  }

  public sealed class ItemIdMsg : pb::IMessage {
    private static readonly pb::MessageParser<ItemIdMsg> _parser = new pb::MessageParser<ItemIdMsg>(() => new ItemIdMsg());
    public static pb::MessageParser<ItemIdMsg> Parser { get { return _parser; } }

    private long lid_;
    public long lid {
      get { return lid_; }
      set {
        lid_ = value;
      }
    }

    private int count_;
    public int count {
      get { return count_; }
      set {
        count_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (lid != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(lid);
      }
      if (count != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(count);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (lid != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(lid);
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
            lid = input.ReadInt64();
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

  public sealed class UnlockAuctionShelve : pb::IMessage {
    private static readonly pb::MessageParser<UnlockAuctionShelve> _parser = new pb::MessageParser<UnlockAuctionShelve>(() => new UnlockAuctionShelve());
    public static pb::MessageParser<UnlockAuctionShelve> Parser { get { return _parser; } }

    private int shelve_;
    /// <summary>
    /// 货架数量
    /// </summary>
    public int shelve {
      get { return shelve_; }
      set {
        shelve_ = value;
      }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (shelve != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(shelve);
      }
    }

    public int CalculateSize() {
      int size = 0;
      if (shelve != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(shelve);
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
            shelve = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed class ItemIdList : pb::IMessage {
    private static readonly pb::MessageParser<ItemIdList> _parser = new pb::MessageParser<ItemIdList>(() => new ItemIdList());
    public static pb::MessageParser<ItemIdList> Parser { get { return _parser; } }

    private static readonly pb::FieldCodec<long> _repeated_lid_codec
        = pb::FieldCodec.ForInt64(10);
    private readonly pbc::RepeatedField<long> lid_ = new pbc::RepeatedField<long>();
    public pbc::RepeatedField<long> lid {
      get { return lid_; }
    }

    public void WriteTo(pb::CodedOutputStream output) {
      lid_.WriteTo(output, _repeated_lid_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += lid_.CalculateSize(_repeated_lid_codec);
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
            lid_.AddEntriesFrom(input, _repeated_lid_codec);
            break;
          }
        }
      }
    }

  }

  public sealed class ItemId : pb::IMessage {
    private static readonly pb::MessageParser<ItemId> _parser = new pb::MessageParser<ItemId>(() => new ItemId());
    public static pb::MessageParser<ItemId> Parser { get { return _parser; } }

    private long lid_;
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

  #endregion

}

#endregion Designer generated code
