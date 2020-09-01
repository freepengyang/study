using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

public class LanguageData
{
    public LanguageData(string t,string e)
    {
        this.t = t;
        this.e = e;
    }
    public string t;

    public string e;

}

public class LanguageTools 
{
    /// <summary>
    /// UI路径
    /// </summary>
    private static string uiPath = Application.dataPath + "/UIAsset/Prefabs";
    /// <summary>
    /// 脚本路径
    /// </summary>
    private static string scriptPath = Application.dataPath + "/Scripts";

    /// <summary>
    /// 输出路径
    /// </summary>
    //导出的中文KEY路径
    private static string examplePath = "";

    private static List<string> Localization = new List<string>();

    private static string staticWriteText = "";

    public static Dictionary<string, LanguageData> languageDic = new Dictionary<string, LanguageData>();

   

    //[MenuItem("Tools/导出多语言")]
    static void ExportChinese()
    {
        Localization.Clear();

        string path = EditorUtility.SaveFilePanel("保存：", "", "example", "txt");

        if (string.IsNullOrEmpty(path)) return;

        examplePath = path;

        staticWriteText = "";

        //提取Prefab上的中文
        staticWriteText += "----------------Prefab----------------------\n";
        AnalyzePrefab();

        ////提取CS中的中文
        staticWriteText += "----------------Script----------------------\n";
        AnalyzeCS(new DirectoryInfo(scriptPath));

        //最终把提取的中文生成出来
        FileTool.CreateFile(examplePath, staticWriteText);
        AssetDatabase.Refresh();
    }

    //[MenuItem("Tools/回填数据")]
    static void BackFill()
    {
        languageDic.Clear();

        string path = EditorUtility.OpenFilePanel("选择数据:", "English", "txt");

        if (string.IsNullOrEmpty(path)) return;

        LoadData(path);

        DirectoryInfo dictoryInfo = new DirectoryInfo(uiPath);

        if (dictoryInfo.Exists)
        {
            FileInfo[] fileInfos = dictoryInfo.GetFiles("*.prefab", SearchOption.AllDirectories);

            for (int i = 0; i < fileInfos.Length; i++)
            {
                FileInfo files = fileInfos[i];

                if (files == null) continue;

                string fullName = files.FullName;

                string text = string.Format("获取进度: {0}/{1}", i, fileInfos.Length - 1);

                EditorUtility.DisplayProgressBar("设置Prefab文本", text, 1f * i / (fileInfos.Length - 1));

                string assetPath = fullName.Substring(fullName.IndexOf("Assets\\"));

                GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

                SetPrefabString(prefab.transform);

                EditorUtility.SetDirty(prefab);
            }
            EditorUtility.ClearProgressBar();

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
        }
        else
        {
            EditorUtility.DisplayDialog("警告!", "uiPath目录不存在", "确定");
        }
    }

    /// <summary>
    /// 分析Prefab，找出对应的中文
    /// </summary>
    /// <param name="dictoryInfo"></param>
    static public void AnalyzePrefab()
    {
        DirectoryInfo dictoryInfo = new DirectoryInfo(uiPath);

        if (dictoryInfo.Exists)
        {
            FileInfo[] fileInfos = dictoryInfo.GetFiles("*.prefab", SearchOption.AllDirectories);

            for (int i = 0; i < fileInfos.Length; i++)
            {
                FileInfo files = fileInfos[i];

                if (files == null) continue;

                string fullName = files.FullName;

                string text = string.Format("获取进度: {0}/{1}", i, fileInfos.Length - 1);

                EditorUtility.DisplayProgressBar("获取Prefab文本", text, 1f * i / (fileInfos.Length - 1));

                string assetPath = fullName.Substring(fullName.IndexOf("Assets\\"));

                GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

                SearchPrefabString(prefab.transform);

            }
            EditorUtility.ClearProgressBar();
        }
        else
        {
            EditorUtility.DisplayDialog("警告!", "uiPath目录不存在", "确定");
        }
    }

    //递归所有C#代码
    static public void AnalyzeCS(DirectoryInfo dictoryInfo)
    {
        if (!dictoryInfo.Exists) return;
        FileInfo[] fileInfos = dictoryInfo.GetFiles("*.cs", SearchOption.AllDirectories);
        foreach (FileInfo files in fileInfos)
        {
            string path = files.FullName;
            string assetPath = path.Substring(path.IndexOf("Assets\\"));
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(TextAsset)) as TextAsset;
            string text = textAsset.text;
            string expr = "(?<=StrUtil.GetText\\s*\\(\\s*\"\\s*)[\\s\\S]*?(?=\\s*\")";
            //用正则表达式把代码里面两种字符串中间的字符串提取出来。
            MatchCollection mc = Regex.Matches(text, expr);
            foreach (Match m in mc)
            {
                string format = m.Value;
                if (!Localization.Contains(format) && !string.IsNullOrEmpty(format))
                {
                    Localization.Add(format);
                    staticWriteText += format + "\n";
                }
            }
        }
    }

    //提取Prefab上的中文
    static public void SearchPrefabString(Transform root)
    {
        foreach (Transform chind in root)
        {
            //因为这里是写例子，所以我用的是UILabel 
            //这里应该是写你用于图文混排的脚本。
            UILabel label = chind.GetComponent<UILabel>();

            if (label != null)
            {
                string text = label.text;
                if (!string.IsNullOrEmpty(text))
                {
                    string p = "[\u4e00-\u9fa5]";
                    if (Regex.IsMatch(text, p))
                    {
                        if (!Localization.Contains(text))
                        {
                            Localization.Add(text);
                            text = text.Replace("\n", "|").Replace("\r", "#"); 
                            staticWriteText += text + "\n";
                        }
                    }
                }
            }

            if (chind.childCount > 0)
            {
                SearchPrefabString(chind);
            }
        }
    }


    static public void SetPrefabString(Transform root)
    {
        foreach (Transform chind in root)
        {
            //因为这里是写例子，所以我用的是UILabel 
            //这里应该是写你用于图文混排的脚本。
            UILabel label = chind.GetComponent<UILabel>();

            if (label != null)
            {
                string text = label.text;
                if (!string.IsNullOrEmpty(text))
                {
                    if (languageDic.ContainsKey(text))
                    {
                        label.text = languageDic[text].e;
                    }
                }
            }

            if (chind.childCount > 0)
            {
                SetPrefabString(chind);
            }
        }
    }

    static void LoadData(string p)
    {
        string text = FileTool.ReadFile(p);
        string[] texts = text.Split('\n');

        for (int i = 0; i < texts.Length; i++)
        {
            string[] t = texts[i].Split('\t');

            for (int j = 0; j < t.Length; j++)
            {
                if (t.Length < 3) continue;

                if (!languageDic.ContainsKey(t[0]))
                {
                    languageDic.Add(t[0], new LanguageData(t[1], t[2]));
                }
            }
        }
    }
}
