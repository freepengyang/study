public partial class CSNetEnergy : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCEnergyInfoMessage:
				ECM_SCEnergyInfoMessage(obj);
			break;
			case ECM.SCEnergyFreeGetInfoMessage:
				ECM_SCEnergyFreeGetInfoMessage(obj);
			break;
			case ECM.SCGetFreeEnergyMessage:
				ECM_SCGetFreeEnergyMessage(obj);
			break;
			case ECM.SCNotifyEnergyChangeMessage:
				ECM_SCNotifyEnergyChangeMessage(obj);
			break;
			case ECM.SCEnergyExchangeInfoMessage:
				ECM_SCEnergyExchangeInfoMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
