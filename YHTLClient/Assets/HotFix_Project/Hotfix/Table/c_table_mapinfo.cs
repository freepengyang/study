namespace TABLE
{
	public partial class MAPINFO
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
		public string mapSize
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
		public string mapSize1
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
		public int img
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
		public int pkMode
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
		public int bgm
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
		public int relivePanelType
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
		public string reliveCoord
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
		public int reliveTime
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
		public int mapPKMode
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
		public int cancross
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
		public int tips
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
		public int firstTransfer
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
		public int banTransfer
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
		public int withHero
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
		public string limit
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
		public string totalTime
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
		public string openEvent
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
		public string limitTimes
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
		public string home
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
		public string param1
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
		public string param2
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
		public string desc
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
	}
	public static class MAPINFOHelper
	{
		public static void Encode(this System.IO.Stream stream,MAPINFO item)
		{
			stream.Encode(item.id);
			stream.Encode(item.name);
			stream.Encode(item.type);
			stream.Encode(item.mapSize);
			stream.Encode(item.mapSize1);
			stream.Encode(item.img);
			stream.Encode(item.pkMode);
			stream.Encode(item.bgm);
			stream.Encode(item.relivePanelType);
			stream.Encode(item.reliveCoord);
			stream.Encode(item.reliveTime);
			stream.Encode(item.mapPKMode);
			stream.Encode(item.cancross);
			stream.Encode(item.tips);
			stream.Encode(item.firstTransfer);
			stream.Encode(item.banTransfer);
			stream.Encode(item.withHero);
			stream.Encode(item.limit);
			stream.Encode(item.totalTime);
			stream.Encode(item.openEvent);
			stream.Encode(item.limitTimes);
			stream.Encode(item.home);
			stream.Encode(item.param1);
			stream.Encode(item.param2);
			stream.Encode(item.desc);
		}
	}
	public class MAPINFOARRAY  : ILFastMode
	{
		public MAPINFOARRAY()
		{
			LongValueFixedLength = 0;
			IntValueFixedLength = 13;
			StringValueFixedLength = 12;
			Rules = new byte[]{1,2,1,2,2,1,1,1,1,2,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2};
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
				TABLE.MAPINFO randAttr = new TABLE.MAPINFO();
				handle = handles[i];
				randAttr.__data = handle.data;
				randAttr.Array = this;
				handle.Value = randAttr;
				randAttr.handle = handle;
			}
		}
	}
}
