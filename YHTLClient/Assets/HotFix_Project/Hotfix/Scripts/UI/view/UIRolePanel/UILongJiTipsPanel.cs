using System.Collections.Generic;
using UnityEngine;

public partial class UILongJiTipsPanel : UIBasePanel
{
    Dictionary<int, LongJiEff> longjiDic = new Dictionary<int, LongJiEff>();
    ILBetterList<LongJiEff> longjiList = new ILBetterList<LongJiEff>();
    ILBetterList<LongJiTipsItem> tipsItemList = new ILBetterList<LongJiTipsItem>();
    public override void Init()
    {
        base.Init();
        AddCollider();
        CSBagInfo.Instance.GetAllLongJiInfo(longjiDic);
        int career = CSMainPlayerInfo.Instance.Career;
        var iter = longjiDic.GetEnumerator();
        while (iter.MoveNext())
        {
            longjiList.Add(iter.Current.Value);
        }
        if (longjiList.Count == 0)
        {
            mlb_noLongJi.text = ClientTipsTableManager.Instance.GetClientTipsContext(1878);
            mscr_longji.gameObject.SetActive(false);
        }
        else
        {
            mscr_longji.gameObject.SetActive(true);
            mgrid_longji.MaxCount = longjiList.Count;
            for (int i = 0; i < longjiList.Count; i++)
            {
                tipsItemList.Add(new LongJiTipsItem(mgrid_longji.controlList[i]));
                tipsItemList[i].Refresh(longjiList[i], i);
            }
        }
    }

    public override void Show()
    {
        base.Show();

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public class LongJiEff
    {
        public int id;
        public int value;
        public int qua;
        public LongJiEff(bag.WolongRandomEffect _eff)
        {
            id = _eff.id;
            value = _eff.effectValue;
            qua = _eff.quality;
        }
        public void AddValue(int _value)
        {
            value = value + _value;
        }
        public string name
        {
            get
            {
                return WoLongRandomAttrTableManager.Instance.GetWoLongRandomAttrName(id);
            }
        }
    }
    class LongJiTipsItem
    {
        GameObject go;
        GameObject bg;
        UILabel name;
        UILabel value;
        public LongJiTipsItem(GameObject _go)
        {
            go = _go;
            bg = go.transform.Find("bg").gameObject;
            name = go.transform.Find("name").GetComponent<UILabel>();
            value = go.transform.Find("value").GetComponent<UILabel>();
        }
        public void Refresh(LongJiEff _eff, int _ind)
        {
            name.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1731), _eff.name);
            value.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1875),_eff.value);
            name.color = UtilityCsColor.Instance.GetColor(5);
            value.color = UtilityCsColor.Instance.GetColor(5);
            bg.SetActive(_ind % 2 == 0 ? true : false);
        }
    }
}
