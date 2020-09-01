namespace TABLE
{
	public partial class CHONGWUSHUXING
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
		public int num
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
	public static class CHONGWUSHUXINGHelper
	{
		public static void Encode(this System.IO.Stream stream,CHONGWUSHUXING item)
		{
			stream.Encode(item.id);
			stream.Encode(item.name);
			stream.Encode(item.num);
		}
	}
	public class CHONGWUSHUXINGARRAY  : ILFastMode
	{
		public CHONGWUSHUXINGARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 2;
			StringValueFixedLength = 1;
			Rules = new byte[]{1,2,1};
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
				TABLE.CHONGWUSHUXING randAttr = new TABLE.CHONGWUSHUXING();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
