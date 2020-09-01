using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CSNetPaodian : CSNetBase
{
    void ECM_ResUPaoDianChangeMessage(NetInfo _info)
    {
        var msg = Network.Deserialize<paodian.PaoDianChange>(_info);
        UtilityTips.ShowGreenTips(563);
        CSTimeExpManager.Instance.SetRankAndStar(msg.rank, msg.star);
    }

    void ECM_SCPaoDianInfoMessage(NetInfo _info)
    {
        var msg = Network.Deserialize<paodian.PaoDianChange>(_info);
        CSTimeExpManager.Instance.InitRankAndStar(msg.rank, msg.star);
    }
    void ECM_SCRandomPaoDianMessage(NetInfo _info)
    {
        var msg = Network.Deserialize<paodian.RandomPaoDian>(_info);
        //Debug.Log("收到随机泉水消息" + msg.nextRefreshTime);
        CSPaoDianInfo.Instance.SetPaoDianRandomInfo(msg);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCRandomPaoDianMessage, msg);
    }
    void ECM_SCPaoDianExpMessage(NetInfo _info)
    {
        var msg = Network.Deserialize<paodian.PaoDianExp>(_info);
        HotManager.Instance.EventHandler.SendEvent(CEvent.SCPaoDianExpMessage, msg.exp);
    }
}
