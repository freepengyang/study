using UnityEngine;
using System.Collections;

public class Item : UIBase , IDispose{
    private UISprite _bg;
    private UISprite mBg { get { return _bg ?? (_bg = Get<UISprite>("icon_bg")); } }
    private UISprite _icon;
    private UISprite mIcon { get { return _icon ?? (_icon = Get<UISprite>( "icon")); } }
    private UILabel _name;
    private UILabel mName { get { return _name ?? (_name = Get<UILabel>( "name")); } }
    private UILabel _id;
    private UILabel mId { get { return _id ?? (_id = Get<UILabel>( "id")); } }
    private UIInput _number;
    private UIInput mNumber { get { return _number ?? (_number = Get<UIInput>( "number")); } }
    private TABLE.ITEM idata;

    public void RefreshUI(TABLE.ITEM itemData) {
        idata = itemData;
        if (itemData == null) return;

        if (mBg != null) mBg.spriteName = ItemTableManager.Instance.GetItemQualityBG(itemData.quality); ;
        if (mIcon != null) mIcon.spriteName = itemData.icon;
        if (mName != null) mName.text = itemData.name;
        if (mId != null) mId.text = itemData.id.ToString();
        if (mNumber != null) mNumber.value = "1";
    }

    public override void Dispose()
    {
        base.Dispose();
        _icon = null;
        _name = null;
        _id = null;
        _number = null;
        _bg = null;
    }
}
