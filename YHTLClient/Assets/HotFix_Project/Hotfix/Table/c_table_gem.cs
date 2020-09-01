namespace TABLE
{
	public partial class GEM
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
		public int lv
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
		public int itemIdPtr
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
		public LongArray itemId
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = itemIdPtr & 0xFFFFF;
				array.__length = (itemIdPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int zsattrPtr
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
				return __data.intValues[5 + handle.intOffset];
			}
			set
			{
				__data.intValues[5 + handle.intOffset] = value;
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
				return __data.intValues[6 + handle.intOffset];
			}
			set
			{
				__data.intValues[6 + handle.intOffset] = value;
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
		public int position
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
	}
	public static class GEMHelper
	{
		public static void Encode(this System.IO.Stream stream,GEM item)
		{
			stream.Encode(item.id);
			stream.Encode(item.name);
			stream.Encode(item.lv);
			stream.Encode(item.costPtr);
			stream.Encode(item.itemIdPtr);
			stream.Encode(item.zsattrPtr);
			stream.Encode(item.fsattrPtr);
			stream.Encode(item.dsattrPtr);
			stream.Encode(item.position);
		}
	}
	public class GEMARRAY  : ILFastMode
	{
		public GEMARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 8;
			StringValueFixedLength = 1;
			Rules = new byte[]{1,2,1,1,1,1,1,1,1};
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
				TABLE.GEM randAttr = new TABLE.GEM();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
