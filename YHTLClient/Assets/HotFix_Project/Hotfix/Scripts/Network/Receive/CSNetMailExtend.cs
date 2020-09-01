public partial class CSNetMail : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResMailListMessage:
				ECM_ResMailListMessage(obj);
			break;
			case ECM.ResNewMailMessage:
				ECM_ResNewMailMessage(obj);
			break;
			case ECM.ResGetMailItemMessage:
				ECM_ResGetMailItemMessage(obj);
			break;
			case ECM.ResDeleteMailMessage:
				ECM_ResDeleteMailMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
