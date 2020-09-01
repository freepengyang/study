using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class CSNetTip : CSNetBase
{
    void ECM_ResTipMessage(NetInfo obj)
    {
        tip.TipResponse tip = Network.Deserialize<tip.TipResponse>(obj);
        UtilityTips.ShowTips(tip.msg);
        //Debug.LogError(tip.type + " --- " + tip.msg);
    }
    int vigorMul = 0;
    fight.BufferInfo JingLiDanbuffinfo;
    //int JingLiDanBuffId = 210019;
    int JingLiDanId = 610;
    TABLE.ITEM JingLiDanCfg;
    /// <summary>
    /// 信息栏变动消息  type1：道具变动
    /// </summary>
    /// <param name="obj"></param>
    void ECM_SCNotifyNoteInfoMessage(NetInfo obj)
    {
        tip.NotifyNoteInfo tip = Network.Deserialize<tip.NotifyNoteInfo>(obj);
        if (tip.type == 1) //经验变动 参数一经验  参数二精力经验
        {
            int exp1 = 0;
            int.TryParse(tip.parameters[0], out exp1);
            int energy = 0;
            int.TryParse(tip.parameters[1], out energy);
            if (exp1 != 0)
            {
                UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1312), exp1), true);
            }
            if (energy != 0)
            {
                JingLiDanbuffinfo = CSMainPlayerInfo.Instance.BuffInfo.GetBuff(210019);
                if (JingLiDanCfg == null) { JingLiDanCfg = ItemTableManager.Instance.GetItemCfg(JingLiDanId); }
                if (vigorMul == 0) { vigorMul = int.Parse(SundryTableManager.Instance.GetSundryEffect(95)); }
                if (JingLiDanbuffinfo != null)
                {
                    UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1313), vigorMul * (1 + (JingLiDanCfg.data * 0.0001))), true);
                }
                else
                {
                    UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1313), vigorMul), true);
                }
                UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1314), energy), true);
            }
        }
        else if (tip.type == 2)
        {
            int exp1 = 0;
            int.TryParse(tip.parameters[0], out exp1);
            //Debug.Log(exp + " 泡点经验  ");
            UtilityTips.ShowCenterRight(1, string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(858), exp1));
        }
        else if (tip.type == 3)
        {
            int exp1 = 0;
            int.TryParse(tip.parameters[0], out exp1);
            int exp2 = 0;
            int.TryParse(tip.parameters[1], out exp2);
            if (exp1 != 0)
            {
                UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1700), exp1), true);
            }
            if (exp2 != 0)
            {
                JingLiDanbuffinfo = CSMainPlayerInfo.Instance.BuffInfo.GetBuff(210019);
                if (JingLiDanCfg == null) { JingLiDanCfg = ItemTableManager.Instance.GetItemCfg(JingLiDanId); }
                if (vigorMul == 0) { vigorMul = int.Parse(SundryTableManager.Instance.GetSundryEffect(95)); }
                if (JingLiDanbuffinfo != null)
                {
                    UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1701), vigorMul * (1 + (JingLiDanCfg.data * 0.0001))), true);
                }
                else
                {
                    UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1701), vigorMul), true);
                }
                UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1702), exp2), true);
            }
        }
        else if (tip.type == 4)
        {
            int exp1 = 0;
            int.TryParse(tip.parameters[0], out exp1);
            if (exp1 != 0)
            {
                UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1706), exp1));
            }
        }
        else if (tip.type == 5)//vip经验
        {
            int exp1 = 0;
            int.TryParse(tip.parameters[0], out exp1);
            if (exp1 != 0)
            {
                UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1863), exp1));
            }
        }
        else if (tip.type == 6)//玛法战令
        {
            int exp1 = int.Parse(tip.parameters[0]);
            if (exp1 != 0)
            {
                UtilityTips.LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1862), exp1));
            }
        }
        else if (tip.type == 7)//精力值增加
        {
            int exp1 = 0;
            int.TryParse(tip.parameters[0], out exp1);
            if (exp1 != 0)
            {
                UtilityTips.LeftDownTips(12, exp1, 622);
            }
        }
    }
    void ECM_TipNtfMessage(NetInfo obj)
    {
        tip.TipNtf res = Network.Deserialize<tip.TipNtf>(obj);
        if (res == null)
        {
            FNDebug.LogErrorFormat("ECM_TipNtfMessage 解析失败");
            return;
        }
        TABLE.TIP tb = null;
        if (!TipTableManager.Instance.TryGetValue(res.id, out tb))
        {
#if UNITY_EDITOR
            UtilityTips.ShowGreenTips(string.Format("Tip表中没有ID={0}TIPS 请相关策划填写", res.id));
            UtilityTips.ShowGreenTips(string.Format("Tip表中没有ID={0}TIPS 请相关策划填写", res.id));
            UtilityTips.ShowGreenTips(string.Format("Tip表中没有ID={0}TIPS 请相关策划填写", res.id));
            UtilityTips.ShowGreenTips(string.Format("Tip表中没有ID={0}TIPS 请相关策划填写", res.id));
            UtilityTips.ShowGreenTips(string.Format("Tip表中没有ID={0}TIPS 请相关策划填写", res.id));
#endif
            return;
        }

        if (string.IsNullOrEmpty(tb.context))
        {
#if UNITY_EDITOR
            UtilityTips.ShowGreenTips(string.Format("Tip表中ID={0} Tips 未填写内容 请相关策划填写", res.id));
            UtilityTips.ShowGreenTips(string.Format("Tip表中ID={0} Tips 未填写内容 请相关策划填写", res.id));
            UtilityTips.ShowGreenTips(string.Format("Tip表中ID={0} Tips 未填写内容 请相关策划填写", res.id));
            UtilityTips.ShowGreenTips(string.Format("Tip表中ID={0} Tips 未填写内容 请相关策划填写", res.id));
            UtilityTips.ShowGreenTips(string.Format("Tip表中ID={0} Tips 未填写内容 请相关策划填写", res.id));
#endif
            return;
        }

        string context = string.Empty;
        try
        {
            context = string.Format(tb.context, res.parameters.ToArray());
        }
        catch (Exception e)
        {
            if (null == res.parameters)
            {
#if UNITY_EDITOR
                UtilityTips.ShowGreenTips(string.Format("Tip表中ID={0} 服务器返回参数错误 parameters is null 请相关策划填写", res.id));
                UtilityTips.ShowGreenTips(string.Format("Tip表中ID={0} 服务器返回参数错误 parameters is null 请相关策划填写", res.id));
                UtilityTips.ShowGreenTips(string.Format("Tip表中ID={0} 服务器返回参数错误 parameters is null 请相关策划填写", res.id));
                UtilityTips.ShowGreenTips(string.Format("Tip表中ID={0} 服务器返回参数错误 parameters is null 请相关策划填写", res.id));
                UtilityTips.ShowGreenTips(string.Format("Tip表中ID={0} 服务器返回参数错误 parameters is null 请相关策划填写", res.id));
                FNDebug.LogError(e);
#else
                Debug.LogErrorFormat("Tip表中ID={0} 服务器返回参数错误 parameters is null 请相关策划填写", res.id);
                Debug.LogErrorFormat("Tip表中ID={0} Exception = {1}", res.id,e.Message);
#endif
            }
            return;
        }
        switch (res.type)
        {
            case 0:
                UtilityTips.ShowTips(context);
                break;
            case 2:
                UtilityTips.ShowTips(context, 1.5f, ColorType.White);
                break;
            case 3:
                FNDebug.LogErrorFormat("ShowCenterMoveUpInfo TipsView 3", context);
                //UtilityTips.ShowCenterMoveUpInfo(context, 1.5f, ColorType.Red);
                break;
            case 4:
                FNDebug.LogErrorFormat("ShowCenterMoveUpInfo TipsView 4", context);
                //UtilityTips.ShowCenterMoveUpInfo(context, 1.5f, ColorType.Green);
                break;
            case 5:
                //传送失败注销cspathfindermanager中协议
                //CSPathFinderManager.Instance.mSocketHandler.UnRegMsg((uint)CEvent.Scene_EnterSceneAfter);
                FNDebug.LogErrorFormat("传送失败注销cspathfindermanager中协议", context);
                FNDebug.LogErrorFormat(context);
                //CSPathFinderManager.Instance.MissionGuideState = MissionGuideState.None;
                //UtilityTips.ShowTips(context);
                break;
            case 6: //李林颖新需求，为了寻宝界面对称，增加新提示
                    //UtilityTips.ShowSeekTreasureTips(context, 1.5f, ColorType.Green);
                break;
            case 7://主界面左下角
                //FNDebug.LogErrorFormat("主界面左下角", context);
                //UtilityTips.ShowLeftDownTips(context);
                break;
            case 8://技能飘字相关
                UtilityTips.LeftDownTips(context, true);
                break;
            default:
                UtilityTips.ShowTips(context, 1.5f, ColorType.Green);
                break;
        }
    }


    private void ECM_ResBulletinMessage(NetInfo obj)
    {
        tip.BulletinResponse info = Network.Deserialize<tip.BulletinResponse>(obj);

        CSNoticeManager.Instance.ResBulletinMessage(info, obj);
    }
}
