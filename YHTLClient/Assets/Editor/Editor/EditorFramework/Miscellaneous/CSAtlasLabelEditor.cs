using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(CSAtlasLabel),true)]
public class CSAtlasLabelEditor : Editor
{
    CSAtlasLabel tar;
    void OnEnable()
    {
        tar = target as CSAtlasLabel;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //tar.uiAtlas = EditorGUILayout.ObjectField("Atlas",tar.uiAtlas, typeof(UIAtlas)) as UIAtlas;
        //tar.Text = EditorGUILayout.TextField("SpriteName",tar.Text);
        //tar.color = EditorGUILayout.ColorField("Color", tar.color);
       // tar.SpaceX = EditorGUILayout.FloatField("SpaceX", tar.SpaceX);
    }
}
