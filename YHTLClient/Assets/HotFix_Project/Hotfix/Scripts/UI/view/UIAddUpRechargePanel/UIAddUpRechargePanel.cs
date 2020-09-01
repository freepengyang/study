using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;
using vip;

public partial class UIAddUpRechargePanel : UIBasePanel
{
	private bool isFirst = true;
	public override void Init()
	{
		base.Init();
		mClientEvent.AddEvent(CEvent.AddUpInfoChange,RefreshUI);
		//mClientEvent.AddEvent(CEvent.ResPlayerInfoMessage,RefreshRecharge);
		CSEffectPlayMgr.Instance.ShowUITexture(mbanner18.gameObject, "banner18");
		UIEventListener.Get(mbtn_recharge).onClick = OnClickRecharge;
		UIEventListener.Get(mbtn_Tips).onClick = OnClickTips;
		for (int i = 0; i < mButtonGroup.MaxCount; i++)
		{
			UIEventListener.Get(mButtonGroup.controlList[i],i).onClick = OnClickGameModel;
		}
		
	}

	private void OnClickGameModel(GameObject obj)
	{
		int index = (int) UIEventListener.Get(obj).parameter;
		mbtn_Tips.SetActive(false);
		switch (index)
		{
			case 0:
				UtilityPanel.JumpToPanel(12600);
				break;
			case 1:
				UtilityPanel.JumpToPanel(12305);
				UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
				//Close();
				break;
			case 2:
				UtilityPanel.JumpToPanel(12613);
				break;
			case 3:
				UtilityPanel.JumpToPanel(12602);
				break;
			default:
				break;
		}
	}

	public override void Show()
	{
		base.Show();
		isFirst = true;
		RefreshUI();
		isFirst = false;
		
	}

	private void RefreshUI(uint uievtid = 0, object data = null)
	{
		int day = CSVipInfo.Instance.GetAddUpDay();
		string daystr = day == 0
			? CSString.Format(1753, day).BBCode(ColorType.Red)
			: CSString.Format(1753, day).BBCode(ColorType.Green);
		bool isRecharge = CSMainPlayerInfo.Instance.RoleExtraValues != null && CSMainPlayerInfo.Instance.RoleExtraValues.todayTimes != 0;
		string strRecharge = isRecharge ? CSString.Format(1829) :CSString.Format(1830); 
		
		mlb_day.text =$"{daystr}{strRecharge}" ;
		mbtn_recharge.SetActive(!isRecharge);
		mlb_hint.text =  CSString.Format(1776);
		var list = CSVipInfo.Instance.GetRewardInfoList();
		
		if (list != null)
		{
			int index = CSVipInfo.Instance.GetFirstReward();
			mgrid_days.MaxCount = list.Count;
			mgrid_days.Bind<RECHARGEREWARD,DayItem>(list,mPoolHandleManager);
			float value = Mathf.Clamp01(index / (list.Count - 3.5f));
			if (isFirst)
			{
				TweenProgressBar.Begin(mscollbar,0.3f,value,value);
			}
			else
			{
				TweenProgressBar.Begin(mscollbar,0.3f,mscollbar.value,value);
			}
		}
		
	}

	protected override void OnDestroy()
	{
		mgrid_days.UnBind<DayItem>();
		mClientEvent.RemoveEvent(CEvent.AddUpInfoChange,RefreshUI);
		base.OnDestroy();	
	}


    public override void OnHide()
    {
        base.OnHide();
        mgrid_days.UnBind<DayItem>();
    }

    private void OnClickTips(GameObject obj)
	{
		mbtn_Tips.SetActive(false);
	}

	private void OnClickRecharge(GameObject obj)
	{
		mbtn_Tips.SetActive(true);
	}
	
}

public class DayItem : UIBinder
{
	public UILabel lb_day;
	public UISprite btn_buy;
	public Transform Item;
	public GameObject sp_soldout;
	private RECHARGEREWARD _rechargereward;
	private Dictionary<int, int> mapItem;
	private UIItemBase _itemBase;
	private GameObject effect;
	private UILabel btn_label;
	
	//private UITexture tex_bg;
	
	//private FastArrayElementFromPool<UIItemBase> items;
	public override void Init(UIEventListener handle)
	{
		Transform trans = handle.transform;
		//tex_bg = handle.GetComponent<UITexture>();
		lb_day = Get<UILabel>("lb_day",trans);
		btn_buy = Get<UISprite>("btn_buy", trans);
		btn_label =  Get<UILabel>("Label", btn_buy.transform);
		Item = Get<Transform>("Item", trans);
		sp_soldout = Get<GameObject>("sp_soldout", trans);
		effect = Get<GameObject>("btn_buy/effect",trans);
		CSEffectPlayMgr.Instance.ShowUIEffect(effect, "effect_button_blue_add");
		//mapItem = PoolHandle.GetSystemClass<Map<int,int>>();
		mapItem = new Dictionary<int, int>();
		var items = PoolHandle.CreateItemPool(PropItemType.Normal, Item,8);
		_itemBase = items.Append();
		CSEffectPlayMgr.Instance.ShowUITexture(handle.gameObject, "add_up_recharge_bg");
		UIEventListener.Get(btn_buy.gameObject).onClick = OnReceiveClick;
	}

	private void OnReceiveClick(GameObject obj)
	{
			if (CSVipInfo.Instance.GetAddUpDay() >= _rechargereward.need)
		{
			Net.CSReceiveAccumulatedRechargeRewardMessage(_rechargereward.id);
		}
		else
		{
			UtilityTips.ShowRedTips(CSString.Format(1797));
		}
	}	

	public override void Bind(object data)
	{
		_rechargereward = (RECHARGEREWARD)data;
		int needDay = _rechargereward.need;
		lb_day.text = CSString.Format(1753,needDay);
		var receiveIds = CSVipInfo.Instance.GetReceiveIds();
		//显示按钮
		bool isFinish = CSVipInfo.Instance.GetAddUpDay() >= needDay; 
		bool isReceive = false;
		if (receiveIds != null)

		{
			isReceive = receiveIds.Contains(_rechargereward.id);
		}
		btn_buy.spriteName = isFinish ? "btn_samll1" : "btn_nbig4";
		btn_label.color = UtilityColor.GetColor(isFinish ? ColorType.CommonButtonGreen : ColorType.CommonButtonGrey);
		effect.SetActive(isFinish);
		btn_buy.gameObject.SetActive(!isReceive);
		sp_soldout.SetActive(isFinish && isReceive);
		mapItem.Clear();
		BoxTableManager.Instance.GetBoxAwardById(_rechargereward.box, mapItem);
		//显示item 
		if (mapItem.Count == 1)
		{
			for (var iter = mapItem.GetEnumerator(); iter.MoveNext();)
			{
				_itemBase.Refresh(iter.Current.Key);
				_itemBase.SetCount(iter.Current.Value);
			}
		}
	}

	public override void OnDestroy()
	{
		lb_day = null;
		btn_buy = null;
		Item = null;
		sp_soldout = null;
		CSEffectPlayMgr.Instance.Recycle(effect);
	}
}
