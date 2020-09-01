public partial class CSNetTip : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResTipMessage:
				ECM_ResTipMessage(obj);
			break;
			case ECM.ResBulletinMessage:
				ECM_ResBulletinMessage(obj);
			break;
			case ECM.TipNtfMessage:
				ECM_TipNtfMessage(obj);
			break;
			case ECM.SCNotifyNoteInfoMessage:
				ECM_SCNotifyNoteInfoMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
