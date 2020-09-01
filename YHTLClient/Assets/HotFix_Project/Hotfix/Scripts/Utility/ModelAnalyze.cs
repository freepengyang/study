using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAnalyze 
{
    public static void GetPetBody(TABLE.MONSTERINFO tbl, int state, int avatarType,ref int bodyID, ref int weaponID)
    {
        if (avatarType == EAvatarType.ZhanHun)
        {
            TABLE.ZHANHUNSUIT tblZhanHunSuit = Utility.GetZhanHunSuit(tbl.id);
            if (tblZhanHunSuit != null)
            {
                bodyID = tblZhanHunSuit.clothesmodel;
                weaponID = tblZhanHunSuit.weaponmodel;
                
            }
        }
        else
        {
            bodyID = tbl.model;
            if (state == EPetChangeState.Fight)
            {
                TABLE.CHANGEMODEL tblChangeModel = null;
                if (ChangeModelTableManager.Instance.TryGetValue(tbl.id, out tblChangeModel))
                {
                    if (tblChangeModel.model > 0)
                    {
                        bodyID = tblChangeModel.model;
                    }
                }
            }
        }
    }
}
