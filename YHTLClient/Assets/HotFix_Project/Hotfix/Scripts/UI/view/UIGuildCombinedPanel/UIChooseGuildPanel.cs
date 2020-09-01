using UnityEngine;

public partial class UIChooseGuildPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override void Init()
	{
		base.Init();

        mbtn_allapply.onClick = OnAllApply;
        mbtn_close.onClick = this.Close;

        mClientEvent.AddEvent(CEvent.OnJoinedGuildSucceed, OnJoinFamily);
    }

    union.canApplyUnions recomList;
    public void Show(union.canApplyUnions unionList)
    {
        if (recomList != null)
        {
            return;
        }
        recomList = unionList;
        for (int i = 0; i < unionList.union.Count; i++)
        {
            InitItem(unionList.union[i], i);
        }
        mContent.repositionNow = true;
        //CSAudioMgr.PlayFirstGuideAudio(21);
    }

    void InitItem(union.canApplyUnion union, int index)
    {
        //  if (union.nation !=CSAvatarManager.MainPlayerInfo.Nation) return; 删除非本国的推荐
        GameObject item = NGUITools.AddChild(mContent.gameObject, mItemPrefab);
        item.name = "item_" + index;
        item.transform.Find("name").GetComponent<UILabel>().text = union.name;
        UIEventListener.Get(item.transform.Find("btn_Join").gameObject, union.id).onClick = OnApplyGuild;
        item.SetActive(true);
    }
    /// <summary>
    /// 申请公会
    /// </summary>
    /// <param name="obj"></param>
    private void OnApplyGuild(GameObject obj)
    {
        if (obj == null) 
            return;

        if(UIEventListener.Get(obj).parameter is long id)
        {
            Net.CSApplyUnionMessage(id);
        }
    }

    private void OnAllApply(GameObject obj)
    {
        for (int i = 0; i < recomList.union.Count; i++)
        {
            Net.CSApplyUnionMessage(recomList.union[i].id);
        }
        this.Close();
    }

    private void OnJoinFamily(uint eventId, object argv)
    {
        this.Close();
    }

    protected override void OnDestroy()
    {
        mClientEvent.RemoveEvent(CEvent.OnJoinedGuildSucceed, OnJoinFamily);
        base.OnDestroy();
    }
}