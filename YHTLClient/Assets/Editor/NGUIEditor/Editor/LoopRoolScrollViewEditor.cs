
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(LoopRoolScrollView), true)]


public class LoopRoolScrollViewEditor : UIWidgetContainerEditor
{
    LoopRoolScrollView mLoopRoolScrollView;
    void OnEnable()
    {
        mLoopRoolScrollView = target as LoopRoolScrollView;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("对象池中缓存个数:");
        mLoopRoolScrollView.MaxShowCount = EditorGUILayout.IntField("MaxCount", mLoopRoolScrollView.MaxShowCount);
        EditorGUILayout.LabelField("每个Item之间间隙[0 - 1]:");
        mLoopRoolScrollView.AddValue = EditorGUILayout.FloatField("AddValue", mLoopRoolScrollView.AddValue);
        //EditorGUILayout.LabelField("起始位置:");
        //mLoopRoolScrollView.StartValue = EditorGUILayout.FloatField("StartValue", mLoopRoolScrollView.StartValue);
        EditorGUILayout.LabelField("显示宽度:");
        mLoopRoolScrollView.Width = EditorGUILayout.FloatField("Width", mLoopRoolScrollView.Width);
        EditorGUILayout.LabelField("位置曲线");
        mLoopRoolScrollView.PositionCurve = EditorGUILayout.CurveField("PositionCurve", mLoopRoolScrollView.PositionCurve);
        EditorGUILayout.LabelField("缩放曲线");
        mLoopRoolScrollView.ScaleCurve = EditorGUILayout.CurveField("ScaleCurve", mLoopRoolScrollView.ScaleCurve);
        EditorGUILayout.LabelField("透明度曲线");
        mLoopRoolScrollView.ApaCurve = EditorGUILayout.CurveField("ApaCurve", mLoopRoolScrollView.ApaCurve);
        //EditorGUILayout.LabelField("每个方向不可见item缓存个数:");
        //mLoopRoolScrollView.InvisibleCache = EditorGUILayout.IntField("InvisibleCache", mLoopScrollView.InvisibleCache);
        //EditorGUILayout.LabelField("排列方式:");
        //mLoopRoolScrollView.arrangeDirection = (LoopRoolScrollView.ArrangeDirection)EditorGUILayout.EnumPopup("ArrangeDirection", mLoopScrollView.arrangeDirection);
        //EditorGUILayout.LabelField("起始位置:");
        //mLoopRoolScrollView.itemStartPos = EditorGUILayout.Vector3Field("ItemStartPos", mLoopScrollView.itemStartPos);
        base.DrawDefaultInspector();
    }
}
