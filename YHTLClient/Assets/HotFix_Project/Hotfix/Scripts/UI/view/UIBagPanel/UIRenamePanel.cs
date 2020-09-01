using UnityEngine;

public partial class UIRenamePanel : UIBasePanel
{
    public override UILayerType PanelLayerType { get { return UILayerType.Tips; } }
    public override void Init()
    {
        base.Init();
        UIEventListener.Get(mbtn_close).onClick = CloseClick;
        UIEventListener.Get(mobj_bg).onClick = CloseClick;
        UIEventListener.Get(mbtn_confirm).onClick = ConfirmClick;
        UIEventListener.Get(mbtn_random).onClick = RandomClick;
        mClientEvent.AddEvent(CEvent.Rename, GetRenameMesBack);
        mClientEvent.AddEvent(CEvent.ResRandomRoleNameMessage, OnReceiveRandomRoleNameMessage);
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }


    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIRenamePanel>();
    }
    void ConfirmClick(GameObject _go)
    {
        if (!string.IsNullOrEmpty(mlb_name.value))
        {
            Net.CSUpdateNameMessage(mlb_name.value);
        }
        else
        {
            UtilityTips.ShowTips(301);
        }
    }
    void RandomClick(GameObject _go)
    {
        Net.ReqRandomRoleName(CSMainPlayerInfo.Instance.Sex);
    }

    void GetRenameMesBack(uint id, object data)
    {
        bool result = (bool)data;
        if (result)
        {
            UIManager.Instance.ClosePanel<UIRenamePanel>();
        }
    }
    void OnReceiveRandomRoleNameMessage(uint id, object data)
    {
        if (data == null) return;
        user.RandomRoleNameResponse m_random = (user.RandomRoleNameResponse)(data);
        if (mlb_name != null) mlb_name.value = m_random.name;
    }
}
