namespace TABLE
{
	public partial class DROPSHOW
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
		public int itemId0Ptr
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
		public IntArray itemId0
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = itemId0Ptr & 0xFFFFF;
				array.__length = (itemId0Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int itemId1Ptr
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
		public IntArray itemId1
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = itemId1Ptr & 0xFFFFF;
				array.__length = (itemId1Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int itemId2Ptr
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
		public IntArray itemId2
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = itemId2Ptr & 0xFFFFF;
				array.__length = (itemId2Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int itemId3Ptr
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
		public IntArray itemId3
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = itemId3Ptr & 0xFFFFF;
				array.__length = (itemId3Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int itemId4Ptr
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
		public IntArray itemId4
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = itemId4Ptr & 0xFFFFF;
				array.__length = (itemId4Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int itemId5Ptr
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
		public IntArray itemId5
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = itemId5Ptr & 0xFFFFF;
				array.__length = (itemId5Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class DROPSHOWHelper
	{
		public static void Encode(this System.IO.Stream stream,DROPSHOW item)
		{
			stream.Encode(item.id);
			stream.Encode(item.itemId0Ptr);
			stream.Encode(item.itemId1Ptr);
			stream.Encode(item.itemId2Ptr);
			stream.Encode(item.itemId3Ptr);
			stream.Encode(item.itemId4Ptr);
			stream.Encode(item.itemId5Ptr);
		}
	}
	public class DROPSHOWARRAY  : ILFastMode
	{
		public DROPSHOWARRAY()
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
				TABLE.DROPSHOW randAttr = new TABLE.DROPSHOW();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
