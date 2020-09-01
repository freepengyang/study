namespace TABLE
{
	public partial class YULINGSOUL
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
		public int level
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
		public int costPtr
		{
			get
			{
				return __data.intValues[2 + handle.intOffset];
			}
			set
			{
				__data.intValues[2 + handle.intOffset] = value;
			}
		}
		public LongArray cost
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = costPtr & 0xFFFFF;
				array.__length = (costPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int zsattrPtr
		{
			get
			{
				return __data.intValues[3 + handle.intOffset];
			}
			set
			{
				__data.intValues[3 + handle.intOffset] = value;
			}
		}
		public LongArray zsattr
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = zsattrPtr & 0xFFFFF;
				array.__length = (zsattrPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int fsattrPtr
		{
			get
			{
				return __data.intValues[4 + handle.intOffset];
			}
			set
			{
				__data.intValues[4 + handle.intOffset] = value;
			}
		}
		public LongArray fsattr
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = fsattrPtr & 0xFFFFF;
				array.__length = (fsattrPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int dsattrPtr
		{
			get
			{
				return __data.intValues[5 + handle.intOffset];
			}
			set
			{
				__data.intValues[5 + handle.intOffset] = value;
			}
		}
		public LongArray dsattr
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = dsattrPtr & 0xFFFFF;
				array.__length = (dsattrPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int zsexattrPtr
		{
			get
			{
				return __data.intValues[6 + handle.intOffset];
			}
			set
			{
				__data.intValues[6 + handle.intOffset] = value;
			}
		}
		public LongArray zsexattr
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = zsexattrPtr & 0xFFFFF;
				array.__length = (zsexattrPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int fsexattrPtr
		{
			get
			{
				return __data.intValues[7 + handle.intOffset];
			}
			set
			{
				__data.intValues[7 + handle.intOffset] = value;
			}
		}
		public LongArray fsexattr
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = fsexattrPtr & 0xFFFFF;
				array.__length = (fsexattrPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int dsexattrPtr
		{
			get
			{
				return __data.intValues[8 + handle.intOffset];
			}
			set
			{
				__data.intValues[8 + handle.intOffset] = value;
			}
		}
		public LongArray dsexattr
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = dsexattrPtr & 0xFFFFF;
				array.__length = (dsexattrPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int exattr
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
		public int position
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
	}
	public static class YULINGSOULHelper
	{
		public static void Encode(this System.IO.Stream stream,YULINGSOUL item)
		{
			stream.Encode(item.id);
			stream.Encode(item.name);
			stream.Encode(item.level);
			stream.Encode(item.costPtr);
			stream.Encode(item.zsattrPtr);
			stream.Encode(item.fsattrPtr);
			stream.Encode(item.dsattrPtr);
			stream.Encode(item.zsexattrPtr);
			stream.Encode(item.fsexattrPtr);
			stream.Encode(item.dsexattrPtr);
			stream.Encode(item.exattr);
			stream.Encode(item.position);
		}
	}
	public class YULINGSOULARRAY  : ILFastMode
	{
		public YULINGSOULARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 11;
			StringValueFixedLength = 1;
			Rules = new byte[]{1,2,1,1,1,1,1,1,1,1,1,1};
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
				TABLE.YULINGSOUL randAttr = new TABLE.YULINGSOUL();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
