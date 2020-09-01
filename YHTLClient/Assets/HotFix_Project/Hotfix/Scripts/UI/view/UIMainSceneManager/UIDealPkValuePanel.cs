using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIDealPkValuePanel : UIBasePanel
{
    UIItemBase item;
    long selfCount = 0;
    int needCount = 0;
    string[] exchangeMes;
    public override void Init()
    {
        base.Init();
        exchangeMes = SundryTableManager.Instance.GetSundryEffect(108).Split('#');
        item = UIItemManager.Instance.GetItem(PropItemType.Normal, mobj_itemPar.transform);
        UIEventListener.Get(mbtn_Exchange).onClick = ExchangeBtnClick;
        UIEventListener.Get(mbtn_Leave).onClick = LeaveBtnClick;
        UIEventListener.Get(mbtnClose).onClick = CloseClick;
        item.Refresh(int.Parse(exchangeMes[0]));
        selfCount = CSBagInfo.Instance.GetItemCount(int.Parse(exchangeMes[0]));
        needCount = int.Parse(exchangeMes[1]);

        mlb_count.text = $"{selfCount}/{needCount}";
        mlb_count.color = (selfCount >= needCount) ? CSColor.gray : CSColor.red;
    }

    public override void Show()
    {
        base.Show();
    }
    protected override void OnDestroy()
    {

        base.OnDestroy();
    }
    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIDealPkValuePanel>();
    }
    void LeaveBtnClick(GameObject _go)
    {
        //请求离开
    }
    void ExchangeBtnClick(GameObject _go)
    {
        if (selfCount < needCount)
        {
            UtilityTips.ShowRedTips(662);
            return;
        }
        //请求交换pk值

    }
}
