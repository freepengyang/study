using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionTaskWayItem : UIBinder
{
    private UILabel label;
    private int type;
    private string param;
    private string[] sunList;
    private int firstRechargeModel;
    private UISprite sp_flag;
    
    
    public override void Init(UIEventListener handle)
    {
        label = Get<UILabel>("Label");
        sp_flag = Get<UISprite>("sp_flag");
        handle.onClick = ItemClick;
        firstRechargeModel = CSVipInfo.Instance.GetFirstRechargeGameModel();
    }

    public override void Bind(object data)
    {
        
        
        string sundry = SundryTableManager.Instance.GetSundryEffect((int) data);
        sunList = sundry.Split('#');
        if(sunList.Length < 3) return;
        
        sp_flag.gameObject.SetActive(int.Parse(sunList[2]) == firstRechargeModel);
        label.text = CSString.Format(int.Parse(sunList[0]));
        int.TryParse(sunList[1], out type);
        param = sunList[2];
    }

    void ItemClick(GameObject _go)
    {
        if (type == 1)
        {
            List<int> transferGroup = UtilityMainMath.SplitStringToIntList(param, '_');
            if (transferGroup != null && transferGroup.Count > 0)
            {
                int indexs = UnityEngine.Random.Range(0, transferGroup.Count);
                UtilityPath.FindWithDeliverIdFight(transferGroup[indexs]);
            }
        }else if (type == 2)
        {
            int gameId;
            if (int.TryParse(param, out gameId))
            {
                UtilityPanel.JumpToPanel(gameId);
            }
        }else if (type == 3)
        {
            int npcId;
            if (int.TryParse(param, out npcId))
            {
                UtilityPath.FlyToNpc(npcId);
            }
        }
        HotManager.Instance.EventHandler.SendEvent(CEvent.HideMissionTips);
    }
    
    public override void OnDestroy()
    {
        label = null;
        param = "";
        sunList = null;
    }
}