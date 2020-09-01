using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIItemFriendShortInfoData : IndexedItem
{
    public int Index { get; set; }
    public social.FriendInfo Info;
    public System.Action<long, string> m_call;
    public System.Action<long, string> m_refusecall;
    public int number;
    public bool canChat;
}

public class UIItemFriendShortInfoBinder : UIBinder
{
    protected UILabel lb_name;
    protected UILabel lb_career;
    protected UILabel lb_level;
    protected GameObject m_Bg;
    protected UILabel btn_add;
    protected UISprite sp_Head;
    protected UISprite sp_Nation;
    protected UILabel btn_refuse;

    protected UIItemFriendShortInfoData data;

    public override void Init(UIEventListener handle)
    {
        lb_name = Get<UILabel>("lb_name");
        lb_career = Get<UILabel>("lb_career");
        lb_level = Get<UILabel>("lb_level");
        m_Bg = Handle.transform.Find("background").gameObject;
        btn_add = Get<UILabel>("btn_add/Label");
        sp_Head = Get<UISprite>("headitem");
        sp_Nation = Get<UISprite>("sp_nation");
        btn_refuse = Get<UILabel>("btn_refuse/Label");
        UIEventListener.Get(btn_add.parent.gameObject).onClick = OnClickAddFriendBtn;
        if (btn_refuse != null)
        {
            UIEventListener.Get(btn_refuse.parent.gameObject).onClick = OnClickRefuseFriendBtn;
        }
    }

    public override void Bind(object data)
    {
        this.data = data as UIItemFriendShortInfoData;
        if(null != this.data)
        {
            //name
            if (null != lb_name)
                lb_name.text = this.data.Info.name;
            //level
            if(null != lb_level)
            {
                lb_level.text = CSString.Format(570, this.data.Info.level);
            }
            //career
            if (null != lb_career)
                lb_career.text = Utility.GetCareerName(this.data.Info.career);
            //bg
            if(null != m_Bg)
                m_Bg.SetActive((this.data.number & 1) == 0);
            //head_icon
            if (null != sp_Head && null != this.data && null != this.data.Info)
            {
                sp_Head.spriteName = Utility.GetPlayerIcon(this.data.Info.sex,this.data.Info.career);
                sp_Head.color = this.data.Info.isOnline ? Color.white : Color.black;
            }
            //nation
            if (null != sp_Nation)
                sp_Nation.gameObject.SetActive(false);
            //chatlabel
            //if (this.data.canChat)
            //{
            //    btn_add.text = CSString.Format(571);
            //}
            //else
            //{
            //    btn_add.text = CSString.Format(572);
            //}
        }
    }

    public override void OnDestroy()
    {
        lb_name = null;
        lb_career = null;
        lb_level = null;
        btn_add = null;
        sp_Head = null;
        sp_Nation = null;
        btn_refuse = null;
    }

    private void OnClickAddFriendBtn(GameObject go)
    {
        if (null == data || null == data.Info)
            return;

        if(null != data.m_call && !string.IsNullOrEmpty(data.Info.name))
        {
            data.m_call(data.Info.roleId, data.Info.name);
        }
    }

    private void OnClickRefuseFriendBtn(GameObject go)
    {
        if (null == data || null == data.Info)
            return;

        if (null != data.m_refusecall && !string.IsNullOrEmpty(data.Info.name))
        {
            data.m_refusecall(data.Info.roleId, data.Info.name);
        }
    }
}
