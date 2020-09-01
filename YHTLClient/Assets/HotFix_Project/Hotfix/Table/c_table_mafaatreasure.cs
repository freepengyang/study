namespace TABLE
{
	public partial class MAFAATREASURE
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
		public int descid
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
		public int boxlevel
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
		public int library1Ptr
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
		public IntArray library1
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = library1Ptr & 0xFFFFF;
				array.__length = (library1Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int librar2Ptr
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
		public IntArray librar2
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = librar2Ptr & 0xFFFFF;
				array.__length = (librar2Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int mailId
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
	}
	public static class MAFAATREASUREHelper
	{
		public static void Encode(this System.IO.Stream stream,MAFAATREASURE item)
		{
			stream.Encode(item.id);
			stream.Encode(item.descid);
			stream.Encode(item.boxlevel);
			stream.Encode(item.library1Ptr);
			stream.Encode(item.librar2Ptr);
			stream.Encode(item.mailId);
		}
	}
	public class MAFAATREASUREARRAY  : ILFastMode
	{
		public MAFAATREASUREARRAY()
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
				TABLE.MAFAATREASURE randAttr = new TABLE.MAFAATREASURE();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
