namespace TABLE
{
	public partial class BAOZHUSKILLLIB
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
		public int libID
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
		public int skill
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
	}
	public static class BAOZHUSKILLLIBHelper
	{
		public static void Encode(this System.IO.Stream stream,BAOZHUSKILLLIB item)
		{
			stream.Encode(item.id);
			stream.Encode(item.libID);
			stream.Encode(item.skill);
		}
	}
	public class BAOZHUSKILLLIBARRAY  : ILFastMode
	{
		public BAOZHUSKILLLIBARRAY()
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
				TABLE.BAOZHUSKILLLIB randAttr = new TABLE.BAOZHUSKILLLIB();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}