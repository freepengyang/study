using System;
using System.Collections.Generic;
using TABLE;

public partial class JijinRewardTableManager
{
    private List<int> loopList = new List<int>();
    
    public List<int> GetLoopList()
    {
        if (loopList.Count > 0)
        {
            return loopList;
        }
        var arr = array.gItem.handles;
        for (int i = 0, max = arr.Length; i < max; ++i)
        {
            var item = arr[i].Value as TABLE.JIJINREWARD;
            if (item.loopSign > 0)
            {
                loopList.Add(arr[i].key);
            }
        }

        return loopList;
    }
    
    
    
    public int GetIdByScore(int score)
    {
        int ingental = int.Parse(SundryTableManager.Instance.GetSundryEffect(629));
        
        //int tempid = score / ingental;
        List<int> loopList = GetLoopList();
        int maxintegral = GetJijinRewardNeedIntegral(loopList[loopList.Count-1]); 
        
        while (score > maxintegral)
        {
            score -= ingental * loopList.Count;
        }

        var jijin = array.gItem.handles.FirstOrNull(x => { return (x.Value as TABLE.JIJINREWARD).needIntegral == score; });
        if (jijin != null)
        {
            return (jijin.Value as TABLE.JIJINREWARD).id;
        }
        else
        {
            return 0;
        }

    }



}