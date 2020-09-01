public class CheckContext
{
	private UICheckBase mCheck;

	public CheckContext(ServerType serverType)
	{
		switch (serverType)
		{
			case ServerType.GameServer:
				mCheck = new UICheckInGame();
				break;
		}
	}
}