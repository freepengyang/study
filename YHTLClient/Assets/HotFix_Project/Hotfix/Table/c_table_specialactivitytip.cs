namespace TABLE
{
	public partial class SPECIALACTIVITYTIP
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
		public string desc
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
		public string name
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
		public string link
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
		public int funcopen
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
		public int opendayPtr
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
		public IntArray openday
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = opendayPtr & 0xFFFFF;
				array.__length = (opendayPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class SPECIALACTIVITYTIPHelper
	{
		public static void Encode(this System.IO.Stream stream,SPECIALACTIVITYTIP item)
		{
			stream.Encode(item.id);
			stream.Encode(item.type);
			stream.Encode(item.desc);
			stream.Encode(item.name);
			stream.Encode(item.link);
			stream.Encode(item.funcopen);
			stream.Encode(item.opendayPtr);
		}
	}
	public class SPECIALACTIVITYTIPARRAY  : ILFastMode
	{
		public SPECIALACTIVITYTIPARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 4;
			StringValueFixedLength = 3;
			Rules = new byte[]{1,1,2,2,2,1,1};
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
				TABLE.SPECIALACTIVITYTIP randAttr = new TABLE.SPECIALACTIVITYTIP();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
