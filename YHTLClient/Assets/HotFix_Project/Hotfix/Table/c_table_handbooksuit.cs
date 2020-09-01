namespace TABLE
{
	public partial class HANDBOOKSUIT
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
		public int judgeType
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
		public int judgeValue
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
		public int requirenum
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
		public string pic
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
		public string effectpic
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
		public int parameterPtr
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
		public IntArray parameter
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = parameterPtr & 0xFFFFF;
				array.__length = (parameterPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int factorPtr
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
		public IntArray factor
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = factorPtr & 0xFFFFF;
				array.__length = (factorPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class HANDBOOKSUITHelper
	{
		public static void Encode(this System.IO.Stream stream,HANDBOOKSUIT item)
		{
			stream.Encode(item.id);
			stream.Encode(item.name);
			stream.Encode(item.judgeType);
			stream.Encode(item.judgeValue);
			stream.Encode(item.requirenum);
			stream.Encode(item.pic);
			stream.Encode(item.effectpic);
			stream.Encode(item.parameterPtr);
			stream.Encode(item.factorPtr);
		}
	}
	public class HANDBOOKSUITARRAY  : ILFastMode
	{
		public HANDBOOKSUITARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 6;
			StringValueFixedLength = 3;
			Rules = new byte[]{1,2,1,1,1,2,2,1,1};
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
				TABLE.HANDBOOKSUIT randAttr = new TABLE.HANDBOOKSUIT();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
