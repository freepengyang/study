namespace TABLE
{
	public partial class BOSSDROP
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
		public int mid
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
		public int drop
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
		public int comDrop
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
		public int extraDropPtr
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
		public IntArray extraDrop
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = extraDropPtr & 0xFFFFF;
				array.__length = (extraDropPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int firstKillDrop
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
	public static class BOSSDROPHelper
	{
		public static void Encode(this System.IO.Stream stream,BOSSDROP item)
		{
			stream.Encode(item.id);
			stream.Encode(item.mid);
			stream.Encode(item.name);
			stream.Encode(item.drop);
			stream.Encode(item.comDrop);
			stream.Encode(item.extraDropPtr);
			stream.Encode(item.firstKillDrop);
		}
	}
	public class BOSSDROPARRAY  : ILFastMode
	{
		public BOSSDROPARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 6;
			StringValueFixedLength = 1;
			Rules = new byte[]{1,1,2,1,1,1,1};
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
				TABLE.BOSSDROP randAttr = new TABLE.BOSSDROP();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
