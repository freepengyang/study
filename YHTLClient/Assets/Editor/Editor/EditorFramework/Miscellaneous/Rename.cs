using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
namespace ExtendEditor
{
    public class Rename : SelectionBase
    {
        public enum EType
        {
            Null,
            Rename,
            ReplaceFile,
            ReplaceDir,
        }

        [MenuItem("Tools/Miscellaneous/Rename")]
        public static void RenameProc()
        {
            EditorWindow win = GetWindow(typeof(Rename));
            win.Show();
        }

        EType eType;
        string mChangeName0;
        string mChangeName1;
        FilterMode filterMode = FilterMode.Point;
        TextureWrapMode textureWrapMode = TextureWrapMode.Clamp;
        public override void OnGUI()
        {
            base.OnGUI();
            eType = (EType)EditorGUILayout.EnumPopup("Select Type", eType);
            switch (eType)
            {
                case EType.Rename:
                    {
                        mChangeName0 = EditorGUILayout.TextField("ChangeName", mChangeName0);
                    }
                    break;
                case EType.ReplaceFile:
                    {
                        mChangeName0 = EditorGUILayout.TextField("ChangeName", mChangeName0);
                        mChangeName1 = EditorGUILayout.TextField("Replace With", mChangeName1);
                    }
                    break;
                case EType.ReplaceDir:
                    {
                        mChangeName0 = EditorGUILayout.TextField("ChangeName", mChangeName0);
                        mChangeName1 = EditorGUILayout.TextField("Replace With", mChangeName1);
                    }
                    break;
            }

            if (GUILayout.Button("Rename"))
            {
                base.BeginHandle();
                if (eType == EType.ReplaceDir)
                {
                    RenameDir();
                    AssetDatabase.Refresh();
                    base.End();
                }
            }

            if (base.CanHandle())
            {
                string path = base.GetCurHandlePath();
                Object obj = base.GetCurHandleObj();
                if (!string.IsNullOrEmpty(path) && obj != null)
                {
                    string assetName = obj.name;
                    string str = "";
                    switch (eType)
                    {
                        case EType.Rename:
                            {
                                str = mChangeName0;
                            }
                            break;
                        case EType.ReplaceFile:
                            {
                                str = assetName.Replace(mChangeName0, mChangeName1);
                            }
                            break;
                    }
                    AssetDatabase.RenameAsset(path, str);
                }
                base.MoveHandle();
            }
            filterMode = (FilterMode)EditorGUILayout.EnumPopup("FilterMode", filterMode);
            textureWrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup("TextureWrapMode", textureWrapMode);

            if (GUILayout.Button("Test Set Font Filter Mode"))
            {
                TestSetFontFilterMode();
            }
        }

        void TestSetFontFilterMode()
        {
            GameObject go = Selection.activeGameObject;
            if (go != null)
            {
                UILabel label = go.GetComponent<UILabel>();
                if (label != null&&label.ambigiousFont!=null)
                {
                    UnityEngine.Font font = label.ambigiousFont as UnityEngine.Font;
                    if(font != null&&font.material!=null&&font.material.mainTexture != null)
                    {
                        font.material.mainTexture.filterMode = filterMode;
                        font.material.mainTexture.wrapMode = textureWrapMode;
                    }
                    //Debug.Log("");
                    //label.ambigiousFont.
                }
            }
        }

        void RenameDir()
        {
            for (int i = 0; i < mDirList.Count; i++)
            {
                string path = mDirList[i];
                string newPath = path.Replace(mChangeName0,mChangeName1);
                if (path != newPath)
                {
                    if (!Directory.Exists(newPath))
                    {
                        Directory.Move(path, newPath);
                    }
                    else
                    {
                        Directory.Move(path, newPath + "_Test");
                        Directory.Move(newPath + "_Test", newPath);
                        //Debug.LogError("目录已经存在 = " + newPath);
                    }
                }
                else
                {
                    //Debug.Log("目录不能修改成相同名称 path=" + path + " newPath = " + newPath);
                }
            }
        }
    }
}