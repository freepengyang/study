namespace TABLE
{
	public partial class COMBINE
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
		public int groupID
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
		public string groupName
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
		public int SubType
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
		public int generateItem
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
		public string SubTypeName
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
		public int needItemPtr
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
		public IntArray needItem
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = needItemPtr & 0xFFFFF;
				array.__length = (needItemPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int needResourcePtr
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
		public IntArray needResource
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = needResourcePtr & 0xFFFFF;
				array.__length = (needResourcePtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int openTimePtr
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
		public IntArray openTime
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = openTimePtr & 0xFFFFF;
				array.__length = (openTimePtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int combineTimePtr
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
		public IntArray combineTime
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = combineTimePtr & 0xFFFFF;
				array.__length = (combineTimePtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class COMBINEHelper
	{
		public static void Encode(this System.IO.Stream stream,COMBINE item)
		{
			stream.Encode(item.id);
			stream.Encode(item.groupID);
			stream.Encode(item.groupName);
			stream.Encode(item.SubType);
			stream.Encode(item.generateItem);
			stream.Encode(item.SubTypeName);
			stream.Encode(item.needItemPtr);
			stream.Encode(item.needResourcePtr);
			stream.Encode(item.openTimePtr);
			stream.Encode(item.combineTimePtr);
		}
	}
	public class COMBINEARRAY  : ILFastMode
	{
		public COMBINEARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 8;
			StringValueFixedLength = 2;
			Rules = new byte[]{1,1,2,1,1,2,1,1,1,1};
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
				TABLE.COMBINE randAttr = new TABLE.COMBINE();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
