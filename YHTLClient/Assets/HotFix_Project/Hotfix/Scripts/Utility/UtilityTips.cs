using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class UtilityTips
{
	/// <summary>
	/// 屏幕下方显示提示文字
	/// </summary>
	public static void ShowDownTips(string content, float timer = 1.5f/*, ColorType type = ColorType.Yellow*/)
	{
		//TODO:ddn
		UITipsViewBase tips = UITipsViewPool.Instance.PopUITipPanel<UITipsViewDown>(UITipsViewPool.TipsViewType.Down);
		if (tips == null) return;

		
		
		tips.ShowTips(content, timer/*,UtilityColor.GetColor(type)*/);

		CSBetterList<UITipsViewBase> tipsList = UITipsViewPool.Instance.GetTipsList(UITipsViewPool.TipsViewType.Down);
		int count = tipsList.Count -1;
		if (count>0)
		{
			
			for (int i = 0; i <= count; i++)
			{
				if (tipsList[i] != null)
				{
					tipsList[i].Move(count - i + 1);
				}
			}
		}

	}

	public static void ShowTips(string content, float timer = 1.5f, ColorType type = ColorType.Yellow)
	{
		string str = content;
		if (type!= ColorType.White)
		{
			str = str.BBCode(type);
		}
		
		ShowDownTips(str, timer);
	}

	/// <summary>
	/// 读表提示，
	/// </summary>
	/// <param name="id">ClientTips  Id</param>
	/// <param name="type">颜色，默认黄色</param>
	public static void ShowTips(int id, float timer = 1.5f, ColorType type = ColorType.Yellow)
	{
		//.BBCode(ColorType.Yellow);
		string str = CSString.Format(id);
		if (type != ColorType.White)
		{
			str = str.BBCode(type);
		}
		
		ShowDownTips(str, timer/*, type*/);
	}
	public static void ShowTips(int id, float timer, ColorType type, params object[] data)
	{
		string str = CSString.Format(id, data);
		if (type != ColorType.White)
		{
			str = str.BBCode(type);
		}
		
		ShowDownTips(str, timer/*, type*/);
	}

	public static void ShowRedTips(int id)
	{
		string str = CSString.Format(id);
		ShowRedTips(str);
	}
	public static void ShowRedTips(int id, params object[] objs)
	{
		string str = CSString.Format(id, objs).BBCode(ColorType.ToolTipUnDone);
		ShowDownTips(str, 1.5f/*, ColorType.ToolTipUnDone*/);
	}

	public static void ShowRedTips(string content)
	{
		string str = content.BBCode(ColorType.ToolTipUnDone);
		ShowDownTips(str, 1.5f/*,ColorType.ToolTipUnDone */);
	}

	public static void ShowRedTips(string content,float timer)
	{
		string str = content.BBCode(ColorType.ToolTipUnDone);
		ShowDownTips(str, timer);
	}

	public static void ShowYellowTips(int id ,float timer = 1.5f)
	{
		string str = CSString.Format(id).BBCode(ColorType.Yellow);
		ShowDownTips(str, timer);
	}

	public static void ShowYellowTips(int id ,float timer = 1.5f, params object[] data)
	{
		string str = CSString.Format(id, data).BBCode(ColorType.Yellow);
		ShowDownTips(str, timer);
	}
	
	
	public static void ShowGreenTips(string content)
	{
		string str = content.BBCode(ColorType.Green);
        ShowDownTips(str, 1.5f/*, ColorType.Green*/);
    }

    public static void ShowGreenTips(int id, params object[] objs)
	{
		string str = CSString.Format(id, objs).BBCode(ColorType.Green);
		ShowDownTips(str, 1.5f/*, ColorType.Green*/);
    }

    public static void ShowGreenTips(string content,float timer)
    {
	    string str = content.BBCode(ColorType.Green);
	    ShowDownTips(str, timer/*, ColorType.Green*/);
    }
    
    #region 左下角提示
    /// <summary>
    /// 道具变动用(id是item表configId，tipId是ClientTip表Id)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="_count"></param>
    /// <param name="tipId"></param>
    public static void LeftDownTips(int id, long _count,int tipId)
    {
        TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(id);
        if (cfg != null)
        {
            LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(tipId),UtilityColor.BBCode($"{cfg.name}*",cfg.quality), 
                UtilityColor.BBCode(_count.ToString(), cfg.quality)));
        }
    }
    public static void LeftDownTips(int id, string _count, int tipId)
    {
        TABLE.ITEM cfg = ItemTableManager.Instance.GetItemCfg(id);
        if (cfg != null)
        {
            LeftDownTips(string.Format(ClientTipsTableManager.Instance.GetClientTipsContext(tipId), UtilityColor.BBCode(cfg.name, cfg.quality),
                UtilityColor.BBCode(_count, cfg.quality)));
        }
    }
    public static void LeftDownTips(string content,bool _ingoreCover = false)
    {
        if (CSGaussianBlurMgr.Instance.IsHaveGaussian() )
        {
            if (!_ingoreCover)
            {
                ShowLeftDownCoverPanelTips(content);
            }
        }
        else
        {
            ShowLeftDownTips(content);
        }
    }
    public static void ShowLeftDownTips(string content, float timer = 3f)
	{
        //Debug.Log("LeftDown   " + content);
		UITipsViewBase tips = UITipsViewPool.Instance.PopUITipPanel<UITipsViewLeftDown>(UITipsViewPool.TipsViewType.LeftDown);
		if (tips == null) return;

		if (tips != null)
		{
			tips.ShowTips(content, timer/*, GetColor(ColorType.White)*/);
		}
		
		CSBetterList<UITipsViewBase> tipsList = UITipsViewPool.Instance.GetTipsList(UITipsViewPool.TipsViewType.LeftDown);
		int count = tipsList.Count -1;
		
		for (int i = 0; i <= count; i++)
		{
			if (tipsList[i] != null)
			{
				//左下角跳字在生成预制后需要设置初始值保证动效正常
				//int y = count - i - 1 <= 0 ? 0 : 22 * (count - i - 1);
				tipsList[i].UIPrefab.transform.localPosition = new Vector3(0,(count - i)*22,0);
				//Debug.Log("eedd" + 22*(count - i - 1));
				tipsList[i].Move(count - i + 1);
			}
		}
	}
    
    public static void ShowCenterMoveUpInfo(string content, float timer = 1.5f, ColorType type = ColorType.White)
    {
        UITipsViewBase tips = UITipsViewPool.Instance.PopUITipPanel<UITipsViewMoveUp>(UITipsViewPool.TipsViewType.MoveUp);
        if (tips == null) return;
        tips.UIPrefab.SetActive(true);
        tips.ShowTips(content, timer/*, GetColor(type)*/);
        CSBetterList<UITipsViewBase> tipsList = UITipsViewPool.Instance.GetTipsList(UITipsViewPool.TipsViewType.MoveUp);
        int count = tipsList.Count - 1;
        for (int i = 0; i <= count; i++)
        {
            if (tipsList[i] != null)
            {
                tipsList[i].Move(count - i + 1);
            }
        }
    }
    public static void ShowLeftDownCoverPanelTips(string content, float timer = 3f/*, ColorType type = ColorType.White*/)
    {
        UITipsViewBase tips = UITipsViewPool.Instance.PopUITipPanel<UITipsViewLeftDownCoverPanel>(UITipsViewPool.TipsViewType.LeftDownCoverPanel);
        if (tips == null) return;
        tips.UIPrefab.SetActive(true);
        tips.ShowTips(content, timer/*, GetColor(type)*/);
        CSBetterList<UITipsViewBase> tipsList = UITipsViewPool.Instance.GetTipsList(UITipsViewPool.TipsViewType.LeftDownCoverPanel);
        int count = tipsList.Count - 1;
        
        
        for (int i = 0; i <= count; i++)
        {
            if (tipsList[i] != null)
            {
	            
            
                tipsList[i].Move(count - i + 1);
            }
        }
    }
    #endregion
    #region  经验、泡点经验飘字
    public static void ShowCenterRight(int _type,string content, float timer = 1.5f/*, ColorType type = ColorType.White*/)
    {
        //Debug.Log("CenterRight    " + content);
        UITipsViewBase tips = UITipsViewPool.Instance.PopUITipPanel<UITipsCenterRight>(UITipsViewPool.TipsViewType.CenterRight);
        if (tips == null) return;

        if (tips != null)
        {
            tips.SetExpType(_type,content, timer/*, GetColor(ColorType.White)*/);
        }

        CSBetterList<UITipsViewBase> tipsList = UITipsViewPool.Instance.GetTipsList(UITipsViewPool.TipsViewType.CenterRight);
        int count = tipsList.Count - 1;
        for (int i = 0; i <= count; i++)
        {
            if (tipsList[i] != null)
            {
                tipsList[i].Move(count - i + 1);
            }
        }
    }
    #endregion

    public static void ShowCenterInfo(int id, /*ColorType type,*/ params object[] data)
    {
        if (data == null || data.Length <= 0)
        {
            ShowCenterInfo(CSString.Format(id), 1.5f/*, type*/);
        }
        else
        {
            ShowCenterInfo(CSString.Format(id, data), 1.5f/*, type*/);
        }
    }

    /// <summary> 显示中间提示信息</summary>
    public static void ShowCenterInfo(string content, float timer = 1.5f/*, ColorType type = ColorType.White*/)
    {
        //TODO:ddn
        UITipsViewBase tips = UITipsViewPool.Instance.PopUITipPanel<UITipsViewCenter>(UITipsViewPool.TipsViewType.Center);
        if (tips == null) return;

        if (tips != null)
        {
            tips.ShowTips(content, timer/*, GetColor(type)*/);
        }
    }
    public static Color GetColor(ColorType type)
	{
		switch (type)
		{
			case ColorType.Blue: return Color.blue;
			case ColorType.Green: return Color.green;
			case ColorType.Yellow: return Color.yellow;
			case ColorType.White: return Color.white;
			case ColorType.Red: return new Color(255, 0, 0);
			case ColorType.Orange: return new Color(254, 102, 0);
		}

		return Color.white;
	}

	/// <summary>
	/// 封装了“下次不再显示”和“本次登入不再显示”
	/// </summary>
	/// <param name="id"></param>
	/// <param name="left"></param>
	/// <param name="right"></param>
	/// <param name="contents"></param>
	public static void ShowPromptWordTips(int id, System.Action left, System.Action right, params System.Object[] contents)
	{
		ShowPromptWordTip(id, left, right, null, 0, contents);
	}

	/// <summary>
	/// 页面显示倒计时，固定时间关闭
	/// </summary>
	public static void ShowPromptWordTips(int id, System.Action left, System.Action right, int CloseTime, params System.Object[] contents)
	{
		ShowPromptWordTip(id, left, right, null, 0, contents);
	}

	public static void ShowPromptWordTips(int id, System.Action right)
	{
		ShowPromptWordTip(id, null, right, null, 0);
	}

	public static void ShowPromptWordTips(int id, System.Action right, params System.Object[] contents)
	{
		ShowPromptWordTip(id, null, right, null, 0, contents);
	}

	public static void ShowPromptWordTips(int id, System.Action left, System.Action right)
	{
		ShowPromptWordTip(id, left, right, null, 0);
	}

	public static void ShowPromptWordTips(int id, System.Action left, System.Action right, System.Action Close,
		params System.Object[] contents)
	{
		ShowPromptWordTip(id, left, right, Close, 0, contents);
	}

	private static void ShowPromptWordTip(int id, System.Action left, System.Action right, System.Action Close, int CloseTime, params System.Object[] contents)
    {
	    if (!Constant.ShowTipsOnceList.Contains((int)id))
        {
            UIManager.Instance.CreatePanel<UIPromptPanel>(f =>
            {
                UIPromptPanel panel = f as UIPromptPanel;
                panel.mPromptID = (int)id;
                panel.Show(id, (state, b) =>
                {
                    if (state == PromptState.leftBtn)
                    {
                        if (left != null) left();
                    }
                    else if (state == PromptState.rightBtn)
                    {
                        if (right != null) right();
                    }
                    else if (state == PromptState.close)
                    {
                        if (Close != null) Close();
                    }

                },CloseTime, contents);
            });
        }
        else
        {
            if (right != null)
                right();
        }
    }


    /// <summary>
    /// 显示屏上方的自动离开副本倒计时
    /// </summary>
    /// <param name="countDownSeconds">时间，秒</param>
    public static void ShowExitInstanceCountDown(long countDownSeconds)
    {
        UIManager.Instance.CreatePanel<UIExitInstanceCountDownPanel>((f) =>
        {
            (f as UIExitInstanceCountDownPanel).SetTimeAndStartCount(countDownSeconds);
        });
    }

    /// <summary>
    /// 显示弹框，表格数据加载前  需要弹框的调用此方法，其他全部读表
    /// </summary>
    public static void ShowPromptWordTips(string content, System.Action left, System.Action right, params System.Object[] contents)
    {
	    ShowPromptWordTips(content, CSStringTip.CANCEL, CSStringTip.CONFIG, left, right, contents);
    }
    
    /// <summary>
    /// 显示弹框，表格数据加载前  需要弹框的调用此方法，其他全部读表
    /// </summary>
	public static void ShowPromptWordTips(string content, string leftBtn, string rightBtn, System.Action left, System.Action right, params System.Object[] contents)
	{
		UIManager.Instance.CreatePanel<UIPromptPanel>(f =>
		{
			UIPromptPanel panel = f as UIPromptPanel;
			panel.Show(content, leftBtn, rightBtn, (state, b) =>
			{
				if (state == PromptState.leftBtn)
				{
					if (left != null) left();
				}
				else if (state == PromptState.rightBtn)
				{
					if (right != null) right();
				}
			}, contents);
		});
	}


    /// <summary>
    /// 显示弹框，强制点击确定
    /// </summary>
    public static void ShowPromptForceWordTip(int id, System.Action right, params System.Object[] contents)
    {
		    UIManager.Instance.CreatePanel<UIPromptForcePanel>(f =>
		    {
			    UIPromptForcePanel panel = f as UIPromptForcePanel;
			    panel.mPromptID = (int)id;
			    panel.Show(id, right, contents);
		    });
	   
    }
}