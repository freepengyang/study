namespace TABLE
{
	public partial class HUANCAI
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
		public int itemId
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
		public int zsattrParaPtr
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
		public IntArray zsattrPara
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = zsattrParaPtr & 0xFFFFF;
				array.__length = (zsattrParaPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int fsattrParaPtr
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
		public IntArray fsattrPara
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = fsattrParaPtr & 0xFFFFF;
				array.__length = (fsattrParaPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int dsattrParaPtr
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
		public IntArray dsattrPara
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = dsattrParaPtr & 0xFFFFF;
				array.__length = (dsattrParaPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int attrNumPtr
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
		public IntArray attrNum
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = attrNumPtr & 0xFFFFF;
				array.__length = (attrNumPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int model
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
	public static class HUANCAIHelper
	{
		public static void Encode(this System.IO.Stream stream,HUANCAI item)
		{
			stream.Encode(item.id);
			stream.Encode(item.itemId);
			stream.Encode(item.name);
			stream.Encode(item.zsattrParaPtr);
			stream.Encode(item.fsattrParaPtr);
			stream.Encode(item.dsattrParaPtr);
			stream.Encode(item.attrNumPtr);
			stream.Encode(item.model);
		}
	}
	public class HUANCAIARRAY  : ILFastMode
	{
		public HUANCAIARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 7;
			StringValueFixedLength = 1;
			Rules = new byte[]{1,1,2,1,1,1,1,1};
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
				TABLE.HUANCAI randAttr = new TABLE.HUANCAI();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
