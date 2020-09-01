public partial class CSNetWildadventure : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCWildAdventrueMessage:
				ECM_SCWildAdventrueMessage(obj);
			break;
			case ECM.SCTakeOutItemMessage:
				ECM_SCTakeOutItemMessage(obj);
			break;
			case ECM.SCBossItemMessage:
				ECM_SCBossItemMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
