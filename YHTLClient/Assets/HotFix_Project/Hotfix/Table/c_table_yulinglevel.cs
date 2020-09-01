namespace TABLE
{
	public partial class YULINGLEVEL
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
		public int levelUpPtr
		{
			get
			{
				return __data.intValues[1 + handle.intOffset];
			}
			set
			{
				__data.intValues[1 + handle.intOffset] = value;
			}
		}
		public LongArray levelUp
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = levelUpPtr & 0xFFFFF;
				array.__length = (levelUpPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int zsattrPtr
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
				return __data.intValues[3 + handle.intOffset];
			}
			set
			{
				__data.intValues[3 + handle.intOffset] = value;
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
				return __data.intValues[4 + handle.intOffset];
			}
			set
			{
				__data.intValues[4 + handle.intOffset] = value;
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
		public string exattr
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
		public int exType
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
		public int limit
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
	public static class YULINGLEVELHelper
	{
		public static void Encode(this System.IO.Stream stream,YULINGLEVEL item)
		{
			stream.Encode(item.id);
			stream.Encode(item.levelUpPtr);
			stream.Encode(item.zsattrPtr);
			stream.Encode(item.fsattrPtr);
			stream.Encode(item.dsattrPtr);
			stream.Encode(item.exattr);
			stream.Encode(item.exType);
			stream.Encode(item.limit);
		}
	}
	public class YULINGLEVELARRAY  : ILFastMode
	{
		public YULINGLEVELARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 7;
			StringValueFixedLength = 1;
			Rules = new byte[]{1,1,1,1,1,2,1,1};
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
				TABLE.YULINGLEVEL randAttr = new TABLE.YULINGLEVEL();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
