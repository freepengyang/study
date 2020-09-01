namespace TABLE
{
	public partial class WORLDCLASS
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
		public int guoQiDaHP
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
	}
	public static class WORLDCLASSHelper
	{
		public static void Encode(this System.IO.Stream stream,WORLDCLASS item)
		{
			stream.Encode(item.id);
			stream.Encode(item.guoQiDaHP);
		}
	}
	public class WORLDCLASSARRAY  : ILFastMode
	{
		public WORLDCLASSARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 2;
			StringValueFixedLength = 0;
			Rules = new byte[]{1,1};
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
				TABLE.WORLDCLASS randAttr = new TABLE.WORLDCLASS();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
