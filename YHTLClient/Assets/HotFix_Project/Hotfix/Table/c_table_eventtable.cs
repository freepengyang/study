namespace TABLE
{
	public partial class EVENTTABLE
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
		public uint icon
		{
			get
			{
				 return (uint)__data.intValues[1 + handle.intOffset];
			}
			set
			{
				__data.intValues[1 + handle.intOffset] = (int)value;
			}
		}
		public string deliverId
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
		public uint startTime
		{
			get
			{
				 return (uint)__data.intValues[2 + handle.intOffset];
			}
			set
			{
				__data.intValues[2 + handle.intOffset] = (int)value;
			}
		}
		public uint endTime
		{
			get
			{
				 return (uint)__data.intValues[3 + handle.intOffset];
			}
			set
			{
				__data.intValues[3 + handle.intOffset] = (int)value;
			}
		}
		public string day
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
		public string awards
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
		public string tips
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
		public string npcName
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
		public uint firstTime
		{
			get
			{
				 return (uint)__data.intValues[4 + handle.intOffset];
			}
			set
			{
				__data.intValues[4 + handle.intOffset] = (int)value;
			}
		}
		public string openTime
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
		public uint lastTime
		{
			get
			{
				 return (uint)__data.intValues[5 + handle.intOffset];
			}
			set
			{
				__data.intValues[5 + handle.intOffset] = (int)value;
			}
		}
		public string openDay
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
		public uint combineTime
		{
			get
			{
				 return (uint)__data.intValues[6 + handle.intOffset];
			}
			set
			{
				__data.intValues[6 + handle.intOffset] = (int)value;
			}
		}
		public string combineDay
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
		public string mapId
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
		public uint transportLimit
		{
			get
			{
				 return (uint)__data.intValues[7 + handle.intOffset];
			}
			set
			{
				__data.intValues[7 + handle.intOffset] = (int)value;
			}
		}
		public string pushOpen
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
		public uint attentionOpen
		{
			get
			{
				 return (uint)__data.intValues[8 + handle.intOffset];
			}
			set
			{
				__data.intValues[8 + handle.intOffset] = (int)value;
			}
		}
		public uint moren
		{
			get
			{
				 return (uint)__data.intValues[9 + handle.intOffset];
			}
			set
			{
				__data.intValues[9 + handle.intOffset] = (int)value;
			}
		}
		public uint needDiYuan
		{
			get
			{
				 return (uint)__data.intValues[10 + handle.intOffset];
			}
			set
			{
				__data.intValues[10 + handle.intOffset] = (int)value;
			}
		}
	}
	public static class EVENTTABLEHelper
	{
		public static void Encode(this System.IO.Stream stream,EVENTTABLE item)
		{
			stream.Encode(item.id);
			stream.Encode(item.icon);
			stream.Encode(item.deliverId);
			stream.Encode(item.startTime);
			stream.Encode(item.endTime);
			stream.Encode(item.day);
			stream.Encode(item.awards);
			stream.Encode(item.tips);
			stream.Encode(item.npcName);
			stream.Encode(item.firstTime);
			stream.Encode(item.openTime);
			stream.Encode(item.lastTime);
			stream.Encode(item.openDay);
			stream.Encode(item.combineTime);
			stream.Encode(item.combineDay);
			stream.Encode(item.mapId);
			stream.Encode(item.transportLimit);
			stream.Encode(item.pushOpen);
			stream.Encode(item.attentionOpen);
			stream.Encode(item.moren);
			stream.Encode(item.needDiYuan);
		}
	}
	public class EVENTTABLEARRAY  : ILFastMode
	{
		public EVENTTABLEARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 11;
			StringValueFixedLength = 10;
			Rules = new byte[]{1,1,2,1,1,2,2,2,2,1,2,1,2,1,2,2,1,2,1,1,1};
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
				TABLE.EVENTTABLE randAttr = new TABLE.EVENTTABLE();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
