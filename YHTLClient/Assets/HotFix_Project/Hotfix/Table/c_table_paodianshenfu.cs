namespace TABLE
{
	public partial class PAODIANSHENFU
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
		public int rank
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
		public int star
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
		public int clientRankParaPtr
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
		public IntArray clientRankPara
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = clientRankParaPtr & 0xFFFFF;
				array.__length = (clientRankParaPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int rankaddNumPtr
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
		public IntArray rankaddNum
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = rankaddNumPtr & 0xFFFFF;
				array.__length = (rankaddNumPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int zsattrParaPtr
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
		public IntArray zsattrPara
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = zsattrParaPtr & 0xFFFFF;
				array.__length = (zsattrParaPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int fsattrParaPtr
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
		public IntArray fsattrPara
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = fsattrParaPtr & 0xFFFFF;
				array.__length = (fsattrParaPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int dsattrParaPtr
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
		public IntArray dsattrPara
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = dsattrParaPtr & 0xFFFFF;
				array.__length = (dsattrParaPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int attrNumPtr
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
		public IntArray attrNum
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = attrNumPtr & 0xFFFFF;
				array.__length = (attrNumPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int costItemPtr
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
		public IntArray costItem
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = costItemPtr & 0xFFFFF;
				array.__length = (costItemPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int costNumPtr
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
		public IntArray costNum
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = costNumPtr & 0xFFFFF;
				array.__length = (costNumPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public string icon
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
		public int PrimaryKey
		{
			get
			{
				 return (int)__data.intValues[11 + handle.intOffset];
			}
			set
			{
				__data.intValues[11 + handle.intOffset] = (int)value;
			}
		}
	}
	public static class PAODIANSHENFUHelper
	{
		public static void Encode(this System.IO.Stream stream,PAODIANSHENFU item)
		{
			stream.Encode(item.id);
			stream.Encode(item.rank);
			stream.Encode(item.star);
			stream.Encode(item.clientRankParaPtr);
			stream.Encode(item.rankaddNumPtr);
			stream.Encode(item.zsattrParaPtr);
			stream.Encode(item.fsattrParaPtr);
			stream.Encode(item.dsattrParaPtr);
			stream.Encode(item.attrNumPtr);
			stream.Encode(item.costItemPtr);
			stream.Encode(item.costNumPtr);
			stream.Encode(item.icon);
			stream.Encode(item.PrimaryKey);
		}
	}
	public class PAODIANSHENFUARRAY  : ILFastMode
	{
		public PAODIANSHENFUARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 12;
			StringValueFixedLength = 1;
			Rules = new byte[]{1,1,1,1,1,1,1,1,1,1,1,2,1};
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
				TABLE.PAODIANSHENFU randAttr = new TABLE.PAODIANSHENFU();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
