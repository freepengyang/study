namespace TABLE
{
	public partial class QIANGHUA
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
		public int library
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
		public int qhLev
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
		public int qhProba
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
		public int jingpoProba
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
		public string itemCost
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
		public string moneyCost
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
		public string jingpoCost
		{
			get
			{
				return __data.stringValues[2 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[2 + handle.stringOffset] = value;
			}
		}
		public string dingxingCost
		{
			get
			{
				return __data.stringValues[3 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[3 + handle.stringOffset] = value;
			}
		}
		public int taozhuangAttrPtr
		{
			get
			{
				return __data.intValues[5 + handle.intOffset];
			}
			set
			{
				__data.intValues[5 + handle.intOffset] = value;
			}
		}
		public LongArray taozhuangAttr
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = taozhuangAttrPtr & 0xFFFFF;
				array.__length = (taozhuangAttrPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int qhAttr
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
	public static class QIANGHUAHelper
	{
		public static void Encode(this System.IO.Stream stream,QIANGHUA item)
		{
			stream.Encode(item.id);
			stream.Encode(item.library);
			stream.Encode(item.qhLev);
			stream.Encode(item.qhProba);
			stream.Encode(item.jingpoProba);
			stream.Encode(item.itemCost);
			stream.Encode(item.moneyCost);
			stream.Encode(item.jingpoCost);
			stream.Encode(item.dingxingCost);
			stream.Encode(item.taozhuangAttrPtr);
			stream.Encode(item.qhAttr);
		}
	}
	public class QIANGHUAARRAY  : ILFastMode
	{
		public QIANGHUAARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 7;
			StringValueFixedLength = 4;
			Rules = new byte[]{1,1,1,1,1,2,2,2,2,1,1};
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
				TABLE.QIANGHUA randAttr = new TABLE.QIANGHUA();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
