using System.Collections.Generic;
using bag;
using UnityEngine;
using user;

public partial class UICheckAttrPanel : UIBasePanel
{
    public override bool ShowGaussianBlur { get => false; }

    private int[] normalEquipIndex = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12}; //格子对应的服务器pos

    private OtherPlayerInfo checkRoleInfo;

    Dictionary<int, bag.BagItemInfo> mEquipData;
    List<EquipItem> equipItems;

    public override void Init()
    {
        base.Init();
        mbtn_ToWoLong.onClick = OnClickToWoLong;
        mbar.onChange.Add(new EventDelegate(OnChange));
    }

    public override void Show()
    {
        base.Show();
        InitData();
    }

    void InitData()
    {
        checkRoleInfo = CSOtherPlayerInfo.Instance.OtherPlayerInfo;
        SetLeftData();
        SetRightData();
    }

    void SetLeftData()
    {
        mlb_rolename.text = checkRoleInfo.roleBrief.roleName;
        mlb_rolelevel.text = $"Lv{checkRoleInfo.roleBrief.level}";
        equipItems = new List<EquipItem>();
        equipItems.Clear();
        for (int i = 0; i < mequips.transform.childCount; i++)
        {
            equipItems.Add(new EquipItem(mequips.transform.GetChild(i).gameObject, i, 1, true));
        }

        //普通道具格子
        mEquipData = new Dictionary<int, BagItemInfo>();
        mEquipData.Clear();
        for (int i = 0; i < checkRoleInfo.equips.Count; i++)
        {
            bag.EquipInfo equipInfo = checkRoleInfo.equips[i];
            if (equipInfo.position >= 1 && equipInfo.position <= 12)
            {
                mEquipData.Add(checkRoleInfo.equips[i].position, checkRoleInfo.equips[i].equip);
            }
        }

        if (mEquipData != null)
        {
            for (int i = 0; i < equipItems.Count; i++)
            {
                EquipItem item = equipItems[i];
                bag.BagItemInfo tempInfo;
                if (mEquipData.ContainsKey(normalEquipIndex[i]))
                {
                    tempInfo = mEquipData[normalEquipIndex[i]];
                    equipItems[i].RefreshItem(mEquipData[normalEquipIndex[i]]);
                }
                else
                {
                    equipItems[i].UnInit();
                }
            }
        }

        //武器
        bag.BagItemInfo weaponInfo;
        if (mEquipData != null && mEquipData.ContainsKey(1))
        {
            weaponInfo = mEquipData[1];
            CSEffectPlayMgr.Instance.ShowUIEffect(mWeapon,
                ItemTableManager.Instance.GetItemModel(weaponInfo.configId).ToString(), ResourceType.UIWeapon);
        }

        //衣服
        bag.BagItemInfo clothesInfo;
        if (mEquipData != null && mEquipData.ContainsKey(2))
        {
            clothesInfo = mEquipData[2];
            CSEffectPlayMgr.Instance.ShowUIEffect(mCloth,
                ItemTableManager.Instance.GetItemModel(clothesInfo.configId).ToString(), ResourceType.UIPlayer);
            mRole.SetActive(false);
            mCloth.SetActive(true);
        }
        else
        {
            mRole.SetActive(true);
            mCloth.SetActive(false);
            string str = checkRoleInfo.roleBrief.sex == 1 ? "615000" : "625000";
            CSEffectPlayMgr.Instance.ShowUIEffect(mRole, str, ResourceType.UIPlayer);
        }
    }

    void SetRightData()
    {
        //战斗力
        mlab_fight.text = checkRoleInfo.attribute.nbValue.ToString();
        //血蓝
        mlb_hp.text = $"{checkRoleInfo.roleBrief.hp}/{checkRoleInfo.roleBrief.maxHp}";
        mslier_hp.value = checkRoleInfo.roleBrief.hp * 1f / checkRoleInfo.roleBrief.maxHp;
        mlb_mp.text = $"{checkRoleInfo.roleBrief.mp}/{checkRoleInfo.roleBrief.maxMp}";
        mslier_mp.value = checkRoleInfo.roleBrief.mp * 1f / checkRoleInfo.roleBrief.maxMp;
        //属性
        List<AttrUnit> units = MakeAttrListDate();
        mgrid_attr.MaxCount = units.Count;
        ScriptBinder go;
        for (int i = 0; i < mgrid_attr.controlList.Count; i++)
        {
            go = mgrid_attr.controlList[i].GetComponent<ScriptBinder>();
            if (go == null) continue;
            go.GetComponent<UILabel>().text = units[i].value;
            go.GetScript<UILabel>("lb_item").text = units[i].name;
        }
    }
    
    const string splitStr = "{0}-{1}";
    private List<AttrUnit> MakeAttrListDate()
    {
        Dictionary<int, TupleProperty> attrDic = new Dictionary<int, TupleProperty>();
        attrDic.Clear();
        for (int i = 0; i < checkRoleInfo.attribute.attrs.Count; i++)
        {
            attrDic.Add(checkRoleInfo.attribute.attrs[i].type, checkRoleInfo.attribute.attrs[i]);
            // if (checkRoleInfo.attribute.attrs[i].type == 11)
            // {
            //     Debug.Log("mainPlayerinfo   hp");
            //     MaxHP = checkRoleInfo.attribute.attrs[i].value;
            //     mClientEvent.SendEvent(CEvent.HP_Change);
            // }
            // else if (checkRoleInfo.attribute.attrs[i].type == 12)
            // {
            //     Debug.Log("mainPlayerinfo   mp");
            //     MaxMP = checkRoleInfo.attribute.attrs[i].value;
            //     mClientEvent.SendEvent(CEvent.MP_Change);
            // }
        }

        List<AttrUnit> list = new List<AttrUnit>();
        list.Add(new AttrUnit(CSString.Format(21), attrDic[11].value.ToString()));
        list.Add(new AttrUnit(CSString.Format(22), attrDic[12].value.ToString()));
        AttrUnit au = new AttrUnit();
        if (checkRoleInfo.roleBrief.career == ECareer.Warrior)
        {
            au.value = string.Format(splitStr, attrDic[2].value, attrDic[1].value);
            au.name = SundryTableManager.Instance.GetDes(116).Split('#')[0];
        }
        else if (checkRoleInfo.roleBrief.career == ECareer.Master)
        {
            au.value = string.Format(splitStr, attrDic[4].value, attrDic[3].value);
            au.name = SundryTableManager.Instance.GetDes(116).Split('#')[1];
        }
        else
        {
            au.value = string.Format(splitStr, attrDic[6].value, attrDic[5].value);
            au.name = SundryTableManager.Instance.GetDes(116).Split('#')[2];
        }
        list.Add(au);
        AttrUnit phydef = new AttrUnit();
        phydef.value = string.Format(splitStr, attrDic[8].value, attrDic[7].value);
        phydef.name = ClientTipsTableManager.GetContext(104);
        list.Add(phydef);
        AttrUnit magicdef = new AttrUnit();
        magicdef.value = string.Format(splitStr, attrDic[10].value, attrDic[9].value);
        magicdef.name = ClientTipsTableManager.GetContext(105);
        list.Add(magicdef);
        list.Add(new AttrUnit(CSString.Format(23), $"{(attrDic[13].value / 100).ToString("F2")}%"));
        list.Add(new AttrUnit(CSString.Format(24), $"{(attrDic[14].value / 100).ToString("F2")}%"));
        list.Add(new AttrUnit(CSString.Format(26), $"{(attrDic[16].value / 100).ToString("F2")}%"));
        list.Add(new AttrUnit(CSString.Format(25), $"{(attrDic[15].value / 100).ToString("F2")}%"));
        list.Add(new AttrUnit(CSString.Format(36), attrDic[26].value.ToString()));
        list.Add(new AttrUnit(CSString.Format(37), attrDic[27].value.ToString()));
        AttrUnit auper = new AttrUnit();
        if (checkRoleInfo.roleBrief.career == ECareer.Warrior)
        {
            auper.name = CSString.Format(29);
            auper.value = $"{(attrDic[19].value / 100).ToString("F2")}%";
        }
        else if (checkRoleInfo.roleBrief.career == ECareer.Master)
        {
            auper.name = CSString.Format(30);
            auper.value = $"{(attrDic[20].value / 100).ToString("F2")}%";
        }
        else
        {
            auper.name = CSString.Format(31);
            auper.value = $"{(attrDic[21].value / 100).ToString("F2")}%";
        }
        list.Add(auper);
        list.Add(new AttrUnit(CSString.Format(32), $"{(attrDic[22].value / 100).ToString("F2")}%"));
        list.Add(new AttrUnit(CSString.Format(33), $"{(attrDic[23].value / 100).ToString("F2")}%"));
        list.Add(new AttrUnit(CSString.Format(40), attrDic[30].value.ToString()));
        list.Add(new AttrUnit(CSString.Format(41), attrDic[31].value.ToString()));
        //移速
        //list.Add(new AttrUnit(CSString.Format(41), attrDic[31].Value.ToString()));
        //攻速
        //list.Add(new AttrUnit(CSString.Format(41), attrDic[31].Value.ToString()));
        list.Add(new AttrUnit(CSString.Format(38), attrDic[28].value.ToString()));
        list.Add(new AttrUnit(CSString.Format(49), $"{(attrDic[39].value / 100).ToString("F2")}%"));
        //PK值 不在attribute表
        list.Add(new AttrUnit(CSString.Format(661), CSMainPlayerInfo.Instance.PkValue.ToString()));
        return list;
    }
    
    public struct AttrUnit
    {
        public AttrUnit(string _name, string _value)
        {
            name = _name;
            value = _value;
        }
        public string name;
        public string value;
    }

    void OnChange()
    {
        marrow.SetActive(mbar.value < 0.95);
    }
    
    void OnClickToWoLong(GameObject go)
    {
        UIManager.Instance.CreatePanel<UICheckInfoCombinePanel>(p =>
        {
            (p as UICheckInfoCombinePanel).OpenChildPanel((int) UICheckInfoCombinePanel.ChildPanelType.CPT_Dragon);
        });
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        CSEffectPlayMgr.Instance.Recycle(mWeapon);
        CSEffectPlayMgr.Instance.Recycle(mCloth);
        CSEffectPlayMgr.Instance.Recycle(mRole);
        equipItems = null;
    }
}