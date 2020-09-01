namespace TABLE
{
	public partial class HANDBOOKSLOT
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
		public int cost
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
		public int kaiFuDay
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
	public static class HANDBOOKSLOTHelper
	{
		public static void Encode(this System.IO.Stream stream,HANDBOOKSLOT item)
		{
			stream.Encode(item.id);
			stream.Encode(item.cost);
			stream.Encode(item.kaiFuDay);
		}
	}
	public class HANDBOOKSLOTARRAY  : ILFastMode
	{
		public HANDBOOKSLOTARRAY()
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
				TABLE.HANDBOOKSLOT randAttr = new TABLE.HANDBOOKSLOT();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
