using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameModelsTableManager
{

    public int GetFuncIdByModleName(string panelName)
    {
        if (null == array || null == array.gItem)
            return -1;

        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            TABLE.GAMEMODELS tbl = arr[i].Value as TABLE.GAMEMODELS;
            if (tbl.model == panelName)
            {
                return tbl.functionId;
            }
        }
        return -1;
    }

}