//-------------------------------------------------------------------------
// UIBase 代码生成器
//Author jiabao
//Time 2016.9.4
//--
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using System.Text.RegularExpressions;

public class CSUIBaseEditor : Editor
{
    //#region 格式
    /// <summary>
    /// {0}：变量名
    /// {1}：变量类型
    /// </summary>
    public const string MEMBER_FORMAT = "private {0} {1} = null;";

    /// <summary>
    /// {0}：变量名
    /// </summary>
    public const string MEMBER_NULL = "{0} = null;";

    /// <summary>
    /// {0}：变量名
    /// {1}：变量类型
    /// </summary>
    public const string MEMBER_ASSIGN =
        "private {0} {1} {{ get {{ return {3} ?? ({3} = Get<{0}>({2}));}} }}";

    /// <summary>
    /// 存取UI模板路径
    /// </summary>
    public const string UIVIEW_PATH = "/../../HotFix_Project/Hotfix/Scripts/UI/view";

    /// <summary>
    /// 代码模板路径
    /// </summary>
    public const string UIVIEW_TEMPLATE_CODE_PATH = "/../../HotFix_Project/Hotfix/Scripts/UI/CodeTemplate/UIBaseTemplate.cs";

    public static string BEHAVIOUR_FORMAT = string.Empty;
    public static string TagDeclare = "//[declare]";
    public static string TagFinalize = "//[finalize]";
    static Regex ms_reg_mul_declare = new Regex(@"#region VarDeclare([^#]*)#endregion", RegexOptions.Multiline);
    static Regex ms_reg_mul_finalize = new Regex(@"#region VarFinalize([^#]*)#endregion", RegexOptions.Multiline);


    //[MenuItem("Tools/Legend/GenerateDeclare", false, 1101)]
    static bool generateCodeDeclare(out string declareCode,out string finalizeCode)
    {
        GameObject mainObject = Selection.activeGameObject;
        var className = mainObject.name;
        //Debug.Log($"<color=#00ffff>{className}</color>");

        declareCode = string.Empty;
        finalizeCode = string.Empty;
        StringBuilder content = new StringBuilder(256);
        StringBuilder finalizeContent = new StringBuilder(256);

        HashSet<string> varSets = new HashSet<string>();

        foreach (var transform in mainObject.transform.GetComponentsInChildren<Transform>(true))
        {
            string varName = string.Empty;
            string varTypeName = string.Empty;
            if (!IsVarValid(transform.name,out varName,out varTypeName))
            {
                continue;
            }
            var propertyDeclare = $"_{varName}";
            var PropertyMethodDeclare = $"{varName[0]}".ToUpper() + varName.Substring(1);
            var varPath = getPath(mainObject, transform);
            if(varSets.Contains(varPath))
            {
                continue;
            }
            varSets.Add(varPath);
            //Debug.Log($"{varName} {varTypeName}");
            content.Append("\t");
            content.AppendFormat(MEMBER_FORMAT, varTypeName, propertyDeclare);
            content.AppendLine();

            content.Append("\t");
            content.AppendFormat(MEMBER_ASSIGN, varTypeName, PropertyMethodDeclare, varPath, propertyDeclare);
            content.AppendLine();
            content.AppendLine();

            finalizeContent.Append("\t\t");
            finalizeContent.AppendFormat(MEMBER_NULL, propertyDeclare);
            finalizeContent.AppendLine();
        }

        foreach (var transform in mainObject.transform.GetComponentsInChildren<Transform>(false))
        {
            string varName = string.Empty;
            string varTypeName = string.Empty;
            if (!IsVarValid(transform.name, out varName, out varTypeName))
            {
                continue;
            }
            var propertyDeclare = $"_{varName}";
            var PropertyMethodDeclare = $"{varName[0]}".ToUpper() + varName.Substring(1);
            var varPath = getPath(mainObject, transform);
            if (varSets.Contains(varPath))
            {
                continue;
            }
            varSets.Add(varPath);
            FNDebug.Log($"{varName} {varTypeName}");
            content.Append("\t");
            content.AppendFormat(MEMBER_FORMAT, varTypeName, propertyDeclare);
            content.AppendLine();

            content.Append("\t");
            content.AppendFormat(MEMBER_ASSIGN, varTypeName, PropertyMethodDeclare, varPath, propertyDeclare);
            content.AppendLine();
            content.AppendLine();

            finalizeContent.Append("\t\t");
            finalizeContent.AppendFormat(MEMBER_NULL, propertyDeclare);
            finalizeContent.AppendLine();
        }

        declareCode = content.ToString();
        //Debug.Log($"<color=#00ffff>{declareCode}</color>");
        finalizeCode = finalizeContent.ToString();
        //Debug.Log($"<color=#00ffff>{finalizeContent}</color>");
        return true;
    }

