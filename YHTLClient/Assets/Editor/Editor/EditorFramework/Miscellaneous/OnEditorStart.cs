using UnityEngine;
using System.Collections;
using UnityEditor;
using ExtendEditor;
using System.IO;
using System.Collections.Generic;
[InitializeOnLoad]
public class OnEditorStart
{
    static OnEditorStart()
    {
        // 这种方法只能监听到修改某个prefab的实例后点击Apply时，无法监听直接修改prefab的属性的
        //PrefabUtility.prefabInstanceUpdated -= PrefabUpdate;
        //PrefabUtility.prefabInstanceUpdated += PrefabUpdate;
        //QualitySettings.SetQualityLevel(3);
        string replaceLine0 = "        public static string hotSaveTxtPath = @\"" + Application.dataPath.Replace("Assets", "") + "HotSave.txt\";";
        string replaceLine1 = "        public static string prePath = @\"" + Application.dataPath + "/\";";
        string xluaConfigPath = Application.dataPath + "/XLuaFrame/XLua/XLuaConfig.cs";
        //UnityEngine.Debug.Log("replaceLine0 = " + replaceLine0);
        //UnityEngine.Debug.Log("replaceLine1 = " + replaceLine1);

        if (!File.Exists(xluaConfigPath)) return;
        DealXluaConfig(xluaConfigPath, replaceLine0, "hotSaveTxtPath");
        DealXluaConfig(xluaConfigPath, replaceLine1, "prePath");

    }

    static void DealXluaConfig(string xluaConfigPath, string replaceLine, string preParamName)
    {
        string content = "";
        List<string> contentLint = new List<string>();
        bool isFindChange = false;
        bool isFindFirst = false;
        using (FileStream fileStream = new FileStream(xluaConfigPath, FileMode.OpenOrCreate, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fileStream, System.Text.Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!isFindFirst && line.Contains(preParamName))
                    {
                        isFindFirst = true;
                        if (line != replaceLine)
                        {
                            content += replaceLine + "\r\n";
                            isFindChange = true;
                        }
                        else
                        {
                            content += line + "\r\n";
                            //UnityEngine.Debug.Log("Same");
                        }
                    }
                    else
                    {
                        content += line + "\r\n";
                    }
                }
            }
        }
        //UnityEngine.Debug.Log(content);
        if (isFindChange)
        {
            using (StreamWriter sw = new StreamWriter(xluaConfigPath, false, System.Text.Encoding.UTF8))
            {
                sw.Write(content);
            }
            //FileUtility.Write(xluaConfigPath, content, false);
        }
    }

    static void PrefabUpdate(GameObject instance)
    {
        //if (instance == null) return;
        ////Debug.LogError(instance.name);
        //UnityEngine.Object obj = PrefabUtility.GetPrefabParent(instance);
        //if (obj != null)
        //{
        //    string path = AssetDatabase.GetAssetPath(obj);
        //    //Debug.LogError(path);
        //    if (path.Contains("Resources/UI/Prefabs"))
        //    {
        //        //Debug.LogError(obj.name);
        //        CSAtlasCollectBatchAdd.AddCAAtlasCollectToUIBase(obj,false);
        //    }
        //}
    }
}
