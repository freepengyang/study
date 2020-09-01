using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TABLE;
using UnityEngine;

public partial class UINostalgiaTipPanel : UITipsBase
{
    private List<TipsBtnData> btnsName;
    private ILBetterList<string> suitStrs;
    private UIItemBase item;
    private TipDataItem _mes;
    public override void Init()
    {
        base.Init();
        btnsName = mPoolHandleManager.GetSystemClass<List<TipsBtnData>>();
        btnsName.Clear();
        suitStrs = mPoolHandleManager.GetSystemClass<ILBetterList<string>>();
        suitStrs.Clear();
        mClientEvent.AddEvent(CEvent.CloseTips, GetCloseTipsEvent);
    }
    
    protected override void OnDestroy()
    {
        mPoolHandleManager.Recycle(btnsName);
        mPoolHandleManager.Recycle(suitStrs);
        UIItemManager.Instance.RecycleSingleItem(item);
        item = null;
        mClientEvent.RemoveEvent(CEvent.CloseTips, GetCloseTipsEvent);
    }

    public override void ShowTip(TipsOpenType _type, TABLE.ITEM _cfg, bag.BagItemInfo _info = null, object data = null, System.Action _action = null)
    {
        structBtn(_type,_cfg,_info);
        RefreshBtns();
        int H = RefreshUIInfo(_type,_cfg,_info);
        mTable.Reposition();
        if (_mes!= null)
            mbg.height = 310 + _mes.Properties.Count * 16 + H;
    }

    /// <summary>
    /// 刷新ui信息
    /// </summary>
    private int RefreshUIInfo(TipsOpenType _type, ITEM _cfg, bag.BagItemInfo _info)
    {
        mlb_title.text = _cfg.name.BBCode(UtilityColor.GetColorTypeByQuality(_cfg.quality));
        mlb_lv.text = CSString.Format(570, _cfg.level);
        mlb_lv.color = (_cfg.level <= CSMainPlayerInfo.Instance.Level ? CSColor.beige : CSColor.red);
        
        int fightPower = UtilityFightPower.GetFightPower(_info, _cfg);
        mlb_fightPower.text = $"{CSString.Format(1267)}{fightPower}";
        
        mlb_job.text = Utility.GetJob(_cfg.career);
        mlb_job.color = (_cfg.career == CSMainPlayerInfo.Instance.Career) ||_cfg.career == 0 ? CSColor.beige : CSColor.red;
        msp_use.gameObject.SetActive(_type == TipsOpenType.HuaijiuEquip);
        if (item == null)
        {
            item = UIItemManager.Instance.GetItem(PropItemType.Normal, mitem.transform, itemSize.Size60);
            item.Refresh(_cfg,null,false);
        }
        else
        {
            item.Refresh(_cfg,null,false);
        }
        //刷新基础属性
        _mes = StructTipData.Instance.GetEquipBasicData(_cfg, _info);
        RefreshBasic(mdespGrid,_mes);
        //刷新套装效果
        int H = 0;
        H = ReFreshSuit(HuaiJiuSuitTableManager.Instance.GetHuaiJiuSuitType(_cfg.huaiJiuSuit));
        // HUAIJIUSUIT suit;
        //
        // if (HuaiJiuSuitTableManager.Instance.TryGetValue(_cfg.huaiJiuSuit,out suit))
        // {
        //     H = ReFreshSuit();
        // }

        return H;
    }
    
    //基础属性
    void RefreshBasic(UIGridContainer _go, TipDataItem _mes)
    {
        _go.MaxCount = _mes.Properties.Count;
        for (int i = 0; i < _mes.Properties.Count; i++)
        {
            UILabel key = Get<UILabel>("key", _go.controlList[i].transform);
            UILabel value = Get<UILabel>("value", _go.controlList[i].transform);
            key.text = _mes.Properties[i].Name;
            value.text = _mes.Properties[i].MaxValueName;
            key.color = _mes.Properties[i].Color;
        }
    }

    /// <summary>
    /// 刷新套装tip
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private int ReFreshSuit(int type)
    {
        int height = 0;
        
        var Info = CSNostalgiaEquipInfo.Instance;
        if (Info.suitInfos.ContainsKey(type))
        {
            var suit = Info.suitInfos[type];
            string maintitlestr = CSString.Format(1042, suit.equips.Count, Info.suitSubTypes.Count);
            string numstr = suit.equips.Count >= Info.suitSubTypes.Count
                ? maintitlestr.BBCode(ColorType.Green)
                : maintitlestr.BBCode(ColorType.Red);
            //套装数量
            suitStrs.Add(numstr);
            //激活条件
            var para = Getdesc1Param(suit.Huaijiusuit,suit.equips);
            if (para.Length == 5) 
                suitStrs.Add(CSString.Format(1973,para));
            //战魂效果
            suitStrs.Add(suit.Huaijiusuit.desc.BBCode(ColorType.MainText));
            if (!string.IsNullOrEmpty(suit.Huaijiusuit.descSmall))
                suitStrs.Add(string.Format(suit.Huaijiusuit.descSmall, UtilityColor.Yellow, UtilityColor.MainText));
            
            for (int i = 0; i < 4; i++)
            {
                var trans = msubTable.transform.GetChild(i);
                var value =UtilityObj.Get<UILabel>(trans, "value");
                if (suitStrs.Count > i)
                {
                    value.text = suitStrs[i];
                    height += value.height;
                }
                else
                {
                    trans.gameObject.SetActive(false);
                }

                if (i == 0)
                {
                    var key =UtilityObj.Get<UILabel>(trans, "key");
                    key.text = suit.Huaijiusuit.name;
                }
            }
            
            
        }

        

        return height;
    }

