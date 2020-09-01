using System;
using System.Collections.Generic;
using lifelongfund;
using TABLE;
using UnityEngine;

public struct FundTaskStruct
{
    public FundTaskInfo _fundTaskInfo;
    public bool isBuy; //是否购买礼包
}

public partial class UILifeTimeFundPanel : UIBasePanel
{
    private LifelongFundInfo lifelongFundInfo;
    private List<rewardClassData> rewardidList;
    private FundTaskStruct fundTaskStruct;
    private CSBetterLisHot<FundTaskStruct> fundTaskStructList = new CSBetterLisHot<FundTaskStruct>();
    //JIJINREWARD jijinreward = new JIJINREWARD();
    private int buyMoney = 0;
    private Map<GameObject, int> itemIndexList = new Map<GameObject, int>();
    private Vector3 mStartLocalPos;
    //Map<int, int> rewardMap = new Map<int, int>();
    private rewardClassData curscoreData;
    private UIPanel _panel;
    
    EndLessList<UIlifeTimeRewardItem, rewardClassData> endLessList;

    /// <summary>
    /// 当前索引
    /// </summary>
    private float curprocess;

    /// <summary>
    /// 总动态列表个数 用来计算进度条
    /// </summary>
    private int rewardNum;

    public override void Init()
    {
        base.Init();
        
        
        UIEventListener.Get(mbtn_close).onClick = Close;
        UIEventListener.Get(mbtn_buy).onClick = OnBuyClick;
        UIEventListener.Get(mbtn_help).onClick = OnHelpClick;
        UIEventListener.Get(mbtn_scrollh).onClick = OnScrollClick;
        UIEventListener.Get(mbtn_scrollh).onPress = OnScrollPress;
        mClientEvent.AddEvent(CEvent.LifeTimeFundChange, RefreshUI);
        mClientEvent.AddEvent(CEvent.LifeTimeFundRewardChange, RefreshReward);
        
        //设置特效
        CSEffectPlayMgr.Instance.ShowUITexture(mbanner2, "banner2");
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_buyeffect.gameObject, 17904, 10, false);
        for (int i = 0; i < mgrid_reward.MaxCount; i++)
        {
            UISprite effect = UtilityObj.Get<UISprite>(mgrid_reward.controlList[i].transform, "effect");
            CSEffectPlayMgr.Instance.ShowUIEffect(effect.gameObject, 17506);
        }

        //获得参数
        buyMoney = int.Parse(SundryTableManager.Instance.GetSundryEffect(621));
        mgrid_reward.MaxCount = 8; //使用动态列表
        _panel = mscrollview_reward.GetComponent<UIPanel>();
        mStartLocalPos = mscrollview_reward.transform.localPosition;
        
