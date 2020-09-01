using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FunctionPromptManager : CSInfo<FunctionPromptManager>
{
    PoolHandleManager mPoolHandleManager;

    Dictionary<int, BubbleChecker> itemdic = new Dictionary<int, BubbleChecker>();
    CSBetterList<BubbleChecker> mRedPointCheckList = new CSBetterList<BubbleChecker>();

    public FunctionPromptManager()
    {
        mPoolHandleManager = new PoolHandleManager();
    }
    public void Init()
    {
        //RegisterChecker(new MailBubbleChecker());
        RegisterChecker(new FriendApplyBubbleChecker());
        RegisterChecker(new TeamApplyBubbleChecker());
        //RegisterChecker(new FriendPrivateChatBubbleChecker());
        RegisterChecker(new InviteUnionBubbleChecker());
        RegisterChecker(new TeamApplicationBubbleChecker());
        RegisterChecker(new UpdateGameBubbleChecker());
        RegisterChecker(new GuildCombatBubbleChecker());
        RegisterChecker(new AuctionSellBubbleChecker());
    }
    public override void Dispose()
    {
        var iter = itemdic.GetEnumerator();
        while (iter.MoveNext())
        {
            iter.Current.Value.Destroy();
        }
        itemdic.Clear();
        itemdic = null;
    }

    void RegisterChecker(BubbleChecker checker)
    {
        if (!itemdic.ContainsKey(checker.ID))
        {
            checker.Create(mClientEvent);
            itemdic.Add(checker.ID, checker);
        }
    }
    public Dictionary<int, BubbleChecker> GetFunctionPromptInfo()
    {
        return itemdic;
    }


    /// <summary>
    /// 限时活动预告气泡
    /// </summary>
    void RegisterTimelimitActivityChecker()
    {
        var dic = CSActivityInfo.Instance.GetTimeLimitBubbles();
        if (dic == null || dic.Count < 1) return;
        for (var it = dic.GetEnumerator(); it.MoveNext();)
        {
            var display = it.Current.Value;
            if (display == null || display.config == null) continue;
            var checker = new TimeLimitActivityBubbleChecker();
            checker.config = display.config;
            if (!itemdic.ContainsKey(checker.ID))
            {
                checker.Create(mClientEvent);
                itemdic.Add(checker.ID, checker);
            }
        }

        mClientEvent.SendEvent(CEvent.TimeLimitBubbleInit);
    }
}
