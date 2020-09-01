namespace TABLE
{
	public partial class SHIZHENREWARD
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
		public int type
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
		public int itemId
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
		public int num
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
		public int probabily
		{
			get
			{
				 return (int)__data.intValues[4 + handle.intOffset];
			}
			set
			{
				__data.intValues[4 + handle.intOffset] = (int)value;
			}
		}
		public int numLimit
		{
			get
			{
				 return (int)__data.intValues[5 + handle.intOffset];
			}
			set
			{
				__data.intValues[5 + handle.intOffset] = (int)value;
			}
		}
		public int openDay
		{
			get
			{
				 return (int)__data.intValues[6 + handle.intOffset];
			}
			set
			{
				__data.intValues[6 + handle.intOffset] = (int)value;
			}
		}
	}
	public static class SHIZHENREWARDHelper
	{
		public static void Encode(this System.IO.Stream stream,SHIZHENREWARD item)
		{
			stream.Encode(item.id);
			stream.Encode(item.type);
			stream.Encode(item.itemId);
			stream.Encode(item.num);
			stream.Encode(item.probabily);
			stream.Encode(item.numLimit);
			stream.Encode(item.openDay);
		}
	}
	public class SHIZHENREWARDARRAY  : ILFastMode
	{
		public SHIZHENREWARDARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 7;
			StringValueFixedLength = 0;
			Rules = new byte[]{1,1,1,1,1,1,1};
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
				TABLE.SHIZHENREWARD randAttr = new TABLE.SHIZHENREWARD();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