        //设置滑动条
        mscrollview_task.SetDynamicArrowVertical(mbtn_scrollv);
        mscrollview_reward.horizontalScrollBar.onChange.Add(new EventDelegate(UpdateScrollbar));
        
        
        //CSEffectPlayMgr.Instance.ShowUIEffect(select, 17506);
        //mwrap_items.onInitializeItem = WrapUpdate;
        //mwrap_items.enabled = true;
        //mwrap_items.maxIndex = 30; //这里需要先设置一个值 保证动态列表的刷新正常
        
    }

    public override void Show()
    {
        CSlifeTimeFundInfo.Instance.isFirstTabClick = false;
        mClientEvent.SendEvent(CEvent.LifeTimeFundTabRed);
        base.Show();
        RefreshUI();
    }
    
    /// <summary>
    /// 滑动条按压
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="isPress"></param>
    private void OnScrollPress(GameObject arg1, bool isPress)
    {
        if (isPress)
        {
            ScriptBinder.InvokeRepeating(0, 1, OnScroll);
        }
        else
        {
            ScriptBinder.StopInvokeRepeating();
        }

    }

    /// <summary>
    /// 滑动条下翻函数
    /// </summary>
    private void OnScroll()
    {
        int itemSize = mwrap_items.itemSize;
        //Math.Ceiling(curprocess * (mwrap_items.maxIndex - 6));
        //int curindex = UtilityMath.GetRoundingInt(curprocess * (mwrap_items.maxIndex - 6));
        int curindex = (int)Math.Ceiling(curprocess * (mwrap_items.maxIndex - 6));
        //Debug.Log("curprocess * (mwrap_items.maxIndex" + curprocess * (mwrap_items.maxIndex - 6));
        curindex++;
        //UnityEngine.Debug.Log("curindex ::::" + curindex );

        Vector3 vec = mStartLocalPos - new Vector3(itemSize * curindex, 0, 0);

        SpringPanel.Begin(mscrollview_reward.gameObject, vec, 10);
    }

    /// <summary>
    /// 滑动条点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnScrollClick(GameObject obj)
    {
        OnScroll();
    }

    /// <summary>
    /// 刷新奖励列表
    /// </summary>
    /// <param name="uievtid"></param>
    /// <param name="data"></param>
    private void RefreshReward(uint uievtid, object data)
    {
        lifelongFundInfo = CSlifeTimeFundInfo.Instance.LifelongFundInfo;
        RefreshReward();
    }

    /// <summary>
    /// 刷新整个界面
    /// </summary>
    /// <param name="uievtid"></param>
    /// <param name="data"></param>
    private void RefreshUI(uint uievtid = 0, object data = null)
    {
        fundTaskStructList.Clear();
        lifelongFundInfo = CSlifeTimeFundInfo.Instance.LifelongFundInfo;
        if (lifelongFundInfo == null)
        {
            return;
        }
        RefreshReward();
        
        mgrid_task.MaxCount = lifelongFundInfo.fundTaskInfos.Count;

        for (int i = 0; i < mgrid_task.MaxCount; i++)
        {
            fundTaskStruct._fundTaskInfo = lifelongFundInfo.fundTaskInfos[i];
            fundTaskStruct.isBuy = lifelongFundInfo.isBuy;
            fundTaskStructList.Add(fundTaskStruct);
        }
        
        fundTaskStructList.Sort((x, y) =>
        {
            return x._fundTaskInfo.taskState.CompareTo(y._fundTaskInfo.taskState);
        });
        
        mlb_point.text = lifelongFundInfo.curPoint.ToString();

        mlb_money.text = buyMoney.ToString();

        string icon = $"tubiao{(int) MoneyType.yuanbao}";
        mSprite.spriteName = icon;
        mbtn_buy.gameObject.SetActive(!lifelongFundInfo.isBuy);
        mbtnbuy_red.SetActive(CSlifeTimeFundInfo.Instance.isFirstBtnClick);
        
        mgrid_task.Bind<FundTaskStruct,UILifeTimeFundTaskBar>(fundTaskStructList.BetterLisToGoogleList(),mPoolHandleManager);
        
    }

    
    private void RefreshReward()
    {
        rewardidList = CSlifeTimeFundInfo.Instance.GetRewardList();
        mwrap_items.maxIndex = rewardidList.Count;
        rewardNum = rewardidList.Count + 1;
        int itemsize = mwrap_items.itemSize;
        int curScoreIndex = CSlifeTimeFundInfo.Instance.GetFirstReward();
        Vector3 vec = mStartLocalPos - new Vector3(itemsize*curScoreIndex,0,0);
        SpringPanel.Begin(mscrollview_reward.gameObject, vec,10);
        // for (itemIndexList.Begin(); itemIndexList.Next();)
        // {
        //     RefreshRewardItem(itemIndexList.Key, itemIndexList.Value);
        // }
        //刷新无线滚动列表
        if (endLessList == null)
        {
            endLessList = new EndLessList<UIlifeTimeRewardItem,rewardClassData>(SortType.Horizen, mwrap_items, mPoolHandleManager, 8, ScriptBinder);
        }
        endLessList.Clear();
        
        for (int i = 0; i < rewardidList.Count; i++)
        {
            var endLessdata = endLessList.Append();
            endLessdata.CopyData(rewardidList[i]);    
        }
        //添加最后一个
        var endLessMaxdata = endLessList.Append();
        endLessMaxdata.isMax = true;
        
        endLessList.Bind();
        
    }

    
    #region warp方法 , 已使用最新

