namespace TABLE
{
	public partial class UNIONBUFF
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
		public int position
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
		public int costPtr
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
		public int attrPtr
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
		public LongArray attr
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = attrPtr & 0xFFFFF;
				array.__length = (attrPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class UNIONBUFFHelper
	{
		public static void Encode(this System.IO.Stream stream,UNIONBUFF item)
		{
			stream.Encode(item.id);
			stream.Encode(item.level);
			stream.Encode(item.position);
			stream.Encode(item.costPtr);
			stream.Encode(item.attrPtr);
		}
	}
	public class UNIONBUFFARRAY  : ILFastMode
	{
		public UNIONBUFFARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 5;
			StringValueFixedLength = 0;
			Rules = new byte[]{1,1,1,1,1};
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
				TABLE.UNIONBUFF randAttr = new TABLE.UNIONBUFF();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
