namespace TABLE
{
	public partial class ITEMCALLBACK
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
		public int quality
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
		public int levClass
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
		public int subTypePtr
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
		public IntArray subType
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = subTypePtr & 0xFFFFF;
				array.__length = (subTypePtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int para1
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
		public int para2
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
	}
	public static class ITEMCALLBACKHelper
	{
		public static void Encode(this System.IO.Stream stream,ITEMCALLBACK item)
		{
			stream.Encode(item.id);
			stream.Encode(item.type);
			stream.Encode(item.quality);
			stream.Encode(item.levClass);
			stream.Encode(item.subTypePtr);
			stream.Encode(item.para1);
			stream.Encode(item.para2);
		}
	}
	public class ITEMCALLBACKARRAY  : ILFastMode
	{
		public ITEMCALLBACKARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 7;
			StringValueFixedLength = 0;
			Rules = new byte[]{1,1,1,1,1,1,1};
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
				TABLE.ITEMCALLBACK randAttr = new TABLE.ITEMCALLBACK();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
