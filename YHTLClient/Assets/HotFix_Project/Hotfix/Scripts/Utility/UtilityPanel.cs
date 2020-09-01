using System;
using System.Collections.Generic;
using UnityEngine;

public class UtilityPanel
{
    /// <summary>
    /// 通过GameModel表 打开面板
    /// </summary>
    /// <param name="panelId"></param>
    /// <returns></returns>
    public static bool JumpToPanel(int panelId, System.Action<UIBase> actionPanelOpen = null)
    {
        TABLE.GAMEMODELS gamemodels;
        if (!GameModelsTableManager.Instance.TryGetValue(panelId, out gamemodels))
            return false;

        if (!UICheckManager.Instance.DoCheckButtonClick(gamemodels.functionId))
            return false;

        //特殊情况判断
        if (panelId == 15000) { Net.CSFloorInfoMessage(); return true; } //GameModle,石阵寻宝
        else if (panelId == 18000)
        {
            Net.CSLianTiFieldMessage();
            UIManager.Instance.CreatePanel<UILianTiLandPanel>();
            return true;
        } //GameModle,炼体之地
        else if (panelId == 40000)//封印
        {
            if (CSSealGradeInfo.Instance.MySealData != null)
            {
                UIManager.Instance.CreatePanel<UISealCombinedPanel>(
                    p => (p as UISealCombinedPanel).SelectChildPanel(1));
                HotManager.Instance.EventHandler.SendEvent(CEvent.GameModelSealPanel);
            }
            else
            {
                UtilityTips.ShowRedTips(1987);
            }
            return true;
        }
        else if (panelId == 40001)//幻境
        {
            if (CSDreamLandInfo.Instance.MyDreamLandData != null)
            {
                UIManager.Instance.CreatePanel<UISealCombinedPanel>(
                    p => (p as UISealCombinedPanel).SelectChildPanel(2));
                HotManager.Instance.EventHandler.SendEvent(CEvent.GameModelDreamPanel);
            }
            else
            {
                UtilityTips.ShowRedTips(1988);
            }
            return true;
        }

        if (!string.IsNullOrEmpty(gamemodels.model))
        {
            Type type = Type.GetType(gamemodels.model);
            UIManager.Instance.CreatePanel(type, f =>
            {
                if (gamemodels.layer > 0)
                {
                    if (gamemodels.subLayer > 0)
                        f.SelectChildPanel(gamemodels.layer, gamemodels.subLayer);
                    else
                        f.SelectChildPanel(gamemodels.layer);
                }
            }, actionPanelOpen);
        }

        return true;
    }


    public static bool CheckGameModelPanelIsThis<T>(int panelId) where T : UIBase
    {
        TABLE.GAMEMODELS gamemodels;
        if (!GameModelsTableManager.Instance.TryGetValue(panelId, out gamemodels)) return false;
        if (!string.IsNullOrEmpty(gamemodels.model))
        {
            Type type = Type.GetType(gamemodels.model);
            if (type == typeof(T)) return true;
        }
        return false;
    }


    static List<Type> exceptList;
    /// <summary>
    /// 打开功能页面时，忽略的关闭面板  Resident层面板默认都不关闭，不需要写到此处
    /// </summary>
    public static List<Type> OpenFuncExceptList
    {
        get
        {
            if (exceptList == null)
            {
                exceptList = new List<Type>()
                {
                    typeof(UIDebugPanel),
                    typeof(UIChatPanel),
					typeof(UIRandomBtnPanel),					//随机夺宝按钮面板
					typeof(UIGuidePanel),
                    typeof(UINoticePanel),
                    typeof(UINoticeSecondPanel),
                    typeof(UINoticeBottomPanel),
                    typeof(UINoticeColoursPanel),
                    typeof(UINoticeBelowPanel),
                    typeof(UIUpdateExitGamePanel),
                };
            }
            return exceptList;
        }
    }



    public static void ShowCompleteWay(string getWayStr, Vector2 worldPos, AnchorType anchor,int _titleType)
    {
        UIManager.Instance.CreatePanel<UICompleteWayPanel>((f) =>
        {
            (f as UICompleteWayPanel).OpenPanel(getWayStr, worldPos, anchor, _titleType);
        });
    }


    public static void ShowCompleteWayWithSelfAdapt<T>(string getWayStr, T target, AnchorType precedenceAnchor = AnchorType.TopCenter, int _titleType = 0) where T : UIWidget
    {
        UIManager.Instance.CreatePanel<UICompleteWayPanel>((f) =>
        {
            (f as UICompleteWayPanel).OpenPanelSelfAdapt(getWayStr, target, precedenceAnchor, _titleType);
        });
    }
}