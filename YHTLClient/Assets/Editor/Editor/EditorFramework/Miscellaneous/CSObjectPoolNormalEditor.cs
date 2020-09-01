using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(CSObjectPoolNormal))]
public class CSObjectPoolNormalEditor : Editor
{
    CSObjectPoolNormal tar;
    void OnEnable()
    {
        tar = target as CSObjectPoolNormal;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("ListCount = ",tar.ListCount+"");
    }
}
