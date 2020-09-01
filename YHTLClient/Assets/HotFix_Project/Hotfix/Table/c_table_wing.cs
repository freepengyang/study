namespace TABLE
{
	public partial class WING
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
		public uint rank
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
		public int model
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
		public uint rankType
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
		public uint rankPara
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
		public uint rankNum
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
		public string pic
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
		public string img
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
		public uint starID
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
		public int zsstarAttrParaPtr
		{
			get
			{
				 return (int)__data.intValues[7 + handle.intOffset];
			}
			set
			{
				__data.intValues[7 + handle.intOffset] = (int)value;
			}
		}
		public IntArray zsstarAttrPara
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = zsstarAttrParaPtr & 0xFFFFF;
				array.__length = (zsstarAttrParaPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int fsstarAttrParaPtr
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
		public IntArray fsstarAttrPara
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = fsstarAttrParaPtr & 0xFFFFF;
				array.__length = (fsstarAttrParaPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int dsstarAttrParaPtr
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
		public IntArray dsstarAttrPara
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = dsstarAttrParaPtr & 0xFFFFF;
				array.__length = (dsstarAttrParaPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int starAttrNumPtr
		{
			get
			{
				 return (int)__data.intValues[10 + handle.intOffset];
			}
			set
			{
				__data.intValues[10 + handle.intOffset] = (int)value;
			}
		}
		public IntArray starAttrNum
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = starAttrNumPtr & 0xFFFFF;
				array.__length = (starAttrNumPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public uint starCostExp
		{
			get
			{
				 return (uint)__data.intValues[11 + handle.intOffset];
			}
			set
			{
				__data.intValues[11 + handle.intOffset] = (int)value;
			}
		}
		public int starCostPtr
		{
			get
			{
				return __data.intValues[12 + handle.intOffset];
			}
			set
			{
				__data.intValues[12 + handle.intOffset] = value;
			}
		}
		public LongArray starCost
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = starCostPtr & 0xFFFFF;
				array.__length = (starCostPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int PrimaryKey
		{
			get
			{
				 return (int)__data.intValues[13 + handle.intOffset];
			}
			set
			{
				__data.intValues[13 + handle.intOffset] = (int)value;
			}
		}
	}
	public static class WINGHelper
	{
		public static void Encode(this System.IO.Stream stream,WING item)
		{
			stream.Encode(item.id);
			stream.Encode(item.rank);
			stream.Encode(item.model);
			stream.Encode(item.rankType);
			stream.Encode(item.rankPara);
			stream.Encode(item.rankNum);
			stream.Encode(item.pic);
			stream.Encode(item.img);
			stream.Encode(item.starID);
			stream.Encode(item.zsstarAttrParaPtr);
			stream.Encode(item.fsstarAttrParaPtr);
			stream.Encode(item.dsstarAttrParaPtr);
			stream.Encode(item.starAttrNumPtr);
			stream.Encode(item.starCostExp);
			stream.Encode(item.starCostPtr);
			stream.Encode(item.PrimaryKey);
		}
	}
	public class WINGARRAY  : ILFastMode
	{
		public WINGARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 14;
			StringValueFixedLength = 2;
			Rules = new byte[]{1,1,1,1,1,1,2,2,1,1,1,1,1,1,1,1};
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
				TABLE.WING randAttr = new TABLE.WING();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
