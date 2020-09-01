using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FuncOpenTableManager : TableManager<TABLE.FUNCOPENARRAY, TABLE.FUNCOPEN, int, FuncOpenTableManager>
{
    Dictionary<int,List<TABLE.FUNCOPEN>> mLevel2OpenIds = new Dictionary<int,List<TABLE.FUNCOPEN>>(32);
    bool mInit;

    void TryInitFuncOpenDic()
    {
        if(null != array)
        {
            if (mInit)
                return;
            mInit = true;
            var arr = array.gItem.handles;
            for (int i = 0, max = arr.Length; i < max; ++i)
            {
                var item = arr[i].Value as TABLE.FUNCOPEN;

                if (!mLevel2OpenIds.ContainsKey(item.needLevel))
                {
                    mLevel2OpenIds.Add(item.needLevel, new List<TABLE.FUNCOPEN>(4));
                }
                var list = mLevel2OpenIds[item.needLevel];
                list.Add(item);
            }
        }
    }

    public List<TABLE.FUNCOPEN> GetNewFuncOpen(int level)
    {
        TryInitFuncOpenDic();
        if (mLevel2OpenIds.ContainsKey(level))
            return mLevel2OpenIds[level];
        return null;
    }

    public TABLE.FUNCOPEN GetTabByModleName(int funcId)
    {
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.FUNCOPEN;
            if (item.functionId == funcId)
            {
                return item;
            }
        }

        return null;
    }


    public int GetIdByModleName(int funcId)
    {
        TABLE.FUNCOPEN funcopen;
        funcopen = GetTabByModleName(funcId);
        if (funcopen != null)
            return funcopen.id;
        return -1;
    }
}