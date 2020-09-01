namespace TABLE
{
	public partial class UNION
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
		public int maxPlayers
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
		public int upgrade
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
		public int storehouseMaxCount
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
		public int dailyMoneyMax
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
	}
	public static class UNIONHelper
	{
		public static void Encode(this System.IO.Stream stream,UNION item)
		{
			stream.Encode(item.id);
			stream.Encode(item.maxPlayers);
			stream.Encode(item.upgrade);
			stream.Encode(item.storehouseMaxCount);
			stream.Encode(item.dailyMoneyMax);
		}
	}
	public class UNIONARRAY  : ILFastMode
	{
		public UNIONARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 5;
			StringValueFixedLength = 0;
			Rules = new byte[]{1,1,1,1,1};
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
				TABLE.UNION randAttr = new TABLE.UNION();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
