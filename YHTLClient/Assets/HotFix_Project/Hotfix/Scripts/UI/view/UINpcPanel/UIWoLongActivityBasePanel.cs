using System;
using System.Collections;
using System.Collections.Generic;
using TABLE;
using UnityEngine;

public partial class UIWoLongActivityBasePanel : UIBasePanel
{
    List<ItemParameter> paraList; //界面选择列表的数据
    private int curIndex = 0; //选中的当前索引
    int maxindex = 0;//玩家当前能进入的最高等级地图索引
    protected int playLv;//玩家会员等级
    protected int mapid = 6;//副本的id
    
    private bool isNoInit = false;

    private ItemParameter curPara;

    private ITEM iconItem;
    
    public override void Init()
    {
        base.Init();
        AddCollider();
        mClientEvent.Reg((uint)CEvent.ShopBuyTimesChange, ShopBuyTimesChange);
        paraList = new List<ItemParameter>();
        CSBetterLisHot<TABLE.INSTANCE> dataList = InstanceTableManager.Instance.GetTableDataByType(Mapid); //instance table数据
        mgrid_rewards.MaxCount = dataList.Count;
        GetPlayLV();
        //playLv = CSWoLongInfo.Instance.ReturnWoLongInfo().wolongLevel;
        UIEventListener.Get(mbtn_buy).onClick = BuyClick;
        UIEventListener.Get(mbtn_enter).onClick = EnterClick;
        mClientEvent.AddEvent(CEvent.ItemListChange,OnRefreshItem);
        
        //根据数据创建列表        
        for (int i = 0; i < dataList.Count; i++)        
        {
            string mapName = MapInfoTableManager.Instance.GetMapInfoName(dataList[i].mapId);
            //string outStr = CSString.Format(728, mapName, dataList[i].reincarnation, dataList[i].maxLevel);
            string outStr = GetBtnStr(mapName,dataList[i]);
            
            
            mgrid_rewards.controlList[i].GetComponentInChildren<UILabel>().text = outStr;
            string _desc = MapInfoTableManager.Instance.GetMapInfoDesc(dataList[i].mapId);
            //设置item所用到的参数            
            ItemParameter itemParameter = new ItemParameter();
            itemParameter.Instance = dataList[i];
            itemParameter.desc = _desc;   
            itemParameter.index = i;
            paraList.Add(itemParameter);

            mgrid_rewards.controlList[i].GetComponentInChildren<UILabel>().color = IsEnter(itemParameter)
                ? UtilityColor.GetColor(ColorType.NPCMainText)
                : UtilityColor.GetColor(ColorType.WeakText);

            if (IsEnter(itemParameter))
            {
                maxindex = i;
            }
            
            UIEventListener.Get(mgrid_rewards.controlList[i], itemParameter).onClick = ItemClick;
        }
        //初次打开面板选中可以进入的最高等级地图
        curIndex = maxindex;
        ItemClick(mgrid_rewards.controlList[curIndex]);
        UIEventListener.Get(msp_icon.gameObject).onClick = OnClickIcon;

    }

    private void OnClickIcon(GameObject obj)
    {
        UITipsManager.Instance.CreateTips(TipsOpenType.Normal, iconItem);
    }

    private void OnRefreshItem(uint uievtid = 0, object data = null)
    {
        string[] cost = curPara.Instance.requireItems.Split('#');
        if (cost.Length>1)
        {
            int id;
            int.TryParse(cost[0], out id);
            if (ItemTableManager.Instance.TryGetValue(id,out iconItem))
            {
                //string icon = ItemTableManager.Instance.GetItemIcon(int.Parse(cost[0]));
                msp_icon.spriteName = $"tubiao{iconItem.icon}";
                long itemcount = int.Parse(cost[0]).GetItemCount();
                //判断当前的物品是否满足，如果满足显示绿色并显示进入 ，如果不满足显示红色并显示购买
                CSStringBuilder.Clear();
                string showNum = CSStringBuilder.Append(itemcount, "/", cost[1]).ToString(); 
                //Debug.Log("showNum :" + showNum);
                if (itemcount >= int.Parse(cost[1]))
                {
                    mbtn_enter.SetActive(true);
                    mbtn_buy.SetActive(false);
                    mlb_value.text = showNum.BBCode(ColorType.Green);
                }
                else
                {
                    mbtn_enter.SetActive(false);
                    mbtn_buy.SetActive(true);
                    mlb_value.text = showNum.BBCode(ColorType.Red);
                }
            }

            
        }
        
    }

