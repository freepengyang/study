using shop;
using ultimate;

public class UltimateTransferNPCOperation : SpecialNpcOperationBase
{
    public override bool DoSpecial(CSAvatar avatar)
    {
        UltimatePassInfo info = CSUltimateInfo.Instance._UltimatePassInfo;
        if (info != null)
        {
            if (info.level >= CSUltimateInfo.Instance.GetTotalLevel())
            {
                JustLeave();
                return true;
            }


            if (info.showCard)
            {
                ultimate.SelectAdditionEffect msg = CSUltimateInfo.Instance._SelectAdditionEffect;
                if (msg == null || (msg.additionAttrs != null && msg.additionAttrs.Count > 0))
                {
                    UtilityTips.ShowPromptWordTips(16, () => { UtilityPath.FindNpc(205); });
                    return true;
                }
            }

            if (info.showShopNpc)
            {
                ShopInfoResponse _ShopInfoResponse = CSUltimateInfo.Instance._ShopInfoResponse;
                if (_ShopInfoResponse == null ||
                    _ShopInfoResponse.shopItemBuyInfos != null && _ShopInfoResponse.shopItemBuyInfos.Count == 0)
                {
                    UtilityTips.ShowPromptWordTips(17, () => { UtilityPath.FindNpc(203); },
                        () => { GoNext(); }
                    );
                    return true;
                }
            }           

        }

        GoNext();

        return true;
    }

    private void GoNext()
    {
        UtilityTips.ShowPromptWordTips(15, () =>
            {
                Net.ReqLeaveInstanceMessage(true);
            },
            () =>
            {
                Net.ReqEnterNextInstanceMessage();
            }
        );
    }


    /// <summary>
    /// 达到顶层，只能离开
    /// </summary>
    void JustLeave()
    {
        UtilityTips.ShowPromptWordTips(94, () =>
        {
            Net.ReqLeaveInstanceMessage(true);
        }
        );
    }
}