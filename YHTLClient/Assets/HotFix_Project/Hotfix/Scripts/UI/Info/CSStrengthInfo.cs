using System.Collections.Generic;
using TABLE;

public class CSStrengthInfo : CSInfo<CSStrengthInfo>
{
	PoolHandleManager mPoolHandle = new PoolHandleManager();
	Dictionary<int, List<int>> strongDic;
	string[] titList;
	public void SetStrengthenData()
	{
		if (strongDic == null)
		{
			strongDic = new Dictionary<int, List<int>>();
			var arr = BeStrongTableManager.Instance.array.gItem.handles;
			int title1 = 0;
			for (int k = 0, max = arr.Length; k < max; ++k)
			{
				var item = arr[k].Value as BESTRONG;
				if (!strongDic.ContainsKey(item.title1))
				{
					List<int> idList = mPoolHandle.GetSystemClass<List<int>>();
					title1 = item.title1;
					strongDic.Add(title1, idList);
				}
				if(item.title1 == title1)
				{
					strongDic[title1].Add(item.id);
				}
			}
		}
		if(titList == null)
		{
			string tempStr = CSString.Format(1760);
			titList = UtilityMainMath.StrToStrArr(tempStr);
		}
	}
	public Dictionary<int, List<int>> GetDealWithStrongDic()
	{
		return strongDic;
	}
	public string[] GetTitList()
	{
		return titList;
	}

	public override void Dispose()
	{
		mPoolHandle?.OnDestroy();
		strongDic = null;
		mPoolHandle = null;
		titList = null;
	}
}