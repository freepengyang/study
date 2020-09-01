using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(CSFillAtlas))]
public class CSFillAtlasEditorEditor : Editor
{
    CSFillAtlas sprite;
    void OnEnable()
    {
        sprite = target as CSFillAtlas;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        sprite.SpriteName = EditorGUILayout.TextField("SpriteName", sprite.SpriteName);
        sprite.Atlas = EditorGUILayout.ObjectField("Atlas", sprite.Atlas, typeof(UIAtlas)) as UIAtlas;
        sprite.fillAmount = EditorGUILayout.Slider("FillCount", sprite.fillAmount, 0, 1);
        sprite.Color = EditorGUILayout.ColorField("Color", sprite.Color);
    }
}
