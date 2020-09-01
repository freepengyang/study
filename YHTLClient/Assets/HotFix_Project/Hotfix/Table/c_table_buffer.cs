namespace TABLE
{
	public partial class BUFFER
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
		public string icon
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
		public uint type
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
		public uint exeType
		{
			get
			{
				 return (uint)__data.intValues[2 + handle.intOffset];
			}
			set
			{
				__data.intValues[2 + handle.intOffset] = (int)value;
			}
		}
		public uint exeParam
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
		public uint dispelType
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
		public uint dispelParam
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
		public int effectId
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
		public string tips
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
		public int tipParam
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
		public uint showDelay
		{
			get
			{
				 return (uint)__data.intValues[8 + handle.intOffset];
			}
			set
			{
				__data.intValues[8 + handle.intOffset] = (int)value;
			}
		}
		public string attBuff
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
		public string hurtBuff
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
		public int expBuff
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
		public string replyBuff
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
		public string reboundBuff
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
		public string parameterBuff
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
		public uint showOrHide
		{
			get
			{
				 return (uint)__data.intValues[10 + handle.intOffset];
			}
			set
			{
				__data.intValues[10 + handle.intOffset] = (int)value;
			}
		}
		public string height
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
		public uint layer
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
		public string junpWord
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
		public string triggerTime
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
		public int exclusionGroup
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
		public int exclusionOrder
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
		public string superposition
		{
			get
			{
				return __data.stringValues[11 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[11 + handle.stringOffset] = value;
			}
		}
		public int layers
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
		public string formula
		{
			get
			{
				return __data.stringValues[12 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[12 + handle.stringOffset] = value;
			}
		}
		public string mapId
		{
			get
			{
				return __data.stringValues[13 + handle.stringOffset];
			}
			set
			{
				__data.stringValues[13 + handle.stringOffset] = value;
			}
		}
		public int level
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
	}
	public static class BUFFERHelper
	{
		public static void Encode(this System.IO.Stream stream,BUFFER item)
		{
			stream.Encode(item.id);
			stream.Encode(item.name);
			stream.Encode(item.icon);
			stream.Encode(item.type);
			stream.Encode(item.exeType);
			stream.Encode(item.exeParam);
			stream.Encode(item.dispelType);
			stream.Encode(item.dispelParam);
			stream.Encode(item.effectId);
			stream.Encode(item.tips);
			stream.Encode(item.tipParam);
			stream.Encode(item.showDelay);
			stream.Encode(item.attBuff);
			stream.Encode(item.hurtBuff);
			stream.Encode(item.expBuff);
			stream.Encode(item.replyBuff);
			stream.Encode(item.reboundBuff);
			stream.Encode(item.parameterBuff);
			stream.Encode(item.showOrHide);
			stream.Encode(item.height);
			stream.Encode(item.layer);
			stream.Encode(item.junpWord);
			stream.Encode(item.triggerTime);
			stream.Encode(item.exclusionGroup);
			stream.Encode(item.exclusionOrder);
			stream.Encode(item.superposition);
			stream.Encode(item.layers);
			stream.Encode(item.formula);
			stream.Encode(item.mapId);
			stream.Encode(item.level);
		}
	}
	public class BUFFERARRAY  : ILFastMode
	{
		public BUFFERARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 16;
			StringValueFixedLength = 14;
			Rules = new byte[]{1,2,2,1,1,1,1,1,1,2,1,1,2,2,1,2,2,2,1,2,1,2,2,1,1,2,1,2,2,1};
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
				TABLE.BUFFER randAttr = new TABLE.BUFFER();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
