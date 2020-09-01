using System.Collections.Generic;

public class CSGaussianBlurMgr : CSInfo<CSGaussianBlurMgr>
{
	public CSGaussianBlurMgr()
	{
		needGaussianList = new List<string>();
	}

	private List<string> needGaussianList;

	public void AddPanel(string panel)
	{
		if (!needGaussianList.Contains(panel))
			needGaussianList.Add(panel);

		HotFix_Invoke.EventHandler.SendEvent(MainEvent.ShowOrCloseGaussian, needGaussianList.Count > 0);
	}

	public void RemovePanel(string panel)
	{
		if (needGaussianList.Contains(panel))
			needGaussianList.Remove(panel);
		HotFix_Invoke.EventHandler.SendEvent(MainEvent.ShowOrCloseGaussian, needGaussianList.Count > 0);
	}

	public void RemoveAll()
	{
		needGaussianList.Clear();
		HotFix_Invoke.EventHandler.SendEvent(MainEvent.ShowOrCloseGaussian, false);
	}

	public bool IsHaveGaussian()
	{
		return needGaussianList != null && needGaussianList.Count > 0;
	}


	public override void Dispose()
	{
		if (needGaussianList != null) needGaussianList.Clear();
		needGaussianList = null;
	}
}