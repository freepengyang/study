namespace TABLE
{
	public partial class PRIZEPOOL
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
		public uint type
		{
			get
			{
				 return (uint)__data.intValues[1 + handle.intOffset];
			}
			set
			{
				__data.intValues[1 + handle.intOffset] = (int)value;
			}
		}
		public uint itemId
		{
			get
			{
				 return (uint)__data.intValues[2 + handle.intOffset];
			}
			set
			{
				__data.intValues[2 + handle.intOffset] = (int)value;
			}
		}
		public uint num
		{
			get
			{
				 return (uint)__data.intValues[3 + handle.intOffset];
			}
			set
			{
				__data.intValues[3 + handle.intOffset] = (int)value;
			}
		}
	}
	public static class PRIZEPOOLHelper
	{
		public static void Encode(this System.IO.Stream stream,PRIZEPOOL item)
		{
			stream.Encode(item.id);
			stream.Encode(item.type);
			stream.Encode(item.itemId);
			stream.Encode(item.num);
		}
	}
	public class PRIZEPOOLARRAY  : ILFastMode
	{
		public PRIZEPOOLARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 4;
			StringValueFixedLength = 0;
			Rules = new byte[]{1,1,1,1};
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
				TABLE.PRIZEPOOL randAttr = new TABLE.PRIZEPOOL();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
