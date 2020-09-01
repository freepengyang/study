using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using bag;
using Google.Protobuf.Collections;
using TABLE;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using wing;

public partial class UIWingColorPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    private WingData myWingData;

    /// <summary>
    /// 排序后的幻彩展示列表
    /// </summary>
    private ILBetterList<WingColorData> listWingColor;

    /// <summary>
    /// 上一个选中的幻彩索引
    /// </summary>
    private int lastSelectIndex = 0;

    /// <summary>
    /// 当前选中的幻彩索引
    /// </summary>
    private int selectIndex = 0;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.GetWingInfo, RefreshData);
        mClientEvent.Reg((uint) CEvent.DressWingColor, RefreshDataDress);
        mClientEvent.Reg((uint) CEvent.WingColorChange, RefreshDataItemChange);
        mbtn_attribute.onClick = OnClickAttribute;
        CSEffectPlayMgr.Instance.ShowUITexture(msp_stage_effect_bg, "wing_bg");
        CSEffectPlayMgr.Instance.ShowUIEffect(meffect_wing_idle_add, 17801);
    }

    void RefreshData(uint id, object data)
    {
        InitData();
    }

    /// <summary>
    /// 穿戴/脱下
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void RefreshDataDress(uint id, object data)
    {
        if (data == null) return;
        DressColorWingResponse msg = (DressColorWingResponse) data;
        lastSelectIndex = 0;
        selectIndex = 0;
        InitData();
        if (msg.type == 1)
            UtilityTips.ShowGreenTips(664, HuanCaiTableManager.Instance.GetHuanCaiName(msg.itemId));
        else
            UtilityTips.ShowTips(1616);
    }

    /// <summary>
    /// 激活/到期
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void RefreshDataItemChange(uint id, object data)
    {
        if (data == null) return;
        WingColorChange msg = (WingColorChange) data;
        lastSelectIndex = 0;
        selectIndex = 0;

        int wingcolorId = 0;
        for (int i = 0; i < msg.wingColorInfos.Count; i++)
        {
            WingColorInfo wingColorInfo = msg.wingColorInfos[i];
            if (wingColorInfo.endTime != 0) //永久为-1  大于0为有时间限制
            {
                wingcolorId = wingColorInfo.id;
                UtilityTips.ShowGreenTips(665, HuanCaiTableManager.Instance.GetHuanCaiName(wingcolorId));
                break;
            }
        }


        if (wingcolorId > 0)
        {
            myWingData = CSWingInfo.Instance.MyWingData;
            if (myWingData == null || ItemTableManager.Instance == null) return;
            SortWingColor();
            if (listWingColor == null || listWingColor.Count == 0) return;
            for (int i = 0; i < listWingColor.Count; i++)
            {
                if (listWingColor[i].configId == wingcolorId)
                {
                    selectIndex = i;
                    break;
                }
            }

            ShowWingColorInfo(listWingColor[0].configId);
            RefreshGrid();
        }
        else
        {
            InitData();
            ShowModel(listWingColor[selectIndex].configId);
        }
    }

    public override void Show()
    {
        base.Show();
        if (CSWingInfo.Instance.MyWingData == null)
        {
            Net.CSWingInfoMessage();
        }
        else
        {
            ScriptBinder.InvokeRepeating(0, 1f, OnSchedule);
            InitData();
            ShowModel(listWingColor[selectIndex].configId);
        }
    }

    /// <summary>
    /// 幻彩展示列表排序
    /// </summary>
    void SortWingColor()
    {
        if (listWingColor == null)
        {
            listWingColor = new ILBetterList<WingColorData>();
        }

        listWingColor.Clear();
        //添加背包的和已激活的
        for (int i = 0; i < myWingData.GetWingColorDatas().Count; i++)
        {
            listWingColor.Add(myWingData.GetWingColorDatas()[i]);
        }

        //添加配置中剩下的
        var arr = ItemTableManager.Instance.array.gItem.handles;
        for (int j = 0, max = arr.Length; j < max; ++j)
        {
            var item = arr[j].Value as TABLE.ITEM;
            if (item.type == 5 && item.subType == 9)
            {
                bool isExist = false;
                for (int i = 0; i < listWingColor.Count; i++)
                {
                    if (listWingColor[i].configId == arr[j].key)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    WingColorData wingColorData = new WingColorData();
                    wingColorData.configId = arr[j].key;
                    listWingColor.Add(wingColorData);
                }
            }
        }

        //排序
        listWingColor.Sort((a, b) =>
        {
            if (a.configId != myWingData.wingColorId && b.configId != myWingData.wingColorId)
            {
                if (a.endTime == 0 && b.endTime == 0)
                {
                    if (a.count == 0 && b.count == 0)
                    {
                        return a.configId < b.configId ? -1 : 1; //id小的优先
                    }
                    else //获得优先
                    {
                        return a.count > 0 ? -1 : 1;
                    }
                }
                else //激活优先
                {
                    return a.endTime != 0 ? -1 : 1; //-1永久 >0限时 都算已激活
                }
            }
            else //穿戴优先
            {
                return a.configId == myWingData.wingColorId ? -1 : 1;
            }
        });
    }

    void InitData()
    {
        myWingData = CSWingInfo.Instance.MyWingData;
        if (myWingData == null || ItemTableManager.Instance == null) return;
        SortWingColor();
        if (listWingColor == null || listWingColor.Count == 0) return;
        ShowWingColorInfo(listWingColor[0].configId);
        RefreshGrid();
    }

    private CSBetterLisHot<IntArray> attrInfo = new CSBetterLisHot<IntArray>();
    private RepeatedField<int> ids = new RepeatedField<int>();
    private RepeatedField<int> values = new RepeatedField<int>();

    /// <summary>
    /// 展示单个幻彩信息
    /// </summary>
    /// <param name="id">幻彩的配置Id</param>
    void ShowWingColorInfo(int id)
    {
        TABLE.ITEM wingColor;
        if (!ItemTableManager.Instance.TryGetValue(id, out wingColor)) return;
        mlb_title.text = wingColor.name;
        mlb_title.color = UtilityCsColor.Instance.GetColor(wingColor.quality);

        //属性加成
        SetAttrParaByCareer(attrInfo, CSMainPlayerInfo.Instance.Career);
        CSWingInfo.Instance.SetWingColorAddition(attrInfo, ids, values, CSMainPlayerInfo.Instance.Career);
        RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;
        // if (attrInfo != null && attrInfo.Count == 2)
        // {
        //     attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, attrInfo[0], attrInfo[1]);
        // }

        if (ids.Count > 0 && values.Count > 0)
        {
            attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, ids, values);
        }

        if (attrItems != null)
        {
            mgrid_effects.MaxCount = Mathf.CeilToInt((float) attrItems.Count / 2);
            GameObject gp;
            ScriptBinder gpBinder;
            UILabel lb_name;
            UILabel lb_nextName;
            for (int i = 0; i < mgrid_effects.MaxCount; i++)
            {
                gp = mgrid_effects.controlList[i];
                gpBinder = gp.transform.GetComponent<ScriptBinder>();
                lb_name = gpBinder.GetObject("lb_name") as UILabel;
                lb_nextName = gpBinder.GetObject("lb_nextName") as UILabel;
                CSStringBuilder.Clear();
                lb_name.text = CSStringBuilder
                    .Append(attrItems[2 * i].Key, CSString.Format(999), attrItems[2 * i].Value).ToString();
                if (i == mgrid_effects.MaxCount - 1 && attrItems.Count < 2 * i + 2)
                {
                    lb_nextName.gameObject.SetActive(false);
                }
                else
                {
                    CSStringBuilder.Clear();
                    lb_nextName.text = CSStringBuilder
                        .Append(attrItems[2 * i + 1].Key, CSString.Format(999), attrItems[2 * i + 1].Value).ToString();
                    lb_nextName.gameObject.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// 根据职业获取相应属性id和数值
    /// </summary>
    /// <param name="career"></param>
    void SetAttrParaByCareer(CSBetterLisHot<IntArray> listAttrInfo, int career)
    {
        if (listAttrInfo == null) return;
        listAttrInfo.Clear();
        var mapHuanCai = HuanCaiTableManager.Instance.array.gItem.id2offset;
        if (mapHuanCai == null) return;
        switch (career)
        {
            case 1:
                listAttrInfo.Add((mapHuanCai[listWingColor[selectIndex].configId].Value as TABLE.HUANCAI).zsattrPara);
                listAttrInfo.Add((mapHuanCai[listWingColor[selectIndex].configId].Value as TABLE.HUANCAI).attrNum);
                break;
            case 2:
                listAttrInfo.Add((mapHuanCai[listWingColor[selectIndex].configId].Value as TABLE.HUANCAI).fsattrPara);
                listAttrInfo.Add((mapHuanCai[listWingColor[selectIndex].configId].Value as TABLE.HUANCAI).attrNum);
                break;
            case 3:
                listAttrInfo.Add((mapHuanCai[listWingColor[selectIndex].configId].Value as TABLE.HUANCAI).dsattrPara);
                listAttrInfo.Add((mapHuanCai[listWingColor[selectIndex].configId].Value as TABLE.HUANCAI).attrNum);
                break;
            default: break;
        }
    }


    private Map<UILabel, long> mapShowTime;

    /// <summary>
    /// 展示所有幻彩
    /// </summary>
    void RefreshGrid()
    {
        if (mapShowTime == null)
            mapShowTime = new Map<UILabel, long>();
        mapShowTime.Clear();
        // mgrid_wing_color.Bind<WingColorData, UIWingColorBinder>(listWingColor, mPoolHandleManager);
        mgrid_wing_color.MaxCount = listWingColor.Count;
        GameObject gp;
        ScriptBinder scriptBinder;
        UILabel lb_time;
        for (int i = 0; i < mgrid_wing_color.MaxCount; i++)
        {
            gp = mgrid_wing_color.controlList[i];
            scriptBinder = gp.GetComponent<ScriptBinder>();
            lb_time = scriptBinder.GetObject("lb_time") as UILabel;
            var eventHandle = UIEventListener.Get(gp);
            UIWingColorBinder Binder;
            if (eventHandle.parameter == null)
            {
                Binder = new UIWingColorBinder();
                Binder.Setup(eventHandle);
            }
            else
            {
                Binder = eventHandle.parameter as UIWingColorBinder;
            }

            Binder.index = i;
            Binder.isSelect = i == selectIndex;
            Binder.actionItem = OnClickWingColorItem;
            Binder.actionWear = OnClickWear;
            Binder.actionGet = OnClickGet;
            Binder.actionActive = OnClickActive;
            Binder.actionTakeOff = OnClickTakeOff;
            Binder.actionItemBase = OnClickItemBase;
            WingColorData wingColorData = listWingColor[i];
            Binder.Bind(wingColorData);
            if (wingColorData.endTime > 0)
            {
                long sec = (wingColorData.endTime - CSServerTime.Instance.TotalMillisecond) / 1000;
                if (!mapShowTime.ContainsKey(lb_time))
                {
                    mapShowTime.Add(lb_time, sec);
                }
            }
        }
    }

    void OnSchedule()
    {
        if (mapShowTime != null && mapShowTime.Count > 0)
        {
            for (mapShowTime.Begin(); mapShowTime.Next();)
            {
                long sec = mapShowTime.Value;
                string time = CSServerTime.Instance.FormatLongToTimeStr(sec, 18);
                mapShowTime.Key.text = $"{CSString.Format(720)}{time}";
                if (mapShowTime[mapShowTime.Key] > 0)
                {
                    mapShowTime[mapShowTime.Key]--;
                }
            }
        }
    }

    /// <summary>
    /// 点击获取所有属性加成
    /// </summary>
    /// <param name="go"></param>
    void OnClickAttribute(GameObject go)
    {
        if (go == null) return;
        Map<int, int> mapInfo = new Map<int, int>();
        List<IntArray> listReInfoOne = new List<IntArray>();
        mapInfo.Clear();
        TABLE.HUANCAI tableHuanCai = null;
        for (int i = 0; i < listWingColor.Count; i++)
        {
            listReInfoOne.Clear();
            if (listWingColor[i].endTime > 0 &&
                HuanCaiTableManager.Instance.TryGetValue(listWingColor[i].configId, out tableHuanCai))
            {
                listReInfoOne = GetAttrParaByCareer(listWingColor[i].configId, CSMainPlayerInfo.Instance.Career);
                if (mapInfo.Count == 0)
                {
                    for (int j = 0; j < listReInfoOne[0].Count; j++)
                    {
                        mapInfo.Add(listReInfoOne[0][j], listReInfoOne[1][j]);
                    }
                }
                else
                {
                    for (int j = 0; j < listReInfoOne[0].Count; j++)
                    {
                        if (!mapInfo.ContainsKey(listReInfoOne[0][j]))
                        {
                            mapInfo.Add(listReInfoOne[0][j], listReInfoOne[1][j]);
                        }
                        else
                        {
                            mapInfo[listReInfoOne[0][j]] += listReInfoOne[1][j];
                        }
                    }
                }
            }
        }

        Utility.ShowAllAttributePanel(780, mapInfo);
    }

    /// <summary>
    /// 根据职业获取相应属性id和数值
    /// </summary>
    /// <param name="career"></param>
    List<IntArray> GetAttrParaByCareer(int configId, int career)
    {
        List<IntArray> listAttrInfo = new List<IntArray>();
        var maoHuanCai = HuanCaiTableManager.Instance.array.gItem.id2offset;
        switch (career)
        {
            case 1:
                listAttrInfo.Add((maoHuanCai[configId].Value as TABLE.HUANCAI).zsattrPara);
                listAttrInfo.Add((maoHuanCai[configId].Value as TABLE.HUANCAI).attrNum);
                break;
            case 2:
                listAttrInfo.Add((maoHuanCai[configId].Value as TABLE.HUANCAI).fsattrPara);
                listAttrInfo.Add((maoHuanCai[configId].Value as TABLE.HUANCAI).attrNum);
                break;
            case 3:
                listAttrInfo.Add((maoHuanCai[configId].Value as TABLE.HUANCAI).dsattrPara);
                listAttrInfo.Add((maoHuanCai[configId].Value as TABLE.HUANCAI).attrNum);
                break;
        }

        return listAttrInfo;
    }

    /// <summary>
    /// 点击穿戴
    /// </summary>
    /// <param name="go"></param>
    void OnClickWear(int index)
    {
        Net.CSDressColorWingMessage(listWingColor[index].configId, 1);
        if (index == selectIndex) return;
        lastSelectIndex = selectIndex;
        selectIndex = index;
        // ShowItemInfo(lastSelectIndex);
        // ShowItemInfo(selectIndex);
        RefreshGrid();
        ShowWingColorInfo(listWingColor[index].configId);
        ShowModel(listWingColor[index].configId);
    }

    /// <summary>
    /// 点击获取
    /// </summary>
    /// <param name="go"></param>
    void OnClickGet(int index)
    {
        Utility.ShowGetWay(listWingColor[index].configId);
        if (index == selectIndex) return;
        lastSelectIndex = selectIndex;
        selectIndex = index;
        // ShowItemInfo(lastSelectIndex);
        // ShowItemInfo(selectIndex);
        RefreshGrid();
        ShowWingColorInfo(listWingColor[index].configId);
        ShowModel(listWingColor[index].configId);
    }

    /// <summary>
    /// 点击激活
    /// </summary>
    /// <param name="go"></param>
    void OnClickActive(int index)
    {
        //二次确认框
        UtilityTips.ShowPromptWordTips(18, () =>
        {
            Net.ReqUseItemMessage(listWingColor[index].bagIndex, 1, false, 0, listWingColor[index].id);
            Close();
        });
        if (index == selectIndex) return;
        lastSelectIndex = selectIndex;
        selectIndex = index;
        // ShowItemInfo(lastSelectIndex);
        // ShowItemInfo(selectIndex);
        RefreshGrid();
        ShowWingColorInfo(listWingColor[index].configId);
        ShowModel(listWingColor[index].configId);
    }

    /// <summary>
    /// 点击脱下
    /// </summary>
    /// <param name="go"></param>
    void OnClickTakeOff(int index)
    {
        Net.CSDressColorWingMessage(listWingColor[index].configId, 2);
        if (index == selectIndex) return;
        lastSelectIndex = selectIndex;
        selectIndex = index;
        // ShowItemInfo(lastSelectIndex);
        // ShowItemInfo(selectIndex);
        RefreshGrid();
        ShowWingColorInfo(listWingColor[index].configId);
        ShowModel(listWingColor[index].configId);
    }


    void OnClickItemBase(int index)
    {
        if (index == selectIndex) return;
        lastSelectIndex = selectIndex;
        selectIndex = index;
        // ShowItemInfo(lastSelectIndex);
        // ShowItemInfo(selectIndex);
        RefreshGrid();
        ShowWingColorInfo(listWingColor[index].configId);
        ShowModel(listWingColor[index].configId);
    }


    /// <summary>
    /// 点击幻彩Item
    /// </summary>
    /// <param name="go"></param>
    void OnClickWingColorItem(int index)
    {
        if (index == selectIndex) return;
        lastSelectIndex = selectIndex;
        selectIndex = index;
        // ShowItemInfo(lastSelectIndex);
        // ShowItemInfo(selectIndex);
        RefreshGrid();
        ShowWingColorInfo(listWingColor[index].configId);
        ShowModel(listWingColor[index].configId);
    }

    void ShowModel(int id)
    {
        TABLE.HUANCAI tableHuanCai;
        if (HuanCaiTableManager.Instance.TryGetValue(id, out tableHuanCai))
        {
            CSEffectPlayMgr.Instance.ShowUIEffect(msp_stage_effect_icon.gameObject, tableHuanCai.model.ToString(),
                ResourceType.UIWing, 6);
        }
    }

    public override void OnHide()
    {
        base.OnHide();
        ScriptBinder.StopInvokeRepeating();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(msp_stage_effect_icon.gameObject);
        CSEffectPlayMgr.Instance.Recycle(meffect_wing_idle_add);
        base.OnDestroy();
    }
}