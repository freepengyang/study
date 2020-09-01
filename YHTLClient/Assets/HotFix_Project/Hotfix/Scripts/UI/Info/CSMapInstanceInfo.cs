using daycharge;
using System.Collections.Generic;

public class CSMapInstanceInfo : CSInfo<CSMapInstanceInfo>
{
	private int bossNum = 0;
	private int monsterNum = 0;
	public void SetLimitMessage(instance.ResDropLimit msg)
	{
		bossNum = msg.monsterNumInfo[0].monsterNum;
		monsterNum = msg.monsterNumInfo[1].monsterNum;
	}
	public int GetBossNum()
	{
		return bossNum;
	}
	public int GetMonsterNum()
	{
		return monsterNum;
	}
	public override void Dispose()
	{
		bossNum = 0;
		monsterNum = 0;
	}
}