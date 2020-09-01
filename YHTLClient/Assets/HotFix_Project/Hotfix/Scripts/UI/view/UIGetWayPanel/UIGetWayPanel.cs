using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public enum CountState
{
    Enough,
    Less,
}
public class UIGetWayPanel : UIBase
{
    public override UILayerType PanelLayerType { get { return UILayerType.Tips; } }

    GameObject _shield;
    GameObject shield { get { return _shield ?? (_shield = Get("window/shield").gameObject); } }
    UISprite _bg;
    UISprite bg { get { return _bg ?? (_bg = Get<UISprite>("window/bg")); } }
    UILabel _lb_des;
    UILabel lb_des { get { return _lb_des??(_lb_des = Get<UILabel> ("view/des")); } }
    GameObject _itemPar;
    GameObject itemPar { get { return _itemPar ?? (_itemPar = Get("view/UIItem").gameObject); } }
    UIGridContainer _grid;
    UIGridContainer grid { get { return _grid ?? (_grid = Get<UIGridContainer>("wayScroll/Grid")); } }
    public override void Init()
    {
        base.Init();

    }
    public override void Show()
    {
        base.Show();
    }
    public void Refresh(TABLE.ITEM _cfg, CountState _state)
    {
        if (_state == CountState.Enough)
        {
            lb_des.text = SundryTableManager.Instance.GetDes(1);
        }
        else
        {
            lb_des.text = SundryTableManager.Instance.GetDes(2);
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
