using UnityEngine;

public partial class UIWelfareActivationCodePanel : UIBasePanel
{
    public override void Init()
    {
        base.Init();
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, mtex_bg.name);
        UIEventListener.Get(mbtn_get).onClick = GetBtnClick;
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mtex_bg);
        base.OnDestroy();
    }

    void GetBtnClick(GameObject _go)
    {
        Net.CSCodeMessage(minput.value);
    }
}