    [MenuItem("Tools/传奇/生成界面代码", false, 1101)]
    public static void CreateUIBase()
    {
        GameObject mainObject = Selection.activeGameObject;

        if (mainObject == null)
        {
            FNDebug.LogError("未选择UI预制体");
            return;
        }

        var className = mainObject.name;
        string path = System.IO.Path.GetFullPath(Application.dataPath + UIVIEW_PATH);
        string behaviourCodeFile = Path.Combine(path, mainObject.name) + ".cs";

        string declareCode = string.Empty;
        string finalizeCode = string.Empty;
        if (!generateCodeDeclare(out declareCode, out finalizeCode))
        {
            return;
        }

        bool create = !System.IO.File.Exists(behaviourCodeFile);
        if (create)
        {
            string code_template_path = System.IO.Path.GetFullPath(Application.dataPath + UIVIEW_TEMPLATE_CODE_PATH);
            BEHAVIOUR_FORMAT = System.IO.File.ReadAllText(code_template_path);
            BEHAVIOUR_FORMAT = BEHAVIOUR_FORMAT.Replace(TagDeclare, declareCode);
            BEHAVIOUR_FORMAT = BEHAVIOUR_FORMAT.Replace(TagFinalize, finalizeCode);
            BEHAVIOUR_FORMAT = BEHAVIOUR_FORMAT.Replace(@"UIBaseTemplate", className);
        }
        else
        {
            BEHAVIOUR_FORMAT = System.IO.File.ReadAllText(behaviourCodeFile);
            string code = string.Empty;
            if (true)
            {
                var match = ms_reg_mul_declare.Match(BEHAVIOUR_FORMAT);
                if (match.Success)
                {
                    code += BEHAVIOUR_FORMAT.Substring(0, match.Groups[1].Index);
                    code += "\n";
                    code += declareCode;
                    code += "\n\t#endregion";
                    code += BEHAVIOUR_FORMAT.Substring(match.Index + match.Length);
                    BEHAVIOUR_FORMAT = code;
                    code = string.Empty;
                }
            }

            if (true)
            {
                var match = ms_reg_mul_finalize.Match(BEHAVIOUR_FORMAT);
                if (match.Success)
                {
                    code += BEHAVIOUR_FORMAT.Substring(0, match.Groups[1].Index);
                    code += "\n";
                    code += finalizeCode;
                    code += "\n\t\t#endregion";
                    code += BEHAVIOUR_FORMAT.Substring(match.Index + match.Length);
                    BEHAVIOUR_FORMAT = code;
                    code = string.Empty;
                }
            }
        }

        using (FileStream fs = File.Create(behaviourCodeFile))
        {
            byte[] bytes = Encoding.Default.GetBytes(BEHAVIOUR_FORMAT);
            fs.Write(bytes, 0, bytes.Length);

            if(create)
            {
                FNDebug.Log($"<color=#00ffff>[{className}]:代码创建成功!</color>");
            }
            else
            {
                FNDebug.Log($"<color=#00ffff>[{className}]:代码重新生成成功!</color>");
            }
        }
    }

    private static string getPath(GameObject _target, Transform tf)
    {
        List<string> ListName = new List<string>();

        string strName = "";

        if (_target.name.Equals(tf.name)) return strName;

        if (tf.parent != _target.transform)
        {
            getPath(_target, tf.gameObject, ref ListName);
        }

        ListName.Reverse();

        foreach (string str in ListName)
        {
            strName += str + "/";
        }
        strName += tf.name;
        return "\"" + strName + "\"";
    }

    private static void getPath(GameObject _target, GameObject Mono, ref List<string> ListName)
    {
        if (_target.name == Mono.transform.parent.name) return;

        ListName.Add(Mono.transform.parent.name);

        getPath(_target, Mono.transform.parent.gameObject, ref ListName);
    }

    private static Dictionary<string,string> ms_type_set = new Dictionary<string,string>(32)
    {
        {"go","GameObject"},
        {"spr","UISprite"},
        {"lab","UILabel"},
        {"grid","UIGrid"},
        {"scroll","UIScrollView"},
        {"slid","UISlider"},
        {"progress","UIProgressBar"},
        {"tg","UIToggle"},
        {"input","UIInput"},
        {"tl","UITextList"},
        {"btn","GameObject"},
        {"tex","UITexture"},
        {"gridc","UIGridContainer"},
    };
    private static bool IsVarValid(string objName,out string varName,out string varType)
    {
        varName = string.Empty;
        varType = string.Empty;
        string[] sName = objName.Split('_');
        if (sName.Length < 2)
        {
            return false;
        }
        if(string.IsNullOrEmpty(sName[0]) || string.IsNullOrEmpty(sName[1]))
        {
            return false;
        }
        if(!ms_type_set.ContainsKey(sName[0]))
        {
            return false;
        }
        varName = objName;
        varType = ms_type_set[sName[0]];
        return true;
    }

    public static bool IsInclude(string _name)
    {
        if (_name.Contains("go_"))
        {
            return true;
        }
        else if (_name.Contains("spr_"))
        {
            return true;
        }
        else if (_name.Contains("lab_"))
        {
            return true;
        }
        else if (_name.Contains("grid_"))
        {
            return true;
        }
        else if (_name.Contains("scroll_"))
        {
            return true;
        }
        else if (_name.Contains("slid_"))
        {
            return true;
        }
        else if (_name.Contains("progress_"))
        {
            return true;
        }
        else if (_name.Contains("tg_"))
        {
            return true;
        }
        else if (_name.Contains("input_"))
        {
            return true;
        }
        else if (_name.Contains("tl_"))
        {
            return true;
        }
        else if (_name.Contains("btn_"))
        {
            return true;
        }
        else if (_name.Contains("tex_"))
        {
            return true;
        }
        else if (_name.Contains("gridc_"))
        {
            return true;
        }

        return false;
    }
}
