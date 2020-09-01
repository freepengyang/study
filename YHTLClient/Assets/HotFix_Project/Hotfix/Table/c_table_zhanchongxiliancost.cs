namespace TABLE
{
	public partial class ZHANCHONGXILIANCOST
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
		public int levClass
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
		public string hunlicost
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
		public string hunjicost
		{
			get
			{
				return __data.stringValues[1 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[1 + handle.stringOffset] = value;
			}
		}
		public string hunzhicost
		{
			get
			{
				return __data.stringValues[2 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[2 + handle.stringOffset] = value;
			}
		}
	}
	public static class ZHANCHONGXILIANCOSTHelper
	{
		public static void Encode(this System.IO.Stream stream,ZHANCHONGXILIANCOST item)
		{
			stream.Encode(item.id);
			stream.Encode(item.levClass);
			stream.Encode(item.hunlicost);
			stream.Encode(item.hunjicost);
			stream.Encode(item.hunzhicost);
		}
	}
	public class ZHANCHONGXILIANCOSTARRAY  : ILFastMode
	{
		public ZHANCHONGXILIANCOSTARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 2;
			StringValueFixedLength = 3;
			Rules = new byte[]{1,1,2,2,2};
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
				TABLE.ZHANCHONGXILIANCOST randAttr = new TABLE.ZHANCHONGXILIANCOST();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
