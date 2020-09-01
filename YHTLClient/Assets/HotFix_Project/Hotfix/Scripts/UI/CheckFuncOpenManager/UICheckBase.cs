using System.Collections.Generic;
using TABLE;
using UnityEngine;

public class UICheckBase
{
    /// <summary>
    /// 特殊类型开启条件
    /// </summary>
    protected enum FuncOpenCondition
    {
        None,
    }

    protected enum UnOpenState
    {
        None = 0,
        Hide = 1,
        ShowTips = 2,
    }


    protected TABLE.FUNCOPEN mtbFuncData = null;

    public virtual bool CheckFunction(int funcType)
    {
        bool sCheck = CheckSpecialType(funcType);
        //bool tCheck = CheckByTable(funcType);

        return sCheck;
    }

    /// <summary>
    /// 功能是否隐藏
    /// </summary>
    public virtual bool IsConfigHideBtn(int funcType)
    {
        if (FuncOpenTableManager.Instance.TryGetValue(funcType, out mtbFuncData))
        {
            if (mtbFuncData != null && mtbFuncData.showtype == (int) UnOpenState.Hide)
            {
                return true;
            }
        }

        return false;
    }

    public virtual void SetBtnIsShow(int fType, GameObject obj, bool state)
    {
        if (obj != null)
            obj.SetActive(state);
    }

    public virtual void ShowTips(int fType)
    {
    }


    /// <summary>
    /// 需要特殊处理的按钮，例如下载有礼，账号绑定
    /// </summary>
    /// <param name="id"></param>
    private bool CheckSpecialType(int id)
    {
        bool state = true;
        if (CSScene.IsLanuchMainPlayer)
        {
            switch (id)
            {
                default: break;
            }
        }

        return state;
    }

    /// <summary>
    /// 通过表格配置检测功能
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool CheckByTable(FunctionType id)
    {
        if (FuncOpenTableManager.Instance.TryGetValue((int) id, out mtbFuncData))
        {
            bool bolLevel = CheckByLevel(mtbFuncData.needLevel);
            bool bolOpenDay = CheckByOpenDay(mtbFuncData.openday);
            //bool condition = CheckByCondition(mtbFuncData.needType, mtbFuncData.typeParam);
            return bolLevel & bolOpenDay;
        }

        return true;
    }

    protected bool CheckByLevel(int configLevel)
    {
        if (CSMainPlayerInfo.Instance != null)
            return CSMainPlayerInfo.Instance.Level >= configLevel;
        return false;
    }

    protected bool CheckByOpenDay(int configOpenDay)
    {
        if (configOpenDay == 0) return true;
        if (CSMainPlayerInfo.Instance != null)
            return CSMainPlayerInfo.Instance.ServerOpenDay >= configOpenDay;
        return false;
    }

    protected bool CheckByWing(int level)
    {
        if (level == 0) return true;
        if (CSWingInfo.Instance.MyWingData != null)
            return WingTableManager.Instance.GetWingRank(CSWingInfo.Instance.MyWingData.id) >= level;

        return false;
    }

    /// <summary>
    /// 处理表中特殊配置
    /// </summary>
    protected bool CheckByCondition(int type, string condition)
    {
        if (type == 0 || string.IsNullOrEmpty(condition)) return true;
        bool result = true;
        //暂时没有特殊情况
        /*switch ((FuncOpenCondition) type)
        {
            default:
                return true;
        }*/
        return result;
    }
}