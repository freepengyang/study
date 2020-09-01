public partial class CSNetMail: CSNetBase
{
    public void ECM_ResMailListMessage(NetInfo obj)
    {
        var rsp = Network.Deserialize<mail.MailList>(obj);
        CSMailManager.Instance.InitMailList(rsp);
    }

    public void ECM_ResNewMailMessage(NetInfo obj)
    {
        var rsp = Network.Deserialize<mail.MailInfo>(obj);
        CSMailManager.Instance.AddNewMail(rsp);
    }

    public void ECM_ResGetMailItemMessage(NetInfo obj)
    {
        var rsp = Network.Deserialize<mail.MailIdMsg>(obj);
        CSMailManager.Instance.ModifyMailStatus(rsp,MailState.MS_FETCHED);
    }

    public void ECM_ResDeleteMailMessage(NetInfo obj)
    {
        var rsp = Network.Deserialize<mail.MailIdMsg>(obj);
        CSMailManager.Instance.DeleteMail(rsp);
    }
}