using System.Collections.Generic;
using UnityEngine;

public partial class UIFastUsePanel : UIBasePanel
{

    //public override PrefabTweenType PanelTweenType
    //{
    //    get => PrefabTweenType.None;
    //}
    //public override bool ShowGaussianBlur
    //{
    //    get { return false; }
    //}

    //List<List<int>> costList;
    //List<QuickUseItem> itemList;
    //List<List<int>> costDic;
    ////int ind = 0;
    //string playerPrefKey;

    //public override void Init()
    //{
    //    UIPrefab.transform.localPosition = new Vector3(-30, 0, 0);
    //    costList = mPoolHandleManager.GetSystemClass<List<List<int>>>();
    //    costList = UtilityMainMath.SplitStringToIntLists(SundryTableManager.Instance.GetSundryEffect(612));
    //    costDic = mPoolHandleManager.GetSystemClass<List<List<int>>>();
    //    itemList = mPoolHandleManager.GetSystemClass<List<QuickUseItem>>();
    //    //mClientEvent.AddEvent(CEvent.ItemChange, GetItemChange);

    //    base.Init();
    //    ResetCostList();
    //    playerPrefKey = $"{CSMainPlayerInfo.Instance.Name}quickUseId";
    //    int v = PlayerPrefs.GetInt(playerPrefKey);
    //    mClientEvent.AddEvent(CEvent.MainPlayer_LevelChange, GetLevelChange);
    //    mClientEvent.AddEvent(CEvent.ItemListChange, GetLevelChange);
    //    UIEventListener.Get(mobj_shield).onClick = CloseClick;
    //}

    //public override void Show()
    //{
    //    base.Show();
    //}

    //protected override void OnDestroy()
    //{
    //    base.OnDestroy();
    //}
    //void GetLevelChange(uint id, object data)
    //{
    //    ResetCostList();
    //}
    //void ResetCostList()
    //{
    //    costDic.Clear();
    //    //ind = 0;
    //    for (int i = 0; i < costList.Count; i++)
    //    {
    //        if (costList[i][1] <= CSMainPlayerInfo.Instance.Level)
    //        {
    //            costDic.Add(costList[i]);
    //        }
    //    }
    //    for (int i = 0; i < itemList.Count; i++)
    //    {
    //        mPoolHandleManager.Recycle(itemList[i]);
    //    }
    //    itemList.Clear();
    //    mgrid_items.MaxCount = costDic.Count;
    //    for (int i = 0; i < costDic.Count; i++)
    //    {
    //        QuickUseItem temp_item = mPoolHandleManager.GetCustomClass<QuickUseItem>();
    //        //temp_item.Init(mgrid_items.controlList[i], QuickItemClick);
    //        itemList.Add(temp_item);
    //        itemList[i].Refresh(costDic[i]);
    //        itemList[i].ChangeBgState(i);
    //    }
    //    int heigh = (costDic.Count >= 6) ? 6 * 62 + 8 - 31 : costDic.Count * 62 + 8;
    //    msp_bg.height = heigh;
    //}

    //void QuickItemClick(QuickUseItem _item)
    //{
    //    //curShowId = _item.cfgId;
    //    PlayerPrefs.SetInt(playerPrefKey, _item.cfgId);
    //    //RefreshShowItemIcon();
    //    //ChangeUsePanelState();
    //    mClientEvent.SendEvent(CEvent.FastUseClick, _item.cfgId);
    //    if (_item.count <= 0)
    //    {
    //        Utility.ShowGetWay(_item.cfgId);
    //    }
    //    mClientEvent.SendEvent(CEvent.FastUseClose);
    //    UIManager.Instance.ClosePanel<UIFastUsePanel>();
    //}
    //void CloseClick(GameObject _go)
    //{
    //    mClientEvent.SendEvent(CEvent.FastUseClose);
    //    UIManager.Instance.ClosePanel<UIFastUsePanel>();
    //}

    //void UpdateFillAmount(float value)
    //{
    //    //mcdmask.CustomActive(true);
    //    //mcdmask.fillAmount = (1.0f - value);
    //    //mcdtime.text = ((1.0f - value) * currentItemCfg.itemcd * 0.001f).ToString("F1");
    //}

    //void StopCD()
    //{
    //    //mcdmask.CustomActive(false);
    //}
}
