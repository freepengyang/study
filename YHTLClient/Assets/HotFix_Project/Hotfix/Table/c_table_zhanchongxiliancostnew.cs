namespace TABLE
{
	public partial class ZHANCHONGXILIANCOSTNEW
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
		public int levClass
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
		public int hunlicostPtr
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
		public IntArray hunlicost
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = hunlicostPtr & 0xFFFFF;
				array.__length = (hunlicostPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int hunlicost1Ptr
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
		public IntArray hunlicost1
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = hunlicost1Ptr & 0xFFFFF;
				array.__length = (hunlicost1Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int hunjicostPtr
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
		public IntArray hunjicost
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = hunjicostPtr & 0xFFFFF;
				array.__length = (hunjicostPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int hunjicost1Ptr
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
		public IntArray hunjicost1
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = hunjicost1Ptr & 0xFFFFF;
				array.__length = (hunjicost1Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class ZHANCHONGXILIANCOSTNEWHelper
	{
		public static void Encode(this System.IO.Stream stream,ZHANCHONGXILIANCOSTNEW item)
		{
			stream.Encode(item.id);
			stream.Encode(item.levClass);
			stream.Encode(item.hunlicostPtr);
			stream.Encode(item.hunlicost1Ptr);
			stream.Encode(item.hunjicostPtr);
			stream.Encode(item.hunjicost1Ptr);
		}
	}
	public class ZHANCHONGXILIANCOSTNEWARRAY  : ILFastMode
	{
		public ZHANCHONGXILIANCOSTNEWARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 6;
			StringValueFixedLength = 0;
			Rules = new byte[]{1,1,1,1,1,1};
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
				TABLE.ZHANCHONGXILIANCOSTNEW randAttr = new TABLE.ZHANCHONGXILIANCOSTNEW();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
