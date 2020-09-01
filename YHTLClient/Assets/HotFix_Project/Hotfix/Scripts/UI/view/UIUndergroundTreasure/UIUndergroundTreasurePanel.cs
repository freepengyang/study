using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUndergroundTreasurePanel : UIActivityBasePanel
{

    TABLE.INSTANCE data;
    public override void Show()
    {
        base.Show();
        data = InstanceTableManager.Instance.GetTableDataByType(14)[0];
        
        RefreshItem(data.show);
        RefreshUI(5);
        
        
    }

    public override void EnterClick(GameObject go)
    {

        int second = 0;
        if (CSReliveInfo.Instance.MyReliveData != null)
            second = (int)((CSServerTime.Instance.TotalMillisecond - CSReliveInfo.Instance.MyReliveData.playerDie.dieTime) / 1000);
        int interval = int.Parse(SundryTableManager.Instance.GetSundryEffect(451));

        if (data.mapId == CSReliveInfo.Instance.MapIdDeath && second < interval)
        {
            UtilityTips.ShowRedTips(CSString.Format(1022, interval - second));
        }
        else {
            Close();
            //跳转到地下寻宝
            Net.ReqEnterInstanceMessage(InstanceTableManager.Instance.GetInstanceMapId(data.id));
        }
    }

}
