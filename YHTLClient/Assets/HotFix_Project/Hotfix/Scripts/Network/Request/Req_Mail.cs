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
    public static void ReqGetMailListMessage()
    {
        CSMailManager.Instance.LogFormat("ECM.ReqGetMailListMessage");
        CSNetwork.Instance.SendMsg((int)ECM.ReqGetMailListMessage,null);
    }

    //标记为已读请求
    public static void ReqReadMailMessage(List<long> mailIds)
    {
        if(null != mailIds && mailIds.Count > 0)
        {
            CSMailManager.Instance.LogFormat("ECM.ReqReadMailMessage Count = {0}",mailIds.Count);
            mail.MailIdMsg mails = CSProtoManager.Get<mail.MailIdMsg>();
            mails.mailIds.Clear();
            mails.mailIds.AddRange(mailIds);
            CSNetwork.Instance.SendMsg((int)ECM.ReqReadMailMessage, mails);
            CSProtoManager.Recycle(mails);
        }
    }

    //标记为已读请求
    public static void ReqReadOneMailMessage(long mailId)
    {
        CSMailManager.Instance.LogFormat("ECM.ReqReadOneMailMessage id = {0}", mailId);
        mail.MailIdMsg mails = CSProtoManager.Get<mail.MailIdMsg>();
        mails.mailIds.Clear();
        mails.mailIds.Add(mailId);
        CSNetwork.Instance.SendMsg((int)ECM.ReqReadMailMessage, mails);
        CSProtoManager.Recycle(mails);
    }

    //提取物品请求
    public static void ReqGetMailItemMessage(List<long> mailIds)
    {
        if (null != mailIds && mailIds.Count > 0)
        {
            CSMailManager.Instance.LogFormat("ECM.ReqGetMailItemMessage Count = {0}", mailIds.Count);
            mail.MailIdMsg mails = CSProtoManager.Get<mail.MailIdMsg>();
            mails.mailIds.Clear();
            mails.mailIds.AddRange(mailIds);
            CSNetwork.Instance.SendMsg((int)ECM.ReqGetMailItemMessage, mails);
            CSProtoManager.Recycle(mails);
        }
    }

    //提取物品请求
    public static void ReqGetOneMailItemMessage(long mailId)
    {
        CSMailManager.Instance.LogFormat("ECM.ReqGetOneMailItemMessage mailId = {0}", mailId);
        mail.MailIdMsg mails = CSProtoManager.Get<mail.MailIdMsg>();
        mails.mailIds.Clear();
        mails.mailIds.Add(mailId);
        CSNetwork.Instance.SendMsg((int)ECM.ReqGetMailItemMessage, mails);
        CSProtoManager.Recycle(mails);
    }

    //删除邮件请求
    public static void ReqDeleteMailMessage(List<long> mailIds)
    {
        if (null != mailIds && mailIds.Count > 0)
        {
            CSMailManager.Instance.LogFormat("ECM.ReqDeleteMailMessage Count = {0}", mailIds.Count);
            mail.MailIdMsg mails = CSProtoManager.Get<mail.MailIdMsg>();
            mails.mailIds.Clear();
            mails.mailIds.AddRange(mailIds);
            CSNetwork.Instance.SendMsg((int)ECM.ReqDeleteMailMessage, mails);
            CSProtoManager.Recycle(mails);
        }
    }

    //删除邮件请求
    public static void ReqDeleteOneMailMessage(long mailId)
    {
        CSMailManager.Instance.LogFormat("ECM.ReqDeleteOneMailMessage mailId = {0}", mailId);
        mail.MailIdMsg mails = CSProtoManager.Get<mail.MailIdMsg>();
        mails.mailIds.Clear();
        mails.mailIds.Add(mailId);
        CSNetwork.Instance.SendMsg((int)ECM.ReqDeleteMailMessage, mails);
        CSProtoManager.Recycle(mails);
    }
}
