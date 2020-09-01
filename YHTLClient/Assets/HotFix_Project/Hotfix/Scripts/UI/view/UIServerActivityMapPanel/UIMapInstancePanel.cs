using System.Collections.Generic;
using UnityEngine;

public partial class UIMapInstancePanel : UIBasePanel
{
	List<int> paramList;
	string mapName, param2, desc;
	public override bool ShowGaussianBlur
	{
		get { return false; }
	}
	public override UILayerType PanelLayerType
	{
		get { return UILayerType.Resident; }
	}
	public override void Init()
	{
		base.Init();
		mClientEvent.AddEvent(CEvent.RefreshMapMonsterInfo, RefreshUI);
	}
	public override void Show()
	{
		base.Show();
		RefreshUI(0,null);
	}
	private void RefreshUI(uint id, object argv)
	{
		int mapId = CSMainPlayerInfo.Instance.MapID;
		TABLE.MAPINFO mapInfo = null;
		if(MapInfoTableManager.Instance.TryGetValue(mapId, out mapInfo))
		{
			mapName = mapInfo.name;
			param2 = mapInfo.param2;
			desc = mapInfo.desc;
			if(!string.IsNullOrWhiteSpace(param2))
			{
				paramList = UtilityMainMath.SplitStringToIntList(param2);
				int bossMax = paramList[0];
				int monsterMax = paramList[1];
				int bossNum = CSMapInstanceInfo.Instance.GetBossNum();
				int monsterNum = CSMapInstanceInfo.Instance.GetMonsterNum();
				GetNumStr(bossNum, bossMax, mLbBossNum);
				GetNumStr(monsterNum, monsterMax, mLbMonsterNum);
				mLbTitle.text = mapName;
				mLbTip.text = desc;
			}
		}
	}
	public void GetNumStr(int num,int maxNum,UILabel lb_num)
	{
		ColorType color = maxNum - num > 0 ? ColorType.Green:ColorType.Red;
		lb_num.text = $"{maxNum - num}/{maxNum}".BBCode(color);
	}
	protected override void OnDestroy()
	{
		if(mClientEvent != null)
			mClientEvent.RemoveEvent(CEvent.RefreshMapMonsterInfo, RefreshUI);
		paramList = null;
		mapName = "";
		param2 = "";
		desc = "";
		base.OnDestroy();
	}
}
