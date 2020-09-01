namespace TABLE
{
	public partial class INSTANCE
	{
		public TableHandle handle;
		public TableData __data;
		public ILFastMode Array;
		public int id
		{
			get
			{
				 return (int)__data.intValues[0 + handle.intOffset];
			}
			set
			{
				__data.intValues[0 + handle.intOffset] = (int)value;
			}
		}
		public int mapId
		{
			get
			{
				 return (int)__data.intValues[1 + handle.intOffset];
			}
			set
			{
				__data.intValues[1 + handle.intOffset] = (int)value;
			}
		}
		public int groupid
		{
			get
			{
				 return (int)__data.intValues[2 + handle.intOffset];
			}
			set
			{
				__data.intValues[2 + handle.intOffset] = (int)value;
			}
		}
		public int type
		{
			get
			{
				 return (int)__data.intValues[3 + handle.intOffset];
			}
			set
			{
				__data.intValues[3 + handle.intOffset] = (int)value;
			}
		}
		public int level
		{
			get
			{
				 return (int)__data.intValues[4 + handle.intOffset];
			}
			set
			{
				__data.intValues[4 + handle.intOffset] = (int)value;
			}
		}
		public int openLevel
		{
			get
			{
				 return (int)__data.intValues[5 + handle.intOffset];
			}
			set
			{
				__data.intValues[5 + handle.intOffset] = (int)value;
			}
		}
		public int openLevelMax
		{
			get
			{
				 return (int)__data.intValues[6 + handle.intOffset];
			}
			set
			{
				__data.intValues[6 + handle.intOffset] = (int)value;
			}
		}
		public int openLianTiLevel
		{
			get
			{
				 return (int)__data.intValues[7 + handle.intOffset];
			}
			set
			{
				__data.intValues[7 + handle.intOffset] = (int)value;
			}
		}
		public int reincarnation
		{
			get
			{
				 return (int)__data.intValues[8 + handle.intOffset];
			}
			set
			{
				__data.intValues[8 + handle.intOffset] = (int)value;
			}
		}
		public int maxLevel
		{
			get
			{
				 return (int)__data.intValues[9 + handle.intOffset];
			}
			set
			{
				__data.intValues[9 + handle.intOffset] = (int)value;
			}
		}
		public int canRelive
		{
			get
			{
				 return (int)__data.intValues[10 + handle.intOffset];
			}
			set
			{
				__data.intValues[10 + handle.intOffset] = (int)value;
			}
		}
		public int reliveTimes
		{
			get
			{
				 return (int)__data.intValues[11 + handle.intOffset];
			}
			set
			{
				__data.intValues[11 + handle.intOffset] = (int)value;
			}
		}
		public int vip
		{
			get
			{
				 return (int)__data.intValues[12 + handle.intOffset];
			}
			set
			{
				__data.intValues[12 + handle.intOffset] = (int)value;
			}
		}
		public string requireItems
		{
			get
			{
				return __data.stringValues[0 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[0 + handle.stringOffset] = value;
			}
		}
		public int fengmo
		{
			get
			{
				 return (int)__data.intValues[13 + handle.intOffset];
			}
			set
			{
				__data.intValues[13 + handle.intOffset] = (int)value;
			}
		}
		public string openTime
		{
			get
			{
				return __data.stringValues[1 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[1 + handle.stringOffset] = value;
			}
		}
		public int limitTimes
		{
			get
			{
				 return (int)__data.intValues[14 + handle.intOffset];
			}
			set
			{
				__data.intValues[14 + handle.intOffset] = (int)value;
			}
		}
		public int totalTime
		{
			get
			{
				 return (int)__data.intValues[15 + handle.intOffset];
			}
			set
			{
				__data.intValues[15 + handle.intOffset] = (int)value;
			}
		}
		public string rewards
		{
			get
			{
				return __data.stringValues[2 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[2 + handle.stringOffset] = value;
			}
		}
		public string conditionSuccess
		{
			get
			{
				return __data.stringValues[3 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[3 + handle.stringOffset] = value;
			}
		}
		public string finishAction
		{
			get
			{
				return __data.stringValues[4 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[4 + handle.stringOffset] = value;
			}
		}
		public string tips
		{
			get
			{
				return __data.stringValues[5 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[5 + handle.stringOffset] = value;
			}
		}
		public int finishCount
		{
			get
			{
				 return (int)__data.intValues[16 + handle.intOffset];
			}
			set
			{
				__data.intValues[16 + handle.intOffset] = (int)value;
			}
		}
		public int autoFight
		{
			get
			{
				 return (int)__data.intValues[17 + handle.intOffset];
			}
			set
			{
				__data.intValues[17 + handle.intOffset] = (int)value;
			}
		}
		public string show
		{
			get
			{
				return __data.stringValues[6 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[6 + handle.stringOffset] = value;
			}
		}
		public int canBuy
		{
			get
			{
				 return (int)__data.intValues[18 + handle.intOffset];
			}
			set
			{
				__data.intValues[18 + handle.intOffset] = (int)value;
			}
		}
		public int price
		{
			get
			{
				 return (int)__data.intValues[19 + handle.intOffset];
			}
			set
			{
				__data.intValues[19 + handle.intOffset] = (int)value;
			}
		}
		public string multiReward
		{
			get
			{
				return __data.stringValues[7 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[7 + handle.stringOffset] = value;
			}
		}
		public int hero
		{
			get
			{
				 return (int)__data.intValues[20 + handle.intOffset];
			}
			set
			{
				__data.intValues[20 + handle.intOffset] = (int)value;
			}
		}
		public string mapName
		{
			get
			{
				return __data.stringValues[8 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[8 + handle.stringOffset] = value;
			}
		}
		public string img
		{
			get
			{
				return __data.stringValues[9 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[9 + handle.stringOffset] = value;
			}
		}
		public int recommendFightCap
		{
			get
			{
				 return (int)__data.intValues[21 + handle.intOffset];
			}
			set
			{
				__data.intValues[21 + handle.intOffset] = (int)value;
			}
		}
		public int canTransfer
		{
			get
			{
				 return (int)__data.intValues[22 + handle.intOffset];
			}
			set
			{
				__data.intValues[22 + handle.intOffset] = (int)value;
			}
		}
		public int openControl
		{
			get
			{
				 return (int)__data.intValues[23 + handle.intOffset];
			}
			set
			{
				__data.intValues[23 + handle.intOffset] = (int)value;
			}
		}
		public int privilege
		{
			get
			{
				 return (int)__data.intValues[24 + handle.intOffset];
			}
			set
			{
				__data.intValues[24 + handle.intOffset] = (int)value;
			}
		}
		public string increaseRein
		{
			get
			{
				return __data.stringValues[10 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[10 + handle.stringOffset] = value;
			}
		}
		public string requireItemsExtra
		{
			get
			{
				return __data.stringValues[11 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[11 + handle.stringOffset] = value;
			}
		}
		public int timerId
		{
			get
			{
				 return (int)__data.intValues[25 + handle.intOffset];
			}
			set
			{
				__data.intValues[25 + handle.intOffset] = (int)value;
			}
		}
		public int param
		{
			get
			{
				 return (int)__data.intValues[26 + handle.intOffset];
			}
			set
			{
				__data.intValues[26 + handle.intOffset] = (int)value;
			}
		}
	}
	public static class INSTANCEHelper
	{
		public static void Encode(this System.IO.Stream stream,INSTANCE item)
		{
			stream.Encode(item.id);
			stream.Encode(item.mapId);
			stream.Encode(item.groupid);
			stream.Encode(item.type);
			stream.Encode(item.level);
			stream.Encode(item.openLevel);
			stream.Encode(item.openLevelMax);
			stream.Encode(item.openLianTiLevel);
			stream.Encode(item.reincarnation);
			stream.Encode(item.maxLevel);
			stream.Encode(item.canRelive);
			stream.Encode(item.reliveTimes);
			stream.Encode(item.vip);
			stream.Encode(item.requireItems);
			stream.Encode(item.fengmo);
			stream.Encode(item.openTime);
			stream.Encode(item.limitTimes);
			stream.Encode(item.totalTime);
			stream.Encode(item.rewards);
			stream.Encode(item.conditionSuccess);
			stream.Encode(item.finishAction);
			stream.Encode(item.tips);
			stream.Encode(item.finishCount);
			stream.Encode(item.autoFight);
			stream.Encode(item.show);
			stream.Encode(item.canBuy);
			stream.Encode(item.price);
			stream.Encode(item.multiReward);
			stream.Encode(item.hero);
			stream.Encode(item.mapName);
			stream.Encode(item.img);
			stream.Encode(item.recommendFightCap);
			stream.Encode(item.canTransfer);
			stream.Encode(item.openControl);
			stream.Encode(item.privilege);
			stream.Encode(item.increaseRein);
			stream.Encode(item.requireItemsExtra);
			stream.Encode(item.timerId);
			stream.Encode(item.param);
		}
	}
	public class INSTANCEARRAY  : ILFastMode
	{
		public INSTANCEARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 27;
			StringValueFixedLength = 12;
			Rules = new byte[]{1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,2,1,1,2,2,2,2,1,1,2,1,1,2,1,2,2,1,1,1,1,2,2,1,1};
		}
		public override void Decode(byte[] contents)
		{
			gItem = GDecoder.LoadTable(contents, this.Rules);
			this.VarIntValues = GDecoder.varIntValues;
			this.VarStringValues = GDecoder.varStringValues;
			this.VarLongValues = GDecoder.varLongValues;
			var handles = gItem.handles;
			TableHandle handle = null;
			for (int i = 0, max = handles.Length; i < max; ++i)
			{
				TABLE.INSTANCE randAttr = new TABLE.INSTANCE();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
