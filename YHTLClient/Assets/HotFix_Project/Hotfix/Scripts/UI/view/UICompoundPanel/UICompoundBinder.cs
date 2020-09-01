using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class UICompoundBinder : UIBinder
{
    public bool isSelect;
    public bool isOpen;
    public Action<int, bool> actionGroup;
    public Action<int> actionItem;
    public CompoundGroupData compoundGroupData;

    public int selectItemId;
    
    private UIEventListener mainTempListener;
    private UILabel lb_name;
    private GameObject redpoint;
    private UIGridContainer grid_sub;
    private List<UIItemBase> listItemBases;
    private UILabel checkmarkName;
    private GameObject checkmark;

    public override void Init(UIEventListener handle)
    {
        mainTempListener = handle.GetComponent<UIEventListener>();
        checkmark = Get<GameObject>("checkmark");
        checkmarkName = Get<UILabel>("name", checkmark.transform);
        lb_name = Get<UILabel>("name");
        redpoint = Get<GameObject>("redpoint");
        grid_sub = Get<UIGridContainer>("grid_sub");

        mainTempListener.onClick = OnClickGroup;
    }

    public override void Bind(object data)
    {
        if (data == null) return;
        compoundGroupData = data as CompoundGroupData;
        RefreshUI();
    }

    void RefreshUI()
    {
        checkmark.SetActive(isOpen);
        redpoint.SetActive(compoundGroupData.IsHasCombined);
        checkmarkName.text = compoundGroupData.GroupName;
        lb_name.text = compoundGroupData.GroupName;
        if (!isSelect || (isSelect && !isOpen))
        {
            grid_sub.MaxCount = 0;
        }
        else
        {
            grid_sub.MaxCount = compoundGroupData.GenerateItems.Count;
            GameObject gp;
            UILabel name;
            GameObject Item;
            GameObject red;
            if (listItemBases == null)
            {
                listItemBases = new List<UIItemBase>();
            }

            for (int i = 0; i < grid_sub.MaxCount; i++)
            {
                gp = grid_sub.controlList[i];
                name = gp.transform.Find("name").GetComponent<UILabel>();
                Item = gp.transform.Find("Item").gameObject;
                red = gp.transform.Find("redpoint").gameObject;

                red.SetActive(compoundGroupData.GenerateItems[i].IsCombine);
                gp.GetComponent<UIToggle>().Set(compoundGroupData.GenerateItems[i].ItemId == selectItemId);
                name.text = compoundGroupData.GenerateItems[i].Name;
                name.color = UtilityCsColor.Instance.GetColor(
                    ItemTableManager.Instance.GetItemQuality(compoundGroupData.GenerateItems[i].ItemId));

                if (listItemBases.Count <= i)
                    listItemBases.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, Item.transform,
                        itemSize.Size64));

                UIItemBase itemBase = listItemBases[i];
                itemBase.Refresh(compoundGroupData.GenerateItems[i].ItemId);
                itemBase.SetCount(CSBagInfo.Instance.GetAllItemCount(compoundGroupData.GenerateItems[i].ItemId), true);
                UIEventListener.Get(gp, i).onClick = OnClickItem;
            }
        }
    }

    void OnClickGroup(GameObject go)
    {
        actionGroup?.Invoke(compoundGroupData.GroupId, isOpen);
    }

    void OnClickItem(GameObject go)
    {
        if (go == null) return;
        int index = (int) UIEventListener.Get(go).parameter;
        actionItem?.Invoke(compoundGroupData.GenerateItems[index].ItemId);
    }

    public override void OnDestroy()
    {
        actionGroup = null;
        actionItem = null;
        compoundGroupData = null;
        lb_name = null;
        redpoint = null;
        grid_sub = null;
        checkmarkName = null;
        checkmark = null;
        UIItemManager.Instance.RecycleItemsFormMediator(listItemBases);
    }
}