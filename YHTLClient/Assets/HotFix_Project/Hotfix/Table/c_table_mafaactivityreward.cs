namespace TABLE
{
	public partial class MAFAACTIVITYREWARD
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
		public int descid
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
		public int layer
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
		public int costPtr
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
		public IntArray cost
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = costPtr & 0xFFFFF;
				array.__length = (costPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int reward1Ptr
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
		public IntArray reward1
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = reward1Ptr & 0xFFFFF;
				array.__length = (reward1Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int reward2Ptr
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
		public IntArray reward2
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = reward2Ptr & 0xFFFFF;
				array.__length = (reward2Ptr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class MAFAACTIVITYREWARDHelper
	{
		public static void Encode(this System.IO.Stream stream,MAFAACTIVITYREWARD item)
		{
			stream.Encode(item.id);
			stream.Encode(item.descid);
			stream.Encode(item.layer);
			stream.Encode(item.costPtr);
			stream.Encode(item.reward1Ptr);
			stream.Encode(item.reward2Ptr);
		}
	}
	public class MAFAACTIVITYREWARDARRAY  : ILFastMode
	{
		public MAFAACTIVITYREWARDARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 6;
			StringValueFixedLength = 0;
			Rules = new byte[]{1,1,1,1,1,1};
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
				TABLE.MAFAACTIVITYREWARD randAttr = new TABLE.MAFAACTIVITYREWARD();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
