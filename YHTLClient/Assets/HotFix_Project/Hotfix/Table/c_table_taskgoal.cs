namespace TABLE
{
	public partial class TASKGOAL
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
		public int type
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
		public string links
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
		public int goalParamPtr
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
		public IntArray goalParam
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = goalParamPtr & 0xFFFFF;
				array.__length = (goalParamPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int goalCount
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
		public string goalTips
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
		public int mapId
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
		public string x
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
		public string y
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
		public string res
		{
			get
			{
				return __data.stringValues[4 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[4 + handle.stringOffset] = value;
			}
		}
		public int deliverPtr
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
		public IntArray deliver
		{
			get
			{
				IntArray array;
				array.__fastMode = this.Array;
				array.__start = deliverPtr & 0xFFFFF;
				array.__length = (deliverPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
	}
	public static class TASKGOALHelper
	{
		public static void Encode(this System.IO.Stream stream,TASKGOAL item)
		{
			stream.Encode(item.id);
			stream.Encode(item.type);
			stream.Encode(item.links);
			stream.Encode(item.goalParamPtr);
			stream.Encode(item.goalCount);
			stream.Encode(item.goalTips);
			stream.Encode(item.mapId);
			stream.Encode(item.x);
			stream.Encode(item.y);
			stream.Encode(item.res);
			stream.Encode(item.deliverPtr);
		}
	}
	public class TASKGOALARRAY  : ILFastMode
	{
		public TASKGOALARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 6;
			StringValueFixedLength = 5;
			Rules = new byte[]{1,1,2,1,1,2,1,2,2,2,1};
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
				TABLE.TASKGOAL randAttr = new TABLE.TASKGOAL();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
