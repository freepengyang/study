public partial class CSNetRankalonetable : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.SCRoleRankInfoMessage:
				ECM_SCRoleRankInfoMessage(obj);
			break;
			case ECM.SCUnionRankInfoMessage:
				ECM_SCUnionRankInfoMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
