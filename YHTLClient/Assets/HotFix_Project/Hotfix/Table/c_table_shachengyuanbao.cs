namespace TABLE
{
	public partial class SHACHENGYUANBAO
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
		public int time
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
		public int basic
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
		public int transform
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
	public static class SHACHENGYUANBAOHelper
	{
		public static void Encode(this System.IO.Stream stream,SHACHENGYUANBAO item)
		{
			stream.Encode(item.id);
			stream.Encode(item.time);
			stream.Encode(item.basic);
			stream.Encode(item.transform);
		}
	}
	public class SHACHENGYUANBAOARRAY  : ILFastMode
	{
		public SHACHENGYUANBAOARRAY()
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
				TABLE.SHACHENGYUANBAO randAttr = new TABLE.SHACHENGYUANBAO();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
