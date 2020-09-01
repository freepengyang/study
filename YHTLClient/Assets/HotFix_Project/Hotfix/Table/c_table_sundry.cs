namespace TABLE
{
	public partial class SUNDRY
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
		public string effect
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
	}
	public static class SUNDRYHelper
	{
		public static void Encode(this System.IO.Stream stream,SUNDRY item)
		{
			stream.Encode(item.id);
			stream.Encode(item.effect);
		}
	}
	public class SUNDRYARRAY  : ILFastMode
	{
		public SUNDRYARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 1;
			StringValueFixedLength = 1;
			Rules = new byte[]{1,2};
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
				TABLE.SUNDRY randAttr = new TABLE.SUNDRY();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
