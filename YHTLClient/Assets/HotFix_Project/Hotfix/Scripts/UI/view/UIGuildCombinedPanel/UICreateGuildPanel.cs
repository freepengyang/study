using UnityEngine;

public partial class UICreateGuildPanel : UIBasePanel
{
    int womaHornId = 30042037;
    public override void Init()
    {
        base.Init();

        bool hasWomaHorn = womaHornId.IsItemEnough(1, 0, false);

        if (null != mlb_requirements)
        {
            mlb_requirements.text = CSString.Format(747, CSGuildInfo.Instance.CreateGuildNeedVip).BBCode(!hasWomaHorn && CSMainPlayerInfo.Instance.VipLevel < CSGuildInfo.Instance.CreateGuildNeedVip ? ColorType.Red : ColorType.Green);
        }

        if (null != mlb_need_golds)
        {
            if(!hasWomaHorn)
            {
                long owned = CSGuildInfo.Instance.GoldCount;
                int need = CSGuildInfo.Instance.CreateBuildMoneyCost;
                bool enough = owned >= need;
                mlb_need_golds.text = $"{need}".BBCode(enough ? ColorType.Green : ColorType.Red);
            }
            else
            {
                long owned = womaHornId.GetItemCount();
                int need = 1;
                bool enough = owned >= need;
                mlb_need_golds.text = $"{need}".BBCode(enough ? ColorType.Green : ColorType.Red);
            }
        }

        if (null != mMoneyIcon)
        {
            if(!hasWomaHorn)
            {
                mMoneyIcon.spriteName = CSGuildInfo.Instance.MoneyId.SmallIcon();
            }
            else
            {
                mMoneyIcon.spriteName = womaHornId.SmallIcon();
            }
        }

        if (null != mbtnMoney)
            mbtnMoney.onClick = TryShowGetWay;

        if (null != mbtn_create)
            mbtn_create.onClick = OnCreateClick;
        if(null != mbtn_close)
            mbtn_close.onClick = this.OnCloseClick;
        if (null != mbtn_bg)
            mbtn_bg.onClick = this.OnCloseClick;
        if (null != mBtnGetHorn)
            mBtnGetHorn.onClick = f =>
            {
                Utility.ShowGetWay(womaHornId);
            };
        mBtnGetHorn.CustomActive(!hasWomaHorn);
    }

    protected bool TryShowMoneyGetWay()
    {
        bool hasWomaHorn = womaHornId.IsItemEnough(1, 0, false);
        if(!hasWomaHorn)
        {
            long owned = CSGuildInfo.Instance.GoldCount;
            int need = CSGuildInfo.Instance.CreateBuildMoneyCost;
            bool enough = owned >= need;
            if (!enough)
            {
                Utility.ShowGetWay(CSGuildInfo.Instance.MoneyId);
                return true;
            }

            return false;
        }

        return false;
    }

    public void TryShowGetWay(GameObject go)
    {
        TryShowMoneyGetWay();
    }

    public void OnCreateClick(GameObject go)
    {
        if (mchatInput == null) return;

        if (string.IsNullOrEmpty(mchatInput.value))
        {
            UtilityTips.ShowRedTips(748);
            return;
        }

        if (mchatInput.value.Length >= 10)
        {
            UtilityTips.ShowRedTips(788);
            return;
        }

        //如果有沃玛号角直接创建行会
        bool hasWomaHorn = womaHornId.IsItemEnough(1, 0, false);
        if(hasWomaHorn)
        {
            Net.CSCreateUnionMessage(mchatInput.value);
            UIManager.Instance.ClosePanel<UICreateGuildPanel>();
            return;
        }

        //如果没有沃玛号角 需要判断VIP等级 否则不需要
        if (!hasWomaHorn && CSMainPlayerInfo.Instance.VipLevel < CSGuildInfo.Instance.CreateGuildNeedVip)
        {
            UtilityTips.ShowPromptWordTips(81, () => { },() =>
            {
                UIManager.Instance.ClosePanel<UICreateGuildPanel>();
                UIManager.Instance.ClosePanel<UIGuildCombinedPanel>();
                UIManager.Instance.CreatePanel<UIVIPPanel>();
            });
            //UtilityTips.ShowRedTips(747, CSGuildInfo.Instance.CreateGuildNeedVip);
            return;
        }

        if (TryShowMoneyGetWay())
        {
            return;
        }

        Net.CSCreateUnionMessage(mchatInput.value);
        UIManager.Instance.ClosePanel<UICreateGuildPanel>();
    }

    public override void Show()
    {
        base.Show();
    }

    public void OnCloseClick(GameObject go)
    {
        UIManager.Instance.ClosePanel<UICreateGuildPanel>(true);
    }
}