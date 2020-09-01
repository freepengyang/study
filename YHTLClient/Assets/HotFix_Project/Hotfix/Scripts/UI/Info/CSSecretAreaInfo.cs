using System.Collections.Generic;
using Google.Protobuf.Collections;
public class CSSecretAreaInfo : CSInfo<CSSecretAreaInfo>
{
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	Dictionary<int, SecretAreaData> secretAreaDic;
	List<int> instanceIdList;
	string instanceIdStr;
	string[] imgStr;
	int itemIdx = 0;
	/// <summary>
	/// 副本数据处理
	/// </summary>
	public void SetMemoryInstanceInfo(memory.MemoryInstanceInfo msg)
	{
		instanceIdStr = SundryTableManager.Instance.GetSundryEffect(1051);
		instanceIdList = UtilityMainMath.SplitStringToIntList(instanceIdStr);
		if (secretAreaDic == null) secretAreaDic = new Dictionary<int, SecretAreaData>();
		int mapId = 0;
		TABLE.INSTANCE instance = null;
		for(int i =0;i< instanceIdList.Count;i++)
		{
			mapId = instanceIdList[i];
			if (secretAreaDic.ContainsKey(mapId))
			{
				secretAreaDic[mapId].isFree = mapId == msg.freeInstanceId;
			}
			else
			{
				if (InstanceTableManager.Instance.TryGetValue(mapId, out instance))
				{
					imgStr = UtilityMainMath.StrToStrArr(instance.img);
					SecretAreaData data = mPoolHandle.GetCustomClass<SecretAreaData>();
					data.mapName = instance.mapName;
					data.mapLv = instance.openLevel;
					data.show = instance.show;
					data.select = imgStr[0];
					data.head = imgStr[1];
					data.bg = imgStr[2];
					data.mapDesc = MapInfoTableManager.Instance.GetMapInfoDesc(mapId);
					data.isFree = mapId == msg.freeInstanceId;
					secretAreaDic.Add(mapId,data);
				}
			}
			if (mapId == msg.freeInstanceId)
				itemIdx = i;
		}
	}
	public int GetItemIdx()
	{
		return itemIdx;
	}
	/// <summary>
	/// 副本表id列表
	/// </summary>
	/// <returns></returns>
	public List<int> GetInstanceList()
	{
		return instanceIdList;
	}
	public Dictionary<int, SecretAreaData> GetSecretAreaDic()
	{
		return secretAreaDic;
	}
	public override void Dispose()
    {
		itemIdx = 0;
		instanceIdStr = "";
		instanceIdList?.Clear();
		mPoolHandle?.OnDestroy();
		
		secretAreaDic = null;
		instanceIdList = null;
		mPoolHandle = null;
		imgStr = null;
	}
}
public class SecretAreaData:IDispose
{
	public int mapLv { set; get; }
	public string mapName { set; get; }
	public string mapDesc { set; get; }
	public string show { set; get; }
	public string head { set; get; }
	public string select { set; get; }
	public string bg { set; get; }
	public bool isFree { set; get; }
	public void Dispose()
	{
		mapLv = 0;
		mapName = "";
		mapDesc = "";
		show = "";
		head = "";
		select = "";
		bg = "";
		isFree = false;
	}
}