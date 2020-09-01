public partial class CSNetGem : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCPosGemChangeMessage:
				ECM_SCPosGemChangeMessage(obj);
			break;
			case ECM.SCPosGemInfoMessage:
				ECM_SCPosGemInfoMessage(obj);
			break;
			case ECM.SCUnlockGemPositionMessage:
				ECM_SCUnlockGemPositionMessage(obj);
			break;
			case ECM.SCGemSuitMessage:
				ECM_SCGemSuitMessage(obj);
			break;
			case ECM.SCGemBossCountChangeMessage:
				ECM_SCGemBossCountChangeMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
