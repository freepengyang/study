namespace TABLE
{
	public partial class JINGJIHUODONGJIANGLI
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
		public int jingjiHuodongId
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
		public string typevalue
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
		public int count
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
		public int rewardPtr
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
		public LongArray reward
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = rewardPtr & 0xFFFFF;
				array.__length = (rewardPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int quota
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
		public int deliver
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
		public int uiModel
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
		public int Categories
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
		public string bulletin
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
	public static class JINGJIHUODONGJIANGLIHelper
	{
		public static void Encode(this System.IO.Stream stream,JINGJIHUODONGJIANGLI item)
		{
			stream.Encode(item.id);
			stream.Encode(item.jingjiHuodongId);
			stream.Encode(item.name);
			stream.Encode(item.typevalue);
			stream.Encode(item.count);
			stream.Encode(item.rewardPtr);
			stream.Encode(item.quota);
			stream.Encode(item.deliver);
			stream.Encode(item.uiModel);
			stream.Encode(item.Categories);
			stream.Encode(item.bulletin);
		}
	}
	public class JINGJIHUODONGJIANGLIARRAY  : ILFastMode
	{
		public JINGJIHUODONGJIANGLIARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 8;
			StringValueFixedLength = 3;
			Rules = new byte[]{1,1,2,2,1,1,1,1,1,1,2};
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
				TABLE.JINGJIHUODONGJIANGLI randAttr = new TABLE.JINGJIHUODONGJIANGLI();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
