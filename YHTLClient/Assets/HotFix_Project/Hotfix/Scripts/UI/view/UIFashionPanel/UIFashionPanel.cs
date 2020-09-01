using System.Collections.Generic;
using fashion;
using Google.Protobuf.Collections;
using UnityEngine;

/// <summary>
/// 时装类型
/// </summary>
public enum TypeFashion
{
    Clothes = 1, //衣服
    Weapon, //武器
    Title, //称号
    FashionSet, //套装
}

public partial class UIFashionPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }
    
    private readonly Vector4 scrollViewPos_other = new Vector4(92, -82, 270, 180);
    private readonly Vector4 scrollViewPos_showTitle = new Vector4(92, -55, 270, 250);
    

    TypeFashion curTabType = TypeFashion.Clothes; //页签
    private AllFashionInfo myAllFashionInfo;

    /// <summary>
    /// 排序后的时装(衣服和套装)信息(包括未获得的)
    /// </summary>
    ILBetterList<FashionItemData> sortedFashions = new ILBetterList<FashionItemData>();

    private int selectIndexFashion = 0; //当前选择的时装
    private int lastSelectIndexFashion = 0; //上一个选择的时装

    /// <summary>
    /// 排序后的所有武器信息（包括未获得的）
    /// </summary>
    ILBetterList<FashionItemData> sortedWeapon = new ILBetterList<FashionItemData>();

    private int selectIndexWeapon = 0; //当前选择的武器
    private int lastSelectIndexWeapon = 0; //上一个选择的武器

    /// <summary>
    /// 排序后的所有称号信息（包括未获得的）
    /// </summary>
    ILBetterList<FashionItemData> sortedTitles = new ILBetterList<FashionItemData>();

    private int selectIndexTitle = 0; //当前选择的称号
    private int lastSelectIndexTitle = 0; //上一个选择的称号

    private bool isFisrt = true;

    string colorTitle;
    string red;
    string green;

    /// <summary>
    /// 当前选中的时装信息
    /// </summary>
    private FashionItemData selectFashionData;

    //我的职业
    private int myCareer = CSMainPlayerInfo.Instance.Career;

    private Map<TypeFashion, Map<TypeFashion, int>> mapFashionModel;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.AllFashionInfo, RefreshData);
        mClientEvent.Reg((uint) CEvent.EquipFashion, EquipFashion);
        mClientEvent.Reg((uint) CEvent.FashionStarLevelUp, ChangeFashion);
        mClientEvent.Reg((uint) CEvent.AddFashion, ChangeFashion);
        mClientEvent.Reg((uint) CEvent.RemoveFashion, ChangeFashion);
        mClientEvent.Reg((uint) CEvent.UnEquipFashion, ChangeFashion);
        mClientEvent.Reg((uint) CEvent.ItemListChange, RefreshDataItemChange);
        mClientEvent.Reg((uint) CEvent.MoneyChange, RefreshDataItemChange);

        UIEventListener.Get(mbtn_clothes.gameObject, TypeFashion.Clothes).onClick = OnClickTab;
        UIEventListener.Get(mbtn_weapon.gameObject, TypeFashion.Weapon).onClick = OnClickTab;
        UIEventListener.Get(mbtn_title.gameObject, TypeFashion.Title).onClick = OnClickTab;
        mbtn_rising_star.onClick = OnClickRisingStar;
        mbtn_wear.onClick = OnClickWear;
        mbtn_unload.onClick = OnClickUnLoad;
        mbtn_access.onClick = OnClickAccess;
        mbtn_all_attribute.onClick = OnClickAllAttribute;
        mbtn_active.onClick = OnClickActive;
        mbtn_rule.onClick = OnClickRule;
        
        mbar.onChange.Add(new EventDelegate(OnChange));
        
        colorTitle = UtilityColor.GetColorString(ColorType.SecondaryText);
        red = UtilityColor.GetColorString(ColorType.Red);
        green = UtilityColor.GetColorString(ColorType.Green);
    }
    
    void OnChange()
    {
        marrow.SetActive(mbar.value < 0.95);
    }

    public override void SelectChildPanel(int type = 1)
    {
        myAllFashionInfo = CSFashionInfo.Instance.MyAllFashionInfo;
        sortedFashions = CSFashionInfo.Instance.SortedFashions;
        sortedWeapon = CSFashionInfo.Instance.SortedWeapons;
        sortedTitles = CSFashionInfo.Instance.SortedTitles;
        SetTabPoint();
        curTabType = (TypeFashion) type;
        switch (curTabType)
        {
            case TypeFashion.Clothes:
                mbtn_clothes.GetComponent<UIToggle>().Set(true);
                ShowFashionTab();
                break;
            case TypeFashion.Weapon:
                mbtn_weapon.GetComponent<UIToggle>().Set(true);
                ShowWeaponTab();
                break;
            case TypeFashion.Title:
                mbtn_title.GetComponent<UIToggle>().Set(true);
                ShowTitleTab();
                break;
            default:
                break;
        }
    }

    public override void Show()
    {
        base.Show();
        CSFashionInfo.Instance.SortFashionForRedPoint();
        InitData();
    }

    /// <summary>
    /// 道具和金币变化
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void RefreshDataItemChange(uint id, object data)
    {
        if (data == null) return;
        RefreshData(false);
    }

    void RefreshData(uint id, object data)
    {
        if (data == null) return;
        InitData();
    }

    void ChangeFashion(uint id, object data)
    {
        if (data == null) return;
        RefreshData();
    }
    
    void EquipFashion(uint id, object data)
    {
        if (data == null) return;
        RefreshData();
        UtilityTips.ShowGreenTips(664, FashionTableManager.Instance.GetFashionName(selectFashionData.Id));
    }

    void RefreshData(bool isInit = true)
    {
        myAllFashionInfo = CSFashionInfo.Instance.MyAllFashionInfo;
        sortedFashions = CSFashionInfo.Instance.SortedFashions;
        sortedWeapon = CSFashionInfo.Instance.SortedWeapons;
        sortedTitles = CSFashionInfo.Instance.SortedTitles;
        SetTabPoint();
        switch (curTabType)
        {
            case TypeFashion.Clothes:
            case TypeFashion.FashionSet:
                if (isInit)
                {
                    selectIndexFashion = 0;
                    lastSelectIndexFashion = 0;
                }

                ShowFashionTab();
                break;
            case TypeFashion.Weapon:
                if (isInit)
                {
                    selectIndexWeapon = 0;
                    lastSelectIndexWeapon = 0;
                }

                ShowWeaponTab();
                break;
            case TypeFashion.Title:
                if (isInit)
                {
                    selectIndexTitle = 0;
                    lastSelectIndexTitle = 0;
                }

                ShowTitleTab();
                break;
        }
    }

    void InitData()
    {
        isFisrt = true;
        selectIndexFashion = 0;
        lastSelectIndexFashion = 0;
        selectIndexWeapon = 0;
        lastSelectIndexWeapon = 0;
        selectIndexTitle = 0;
        lastSelectIndexTitle = 0;
        mbtn_clothes.gameObject.GetComponent<UIToggle>().Set(true);
        myAllFashionInfo = CSFashionInfo.Instance.MyAllFashionInfo;
        sortedFashions = CSFashionInfo.Instance.SortedFashions;
        sortedWeapon = CSFashionInfo.Instance.SortedWeapons;
        sortedTitles = CSFashionInfo.Instance.SortedTitles;
        SetGridbtns();
        SetTabPoint();
        ShowFashionTab();
    }

    void SetGridbtns()
    {
        mbtn_clothes.gameObject.SetActive(sortedFashions.Count>0);
        mbtn_weapon.gameObject.SetActive(sortedWeapon.Count>0);
        mbtn_title.gameObject.SetActive(sortedTitles.Count>0);
        mgrid_btns.repositionNow = true;
        mgrid_btns.Reposition();
    }

    void SetTabPoint()
    {
        mredpoint_fashion.SetActive(CSFashionInfo.Instance.HasActiveAndUpStarForFashion());
        mredpoint_weapon.SetActive(CSFashionInfo.Instance.HasActiveAndUpStarForWeapon());
        mredpoint_title.SetActive(CSFashionInfo.Instance.HasActiveAndUpStarForTitle());
    }

    void OnClickTab(GameObject go)
    {
        if (go == null) return;
        TypeFashion type = (TypeFashion) UIEventListener.Get(go).parameter;
        curTabType = type;
        switch (type)
        {
            case TypeFashion.Clothes:
                ShowFashionTab();
                break;
            case TypeFashion.Weapon:
                ShowWeaponTab();
                break;
            case TypeFashion.Title:
                ShowTitleTab();
                break;
            default:
                break;
        }
    }


    List<UIItemBase> listFashionItemBases = new List<UIItemBase>();

    /// <summary>
    /// 时装(衣服和套装)
    /// </summary>
    void ShowFashionTab()
    {
        mScrollView_fashion.SetActive(true);
        mScrollView_weapon.SetActive(false);
        mScrollView_title.SetActive(false);
        if (sortedFashions.Count <= 0)
        {
            // Debug.Log("时装配置表没有衣服----------@吕惠铭");
            return;
        }

        mgrid_fashion.MaxCount = sortedFashions.Count;
        GameObject gp;
        ScriptBinder gpBinder;
        // GameObject itemBase;
        UILabel lb_name;
        UILabel lb_time;
        UILabel lb_combine;
        GameObject obj_using;
        GameObject obj_preview;
        GameObject effect;
        GameObject redpoint;
        GameObject item;
        UIItemBase uiItemBase;
        // FashionCombineData fashionCombineData;
        for (int i = 0; i < mgrid_fashion.MaxCount; i++)
        {
            gp = mgrid_fashion.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            // itemBase = gpBinder.GetObject("ItemBase") as GameObject;
            lb_name = gpBinder.GetObject("lb_name") as UILabel;
            lb_time = gpBinder.GetObject("lb_time") as UILabel;
            lb_combine = gpBinder.GetObject("lb_combine") as UILabel;
            obj_using = gpBinder.GetObject("obj_using") as GameObject;
            obj_preview = gpBinder.GetObject("obj_preview") as GameObject;
            effect = gpBinder.GetObject("effect") as GameObject;
            redpoint = gpBinder.GetObject("redpoint") as GameObject;
            item = gpBinder.GetObject("item") as GameObject;


            lb_name.text = FashionTableManager.Instance.GetFashionName(sortedFashions[i].Id);
            lb_name.color =
                UtilityCsColor.Instance.GetColor(FashionTableManager.Instance.GetFashionQuality(sortedFashions[i].Id));
            if (sortedFashions[i].IsActive)
            {
                if (sortedFashions[i].TimeLimit == 0)
                {
                    lb_time.text = CSString.Format(658);
                }
                else if (sortedFashions[i].TimeLimit > 0)
                {
                    int hours = (int) Mathf.Ceil(((float) (sortedFashions[i].TimeLimit -
                                                           CSServerTime.Instance.TotalMillisecond) / 3600000));
                    lb_time.text = CSString.Format(CSString.Format(657), hours);
                }

                // redpoint.SetActive(sortedFashions[i].IsUpStar);
                redpoint.SetActive(false);
            }
            else
            {
                //未激活显示碎片个数
                // fashionCombineData = CSFashionInfo.Instance.GetFashionAcitveData(sortedFashions[i].id);
                string color = sortedFashions[i].IsCanBeActivated ? green : red;
                CSStringBuilder.Clear();
                lb_combine.text = CSStringBuilder.Append(colorTitle,
                    ItemTableManager.Instance.GetItemName(sortedFashions[i].CombineEquipId),
                    CSString.Format(999), "[-]", color, sortedFashions[i].OwnedCombineNum, '/',
                    sortedFashions[i].CombineNum, "[-]").ToString();

                redpoint.SetActive(sortedFashions[i].IsCanBeActivated);
            }

            lb_time.gameObject.SetActive(sortedFashions[i].IsActive);
            lb_combine.gameObject.SetActive(!sortedFashions[i].IsActive);
            obj_using.SetActive(sortedFashions[i].IsEquip);
            effect.SetActive(i == selectIndexFashion);
            obj_preview.SetActive(i == selectIndexFashion && !sortedFashions[i].IsEquip);

            if (listFashionItemBases.Count <= i)
                listFashionItemBases.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, item.transform));
            uiItemBase = listFashionItemBases[i];
            uiItemBase.ShowIconFashion(sortedFashions[i].Id);
            UIEventListener.Get(gp, i).onClick = OnClickClothes;
        }


        if (sortedFashions.Count > 0)
        {
            ShowModel(sortedFashions[selectIndexFashion], selectFashionData);
            selectFashionData = sortedFashions[selectIndexFashion];
        }

        ShowItemInfo(TypeFashion.Clothes, selectIndexFashion);
    }


    /// <summary>
    /// 点击某个时装item
    /// </summary>
    /// <param name="go"></param>
    void OnClickClothes(GameObject go)
    {
        if (go == null) return;
        int index = (int) UIEventListener.Get(go).parameter;
        if (index == selectIndexFashion) return; //如果点的是同一个不处理
        lastSelectIndexFashion = selectIndexFashion;
        selectIndexFashion = index;

        GameObject gp;
        ScriptBinder gpBinder;
        GameObject effect;
        GameObject obj_preview;
        gp = mgrid_fashion.controlList[lastSelectIndexFashion];
        gpBinder = gp.transform.GetComponent<ScriptBinder>();
        effect = gpBinder.GetObject("effect") as GameObject;
        effect.SetActive(false);
        obj_preview = gpBinder.GetObject("obj_preview") as GameObject;
        obj_preview.SetActive(false);
        gp = mgrid_fashion.controlList[index];
        gpBinder = gp.transform.GetComponent<ScriptBinder>();
        effect = gpBinder.GetObject("effect") as GameObject;
        effect.SetActive(true);
        obj_preview = gpBinder.GetObject("obj_preview") as GameObject;
        obj_preview.SetActive(!sortedFashions[index].IsEquip);

        ShowModel(sortedFashions[index], selectFashionData);
        selectFashionData = sortedFashions[selectIndexFashion];
        ShowItemInfo(TypeFashion.Clothes, selectIndexFashion);
    }


    List<UIItemBase> listWeaponItemBases = new List<UIItemBase>();

    /// <summary>
    /// 幻武
    /// </summary>
    void ShowWeaponTab()
    {
        mScrollView_fashion.SetActive(false);
        mScrollView_weapon.SetActive(true);
        mScrollView_title.SetActive(false);
        if (sortedWeapon.Count <= 0)
        {
            // Debug.Log("时装配置表没有幻武----------@吕惠铭");
            return;
        }

        mgrid_weapon.MaxCount = sortedWeapon.Count;
        GameObject gp;
        ScriptBinder gpBinder;
        // GameObject itemBase;
        UILabel lb_name;
        UILabel lb_time;
        UILabel lb_combine;
        GameObject obj_using;
        GameObject obj_preview;
        GameObject effect;
        // FashionCombineData fashionCombineData;
        GameObject redpoint;
        GameObject item;
        UIItemBase uiItemBase;
        for (int i = 0; i < mgrid_weapon.MaxCount; i++)
        {
            gp = mgrid_weapon.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            // itemBase = gpBinder.GetObject("ItemBase") as GameObject;
            lb_name = gpBinder.GetObject("lb_name") as UILabel;
            lb_time = gpBinder.GetObject("lb_time") as UILabel;
            lb_combine = gpBinder.GetObject("lb_combine") as UILabel;
            obj_using = gpBinder.GetObject("obj_using") as GameObject;
            obj_preview = gpBinder.GetObject("obj_preview") as GameObject;
            effect = gpBinder.GetObject("effect") as GameObject;
            redpoint = gpBinder.GetObject("redpoint") as GameObject;
            item = gpBinder.GetObject("item") as GameObject;

            // lb_name.text = configFashionMap[sortedWeapon[i].id].name;
            lb_name.text = FashionTableManager.Instance.GetFashionName(sortedWeapon[i].Id);
            lb_name.color =
                UtilityCsColor.Instance.GetColor(FashionTableManager.Instance.GetFashionQuality(sortedWeapon[i].Id));
            if (sortedWeapon[i].IsActive)
            {
                if (sortedWeapon[i].TimeLimit == 0)
                {
                    lb_time.text = CSString.Format(658);
                }
                else if (sortedWeapon[i].TimeLimit > 0)
                {
                    int hours = (int) Mathf.Ceil(((float) (sortedWeapon[i].TimeLimit -
                                                           CSServerTime.Instance.TotalMillisecond) / 3600000));
                    lb_time.text = CSString.Format(657, hours);
                }

                // redpoint.SetActive(sortedWeapon[i].IsUpStar);
                redpoint.SetActive(false);
            }
            else
            {
                //未激活显示碎片个数
                // fashionCombineData = CSFashionInfo.Instance.GetFashionAcitveData(sortedWeapon[i].id);
                string color = sortedWeapon[i].IsCanBeActivated ? green : red;
                CSStringBuilder.Clear();
                lb_combine.text = CSStringBuilder.Append(colorTitle,
                        ItemTableManager.Instance.GetItemName(sortedWeapon[i].CombineEquipId),
                        CSString.Format(999), "[-]", color, sortedWeapon[i].OwnedCombineNum, '/',
                        sortedWeapon[i].CombineNum, "[-]")
                    .ToString();

                redpoint.SetActive(sortedWeapon[i].IsCanBeActivated);
            }

            lb_time.gameObject.SetActive(sortedWeapon[i].IsActive);
            lb_combine.gameObject.SetActive(!sortedWeapon[i].IsActive);
            obj_using.SetActive(sortedWeapon[i].IsEquip);
            effect.SetActive(i == selectIndexWeapon);
            obj_preview.SetActive(i == selectIndexWeapon && !sortedWeapon[i].IsEquip);
            if (listWeaponItemBases.Count <= i)
                listWeaponItemBases.Add(UIItemManager.Instance.GetItem(PropItemType.Normal, item.transform));
            uiItemBase = listWeaponItemBases[i];
            uiItemBase.ShowIconFashion(sortedWeapon[i].Id);
            UIEventListener.Get(gp, i).onClick = OnClickWeapon;
        }

        if (sortedWeapon.Count > 0)
        {
            ShowModel(sortedWeapon[selectIndexWeapon], selectFashionData);
            selectFashionData = sortedWeapon[selectIndexWeapon];
        }

        ShowItemInfo(TypeFashion.Weapon, selectIndexWeapon);
    }


    /// <summary>
    /// 点击某个幻武item
    /// </summary>
    void OnClickWeapon(GameObject go)
    {
        if (go == null) return;
        int index = (int) UIEventListener.Get(go).parameter;
        if (index == selectIndexWeapon) return; //如果点的是同一个不处理
        lastSelectIndexWeapon = selectIndexWeapon;
        selectIndexWeapon = index;

        GameObject gp;
        ScriptBinder gpBinder;
        GameObject effect;
        GameObject obj_preview;
        gp = mgrid_weapon.controlList[lastSelectIndexWeapon];
        gpBinder = gp.transform.GetComponent<ScriptBinder>();
        effect = gpBinder.GetObject("effect") as GameObject;
        effect.SetActive(false);
        obj_preview = gpBinder.GetObject("obj_preview") as GameObject;
        obj_preview.SetActive(false);
        gp = mgrid_weapon.controlList[index];
        gpBinder = gp.transform.GetComponent<ScriptBinder>();
        effect = gpBinder.GetObject("effect") as GameObject;
        effect.SetActive(true);
        obj_preview = gpBinder.GetObject("obj_preview") as GameObject;
        obj_preview.SetActive(!sortedWeapon[index].IsEquip);

        ShowModel(sortedWeapon[index], selectFashionData);
        selectFashionData = sortedWeapon[selectIndexWeapon];
        ShowItemInfo(TypeFashion.Weapon, selectIndexWeapon);
    }

    /// <summary>
    /// 显示称号
    /// </summary>
    void ShowTitleTab()
    {
        mScrollView_fashion.SetActive(false);
        mScrollView_weapon.SetActive(false);
        mScrollView_title.SetActive(true);
        if (sortedTitles.Count <= 0)
        {
            // Debug.Log("时装配置表没有称号----------@吕惠铭");
            return;
        }

        mgrid_title.MaxCount = sortedTitles.Count;
        GameObject gp;
        ScriptBinder gpBinder;
        GameObject obj_using;
        GameObject effect;
        UISprite sp_icon;
        GameObject obj_preview;
        GameObject redpoint;
        GameObject effect_icon;
        // FashionCombineData fashionCombineData;
        for (int i = 0; i < mgrid_title.MaxCount; i++)
        {
            gp = mgrid_title.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            obj_using = gpBinder.GetObject("obj_using") as GameObject;
            effect = gpBinder.GetObject("effect") as GameObject;
            sp_icon = gpBinder.GetObject("sp_icon") as UISprite;
            obj_preview = gpBinder.GetObject("obj_preview") as GameObject;
            redpoint = gpBinder.GetObject("redpoint") as GameObject;
            effect_icon = gpBinder.GetObject("effect_icon") as GameObject;
            obj_using.SetActive(sortedTitles[i].IsEquip);
            effect.SetActive(i == selectIndexTitle);
            obj_preview.SetActive(i == selectIndexTitle && !sortedTitles[i].IsEquip);
            //加载称号
            if (sortedTitles[i].IsActive)
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(effect_icon,
                    FashionTableManager.Instance.GetFashionTitleModel(sortedTitles[i].Id).ToString(), ResourceType.UIEffect);
                msp_title.SetActive(true);
                effect_icon.SetActive(true);
                sp_icon.gameObject.SetActive(false);
                // redpoint.SetActive(sortedTitles[i].IsUpStar);
                redpoint.SetActive(false);
            }
            else
            {
                // CSEffectPlayMgr.Instance.Recycle(effect_icon);
                effect_icon.SetActive(false);
                sp_icon.spriteName = FashionTableManager.Instance.GetFashionTitleModel(sortedTitles[i].Id).ToString();
                sp_icon.color = Color.black;
                sp_icon.gameObject.SetActive(true);
                redpoint.SetActive(sortedTitles[i].IsCanBeActivated);
            }

            UIEventListener.Get(gp, i).onClick = OnClickTitle;
        }

        if (sortedTitles.Count > 0)
        {
            ShowTitleSprite(sortedTitles[selectIndexTitle]);
            selectFashionData = sortedTitles[selectIndexTitle];
        }

        ShowItemInfo(TypeFashion.Title, selectIndexTitle);
    }

    /// <summary>
    /// 点击某个称号item
    /// </summary>
    void OnClickTitle(GameObject go)
    {
        if (go == null) return;
        int index = (int) UIEventListener.Get(go).parameter;
        if (index == selectIndexTitle) return; //如果点的是同一个不处理
        lastSelectIndexTitle = selectIndexTitle;
        selectIndexTitle = index;

        GameObject gp;
        ScriptBinder gpBinder;
        GameObject effect;
        GameObject obj_preview;
        gp = mgrid_title.controlList[lastSelectIndexTitle];
        gpBinder = gp.transform.GetComponent<ScriptBinder>();
        effect = gpBinder.GetObject("effect") as GameObject;
        effect.SetActive(false);
        obj_preview = gpBinder.GetObject("obj_preview") as GameObject;
        obj_preview.SetActive(false);
        gp = mgrid_title.controlList[index];
        gpBinder = gp.transform.GetComponent<ScriptBinder>();
        effect = gpBinder.GetObject("effect") as GameObject;
        effect.SetActive(true);
        obj_preview = gpBinder.GetObject("obj_preview") as GameObject;
        obj_preview.SetActive(!sortedTitles[index].IsEquip);

        ShowTitleSprite(sortedTitles[selectIndexTitle]);
        selectFashionData = sortedTitles[selectIndexTitle];
        ShowItemInfo(TypeFashion.Title, selectIndexTitle);
    }


    private UIItemBase itemBaseNeed;

    /// <summary>
    /// 显示当前选中的各项信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="index"></param>
    void ShowItemInfo(TypeFashion type, int index)
    {
        mScrollView_attr.ResetPosition();
        // mScrollView_attr.transform.localPosition = new Vector3(297f, type == TypeFashion.Title ? 130f : 70f, 0f);
        mScrollView_attr.panel.baseClipRegion = type == TypeFashion.Title ? scrollViewPos_showTitle :scrollViewPos_other;
        mgrid_star.gameObject.SetActive(type != TypeFashion.Title);
        mlb_upAttribute.text = type != TypeFashion.Title ? CSString.Format(1849) : CSString.Format(1850);
        ILBetterList<FashionItemData> tempList = null;
        // FashionCombineData fashionCombineData = new FashionCombineData();
        FashionItemData fashionCombineData = null;
        switch (type)
        {
            case TypeFashion.Clothes:
                tempList = sortedFashions;
                fashionCombineData = sortedFashions[index];
                break;
            case TypeFashion.Weapon:
                tempList = sortedWeapon;
                fashionCombineData = sortedWeapon[index];
                break;
            case TypeFashion.Title:
                tempList = sortedTitles;
                fashionCombineData = sortedTitles[index];
                break;
        }

        if (tempList != null)
        {
            List<List<int>> listArribute = null;
            //升星按钮（只有激活的永久装备才显示）
            mbtn_rising_star.gameObject.SetActive(fashionCombineData.IsActive && fashionCombineData.TimeLimit == 0 &&
                                                  type != TypeFashion.Title);
            //升星红点
            if (mbtn_rising_star.gameObject.activeSelf)
                mredpoint_rising_star.SetActive(fashionCombineData.IsUpStar);
            
            mbtn_wear.gameObject.SetActive(fashionCombineData.IsActive && !fashionCombineData.IsEquip);
            mlb_wear_hint.gameObject.SetActive(mbtn_wear.gameObject.activeSelf);
            if (mlb_wear_hint.gameObject.activeSelf)
            {
                switch (type)
                {
                    case TypeFashion.Clothes:
                        mlb_wear_hint.text = CSString.Format(1872);
                        break;
                    case TypeFashion.Weapon:
                        mlb_wear_hint.text = CSString.Format(1873);
                        break;
                    case TypeFashion.Title:
                        mlb_wear_hint.text = CSString.Format(1874);
                        break;
                }    
            }
            mbtn_unload.gameObject.SetActive(fashionCombineData.IsActive && fashionCombineData.IsEquip);
            //获取按钮(只有未激活且碎片不足时才有)
            mbtn_access.gameObject.SetActive(!fashionCombineData.IsActive && !fashionCombineData.IsCanBeActivated);
            mlb_tips.SetActive(type == TypeFashion.Title && mbtn_access.gameObject.activeSelf);
            //激活按钮 (只有未激活时装且碎片足够时才有）
            mbtn_active.gameObject.SetActive(!fashionCombineData.IsActive && fashionCombineData.IsCanBeActivated);
            //获取和激活时显示的碎片(未激活时显示)
            mItemBase.SetActive(!fashionCombineData.IsActive && type != TypeFashion.Title);
            if (mItemBase.activeSelf)
            {
                if (itemBaseNeed == null)
                    itemBaseNeed = UIItemManager.Instance.GetItem(PropItemType.Normal, mItemBase.transform);
                itemBaseNeed.Refresh(fashionCombineData.CombineEquipId);
                //名字
                mlb_itemName.text = ItemTableManager.Instance.GetItemName(fashionCombineData.CombineEquipId);
                mlb_itemName.color =
                    UtilityCsColor.Instance.GetColor(
                        ItemTableManager.Instance.GetItemQuality(fashionCombineData.CombineEquipId));
                //数量
                itemBaseNeed.SetCount(fashionCombineData.OwnedCombineNum, fashionCombineData.CombineNum);
            }

            TABLE.FASHION fashion;
            if (!FashionTableManager.Instance.TryGetValue(fashionCombineData.Id, out fashion))
                return;

            //属性
            switch (myCareer)
            {
                case 1:
                    listArribute = UtilityMainMath.SplitStringToIntLists(fashion.phy);
                    break;
                case 2:
                    listArribute = UtilityMainMath.SplitStringToIntLists(fashion.magic);
                    break;
                case 3:
                    listArribute = UtilityMainMath.SplitStringToIntLists(fashion.tao);
                    break;
            }

            List<List<int>> listConversion = new List<List<int>>();
            listConversion.Clear();
            for (int i = 0; i < listArribute.Count; i++)
            {
                if (listArribute[i].Count == 2)
                {
                    List<int> temp = new List<int>();
                    temp.Add(listArribute[i][0]);
                    temp.Add(listArribute[i][1]);
                    listConversion.Add(temp);
                }
            }

            RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;
            if (listConversion != null)
            {
                attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, listConversion);
            }

            if (attrItems != null)
            {
                mgrid_attribute.MaxCount = attrItems.Count;
                GameObject gp;
                UILabel lb_attribute;
                for (int i = 0; i < mgrid_attribute.MaxCount; i++)
                {
                    gp = mgrid_attribute.controlList[i];
                    lb_attribute = gp.transform.Find("lb_attribute").gameObject.GetComponent<UILabel>();
                    CSStringBuilder.Clear();
                    lb_attribute.text = CSStringBuilder.Append("[cbb694]", attrItems[i].Key,
                        CSString.Format(999), "[-][DCD5B8]", attrItems[i].Value, "[-]").ToString();
                }
                
                marrow.SetActive(mbar.value < 0.95 && mScrollView_attr.panel.height < mgrid_attribute.MaxCount*mgrid_attribute.CellHeight);
            }

            //星级(一直在的 未激活按一星显示 激活的永久和非永久的都按实服务器给的数据显示)
            if (type != TypeFashion.Title)
            {
                mgrid_star.MaxCount = 7;
                GameObject gpStar;
                GameObject obj_star;
                if (fashionCombineData.IsActive)
                {
                    //激活显示当前星级

                    int starNum = 0;
                    if (fashionCombineData.Star <= 7)
                    {
                        starNum = fashionCombineData.Star;
                    }
                    else
                    {
                        starNum = fashionCombineData.Star - 7;
                    }

                    for (int i = 0; i < mgrid_star.controlList.Count; i++)
                    {
                        gpStar = mgrid_star.controlList[i];
                        obj_star = gpStar.transform.Find("star").gameObject;
                        obj_star.SetActive(i < starNum);
                    }
                }
                else
                {
                    //未激活只显示一星
                    for (int i = 0; i < mgrid_star.controlList.Count; i++)
                    {
                        gpStar = mgrid_star.controlList[i];
                        obj_star = gpStar.transform.Find("star").gameObject;
                        obj_star.SetActive(i == 0);
                    }
                }
            }
        }
    }


    /// <summary>
    /// 显示模型(type1,2和4互斥)
    /// </summary>
    /// <param name="fashionItemData">当前选中时装信息</param>
    /// <param name="lastFashionItemData">上一个时装选中信息</param>
    void ShowModel(FashionItemData fashionItemData, FashionItemData lastFashionItemData)
    {
        if (fashionItemData == null) return;
        mapFashionModel = CSFashionInfo.Instance.GetMapFashionModel();

        //首次进入
        if (isFisrt)
        {
            //称号隐藏
            ShowTitleSprite(null);
            bool isHasClothes = false;
            if (mapFashionModel.ContainsKey(TypeFashion.Clothes))
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(mRole,
                    mapFashionModel[TypeFashion.Clothes][TypeFashion.Clothes].ToString(), ResourceType.UIPlayer);
                isHasClothes = true;
            }

            if (mapFashionModel.ContainsKey(TypeFashion.Weapon))
            {
                CSEffectPlayMgr.Instance.ShowUIEffect(mWeapon,
                    mapFashionModel[TypeFashion.Weapon][TypeFashion.Weapon].ToString(), ResourceType.UIWeapon);
            }

            if (mapFashionModel.ContainsKey(TypeFashion.Title))
            {
                //加载称号
                CSEffectPlayMgr.Instance.ShowUIEffect(msp_title,
                    mapFashionModel[TypeFashion.Title][TypeFashion.Title].ToString(), ResourceType.UIEffect);
                msp_title.SetActive(true);
            }

            //套装
            if (mapFashionModel.ContainsKey(TypeFashion.FashionSet))
            {
                Map<TypeFashion, int> tempMap = mapFashionModel[TypeFashion.FashionSet];
                if (tempMap.ContainsKey(TypeFashion.Clothes))
                {
                    CSEffectPlayMgr.Instance.ShowUIEffect(mRole,
                        tempMap[TypeFashion.Clothes].ToString(), ResourceType.UIPlayer);
                    isHasClothes = true;
                }

                if (tempMap.ContainsKey(TypeFashion.Weapon))
                {
                    CSEffectPlayMgr.Instance.ShowUIEffect(mWeapon,
                        tempMap[TypeFashion.Weapon].ToString(), ResourceType.UIWeapon);
                }

                if (tempMap.ContainsKey(TypeFashion.Title))
                {
                    //加载称号
                    CSEffectPlayMgr.Instance.ShowUIEffect(msp_title,
                        mapFashionModel[TypeFashion.Title][TypeFashion.Title].ToString(), ResourceType.UIEffect);
                    msp_title.SetActive(true);
                }
            }

            //如果默认选中的第一个时装未装备,默认显示第一件选中的时装
            FashionItemData firstFashionItemData = null;
            switch (curTabType)
            {
                case TypeFashion.Clothes:
                    firstFashionItemData = sortedFashions[0];
                    break;
                case TypeFashion.Weapon:
                    firstFashionItemData = sortedWeapon[0];
                    break;
                case TypeFashion.Title:
                    firstFashionItemData = sortedTitles[0];
                    break;
            }

            if (firstFashionItemData != null)
            {
                if (!firstFashionItemData.IsEquip)
                {
                    if (FashionTableManager.Instance.GetFashionType(firstFashionItemData.Id) == 1)
                    {
                        CSEffectPlayMgr.Instance.ShowUIEffect(mRole,
                            FashionTableManager.Instance.GetFashionClothesModel(firstFashionItemData.Id).ToString(),
                            ResourceType.UIPlayer);
                        isHasClothes = true;
                    }
                    else if (FashionTableManager.Instance.GetFashionType(firstFashionItemData.Id) == 4)
                    {
                        if (FashionTableManager.Instance.GetFashionClothesModel(firstFashionItemData.Id) > 0)
                        {
                            CSEffectPlayMgr.Instance.ShowUIEffect(mRole,
                                FashionTableManager.Instance.GetFashionClothesModel(firstFashionItemData.Id).ToString(),
                                ResourceType.UIPlayer);
                            isHasClothes = true;
                        }

                        if (FashionTableManager.Instance.GetFashionWeaponryModel(firstFashionItemData.Id) > 0)
                        {
                            CSEffectPlayMgr.Instance.ShowUIEffect(mWeapon,
                                FashionTableManager.Instance.GetFashionWeaponryModel(firstFashionItemData.Id)
                                    .ToString(),
                                ResourceType.UIWeapon);
                        }

                        if (FashionTableManager.Instance.GetFashionTitleModel(firstFashionItemData.Id) > 0)
                        {
                            ShowTitleSprite(firstFashionItemData);
                        }
                    }
                }

                //如果当前没有衣服类型的装备,则装备裸模
                if (!isHasClothes)
                {
                    AvatarModelHelper.LoadAvatarModel(mRole, 0, 0, CSMainPlayerInfo.Instance.Sex,
                        AvatarModelType.AMT_Cloth);
                }

                isFisrt = false;
                return;
            }
        }

        //之后通过点击预览刷新
        TABLE.FASHION fashion;
        if (!FashionTableManager.Instance.TryGetValue(fashionItemData.Id, out fashion))
            return;

        switch ((TypeFashion) fashion.type)
        {
            case TypeFashion.Clothes:
                if (FashionTableManager.Instance.GetFashionType(lastFashionItemData.Id) == 4)
                {
                    // CSEffectPlayMgr.Instance.Recycle(mRole);
                    // CSEffectPlayMgr.Instance.Recycle(mWeapon);
                    mWeapon.SetActive(false);
                }

                CSEffectPlayMgr.Instance.ShowUIEffect(mRole,
                    fashion.clothesModel.ToString(), ResourceType.UIPlayer);
                break;
            case TypeFashion.Weapon:
                if (FashionTableManager.Instance.GetFashionType(lastFashionItemData.Id) == 4)
                {
                    // CSEffectPlayMgr.Instance.Recycle(mRole);
                    // CSEffectPlayMgr.Instance.Recycle(mWeapon);
                    AvatarModelHelper.LoadAvatarModel(mRole, 0, 0, CSMainPlayerInfo.Instance.Sex,
                        AvatarModelType.AMT_Cloth);
                }

                CSEffectPlayMgr.Instance.ShowUIEffect(mWeapon,
                    fashion.weaponryModel.ToString(), ResourceType.UIWeapon);
                mWeapon.SetActive(true);
                break;
            case TypeFashion.Title:
                ShowTitleSprite(fashionItemData);
                break;
            case TypeFashion.FashionSet:
                // CSEffectPlayMgr.Instance.Recycle(mRole);
                // CSEffectPlayMgr.Instance.Recycle(mWeapon);

                if (fashion.clothesModel > 0)
                {
                    CSEffectPlayMgr.Instance.ShowUIEffect(mRole,
                        fashion.clothesModel.ToString(), ResourceType.UIPlayer);
                }
                else
                {
                    AvatarModelHelper.LoadAvatarModel(mRole, 0, 0, CSMainPlayerInfo.Instance.Sex,
                        AvatarModelType.AMT_Cloth);
                }

                if (fashion.weaponryModel > 0)
                {
                    CSEffectPlayMgr.Instance.ShowUIEffect(mWeapon,
                        fashion.weaponryModel.ToString(), ResourceType.UIWeapon);
                }

                mWeapon.SetActive(fashion.weaponryModel > 0);

                if (fashion.titleModel > 0)
                {
                    ShowTitleSprite(fashionItemData);
                }
                else
                {
                    ShowTitleSprite(null);
                }

                break;
        }
    }

    /// <summary>
    /// 显示头顶称号图标
    /// </summary>
    void ShowTitleSprite(FashionItemData fashionItemData)
    {
        msp_title.SetActive(fashionItemData != null);
        if (fashionItemData != null)
        {
            //加载称号
            CSEffectPlayMgr.Instance.ShowUIEffect(msp_title,
                FashionTableManager.Instance.GetFashionTitleModel(fashionItemData.Id).ToString(), ResourceType.UIEffect);
        }
    }


    /// <summary>
    /// 点击升星
    /// </summary>
    /// <param name="go"></param>
    void OnClickRisingStar(GameObject go)
    {
        if (go == null) return;
        if (selectFashionData.IsActive && selectFashionData.TimeLimit == 0)
        {
            UIManager.Instance.CreatePanel<UIFashionLevelUPPanel>((f) =>
            {
                (f as UIFashionLevelUPPanel).SetStarUpData(selectFashionData);
            });
        }
    }

    /// <summary>
    /// 点击穿戴
    /// </summary>
    /// <param name="go"></param>
    void OnClickWear(GameObject go)
    {
        if (go == null) return;
        if (selectFashionData != null)
        {
            if (selectFashionData.IsActive && !selectFashionData.IsEquip)
            {
                //点击穿戴
                Net.CSEquipFashionMessage(selectFashionData.FashionId);
            }
        }
    }

    /// <summary>
    /// 点击卸下
    /// </summary>
    /// <param name="go"></param>
    void OnClickUnLoad(GameObject go)
    {
        if (go == null) return;
        if (selectFashionData != null)
        {
            if (selectFashionData.IsActive && selectFashionData.IsEquip)
            {
                //点击卸下
                Net.CSUnEquipFashionMessage(selectFashionData.FashionId);
            }
        }
    }

    /// <summary>
    /// 点击激活
    /// </summary>
    /// <param name="go"></param>
    void OnClickActive(GameObject go)
    {
        if (go == null) return;
        if (selectFashionData.IsActive) return;
        // FashionCombineData fashionCombineData = CSFashionInfo.Instance.GetFashionAcitveData(selectFashionData.id);
        if (selectFashionData.IsCanBeActivated)
        {
            Net.CSActiveFashionMessage(selectFashionData.FashionId);
        }
        else
        {
            UtilityTips.ShowTips(680, 1.5f, ColorType.Red);
        }
    }

    /// <summary>
    /// 点击说明
    /// </summary>
    /// <param name="go"></param>
    void OnClickRule(GameObject go)
    {
        //查看规则(调用统一接口)
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.Fashion);
    }

    /// <summary>
    /// 点击获取途径
    /// </summary>
    /// <param name="go"></param>
    void OnClickAccess(GameObject go)
    {
        if (go == null) return;
        if (selectFashionData != null && !selectFashionData.IsActive)
        {
            // FashionCombineData fashionCombineData = CSFashionInfo.Instance.GetFashionAcitveData(selectFashionData.id);
            Utility.ShowGetWay(selectFashionData.CombineEquipId);
        }
    }

    /// <summary>
    /// 点击显示总属性加成
    /// </summary>
    /// <param name="go"></param>
    void OnClickAllAttribute(GameObject go)
    {
        List<List<int>> listInfo = new List<List<int>>();
        listInfo.Clear();
        Map<int, int> dicInfo = new Map<int, int>();
        dicInfo.Clear();
        //获取所有加成信息
        List<List<int>> tempList = new List<List<int>>();
        tempList.Clear();
        for (int i = 0; i < myAllFashionInfo.fashions.Count; i++)
        {
            TABLE.FASHION fashion = null;
            if (!FashionTableManager.Instance.TryGetValue(
                myAllFashionInfo.fashions[i].fashionId + 100 * myAllFashionInfo.fashions[i].star, out fashion)) return;
            switch (myCareer)
            {
                case 1: //战
                    tempList = UtilityMainMath.SplitStringToIntLists(
                        fashion.phy);
                    break;
                case 2: //法
                    tempList = UtilityMainMath.SplitStringToIntLists(
                        fashion.magic);
                    break;
                case 3: //道
                    tempList = UtilityMainMath.SplitStringToIntLists(
                        fashion.tao);
                    break;
            }

            for (int j = 0; j < tempList.Count; j++)
            {
                if (dicInfo.ContainsKey(tempList[j][0]))
                {
                    dicInfo[tempList[j][0]] += tempList[j][1];
                }
                else
                {
                    dicInfo.Add(tempList[j][0], tempList[j][1]);
                }
            }
        }

        for (dicInfo.Begin(); dicInfo.Next();)
        {
            List<int> tempList2 = new List<int>();
            tempList2.Add(dicInfo.Key);
            tempList2.Add(dicInfo.Value);
            listInfo.Add(tempList2);
        }

        Utility.ShowAllAttributePanel(676, listInfo);
    }


    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mRole);
        CSEffectPlayMgr.Instance.Recycle(mWeapon);
        UIItemManager.Instance.RecycleItemsFormMediator(listFashionItemBases);
        UIItemManager.Instance.RecycleItemsFormMediator(listWeaponItemBases);
        UIItemManager.Instance.RecycleSingleItem(itemBaseNeed);
        base.OnDestroy();
    }
}