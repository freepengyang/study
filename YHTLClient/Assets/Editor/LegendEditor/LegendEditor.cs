using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
namespace Smart.Editor
{
    class LegendEditorWindow : EditorWindow
    {
        [MenuItem("Tools/传奇/界面测试")]
        static void AddWindow()
        {
            //创建窗口
            Rect wr = new Rect(0, 0, 316, 420);
            LegendEditorWindow window = (LegendEditorWindow)EditorWindow.GetWindowWithRect(typeof(LegendEditorWindow), wr, true, "界面测试");
            window.Show();
        }

        TABLE.GAMEMODELSARRAY modelTable;
        Vector2 _scrollPos;

        protected void OnEnable()
        {
            try
            {
                var path = Application.dataPath + "/../../Normal/zt_android/Table/gamemodels.bytes";
                var bytes = System.IO.File.ReadAllBytes(path);
                modelTable = new TABLE.GAMEMODELSARRAY();
                modelTable.Decode(bytes);
                FNDebug.LogFormat("<color=#00ff00>[Load GameModelTable Succeed !]</color>");
            }
            catch(Exception e)
            {
                FNDebug.LogErrorFormat("Load GameModelTable Failed ... reson:{0}",e.Message);
            }
        }

        protected void OnGUI()
        {
            if(null != modelTable)
            {
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
                EditorGUILayout.BeginVertical();
                var defColor = GUI.color;

                int i = 0;
                var arr = modelTable.gItem.handles;
                for (int k = 0, max = arr.Length; k < max; ++k)
                {
                    var model = arr[k].Value as TABLE.GAMEMODELS;
                    GUI.color = (i & 1) == 0 ? Color.cyan : Color.gray * 1.2f;
                    if (GUILayout.Button($"[{model.modelName}][Layer:{model.layer}][SubLayer:{model.subLayer}]"))
                    {
                        UtilityPanel.JumpToPanel(model.id);
                    }
                    ++i;
                }

                GUI.color = defColor;
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
            }
        }
    }
}