namespace TABLE
{
	public partial class CHONGWUXILIAN
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
		public int petLevClass
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
		public int getlntimes
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
		public int library
		{
			get
			{
				 return (int)__data.intValues[3 + handle.intOffset];
			}
			set
			{
				__data.intValues[3 + handle.intOffset] = (int)value;
			}
		}
	}
	public static class CHONGWUXILIANHelper
	{
		public static void Encode(this System.IO.Stream stream,CHONGWUXILIAN item)
		{
			stream.Encode(item.id);
			stream.Encode(item.petLevClass);
			stream.Encode(item.getlntimes);
			stream.Encode(item.library);
		}
	}
	public class CHONGWUXILIANARRAY  : ILFastMode
	{
		public CHONGWUXILIANARRAY()
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
				TABLE.CHONGWUXILIAN randAttr = new TABLE.CHONGWUXILIAN();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
