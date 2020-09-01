using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIFriendItemBinderData
{
    public social.FriendInfo friendInfo;
    public int itemId;
    public bool showRedPoint;
    public System.Action<social.FriendInfo> action;
    public bool isON;
}
public class UIFriendItemBinder : UIBinder {
    private UISprite HeadSprite;
    private UILabel Level;
    private UILabel Name;
    private UISprite heartSprite;
    private UILabel heartLabel;
    private UISprite Nation;
    private GameObject debt;
    private UILabel active;
    private GameObject RedDot;
    private GameObject Removebtn;
    private UIEventListener btnHeartTips;
    private GameObject PrisonIcon;
    private UIToggle mToggle;
    int ItemID;
    social.FriendInfo friendInfo;
    System.Action<social.FriendInfo> action;

    public override void Init(UIEventListener handle)
    {
        mToggle = handle.GetComponent<UIToggle>();
        HeadSprite = Get<UISprite>("headitem");
        Level = Get<UILabel>("lb_level/lb_skill_lv");
        Name = Get<UILabel>("lb_name");
        heartSprite = Get<UISprite>("heart/Sprite");
        heartLabel = Get<UILabel>("heart/Label");
        Nation = Get<UISprite>("sp_nation");
        debt = handle.transform.Find("debt").gameObject;
        active = Get<UILabel>("active");
        RedDot = handle.transform.Find("reddot").gameObject;
        Removebtn = handle.transform.Find("removebtn").gameObject;
        btnHeartTips = UIEventListener.Get(heartSprite.gameObject);
        btnHeartTips.onClick = OnClickTips;
        PrisonIcon = handle.transform.Find("sp_prison").gameObject;
    }

    public override void Bind(object data)
    {
        UIFriendItemBinderData gData = data as UIFriendItemBinderData;
        if(null != gData)
        {
            mToggle.Set(gData.isON);
            Handle.onClick = this.OnClick;
            this.action = gData.action;
            gData.action = null;
            RefreshUI(gData.friendInfo, gData.itemId, gData.showRedPoint);
        }
    }

    protected void OnClick(GameObject go)
    {
        if(null != this.friendInfo)
        {
            this.action?.Invoke(this.friendInfo);
        }
    }

    public override void OnDestroy()
    {
        HeadSprite = null;
        Name = null;
        heartSprite = null;
        heartLabel = null;
        Nation = null;
        debt = null;
        active = null;
        RedDot = null;
        Removebtn = null;
        btnHeartTips = null;
        PrisonIcon = null;
        ItemID = 0;
    }

    protected void OnClickTips(GameObject go)
    {
        if (null != this.friendInfo)
        {
            if(this.friendInfo.relation == (int)FriendType.FT_FRIEND)
                UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.HeartValue);
            else
                UIHelpTipsPanel.CreateHelpTipsPanel(HelpType.DebtValue);
        }
    }

    protected void RefreshUI(social.FriendInfo info,int ItemID,bool IsShowRed=false)
    {
        this.friendInfo = info;
        if (info==null)
        {
            ResetUI();
        }
        else
        {
            heartLabel.transform.localPosition = new Vector3(15, -3, 0);
            this.ItemID = ItemID;
            debt.SetActive(false);
            Removebtn.SetActive(false);
            active.text = "";
            if (HeadSprite!=null)
            {
                HeadSprite.spriteName = Utility.GetPlayerIcon(info.sex, info.career);
                HeadSprite.color = info.isOnline ? Color.white : Color.black;
            }

            var relation = (FriendType)info.relation;
            //Nation.spriteName = string.Format("{0}{1}", "country", info.nationId);
            Nation.gameObject.SetActive(false);
            heartSprite.gameObject.SetActive(info.relation == 1|| info.relation == 2);
            //if (heartSpriteTips != null) heartSpriteTips.Id = HelpTipsCtrl.HelpType.None;
            if (relation == FriendType.FT_FRIEND)
            {
                //if (heartSpriteTips != null) heartSpriteTips.Id = HelpTipsCtrl.HelpType.HeartHelp;       
                heartSprite.spriteName = "friend_intimate";
                heartLabel.text = info.friendLove.ToString().BBCode(ColorType.ProperyColor);
                Name.text = info.name;
            }
            else if (relation == FriendType.FT_ENEMY)
            {
                //if (heartSpriteTips != null) heartSpriteTips.Id = HelpTipsCtrl.HelpType.HatredHelp;
                heartSprite.spriteName = "friend_hater";
                heartLabel.text = info.enemy.ToString().BBCode(ColorType.ProperyColor);
                debt.SetActive(false);
                active.text = string.Empty;//info.yaBiao ? CSString.Format(101941) : "";
                Name.text = CSString.Format(636,info.name,string.Empty);
            }
            else if (relation == FriendType.FT_BLACK_LIST)
            {
                heartSprite.spriteName = "";
                heartLabel.text = "";
                Removebtn.SetActive(true);
                Name.text = info.name;
            }
            else if (relation == FriendType.FT_NONE)
            {
                heartSprite.spriteName = "";
                heartLabel.text = CSString.Format(610);
                heartLabel.transform.localPosition = new Vector3(-10, -3,0);
                Name.text = info.name;
            }

            if(null != Level)
            {
                Level.text = $"{info.level}";
            }
            //else if (info.relation == 7)
            //{
            //    heartSprite.spriteName = "";
            //    heartLabel.text = CSString.Format(101943);
            //    heartLabel.transform.localPosition = new Vector3(-10, -3, 0);
            //}
            heartSprite.MakePixelPerfect();
            RedDot.SetActive(IsShowRed);
            PrisonIcon.SetActive(false);
        }

        UIEventListener.Get(HeadSprite.gameObject).onClick = OnFriendClick;
        UIEventListener.Get(Removebtn).onClick = OnClickRemoveEnemyBtn;
        //UIEventListener.Get(PrisonIcon).onClick = (p) => { UtilityTips.ShowRedTips(105355); };
    }
  
    private void ResetUI()
    {
        this.action = null;
        this.friendInfo = null;
        HeadSprite.spriteName = "";
        Name.text = "";
        heartSprite.spriteName = "";
        heartLabel.text = "";
        Nation.spriteName = "";
        debt.SetActive(false);
        active.text = "";
        RedDot.SetActive(false);
    }
    private void OnFriendClick(GameObject gp)
    {
        social.FriendInfo info = this.friendInfo;
        MenuInfo data = new MenuInfo();
        data.SetFriendTips(info.roleId, info.name, info.enemy, info.sex, ItemID, info.level, info.career, 0, 0);
        data.sundryId = 375;
        UIManager.Instance.CreatePanel<UIRoleSelectionMenuPanel>((f) =>
        {
            (f as UIRoleSelectionMenuPanel).ShowSelectData(data);
        });
    }

    private void OnClickRemoveEnemyBtn(GameObject go)
    {
        if(null != this.friendInfo)
        {
            var req = CSNetRepeatedFieldPool.Get<long>();
            req.Clear();
            req.Add(friendInfo.roleId);
            Net.ReqDeleteRelationMessage(req);
        }
    }
}
