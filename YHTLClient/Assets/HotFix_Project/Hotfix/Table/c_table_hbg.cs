namespace TABLE
{
	public partial class HBG
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
		public int camp
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
		public int sitemap
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
		public int status
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
		public int levelUpCostPtr
		{
			get
			{
				return __data.intValues[4 + handle.intOffset];
			}
			set
			{
				__data.intValues[4 + handle.intOffset] = value;
			}
		}
		public LongArray levelUpCost
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = levelUpCostPtr & 0xFFFF;
				array.__end = (levelUpCostPtr >> 16) & 0xFFFF;
				array.__length = array.__end - array.__start;
				return array;
			}
		}
		public string qualityUpCost
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
		public string type
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
		public int parameterPtr
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
		public IntArray parameter
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = parameterPtr & 0xFFFF;
				array.__end = (parameterPtr >> 16) & 0xFFFF;
				array.__length = array.__end - array.__start;
				return array;
			}
		}
		public int factorPtr
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
		public IntArray factor
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = factorPtr & 0xFFFF;
				array.__end = (factorPtr >> 16) & 0xFFFF;
				array.__length = array.__end - array.__start;
				return array;
			}
		}
	}
	public static class HBGHelper
	{
		public static void Encode(this System.IO.Stream stream,HBG item)
		{
			stream.Encode(item.id);
			stream.Encode(item.camp);
			stream.Encode(item.sitemap);
			stream.Encode(item.status);
			stream.Encode(item.levelUpCostPtr);
			stream.Encode(item.qualityUpCost);
			stream.Encode(item.type);
			stream.Encode(item.parameterPtr);
			stream.Encode(item.factorPtr);
		}
	}
	public class HBGARRAY  : ILFastMode
	{
		public HBGARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 7;
			StringValueFixedLength = 2;
			Rules = new byte[]{1,1,1,1,1,2,2,1,1};
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
				TABLE.HBG randAttr = new TABLE.HBG();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
