using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
namespace ExtendEditor
{
    public class CheckMissRes : SelectionBase
    {
        [MenuItem("Tools/Miscellaneous/CheckMissRes")]
        public static void RenameAssetbundleProc()
        {
            EditorWindow win = GetWindow(typeof(CheckMissRes));
            win.titleContent = new UnityEngine.GUIContent("资源遗漏检查");
            win.Show();
        }

        string resDir;
        string assetbundleDir;
        List<string> resList = new List<string>();
        List<string> assetbundleList = new List<string>();
        int dealIndex = -1;
        bool isClearResDir = false;

        protected List<List<string>> mBuildPathList = new List<List<string>>()
        {
            new List<string>(){"/Map/"},
            new List<string>(){"/MiniMap/"},
            //new List<string>(){"/UI/Prefabs/"},
            new List<string>(){"/Model/",".prefab"},
            new List<string>(){"/Audio/"},
			new List<string>(){"/ScaleMap/"},
            new List<string>(){"/Model/Effect/"},
			new List<string>(){"/UIPlayer/"},
			new List<string>(){"/UIWeapon/"},
			new List<string>(){"/UIWing/"},        };

        public override void OnEnable()
        {
            GetPrefs();
        }
        public override void OnGUI()
        {
            EditorGUILayout.LabelField("只支持检测一个文件夹中的资源是否遗漏");
            string rDir = EditorGUILayout.TextField("原资源路径:", resDir);
            string aDir = EditorGUILayout.TextField("Assetbundle资源路径:", assetbundleDir);
            EditorGUILayout.LabelField("当前进度:" + dealIndex + "/" + resList.Count);
            if (rDir != resDir||aDir!=assetbundleDir)
            {
                resDir = rDir.Replace("\\", "/");
                assetbundleDir = aDir.Replace("\\", "/");
                SavePrefs();
            }

            if (GUILayout.Button("Deal"))
            {
                FNDebug.Log(rDir);
                resList.Clear();
                FileUtility.GetDeepAssetPaths(rDir, resList);
                isClearResDir = true;
            }
            if (isClearResDir)
            {
                isClearResDir = false;
                ClearResPaths();
                dealIndex = 0;
            }

            if (dealIndex >= 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    if (dealIndex < resList.Count)
                    {
                        string path = resList[i];
                        string old = path.Replace("Assets/AssetBundleRes/", "");
                        string fileName = FileUtility.GetFileName(path);
                        FNDebug.Log("fileName = " + fileName);
                        path = assetbundleDir + "/"+fileName;
                        FNDebug.Log(path);
                        if (!File.Exists(path))
                        {
                            FileUtility.Write(Application.dataPath + "/AssetBundleRes/CheckMissRes.txt", old + "\r\n");
                        }
                    }
                    else
                    {
                        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                        EditorUtility.DisplayDialog("Finish", "Done", "Ok");
                        dealIndex = -1;
                        break;
                    }
                    dealIndex++;
                }
            }
            else
            {
                dealIndex = -1;
            }
        }

        void ClearResPaths()
        {
            for (int i = resList.Count-1; i >= 0; i--)
            {
                string path = resList[i];
                for (int j = 0; j < mBuildPathList.Count; j++)
                {
                    if (path.Contains(mBuildPathList[j][0]))
                    {
                        if (mBuildPathList.Count > 1)
                        {
                            for (int t = 1; t < mBuildPathList[j].Count; t++)
                            {
                                if (!path.Contains(mBuildPathList[j][t]))
                                {
                                    resList.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }

        void ClearAssetbundlePaths()
        {

        }

        void DealProc(string path)
        {

        }

        void GetPrefs()
        {
            resDir = EditorPrefsUtility.GetString(Application.dataPath + "/resDir");
            assetbundleDir = EditorPrefsUtility.GetString(Application.dataPath + "/assetbundleDir");
        }

        void SavePrefs()
        {
            EditorPrefsUtility.SetString(Application.dataPath + "/resDir", resDir);
            EditorPrefsUtility.SetString(Application.dataPath + "/assetbundleDir", assetbundleDir);
        }
    }
}


