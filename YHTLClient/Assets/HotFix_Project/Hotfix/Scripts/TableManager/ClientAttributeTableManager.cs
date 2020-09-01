using System.Collections.Generic;
public partial class ClientAttributeTableManager : TableManager<TABLE.CLIENTATTRIBUTEARRAY, TABLE.CLIENTATTRIBUTE,int,ClientAttributeTableManager>
{
	public string GetAttributeName(int id)
	{
		TABLE.CLIENTATTRIBUTE attribute = null;
		if (!TryGetValue(id,out attribute))
		{
			return string.Empty;
		}

		TABLE.CLIENTTIPS clientTip = null;
		if(!ClientTipsTableManager.Instance.TryGetValue((int)attribute.tipID,out clientTip))
		{
			return string.Empty;
		}

		return clientTip.context;
	}
}