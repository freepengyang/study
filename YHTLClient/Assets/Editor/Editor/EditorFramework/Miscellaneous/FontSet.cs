using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
namespace ExtendEditor
{
    public class FontSet : SelectionBase
    {
        public enum EType
        {
            SetFontFilterMode,
            SetPrefabUseFont,
        }
        [MenuItem("Tools/Miscellaneous/3003_字体设置",false,3003)]
        public static void FontSetProc()
        {
            EditorWindow win = GetWindow(typeof(FontSet));
            win.Show();
        }
        EType eType = EType.SetFontFilterMode;
        FilterMode filterMode = FilterMode.Point;
        TextureWrapMode textureWrapMode = TextureWrapMode.Clamp;
        Font mFont_0;
        Font mFont_1;
        public override void OnGUI()
        {
            base.OnGUI();
            eType = (EType)EditorGUILayout.EnumPopup("类型", eType);
            if (eType == EType.SetFontFilterMode)
            {
                filterMode = (FilterMode)EditorGUILayout.EnumPopup("FilterMode", filterMode);
                textureWrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup("TextureWrapMode", textureWrapMode);

                if (GUILayout.Button("将字体文件中图片修改格式"))
                {
                    base.BeginHandle();
                }
            }
            else
            {
                //mFont_0 = (Font)EditorGUILayout.ObjectField("源字体文件", mFont_0, typeof(Font));
                mFont_1 = (Font)EditorGUILayout.ObjectField("替换字体文件", mFont_1, typeof(Font));
                if (/*mFont_0 != null &&*/ mFont_1!=null&&GUILayout.Button("替换UI Prefab中的字体"))
                {
                    base.BeginHandle();
                }
            }

            if (base.CanHandle())
            {
                if (eType == EType.SetFontFilterMode)
                {
                    TestSetFontFilterMode();
                }
                else
                {
                    ReplaceFontInPrefab();
                }
                
                base.MoveHandle();
                if (!base.CanHandle())
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                }
            }
        }

        void TestSetFontFilterMode()
        {
            UnityEngine.Font font = base.GetCurHandleObj() as UnityEngine.Font;
            if (font != null&&font.material!=null&&font.material.mainTexture!=null)
            {
                font.material.mainTexture.filterMode = filterMode;
                font.material.mainTexture.wrapMode = textureWrapMode;
            }
        }

        List<string> tempList = new List<string>()
        {
            "AdobeHeitiStd-Regular",
            "AdobeKaitiStd-Regular (v5.010)",
            "DFP_GB_Y7_0",
            "Wx",
            "Arial",
            "fzcy",
        };

        void ReplaceFontInPrefab()
        {
            GameObject g = base.GetCurHandleObj() as GameObject;
            bool isChange = false;
            UILabel[] labs = g.GetComponentsInChildren<UILabel>(true);
            foreach (UILabel lab in labs)
            {
                if (lab == null || lab.ambigiousFont == null) continue;
                Font font = lab.ambigiousFont as Font;
                if (font == null)
                {
                    isChange = true;
                    continue;
                }
                if (tempList.Contains(font.name))
                {
                    isChange = true;
                }
            }
            if (!isChange) return;

            GameObject go = GameObject.Instantiate(base.GetCurHandleObj()) as GameObject; ;
            if (go == null) return;

            labs = go.GetComponentsInChildren<UILabel>(true);
            foreach (UILabel lab in labs)
            {
                if (lab == null || lab.ambigiousFont == null) continue;
                Font font = lab.ambigiousFont as Font;
                if (font == null)
                {
                    //lab.ambigiousFont = mFont_1;
                    continue;
                }
                if (tempList.Contains(font.name))
                {
                    lab.ambigiousFont = mFont_1;
                }
            }

            PrefabUtility.ReplacePrefab(go, base.GetCurHandleObj());
            GameObject.DestroyImmediate(go);
        }
    }
}