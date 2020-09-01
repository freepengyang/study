using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace ExtendEditor
{
    public class SetAlphaSplitFlag : SelectionBase
    {
        [MenuItem("Tools/Miscellaneous/设置透明通道标记")]
        public static void SetAlphaSplitFlagProc()
        {
            EditorWindow win = GetWindow(typeof(SetAlphaSplitFlag));
            win.Show();
        }

        public override void OnGUI()
        {
            base.OnGUI();
            if (GUILayout.Button("Find"))
            {
                base.BeginHandle();
            }

            if (base.CanHandle())
            {
                for (int i = 0; i < 100; i++)
                {
                    FindAtlasAndMaterial();
                    base.MoveHandle();
                    if (!base.CanHandle())
                    {
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                        break;
                    }
                }
                base.MoveHandle();
            }
        }

        void FindAtlasAndMaterial()
        {
            UnityEngine.Object obj = base.GetCurHandleObj();
            Material mat = obj as Material;
            if (mat == null) return;
            if (mat.HasProperty("_IsAlphaSplit"))
            {
                mat.SetInt("_IsAlphaSplit", 1);
            }
        }
    }
}

