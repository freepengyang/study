using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
namespace ExtendEditor
{
    public class CheckMissSplitTex: SelectionBase
    {
        [MenuItem("Tools/Miscellaneous/CheckMissSplitTex")]
        public static void CheckMissSplitTexProc()
        {
            EditorWindow win = GetWindow(typeof(CheckMissSplitTex));
            win.titleContent = new GUIContent("检测通道分离是否成功");
            win.Show();
        }

        public override void OnGUI()
        {
            base.OnGUI();
            if (GUILayout.Button("Deal"))
            {
                base.BeginHandle();
            }
            if (base.CanHandle())
            {
                for (int i = 0; i < 100; i++)
                {
                    if (base.CanHandle())
                    {
                        UnityEngine.Object obj = base.GetCurHandleObj();
                        GameObject go = obj as GameObject;
                        if (go != null)
                        {
                            UIAtlas atlas = go.GetComponent<UIAtlas>();
                            if (atlas != null)
                            {
                                if (atlas.spriteMaterial == null || atlas.spriteMaterial.GetTexture("_MainTex") == null || atlas.spriteMaterial.GetTexture("_AlphaTex")||
                                   !atlas.spriteMaterial.HasProperty("_IsAlphaSplit")||atlas.spriteMaterial.GetInt("_IsAlphaSplit") == 0)
                                {
                                    FileUtility.Write(Application.dataPath + "/AssetBundleRes/检测通道分离是否成功.txt", base.GetCurHandlePath() + "\r\n");
                                }
                            }
                        }
                    }
                    else
                    {
                        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                        break;
                    }
                }
                base.MoveHandle();
            }
        }

    }
}


