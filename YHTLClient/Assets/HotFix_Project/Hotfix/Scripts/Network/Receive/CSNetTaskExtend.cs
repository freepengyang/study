public partial class CSNetTask : CSNetBase
{
	public override void NetCallback(ECM _type, NetInfo obj)
	{
		switch (_type)
		{
			case ECM.ResTaskListMessage:
				ECM_ResTaskListMessage(obj);
			break;
			case ECM.ResTaskStateChangedMessage:
				ECM_ResTaskStateChangedMessage(obj);
			break;
			case ECM.ResTaskGoalUpdatedMessage:
				ECM_ResTaskGoalUpdatedMessage(obj);
			break;
			case ECM.ResAcceptTaskMessage:
				ECM_ResAcceptTaskMessage(obj);
			break;
			case ECM.ResSubmitTaskMessage:
				ECM_ResSubmitTaskMessage(obj);
			break;
			case ECM.SCCycTaskMessage:
				ECM_SCCycTaskMessage(obj);
			break;
			case ECM.ResFlyToGoalMessage:
				ECM_ResFlyToGoalMessage(obj);
			break;
			case ECM.SCNewTaskMessage:
				ECM_SCNewTaskMessage(obj);
			break;
			default:
				HandByNetCallback(_type, obj);
			break;
		}
	}

}
