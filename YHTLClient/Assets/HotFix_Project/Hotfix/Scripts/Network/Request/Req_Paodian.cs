using Google.Protobuf.Collections;
using bag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Net
{
    //获取邮件列表请求
    public static void ReqUpgradePaoDianMessage()
    {
        paodian.UpgradePaoDian msg = CSProtoManager.Get<paodian.UpgradePaoDian>();
        CSNetwork.Instance.SendMsg((int)ECM.ReqUpgradePaoDianMessage, msg);
        CSProtoManager.Recycle(msg);
    }
}
