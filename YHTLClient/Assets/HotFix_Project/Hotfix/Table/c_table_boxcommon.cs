namespace TABLE
{
	public partial class BOXCOMMON
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
	}
	public static class BOXCOMMONHelper
	{
		public static void Encode(this System.IO.Stream stream,BOXCOMMON item)
		{
			stream.Encode(item.id);
		}
	}
	public class BOXCOMMONARRAY  : ILFastMode
	{
		public BOXCOMMONARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 1;
			StringValueFixedLength = 0;
			Rules = new byte[]{1};
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
				TABLE.BOXCOMMON randAttr = new TABLE.BOXCOMMON();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