// private void WrapUpdate(GameObject go, int wrapIndex, int realIndex)
    // {
    //     if (itemIndexList.ContainsKey(go))
    //         itemIndexList[go] = realIndex;
    //     else
    //         itemIndexList.Add(go,realIndex);
    //
    //     RefreshRewardItem(go,realIndex);
    //     
    // }
    
    // private void RefreshRewardItem(GameObject go , int realIndex)
    // {
    //     if (realIndex < 0)
    //     {
    //         return;
    //     }
    //     ScriptBinder binder =  go.GetComponent<ScriptBinder>();
    //     //设置数据
    //     UISprite msp_icon = binder.GetObject("sp_icon") as UISprite;
    //     UISprite mobj_check = binder.GetObject("obj_check") as UISprite;
    //     UILabel mlb_point = binder.GetObject("lb_point") as UILabel;
    //     GameObject mobj_select = binder.GetObject("obj_select") as GameObject;
    //     GameObject mobj_bg = binder.GetObject("obj_bg") as GameObject;
    //     GameObject mobj_text = binder.GetObject("obj_text") as GameObject;
    //     GameObject obj_process = binder.GetObject("obj_process") as GameObject;
    //     GameObject effect = binder.GetObject("effect") as GameObject;
    //     UILabel mlb_num = binder.GetObject("lb_num") as UILabel;
    //     //GameObject effect = binder.GetObject("obj_process") as GameObject;
    //     //GameObject mred = binder.GetObject("red") as GameObject;
    //     
    //     if (rewardidList!= null &&lifelongFundInfo != null)
    //     {
    //         int count = rewardidList.Count;
    //         //最大值不显示item数据 ,显示解锁更多text
    //         bool IsMaxItem = realIndex == count;
    //         
    //         mobj_text.SetActive(IsMaxItem);
    //         mobj_check.gameObject.SetActive(!IsMaxItem);
    //         mlb_point.gameObject.SetActive(!IsMaxItem);
    //         mobj_select.gameObject.SetActive(!IsMaxItem);
    //         msp_icon.gameObject.SetActive(!IsMaxItem);
    //         effect.SetActive(!IsMaxItem);
    //         mlb_num.gameObject.SetActive(!IsMaxItem);
    //         if (IsMaxItem)
    //         {
    //             UIEventListener.Get(mobj_bg).onClick = null;
    //             return;
    //         }
    //
    //         if (rewardidList.Count > realIndex)
    //         {
    //             rewardClassData rewardclass = rewardidList[realIndex];
    //             int id = rewardclass.id;
    //             int score = rewardclass.score;
    //             if (JijinRewardTableManager.Instance.TryGetValue(id, out jijinreward))
    //             {
    //                 msp_icon.spriteName = ItemTableManager.Instance.GetItemIcon(jijinreward.reward);
    //             }
    //
    //             bool isReceived = lifelongFundInfo.isBuy&&!lifelongFundInfo.unreceivedRewards.Contains(score) && score <= lifelongFundInfo.curPoint; //已领取
    //             mobj_check.gameObject.SetActive(isReceived);
    //
    //             bool isReceive = lifelongFundInfo.unreceivedRewards.Contains(score);
    //
    //             bool isMoreScore = rewardclass.score <= rewardclass.GetScore;
    //             mobj_select.SetActive(isReceive);
    //             //根据是否购买显示特效
    //             effect.SetActive(lifelongFundInfo.isBuy ? isReceive:isMoreScore);
    //             mlb_point.text = score.ToString();
    //             msp_icon.color = isReceived ? Color.black : Color.white; 
    //             obj_process.SetActive(isMoreScore);
    //             mlb_num.text = jijinreward.num.ToString();
    //             //mred.SetActive(isReceive);
    //             //GameObject obj = utility
    //             UIEventListener.Get(mobj_bg,rewardclass).onClick = OnReceiveRewardClick;
    //         }
    //         
    //     }
    // }

    #endregion
    
    private void OnHelpClick(GameObject obj)
    {
        UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.LifeTimeFund);
    }

    private void OnBuyClick(GameObject obj)
    {
        int moneyID = (int)MoneyType.yuanbao;
        mbtnbuy_red.SetActive(false);
        CSlifeTimeFundInfo.Instance.isFirstBtnClick = false;
        if (moneyID.GetItemCount() < buyMoney)
        {
            UtilityPanel.JumpToPanel(12305);
            //UIManager.Instance.ClosePanel<UILifeTimeFundPanel>();
            return;
        }
        Net.CSBuylifelongFundMessage();
    }
    
    /// <summary>
    /// 计算进度条的值 用来解决进度条跟随组件的问题
    /// </summary>
    private void UpdateScrollbar()
    {
        //初始化赋值 
        int mScrollLegth = mwrap_items.itemSize*rewardNum;
        //float pct = 0.0f;
        float endPos = (mScrollLegth - _panel.GetViewSize().x) - Mathf.Abs(mStartLocalPos.x);
        curprocess = Mathf.Clamp((mscrollview_reward.transform.localPosition.x - mStartLocalPos.x) 
                                 / (mStartLocalPos.x - endPos), 0, 100f);
        
        mbtn_scrollh.SetActive(curprocess<= 1f);
        if (mbtn_scrollh.activeSelf == false)
        {
            ScriptBinder.StopInvokeRepeating();
        }
    }


    protected override void OnDestroy()
	{
        mgrid_task.UnBind<UILifeTimeFundTaskBar>();
        fundTaskStructList = null;
        rewardidList = null;
        lifelongFundInfo = null;
        //jijinreward = null;
        CSEffectPlayMgr.Instance.Recycle(mobj_buyeffect.gameObject);
        endLessList.Destroy();
        base.OnDestroy();
    }
}

