using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UILongLiTipsPanel : UIBasePanel
{
    Dictionary<int, LongLiBaseAff> baseAffixCount = new Dictionary<int, LongLiBaseAff>();
    Dictionary<int, Dictionary<int, LongLiEff>> wolongAffixCount = new Dictionary<int, Dictionary<int, LongLiEff>>();
    ILBetterList<LongLiBaseAff> baseCountList = new ILBetterList<LongLiBaseAff>(12);
    ILBetterList<LongLiBase> baseItems = new ILBetterList<LongLiBase>(12);
    ILBetterList<LongLiIntenDes> desItems = new ILBetterList<LongLiIntenDes>(12);
    string para1 = "";
    string para2 = "";
    string para3 = "";
    string para4 = "";
    string para5 = "";
    string add = "+";
    string reduce = "-";

    public override void Init()
    {
        base.Init();
        AddCollider();
        mscrBar.onChange.Add(new EventDelegate(OnChange));
        CSBagInfo.Instance.GetAllLongLiInfo(baseAffixCount, wolongAffixCount);
        for (var it = baseAffixCount.GetEnumerator(); it.MoveNext();)
        {
            baseCountList.Add(it.Current.Value);
        }
        baseCountList.Sort((a, b) =>
        {
            return b.value - a.value;
        });
        mgrid_base.MaxCount = baseCountList.Count;
        for (int i = 0; i < baseCountList.Count; i++)
        {
            baseItems.Add(new LongLiBase(mgrid_base.controlList[i]));
            baseItems[i].Refresh(baseCountList[i]);
        }
        string str = "";


        //des
        desItems.Add(new LongLiIntenDes(mlb_intenDes.gameObject));
        for (int i = 0; i < wolongAffixCount.Count - 1; i++)
        {
            GameObject go = GameObject.Instantiate(mlb_intenDes.gameObject, mtable_des.transform);
            desItems.Add(new LongLiIntenDes(go));
        }
        for (int j = 0; j < baseCountList.Count; j++)
        {
            int key = baseCountList[j].id;
            Dictionary<int, LongLiEff> perDic = wolongAffixCount[baseCountList[j].id];
            for (int i = 1; i <= 5; i++)
            {
                if (perDic.ContainsKey(i))
                {
                    LongLiEff eff = perDic[i];
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

            int skillLv = CSWoLongInfo.Instance.WoLongEnabledSkillLevel(key);
            string t_str = string.Format(SkillTableManager.Instance.GetDesByGroupId(key, skillLv), para1, para2, para3, para4, para5);
            string name = SkillTableManager.Instance.GetNameByGroupId(key);
            if (baseAffixCount.ContainsKey(key) && baseAffixCount[key].value < 12)
            {
                name = name.Replace("00fff0", "888580");
                t_str = t_str.Replace("dcd5b8", "888580");
                t_str = t_str.Replace("00ff00", "888580");
            }
            desItems[j].Text($"{str}{name}:{t_str}");
        }
        //var iter = wolongAffixCount.GetEnumerator();
        //int ind = 0;
        //while (iter.MoveNext())
        //{
        //    Dictionary<int, LongLiEff> perDic = iter.Current.Value;
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        if (perDic.ContainsKey(i))
        //        {
        //            LongLiEff eff = perDic[i];
        //            string AddOrReduce = (eff.plus == 1) ? add : reduce;
        //            string value = eff.value.ToString();
        //            int point = ZhanChongCiZhuiEffectTableManager.Instance.GetZhanChongCiZhuiEffectPoint(eff.id);
        //            if (eff.per == 10000)
        //            {
        //                value = Math.Round(Convert.ToDecimal(eff.value * 0.01f), point, MidpointRounding.AwayFromZero).ToString();
        //            }
        //            else if (eff.per == 1000)
        //            {
        //                value = Math.Round(Convert.ToDecimal(eff.value * 0.001f), point, MidpointRounding.AwayFromZero).ToString();
        //            }
        //            if (i == 1)
        //            {
        //                para1 = $"[00ff00]({AddOrReduce}{value})[-]";
        //            }
        //            else if (i == 2)
        //            {
        //                para2 = $"[00ff00]({AddOrReduce}{value})[-]";
        //            }
        //            else if (i == 3)
        //            {
        //                para3 = $"[00ff00]({AddOrReduce}{value})[-]";
        //            }
        //            else if (i == 4)
        //            {
        //                para4 = $"[00ff00]({AddOrReduce}{value})[-]";
        //            }
        //            else if (i == 5)
        //            {
        //                para5 = $"[00ff00]({AddOrReduce}{value})[-]";
        //            }
        //        }
        //    }

        //    int skillLv = CSWoLongInfo.Instance.WoLongEnabledSkillLevel(iter.Current.Key);
        //    string t_str = string.Format(SkillTableManager.Instance.GetDesByGroupId(iter.Current.Key, skillLv), para1, para2, para3, para4, para5);
        //    string name = SkillTableManager.Instance.GetNameByGroupId(iter.Current.Key);
        //    if (baseAffixCount.ContainsKey(iter.Current.Key) && baseAffixCount[iter.Current.Key].value < 12)
        //    {
        //        name = name.Replace("00fff0", "888580");
        //        t_str = t_str.Replace("dcd5b8", "888580");
        //        t_str = t_str.Replace("00ff00", "888580");
        //    }
        //    desItems[ind].Text($"{str}{name}:{t_str}");
        //    ind++;
        //}
        mtable_des.Reposition();
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    void OnChange()
    {
        if (!mscr_des.shouldMoveVertically)
        {
            mobj_arrow.SetActive(false);
            return;
        }
        if (mscrBar.value >= 0.95)
        {
            mobj_arrow.SetActive(false);
        }
        else
        {
            mobj_arrow.SetActive(true);
        }
    }

}
class LongLiBase
{
    GameObject go;
    UISprite icon;
    UILabel name;
    UILabel count;
    public LongLiBase(GameObject _go)
    {
        go = _go;
        icon = go.transform.Find("sp_buff").GetComponent<UISprite>();
        name = go.transform.Find("key").GetComponent<UILabel>();
        count = go.transform.Find("value").GetComponent<UILabel>();
    }
    public void Refresh(LongLiBaseAff _data)
    {
        icon.spriteName = SkillTableManager.Instance.GetIconByGroupId(_data.id);
        name.text = SkillTableManager.Instance.GetNameByGroupId(_data.id);
        count.text = $"{_data.value}/12";
        count.color = (_data.value >= 12) ? CSColor.green : CSColor.red;
        icon.color = (_data.value >= 12) ? CSColor.white : CSColor.gray;
    }
}
class LongLiIntenDes
{
    UILabel des;
    public LongLiIntenDes(GameObject _go)
    {
        des = _go.GetComponent<UILabel>();
    }
    public void Text(string _str)
    {
        des.text = _str;
    }
}
public class LongLiEff
{
    public int id;
    public int per;
    public int plus;
    public int value;
    public LongLiEff()
    {

    }
}
public class LongLiBaseAff
{
    public int id;
    public int value;
    public LongLiBaseAff()
    {

    }
}

