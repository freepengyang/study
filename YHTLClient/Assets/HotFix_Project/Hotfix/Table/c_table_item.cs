namespace TABLE
{
	public partial class ITEM
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
		public int subType
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
		public int overlaying
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
		public int levClass
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
		public int quality
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
		public int exValue
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
		public string tips
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
		public string icon
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
		public string img
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
		public int sex
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
		public int career
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
		public int model
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
		public int data
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
		public int phyAttMax
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
		public int phyAttMin
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
		public int magicAttMax
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
		public int magicAttMin
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
		public int taoAttMax
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
		public int taoAttMin
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
		public int phyDefMax
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
		public int phyDefMin
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
		public int magicDefMax
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
		public int magicDefMin
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
		public int accurate
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
		public int dodge
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
		public int curse
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
		public int luck
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
		public int criticalDamage
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
		public int critical
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
		public int hp
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
		public int mp
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
		public int holyAtt
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
		public int pkAtt
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
		public int pkDef
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
		public int resistanceCrit
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
		public int Destroy
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
		public int dropShow
		{
			get
			{
				 return (int)__data.intValues[35 + handle.intOffset];
			}
			set
			{
				__data.intValues[35 + handle.intOffset] = (int)value;
			}
		}
		public int binding
		{
			get
			{
				 return (int)__data.intValues[36 + handle.intOffset];
			}
			set
			{
				__data.intValues[36 + handle.intOffset] = (int)value;
			}
		}
		public int notPrompt
		{
			get
			{
				 return (int)__data.intValues[37 + handle.intOffset];
			}
			set
			{
				__data.intValues[37 + handle.intOffset] = (int)value;
			}
		}
		public string timeLimit
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
		public int pickUpType
		{
			get
			{
				 return (int)__data.intValues[38 + handle.intOffset];
			}
			set
			{
				__data.intValues[38 + handle.intOffset] = (int)value;
			}
		}
		public int wolongLv
		{
			get
			{
				 return (int)__data.intValues[39 + handle.intOffset];
			}
			set
			{
				__data.intValues[39 + handle.intOffset] = (int)value;
			}
		}
		public string tips2
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
		public int callbackPtr
		{
			get
			{
				return __data.intValues[40 + handle.intOffset];
			}
			set
			{
				__data.intValues[40 + handle.intOffset] = value;
			}
		}
		public LongArray callback
		{
			get
			{
				LongArray array;
				array.__fastMode = this.Array;
				array.__start = callbackPtr & 0xFFFFF;
				array.__length = (callbackPtr >> 20) & 0xFFF;
				array.__end = array.__length + array.__start;
				return array;
			}
		}
		public int huaiJiuSuit
		{
			get
			{
				 return (int)__data.intValues[41 + handle.intOffset];
			}
			set
			{
				__data.intValues[41 + handle.intOffset] = (int)value;
			}
		}
		public string bufferParam
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
		public int saleType
		{
			get
			{
				 return (int)__data.intValues[42 + handle.intOffset];
			}
			set
			{
				__data.intValues[42 + handle.intOffset] = (int)value;
			}
		}
		public int showType
		{
			get
			{
				 return (int)__data.intValues[43 + handle.intOffset];
			}
			set
			{
				__data.intValues[43 + handle.intOffset] = (int)value;
			}
		}
		public int currencyType
		{
			get
			{
				 return (int)__data.intValues[44 + handle.intOffset];
			}
			set
			{
				__data.intValues[44 + handle.intOffset] = (int)value;
			}
		}
		public int recommend
		{
			get
			{
				 return (int)__data.intValues[45 + handle.intOffset];
			}
			set
			{
				__data.intValues[45 + handle.intOffset] = (int)value;
			}
		}
		public int zhanHunSuit
		{
			get
			{
				 return (int)__data.intValues[46 + handle.intOffset];
			}
			set
			{
				__data.intValues[46 + handle.intOffset] = (int)value;
			}
		}
		public string getWay
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
		public int funcopen
		{
			get
			{
				 return (int)__data.intValues[47 + handle.intOffset];
			}
			set
			{
				__data.intValues[47 + handle.intOffset] = (int)value;
			}
		}
		public int uniondonate
		{
			get
			{
				 return (int)__data.intValues[48 + handle.intOffset];
			}
			set
			{
				__data.intValues[48 + handle.intOffset] = (int)value;
			}
		}
		public string effect
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
		public int group
		{
			get
			{
				 return (int)__data.intValues[49 + handle.intOffset];
			}
			set
			{
				__data.intValues[49 + handle.intOffset] = (int)value;
			}
		}
		public int limit
		{
			get
			{
				 return (int)__data.intValues[50 + handle.intOffset];
			}
			set
			{
				__data.intValues[50 + handle.intOffset] = (int)value;
			}
		}
		public int itemcd
		{
			get
			{
				 return (int)__data.intValues[51 + handle.intOffset];
			}
			set
			{
				__data.intValues[51 + handle.intOffset] = (int)value;
			}
		}
		public int randomLibrary
		{
			get
			{
				 return (int)__data.intValues[52 + handle.intOffset];
			}
			set
			{
				__data.intValues[52 + handle.intOffset] = (int)value;
			}
		}
		public int Operationtype
		{
			get
			{
				 return (int)__data.intValues[53 + handle.intOffset];
			}
			set
			{
				__data.intValues[53 + handle.intOffset] = (int)value;
			}
		}
	}
	public static class ITEMHelper
	{
		public static void Encode(this System.IO.Stream stream,ITEM item)
		{
			stream.Encode(item.id);
			stream.Encode(item.name);
			stream.Encode(item.type);
			stream.Encode(item.subType);
			stream.Encode(item.overlaying);
			stream.Encode(item.levClass);
			stream.Encode(item.level);
			stream.Encode(item.quality);
			stream.Encode(item.exValue);
			stream.Encode(item.tips);
			stream.Encode(item.icon);
			stream.Encode(item.img);
			stream.Encode(item.sex);
			stream.Encode(item.career);
			stream.Encode(item.model);
			stream.Encode(item.data);
			stream.Encode(item.phyAttMax);
			stream.Encode(item.phyAttMin);
			stream.Encode(item.magicAttMax);
			stream.Encode(item.magicAttMin);
			stream.Encode(item.taoAttMax);
			stream.Encode(item.taoAttMin);
			stream.Encode(item.phyDefMax);
			stream.Encode(item.phyDefMin);
			stream.Encode(item.magicDefMax);
			stream.Encode(item.magicDefMin);
			stream.Encode(item.accurate);
			stream.Encode(item.dodge);
			stream.Encode(item.curse);
			stream.Encode(item.luck);
			stream.Encode(item.criticalDamage);
			stream.Encode(item.critical);
			stream.Encode(item.hp);
			stream.Encode(item.mp);
			stream.Encode(item.holyAtt);
			stream.Encode(item.pkAtt);
			stream.Encode(item.pkDef);
			stream.Encode(item.resistanceCrit);
			stream.Encode(item.Destroy);
			stream.Encode(item.dropShow);
			stream.Encode(item.binding);
			stream.Encode(item.notPrompt);
			stream.Encode(item.timeLimit);
			stream.Encode(item.pickUpType);
			stream.Encode(item.wolongLv);
			stream.Encode(item.tips2);
			stream.Encode(item.callbackPtr);
			stream.Encode(item.huaiJiuSuit);
			stream.Encode(item.bufferParam);
			stream.Encode(item.saleType);
			stream.Encode(item.showType);
			stream.Encode(item.currencyType);
			stream.Encode(item.recommend);
			stream.Encode(item.zhanHunSuit);
			stream.Encode(item.getWay);
			stream.Encode(item.funcopen);
			stream.Encode(item.uniondonate);
			stream.Encode(item.effect);
			stream.Encode(item.group);
			stream.Encode(item.limit);
			stream.Encode(item.itemcd);
			stream.Encode(item.randomLibrary);
			stream.Encode(item.Operationtype);
		}
	}
	public class ITEMARRAY  : ILFastMode
	{
		public ITEMARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 54;
			StringValueFixedLength = 9;
			Rules = new byte[]{1,2,1,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,2,1,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,1};
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
				TABLE.ITEM randAttr = new TABLE.ITEM();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
