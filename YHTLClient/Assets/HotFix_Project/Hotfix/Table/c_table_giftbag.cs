namespace TABLE
{
	public partial class GIFTBAG
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
		public string name
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
		public string desc
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
		public int type
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
		public int subType
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
		public int group
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
		public string groupName
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
		public int rewards
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
		public int limitType
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
		public int limitTime
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
		public int gainType
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
		public int para
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
		public int showPrice
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
		public int discount
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
		public int showPic
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
		public int tag1
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
	}
	public static class GIFTBAGHelper
	{
		public static void Encode(this System.IO.Stream stream,GIFTBAG item)
		{
			stream.Encode(item.id);
			stream.Encode(item.name);
			stream.Encode(item.desc);
			stream.Encode(item.type);
			stream.Encode(item.subType);
			stream.Encode(item.group);
			stream.Encode(item.groupName);
			stream.Encode(item.rewards);
			stream.Encode(item.limitType);
			stream.Encode(item.limitTime);
			stream.Encode(item.gainType);
			stream.Encode(item.para);
			stream.Encode(item.showPrice);
			stream.Encode(item.discount);
			stream.Encode(item.showPic);
			stream.Encode(item.tag1);
		}
	}
	public class GIFTBAGARRAY  : ILFastMode
	{
		public GIFTBAGARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 13;
			StringValueFixedLength = 3;
			Rules = new byte[]{1,2,2,1,1,1,2,1,1,1,1,1,1,1,1,1};
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
				TABLE.GIFTBAG randAttr = new TABLE.GIFTBAG();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
