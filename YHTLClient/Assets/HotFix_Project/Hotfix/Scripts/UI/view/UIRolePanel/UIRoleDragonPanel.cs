using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class UIRoleDragonPanel
{
    List<WolongInfoItem> attrList = new List<WolongInfoItem>();
    UILabel Costnum;
    UIRoleEquipPanel roleEquip;
    int maxId = 0;
    public override void Init()
    {
        _InitScriptBinder();
        if (roleEquip == null)
        {
            roleEquip = new UIRoleEquipPanel();
            roleEquip.UIPrefab = mequipShow;
            roleEquip.Init();
            roleEquip.Show();
            roleEquip.SetEquipType(UIRoleEquipPanel.equipType.WoLong);
        }
        mClientEvent.Reg((uint)CEvent.WoLongLevelUpgrade, GetWoLongUpGrade);
        mClientEvent.Reg((uint)CEvent.MoneyChange, GetMoneyChange);
        UIEventListener.Get(mbtn_upgrade).onClick = UpGradeClick;
        UIEventListener.Get(mbtn_help).onClick = HelpClick;
        UIEventListener.Get(mbtn_add).onClick = AddClick;
        UIEventListener.Get(msp_icon.gameObject).onClick = CostIconClick;
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_showTex, "rolezlbg2_cq_1");
        attrList.Clear();
        attrList.Add(new WolongInfoItem(mobj_att));
        attrList.Add(new WolongInfoItem(mobj_phydef));
        attrList.Add(new WolongInfoItem(mobj_magicdef));
        attrList.Add(new WolongInfoItem(mobj_hp));
    }
    public override void Show()
    {
        Refresh();
        roleEquip.SetEquipType(UIRoleEquipPanel.equipType.WoLong);
    }
    void Refresh()
    {
        if (Costnum == null) { Costnum = mobj_itembar.transform.Find("lb_value").GetComponent<UILabel>(); }
        int wolongLv = CSWoLongInfo.Instance.GetWoLongLevel();
        mlb_level.text = wolongLv.ToString();
        maxId = WoLongLevelTableManager.Instance.GetMaxId();
        if (wolongLv == maxId)
        {
            mobj_maxState.SetActive(true);
            mbtn_upgrade.SetActive(false);

            long mNum = CSItemCountManager.Instance.GetItemCount((int)MoneyType.wolongxiuwei);
            msp_icon.spriteName = $"tubiao{(int)MoneyType.wolongxiuwei}";
            Costnum.text = $"{/*UtilityMath.GetDecimalValue(mNum)*/mNum}/Max";
            Costnum.color = CSColor.green;
            for (int i = 0; i < attrList.Count; i++)
            {
                attrList[i].Refresh(i + 1, wolongLv, true);
            }
        }
        else
        {
            mobj_maxState.SetActive(false);
            mbtn_upgrade.SetActive(true);
            for (int i = 0; i < attrList.Count; i++)
            {
                attrList[i].Refresh(i + 1, wolongLv, false);
            }
            long mNum = CSItemCountManager.Instance.GetItemCount((int)MoneyType.wolongxiuwei);
            int cfgNum = WoLongLevelTableManager.Instance.GetCostByWoLongLevel(wolongLv + 1);
            msp_icon.spriteName = $"tubiao{(int)MoneyType.wolongxiuwei}";
            Costnum.text = $"{/*UtilityMath.GetDecimalValue(mNum)*/mNum}/{/*UtilityMath.GetDecimalValue(cfgNum)*/cfgNum}";
            Costnum.color = (mNum >= cfgNum) ? CSColor.green : CSColor.red;
            mobj_btnRedPoint.SetActive((mNum >= cfgNum) ? true : false);
        }

        int offset = Mathf.Max(Costnum.width - 130, 0);
        msp_itembar.width = 200 + offset;
    }
    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_levelEff);
        for (int i = 0; i < attrList.Count; i++)
        {
            CSEffectPlayMgr.Instance.Recycle(attrList[i].GetEffObj());

        }
        CSEffectPlayMgr.Instance.Recycle(mtex_showTex);
        base.OnDestroy();
        Costnum = null;
        if (roleEquip != null) { roleEquip.Destroy(); }
    }
    void UpGradeClick(GameObject _go)
    {
        if (Costnum.color == CSColor.red)
        {
            int lv = (CSWoLongInfo.Instance.GetWoLongLevel() == 0 ? 1 : CSWoLongInfo.Instance.GetWoLongLevel());
            //Utility.ShowGetWayTwo(WoLongLevelTableManager.Instance.GetWoLongLevelBuyprops(lv + 1));
            Utility.ShowGetWay(11);
            return;
        }
        Net.CSWoLongLevelUpMessage();
    }
    void HelpClick(GameObject _go)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.WoLongLevel);
    }
    void AddClick(GameObject _go)
    {
        int lv = (CSWoLongInfo.Instance.GetWoLongLevel() == 0 ? 1 : CSWoLongInfo.Instance.GetWoLongLevel());
        //Utility.ShowGetWayTwo(WoLongLevelTableManager.Instance.GetWoLongLevelBuyprops(lv + 1));
        Utility.ShowGetWay(11);
    }
    void CostIconClick(GameObject _go)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, (int)MoneyType.wolongxiuwei);
    }
    void GetWoLongUpGrade(uint id, object data)
    {
        UtilityTips.ShowTips(563);
        Refresh();
        for (int i = 0; i < attrList.Count; i++)
        {
            attrList[i].ShowEffect();
        }
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_levelEff, "effect_dragon_icon_add", 10, false);
    }
    void GetMoneyChange(uint id, object data)
    {
        if (Costnum == null) { Costnum = mobj_itembar.transform.Find("lb_value").GetComponent<UILabel>(); }
        long mNum = CSItemCountManager.Instance.GetItemCount((int)MoneyType.wolongxiuwei);
        int cfgNum = WoLongLevelTableManager.Instance.GetCostByWoLongLevel(CSWoLongInfo.Instance.GetWoLongLevel() + 1);
        Costnum.text = $"{/*UtilityMath.GetDecimalValue(mNum)*/mNum}/{/*UtilityMath.GetDecimalValue(cfgNum)*/cfgNum}";
        Costnum.color = (mNum >= cfgNum) ? CSColor.green : CSColor.red;
        mobj_btnRedPoint.SetActive((mNum >= cfgNum) ? true : false);
    }

}

