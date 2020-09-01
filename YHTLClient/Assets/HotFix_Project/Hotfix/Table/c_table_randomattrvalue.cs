namespace TABLE
{
	public partial class RANDOMATTRVALUE
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
		public uint level
		{
			get
			{
				 return (uint)__data.intValues[1 + handle.intOffset];
			}
			set
			{
				__data.intValues[1 + handle.intOffset] = (int)value;
			}
		}
		public uint levClass
		{
			get
			{
				 return (uint)__data.intValues[2 + handle.intOffset];
			}
			set
			{
				__data.intValues[2 + handle.intOffset] = (int)value;
			}
		}
		public uint type
		{
			get
			{
				 return (uint)__data.intValues[3 + handle.intOffset];
			}
			set
			{
				__data.intValues[3 + handle.intOffset] = (int)value;
			}
		}
		public uint parameter
		{
			get
			{
				 return (uint)__data.intValues[4 + handle.intOffset];
			}
			set
			{
				__data.intValues[4 + handle.intOffset] = (int)value;
			}
		}
		public uint MaxValue
		{
			get
			{
				 return (uint)__data.intValues[5 + handle.intOffset];
			}
			set
			{
				__data.intValues[5 + handle.intOffset] = (int)value;
			}
		}
		public uint show
		{
			get
			{
				 return (uint)__data.intValues[6 + handle.intOffset];
			}
			set
			{
				__data.intValues[6 + handle.intOffset] = (int)value;
			}
		}
	}
	public static class RANDOMATTRVALUEHelper
	{
		public static void Encode(this System.IO.Stream stream,RANDOMATTRVALUE item)
		{
			stream.Encode(item.id);
			stream.Encode(item.level);
			stream.Encode(item.levClass);
			stream.Encode(item.type);
			stream.Encode(item.parameter);
			stream.Encode(item.MaxValue);
			stream.Encode(item.show);
		}
	}
	public class RANDOMATTRVALUEARRAY  : ILFastMode
	{
		public RANDOMATTRVALUEARRAY()
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
				TABLE.RANDOMATTRVALUE randAttr = new TABLE.RANDOMATTRVALUE();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
