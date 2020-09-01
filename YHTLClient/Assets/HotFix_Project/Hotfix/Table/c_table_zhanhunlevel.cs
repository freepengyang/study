namespace TABLE
{
	public partial class ZHANHUNLEVEL
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
		public int level
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
		public int needExp
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
		public int hp
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
		public int mp
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
		public int attType
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
		public int att
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
		public int attMax
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
		public int phyDef
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
		public int phyDefMax
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
		public int magicDef
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
		public int magicDefMax
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
		public int heathRecover
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
		public int mpHeal
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
		public int zhanHunShield
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
		public int zhanHunShieldRecover
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
		public int accurate
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
		public int dodge
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
	}
	public static class ZHANHUNLEVELHelper
	{
		public static void Encode(this System.IO.Stream stream,ZHANHUNLEVEL item)
		{
			stream.Encode(item.id);
			stream.Encode(item.level);
			stream.Encode(item.needExp);
			stream.Encode(item.hp);
			stream.Encode(item.mp);
			stream.Encode(item.attType);
			stream.Encode(item.att);
			stream.Encode(item.attMax);
			stream.Encode(item.phyDef);
			stream.Encode(item.phyDefMax);
			stream.Encode(item.magicDef);
			stream.Encode(item.magicDefMax);
			stream.Encode(item.heathRecover);
			stream.Encode(item.mpHeal);
			stream.Encode(item.zhanHunShield);
			stream.Encode(item.zhanHunShieldRecover);
			stream.Encode(item.accurate);
			stream.Encode(item.dodge);
		}
	}
	public class ZHANHUNLEVELARRAY  : ILFastMode
	{
		public ZHANHUNLEVELARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 18;
			StringValueFixedLength = 0;
			Rules = new byte[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
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
				TABLE.ZHANHUNLEVEL randAttr = new TABLE.ZHANHUNLEVEL();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
