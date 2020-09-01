using UnityEngine;
public partial class UINewSkillPanel : UIBasePanel
{
    public override bool ShowGaussianBlur
    {
        get { return false; }
    }

    public override void Init()
    {
        base.Init();
        ScriptBinder.Invoke(ScriptBinder.GetIntArgv(0), OnDelayFade);
    }

    protected void OnDelayFade()
    {
        var tweens = ScriptBinder.GetComponentsInChildren<TweenAlpha>();
        for (int i = 0; i < tweens.Length; ++i)
        {
            tweens[i].enabled = true;
            tweens[i].ResetToBeginning();
            tweens[i].PlayForward();
            if (i == 0)
            {
                EventDelegate.Add(tweens[i].onFinished, OnPlayFinish);
            }
        }
    }

    protected string mFmtPath = @"bottom_right/Root/UIMainSkillPanel/right_bottom/skillTurntable/skills/initiativeSkill{0}";
    protected void OnPlayFinish()
    {
        if (null != mSkillItem)
        {
            Vector3 current = mtweenPosition.from;

            var panel = UIManager.Instance.GetPanel<UIMainSceneManager>();
            if (null != panel && slotId >= 1 && slotId <= 6)
            {
                var child = panel.UIPrefab.transform.Find(string.Format(mFmtPath,this.slotId));
                if (null != child)
                {
                    current = mtweenPosition.transform.parent.InverseTransformPoint(child.transform.position);
                }
            }

            mtweenPosition.to = current;
            mtweenPosition.enabled = true;
            mtweenPosition.ResetToBeginning();
            mtweenPosition.PlayForward();
            float v = 0.8f;
            mtweenScale.to = new Vector3(v, v, v);
            mtweenScale.enabled = true;
            mtweenScale.ResetToBeginning();
            mtweenScale.PlayForward();

            EventDelegate.Add(mtweenPosition.onFinished, this.Close);
        }
    }

    public override void Show()
    {
        base.Show();
    }

    TABLE.SKILL mSkillItem;
    int slotId;
    public void Show(TABLE.SKILL skillItem,int slotId)
    {
        this.slotId = slotId;
        mSkillItem = skillItem;
        if (null != mSkillItem)
        {
            if (null != mSkillName)
            {
                mSkillName.text = mSkillItem.name;
            }
            if (null != mSkillIcon)
            {
                mSkillIcon.spriteName = skillItem.icon;
            }
        }
    }

    protected override void OnDestroy()
    {
        mSkillItem = null;
        base.OnDestroy();

        CSNewFunctionUnlockManager.Instance.Poped = false;
        CSNewFunctionUnlockManager.Instance.TriggerNextAction();
    }
}
