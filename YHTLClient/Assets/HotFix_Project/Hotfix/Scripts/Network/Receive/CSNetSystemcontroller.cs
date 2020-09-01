public partial class CSNetSystemcontroller : CSNetBase
{

	void ECM_SystemFunctionStateNtfMessage(NetInfo info)
	{
		systemcontroller.SystemFunctionStateNtf msg = Network.Deserialize<systemcontroller.SystemFunctionStateNtf>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsystemcontroller.SystemFunctionStateNtf");
			return;
		}
		UICheckManager.Instance.InitServerControl(msg.info);
	}
	void ECM_SCRoleFunctionDataMessage(NetInfo info)
	{
		systemcontroller.RoleFunctionData msg = Network.Deserialize<systemcontroller.RoleFunctionData>(info);
		if(null == msg)
		{
			UnityEngine.Debug.LogError("Deserialize Msg Failed Forsystemcontroller.RoleFunctionData");
			return;
		}

		UICheckManager.Instance.FunctionOpenState(msg.info);
	}

}
