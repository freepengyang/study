using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIEquipZhuFuPanel : UIBasePanel
{
    public override UILayerType PanelLayerType
    {
        get
        {
            return UILayerType.Tips;
        }
    }
    public UIEquipZhuFuPanel()
    {
        //Init();
    }
    #region forms
    private UIItemBase equipBase;
    private UIItemBase costbase;
    private bag.BagItemInfo equipData;
    const int ZhuFuId = 50000028;
    #endregion



    #region  variable
    #endregion


    public override void Init()
    {
        base.Init();
        CSEffectPlayMgr.Instance.ShowUITexture(mobj_bg.gameObject, "bag_bg");
        //UIEventListener.Get(mbtn_bg_close).onClick = ClosePanel;
        equipBase = UIItemManager.Instance.GetItem(PropItemType.Recycle, mitemBase.transform);
        costbase = UIItemManager.Instance.GetItem(PropItemType.Normal, mcostItemBase.transform);
        UIEventListener.Get(mbtn_zhufu).onClick = ZhufuAction;
        mClientEvent.Reg((uint)CEvent.EquipRebuildNtfMessage, GetRecastBack);
        mClientEvent.Reg((uint)CEvent.ItemListChange, OnItemChanged);
        mitemBase.SetActive(true);
        mcostItemBase.SetActive(true);
        mlb_cost_num.gameObject.SetActive(true);
        UIEventListener.Get(mbtn_help).onClick = InstructionClick;
    }
    public void RefreshInfo(bag.BagItemInfo info)
    {
        equipData = info;
        equipBase.Refresh(equipData);
        TABLE.ITEM equipCfg = ItemTableManager.Instance.GetItemCfg(equipData.configId);
        if (equipData != null)
        {
            mlb_equip_name.text = equipCfg.name;
            mlb_equip_name.color = UtilityCsColor.Instance.GetColor(equipData.quality);
        }
        TABLE.ITEM costCfg = ItemTableManager.Instance.GetItemCfg(ZhuFuId);
        if (costbase != null) costbase.Refresh(costCfg, ItemClick);
        mlb_equip_lucky.text = string.Concat("+", info.weaponLuckLv.ToString());
        RefreshCostNum();
    }
    private void RefreshCostNum()
    {
        mlb_cost_num.text = $"{ZhuFuId.GetItemCount()}/1";
        mlb_cost_num.color = (ZhuFuId.GetItemCount() > 0) ? CSColor.green : CSColor.red;
    }
    private void ItemClick(UIItemBase item)
    {
        if (item != null)
        {
            UITipsManager.Instance.CreateTips(TipsOpenType.Normal, item.itemCfg, item.infos);
        }
    }
    private void ClosePanel(GameObject go)
    {
        UIManager.Instance.ClosePanel(this.GetType());
    }
    void InstructionClick(GameObject go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.ZhuFuYou);
    }
    private void ZhufuAction(GameObject go)
    {
        long num = ZhuFuId.GetItemCount();
        string noticeTittle;
        if (num <= 0)
        {
            Utility.ShowGetWay(ZhuFuId);
            mClientEvent.SendEvent(CEvent.CloseTips);
            return;
        }
        noticeTittle = PromptWordTableManager.Instance.GetPromptWordTitle(11);
        string noticeDes = PromptWordTableManager.Instance.GetPromptWordDec(11);
        string equipName = "";
        TABLE.ITEM equipCfg = ItemTableManager.Instance.GetItemCfg(equipData.configId);
        if (equipData != null) equipName = equipCfg.name;
        UtilityTips.ShowPromptWordTips(11, null, SendZhuFu, string.Concat("<[00ff0f]", equipName, "[-]>"));

    }
    Dictionary<int, bag.BagItemInfo> selfquips = new Dictionary<int, bag.BagItemInfo>();
    private void SendZhuFu()
    {
        selfquips.Clear();
        CSBagInfo.Instance.GetNormalEquip(selfquips);
        if (selfquips.Count > 0)
        {
            var iter = selfquips.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value.id == equipData.id)
                {
                    Net.ReqUseLuckOilMessage(-iter.Current.Key);
                    return;
                }
            }
        }
        Net.ReqUseLuckOilMessage(equipData.bagIndex);
    }
    private void GetRecastBack(uint id, object data)
    {
        if (data == null) return;
        bag.EquipRebuildNtf msg = (bag.EquipRebuildNtf)data;

        FNDebug.Log(msg.equip.equip.weaponLuckLv + "    " + equipData.weaponLuckLv);

        if (msg.equip.equip.id == equipData.id)
        {
            if (msg.equip.equip.weaponLuckLv > equipData.weaponLuckLv)
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(meffect, 17130);
            }
            else
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(meffect, 17131);
            }
            RefreshInfo(msg.equip.equip);
        }
    }
    protected void OnItemChanged(uint uiEvtID, object data)
    {
        RefreshCostNum();
    }
    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(meffect);
        CSEffectPlayMgr.Instance.Recycle(mobj_bg.gameObject);
        UIItemManager.Instance.RecycleSingleItem(equipBase);
        UIItemManager.Instance.RecycleSingleItem(costbase);
        equipBase = null;
        costbase = null;
        equipData = null;
    }
}