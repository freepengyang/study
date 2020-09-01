public partial class CSNetWing : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCWingInfoMessage:
				ECM_SCWingInfoMessage(obj);
			break;
			case ECM.SCWingUpStarMessage:
				ECM_SCWingUpStarMessage(obj);
			break;
			case ECM.SCWingAdvanceMessage:
				ECM_SCWingAdvanceMessage(obj);
			break;
			case ECM.SCDressColorWingMessage:
				ECM_SCDressColorWingMessage(obj);
			break;
			case ECM.SCWingExpItemUseMessage:
				ECM_SCWingExpItemUseMessage(obj);
			break;
			case ECM.SCColorWingChangeMessage:
				ECM_SCColorWingChangeMessage(obj);
			break;
			case ECM.SCYuLingInfoMessage:
				ECM_SCYuLingInfoMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