public class UILifeTimeFundTaskBar : UIBinder
{
    UISprite msp_flag;
    UILabel mlb_point;
    UILabel mlb_des;
    UISprite mbtn_go;
    GameObject mlb_hint;
    GameObject msp_complete;
    GameObject mbtn_Receive;
    private GameObject red;

    private string[] flags;
    private FundTaskStruct _data;
    private JIJINTASK jijintask;
    public override void Init(UIEventListener handle)
    {
        msp_flag = Get<UISprite>("sp_flag");
        mlb_point = Get<UILabel>("lb_point");
        mlb_des = Get<UILabel>("lb_des");
        mbtn_go = Get<UISprite>("btn_go");
        mlb_hint = Get<GameObject>("lb_hint");
        msp_complete = Get<GameObject>("sp_complete");
        mbtn_Receive = Get<GameObject>("btn_Receive");
        
        red = Get<GameObject>("red");
        
        flags = SundryTableManager.Instance.GetSundryEffect(627).Split('#');

        UIEventListener.Get(mbtn_go.gameObject).onClick = OnGoClick;
        UIEventListener.Get(mbtn_Receive).onClick = OnReceiveClick;
    }

    private void OnReceiveClick(GameObject obj)
    {
        Net.CSReceiveFundTaskRewardMessage(_data._fundTaskInfo.taskId);
    }

    private void OnGoClick(GameObject obj)
    {
        if (jijintask.deliver != 0)
        {
            UtilityPath.FindWithDeliverId(jijintask.deliver);
        }
        else if (jijintask.uiModel != 0)
        {
            UtilityPanel.JumpToPanel(jijintask.uiModel);
        }
        UIManager.Instance.ClosePanel<UIWelfareActivityPanel>();
    }

    public override void Bind(object data)
    {
        _data = (FundTaskStruct)data;
        FundTaskInfo Info = _data._fundTaskInfo;
        if (JijinTaskTableManager.Instance.TryGetValue(Info.taskId,out jijintask))
        {
            if (flags.Length > jijintask.type)
            {
                msp_flag.spriteName = flags[jijintask.type];
            }
            
            mlb_point.text =  jijintask.getIntegral.ToString();
            string[] para = jijintask.typeParameter.Split('#');
            CSStringBuilder.Clear();
            string processStr = CSString.Format(1042, Info.curProgress, jijintask.count);
            string processcolor = Info.curProgress == jijintask.count
                ? processStr.BBCode(ColorType.NPCImportantText)
                : processStr.BBCode(ColorType.Red);
            mlb_des.text = CSStringBuilder.Append(string.Format(jijintask.name, para),processcolor).ToString();
            mbtn_go.gameObject.SetActive(false);

            bool isDisable = jijintask.deliver == 0 && jijintask.uiModel == 0;
            
            mbtn_go.color = isDisable ? Color.black : Color.white;
            
            mlb_hint.SetActive(false);
            msp_complete.SetActive(false);
            mbtn_Receive.SetActive(false);
            red.SetActive(false);
            // if (!_data.isBuy)
            // {
            //     mlb_hint.SetActive(true);
            // }
            // else
            // {
                switch (Info.taskState)
                {
                    case 0:
                        mbtn_go.gameObject.SetActive(true);
                        break;
                    case 1:
                        mbtn_Receive.SetActive(true);
                        red.SetActive(true);
                        break;
                    case 2:
                        msp_complete.SetActive(true);
                        break;
                }
            //}
            
        }
    }
    
    public override void OnDestroy()
    {
        msp_flag = null;
        mlb_point= null;
        mlb_des= null;
        mbtn_go= null;
        mlb_hint= null;
        msp_complete= null;
        mbtn_Receive= null;
        flags = null;
        jijintask = null;
        red = null;
    }
}

public class UIlifeTimeRewardItem : UIBinder
{
    UISprite msp_icon;
    UISprite mobj_check;
    UILabel mlb_point;
    GameObject mobj_select;
    GameObject mobj_bg;
    GameObject mobj_text;
    GameObject obj_process;
    GameObject effect;
    UILabel mlb_num;
    
