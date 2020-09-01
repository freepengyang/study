namespace TABLE
{
	public partial class GAMEVERSION
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
		public string version
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
	public static class GAMEVERSIONHelper
	{
		public static void Encode(this System.IO.Stream stream,GAMEVERSION item)
		{
			stream.Encode(item.id);
			stream.Encode(item.version);
		}
	}
	public class GAMEVERSIONARRAY  : ILFastMode
	{
		public GAMEVERSIONARRAY()
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
				TABLE.GAMEVERSION randAttr = new TABLE.GAMEVERSION();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
