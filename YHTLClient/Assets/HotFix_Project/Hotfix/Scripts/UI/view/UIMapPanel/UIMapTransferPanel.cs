using System.Reflection;
using TABLE;
using UnityEngine;

public partial class UIMapTransferPanel : UIBasePanel
{
    private CSBetterList<DELIVER> deliverList;
    
    public override void Init()
    {
        base.Init();
        deliverList = mPoolHandleManager.GetSystemClass<CSBetterList<DELIVER>>();
        deliverList.Clear();
        EventDelegate.Add(mmainToggle.onChange, OnSelectMapMain);
        EventDelegate.Add(mfieldToggle.onChange, OnSelectMapFiled);
        EventDelegate.Add(mspecialToggle.onChange, OnSelectMapSpecial);
    }

    public override void Show()
    {
        base.Show();
    }

    public override UIBasePanel OpenChildPanel(int type, bool fromToggle = false)
    {
        mmainToggle.Set(false);
        mfieldToggle.Set(false);
        mspecialToggle.Set(false);

        switch (type)
        {
            case 1:
                mmainToggle.value = true;
                break;
            case 2:
                mfieldToggle.value = true;
                break;
            case 3:
                mspecialToggle.value = true;
                break;
        }

        return this;
    }

    private void OnSelectMapMain()
    {
        if (!mmainToggle.value) return;
        GetMapByType(1);
    }

    private void OnSelectMapFiled()
    {
        if (!mfieldToggle.value) return;
        GetMapByType(2);
    }

    private void OnSelectMapSpecial()
    {
        if (!mspecialToggle.value) return;
        GetMapByType(3);
    }

    private void GetMapByType(int type)
    {
        var arr = DeliverTableManager.Instance.array.gItem.handles;
        deliverList.Clear();
        TABLE.DELIVER deliverItem = null;
        int playerlv = CSMainPlayerInfo.Instance.Level;
        int templv = int.MaxValue;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.DELIVER;
            int level2 = item.level2;
            if (level2 >= playerlv && level2 != 1)
            {
                if (level2 < templv)
                {
                    CSMapManager.Instance.recommendLv = level2;
                    templv = level2;
                }
            }
            deliverItem = item;
            if (deliverItem.deliverClass == type)
            {
                deliverList.Add(deliverItem);
            }
        }

        ShowMap();
    }

    private void ShowMap()
    {
        mrightGroup.Bind<DELIVER, MapDeliverItem>(deliverList, mPoolHandleManager);
        mScrollView.ResetPosition();
    }

    protected override void OnDestroy()
    {
        mrightGroup.UnBind<MapDeliverItem>();
        base.OnDestroy();
    }
}


public class MapDeliverItem : UIBinder
{
    private DELIVER _deliver;
    private UILabel deliverLab;
    private GameObject sp_title;

    public override void Init(UIEventListener handle)
    {
        deliverLab = Get<UILabel>("Label");
        sp_title = Get<Transform>("sp_title").gameObject;
        UIEventListener.Get(handle.gameObject).onClick = OnTransferMapClick;
    }

    public override void Bind(object data)
    {
        _deliver = data as DELIVER;
        if (_deliver == null) return;
        //string mapName = MapInfoTableManager.Instance.GetMapInfoName(_deliver.deliverParameter);
        if (_deliver.level2 == 0)
            deliverLab.text = _deliver.tip;
        else
            deliverLab.text = $"{_deliver.tip}(Lv.{_deliver.level2})";
        
        //根据玩家等级显示推荐角标
        
        int playerlv = CSMainPlayerInfo.Instance.Level;
        var levels = _deliver.level3.Split('#');
        if (levels.Length>=2)
        {
            int min, max;
            int.TryParse(levels[0], out min);
            int.TryParse(levels[1], out max);
            sp_title.SetActive(min<=playerlv&&playerlv<max);
        }
        
        //sp_title.SetActive(CSMapManager.Instance.recommendLv == _deliver.level2);
    }

    private void OnTransferMapClick(GameObject go)
    {
        if (_deliver == null) return;

        //藏宝阁 
        if (_deliver.id == 114)
        {
            if (!CSGuildFightManager.Instance.IsSabakeMember)
            {
                UtilityTips.ShowRedTips(1622);
                return;
            }
        }

        if (_deliver.id == 111 || _deliver.id == 112)
        {
            UtilityPanel.JumpToPanel(12607);
            UIManager.Instance.ClosePanel<UIMapCombinePanel>();
            return;
        }
        
        // // 勇者之地
        // if (_deliver.id == 111)
        // {
        //     if (!IsCardMap(76, 1)) return;
        // }
        //
        // // 王者之地
        // if (_deliver.id == 112)
        // {
        //     if (!IsCardMap(77, 2)) return;
        // }

        if (_deliver.item.Count < 2 || _deliver.item[0].GetItemCount() > _deliver.item[1])
        {
            UtilityPath.FindWithDeliverId(_deliver.id);
            UIManager.Instance.ClosePanel<UIMapCombinePanel>();
        }
        else
        {
            Utility.ShowGetWay(_deliver.item[0]);
        }
    }

    private bool IsCardMap(int promptWordId, int monsthCardId)
    {
        if (!CSMonthCardInfo.Instance.HasMonthCard(monsthCardId))
        {
            if (MonthCardTableManager.Instance.TryGetValue(monsthCardId, out MONTHCARD monthcard))
            {
                UtilityTips.ShowPromptWordTips(promptWordId, null, () =>
                {
                    if (monthcard.price > CSItemCountManager.Instance.GetItemCount((int)MoneyType.yuanbao))
                    {
                        UtilityTips.ShowRedTips(1621);
                    }
                    else
                    {
                        Net.CSBuyMonthCardMessage(monsthCardId);
                    }
                },0,  monthcard.price);
            }

            return false;
        }

        return true;
    }

    public override void OnDestroy()
    {
        _deliver = null;
        deliverLab = null;
    }
}