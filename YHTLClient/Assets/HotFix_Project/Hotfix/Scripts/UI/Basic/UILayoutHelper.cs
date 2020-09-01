using System.Linq;
using UnityEngine;

public enum Alignment
{
    TL = 0,
    TM,
    TR,
    ML,
    MM,
    MR,
    BL,
    BM,
    BR,
}

public enum AlignmentDirection
{
    Normal = 0,
    HorizenR2L = (1 << 0),
    VertialD2U = (1 << 1),
}

public static class UILayoutHelper
{
    static Vector2[] pivots = new Vector2[]
    {
        new Vector2(0,1),
        new Vector2(0.5f,1),
        new Vector2(1,1),
        new Vector2(0,0.5f),
        new Vector2(0.5f,0.5f),
        new Vector2(1,0.5f),
        new Vector2(0,1),
        new Vector2(0.5f,1),
        new Vector2(1,1),
    };

    /// <summary>
    /// parentPivot 修改父物体计算锚点
    /// childPivot 联合的子物体计算锚点
    /// </summary>
    /// <param name="widget"></param>
    /// <param name="childPivot"></param>
    /// <param name="parentPivot"></param>
    /// <param name="alignmentDirection"></param>
    static void AlignChild(this UIWidget widget, Vector2 childPivot,Vector2 parentPivot,bool horizen = true,AlignmentDirection alignmentDirection = AlignmentDirection.Normal)
    {
        if (null == widget)
            return;

        UIWidget[] widgets = widget.GetComponentsInChildren<UIWidget>();
        if (widgets.Length <= 0)
            return;

        Vector2 starPivot = parentPivot - widget.pivotOffset;
        Vector2 starPos = starPivot * widget.localSize;
        Vector2 size = Vector2.zero;
        for (int i = 0; i < widgets.Length; ++i)
        {
            if (!widgets[i].enabled || widgets[i] == widget)
                continue;

            var localSize = widgets[i].localSize;
            Vector3 localPosition = widgets[i].transform.localPosition;
            Vector2 pivotOffset = NGUIMath.GetPivotOffset(widgets[i].pivot);

            if(horizen)
            {
                if ((alignmentDirection & AlignmentDirection.HorizenR2L) != AlignmentDirection.HorizenR2L)
                {
                    localPosition.x = starPos.x + pivotOffset.x * localSize.x;
                    starPos.x += localSize.x;
                    size.x += localSize.x;
                }
                else
                {
                    localPosition.x = starPos.x + (pivotOffset.x - 1.0f) * localSize.x;
                    starPos.x -= localSize.x;
                    size.x += localSize.x;
                }
            }
            else
            {
                if ((alignmentDirection & AlignmentDirection.VertialD2U) == AlignmentDirection.VertialD2U)
                {
                    localPosition.y = starPos.y + pivotOffset.y * localSize.y;
                    starPos.y += localSize.y;
                    size.y += localSize.y;
                }
                else
                {
                    localPosition.y = starPos.y + (pivotOffset.y - 1.0f) * localSize.y;
                    starPos.y -= localSize.y;
                    size.y += localSize.y;
                }
            }

            widgets[i].transform.localPosition = localPosition;
        }

        Vector2 childOffset = -childPivot * size;
        for (int i = 0; i < widgets.Length; ++i)
        {
            if (!widgets[i].enabled || widgets[i] == widget)
                continue;
            Vector3 localPosition = widgets[i].transform.localPosition;
            localPosition += new Vector3(childOffset.x, childOffset.y);
            widgets[i].transform.localPosition = localPosition;
        }
    }

    //子结点中点对父结点中点
    public static void CenterChild(this UIWidget widget,bool horizen = true,AlignmentDirection alignmentDirection = AlignmentDirection.Normal)
    {
        AlignChild(widget, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), horizen,alignmentDirection);
    }
}