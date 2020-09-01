namespace Smart.Editor
{
	public static class ExcelLogicHelper
	{
		public static void ModifyRowContent(object tableItem)
		{
			if (tableItem is TABLE.PROMPTWORD promptWorldItem)
			{
				promptWorldItem.dec = promptWorldItem.dec.Replace("\\n", "\n");
			}
		}
	}
}