using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using TABLE;
using UnityEngine;

public partial class UIPearlSkillslotPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    private PearlData myPearlData;
    private List<SkillSlotData> listSkillSlots;

    private int selectIndex = 0;
    private int lastSelectIndex = 0;

    private List<UIItemBase> itemListBaoZhu;

    private List<ConsumePropData> consumePropDatas;
    /// <summary>
    /// 技能图集
    /// </summary>
    private UIAtlas atlasSkillIcon;

    /// <summary>
    /// 是否继续刷新重铸技能
    /// </summary>
    private bool isContinuance = false;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.RefreshBaoZhuSkills, RefreshBaoZhuSkills);
        mClientEvent.Reg((uint) CEvent.ReplaceBaoZhuSkills, ReplaceBaoZhuSkills);
        mClientEvent.Reg((uint)CEvent.ItemListChange, RefreshDataItemChange);
        
        mbtn_rule.onClick = OnClickRule;
        mbtn_preview.onClick = OnClickPreview;
        mbtn_add.onClick = OnClickAdd;
        mbtn_add1.onClick = OnClickAdd1;
        mbtn_replace.onClick = OnClickReplace;
        mbtn_refresh.onClick = OnClickReFresh;
        mbtn_inject.onClick = OnClickInject;
        mbtn_reinject.onClick = OnClickReInject;

        atlasSkillIcon = msp_itemicon1.atlas;
    }
    
    /// <summary>
    /// 道具和金币变化
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void RefreshDataItemChange(uint id, object data)
    {
        SetConsumeProps();
    }
    
    /// <summary>
    /// 返回刷新的技能
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    void RefreshBaoZhuSkills(uint id, object data)
    {
        isContinuance = true;
        InitGrid();
        RefreshItem(selectIndex);
        SetRightInfo(selectIndex);
    }
    
    void ReplaceBaoZhuSkills(uint id, object data)
    {
        listSkillSlots = CSPearlInfo.Instance.MyPearlData.ListSkillSlotDatas;
        isContinuance = false;
        InitGrid();
        RefreshItem(selectIndex);
        SetRightInfo(selectIndex);
    }
    
    /// <summary>
    /// 替换
    /// </summary>
    /// <param name="go"></param>
    void OnClickReplace(GameObject go)
    {
        if (go == null) return;
        Net.CSChoseBaoZhuSkillMessage(listSkillSlots[selectIndex].SkillSlotId);
    }
    
    /// <summary>
    /// 注入
    /// </summary>
    /// <param name="go"></param>
    void OnClickInject(GameObject go)
    {
        if (go == null) return;
        bool isEnough =  GetOnClickIsEnough();
        if (isEnough)
        {
            Net.CSRandBaoZhuSkillMessage(listSkillSlots[selectIndex].SkillSlotId);
        }
    }
    
    /// <summary>
    /// 重铸 
    /// </summary>
    /// <param name="go"></param>
    void OnClickReInject(GameObject go)
    {
        if (go == null) return;
        bool isEnough =  GetOnClickIsEnough();
        if (isEnough)
        {
            Net.CSRandBaoZhuSkillMessage(listSkillSlots[selectIndex].SkillSlotId);
        }
    }

    /// <summary>
    /// 继续重铸
    /// </summary>
    /// <param name="go"></param>
    void OnClickReFresh(GameObject go)
    {
        if (go == null) return;
        bool isEnough =  GetOnClickIsEnough();
        if (isEnough)
        {
            Net.CSRandBaoZhuSkillMessage(listSkillSlots[selectIndex].SkillSlotId);
        }
    }

    /// <summary>
    /// 处理道具够不够的情况
    /// </summary>
    bool GetOnClickIsEnough()
    {
        bool isEnough = true;
        if (consumePropDatas!=null)
        {
            for (int i = 0; i < consumePropDatas.Count; i++)
            {
                if (!consumePropDatas[i].isEnough)
                {
                    Utility.ShowGetWay(consumePropDatas[i].propId);
                    isEnough = false;
                    break;
                }
            }
        }

        return isEnough;
    }
    
    void OnClickAdd(GameObject go)
    {
        if (go == null) return;
        if (consumePropDatas!=null&&consumePropDatas.Count>=1)
        {
            Utility.ShowGetWay(consumePropDatas[0].propId);
        }
    }
    
    void OnClickAdd1(GameObject go)
    {
        if (go == null) return;
        if (consumePropDatas!=null&&consumePropDatas.Count==2)
        {
            Utility.ShowGetWay(consumePropDatas[1].propId);    
        }
    }

    void OnClickRule(GameObject go)
    {
        if (go == null) return;
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.SkillslotBaoZhu);
    }

    void OnClickPreview(GameObject go)
    {
        if (go == null) return;
        UIManager.Instance.CreatePanel<UIPearSkillslotlPreviewPanel>();
    }

    public override void Show()
    {
        base.Show();
        CSEffectPlayMgr.Instance.ShowUITexture(mbg0, "pearl_bg2");
        CSEffectPlayMgr.Instance.ShowUITexture(mbg1, "pearl_bg2");
        CSEffectPlayMgr.Instance.ShowUITexture(mbg2, "pearl_bg2");
        InitData();
    }
    
    void InitData()
    {
        myPearlData = CSPearlInfo.Instance.MyPearlData;
        if (myPearlData == null) return;
        listSkillSlots = CSPearlInfo.Instance.MyPearlData.ListSkillSlotDatas;
        if (listSkillSlots!=null&&listSkillSlots.Count>0)
        {
            InitGrid();
            RefreshItem(selectIndex);
            SetRightInfo(selectIndex);
        }
    }
    
    void InitGrid()
    {
        itemListBaoZhu = new List<UIItemBase>();
        itemListBaoZhu.Clear();
        mgrid_skillslot.MaxCount = listSkillSlots.Count;
        GameObject gp;
        ScriptBinder gpBinder;
        UISprite sp_itemicon;
        UILabel lb_name;
        UILabel lb_skillname;
        UILabel lb_hint;
        GameObject effect;
        GameObject itemGrid;
        GameObject objlock;
        GameObject redpoint;
        var arr = BaoZhuJinHuaTableManager.Instance.array.gItem.handles;
        for (int i = 0; i < mgrid_skillslot.MaxCount; i++)
        {
            gp = mgrid_skillslot.controlList[i];
            gpBinder = gp.transform.GetComponent<ScriptBinder>();
            itemGrid = gpBinder.GetObject("itemGrid") as GameObject;
            lb_name = gpBinder.GetObject("lb_name") as UILabel;
            lb_skillname = gpBinder.GetObject("lb_skillname") as UILabel;
            lb_hint = gpBinder.GetObject("lb_hint") as UILabel;
            effect = gpBinder.GetObject("effect") as GameObject;
            redpoint = gpBinder.GetObject("redpoint") as GameObject;
            
            UIItemBase itemBase =  UIItemManager.Instance.GetItem(PropItemType.Normal, itemGrid.transform, itemSize.Size66);
            itemListBaoZhu.Add(itemBase);
            sp_itemicon = itemBase.obj.transform.Find("sp_itemicon").GetComponent<UISprite>();
            objlock = itemBase.obj.transform.Find("lock").gameObject;

            sp_itemicon.atlas = atlasSkillIcon;
            lb_name.text = CSString.Format(947, listSkillSlots[i].SkillSlotId);
            effect.SetActive(i==selectIndex);
            //分 未解锁/已解锁没有技能/已解锁有技能  3种状态
            if (!listSkillSlots[i].IsUnlock)//未解锁
            {
                int gradeUnlock = 0;
                for(int k = 0,max = arr.Length;k < max;++k)
                {
                    var item = arr[k].Value as TABLE.BAOZHUJINHUA;
                    if (item.skillSlotID == listSkillSlots[i].SkillSlotId)
                    {
                        gradeUnlock = item.rank;
                        lb_hint.text = CSString.Format(1028, gradeUnlock);
                        break;
                    }
                }
                lb_hint.gameObject.SetActive(true);
                lb_skillname.gameObject.SetActive(false);
                sp_itemicon.gameObject.SetActive(false);
                objlock.SetActive(true);
                redpoint.SetActive(false);
            }
            else
            {
                if (listSkillSlots[i].SkillId==0)//已解锁没有技能
                {
                    lb_hint.gameObject.SetActive(false);
                    lb_skillname.text = CSString.Format(1027);
                    lb_skillname.gameObject.SetActive(true);
                    sp_itemicon.gameObject.SetActive(false);
                    objlock.SetActive(false);
                    redpoint.SetActive(true);
                }
                else//已解锁有技能
                {
                    lb_hint.gameObject.SetActive(false);
                    lb_skillname.text = SkillTableManager.Instance.GetSkillName(listSkillSlots[i].SkillId);
                    lb_skillname.gameObject.SetActive(true);
                    sp_itemicon.spriteName = SkillTableManager.Instance.GetSkillIcon(listSkillSlots[i].SkillId);
                    sp_itemicon.gameObject.SetActive(true);
                    objlock.SetActive(false);
                    redpoint.SetActive(false);
                }
            }

            UIEventListener.Get(gp, i).onClick = OnClickItem;
        }
    }

    void OnClickItem(GameObject go)
    {
        if (go == null) return;
        int index = (int)UIEventListener.Get(go).parameter;
        if (index == selectIndex) return;
        var arr = BaoZhuJinHuaTableManager.Instance.array.gItem.handles;
        //如果未解锁在这里处理掉
        if (!listSkillSlots[index].IsUnlock)//未解锁
        {
            int gradeUnlock = 0;
            for(int k = 0,max = arr.Length;k < max;++k)
            {
                var item = arr[k].Value as TABLE.BAOZHUJINHUA;
                if (item.skillSlotID == listSkillSlots[index].SkillSlotId)
                {
                    gradeUnlock = item.rank;
                    UtilityTips.ShowTips(CSString.Format(1028, gradeUnlock));
                    break;
                }
            }
            return;
        }
        isContinuance = false;
        lastSelectIndex = selectIndex;
        selectIndex = index;
        RefreshItem(selectIndex);
        RefreshItem(lastSelectIndex);
        SetRightInfo(index);
    }

    void RefreshItem(int index)
    {
        GameObject gp;
        GameObject effect;
        gp = mgrid_skillslot.controlList[index];
        effect = gp.transform.Find("effect").gameObject;
        effect.SetActive(index==selectIndex);
    }
    
    /// <summary>
    /// 设置消耗道具
    /// </summary>
    void SetConsumeProps()
    {
        consumePropDatas = new List<ConsumePropData>();
        consumePropDatas.Clear();
        
        List<List<int>> consumeProps = UtilityMainMath.SplitStringToIntLists(
        BaoZhuSlotTableManager.Instance.GetBaoZhuSlotRefresh(listSkillSlots[selectIndex].SkillSlotId));
        
        if (consumeProps.Count==1)
        {
            mUIItemBarPrefab.SetActive(true);
            mUIItemBarPrefab1.SetActive(false);
            mgrid_UIItemBarPrefab.repositionNow = true;
            mgrid_UIItemBarPrefab.Reposition();

            ConsumePropData consumePropData = new ConsumePropData(
                0, 
                consumeProps[0][0],
                CSBagInfo.Instance.GetAllItemCount(consumeProps[0][0]),
                consumeProps[0][1]);
            consumePropDatas.Add(consumePropData);
            
            msp_icon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(consumePropData.propId)}";
            if (consumePropData.isEnough)
            {
                mlb_value.text = $"[00ff0c]{consumePropData.curCount}/{consumePropData.consumeCount}[-]";
            }
            else
            {
                mlb_value.text = $"[ff0000]{consumePropData.curCount}/{consumePropData.consumeCount}[-]";
            }
        }
        else if (consumeProps.Count==2)
        {
            mUIItemBarPrefab.SetActive(true);
            mUIItemBarPrefab1.SetActive(true);
            mgrid_UIItemBarPrefab.repositionNow = true;
            mgrid_UIItemBarPrefab.Reposition();
            
            ConsumePropData consumePropData = new ConsumePropData(
                0, 
                consumeProps[0][0],
                CSBagInfo.Instance.GetAllItemCount(consumeProps[0][0]),
                consumeProps[0][1]);
            consumePropDatas.Add(consumePropData);
            
            ConsumePropData consumePropData1 = new ConsumePropData(
                1, 
                consumeProps[1][0],
                CSBagInfo.Instance.GetAllItemCount(consumeProps[1][0]),
                consumeProps[1][1]);
            consumePropDatas.Add(consumePropData1);
            
            msp_icon.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(consumePropData.propId)}";
            msp_icon1.spriteName = $"tubiao{ItemTableManager.Instance.GetItemIcon(consumePropData1.propId)}";
            if (consumePropData.isEnough)
            {
                mlb_value.text = $"[00ff0c]{consumePropData.curCount}/{consumePropData.consumeCount}[-]";
                mlb_value1.text = $"[00ff0c]{consumePropData1.curCount}/{consumePropData1.consumeCount}[-]";
            }
            else
            {
                mlb_value.text = $"[ff0000]{consumePropData.curCount}/{consumePropData.consumeCount}[-]";
                mlb_value1.text = $"[ff0000]{consumePropData1.curCount}/{consumePropData1.consumeCount}[-]";
            }
        }
        // else
        // {
        //     Debug.Log("-----------------消耗道具配置无效");
        // }
    }

    //继续
    void SetRightInfo(int index)
    {
        SetConsumeProps();
        //未解锁状态在点击处处理掉，这里只显示解锁状态的信息
        if (listSkillSlots[index].IsUnlock)//解锁状态
        {
            if (listSkillSlots[index].SkillId==0)//解锁无技能
            {
                mcolInjection.SetActive(true);
                mcolRefresh.SetActive(false);
            }
            else//解锁有技能
            {
                mcolInjection.SetActive(false);
                mcolRefresh.SetActive(true);
                msp_itemicon1.spriteName = SkillTableManager.Instance.GetSkillIcon(listSkillSlots[index].SkillId);
                mlb_itemName1.text = SkillTableManager.Instance.GetSkillName(listSkillSlots[index].SkillId);
                mlb_content1.text = CSString.Format(550,
                    SkillTableManager.Instance.GetSkillDescription(listSkillSlots[index].SkillId));
                mlb_skillCD1.text = CSString.Format(957,SkillTableManager.Instance.GetSkillCdTime(listSkillSlots[index].SkillId)/1000);
                
                //分情况是否继续刷新
                msp_itemicon2.gameObject.SetActive(isContinuance);
                mlb_itemName2.gameObject.SetActive(isContinuance);
                mlb_content2.gameObject.SetActive(isContinuance);
                mlb_skillCD2.gameObject.SetActive(isContinuance);
                mlb_hint2.SetActive(!isContinuance);
                mgrid_btns.SetActive(isContinuance);
                mbtn_reinject.gameObject.SetActive(!isContinuance);
                if (isContinuance)
                {
                    msp_itemicon2.spriteName = SkillTableManager.Instance.GetSkillIcon(listSkillSlots[index].SkillId);
                    mlb_itemName2.text = SkillTableManager.Instance.GetSkillName(listSkillSlots[index].SkillId);
                    
                    int reFreshSkillId = 0;
                    if (CSPearlInfo.Instance.MyPearlData.dicRefreshSkill.ContainsKey(listSkillSlots[index].SkillSlotId))
                    {
                        reFreshSkillId = CSPearlInfo.Instance.MyPearlData.dicRefreshSkill[listSkillSlots[index].SkillSlotId];
                    }
                    mlb_content2.text =CSString.Format(550,
                        SkillTableManager.Instance.GetSkillDescription(reFreshSkillId));
                    mlb_skillCD2.text = CSString.Format(957,SkillTableManager.Instance.GetSkillCdTime(reFreshSkillId)/1000);
                }
            }
        }
        // else//未解锁
        // {
        //     mcolInjection.SetActive(true);
        //     mcolRefresh.SetActive(false);
        // }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UIItemManager.Instance.RecycleItemsFormMediator(itemListBaoZhu);
    }

    /// <summary>
    /// 单个消耗道具的数据类
    /// </summary>
    class ConsumePropData
    {
        public ConsumePropData(){
        }

        public ConsumePropData(int i, int id, long curNum, int consumeNum)
        {
            index = i;
            propId = id;
            curCount = curNum;
            consumeCount = consumeNum;
            isEnough = (curNum >= consumeNum);
        }

        //第几个道具
        public int index = 0;
        public int propId = 0;
        public long curCount = 0;
        public int consumeCount = 0;
        //是否足够
        public bool isEnough = false;
    }
}