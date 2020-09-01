using System;
using UnityEngine;

public class UIBtnsMenuCulation
{
    private static UIBtnsMenuCulation instance;
    public static UIBtnsMenuCulation Instance
    {
        get
        {
            if (instance == null)
                instance = (UIBtnsMenuCulation)Activator.CreateInstance(typeof(UIBtnsMenuCulation));
            return instance;
        }
        set
        {
            if (instance.Equals(value))
                return;
            instance = value;
        }
    }

    /// <summary>
    /// 处理按钮背景图自适应大小
    /// </summary>
    /// <param name="bgHead">头像层背景</param>
    /// <param name="bgBtns">按钮层背景</param>
    /// <param name="uiGrid">按钮所在排列组件</param>
    /// <param name="objBtn">按钮物体</param>
    public void SetUIBtnsAdaption(UISprite bgBtns, UIGridContainer uiGrid, UISprite spHead = null)
    {
        //获取UIGrid组件信息并处理按钮背景图
        if (uiGrid.arrangement == UIGridContainer.Arrangement.Horizontal)
        {
            //处理按钮背景图
            bgBtns.width = (int) uiGrid.CellWidth * uiGrid.MaxPerLine+10;
            int spriteHeight = (spHead == null) ? 0 : spHead.height+25;
            bgBtns.height = (int)uiGrid.CellHeight * (((int)Mathf.Floor((uiGrid.MaxCount - 1)/uiGrid.MaxPerLine))+1)+spriteHeight;
        }
        else if (uiGrid.arrangement == UIGridContainer.Arrangement.Vertical)
        {
            //处理按钮背景图
            int spriteWidth = (spHead == null) ? 0 : spHead.width+25;
            bgBtns.width = (int)uiGrid.CellWidth * (((int)Mathf.Floor((uiGrid.MaxCount - 1)/uiGrid.MaxPerLine))+1)+spriteWidth;
            bgBtns.height = (int) uiGrid.CellHeight * uiGrid.MaxPerLine+10;
        }
    }
}