    private LifelongFundInfo lifelongFundInfo;
    JIJINREWARD jijinreward;
    Dictionary<int, int> rewardMap = new Dictionary<int, int>();
    
    public override void Init(UIEventListener handle)
    {
        //msp_flag = Get<UISprite>("sp_flag");
         msp_icon = Get<UISprite>("sp_icon"); 
         mobj_check = Get<UISprite>("check");  
         mlb_point = Get<UILabel>("lb_point");  
         mobj_select = Get<GameObject>("select");
         mobj_bg =  Get<GameObject>("bg");
         mobj_text =  Get<GameObject>("obj_text");  
         obj_process =  Get<GameObject>("process"); 
         effect =  Get<GameObject>("effect"); 
         mlb_num =  Get<UILabel>("lb_num");
        //rewardidList = CSlifeTimeFundInfo.Instance.GetRewardList();
        //lifelongFundInfo = CSlifeTimeFundInfo.Instance.LifelongFundInfo;
    }
    
    public override void Bind(object data)
    {
        rewardClassData rewardclass = data as rewardClassData;
        if (rewardclass == null) return;
        lifelongFundInfo = CSlifeTimeFundInfo.Instance.LifelongFundInfo;
        
        bool IsMaxItem = rewardclass.isMax;
        mobj_text.SetActive(IsMaxItem);
        mobj_check.gameObject.SetActive(!IsMaxItem);
        mlb_point.gameObject.SetActive(!IsMaxItem);
        mobj_select.gameObject.SetActive(!IsMaxItem);
        msp_icon.gameObject.SetActive(!IsMaxItem);
        effect.SetActive(!IsMaxItem);
        mlb_num.gameObject.SetActive(!IsMaxItem);
        if (IsMaxItem)
        {
            UIEventListener.Get(mobj_bg).onClick = null;
            return;
        }
        
        //rewardClassData rewardclass = rewardidList[realIndex];
        int id = rewardclass.id;
        int score = rewardclass.score;
        
        if (JijinRewardTableManager.Instance.TryGetValue(id, out jijinreward))
        {
            msp_icon.spriteName = ItemTableManager.Instance.GetItemIcon(jijinreward.reward);
        }

        bool isReceived = lifelongFundInfo.isBuy&&!lifelongFundInfo.unreceivedRewards.Contains(score) && score <= lifelongFundInfo.curPoint; //已领取
        mobj_check.gameObject.SetActive(isReceived);

        bool isReceive = lifelongFundInfo.unreceivedRewards.Contains(score);

        bool isMoreScore = rewardclass.score <= rewardclass.GetScore;
        mobj_select.SetActive(isReceive);
        //根据是否购买显示特效
        effect.SetActive(lifelongFundInfo.isBuy ? isReceive:isMoreScore);
        mlb_point.text = score.ToString();
        msp_icon.color = isReceived ? Color.black : Color.white; 
        obj_process.SetActive(isMoreScore);
        mlb_num.text = jijinreward.num.ToString();
        //mred.SetActive(isReceive);
        //GameObject obj = utility
        UIEventListener.Get(mobj_bg,rewardclass).onClick = OnReceiveRewardClick;
    }
    
    private void OnReceiveRewardClick(GameObject obj)
    {
        
        if (UIEventListener.Get(obj).parameter is rewardClassData rewardClassData)
        {
            if (lifelongFundInfo.isBuy == false && rewardClassData.score <= rewardClassData.GetScore)
            {
                UtilityTips.ShowRedTips(1318);
                return;
            }
            
            int score = rewardClassData.score;
            if (lifelongFundInfo!=null&&!lifelongFundInfo.unreceivedRewards.Contains(score))
            {
                rewardMap.Clear();
                if (JijinRewardTableManager.Instance.TryGetValue(rewardClassData.id, out jijinreward))
                {
                    rewardMap.Add(jijinreward.reward,jijinreward.num);
                    UIManager.Instance.CreatePanel<UIUnsealRewardPanel>(f =>
                    {
                        (f as UIUnsealRewardPanel).Show(rewardMap);
                    });
                }
            }
            
            if (score != 0)
            {
                Net.CSReceiveFundRewardMessage(score);
            }
        }
    }
    
    public override void OnDestroy()
    {
        msp_icon = null;
        mobj_check = null;
        mlb_point = null;
        mobj_select = null;
        mobj_bg = null;
        mobj_text = null;
        obj_process = null;
        effect = null;
        mlb_num = null;
        lifelongFundInfo = null;
    }
    
}
