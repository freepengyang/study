using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIRoleAttrInfoPanel : UIBasePanel
{
    #region form
    //slider
    private UISlider _sli_Hp;
    private UISlider sliHp { get { return _sli_Hp ?? (_sli_Hp = Get<UISlider>("view/life/rolelife")); } }
    private UILabel _lb_Hp;
    private UILabel lb_Hp { get { return _lb_Hp ?? (_lb_Hp = Get<UILabel>("view/life/hpvalue")); } }
    private UISlider _sli_Mp;
    private UISlider sliMp { get { return _sli_Mp ?? (_sli_Mp = Get<UISlider>("view/life/rolemagic")); } }
    private UILabel _lb_Mp;
    private UILabel lb_Mp { get { return _lb_Mp ?? (_lb_Mp = Get<UILabel>("view/life/mpvalue")); } }
    //详细属性
    private GameObject _obj_detail;
    private GameObject obj_detail { get { return _obj_detail ?? (_obj_detail = Get("view/detail").gameObject); } }
    private UIGridContainer _grid_detailGrid;
    private UIGridContainer grid_detailGrid { get { return _grid_detailGrid ?? (_grid_detailGrid = Get<UIGridContainer>("Scroll/grid/", obj_detail.transform)); } }
    private GameObject _obj_arrow;
    private GameObject obj_arrow { get { return _obj_arrow ?? (_obj_arrow = Get("arrow", obj_detail.transform).gameObject); } }
    private UIScrollBar _obj_bar;
    private UIScrollBar obj_bar { get { return _obj_bar ?? (_obj_bar = Get<UIScrollBar>("view/life/bar")); } }

    GameObject _obj_roleequip;
    GameObject obj_roleequip { get { return _obj_roleequip ?? (_obj_roleequip = Get<GameObject>("view/UIRoleShowPanel")); } }
    UILabel _lb_Power;
    UILabel lb_Power { get { return _lb_Power ?? (_lb_Power = Get<UILabel>("view/life/lab_fight")); } }
    GameObject _obj_fpBg;
    GameObject obj_fpBg { get { return _obj_fpBg ?? (_obj_fpBg = Get<GameObject>("view/life/role_fight_bg")); } }
    #endregion

    UIRoleEquipPanel roleEquip;
    const string splitStr = "{0}-{1}";
    public override void Init()
    {
        base.Init();
        if (roleEquip == null)
        {
            roleEquip = new UIRoleEquipPanel();
            roleEquip.UIPrefab = obj_roleequip;
            roleEquip.Init();
            roleEquip.Show();
            roleEquip.SetEquipType(UIRoleEquipPanel.equipType.Normal);
        }
        CSMainPlayerInfo.Instance.mClientEvent.AddEvent(CEvent.FightPowerChange, GetFightPowerChange);
        CSMainPlayerInfo.Instance.mClientEvent.AddEvent(CEvent.HP_Change, GetFightPowerChange);
        CSMainPlayerInfo.Instance.mClientEvent.AddEvent(CEvent.MP_Change, GetFightPowerChange);
        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.HP_Change, GetFightPowerChange);
        CSMainPlayerInfo.Instance.EventHandler.AddEvent(CEvent.MP_Change, GetFightPowerChange);
        obj_bar.onChange.Add(new EventDelegate(OnChange));
        CSEffectPlayMgr.Instance.ShowUITexture(obj_fpBg, obj_fpBg.name);
    }
    public override void Show()
    {
        base.Show();
        RefreshInfo();
        roleEquip?.SetEquipType(UIRoleEquipPanel.equipType.Normal);
    }
    void RefreshInfo()
    {
        //战斗力
        lb_Power.text = CSMainPlayerInfo.Instance.fightPower.ToString();
        //slider
        lb_Hp.text = $"{CSMainPlayerInfo.Instance.HP}/{CSMainPlayerInfo.Instance.MaxHP}";
        sliHp.value = CSMainPlayerInfo.Instance.HP * 1f / CSMainPlayerInfo.Instance.MaxHP;
        lb_Mp.text = $"{CSMainPlayerInfo.Instance.MP}/{CSMainPlayerInfo.Instance.MaxMP}";
        sliMp.value = CSMainPlayerInfo.Instance.MP * 1f / CSMainPlayerInfo.Instance.MaxMP;
        //详细
        List<AttrUnit> units = MakeAttrListDate();
        grid_detailGrid.MaxCount = units.Count;
        for (int i = 0; i < grid_detailGrid.controlList.Count; i++)
        {
            GameObject go = grid_detailGrid.controlList[i];
            if (go == null) continue;
            go.GetComponent<UILabel>().text = units[i].value;
            go.transform.Find("Label").GetComponent<UILabel>().text = units[i].name;
        }
    }
    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(obj_fpBg);
        CSMainPlayerInfo.Instance.mClientEvent.RemoveEvent(CEvent.HP_Change, GetFightPowerChange);
        CSMainPlayerInfo.Instance.mClientEvent.RemoveEvent(CEvent.MP_Change, GetFightPowerChange);
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.HP_Change, GetFightPowerChange);
        CSMainPlayerInfo.Instance.EventHandler.RemoveEvent(CEvent.MP_Change, GetFightPowerChange);
        CSMainPlayerInfo.Instance.mClientEvent.RemoveEvent(CEvent.FightPowerChange, GetFightPowerChange);
        if (roleEquip != null) { roleEquip.Destroy(); }
        base.OnDestroy();
    }
    private List<AttrUnit> MakeAttrListDate()
    {
        Dictionary<int, user.TupleProperty> attrDic = CSMainPlayerInfo.Instance.GetMyAttr();
        List<AttrUnit> list = new List<AttrUnit>(30);
        AttrUnit au = new AttrUnit();
        if (CSMainPlayerInfo.Instance.Career == ECareer.Warrior)
        {
            au.value = string.Format(splitStr, attrDic[2].value, attrDic[1].value);
            au.name = SundryTableManager.Instance.GetDes(116).Split('#')[0];
        }
        else if (CSMainPlayerInfo.Instance.Career == ECareer.Master)
        {
            au.value = string.Format(splitStr, attrDic[4].value, attrDic[3].value);
            au.name = SundryTableManager.Instance.GetDes(116).Split('#')[1];
        }
        else
        {
            au.value = string.Format(splitStr, attrDic[6].value, attrDic[5].value);
            au.name = SundryTableManager.Instance.GetDes(116).Split('#')[2];
        }
        //攻击（分职业）
        list.Add(au);
        AttrUnit phydef = new AttrUnit();
        phydef.value = string.Format(splitStr, attrDic[8].value, attrDic[7].value);
        phydef.name = ClientTipsTableManager.GetContext(104);
        //物防
        list.Add(phydef);
        AttrUnit magicdef = new AttrUnit();
        magicdef.value = string.Format(splitStr, attrDic[10].value, attrDic[9].value);
        magicdef.name = ClientTipsTableManager.GetContext(105);
        //魔防
        list.Add(magicdef);
        //暴击率
        list.Add(new AttrUnit(CSString.Format(24), $"{Math.Round(Convert.ToDecimal(attrDic[14].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //命中
        list.Add(new AttrUnit(CSString.Format(25), $"{Math.Round(Convert.ToDecimal(attrDic[15].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //PK值 不在attribute表
        list.Add(new AttrUnit(CSString.Format(661), CSMainPlayerInfo.Instance.PkValue.ToString()));

        //hp
        list.Add(new AttrUnit(CSString.Format(21), attrDic[11].value.ToString()));
        //mp
        list.Add(new AttrUnit(CSString.Format(22), attrDic[12].value.ToString()));
        //  13  爆伤 策划需求直接+10000 显示
        list.Add(new AttrUnit(CSString.Format(23), $"{Math.Round(Convert.ToDecimal((attrDic[13].value + 10000) * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        
        //闪避
        list.Add(new AttrUnit(CSString.Format(26), $"{Math.Round(Convert.ToDecimal(attrDic[16].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //幸运
        list.Add(new AttrUnit(CSString.Format(36), attrDic[26].value.ToString()));
        //诅咒
        //list.Add(new AttrUnit(CSString.Format(37), attrDic[27].value.ToString()));
        //百分比物防
        list.Add(new AttrUnit(CSString.Format(27), $"{Math.Round(Convert.ToDecimal(attrDic[17].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //百分比魔防
        list.Add(new AttrUnit(CSString.Format(28), $"{Math.Round(Convert.ToDecimal(attrDic[18].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        AttrUnit auper = new AttrUnit();
        if (CSMainPlayerInfo.Instance.Career == ECareer.Warrior)
        {
            auper.name = CSString.Format(29);
            auper.value = $"{Math.Round(Convert.ToDecimal(attrDic[19].value * 0.01f), 3, MidpointRounding.AwayFromZero)}%";
        }
        else if (CSMainPlayerInfo.Instance.Career == ECareer.Master)
        {
            auper.name = CSString.Format(30);
            auper.value = $"{Math.Round(Convert.ToDecimal(attrDic[20].value * 0.01f), 3, MidpointRounding.AwayFromZero)}%";
        }
        else
        {
            auper.name = CSString.Format(31);
            auper.value = $"{Math.Round(Convert.ToDecimal(attrDic[21].value * 0.01f), 3, MidpointRounding.AwayFromZero)}%";
        }
        //攻击加成
        list.Add(auper);
        //生命加成
        list.Add(new AttrUnit(CSString.Format(32), $"{Math.Round(Convert.ToDecimal(attrDic[22].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //魔法加成
        list.Add(new AttrUnit(CSString.Format(33), $"{Math.Round(Convert.ToDecimal(attrDic[23].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //生命值回复绝对值/10秒
        list.Add(new AttrUnit(CSString.Format(1803), attrDic[48].value.ToString()));
        //魔法值回复绝对值/10秒
        list.Add(new AttrUnit(CSString.Format(1804), attrDic[51].value.ToString()));
        //泡点经验
        list.Add(new AttrUnit(CSString.Format(38), attrDic[28].value.ToString()));
        //泡点加成
        list.Add(new AttrUnit(CSString.Format(49), $"{Math.Round(Convert.ToDecimal(attrDic[39].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //抗暴百分比（万分位）
        list.Add(new AttrUnit(CSString.Format(42), $"{Math.Round(Convert.ToDecimal(attrDic[32].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //装备防爆
        list.Add(new AttrUnit(CSString.Format(43), $"{Math.Round(Convert.ToDecimal(attrDic[33].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //装备掉率加成，万分比
        list.Add(new AttrUnit(CSString.Format(537), $"{Math.Round(Convert.ToDecimal(attrDic[47].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //增加或者减少速度万分比
        //list.Add(new AttrUnit(CSString.Format(34), $"{Math.Round(Convert.ToDecimal(attrDic[24].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //治疗效果提升万分比
        list.Add(new AttrUnit(CSString.Format(35), $"{Math.Round(Convert.ToDecimal(attrDic[25].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //增伤-造成所有伤害增加,万分比
        list.Add(new AttrUnit(CSString.Format(45), $"{Math.Round(Convert.ToDecimal(attrDic[35].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //减伤-受到所有伤害减少,万分比
        list.Add(new AttrUnit(CSString.Format(46), $"{Math.Round(Convert.ToDecimal(attrDic[36].value * 0.01f), 2, MidpointRounding.AwayFromZero)}%"));


        //神圣攻击属性，无视防御的伤害
        //list.Add(new AttrUnit(CSString.Format(39), attrDic[29].value.ToString()));
        //PK伤害加成
        //list.Add(new AttrUnit(CSString.Format(40), $"{Math.Round(Convert.ToDecimal(attrDic[30].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //PK伤害减免
        //list.Add(new AttrUnit(CSString.Format(41), $"{Math.Round(Convert.ToDecimal(attrDic[31].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //攻击速度（用于角色或者怪物执行技能的公共间隔时间，也即公共CD。另外怪物攻打1次后，经过这个时间后再去查找敌人）
        //list.Add(new AttrUnit(CSString.Format(44), $"{Math.Round(Convert.ToDecimal(attrDic[34].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //无视所有防御,万分比
        //list.Add(new AttrUnit(CSString.Format(47), $"{Math.Round(Convert.ToDecimal(attrDic[37].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));
        //减伤-受到所有伤害减少,万分比
        //list.Add(new AttrUnit(CSString.Format(48), $"{Math.Round(Convert.ToDecimal(attrDic[38].value * 0.01f), 4, MidpointRounding.AwayFromZero)}%"));

        
        return list;
    }
    public class AttrUnit
    {
        public AttrUnit()
        {

        }
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
        if (obj_bar.value >= 0.95)
        {
            obj_arrow.SetActive(false);
        }
        else
        {
            obj_arrow.SetActive(true);
        }
    }
    void GetFightPowerChange(uint id, object data)
    {
        RefreshInfo();
    }
}
