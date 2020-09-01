using System.Collections.Generic;
using UnityEngine;

public partial class UINewPlayerPlacePanel : UIActivityBasePanel
{
    int instanceId = 300025;

    public override void Show()
    {
        base.Show();
        if(InstanceTableManager.Instance.TryGetValue(instanceId,out TABLE.INSTANCE instanceItem))
        {
            RefreshItem(instanceItem.show);
        }
    }

    public override void EnterClick(GameObject obj)
    {
        UIManager.Instance.ClosePanel<UINewPlayerPlacePanel>();
        if (InstanceTableManager.Instance.TryGetValue(instanceId, out TABLE.INSTANCE instanceItem))
            Net.ReqEnterInstanceMessage(instanceItem.mapId);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
