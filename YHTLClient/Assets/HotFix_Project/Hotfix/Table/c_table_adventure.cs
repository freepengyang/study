namespace TABLE
{
	public partial class ADVENTURE
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
		public int zhanhun
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
		public int level
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
		public string exp
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
		public string yinzi
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
		public string item
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
		public string bossProbability
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
		public int bossDrop
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
		public int bossNum
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
		public string mmodel1
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
		public string mmodel2
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
		public string mmodel3
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
		public string bossmodel1
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
		public string bossmodel2
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
		public string bossmodel3
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
	}
	public static class ADVENTUREHelper
	{
		public static void Encode(this System.IO.Stream stream,ADVENTURE item)
		{
			stream.Encode(item.id);
			stream.Encode(item.zhanhun);
			stream.Encode(item.level);
			stream.Encode(item.exp);
			stream.Encode(item.yinzi);
			stream.Encode(item.item);
			stream.Encode(item.bossProbability);
			stream.Encode(item.bossDrop);
			stream.Encode(item.bossNum);
			stream.Encode(item.mmodel1);
			stream.Encode(item.mmodel2);
			stream.Encode(item.mmodel3);
			stream.Encode(item.bossmodel1);
			stream.Encode(item.bossmodel2);
			stream.Encode(item.bossmodel3);
		}
	}
	public class ADVENTUREARRAY  : ILFastMode
	{
		public ADVENTUREARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 5;
			StringValueFixedLength = 10;
			Rules = new byte[]{1,1,1,2,2,2,2,1,1,2,2,2,2,2,2};
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
				TABLE.ADVENTURE randAttr = new TABLE.ADVENTURE();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
