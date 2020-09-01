using System.Collections;
using UnityEngine;

public partial class UITotemPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }
    public override UILayerType PanelLayerType
    {
        get { return UILayerType.Tips; }
    }
    WaitForSeconds wait;
    Coroutine coroutine;
    public override void Init()
    {
        base.Init();
        mClientEvent.AddEvent(CEvent.BreathRedMask, GetMaskStateChange);
        CSEffectPlayMgr.Instance.ShowUITexture(mtex_bg, mtex_bg.name);
    }

    public override void Show()
    {
        base.Show();
        coroutine = ScriptBinder.StartCoroutine(DelayShow());
    }

    protected override void OnDestroy()
    {
        CSEffectPlayMgr.Instance.Recycle(mtex_bg);
        ScriptBinder.StopCoroutine(coroutine);
        base.OnDestroy();
    }
    void GetMaskStateChange(uint id, object data)
    {
        ScriptBinder.StopCoroutine(coroutine);
        coroutine = ScriptBinder.StartCoroutine(DelayShow());
    }
    IEnumerator DelayShow()
    {
        if (wait == null) { wait = new WaitForSeconds(10f); }
        yield return wait;
        UIManager.Instance.ClosePanel<UITotemPanel>();
    }
}
