namespace TABLE
{
	public partial class DELIVER
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
		public int DeliverType
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
		public string tip
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
		public int deliverParameter
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
		public int x
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
		public int y
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
		public int deliverClass
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
		public string posDesc
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
		public int itemPtr
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
		public IntArray item
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = itemPtr & 0xFFFFF;
				array.__length = (itemPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int level
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
		public int level2
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
		public string level3
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
		public int vip
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
		public int woLongLevel
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
		public int timeOpen
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
		public int mainTaskIdPtr
		{
			get
			{
				return __data.intValues[12 + handle.intOffset];
			}
			set
			{
				__data.intValues[12 + handle.intOffset] = value;
			}
		}
		public StringArray mainTaskId
		{
			get
			{
				StringArray array;
				array.__fastMode = this.Array;
				array.__start = mainTaskIdPtr & 0xFFFFF;
				array.__length = (mainTaskIdPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int openCombineTime
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
	}
	public static class DELIVERHelper
	{
		public static void Encode(this System.IO.Stream stream,DELIVER item)
		{
			stream.Encode(item.id);
			stream.Encode(item.DeliverType);
			stream.Encode(item.tip);
			stream.Encode(item.deliverParameter);
			stream.Encode(item.x);
			stream.Encode(item.y);
			stream.Encode(item.deliverClass);
			stream.Encode(item.posDesc);
			stream.Encode(item.itemPtr);
			stream.Encode(item.level);
			stream.Encode(item.level2);
			stream.Encode(item.level3);
			stream.Encode(item.vip);
			stream.Encode(item.woLongLevel);
			stream.Encode(item.timeOpen);
			stream.Encode(item.mainTaskIdPtr);
			stream.Encode(item.openCombineTime);
		}
	}
	public class DELIVERARRAY  : ILFastMode
	{
		public DELIVERARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 14;
			StringValueFixedLength = 3;
			Rules = new byte[]{1,1,2,1,1,1,1,2,1,1,1,2,1,1,1,1,1};
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
				TABLE.DELIVER randAttr = new TABLE.DELIVER();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
