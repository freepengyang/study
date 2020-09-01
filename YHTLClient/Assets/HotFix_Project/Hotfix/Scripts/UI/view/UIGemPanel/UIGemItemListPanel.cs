using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bag;
using System;
using Google.Protobuf.Collections;
using TABLE;

public partial class UIGemItemListPanel : UIBasePanel
{
    private int subType;
    private int pos;
    public override void Init()
    {
        base.Init();
        AddCollider();
        UIEventListener.Get(mbtn_close).onClick = Close;
    }

    public void OpenPanel(ILBetterList<BagItemInfo> itemList,int subType,int pos) {
        this.subType = subType;
        this.pos = pos;
        mgrid.MaxCount = itemList.Count;
        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject obj = mgrid.controlList[i];
            UILabel mlb_name = UtilityObj.Get<UILabel>(obj.transform, "lb_name");
            UILabel mlb_attr = UtilityObj.Get<UILabel>(obj.transform, "lb_attr");
            UILabel mlb_recommend = UtilityObj.Get<UILabel>(obj.transform, "lb_recommend");
            UISprite spr_icon = UtilityObj.Get<UISprite>(obj.transform, "spr_icon");
            GEM gemTableData;
            GemTableManager.Instance.TryGetValue(itemList[i].configId ,out gemTableData);

            if (gemTableData == null)
            {
                FNDebug.LogError("表格配置数据错误,找不到对应id 请检查表格 id:" +itemList[i].configId );
                return;
            }

            int quality = ItemTableManager.Instance.GetItemQuality(gemTableData.id);
            mlb_name.text = UtilityColor.BBCode(gemTableData.name,quality) ;
            var listAttrInfo = CSGemInfo.Instance.GetAttrParaByCareer(CSMainPlayerInfo.Instance.Career, gemTableData);
           
            RepeatedField<CSAttributeInfo.KeyValue> attrItems = null;

            if (listAttrInfo.Count > 0)
            {
                attrItems = CSAttributeInfo.Instance.GetAttributes(mPoolHandleManager, listAttrInfo);
            }
            
            CSStringBuilder.Clear();
            mlb_attr.text = CSStringBuilder.Append(attrItems[0].Key, CSString.Format(999), attrItems[0].Value).ToString();
            CSStringBuilder.Clear();
            mlb_recommend.gameObject.SetActive(i == 0);
            spr_icon.spriteName = ItemTableManager.Instance.GetItemIcon(itemList[i].configId);
            UIEventListener.Get(mgrid.controlList[i], itemList[i]).onClick = GemItemClick;
        }

    }

    

    private void GemItemClick(GameObject obj)
    {
        BagItemInfo para = (BagItemInfo)UIEventListener.Get(obj).parameter;
        //Debug.Log(subType + "|" + pos + "|" + para.bagIndex);
        Net.CSEquipGemMessage(subType,pos, para.bagIndex);
        //发消息穿戴
        Close();
    }

    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

}
