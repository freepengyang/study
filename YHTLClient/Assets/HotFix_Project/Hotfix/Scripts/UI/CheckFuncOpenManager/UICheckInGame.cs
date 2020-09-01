using System;
using System.Collections.Generic;

public class UICheckInGame : UICheckBase
{
    public override void ShowTips(int fType)
    {
        base.ShowTips(fType);
        string tips = TipsByTable(fType);

        if (!string.IsNullOrEmpty(tips))
            UtilityTips.ShowRedTips(tips);
    }

    private string TipsByTable(int id)
    {
        string strTips = String.Empty;
        if (FuncOpenTableManager.Instance.TryGetValue(id, out mtbFuncData))
        {
            if (!CheckByLevel(mtbFuncData.needLevel))
            {
                strTips = CSString.Format(510, mtbFuncData.functionName, mtbFuncData.needLevel);
            }
            else if(!CheckByOpenDay(mtbFuncData.openday))
            {
                strTips = CSString.Format(1171, mtbFuncData.functionName, mtbFuncData.openday);
            }
            else if (mtbFuncData.needType == 2 && !CheckByWing(Int32.Parse(mtbFuncData.typeParam)))
            {
                strTips = CSString.Format(2000,  mtbFuncData.typeParam);
            }
            else
            {
                strTips = mtbFuncData.tips;
            }
            
            //现在功能开放由服务器判断了，，前端无脑显示
            /*else if (!string.IsNullOrEmpty(mtbFuncData.needtype))
            {
                List<List<int>> conditionList = UtilityMainMath.SplitStringToIntLists(mtbFuncData.needtype);
                if (conditionList == null || conditionList.Count == 0) return strTips;
                for (int i = 0; i < conditionList.Count; i++)
                {
                    if (conditionList[i].Count != 2) continue;
                    strTips = ConditionValue((FuncOpenCondition) conditionList[i][0], conditionList[i][1]);
                    if (!string.IsNullOrEmpty(strTips)) break;
                }
            }*/
        }

        return strTips;
    }
}