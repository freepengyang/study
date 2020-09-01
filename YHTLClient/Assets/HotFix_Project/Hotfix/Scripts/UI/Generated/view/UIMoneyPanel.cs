public partial class UIMoneyPanel : UIBasePanel
{
	protected UIGridContainer mMoneyGrids;
	protected UITable mtable_con;
	protected override void _InitScriptBinder()
	{
		mMoneyGrids = ScriptBinder.GetObject("MoneyGrids") as UIGridContainer;
		mtable_con = ScriptBinder.GetObject("table_con") as UITable;
	}
}
