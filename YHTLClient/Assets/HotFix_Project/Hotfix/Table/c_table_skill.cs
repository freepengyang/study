namespace TABLE
{
	public partial class SKILL
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
		public int effectId
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
		public int usertype
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
		public int career
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
		public int skillGroup
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
		public int showorder
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
		public int show
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
		public int automatically
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
		public int autofightflag
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
		public int mpCost
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
		public int studyLevel
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
		public int cdTime
		{
			get
			{
				 return (int)__data.intValues[12 + handle.intOffset];
			}
			set
			{
				__data.intValues[12 + handle.intOffset] = (int)value;
			}
		}
		public int type
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
		public int hurtPoint
		{
			get
			{
				 return (int)__data.intValues[14 + handle.intOffset];
			}
			set
			{
				__data.intValues[14 + handle.intOffset] = (int)value;
			}
		}
		public string exHurt
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
		public string curePoint
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
		public string exCure
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
		public int clientTargetType
		{
			get
			{
				 return (int)__data.intValues[15 + handle.intOffset];
			}
			set
			{
				__data.intValues[15 + handle.intOffset] = (int)value;
			}
		}
		public int effectArea
		{
			get
			{
				 return (int)__data.intValues[16 + handle.intOffset];
			}
			set
			{
				__data.intValues[16 + handle.intOffset] = (int)value;
			}
		}
		public int effectRange
		{
			get
			{
				 return (int)__data.intValues[17 + handle.intOffset];
			}
			set
			{
				__data.intValues[17 + handle.intOffset] = (int)value;
			}
		}
		public int clientRange
		{
			get
			{
				 return (int)__data.intValues[18 + handle.intOffset];
			}
			set
			{
				__data.intValues[18 + handle.intOffset] = (int)value;
			}
		}
		public string Speciallv
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
		public string description
		{
			get
			{
				return __data.stringValues[5 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[5 + handle.stringOffset] = value;
			}
		}
		public int buff
		{
			get
			{
				 return (int)__data.intValues[19 + handle.intOffset];
			}
			set
			{
				__data.intValues[19 + handle.intOffset] = (int)value;
			}
		}
		public string icon
		{
			get
			{
				return __data.stringValues[6 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[6 + handle.stringOffset] = value;
			}
		}
		public int action
		{
			get
			{
				 return (int)__data.intValues[20 + handle.intOffset];
			}
			set
			{
				__data.intValues[20 + handle.intOffset] = (int)value;
			}
		}
		public int audio
		{
			get
			{
				 return (int)__data.intValues[21 + handle.intOffset];
			}
			set
			{
				__data.intValues[21 + handle.intOffset] = (int)value;
			}
		}
		public int audioMan
		{
			get
			{
				 return (int)__data.intValues[22 + handle.intOffset];
			}
			set
			{
				__data.intValues[22 + handle.intOffset] = (int)value;
			}
		}
		public int beattackAudio
		{
			get
			{
				 return (int)__data.intValues[23 + handle.intOffset];
			}
			set
			{
				__data.intValues[23 + handle.intOffset] = (int)value;
			}
		}
		public int put
		{
			get
			{
				 return (int)__data.intValues[24 + handle.intOffset];
			}
			set
			{
				__data.intValues[24 + handle.intOffset] = (int)value;
			}
		}
		public int order
		{
			get
			{
				 return (int)__data.intValues[25 + handle.intOffset];
			}
			set
			{
				__data.intValues[25 + handle.intOffset] = (int)value;
			}
		}
		public int costPtr
		{
			get
			{
				return __data.intValues[26 + handle.intOffset];
			}
			set
			{
				__data.intValues[26 + handle.intOffset] = value;
			}
		}
		public LongArray cost
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = costPtr & 0xFFFFF;
				array.__length = (costPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public string publicCdGroup
		{
			get
			{
				return __data.stringValues[7 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[7 + handle.stringOffset] = value;
			}
		}
		public string publicCd
		{
			get
			{
				return __data.stringValues[8 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[8 + handle.stringOffset] = value;
			}
		}
		public string nbValue
		{
			get
			{
				return __data.stringValues[9 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[9 + handle.stringOffset] = value;
			}
		}
		public int launchType
		{
			get
			{
				 return (int)__data.intValues[27 + handle.intOffset];
			}
			set
			{
				__data.intValues[27 + handle.intOffset] = (int)value;
			}
		}
		public string clientDescription
		{
			get
			{
				return __data.stringValues[10 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[10 + handle.stringOffset] = value;
			}
		}
		public int notice
		{
			get
			{
				 return (int)__data.intValues[28 + handle.intOffset];
			}
			set
			{
				__data.intValues[28 + handle.intOffset] = (int)value;
			}
		}
		public int getPoint
		{
			get
			{
				 return (int)__data.intValues[29 + handle.intOffset];
			}
			set
			{
				__data.intValues[29 + handle.intOffset] = (int)value;
			}
		}
	}
	public static class SKILLHelper
	{
		public static void Encode(this System.IO.Stream stream,SKILL item)
		{
			stream.Encode(item.id);
			stream.Encode(item.effectId);
			stream.Encode(item.usertype);
			stream.Encode(item.career);
			stream.Encode(item.skillGroup);
			stream.Encode(item.level);
			stream.Encode(item.name);
			stream.Encode(item.showorder);
			stream.Encode(item.show);
			stream.Encode(item.automatically);
			stream.Encode(item.autofightflag);
			stream.Encode(item.mpCost);
			stream.Encode(item.studyLevel);
			stream.Encode(item.cdTime);
			stream.Encode(item.type);
			stream.Encode(item.hurtPoint);
			stream.Encode(item.exHurt);
			stream.Encode(item.curePoint);
			stream.Encode(item.exCure);
			stream.Encode(item.clientTargetType);
			stream.Encode(item.effectArea);
			stream.Encode(item.effectRange);
			stream.Encode(item.clientRange);
			stream.Encode(item.Speciallv);
			stream.Encode(item.description);
			stream.Encode(item.buff);
			stream.Encode(item.icon);
			stream.Encode(item.action);
			stream.Encode(item.audio);
			stream.Encode(item.audioMan);
			stream.Encode(item.beattackAudio);
			stream.Encode(item.put);
			stream.Encode(item.order);
			stream.Encode(item.costPtr);
			stream.Encode(item.publicCdGroup);
			stream.Encode(item.publicCd);
			stream.Encode(item.nbValue);
			stream.Encode(item.launchType);
			stream.Encode(item.clientDescription);
			stream.Encode(item.notice);
			stream.Encode(item.getPoint);
		}
	}
	public class SKILLARRAY  : ILFastMode
	{
		public SKILLARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 30;
			StringValueFixedLength = 11;
			Rules = new byte[]{1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,2,2,2,1,1,1,1,2,2,1,2,1,1,1,1,1,1,1,2,2,2,1,2,1,1};
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
				TABLE.SKILL randAttr = new TABLE.SKILL();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
