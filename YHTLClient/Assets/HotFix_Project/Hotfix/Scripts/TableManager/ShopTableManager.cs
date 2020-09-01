using System.Collections.Generic;
using Google.Protobuf.Collections;
public partial class ShopTableManager : TableManager<TABLE.SHOPARRAY, TABLE.SHOP,int,ShopTableManager>
{
	public string GetPriceByItemID(int itemid)
	{
        var arr = array.gItem.handles;
        for (int k = 0, max = arr.Length; k < max; ++k)
        {
            var value = arr[k].Value as TABLE.SHOP;
			if (value.itemId == itemid)
			{
				CSStringBuilder.Clear();
				return CSStringBuilder.Append(value.payType,"#", value.value).ToString();
			}
		}
		return "";
	}
}