using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIGridBinderContainer), true)]
public class UIGridBinderContainerEditor : UIWidgetContainerEditor
{
    UIGridBinderContainer mUiGrid;
    void OnEnable()
    {
        mUiGrid = target as UIGridBinderContainer;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("克隆的最大数量:");
        mUiGrid.MaxCount = EditorGUILayout.IntField("MaxCount", mUiGrid.MaxCount);
        EditorGUILayout.LabelField("每行或者每列显示的最大个数:");
        mUiGrid.MaxPerLine = EditorGUILayout.IntField("MaxPerLine", mUiGrid.MaxPerLine);
        EditorGUILayout.LabelField("每个高度间隔:");
        mUiGrid.CellHeight = (float)EditorGUILayout.IntField("CellHeight", (int)mUiGrid.CellHeight);
        EditorGUILayout.LabelField("每个宽度间隔:");
        mUiGrid.CellWidth = (float)EditorGUILayout.IntField("CellWidth", (int)mUiGrid.CellWidth);
        EditorGUILayout.LabelField("横排还是竖排:");
        mUiGrid.arrangement = (UIGridBinderContainer.Arrangement)EditorGUILayout.EnumPopup("arrangement", mUiGrid.arrangement);
        base.DrawDefaultInspector();
    }
}
