using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityComponent
{

    /// <summary>
    /// 设置垂直的动态箭头.注1：(down箭头在滑到底时隐藏，up箭头在滑到顶部时隐藏。其他位置均显示。若scroll不允许滑动时均隐藏)。
    /// 注2：scroll组件上必须添加VerticalBar组件。
    /// </summary>
    /// <param name="scroll"></param>
    /// <param name="arrowDown"></param>
    /// <param name="arrowUp"></param>
    public static void SetDynamicArrowVertical(this UIScrollView scroll, GameObject arrowDown, GameObject arrowUp = null)
    {
        if (scroll.verticalScrollBar == null)
        {
            FNDebug.LogError("设置垂直动态箭头失败，请先在Scroll上添加VerticalScrollBar组件");
            return;
        }

        UIProgressBar bar = scroll.verticalScrollBar;
        bar.fillDirection = UIProgressBar.FillDirection.TopToBottom;//从上到下和从下到上似乎效果一样

        arrowDown?.CustomActive(scroll.canMoveVertically && bar.value <= 0.95f);
        arrowUp?.CustomActive(scroll.canMoveVertically && bar.value >= 0.05f);
        
        bar.onChange.Add(new EventDelegate(() =>
        {
            if (!scroll.canMoveVertically)
            {
                arrowDown?.CustomActive(false);
                arrowUp?.CustomActive(false);
                return;
            }

            arrowDown?.CustomActive(bar.value <= 0.95f);
            arrowUp?.CustomActive(bar.value >= 0.05f);
        }));
    }


    /// <summary>
    /// 设置水平的动态箭头
    /// </summary>
    /// <param name="scroll"></param>
    /// <param name="arrowRight"></param>
    /// <param name="arrowLeft"></param>
    public static void SetDynamicArrowHorizontal(this UIScrollView scroll, GameObject arrowRight, GameObject arrowLeft = null)
    {
        if (scroll.horizontalScrollBar == null)
        {
            FNDebug.LogError("设置水平动态箭头失败，请先在Scroll上添加HorizontalScrollBar组件");
            return;
        }

        UIProgressBar bar = scroll.horizontalScrollBar;
        bar.fillDirection = UIProgressBar.FillDirection.LeftToRight;

        arrowRight?.CustomActive(scroll.canMoveHorizontally && bar.value <= 0.95f);
        arrowLeft?.CustomActive(scroll.canMoveHorizontally && bar.value >= 0.05f);

        bar.onChange.Add(new EventDelegate(() =>
        {
            if (!scroll.canMoveHorizontally)
            {
                arrowRight?.CustomActive(false);
                arrowLeft?.CustomActive(false);
                return;
            }

            arrowRight?.CustomActive(bar.value <= 0.95f);
            arrowLeft?.CustomActive(bar.value >= 0.05f);
        }));
    }


    /// <summary>
    /// 垂直的动态箭头。Scroll下使用了WrapContent使用此方法。maxLength为wrapContent的ItemSize * 数据总长度
    /// </summary>
    /// <param name="scroll"></param>
    /// <param name="maxLength"></param>
    /// <param name="arrowDown"></param>
    /// <param name="arrowUp"></param>
    public static void SetDynamicArrowVerticalWithWrap(this UIScrollView scroll, int maxLength, GameObject arrowDown, GameObject arrowUp = null)
    {
        if (scroll.verticalScrollBar == null)
        {
            FNDebug.LogError("设置垂直动态箭头失败，请先在Scroll上添加VerticalScrollBar组件");
            return;
        }
        UIPanel panel = scroll.GetComponent<UIPanel>();
        if (panel == null) return;
        UIProgressBar bar = scroll.verticalScrollBar;
        bar.fillDirection = UIProgressBar.FillDirection.TopToBottom;

        float startY = scroll.transform.localPosition.y;
        float curprocess = 0;
        arrowDown?.CustomActive(scroll.canMoveVertically && curprocess <= 0.95f);
        arrowUp?.CustomActive(scroll.canMoveVertically && curprocess >= 0.05f);

        bar.onChange.Add(new EventDelegate(() =>
        {
            if (!scroll.canMoveVertically)
            {
                arrowDown?.CustomActive(false);
                arrowUp?.CustomActive(false);
                return;
            }
            //float endY = (maxLength - panel.GetViewSize().y) - Mathf.Abs(startY);
            var a = Mathf.Abs(scroll.transform.localPosition.y - startY);
            var b = Mathf.Abs(maxLength - panel.GetViewSize().y);
            curprocess = Mathf.Clamp(a / b, 0, 1f);

            //Debug.LogError("@@@curprocess:" + curprocess + ", a:" + a + ", b:" + b);
            arrowDown?.CustomActive(curprocess <= 0.98f);
            arrowUp?.CustomActive(curprocess >= 0.05f);
        }));
    }


    /// <summary>
    /// 水平的动态箭头。Scroll下使用了WrapContent使用此方法。maxLength为wrapContent的ItemSize * 数据总长度
    /// </summary>
    /// <param name="scroll"></param>
    /// <param name="maxLength"></param>
    /// <param name="arrowDown"></param>
    /// <param name="arrowUp"></param>
    public static void SetDynamicArrowHorizontalWithWrap(this UIScrollView scroll, int maxLength, GameObject arrowRight, GameObject arrowLeft = null)
    {
        if (scroll.horizontalScrollBar == null)
        {
            FNDebug.LogError("设置水平动态箭头失败，请先在Scroll上添加HorizontalScrollBar组件");
            return;
        }
        UIPanel panel = scroll.GetComponent<UIPanel>();
        if (panel == null) return;
        UIProgressBar bar = scroll.horizontalScrollBar;
        bar.fillDirection = UIProgressBar.FillDirection.LeftToRight;

        float startX = scroll.transform.localPosition.x;
        float curprocess = 0;
        arrowRight?.CustomActive(scroll.canMoveHorizontally && curprocess <= 0.98f);
        arrowLeft?.CustomActive(scroll.canMoveHorizontally && curprocess >= 0.02f);

        bar.onChange.Add(new EventDelegate(() =>
        {
            if (!scroll.canMoveHorizontally)
            {
                arrowRight?.CustomActive(false);
                arrowLeft?.CustomActive(false);
                return;
            }
            //float endY = (maxLength - panel.GetViewSize().y) - Mathf.Abs(startY);
            var a = Mathf.Abs(scroll.transform.localPosition.x - startX);
            var b = Mathf.Abs(maxLength - panel.GetViewSize().x);
            curprocess = Mathf.Clamp(a / b, 0, 1f);

            //Debug.LogError("@@@curprocess:" + curprocess + ", a:" + a + ", b:" + b);
            arrowRight?.CustomActive(curprocess <= 0.98f);
            arrowLeft?.CustomActive(curprocess >= 0.02f);
        }));
    }


}
