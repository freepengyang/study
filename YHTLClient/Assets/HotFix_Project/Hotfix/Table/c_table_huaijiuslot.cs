namespace TABLE
{
	public partial class HUAIJIUSLOT
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
		public int paramPtr
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
		public LongArray param
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = paramPtr & 0xFFFFF;
				array.__length = (paramPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class HUAIJIUSLOTHelper
	{
		public static void Encode(this System.IO.Stream stream,HUAIJIUSLOT item)
		{
			stream.Encode(item.id);
			stream.Encode(item.type);
			stream.Encode(item.paramPtr);
		}
	}
	public class HUAIJIUSLOTARRAY  : ILFastMode
	{
		public HUAIJIUSLOTARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 3;
			StringValueFixedLength = 0;
			Rules = new byte[]{1,1,1};
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
				TABLE.HUAIJIUSLOT randAttr = new TABLE.HUAIJIUSLOT();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
