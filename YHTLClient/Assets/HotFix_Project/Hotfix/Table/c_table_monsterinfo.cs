namespace TABLE
{
	public partial class MONSTERINFO
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
		public int supType
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
		public uint level
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
		public int model
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
		public uint hp
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
		public uint mp
		{
			get
			{
				 return (uint)__data.intValues[6 + handle.intOffset];
			}
			set
			{
				__data.intValues[6 + handle.intOffset] = (int)value;
			}
		}
		public int att
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
		public int attMax
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
		public int phyDef
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
		public int phyDefMax
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
		public int magicDef
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
		public int magicDefMax
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
		public int accurate
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
		public int dodge
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
		public int heathRecover
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
		public int magicRecover
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
		public int Shield
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
		public int ShieldRecover
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
		public int criticalDamage
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
		public int critical
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
		public uint moveInterval
		{
			get
			{
				 return (uint)__data.intValues[21 + handle.intOffset];
			}
			set
			{
				__data.intValues[21 + handle.intOffset] = (int)value;
			}
		}
		public int head
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
		public uint initDirection
		{
			get
			{
				 return (uint)__data.intValues[23 + handle.intOffset];
			}
			set
			{
				__data.intValues[23 + handle.intOffset] = (int)value;
			}
		}
		public int noAttack
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
		public int PropertiesSuit
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
		public int showName
		{
			get
			{
				 return (int)__data.intValues[26 + handle.intOffset];
			}
			set
			{
				__data.intValues[26 + handle.intOffset] = (int)value;
			}
		}
		public int quality
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
		public int bottomMark
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
		public int ownerType
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
		public int headHeight
		{
			get
			{
				 return (int)__data.intValues[30 + handle.intOffset];
			}
			set
			{
				__data.intValues[30 + handle.intOffset] = (int)value;
			}
		}
		public int bossCarnivalPoints
		{
			get
			{
				 return (int)__data.intValues[31 + handle.intOffset];
			}
			set
			{
				__data.intValues[31 + handle.intOffset] = (int)value;
			}
		}
		public int height
		{
			get
			{
				 return (int)__data.intValues[32 + handle.intOffset];
			}
			set
			{
				__data.intValues[32 + handle.intOffset] = (int)value;
			}
		}
		public int showType
		{
			get
			{
				 return (int)__data.intValues[33 + handle.intOffset];
			}
			set
			{
				__data.intValues[33 + handle.intOffset] = (int)value;
			}
		}
		public int newModel
		{
			get
			{
				 return (int)__data.intValues[34 + handle.intOffset];
			}
			set
			{
				__data.intValues[34 + handle.intOffset] = (int)value;
			}
		}
	}
	public static class MONSTERINFOHelper
	{
		public static void Encode(this System.IO.Stream stream,MONSTERINFO item)
		{
			stream.Encode(item.id);
			stream.Encode(item.type);
			stream.Encode(item.supType);
			stream.Encode(item.name);
			stream.Encode(item.level);
			stream.Encode(item.model);
			stream.Encode(item.hp);
			stream.Encode(item.mp);
			stream.Encode(item.att);
			stream.Encode(item.attMax);
			stream.Encode(item.phyDef);
			stream.Encode(item.phyDefMax);
			stream.Encode(item.magicDef);
			stream.Encode(item.magicDefMax);
			stream.Encode(item.accurate);
			stream.Encode(item.dodge);
			stream.Encode(item.heathRecover);
			stream.Encode(item.magicRecover);
			stream.Encode(item.Shield);
			stream.Encode(item.ShieldRecover);
			stream.Encode(item.criticalDamage);
			stream.Encode(item.critical);
			stream.Encode(item.moveInterval);
			stream.Encode(item.head);
			stream.Encode(item.initDirection);
			stream.Encode(item.noAttack);
			stream.Encode(item.PropertiesSuit);
			stream.Encode(item.showName);
			stream.Encode(item.quality);
			stream.Encode(item.bottomMark);
			stream.Encode(item.ownerType);
			stream.Encode(item.headHeight);
			stream.Encode(item.bossCarnivalPoints);
			stream.Encode(item.height);
			stream.Encode(item.showType);
			stream.Encode(item.newModel);
		}
	}
	public class MONSTERINFOARRAY  : ILFastMode
	{
		public MONSTERINFOARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 35;
			StringValueFixedLength = 1;
			Rules = new byte[]{1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
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
				TABLE.MONSTERINFO randAttr = new TABLE.MONSTERINFO();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