    private string[] Getdesc1Param(HUAIJIUSUIT suit,Dictionary<int,NostalgiaBagClass> equips)
    {
        List<string> list = mPoolHandleManager.GetSystemClass<List<string>>();
        list.Clear();
        var desc1color = UtilityColor.MainText;
        list.Add(desc1color);
        var equipNames = suit.descEquipName.Split('&');
        for (int i = 0; i < equipNames.Length; i++)
        {
            var temp = equipNames[i].Split('#');
            if (temp!= null)
            {
                int key;
                int.TryParse(temp[0], out key);
                var name = temp[1];
                if (equips.ContainsKey(key))
                    list.Add(name.BBCode(ColorType.Green));
            
                else
                    list.Add(name.BBCode(ColorType.WeakText));
            }
        }

        return list.ToArray();
    }
    
    public void structBtn(TipsOpenType _type, TABLE.ITEM _itemCfg, bag.BagItemInfo _info = null)
    {
        if (_type != TipsOpenType.HuaijiuEquip && _type != TipsOpenType.HuaijiuBag)
        {
            return;
        }
              
        btnsName.Clear();
        btnsName = StructTipData.Instance.StructBtnData(_type, _itemCfg, _info);
        
        if (_type == TipsOpenType.HuaijiuEquip)
        {
            //添加取出,升阶,丢弃
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(26), TipBtnType.HuaijiuTakeOff
                , _itemCfg, _info, _type));
        }

        if (_type == TipsOpenType.HuaijiuBag)
        {
            //添加放入,升阶,丢弃

            //判断,如果当前选中的装备
            bool isReplace = false;
            if (CSNostalgiaEquipInfo.Instance.IsRepleace(_itemCfg.huaiJiuSuit,_itemCfg.subType) != 0)
            {
                btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(29), 
                    TipBtnType.Huaijiureplace, _itemCfg, _info, _type));
                isReplace = true;
            }
            
            // var list = CSNostalgiaEquipInfo.Instance.EquipList;
            // HUAIJIUSUIT suit;
            //
            // if (HuaiJiuSuitTableManager.Instance.TryGetValue(_itemCfg.huaiJiuSuit,out suit))
            // {
            //     for (var it = list.GetEnumerator(); it.MoveNext();)
            //     {
            //         var huaijiusuit = it.Current.Value.Huaijiusuit;
            //         
            //         
            //         if (suit.type == huaijiusuit.type && suit.rank > huaijiusuit.rank && it.Current.Value.item.subType == _itemCfg.subType)
            //         {
            //             
            //            
            //         }
            //     }
            // }

            if (!isReplace)
                btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(25), 
                    TipBtnType.HuaijiuWear, _itemCfg, _info, _type));
        }
        
    }

    void RefreshBtns()
    {
        if (btnsName == null) return;
        btnsName.Reverse();
        mSubBtns.MaxCount = btnsName.Count;
        for (int i = 0; i < mSubBtns.controlList.Count; i++)
        {
            UILabel name = mSubBtns.controlList[i].transform.Find("Label").GetComponent<UILabel>();
            name.text = btnsName[i].name;

            if (btnsName[i].type == TipBtnType.HuaijiuWear || btnsName[i].type == TipBtnType.HuaijiuTakeOff || btnsName[i].type == TipBtnType.Huaijiureplace)
            {
                UISprite sprite = mSubBtns.controlList[i].transform.Find("Background").GetComponent<UISprite>();
                sprite.spriteName = "btn_samll1";
                name.color = UtilityColor.HexToColor("#b0bbcf");
            }
            
            UIEventListener.Get(mSubBtns.controlList[i], btnsName[i]).onClick = TipBtnClick;
            
            Transform RedPoint = mSubBtns.controlList[i].transform.Find("sp_icon");
            if (RedPoint != null)
            {
                var type = btnsName[i].type;
                if (type == TipBtnType.HuaijiuLevelUp)
                {
                    var Info = CSNostalgiaEquipInfo.Instance;
                    if (btnsName[i].info != null)
                    {
                        int configid = btnsName[i].cfg.id;
                        NostalgiaBagClass bagClass = null;
                        if (Info.BagList.ContainsKey(btnsName[i].info.id))
                            bagClass = Info.BagList[btnsName[i].info.id];
                        if (Info.EquipList.ContainsKey(btnsName[i].info.id))
                            bagClass = Info.EquipList[btnsName[i].info.id];
                        
                        if (Info.bagStackList.ContainsKey(configid))
                        {
                            var list = Info.bagStackList[configid];
                            int num = list.Count;
                        
                            long bagid = btnsName[i].info.id;
                            for (int j = 0; j < list.Count; j++)
                            {
                                if (list[j].bagiteminfo.id == bagid)
                                    num--;
                            }

                            if (bagClass != null)
                            {
                                RedPoint.gameObject.SetActive(num>=2 && bagClass.Huaijiusuit.nextID != 0);
                            }
                            
                    }
                  }
                }
                if (type == TipBtnType.Huaijiureplace)
                {
                    RedPoint.gameObject.SetActive(true);
                }
                
            }

            
        }
    }
    
    void GetCloseTipsEvent(uint id, object data)
    {
        Close();
    }
    
}