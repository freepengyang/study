using UnityEngine;

public partial class UIIngotHeadPanel : UIBasePanel
{
	public override void Init()
	{
		base.Init();
		mBtnIcon.onClick = BtnIcon;
		HotManager.Instance.EventHandler.AddEvent(CEvent.SetIngotInfo, RefreshFillAmount);
		mClientEvent.AddEvent(CEvent.Role_ChangeMapId, EnterMap);//玩家地图更改
		mClientEvent.AddEvent(CEvent.FirstShowIngotHead, EnterMap);//首次登陆游戏检测
		UICheckManager.Instance.RegBtnAndCheck(FunctionType.funcP_GiveIngot, mUIIngotHeadPanel);
	}
	public override void Show()
	{
		RefreshFillAmount(0,null);
	}
	private void EnterMap(uint id, object argv)
	{
		if (UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_GiveIngot))
		{
			int mapId = CSMainPlayerInfo.Instance.MapID;
			TABLE.INSTANCE instance = null;
			if (InstanceTableManager.Instance.TryGetValue(mapId, out instance))
				mUIIngotHeadPanel.CustomActive(false);
			else
				mUIIngotHeadPanel.CustomActive(true);
		}
		else
			mUIIngotHeadPanel.CustomActive(false);
	}
	private void RefreshFillAmount(uint id, object argv)
	{
		float fillAmount = 0;
		int getIngot = CSGiveMeIngotInfo.Instance.GetAllIngot();
		int allIngot = CSGiveMeIngotInfo.Instance.GetAllLimitIngot();
		if (getIngot > 0 && getIngot < allIngot)
			fillAmount = (float)getIngot / allIngot;
		else if (getIngot >= allIngot)
			fillAmount = 1f;
		mSpHp.fillAmount = fillAmount;
	}
	private void BtnIcon(GameObject _go)
	{
		UIManager.Instance.CreatePanel<UIGiveMeIngotPanel>();
	}
	protected override void OnDestroy()
	{
		HotManager.Instance.EventHandler.RemoveEvent(CEvent.SetIngotInfo, RefreshFillAmount);
		if (mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.Role_ChangeMapId, EnterMap);
			mClientEvent.RemoveEvent(CEvent.FirstShowIngotHead, EnterMap);
		}
		base.OnDestroy();
	}
}