public class WolongInfoItem
{
    GameObject go;
    UILabel key;
    UILabel value;
    UILabel nextKey;
    UILabel nextValue;
    GameObject bg;
    GameObject arrow;
    GameObject effect;
    string deil = "{0}-{1}";
    public WolongInfoItem(GameObject _go)
    {
        go = _go;
        key = go.transform.Find("key").GetComponent<UILabel>();
        value = go.transform.Find("value").GetComponent<UILabel>();
        nextKey = go.transform.Find("nextKey").GetComponent<UILabel>();
        nextValue = go.transform.Find("nextValue").GetComponent<UILabel>();
        bg = go.transform.Find("bg").gameObject;
        arrow = go.transform.Find("arrow").gameObject;
        effect = go.transform.Find("effect").gameObject;
    }
    public void Refresh(int _index, int _lv, bool _state)
    {
        int id = _lv + 1;
        int nextId = id + 1;
        string name = "";
        ClientTipsTableManager ins = ClientTipsTableManager.Instance;
        WoLongLevelTableManager wlIns = WoLongLevelTableManager.Instance;

        if (_index == 1)//攻击
        {
            if (CSMainPlayerInfo.Instance.Career == 1)
            {
                name = $"{ins.GetClientTipsContext(101)}：";
                value.text = string.Format(deil, wlIns.GetWoLongLevelZsatt(id), wlIns.GetWoLongLevelZsattMax(id));
                nextValue.text = string.Format(deil, wlIns.GetWoLongLevelZsatt(nextId), wlIns.GetWoLongLevelZsattMax(nextId));
            }
            else if (CSMainPlayerInfo.Instance.Career == 2)
            {
                name = $"{ins.GetClientTipsContext(102)}：";
                value.text = string.Format(deil, wlIns.GetWoLongLevelFsatt(id), wlIns.GetWoLongLevelFsattMax(id));
                nextValue.text = string.Format(deil, wlIns.GetWoLongLevelFsatt(nextId), wlIns.GetWoLongLevelFsattMax(nextId));
            }
            else if (CSMainPlayerInfo.Instance.Career == 3)
            {
                name = $"{ins.GetClientTipsContext(103)}：";
                value.text = string.Format(deil, wlIns.GetWoLongLevelDsatt(id), wlIns.GetWoLongLevelDsattMax(id));
                nextValue.text = string.Format(deil, wlIns.GetWoLongLevelDsatt(nextId), wlIns.GetWoLongLevelDsattMax(nextId));
            }

        }
        else if (_index == 2)
        {
            name = $"{ins.GetClientTipsContext(104)}：";
            if (CSMainPlayerInfo.Instance.Career == 1)
            {
                value.text = string.Format(deil, wlIns.GetWoLongLevelZsdef(id), wlIns.GetWoLongLevelZsdefMax(id));
                nextValue.text = string.Format(deil, wlIns.GetWoLongLevelZsdef(nextId), wlIns.GetWoLongLevelZsdefMax(nextId));
            }
            else if (CSMainPlayerInfo.Instance.Career == 2)
            {
                value.text = string.Format(deil, wlIns.GetWoLongLevelFsdef(id), wlIns.GetWoLongLevelFsdefMax(id));
                nextValue.text = string.Format(deil, wlIns.GetWoLongLevelFsdef(nextId), wlIns.GetWoLongLevelFsdefMax(nextId));
            }
            else if (CSMainPlayerInfo.Instance.Career == 3)
            {
                value.text = string.Format(deil, wlIns.GetWoLongLevelDsdef(id), wlIns.GetWoLongLevelDsdefMax(id));
                nextValue.text = string.Format(deil, wlIns.GetWoLongLevelDsdef(nextId), wlIns.GetWoLongLevelDsdefMax(nextId));
            }
        }
        else if (_index == 3)
        {
            name = $"{ins.GetClientTipsContext(105)}：";
            if (CSMainPlayerInfo.Instance.Career == 1)
            {
                value.text = string.Format(deil, wlIns.GetWoLongLevelZsmdef(id), wlIns.GetWoLongLevelZsmdefMax(id));
                nextValue.text = string.Format(deil, wlIns.GetWoLongLevelZsmdef(nextId), wlIns.GetWoLongLevelZsmdefMax(nextId));
            }
            else if (CSMainPlayerInfo.Instance.Career == 2)
            {
                value.text = string.Format(deil, wlIns.GetWoLongLevelFsmdef(id), wlIns.GetWoLongLevelFsmdefMax(id));
                nextValue.text = string.Format(deil, wlIns.GetWoLongLevelFsmdef(nextId), wlIns.GetWoLongLevelFsmdefMax(nextId));
            }
            else if (CSMainPlayerInfo.Instance.Career == 3)
            {
                value.text = string.Format(deil, wlIns.GetWoLongLevelDsmdef(id), wlIns.GetWoLongLevelDsmdefMax(id));
                nextValue.text = string.Format(deil, wlIns.GetWoLongLevelDsmdef(nextId), wlIns.GetWoLongLevelDsmdefMax(nextId));
            }
        }
        else if (_index == 4)
        {
            name = $"{ins.GetClientTipsContext(21)}：";
            value.text = wlIns.GetWoLongLevelZshp(id).ToString();
            nextValue.text = wlIns.GetWoLongLevelZshp(nextId).ToString();
        }
        key.text = name;
        nextKey.text = name;

        if (_state)
        {
            nextKey.gameObject.SetActive(false);
            nextValue.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);
            float x = key.transform.localPosition.x + 100;
            float nX = value.transform.localPosition.x + 100;
            key.transform.localPosition = new Vector3(-87, key.transform.localPosition.y, 0);
            value.transform.localPosition = new Vector3(-33, key.transform.localPosition.y, 0);
        }
        else
        {

        }
    }
    public void ShowEffect()
    {
        CSEffectPlayMgr.Instance.ShowUIEffect(effect, "effect_dragon_levelup_add", 10, false);
    }
    public GameObject GetEffObj()
    {
        return effect;
    }
}

