using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIPromptItemSplitPanel : UIBasePanel
{
    #region
    bag.BagItemInfo info;
    int num = 1;
    UIItemBase item;
    #endregion
    public override void Init()
    {
        base.Init();
        UIEventListener.Get(mbtn_close).onClick = CloseClick;
        UIEventListener.Get(mbtn_shield).onClick = CloseClick;
        UIEventListener.Get(mbtn_left).onClick = CloseClick;
        UIEventListener.Get(mbtn_right).onClick = SplitClick;
        UIEventListener.Get(mbtn_add).onClick = AddClick;
        UIEventListener.Get(mbtn_minus).onClick = ReduceClick;
        minput_Num.onValidate = OnValidateInput;
        minput_Num.onChange.Add(new EventDelegate(OnSellPriceChange));
        item = UIItemManager.Instance.GetItem(PropItemType.Normal, mobj_itemPar.transform);
    }
    public void SetData(bag.BagItemInfo _info)
    {
        info = _info;
        item.Refresh(info);
    }
    public override void Show()
    {

        base.Show();
    }

    protected override void OnDestroy()
    {
        if (item != null) { UIItemManager.Instance.RecycleSingleItem(item); }
        num = 1;
        temp_num = 0;
        base.OnDestroy();
    }
    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIPromptItemSplitPanel>();
    }
    void SplitClick(GameObject _go)
    {
        Net.ReqSplitBagItemMessage(info.bagIndex, num);
        UIManager.Instance.ClosePanel<UIPromptItemSplitPanel>();
    }
    void AddClick(GameObject _go)
    {
        num++;
        num = (num >= info.count) ? info.count : num;
        minput_Num.value = num.ToString();
    }
    void ReduceClick(GameObject _go)
    {
        num--;
        num = (num <= 1) ? 1 : num;
        minput_Num.value = num.ToString();
    }

    char OnValidateInput(string text, int charIndex, char addedChar)
    {
        if (addedChar == '-')
        {
            return (char)0;

        }
        return addedChar;
    }
    int temp_num = 0;
    void OnSellPriceChange()
    {
        temp_num = (minput_Num.value == "") ? 0 : int.Parse(minput_Num.value);
        temp_num = (temp_num >= info.count) ? info.count : temp_num;
        temp_num = (temp_num <= 1) ? 1 : temp_num;
        //Debug.Log(temp_num + "   " + num);
        if (temp_num != num)
        {
            num = temp_num;
            minput_Num.value = num.ToString();
        }
    }
}
