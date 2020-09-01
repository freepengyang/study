using UnityEngine;

public class UIMissionHint : UIBase
{
    UILabel _lb_hint;

    public override void Init()
    {
        base.Init();

        _lb_hint = Get<UILabel>("lb_hint");
        mClientEvent.AddEvent(CEvent.Task_GoalUpdate, OnTaskChanged);
    }

    public override void Show()
    {
        base.Show();
        CheckHint();
    }

    protected void OnTaskChanged(uint id,object argv)
    {
        CheckHint();
    }

    protected void CheckHint()
    {
        var data = CSMissionManager.Instance.GetAcceptedHintedMainMission();
        if (null == data || null == data.TasksTab)
        {
            UIPrefabTrans.CustomActive(false);
            return;
        }

        if (string.IsNullOrEmpty(data.TasksTab.tip4))
        {
            UIPrefabTrans.CustomActive(false);
            return;
        }

        UIPrefabTrans.CustomActive(true);
        if (null != _lb_hint)
            _lb_hint.text = data.TasksTab.tip4;
    }
}