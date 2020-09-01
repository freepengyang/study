using System.Collections;
using UnityEngine;

public partial class UIBloodMaskPanel : UIBasePanel
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
    }

    public override void Show()
    {
        base.Show();
        coroutine = ScriptBinder.StartCoroutine(DelayShow());
    }
    protected override void OnDestroy()
    {
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
        UIManager.Instance.ClosePanel<UIBloodMaskPanel>();
    }
}
