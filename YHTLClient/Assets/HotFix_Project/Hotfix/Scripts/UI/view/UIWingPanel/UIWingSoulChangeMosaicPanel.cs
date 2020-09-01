using bag;
using TABLE;
using UnityEngine;

public partial class UIWingSoulChangeMosaicPanel : UIBasePanel
{
    private ILBetterList<BagItemInfo> wingSoulList;
    private int curPosition = 0;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.ItemListChange, RefreshData);
        AddCollider();
        mbtn_close.onClick = Close;
    }
    
    void RefreshData(uint id, object data)
    {
        OpenWingSoulChangeMosaicPanel(curPosition);
    }

    public override void Show()
    {
        base.Show();
    }

    public void OpenWingSoulChangeMosaicPanel(int position)
    {
        curPosition = position;
        switch (position)
        {
            case 1:
                mlb_title.text = CSString.Format(1936);
                wingSoulList = CSWingInfo.Instance.GetWingSoulList();
                break;
            case 2:
                mlb_title.text = CSString.Format(1937);
                wingSoulList = CSWingInfo.Instance.GetWingColorSoulList();
                break;
            case 3:
                mlb_title.text = CSString.Format(1938);
                wingSoulList = CSWingInfo.Instance.GetWingTechniqueSoulList();
                break;
        }

        if (wingSoulList == null) return;
        mgrid_wingSoul.MaxCount = wingSoulList.Count;
        GameObject gp;
        for (int i = 0, max = mgrid_wingSoul.MaxCount; i < max; i++)
        {
            gp = mgrid_wingSoul.controlList[i];
            var eventHandle = UIEventListener.Get(gp);
            UIWingSoulChangeMosaicBinder Binder;
            if (eventHandle.parameter == null)
            {
                Binder = new UIWingSoulChangeMosaicBinder();
                Binder.Setup(eventHandle);
            }
            else
            {
                Binder = eventHandle.parameter as UIWingSoulChangeMosaicBinder;
            }
            
            Binder.position = curPosition;
            Binder.index = i;
            Binder.Bind(wingSoulList[i]);
        }
    }

    protected override void OnDestroy()
    {
        mgrid_wingSoul.UnBind<UIWingSoulChangeMosaicBinder>();
        base.OnDestroy();
    }
}

public class UIWingSoulChangeMosaicBinder : UIBinder
{
    private GameObject item74;
    private UILabel lb_name;
    private UILabel lb_attr;
    private GameObject sp_flag;
    private UIItemBase itemBase;
    private BagItemInfo bagItemInfo;
    private GameObject bg;
    public int position = 0;

    public override void Init(UIEventListener handle)
    {
        item74 = Get<GameObject>("item74");
        lb_name = Get<UILabel>("lb_name");
        lb_attr = Get<UILabel>("lb_attr");
        sp_flag = Get<GameObject>("sp_flag");
        bg = Get<GameObject>("bg");

        UIEventListener.Get(bg).onClick = OnClickItem;
    }

    public override void Bind(object data)
    {
        if (data == null) return;
        bagItemInfo = (BagItemInfo) data;
        RefreshUI();
    }

    void RefreshUI()
    {
        TABLE.YULINGSOUL Cfg;
        if (YuLingSoulTableManager.Instance.TryGetValue(bagItemInfo.configId, out Cfg))
        {
            sp_flag.SetActive(index == 0);
            itemBase = UIItemManager.Instance.GetItem(PropItemType.Normal, item74.transform);
            itemBase.Refresh(bagItemInfo);
            itemBase.SetCount(0);
            lb_name.text = Cfg.name;
            lb_name.color = UtilityCsColor.Instance.GetColor(
                ItemTableManager.Instance.GetItemQuality(Cfg.id));

            if (Cfg.exattr > 0)
            {
                lb_attr.text = CSString.Format(1950, $"+{Cfg.exattr * 1f / 10000}%").BBCode(ColorType.Green);
                lb_attr.gameObject.SetActive(true);    
            }
            else
                lb_attr.gameObject.SetActive(false);
        }
    }

    void OnClickItem(GameObject go)
    {
        if (position <= 0) return;
        Net.CSYuLingSoulSingleSetMessage(position, bagItemInfo.bagIndex);
    }

    public override void OnDestroy()
    {
        item74 = null;
        lb_name = null;
        lb_attr = null;
        sp_flag = null;
        bagItemInfo = null;
        bg = null;
        UIItemManager.Instance.RecycleSingleItem(itemBase);
    }
}