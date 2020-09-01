namespace TABLE
{
	public partial class HANDBOOK
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
		public int itemID
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
		public int bookMarkScore
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
		public int camp
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
		public int sitemap
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
		public int status
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
		public int levelUpCostPtr
		{
			get
			{
				return __data.intValues[6 + handle.intOffset];
			}
			set
			{
				__data.intValues[6 + handle.intOffset] = value;
			}
		}
		public LongArray levelUpCost
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = levelUpCostPtr & 0xFFFFF;
				array.__length = (levelUpCostPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int qualityUpCostPtr
		{
			get
			{
				return __data.intValues[7 + handle.intOffset];
			}
			set
			{
				__data.intValues[7 + handle.intOffset] = value;
			}
		}
		public LongArray qualityUpCost
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = qualityUpCostPtr & 0xFFFFF;
				array.__length = (qualityUpCostPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int parameterPtr
		{
			get
			{
				 return (int)__data.intValues[8 + handle.intOffset];
			}
			set
			{
				__data.intValues[8 + handle.intOffset] = (int)value;
			}
		}
		public IntArray parameter
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = parameterPtr & 0xFFFFF;
				array.__length = (parameterPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int factorPtr
		{
			get
			{
				 return (int)__data.intValues[9 + handle.intOffset];
			}
			set
			{
				__data.intValues[9 + handle.intOffset] = (int)value;
			}
		}
		public IntArray factor
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = factorPtr & 0xFFFFF;
				array.__length = (factorPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class HANDBOOKHelper
	{
		public static void Encode(this System.IO.Stream stream,HANDBOOK item)
		{
			stream.Encode(item.id);
			stream.Encode(item.itemID);
			stream.Encode(item.bookMarkScore);
			stream.Encode(item.camp);
			stream.Encode(item.sitemap);
			stream.Encode(item.status);
			stream.Encode(item.levelUpCostPtr);
			stream.Encode(item.qualityUpCostPtr);
			stream.Encode(item.parameterPtr);
			stream.Encode(item.factorPtr);
		}
	}
	public class HANDBOOKARRAY  : ILFastMode
	{
		public HANDBOOKARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 10;
			StringValueFixedLength = 0;
			Rules = new byte[]{1,1,1,1,1,1,1,1,1,1};
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
				TABLE.HANDBOOK randAttr = new TABLE.HANDBOOK();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
