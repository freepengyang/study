using UnityEngine;

public partial class UIHonorPromotionPromptPanel : UIBasePanel
{
    string[] groupDes;
    public override void Init()
    {
        base.Init();
        groupDes = ClientTipsTableManager.Instance.GetClientTipsContext(1118).Split('#');
        UIEventListener.Get(mobj_bg).onClick = CloseClick;
        CSEffectPlayMgr.Instance.ShowUIEffect(mobj_effect, 17750);
    }
    public void SetData(int _tpye)
    {
        mlb_des1.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1134), groupDes[_tpye]);
        mlb_des2.text = string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(1133), groupDes[_tpye]);
    }
    public override void Show()
    {
        base.Show();
    }
    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mobj_effect);
        base.OnDestroy();
    }
    void CloseClick(GameObject _go)
    {
        UIManager.Instance.ClosePanel<UIHonorPromotionPromptPanel>();
    }
}
