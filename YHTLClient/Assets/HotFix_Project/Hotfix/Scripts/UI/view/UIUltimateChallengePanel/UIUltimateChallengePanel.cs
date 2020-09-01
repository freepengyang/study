public partial class UIUltimateChallengePanel : UIBasePanel
{
    public override PrefabTweenType PanelTweenType
    {
        get => PrefabTweenType.FirstPanel;
    }
    public enum Challenge
    {
        UltimateChallenge = 1,
    }

    public override void Init()
    {
        base.Init();
        AddCollider();
        UIEventListener.Get(mbtn_close).onClick = Close;
        RegChildPanel<UIChallengePanel>((int)Challenge.UltimateChallenge, mUIChallengePanel.gameObject,mto_challenge);
    }
}