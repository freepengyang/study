using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class ImessageNoAttr
{
    //@"    private (\w+) (\w+)_( = \"\"){0,1};\n";
    //@"[ ]+/// <summary>[^<]*</summary>\n";
    //@"    public (\w+) (\w+) {\n      get { return (\w+)_; }\n      set {\n[^}]+}\n    }\n";
    //@"    private (\w+) (\w+)_( = \"\"){0,1};\n([ ]+/// <summary>[^<]*</summary>\n)*    public \w+ \w+ {\n      get { return (\w+)_; }\n      set {\n[^}]+}\n    }\n";

    static Regex ms_reg = new Regex(@"private (\w+) (\w+)_;");
    static Regex ms_reg_line = new Regex(@"    private (\w+) \w+_( = .+){0,1};\n([ ]+/// <summary>[^<]*</summary>\n)*    public \w+ (\w+) {\n      get { return \w+_; }\n      set {\n[^}]+}\n    }\n");
    static StringBuilder ms_sb = new StringBuilder(2048);
    //[MenuItem("Tools/传奇/去掉IMESSAGE属性")]
    public static void Replace()
    {
        var dir = Application.dataPath + "/HotFix_Project/Hotfix/Table/";
        var fileNames = System.IO.Directory.GetFiles(dir, "c_table_*.cs");
        for(int i = 0; i < fileNames.Length; ++i)
        {
            if(fileNames[i].EndsWith("_extend.cs"))
            {
                continue;
            }

            var fileName = System.IO.Path.GetFileName(fileNames[i]);
            var fullPath = System.IO.Path.GetFullPath(dir + fileName);
            if (!System.IO.File.Exists(fullPath))
            {
                FNDebug.LogErrorFormat("File Not Found For {0}", fullPath);
                return;
            }
            var content = System.IO.File.ReadAllText(fullPath);
            //Debug.Log(content);
            var match = ms_reg_line.Match(content);
            ms_sb.Clear();
            int pos = 0;
            while (match.Success)
            {
                ms_sb.Append(content.Substring(pos, match.Groups[0].Index - pos));
                pos = match.Groups[0].Index + match.Groups[0].Length;

                var varType = match.Groups[1].Value;
                var varName = match.Groups[4].Value;
                if (varType == "string")
                {
                    ms_sb.Append($"    public {varType} {varName} = \"\";");
                }
                else
                {
                    ms_sb.Append($"    public {varType} {varName};");
                }
                var matchReplace = ms_reg_line.Match(match.Groups[0].Value);
                match = match.NextMatch();
            }
            if (pos < content.Length)
            {
                ms_sb.Append(content.Substring(pos, content.Length - pos));
            }
            System.IO.File.WriteAllText(fileNames[i], ms_sb.ToString());

            FNDebug.LogFormat("<color=#00ff00>[WriteSucceed]:{0}</color>", fileNames[i]);
        }
        AssetDatabase.Refresh();
    }

    public static string ReplaceContent(string content)
    {
        //Debug.Log(content);
        var match = ms_reg_line.Match(content);
        ms_sb.Clear();
        int pos = 0;
        while (match.Success)
        {
            ms_sb.Append(content.Substring(pos, match.Groups[0].Index - pos));
            pos = match.Groups[0].Index + match.Groups[0].Length;

            var varType = match.Groups[1].Value;
            var varName = match.Groups[4].Value;
            if (varType == "string")
            {
                ms_sb.Append($"    public {varType} {varName} = \"\";");
            }
            else
            {
                ms_sb.Append($"    public {varType} {varName};");
            }
            var matchReplace = ms_reg_line.Match(match.Groups[0].Value);
            match = match.NextMatch();
        }
        if (pos < content.Length)
        {
            ms_sb.Append(content.Substring(pos, content.Length - pos));
        }
        return ms_sb.ToString();
    }

    [MenuItem("Tools/传奇/读数据流")]
    public static void ReadStream()
    {
        var excelPath = System.IO.Path.GetFullPath(Application.dataPath + "/../../table/workbook/Item.xls");
        var excelUnit = new Smart.Editor.ExcelUnit(excelPath);
        excelUnit.Init(Application.dataPath,Smart.Editor.ExcelHelper.ConvertMode.CM_IL_FAST_MODE,false);
        excelUnit.LoadProtoBase();
        excelUnit.generateILFastMode();
        excelUnit.Close();
    }
}