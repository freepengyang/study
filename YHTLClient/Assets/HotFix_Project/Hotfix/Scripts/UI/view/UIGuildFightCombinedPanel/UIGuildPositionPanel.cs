using UnityEngine;

public partial class UIGuildPositionPanel : UIBasePanel
{
    FastArrayMeta<int> mPosList = new FastArrayMeta<int>(8);

    public override void Init()
	{
		base.Init();

        mBtnClose.onClick = this.Close;
	}

    int[] PosList = new int[]
    {
        (int)GuildPos.President,
        (int)GuildPos.VicePresident,
        (int)GuildPos.Presbyter,
        (int)GuildPos.Elite,
        (int)GuildPos.Member,
    };

    MenuInfo mMenuInfo;

    public void Show(MenuInfo info)
	{
        mMenuInfo = info;
        mPosList.Clear();
        int myPosition = (int)CSMainPlayerInfo.Instance.GuildPos;
        for (int i = 0; i < PosList.Length; i++)
        {
            int v = PosList[i];
            //相同职位不可调整
            if(v == info.position)
            {
                continue;
            }

            //会长可调整任何职位包括会长
            if(myPosition == (int)GuildPos.President)
            {
                mPosList.Add(v);
            }
            else if(v > myPosition)
            {
                //其他可任命比他小一级职位
                mPosList.Add(v);
            }
        }
        mGrildList.MaxCount = mPosList.Count;

        for (int i = 0; i < mPosList.Count; i++)
        {
            int pos = mPosList[i];
            var gp = mGrildList.controlList[i];
            var btn = gp.transform.Find("btn").gameObject;
            //UISprite sprite = btn.GetComponent<UISprite>();
            //if (sprite != null)
            //    sprite.spriteName = (CSMainPlayerInfo.Instance.GuildPos > pos || pos == info.position) ? "btn_small" : "btn_small";
            UILabel lb = btn.transform.Find("Label").GetComponent<UILabel>();
            if (lb != null) 
                lb.text = CSGuildInfo.Instance.GetGuildPos(pos);
            UIEventListener.Get(btn,pos).onClick = OnPosClick;
        }
    }

    private void OnPosClick(GameObject button)
    {
        if (!(button.GetComponent<UIEventListener>().parameter is int pos))
            return;

        if(pos < CSMainPlayerInfo.Instance.GuildPos)
        {
            UtilityTips.ShowRedTips(960);
            this.Close();
            return;
        }

        if(pos == mMenuInfo.position)
        {
            UtilityTips.ShowRedTips(961);
            this.Close();
            return;
        }

        if (pos != (int)GuildPos.President)
        {
            UtilityTips.ShowPromptWordTips(55, null, () =>
            {
                Net.CSChangeUnionPositionMessage(mMenuInfo.roleId, pos);
            }, mMenuInfo.roleName, CSGuildInfo.Instance.GetGuildPos(pos));
            this.Close();
            return;
        }

        if (!mMenuInfo.isOnline)
        {
            UtilityTips.ShowRedTips(962);
            this.Close();
            return;
        }

        UtilityTips.ShowPromptWordTips(56, () =>
         {
             Net.CSUnionChangePresidentMessage(mMenuInfo.roleId);
         });

        this.Close();
    }

    protected override void OnDestroy()
	{
        mPosList?.Clear();
        mPosList = null;

        base.OnDestroy();
	}
}
