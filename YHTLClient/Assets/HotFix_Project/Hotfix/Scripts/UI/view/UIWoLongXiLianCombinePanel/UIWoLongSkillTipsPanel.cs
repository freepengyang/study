using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoLongAffixEffect
{
    public int id;
    public int per;
    public int plus;
    public int value;
}
public partial class UIWoLongSkillTipsPanel : UIBasePanel
{
    public override UILayerType PanelLayerType { get { return UILayerType.Tips; } }
    int skillId = 0;
    long itemId = 0;
    RepeatedField<bag.WolongRandomEffect> mesList;
    int configId = 0;
    string add = "+";
    string reduce = "-";
    public override void Init()
    {
        base.Init();
        AddCollider();
    }
    public void SetData(int _skillId, long _itemId, int _configid, RepeatedField<bag.WolongRandomEffect> _mesList)
    {
        skillId = _skillId;
        itemId = _itemId;
        configId = _configid;
        mesList = _mesList;
        Refresh();
    }
    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    string para1 = "";
    string para2 = "";
    string para3 = "";
    string para4 = "";
    string para5 = "";
    void Refresh()
    {
        mobj_view.alpha = 0;
        Dictionary<int, WoLongAffixEffect> paraDic = CSBagInfo.Instance.GetWoLongIntenAffixState(skillId, itemId);
        if (mesList != null)
        {
            for (int i = 0; i < mesList.Count; i++)
            {
                TABLE.WOLONGRANDOMATTR cfg = WoLongRandomAttrTableManager.Instance.GetCfgById(mesList[i].id);
                TABLE.ZHANCHONGCIZHUIEFFECT cizhui = ZhanChongCiZhuiEffectTableManager.Instance.GetDataByType(cfg.parameter);
                Dictionary<int, WoLongAffixEffect> temp_dic = paraDic;
                if (!temp_dic.ContainsKey(cizhui.type))
                {

                    temp_dic.Add(cizhui.type, new WoLongAffixEffect());
                }
                WoLongAffixEffect eff = temp_dic[cizhui.type];
                eff.id = cizhui.id;
                eff.per = cizhui.per;
                eff.plus = cizhui.plus;
                eff.value = eff.value + mesList[i].effectValue;
            }
        }
        if (paraDic != null)
        {
            var iter = paraDic.GetEnumerator();
            for (int i = 1; i <= 5; i++)
            {
                if (paraDic.ContainsKey(i))
                {
                    WoLongAffixEffect eff = paraDic[i];
                    string AddOrReduce = (eff.plus == 1) ? add : reduce;
                    string value = eff.value.ToString();
                    int point = ZhanChongCiZhuiEffectTableManager.Instance.GetZhanChongCiZhuiEffectPoint(eff.id);
                    if (eff.per == 10000)
                    {
                        value = Math.Round(Convert.ToDecimal(eff.value * 0.01f), point, MidpointRounding.AwayFromZero).ToString();
                    }
                    else if (eff.per == 1000)
                    {
                        value = Math.Round(Convert.ToDecimal(eff.value * 0.001f), point, MidpointRounding.AwayFromZero).ToString();
                    }
                    if (i == 1)
                    {
                        para1 = $"[00ff00]({AddOrReduce}{value})[-]";
                    }
                    else if (i == 2)
                    {
                        para2 = $"[00ff00]({AddOrReduce}{value})[-]";
                    }
                    else if (i == 3)
                    {
                        para3 = $"[00ff00]({AddOrReduce}{value})[-]";
                    }
                    else if (i == 4)
                    {
                        para4 = $"[00ff00]({AddOrReduce}{value})[-]";
                    }
                    else if (i == 5)
                    {
                        para5 = $"[00ff00]({AddOrReduce}{value})[-]";
                    }
                }
            }
        }
        SkillTableManager skillIns = SkillTableManager.Instance;
        int skillLv = CSWoLongInfo.Instance.WoLongEnabledSkillLevel(skillId);
        msp_skill_icon.spriteName = skillIns.GetIconByGroupId(skillId);
        mlb_skill_name.text = skillIns.GetNameByGroupId(skillId);
        int count = CSBagInfo.Instance.GetWoLongLongLiAffixCount(skillId);
        mlb_skill_lv.text = $"{count}/12";
        mlb_skill_lv.color = count >= 12 ? CSColor.green : CSColor.red;
        mlb_skill_desc.text = string.Format(skillIns.GetDesByGroupId(skillId, skillLv), para1, para2, para3, para4, para5);
        mobj_line.localPosition = new Vector3(-6, mlb_skill_desc.transform.localPosition.y - mlb_skill_desc.height, 0);
        if (count >= 12)
        {
            mlb_skill_cd_time.text = skillIns.GetTipsDesByGroupId(skillId, skillLv);
            mlb_lock.gameObject.SetActive(false);

            mlb_skill_cd_time.transform.localPosition = new Vector3(-156, mobj_line.localPosition.y - 13, 0);
            msp_bg.height = 109 + mlb_skill_desc.height + 13 + mlb_skill_cd_time.height + 5;
        }
        else
        {
            mlb_lock.gameObject.SetActive(true);
            int levClass = ItemTableManager.Instance.GetItemLevClass(configId);
            int levClassResult = (levClass - 3) < 0 ? 1 : levClass - 3;
            string[] suitName = SundryTableManager.Instance.GetSundryEffect(1127).Split('#');
            mlb_lock.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(2028), suitName[levClass]);
            mlb_skill_cd_time.text = skillIns.GetTipsDesByGroupId(skillId, levClassResult);

            mlb_lock.transform.localPosition = new Vector3(-156, mobj_line.localPosition.y - 13, 0);
            mlb_skill_cd_time.transform.localPosition = new Vector3(-156, mlb_lock.transform.localPosition.y - mlb_lock.height - 10, 0);
            msp_bg.height = 109 + mlb_skill_desc.height + 13 + mlb_lock.height + 10 + mlb_skill_cd_time.height + 5;
        }


        ScriptBinder.StartCoroutine(DelayShow());
    }

    IEnumerator DelayShow()
    {
        yield return new WaitForSeconds(0.1f);
        mobj_view.alpha = 1;
    }
}
