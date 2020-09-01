using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 显示在组合信息中的迷你预览图 </summary>
public class UISignCardMiniItem : UIBinder
{
    
    UISprite sp_bg;
    UISprite sp_frame;
    UISprite sp_card;
    GameObject obj_select;
    GameObject obj_mask;

    public override void Init(UIEventListener handle)
    {
        sp_bg = Get<UISprite>("sp_bg");
        sp_frame = Get<UISprite>("sp_frame");
        sp_card = Get<UISprite>("sp_card");
        obj_select = Get<GameObject>("select");
        obj_mask = Get<GameObject>("sp_mask");
    }

    public override void Bind(object data)
    {
        MiniCardSlot mData = data as MiniCardSlot;
        sp_bg.gameObject.SetActive(string.IsNullOrEmpty(mData.icon));
        sp_card.spriteName = mData.icon;
        sp_frame.spriteName = CSSignCardInfo.Instance.GetMiniCardsFrameSp(mData.quality);
        //obj_select.SetActive(mData.isActive);
        obj_select.SetActive(false);
        //sp_card.color = mData.isActive ? CSColor.white : Color.black;
        obj_mask.SetActive(!mData.isActive);
    }

    public override void OnDestroy()
    {
        sp_bg = null;
        sp_frame = null;
        sp_card = null;
        obj_select = null;
        obj_mask = null;
    }
}
