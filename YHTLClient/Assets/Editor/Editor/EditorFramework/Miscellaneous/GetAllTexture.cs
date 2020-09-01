using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
namespace ExtendEditor
{
    public class GetAllTexture : SelectionBase
    {
        [MenuItem("Tools/Miscellaneous/3000_将选中目录中的图片移动到Assets同级目录下（没有.meta文件）", false, 3000)]
        public static void GetAllTextureProc()
        {
            EditorWindow win = GetWindow(typeof(GetAllTexture));
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
                for (int i = 0; i < 10; i++)
                {
                    string path = base.GetCurHandlePath();
                    string srcPath = Application.dataPath.Replace("Assets", "") + path;
                    string destPath = Application.dataPath + "/" + path;
                    destPath = destPath.Replace("Assets/", "");
                    FileUtility.DetectCreateDirectory(destPath);
                    //Debug.LogError(destPath);

                    //Debug.LogError(srcPath);
                    File.Copy(srcPath, destPath, true);
                    base.MoveHandle();
                }
            }
        }
    }
}
