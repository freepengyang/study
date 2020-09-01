namespace TABLE
{
	public partial class GEMSUIT
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
		public int gamPosition
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
		public int gamLevel
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
		public int gamNum
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
		public int attrPtr
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
		public LongArray attr
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = attrPtr & 0xFFFFF;
				array.__length = (attrPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int level
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
		public string name
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
		public string des
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
		public string pic
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
	}
	public static class GEMSUITHelper
	{
		public static void Encode(this System.IO.Stream stream,GEMSUIT item)
		{
			stream.Encode(item.id);
			stream.Encode(item.gamPosition);
			stream.Encode(item.gamLevel);
			stream.Encode(item.gamNum);
			stream.Encode(item.attrPtr);
			stream.Encode(item.level);
			stream.Encode(item.name);
			stream.Encode(item.des);
			stream.Encode(item.pic);
		}
	}
	public class GEMSUITARRAY  : ILFastMode
	{
		public GEMSUITARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 6;
			StringValueFixedLength = 3;
			Rules = new byte[]{1,1,1,1,1,1,2,2,2};
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
				TABLE.GEMSUIT randAttr = new TABLE.GEMSUIT();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
