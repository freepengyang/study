using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABLE;
using UnityEngine;

public partial class UIPropItemPanel : UITipsBase
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.None;
    }

    protected UISprite msp_bg;
    protected UILabel mlb_title;
    protected UnityEngine.GameObject mobj_Isbind;
    protected UILabel mlb_lv;
    protected UILabel mlb_job;
    protected UISprite msp_itembg;
    protected UISprite msp_icon;
    protected UILabel mlb_type;
    protected UIGridContainer mgrid_btns;
    protected UILabel mlb_des;
    protected GameObject mobj_mask;
    protected UISprite msp_line;
    protected GameObject mtex_bg;
    protected GameObject mobj_desPart;
    protected UILabel mlb_Extrades;
    protected UITable mtable_conPar;
    protected GameObject mobj_gemAttrs;
    protected UIGridContainer mgrid_gemAttrs;
    protected GameObject mobj_gemLine;
    protected UIWidget mscroll_widget;
    protected GameObject mobj_titlePart;
    protected UILabel mlb_UseCount;

    protected override void _InitScriptBinder()
    {
        msp_bg = ScriptBinder.GetObject("sp_bg") as UISprite;
        mlb_title = ScriptBinder.GetObject("lb_title") as UILabel;
        mobj_Isbind = ScriptBinder.GetObject("obj_Isbind") as UnityEngine.GameObject;
        mlb_lv = ScriptBinder.GetObject("lb_lv") as UILabel;
        mlb_job = ScriptBinder.GetObject("lb_job") as UILabel;
        msp_itembg = ScriptBinder.GetObject("sp_itembg") as UISprite;
        msp_icon = ScriptBinder.GetObject("sp_icon") as UISprite;
        mlb_type = ScriptBinder.GetObject("lb_type") as UILabel;
        mgrid_btns = ScriptBinder.GetObject("grid_btns") as UIGridContainer;
        mlb_des = ScriptBinder.GetObject("lb_des") as UILabel;
        mobj_mask = ScriptBinder.GetObject("obj_mask") as UnityEngine.GameObject;
        msp_line = ScriptBinder.GetObject("sp_line") as UISprite;
        mtex_bg = ScriptBinder.GetObject("tex_bg") as GameObject;
        mobj_desPart = ScriptBinder.GetObject("obj_desPart") as GameObject;
        mlb_Extrades = ScriptBinder.GetObject("lb_Extrades") as UILabel;
        mtable_conPar = ScriptBinder.GetObject("table_conPar") as UITable;
        mobj_gemAttrs = ScriptBinder.GetObject("obj_gemAttrs") as GameObject;
        mgrid_gemAttrs = ScriptBinder.GetObject("grid_gemAttrs") as UIGridContainer;
        mobj_gemLine = ScriptBinder.GetObject("obj_gemLine") as GameObject;
        mscroll_widget = ScriptBinder.GetObject("scroll_widget") as UIWidget;
        mobj_titlePart = ScriptBinder.GetObject("obj_titlePart") as GameObject;
        mlb_UseCount = ScriptBinder.GetObject("lb_UseCount") as UILabel;
    }

    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Tips; }
    }

    TABLE.ITEM cfg;
    bag.BagItemInfo info;
    List<TipsBtnData> btnsName = new List<TipsBtnData>();

    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.CloseTips, GetCloseTipsEvent);
    }

    public override void ShowTip(TipsOpenType _type, TABLE.ITEM _cfg, bag.BagItemInfo _info = null, object data = null,
        System.Action _action = null)
    {
        UIEventListener.Get(mobj_mask).onClick = p => { UIManager.Instance.ClosePanel(this.GetType()); };
        cfg = _cfg;
        info = _info;
        base.ShowTip(_type, _cfg, _info);
        StruectureData(_type, _cfg, _info);
        StructBtnData(_type, _cfg, _info);
        Refresh();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mtex_bg);
        base.OnDestroy();
    }

    void GetCloseTipsEvent(uint id, object data)
    {
        UIManager.Instance.ClosePanel<UIPropItemPanel>();
    }

    public void StruectureData(TipsOpenType _type, TABLE.ITEM _itemCfg, bag.BagItemInfo _info = null)
    {
    }

    public void StructBtnData(TipsOpenType _type, TABLE.ITEM _itemCfg, bag.BagItemInfo _info = null)
    {
        btnsName.Clear();
        if (_type == TipsOpenType.Bag)
        {
            btnsName = StructTipData.Instance.StructBtnData(_type, _itemCfg, _info);
            for (int i = btnsName.Count - 1; i >= 0; i--)
            {
                if (btnsName[i].type == TipBtnType.Split)
                {
                    if (_info.count <= 1)
                    {
                        btnsName.RemoveAt(i);
                    }
                }
                else if (btnsName[i].type == TipBtnType.Compound)
                {
                    if (!UICheckManager.Instance.DoCheckFunction(FunctionType.funcP_HeCheng) ||
                        !CSCompoundInfo.Instance.IsAbilityCombine(info.configId))
                    {
                        btnsName.RemoveAt(i);
                    }
                }
                else if (btnsName[i].type == TipBtnType.BatchUse)
                {
                    int batchUse = ItemOperateTableManager.Instance.GetOperaTypeForBatchUse(_itemCfg.type,
                        _itemCfg.subType, _itemCfg.Operationtype);
                    if (batchUse == 0 || _info.count <= 1)
                    {
                        btnsName.RemoveAt(i);
                    }
                }
                else if (btnsName[i].type == TipBtnType.Compound)
                {
                    if (!CSCompoundInfo.Instance.IsCombined(_itemCfg.id))
                    {
                        btnsName.RemoveAt(i);
                    }
                }
            }
        }
        else if (_type == TipsOpenType.BagWarehouse) //只有一个放入 (仓库打开时背包格子)
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(10), TipBtnType.PutIn, _itemCfg,
                _info, _type));
        }
        else if (_type == TipsOpenType.WarehouseBag) //只有一个取出  （仓库打开时仓库格子点击）
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(11), TipBtnType.TakeOut, _itemCfg,
                _info, _type));
        }
        else if (_type == TipsOpenType.WildAdventureRewardReceive) //只有一个取出  （野外探险用）
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(11), TipBtnType.TakeOut, _itemCfg,
                _info, _type));
        }
        else if (_type == TipsOpenType.GuildWareHouseDonate)//公会仓库捐赠
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(12), TipBtnType.Donate, _itemCfg, _info, _type));
        }
        else if (_type == TipsOpenType.GuildWareHouseExchange)//公会仓库兑换
        {
            btnsName.Add(new TipsBtnData(StructTipData.Instance.GetTipBtnNameByType(13), TipBtnType.Exchange, _itemCfg, _info, _type));
        }
    }

    public void Refresh()
    {
        RefreshTitle();
    }

    void RefreshTitle()
    {
        msp_bg.alpha = 0;
        mscroll_widget.alpha = 0;
        mgrid_btns.gameObject.SetActive(false);
        mobj_titlePart.SetActive(false);

        mlb_title.text = cfg.name;
        mlb_title.color = UtilityCsColor.Instance.GetColor(cfg.quality);
        if (info == null)
        {
            mobj_Isbind.gameObject.SetActive(false);
        }
        else
        {
            mobj_Isbind.gameObject.SetActive(info.bind == 1);
        }

        msp_itembg.spriteName = ItemTableManager.Instance.GetItemQualityBG(cfg.quality);
        CSStringBuilder.Clear();
        msp_line.spriteName = CSStringBuilder.Append("line_eqtips", cfg.quality).ToString();
        CSStringBuilder.Clear();
        string str = CSStringBuilder.Append("qualitybg", cfg.quality).ToString();
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, str);


        mlb_lv.text = cfg.level + "级";
        mlb_lv.color = (cfg.level <= CSMainPlayerInfo.Instance.Level ? CSColor.beige : CSColor.red);
        mlb_job.text = Utility.GetJob(cfg.career);
        if ((cfg.career == CSMainPlayerInfo.Instance.Career) || cfg.career == 0)
        {
            mlb_job.color = CSColor.beige;
        }
        else
        {
            mlb_job.color = CSColor.red;
        }

        //mlb_job.color = ((int)cfg.career == CSMainPlayerInfo.Instance.GetMyInfo().roleBrief.career ? CSColor.beige : CSColor.red);
        msp_itembg.spriteName = ItemTableManager.Instance.GetItemQualityBG(cfg.quality);
        msp_icon.spriteName = cfg.icon;
        if (cfg.type == (int)ItemType.Box && cfg.subType == 7) //真气内丹特殊处理 根据level表走
        {
            int mul = LevelTableManager.Instance.GetLevelZhenqi(CSMainPlayerInfo.Instance.Level);
            mlb_des.text = $"[dcd5b8]{string.Format(cfg.tips, mul * cfg.data)}";
        }
        else
        {
            mlb_des.text = $"[dcd5b8]{cfg.tips}";
        }

        if (!string.IsNullOrEmpty(cfg.tips2))
        {
            mlb_Extrades.text = $"[dcd5b8]{ClientTipsTableManager.Instance.GetClientTipsContext(743)}{cfg.tips2}";
            mobj_desPart.SetActive(true);
        }
        else
        {
            mobj_desPart.SetActive(false);
        }

        //gam相关
        if (cfg.type == 9)
        {
            //LongArray attrs = new LongArray();
            RepeatedField<CSAttributeInfo.KeyValue> attrItems = new RepeatedField<CSAttributeInfo.KeyValue>();
            if (cfg.subType == 6 || cfg.subType == 7)
            {
                string[] typeName = SundryTableManager.Instance.GetSundryEffect(1076).Split('#');
                mlb_type.text = typeName[cfg.subType - 6];
                YULINGSOUL yulingsoul;
                if (YuLingSoulTableManager.Instance.TryGetValue(cfg.id, out yulingsoul))
                {
                    var attrs = CSWingInfo.Instance.GetBaseAttrParaByCareer(CSMainPlayerInfo.Instance.Career,
                        yulingsoul);
                    attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrs);
                    attrs = CSWingInfo.Instance.GetSpecialAttrParaByCareer(CSMainPlayerInfo.Instance.Career,
                        yulingsoul);
                    attrItems.AddRange(CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrs));
                }

                mobj_gemAttrs.SetActive(true);

                //var attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrs);
                mgrid_gemAttrs.MaxCount = attrItems.Count;
                for (int i = 0; i < attrItems.Count; i++)
                {
                    UILabel des = mgrid_gemAttrs.controlList[i].GetComponent<UILabel>();
                    des.text = $"{attrItems[i].Key}{CSString.Format(999)}{attrItems[i].Value}";
                    //Debug.Log(attrItems[i].Key + "   " + CSString.Format(999) + "   " + attrItems[i].Value);
                }

                mobj_gemLine.transform.localPosition = new Vector3(109, 10 - 25 * attrItems.Count, 0);
            }
            else if (cfg.subType == 8)
            {
                mobj_gemAttrs.SetActive(true);
                YULINGSOUL yulingsoul;
                if (YuLingSoulTableManager.Instance.TryGetValue(cfg.id, out yulingsoul))
                {
                    var attrs = CSWingInfo.Instance.GetBaseAttrParaByCareer(CSMainPlayerInfo.Instance.Career,
                        yulingsoul);
                    attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrs);
                }

                mgrid_gemAttrs.MaxCount = attrItems.Count + 1;
                for (int i = 0, max = mgrid_gemAttrs.MaxCount; i < max; i++)
                {
                    UILabel des = mgrid_gemAttrs.controlList[i].GetComponent<UILabel>();
                    if (i < max - 1)
                        des.text = $"{attrItems[i].Key}{CSString.Format(999)}{attrItems[i].Value}";
                    else
                        des.text = CSString.Format(1950, $"+{yulingsoul.exattr * 1f / 100}%").BBCode(ColorType.Green);
                }

                mobj_gemLine.transform.localPosition = new Vector3(109, 10 - 25 * (attrItems.Count + 1), 0);
            }
            else
            {
                mlb_type.text = StructTipData.Instance.GetItemTypeName(cfg.type);
                GEM gemTableData;
                if (GemTableManager.Instance.TryGetValue(cfg.id, out gemTableData))
                {
                    var attrs = CSGemInfo.Instance.GetAttrParaByCareer(CSMainPlayerInfo.Instance.Career, gemTableData);
                    attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrs);
                }

                mobj_gemAttrs.SetActive(true);

                //var attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrs);
                mgrid_gemAttrs.MaxCount = attrItems.Count;
                for (int i = 0; i < attrItems.Count; i++)
                {
                    UILabel des = mgrid_gemAttrs.controlList[i].GetComponent<UILabel>();
                    des.text = $"{attrItems[i].Key}{CSString.Format(999)}{attrItems[i].Value}";
                    //Debug.Log(attrItems[i].Key + "   " + CSString.Format(999) + "   " + attrItems[i].Value);
                }
                mobj_gemLine.transform.localPosition = new Vector3(109, 10 - 25 * attrItems.Count, 0);
            }
        }
        else
        {
            mobj_gemAttrs.SetActive(false);
            mlb_type.text = StructTipData.Instance.GetItemTypeName(cfg.type);
        }

        //今日使用次数
        if (cfg.limit != 0)
        {
            mlb_UseCount.transform.parent.gameObject.SetActive(true);
            bag.ResItemUsedDaily count = CSBagInfo.Instance.RetuanSingleItemUseCountMes(cfg.group);
            string countStr = "";
            if (count != null)
            {
                int leftusecount = count.dailyMaxUseCount - count.dailyUsedCount;
                if (leftusecount > 0)
                {
                    countStr = $"[00ff00]{leftusecount}/{count.dailyMaxUseCount}";
                }
                else
                {
                    countStr = $"[ff0000]{leftusecount}/{count.dailyMaxUseCount}";
                }
            }

            mlb_UseCount.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(2003), countStr);
        }
        else
        {
            mlb_UseCount.transform.parent.gameObject.SetActive(false);
        }

        int gap = 10;
        float temp_toHeigh = 0;
        for (int i = 0; i < mtable_conPar.transform.childCount; i++)
        {
            float temp_heigh = NGUIMath.CalculateRelativeWidgetBounds(mtable_conPar.transform.GetChild(i), false).size
                .y;
            temp_toHeigh = temp_toHeigh + temp_heigh;
        }

        int total_high = (int)(140 + temp_toHeigh + gap);
        msp_bg.height = total_high;
        RefreshBtns();
        mtable_conPar.Reposition();

        ScriptBinder.StartCoroutine(ChangePos());
    }

    IEnumerator ChangePos()
    {
        yield return null;
        msp_bg.alpha = 1f;
        mscroll_widget.alpha = 1f;
        mgrid_btns.gameObject.SetActive(true);
        mobj_titlePart.SetActive(true);
    }

    void RefreshBtns()
    {
        if (btnsName == null) return;
        btnsName.Reverse();
        mgrid_btns.MaxCount = btnsName.Count;
        for (int i = 0; i < mgrid_btns.controlList.Count; i++)
        {
            UILabel btnName = mgrid_btns.controlList[i].transform.Find("Label").GetComponent<UILabel>();
            btnName.text = btnsName[i].name;
            UIEventListener.Get(mgrid_btns.controlList[i], btnsName[i]).onClick = TipBtnClick;
            if (btnsName[i].type == TipBtnType.Use || btnsName[i].type == TipBtnType.BatchUse ||
                btnsName[i].type == TipBtnType.Inlaid)
            {
                UISprite sprite = mgrid_btns.controlList[i].transform.Find("Background").GetComponent<UISprite>();
                sprite.spriteName = "btn_samll1";
                btnName.color = UtilityColor.HexToColor("#b0bbcf");
            }
        }
    }
}