    /// <summary>
    /// 选择进入卧龙山庄层数
    /// </summary>
    /// <param name="go"></param>
    void ItemClick(GameObject go)
    {
        curPara = (ItemParameter)UIEventListener.Get(go).parameter;

        if (!IsEnter(paraList[curPara.index]) && isNoInit)
        {
            UtilityTips.ShowRedTips(RedTipId);
            return; 
        }
        isNoInit = true;
        mlb_desc.text = curPara.desc;

        OnRefreshItem();
        
        SetItemActive(mgrid_rewards.controlList[curIndex], false);
        SetItemActive(go, true);
        curIndex = curPara.index;

        
        //mlb_level.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(729), curPara.Instance.reincarnation); ;
        mlb_level.text = GetlevelStr(curPara.Instance);
        
        if (IsEnter(paraList[curIndex]))
            mlb_level.color = UtilityColor.GetColor(ColorType.NPCImportantText);
        else
            mlb_level.color = UtilityColor.GetColor(ColorType.Red);
    }
    
    void BuyClick(GameObject go)
    {

        string[] cost = paraList[curIndex].Instance.requireItems.Split('#');
        int id;
        int.TryParse(cost[0],out id);
        var mData = CSShopInfo.Instance.GetShopData(id);
        if (mData == null)
        {
            Utility.ShowGetWay(id);
            //UtilityTips.ShowRedTips(1699);
            return;
        }

        
        
        UIManager.Instance.CreatePanel<UIBuyConfirmPanel>((f) =>
        {
            (f as UIBuyConfirmPanel).OpenPanel(int.Parse(cost[0]), mData.config, mData.buyTimesLimit, mData.BuyTimes);
        });
    }

    void EnterClick(GameObject go)
    {
        if (IsEnter(paraList[curIndex]))
        {
            Net.ReqEnterInstanceMessage(paraList[curIndex].Instance.mapId);
            Close();
            //UIManager.Instance.ClosePanel<UIWoLongActivitybasPanel>();
        }
        else
            UtilityTips.ShowRedTips(CSString.Format(1021));
    }
    
    void SetItemActive(GameObject go , bool active)
    {
        GameObject bg = go.transform.Find("bg").gameObject;
        UILabel uiLabel = go.transform.Find("lb_level").GetComponent<UILabel>();
        bg.SetActive(active);
        //改变字体颜色
        if (active)
            uiLabel.color = UtilityColor.GetColor(ColorType.MainText);
        else
            if (IsEnter(paraList[curIndex]))
                uiLabel.color = UtilityColor.GetColor(ColorType.NPCMainText);
            else 
                uiLabel.color = UtilityColor.GetColor(ColorType.WeakText);
    }

    void ShopBuyTimesChange(uint id, object data)
    {
        ItemClick(mgrid_rewards.controlList[curIndex]);
    }

    /// <summary>
    /// 判断是否满足进入条件
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsEnter(ItemParameter para)
    {
        if (para.Instance.reincarnation <= playLv && playLv <= para.Instance.maxLevel)
            return true;
        else
            return false;
    }
    
    

    #region 需要实现的虚方法
    protected struct ItemParameter
    {
        public string desc; //卧龙山庄掉落信息
        public int index; //索引
        public TABLE.INSTANCE Instance;
    }
    
    public virtual void GetPlayLV()
    {
        
    }
    
    protected virtual string GetBtnStr(string mapName,TABLE.INSTANCE para)
    {
        return CSString.Format(728, mapName, para.reincarnation, para.maxLevel);
    }

    protected virtual string GetlevelStr(TABLE.INSTANCE para)
    {
        
        return string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(729), para.reincarnation);
    }

    public virtual int Mapid
    {
        get { return mapid; }
    }

    private int _redTipId = 1617;
    
    public virtual int RedTipId
    {
        get { return _redTipId; }
    } 


    #endregion
    
   
    
    
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.NpcDialog;
    }
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
}

