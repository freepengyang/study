using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace ExtendEditor
{
    public class FindMissAtlas : SelectionBase
    {
        [MenuItem("Tools/Miscellaneous/设置丢失的Atlas和Material")]
        public static void FindMissAtlasProc()
        {
            EditorWindow win = GetWindow(typeof(FindMissAtlas));
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
                FindAtlasAndMaterial();
                base.MoveHandle();
            }
        }

        void FindAtlasAndMaterial()
        {
            UnityEngine.Object obj = base.GetCurHandleObj();
            string path = base.GetCurHandlePath();
            GameObject go = obj as GameObject;
            if (go == null) return;
            UIAtlas[] atlass = go.GetComponentsInChildren<UIAtlas>(true);
            if (atlass.Length == 0) return;
            string dirPath = FileUtility.GetDirectory(obj);
            List<string> matList = new List<string>();
            List<string> texList = new List<string>();
            FileUtility.GetDeepAssetPaths(dirPath,matList,".mat");
            FileUtility.GetDeepAssetPaths(dirPath,texList,".png");
            for (int i = 0; i<atlass.Length; i++)
            {
                UIAtlas atlas = atlass[i];
                if (atlass == null) continue;
                if (atlas.spriteMaterial == null)
                {
                    for (int j = 0; j < matList.Count; j++)
                    {
                        string matPath = matList[j];
                        if (matPath.Contains(atlas.name + ".mat"))
                        {
                            atlas.spriteMaterial = AssetDatabase.LoadAssetAtPath(matPath,typeof(Material)) as Material;
                            break;
                        }
                    }
                }

                if (atlas.spriteMaterial != null&&atlas.spriteMaterial.mainTexture == null)
                {
                    for (int j = 0; j < texList.Count; j++)
                    {
                        string texPath = texList[j];
                        if (texPath.Contains(atlas.name + ".png"))
                        {
                            atlas.spriteMaterial.mainTexture = AssetDatabase.LoadAssetAtPath(texPath, typeof(Texture)) as Texture;
                            break;
                        }
                    }
                }
            }
        }
    }
}

