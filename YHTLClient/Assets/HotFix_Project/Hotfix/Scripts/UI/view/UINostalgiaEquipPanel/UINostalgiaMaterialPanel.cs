using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class UINostalgiaMaterialPanel : UIBasePanel
{
	private CSNostalgiaEquipInfo Info;
	private Dictionary<NostalgiaMaterilalClass, NostalgiaBagClass> _dictionary;
	private List<NostalgiaBagClass> ShowList;
	
	public override void Init()
	{
		base.Init();
		AddCollider();
		UIEventListener.Get(mbtn_close).onClick = Close;
		UIEventListener.Get(mlb_go).onClick = OnShowGetWay;
		
		Info = CSNostalgiaEquipInfo.Instance;
		ShowList = mPoolHandleManager.GetSystemClass<List<NostalgiaBagClass>>();
		
		//将数据存于字典中便于回收
		_dictionary = mPoolHandleManager
			.GetSystemClass<Dictionary<NostalgiaMaterilalClass, NostalgiaBagClass>>();
		_dictionary.Clear();
		mClientEvent.AddEvent(CEvent.NostalgiaSelectItem,OnClosePanel);
	}

	private void OnClosePanel(uint uievtid, object data)
	{
		Close();
	}

	private void OnShowGetWay(GameObject obj)
	{
		Utility.ShowGetWay(SundryTableManager.Instance.GetSundryEffect(1063));
	}

	public override void Show()
	{
		base.Show();
		OpenPanel();
	}

	public void OpenPanel()
	{
		ShowList.Clear();
		var selectList = Info.curSelectlist;
		//已经选中的列表
		if (selectList.Count <= 0)
			return;
		
		int itemid = selectList[0].item.id;
		
		//背包中的堆叠列表
		List<NostalgiaBagClass> bagstackList;

		if (Info.bagStackList.ContainsKey(itemid))
			bagstackList = Info.bagStackList[itemid];
		else
			bagstackList = null;
		bool isnull;
		if (bagstackList == null)
		{
			isnull = true;
		}
		else
		{
			for (int i = 0; i < bagstackList.Count; i++)
			{
				var bagclass =  bagstackList[i];
				if (!selectList.Contains(bagclass))
					ShowList.Add(bagclass);
			}

			isnull = ShowList.Count <= 0;
		}

		
		mlb_go.gameObject.SetActive(isnull);
		mFull.gameObject.SetActive(!isnull);
		
		if (!isnull)
		{
			mgrid.MaxCount = ShowList.Count;
			
			for (int i = 0; i < mgrid.MaxCount; i++)
			{
				var gp = mgrid.controlList[i];
				var eventHandle = UIEventListener.Get(gp);
				NostalgiaMaterilalClass Binder;
				if (eventHandle.parameter == null)
				{
					Binder = new NostalgiaMaterilalClass();
					Binder.Setup(eventHandle);
				}
				else
					Binder = eventHandle.parameter as NostalgiaMaterilalClass;
				Binder.Bind(ShowList[i]);
			}
			
		}
		
	}


	protected override void OnDestroy()
	{
		for (var it = _dictionary.GetEnumerator(); it.MoveNext();)
		{
			mPoolHandleManager.Recycle(it.Current.Key);		
		}
		mPoolHandleManager.Recycle(_dictionary);
		mClientEvent.RemoveEvent(CEvent.NostalgiaSelectItem,OnClosePanel);
		base.OnDestroy();
	}
}

public class NostalgiaMaterilalClass : UIBinder
{
	public UILabel lb_itemname;
	public UILabel lb_state;
	public Transform bg;
	public Transform item;
	//public Transform sp_falg;
	private NostalgiaBagClass _info;
	private UIItemBase _itemBase;

	// public void Init(Transform trans,NostalgiaBagClass  Info)
	// {
	// 	bg = UtilityObj.Get<Transform>(trans,"bg");
	// 	item = UtilityObj.Get<Transform>(trans,"item");
	// 	//sp_falg = UtilityObj.Get<Transform>(trans,"sp_falg");
	// 	lb_state = UtilityObj.Get<UILabel>(trans,"lb_state");
	// 	lb_itemname = UtilityObj.Get<UILabel>(trans,"lb_itemname");
	// 	UIEventListener.Get(bg.gameObject).onClick = OnSelectClick;
	//
	// 	//lb_state.text = Utility.GetJob(itemTable.career);
	// 	
	// }

	private void OnSelectClick(GameObject obj)
	{
		//FNDebug.Log("NostalgiaSelectItem");
		HotManager.Instance.EventHandler.SendEvent(CEvent.NostalgiaSelectItem,_info);
		
	}

	public override void Init(UIEventListener handle)
	{
		var trans = handle.transform;
		bg = UtilityObj.Get<Transform>(trans,"bg");
		item = UtilityObj.Get<Transform>(trans,"item");
		//sp_falg = UtilityObj.Get<Transform>(trans,"sp_falg");
		lb_state = UtilityObj.Get<UILabel>(trans,"lb_state");
		lb_itemname = UtilityObj.Get<UILabel>(trans,"lb_itemname");
		UIEventListener.Get(bg.gameObject).onClick = OnSelectClick;
	}

	public override void Bind(object data)
	{
		_info = data as NostalgiaBagClass;
		_itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal,item);
		var itemTable = _info.item;
		_itemBase.Refresh(itemTable);
		lb_itemname.text = itemTable.name.BBCode(UtilityColor.GetColorTypeByQuality(itemTable.quality));
		lb_state.text = "";
	}

	public override void OnDestroy()
	{
		lb_itemname = null;
		lb_state = null;
		bg = null;
		item = null;
		_info = null;
		if (_itemBase != null)
			UIItemManager.Instance.RecycleSingleItem(_itemBase);
	}
}


