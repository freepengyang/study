namespace TABLE
{
	public partial class CHONGWUXILIANCOST
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
		public int normalCostPtr
		{
			get
			{
				return __data.intValues[2 + handle.intOffset];
			}
			set
			{
				__data.intValues[2 + handle.intOffset] = value;
			}
		}
		public LongArray normalCost
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = normalCostPtr & 0xFFFFF;
				array.__length = (normalCostPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int specialCostPtr
		{
			get
			{
				return __data.intValues[3 + handle.intOffset];
			}
			set
			{
				__data.intValues[3 + handle.intOffset] = value;
			}
		}
		public LongArray specialCost
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = specialCostPtr & 0xFFFFF;
				array.__length = (specialCostPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class CHONGWUXILIANCOSTHelper
	{
		public static void Encode(this System.IO.Stream stream,CHONGWUXILIANCOST item)
		{
			stream.Encode(item.id);
			stream.Encode(item.levClass);
			stream.Encode(item.normalCostPtr);
			stream.Encode(item.specialCostPtr);
		}
	}
	public class CHONGWUXILIANCOSTARRAY  : ILFastMode
	{
		public CHONGWUXILIANCOSTARRAY()
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
				TABLE.CHONGWUXILIANCOST randAttr = new TABLE.CHONGWUXILIANCOST();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
