using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSWaitFrameDealManager : CSInfo<CSWaitFrameDealManager>
{
    public Dictionary<GameObject, CSWaitFrameDeal> AnchorToWaitDealDic = new Dictionary<GameObject, CSWaitFrameDeal>();

    public override void Dispose()
    {
        if (AnchorToWaitDealDic != null)
        {
            var dic = AnchorToWaitDealDic.GetEnumerator();
            while (dic.MoveNext())
            {
                CSWaitFrameDeal a = dic.Current.Value;
                a.Release();
            }
            AnchorToWaitDealDic.Clear();
        }
    }
}
