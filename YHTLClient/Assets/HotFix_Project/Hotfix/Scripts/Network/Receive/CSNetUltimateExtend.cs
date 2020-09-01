public partial class CSNetUltimate : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCUltimateInfoMessage:
				ECM_SCUltimateInfoMessage(obj);
			break;
			case ECM.SCResetUltimateMessage:
				ECM_SCResetUltimateMessage(obj);
			break;
			case ECM.SCSelectAdditionEffectMessage:
				ECM_SCSelectAdditionEffectMessage(obj);
			break;
			case ECM.SCRankInfoMessage:
				ECM_SCRankInfoMessage(obj);
			break;
			case ECM.SCSelectAdditionIndexMessage:
				ECM_SCSelectAdditionIndexMessage(obj);
			break;
			case ECM.SCResponseCardMessage:
				ECM_SCResponseCardMessage(obj);
			break;
			case ECM.SCOpenCardInfoMessage:
				ECM_SCOpenCardInfoMessage(obj);
			break;
			case ECM.SCSelectCardIndexMessage:
				ECM_SCSelectCardIndexMessage(obj);
			break;
			case ECM.SCGodBlessMessage:
				ECM_SCGodBlessMessage(obj);
			break;
			case ECM.SCUltimatePassInfoMessage:
				ECM_SCUltimatePassInfoMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
