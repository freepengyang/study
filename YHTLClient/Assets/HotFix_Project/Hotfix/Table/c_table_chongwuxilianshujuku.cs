namespace TABLE
{
	public partial class CHONGWUXILIANSHUJUKU
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
		public int attrIdsPtr
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
		public IntArray attrIds
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = attrIdsPtr & 0xFFFFF;
				array.__length = (attrIdsPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public string description
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
		public int parameter1
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
		public int value1Ptr
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
		public IntArray value1
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = value1Ptr & 0xFFFFF;
				array.__length = (value1Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int parameter2
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
		public int value2Ptr
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
		public IntArray value2
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = value2Ptr & 0xFFFFF;
				array.__length = (value2Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int parameter3
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
		public int value3Ptr
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
		public IntArray value3
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = value3Ptr & 0xFFFFF;
				array.__length = (value3Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int parameter4
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
		public int value4Ptr
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
		public IntArray value4
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = value4Ptr & 0xFFFFF;
				array.__length = (value4Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class CHONGWUXILIANSHUJUKUHelper
	{
		public static void Encode(this System.IO.Stream stream,CHONGWUXILIANSHUJUKU item)
		{
			stream.Encode(item.id);
			stream.Encode(item.type);
			stream.Encode(item.attrIdsPtr);
			stream.Encode(item.description);
			stream.Encode(item.parameter1);
			stream.Encode(item.value1Ptr);
			stream.Encode(item.parameter2);
			stream.Encode(item.value2Ptr);
			stream.Encode(item.parameter3);
			stream.Encode(item.value3Ptr);
			stream.Encode(item.parameter4);
			stream.Encode(item.value4Ptr);
		}
	}
	public class CHONGWUXILIANSHUJUKUARRAY  : ILFastMode
	{
		public CHONGWUXILIANSHUJUKUARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 11;
			StringValueFixedLength = 1;
			Rules = new byte[]{1,1,1,2,1,1,1,1,1,1,1,1};
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
				TABLE.CHONGWUXILIANSHUJUKU randAttr = new TABLE.CHONGWUXILIANSHUJUKU();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
