using UnityEngine;

public partial class UIGiveMeIngotPanel : UIBasePanel
{
	int ingotId = 0;
	int allIngot = 0;
	int allLimitIngot = 0;
	string icon = string.Empty;
	ILBetterList<GetWayData> costWayList;
	ILBetterList<GiveMeIngotData> giveIngotList;
	public override PrefabTweenType PanelTweenType
	{
		get => PrefabTweenType.FirstPanel;
	}
	public override void Init()
	{
		base.Init();
		mBtnClose.onClick = OnBtnClose;
		mClientEvent.AddEvent(CEvent.SetIngotInfo, RefreshIngotUI);		//��˷���Ԫ����Ϣˢ��
		mClientEvent.AddEvent(CEvent.PetTalentLvChange, RefreshIngotUI);//�츳����ˢ��
		mClientEvent.AddEvent(CEvent.MainPlayer_LevelChange, RefreshIngotUI);//����ˢ��

		SetMoneyIds(1, 4);
	}
	public override void Show()
	{
		base.Show();
		RefreshIngotUI(0,null);
	}
	private void RefreshIngotUI(uint id, object argv)
	{
		CommonData();
		RefreshUI();
	}
	#region ����
	private void CommonData()
	{
		if(ingotId == 0)
		{
			ingotId = CSGiveMeIngotInfo.Instance.GetIngotId();
			icon = ItemTableManager.Instance.GetItemIcon(ingotId);
			icon = $"tubiao{icon}";
		}
		allIngot = CSGiveMeIngotInfo.Instance.GetAllIngot();
		allLimitIngot = CSGiveMeIngotInfo.Instance.GetAllLimitIngot();
		if (costWayList == null)
			costWayList = CSGiveMeIngotInfo.Instance.GetCostWayList();

		giveIngotList = CSGiveMeIngotInfo.Instance.GetGiveIngotList();
		if (giveIngotList == null) return;
		//���ս���츳�ȼ�����ҵȼ����޸�����
		int isCheck = 0;
		for (int i = 0; i < giveIngotList.Count; i++)
		{
			if (!giveIngotList[i].isShowBtn)
			{
				isCheck = 1;
				break;
			}
		}
		if (isCheck > 0)
			CSGiveMeIngotInfo.Instance.SetDicLimit();
	}
	#endregion
	private void RefreshUI()
	{
		//Ԫ�����ֺͱ���
		CSEffectPlayMgr.Instance.ShowUITexture(mIngotBg, "ingot_bg");
		mGetIcon.spriteName = icon;
		mLimitIcon.spriteName = icon;
		mLbGetNum.text = $"{allIngot}";
		mLbLimitNum.text = $"{allLimitIngot}";
		//����;��
		mTextGrid.Bind<GetWayData, CostWayBtnByIngot>(costWayList, mPoolHandleManager);
		//ˢ�����
		mRightGrid.Bind<GiveMeIngotData, GiveMeIngotItem>(giveIngotList, mPoolHandleManager);
	}
	#region ��ť
	private void OnBtnClose(GameObject _go)
	{
		Close();
	}
	#endregion
	protected override void OnDestroy()
	{
		if(mIngotBg != null)
			CSEffectPlayMgr.Instance.Recycle(mIngotBg);
		if(mClientEvent != null)
		{
			mClientEvent.RemoveEvent(CEvent.SetIngotInfo, RefreshIngotUI);
			mClientEvent.RemoveEvent(CEvent.PetTalentLvChange, RefreshIngotUI);
			mClientEvent.RemoveEvent(CEvent.MainPlayer_LevelChange, RefreshIngotUI);
		}
		mRightGrid.UnBind<GiveMeIngotItem>();
		mTextGrid.UnBind<CostWayBtnByIngot>();
		ingotId = 0;
		allIngot = 0;
		allLimitIngot = 0;
		icon = "";
		base.OnDestroy();
	}
	private class GiveMeIngotItem : UIBinder
	{
		UILabel lb_way, lb_desc, lb_hint, lb_num;
		UISlider slider;
		GameObject btn_go, sliderObj;
		GiveMeIngotData mData;
		UIEventListener goBtn;

		float fillAmount = 0;
		public override void Init(UIEventListener handle)
		{
			lb_way = Get<UILabel>("lb_way");
			lb_desc = Get<UILabel>("lb_des");
			lb_hint = Get<UILabel>("lb_hint");
			slider = Get<UISlider>("slider");
			sliderObj = Get<GameObject>("slider");
			lb_num = Get<UILabel>("slider/lb_num");
			btn_go = Get<GameObject>("btn_go");
			goBtn = UIEventListener.Get(btn_go);
			goBtn.onClick = OnBtnGo;
		}
		public override void Bind(object data)
		{
			mData = data as GiveMeIngotData;
			if (mData == null) return;
			lb_way.text = mData.name;
			lb_desc.text = mData.desc;
			//��������
			sliderObj.CustomActive(mData.maxValue > 0);
			if(mData.maxValue > 0)
			{
				if (mData.value > 0 && mData.value < mData.maxValue)
					fillAmount = (float)mData.value / mData.maxValue;
				else if (mData.value >= mData.maxValue)
					fillAmount = 1f;
				lb_num.text = $"{mData.value}/{mData.maxValue}";
				slider.value = fillAmount;
			}
			//��ⰴť
			btn_go.CustomActive(mData.isShowBtn);
			lb_hint.gameObject.CustomActive(!mData.isShowBtn);
			if (lb_hint.gameObject.activeSelf)
				lb_hint.text = mData.limitDesc;
		}

		private void OnBtnGo(GameObject _go)
		{
			int gameModelId = mData.gameModelId;
			int deliverId = mData.deliverId;
			if (gameModelId > 0)
				UtilityPanel.JumpToPanel(gameModelId);
			else if (deliverId > 0)
				UtilityPath.FindWithDeliverId(deliverId);
			UIManager.Instance.ClosePanel<UIGiveMeIngotPanel>();
		}
		public override void OnDestroy()
		{
			if (null != goBtn)
			{
				goBtn.onClick = null;
				goBtn = null;
			}
			lb_way = null;
			lb_desc = null;
			lb_hint = null;
			slider = null;
			sliderObj = null;
			btn_go = null;
			mData = null;

			fillAmount = 0;
		}
	}
}
public class CostWayBtnByIngot : GetWayBtn
{
	public override void OpenPanelCallback(GameObject obj)
	{
		TABLE.GETWAY getWay = (TABLE.GETWAY)UIEventListener.Get(obj).parameter;
		int funcId = (int)getWay.function;
		UtilityPanel.JumpToPanel(funcId);
		UIManager.Instance.ClosePanel<UIGiveMeIngotPanel>();
	}
}