using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RelivePanelType
{
    /// <summary>
    /// 原地复活和安全点复活
    /// </summary>
    BornpointAndInplace = 0,

    /// <summary>
    /// 安全点复活
    /// </summary>
    Bornpoint = 1,

    /// <summary>
    /// 指定地点复活
    /// </summary>
    Destination = 2,
}

public enum KillerType
{
    Player = 1, //玩家
    Monster = 2, //怪物
    Pet = 7, //宠物
}

public enum ReqInplaceReliveType
{
    HasFree,
    NoHasFree,
    // UseYuanBao,
}

/// <summary>
/// 复活选项界面
/// </summary>
public partial class UIRelivePanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get => false;
    }

    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.Special;
    }

    TABLE.MAPINFO configMap; //地图配置
    ReliveData myReliveData;

    ReqInplaceReliveType inplaceReliveType;

    int time = 0;
    private bool isNormalMap = false;

    public override void Init()
    {
        base.Init();
        mClientEvent.Reg((uint) CEvent.ReliveMainPlayer, ClosePanel);
        mbtn_bornpoint.onClick = OnClickBornPoint;
        mbtn_inplace.onClick = OnClickInPlace;
        mbtn_instance.onClick = OnClickInstance;
        mbtn_redName.onClick = OnClickRedName;
    }

    void ClosePanel(uint id, object data)
    {
        if (data == null) return;
        Close();
    }

    public override void Show()
    {
        base.Show();
        InitData();
    }

    void InitData()
    {
        myReliveData = CSReliveInfo.Instance.MyReliveData;
        if (myReliveData == null) return;
        if (!MapInfoTableManager.Instance.TryGetValue(CSMainPlayerInfo.Instance.MapID, out configMap)) return;

        CSEffectPlayMgr.Instance.ShowUITexture(mbgtitle, "relive_title");
        mgridbtns.SetActive(true);
        mobj_auto_relive.SetActive(false);
        SetKillerInfo();
        //红名优先级最高(只显示原地复活和红名村复活)
        if (CSMainPlayerInfo.Instance.IsRedNameResurrection() /*true*/) //红名
        {
            HandleRedName();
        }
        else
        {
            if (configMap.type == 0) //普通地图
            {
                HandleNormalMap(configMap.relivePanelType);
            }
            else //如果是副本
            {
                HandleInstance(InstanceTableManager.Instance.GetInstanceType(CSMainPlayerInfo.Instance.MapID));
            }
        }
    }

    /// <summary>
    /// 处理所有非特殊副本方式
    /// </summary>
    void HandleNormalMap(int type)
    {
        isNormalMap = true;
        switch ((RelivePanelType) type)
        {
            case RelivePanelType.BornpointAndInplace:
                HandleBornpointAndInplace();
                break;
            case RelivePanelType.Bornpoint:
                HandleBornpoint();
                break;
            //case RelivePanelType.Destination://指定地点复活暂时不做
            //    HandleDestination();
            // break;
            default:
                break;
        }
    }

    /// <summary>
    /// 处理副本复活
    /// </summary>
    void HandleInstance(int type)
    {
        isNormalMap = false;
        switch (type) //（后续会不断添加特殊副本类型）
        {
            case 5: //极限挑战副本
                HandleExtremeChallenge();
                break;
            default: //其他走的还是普通地图逻辑，只是安全点复活的UI显示成离开副本
                HandleNormalMap(configMap.relivePanelType);
                break;
        }
    }

    /// <summary>
    /// 处理极限挑战副本原地复活信息
    /// </summary>
    void HandleExtremeChallenge()
    {
        mbtn_instance.gameObject.SetActive(true);
        mbtn_bornpoint.gameObject.SetActive(false);
        SetBtnBornpoint();
        //处理极限挑战原地复活按钮
        int reliveNum = CSUltimateInfo.Instance.GetUltimateReliveCount();
        mbtn_inplace.gameObject.SetActive(reliveNum > 0);
        if (reliveNum > 0)
        {
            string[] arrContent = mlb_inplace.FormatStr.Split('#');
            if (arrContent != null && arrContent.Length == 2)
            {
                mlb_inplace.text = CSString.Format(arrContent[0], reliveNum);
            }
        }

        mbtn_redName.gameObject.SetActive(false);

        mgridbtns.GetComponent<UIGrid>().repositionNow = true;
        mgridbtns.GetComponent<UIGrid>().Reposition();
    }

    /// <summary>
    /// 原地复活和安全复活两种处理
    /// </summary>
    void HandleBornpointAndInplace()
    {
        mbtn_inplace.gameObject.SetActive(true); //原地复活
        SetBtnInplace();
        mbtn_instance.gameObject.SetActive(configMap.type != 0); //离开副本
        mbtn_bornpoint.gameObject.SetActive(configMap.type == 0); //安全点复活
        mbtn_redName.gameObject.SetActive(false);
        SetBtnBornpoint();
        mgridbtns.GetComponent<UIGrid>().repositionNow = true;
        mgridbtns.GetComponent<UIGrid>().Reposition();
    }

    /// <summary>
    /// 只有安全点复活选项
    /// </summary>
    void HandleBornpoint()
    {
        mbtn_inplace.gameObject.SetActive(false);
        mbtn_instance.gameObject.SetActive(configMap.type != 0);
        mbtn_bornpoint.gameObject.SetActive(configMap.type == 0);
        mbtn_redName.gameObject.SetActive(false);
        SetBtnBornpoint();
        mgridbtns.GetComponent<UIGrid>().repositionNow = true;
        mgridbtns.GetComponent<UIGrid>().Reposition();
    }

    /// <summary>
    /// 自动倒计时指定地点复活
    /// </summary>
    void HandleDestination()
    {
        mgridbtns.SetActive(false);
        mobj_auto_relive.SetActive(true);
        SetKillerInfo();
        SetAutoReliveInfo();
    }

    /// <summary>
    /// 处理红名
    /// </summary>
    void HandleRedName()
    {
        mbtn_inplace.gameObject.SetActive(true); //原地复活
        SetBtnInplace();
        mbtn_instance.gameObject.SetActive(false);
        mbtn_bornpoint.gameObject.SetActive(false);
        mbtn_redName.gameObject.SetActive(true); //红名村复活
        SetBtnBornpoint();
        mgridbtns.GetComponent<UIGrid>().repositionNow = true;
        mgridbtns.GetComponent<UIGrid>().Reposition();
    }

    /// <summary>
    /// 设置自动倒计时复活信息
    /// </summary>
    void SetAutoReliveInfo()
    {
        string[] arrContent;
        arrContent = configMap.reliveCoord.Split('#');
        if (arrContent.Length != 3) return;
        int[] arrIntContent = Array.ConvertAll(arrContent, int.Parse);
        time = arrIntContent[2];
        if (time <= 0) return;
        ScriptBinder.InvokeRepeating(0f, 1f, OnDesParticleAutoReliveInfo);
        mlb_place_relive.text = CSString.Format(632, arrIntContent[0], arrIntContent[1]);
    }

    void OnDesParticleAutoReliveInfo()
    {
        if (time < 0)
        {
            Net.ReqReliveMessage(ReliveType.BornPoint, false);
            ScriptBinder.StopInvokeRepeating();
        }
        else
        {
            mlb_time.text = time.ToString();
            time--;
        }
    }

    /// <summary>
    /// 设置安全点复活按钮
    /// </summary>
    void SetBtnBornpoint()
    {
        try
        {
            time = Convert.ToInt32(SundryTableManager.Instance.GetSundryEffect(112));
        }
        catch (Exception)
        {
            throw;
        }

        if (time <= 0) return;
        ScriptBinder.InvokeRepeating2(0f, 1f, OnDesParticle);
    }

    void OnDesParticle()
    {
        if (time < 0)
        {
            Net.ReqReliveMessage(ReliveType.BornPoint, false);
            ScriptBinder.StopInvokeRepeating2();
        }
        else
        {
            UILabel lb;
            if (CSMainPlayerInfo.Instance.IsRedNameResurrection() /*true*/)
            {
                lb = mlb_redName;
            }
            else
            {
                lb = configMap.type == 0 ? mlb_bornpoint : mlb_instance;
            }

            lb.text = CSString.Format(1277, time);
            time--;
        }
    }

    
    ILBetterList<int> listConfig = new ILBetterList<int>();
    ILBetterList<int> listReliveCount = new ILBetterList<int>(); //存放剩余可复活次数
    /// <summary>
    /// 设置原地复活按钮
    /// </summary>
    void SetBtnInplace()
    {
        listConfig.Clear();
        // List<int> listConfig = UtilityMainMath.SplitStringToIntList(SundryTableManager.Instance.GetSundryEffect(111));
        //加上VIP免费次数
        int rebornCount = VIPTableManager.Instance.GetVIPReborn(CSMainPlayerInfo.Instance.VipLevel);
        if (rebornCount > 0)
        {
            for (int i = 0; i < rebornCount; i++)
            {
                listConfig.Add(0);
            }
        }
        
        listReliveCount.Clear();
        for (int i = 0; i < listConfig.Count; i++)
        {
            if (i > myReliveData.playerDie.reliveCount - 1)
            {
                listReliveCount.Add(listConfig[i]);
            }
        }

        if (listReliveCount.Count <= 0)//没有免费次数
        {
            mobj_yuanbao.SetActive(false);
            mlb_inplace.gameObject.SetActive(true);
            mobj_mask.SetActive(true);
            mlb_inplace.text = CSString.Format(2022, 0);
            inplaceReliveType = ReqInplaceReliveType.NoHasFree;
            // mlb_inplace.text = CSString.Format(1278);
        }
        else//还有免费次数
        {
            // if (listReliveCount[0] == 0) 
            // {
                mobj_yuanbao.SetActive(false);
                mlb_inplace.gameObject.SetActive(true);
                mobj_mask.SetActive(false);
                int freeCount = 0;
                for (int i = 0; i < listReliveCount.Count; i++)
                {
                    // if (listReliveCount[i] == 0)
                    // {
                        freeCount++;
                    // }
                }

                mlb_inplace.text = CSString.Format(1275, freeCount);
                inplaceReliveType = ReqInplaceReliveType.HasFree;
            // }
            // else //只能用元宝买活
            // {
            //     mobj_yuanbao.SetActive(true);
            //     mlb_inplace.gameObject.SetActive(false);
            //     mobj_mask.SetActive(false);
            //     long myJInBi = CSBagInfo.Instance.GetMoneyCount((int) MoneyType.gold);
            //     int cost = listReliveCount[0];
            //     mlb_num_yuanbao.text = cost.ToString()
            //         .BBCode(myJInBi >= cost ? ColorType.SecondaryText : ColorType.ToolTipUnDone);
            //
            //     inplaceReliveType = ReqInplaceReliveType.UseYuanBao;
            // }
        }
    }

    /// <summary>
    /// 设置击杀者信息
    /// </summary>
    void SetKillerInfo()
    {
        if (CSMainPlayerInfo.Instance.MapID == (int)ESpecialMap.DiXiaXunBao)//如果是地下寻宝
        {
            mlb_title.text = CSString.Format(2039);
        }
        else
        {
            switch ((KillerType) myReliveData.playerDie.killlerType)
            {
                case KillerType.Player:
                    mlb_title.text = CSString.Format(1274, myReliveData.playerDie.killerName);
                    break;
                case KillerType.Monster:
                    mlb_title.text = CSString.Format(1273, myReliveData.playerDie.killerName);
                    break;
                case KillerType.Pet: //被宠物杀死服务器返回宠物归属者的名字
                    mlb_title.text = CSString.Format(1274, myReliveData.playerDie.killerName);
                    break;
                default:
                    break;
            }
        }
        
        mlb_title.gameObject.SetActive(true);
    }

    /// <summary>
    /// 点击安全点复活
    /// </summary>
    /// <param name="go"></param>
    void OnClickBornPoint(GameObject go)
    {
        Net.ReqReliveMessage(ReliveType.BornPoint, false);
        ScriptBinder.StopInvokeRepeating();
        ScriptBinder.StopInvokeRepeating2();
    }

    /// <summary>
    /// 点击原地复活
    /// </summary>
    /// <param name="go"></param>
    void OnClickInPlace(GameObject go)
    {
        if (isNormalMap)
        {
            switch (inplaceReliveType)
            {
                case ReqInplaceReliveType.HasFree:
                    Net.ReqReliveMessage(ReliveType.InPlace, false);
                    break;
                case ReqInplaceReliveType.NoHasFree:
                    UtilityTips.ShowRedTips(2021);
                    break;
                // case ReqInplaceReliveType.UseYuanBao:
                    // Net.ReqReliveMessage(ReliveType.InPlace, true);
                    // break;
                default:
                    break;
            }
        }
        else
        {
            //特殊副本原地复活只有免费一种
            Net.ReqReliveMessage(ReliveType.InPlace, false);
        }
    }

    /// <summary>
    /// 点击离开副本
    /// </summary>
    /// <param name="go"></param>
    void OnClickInstance(GameObject go)
    {
        Net.ReqReliveMessage(ReliveType.BornPoint, false);
        ScriptBinder.StopInvokeRepeating();
        ScriptBinder.StopInvokeRepeating2();
    }

    /// <summary>
    /// 点击红名村复活
    /// </summary>
    /// <param name="go"></param>
    void OnClickRedName(GameObject go)
    {
        Net.ReqReliveMessage(ReliveType.BornPoint, false);
        ScriptBinder.StopInvokeRepeating();
        ScriptBinder.StopInvokeRepeating2();
    }

    public override void OnHide()
    {
        base.OnHide();
        ScriptBinder.StopInvokeRepeating();
        ScriptBinder.StopInvokeRepeating2();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mbgtitle);
        base.OnDestroy();
    }
}