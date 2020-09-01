using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;

public class CombinePropertyWindow : EditorWindow
{
    //[MenuItem("Tools/检查合并字段", false, 2000)]
    //public static void OpenWindow()
    //{
    //    string filePath = EditorUtility.OpenFilePanel("Check Proto", TablePacker.protoPath, "proto");
    //    List<List<string>> data = GetTableCombinePropertys(filePath);
    //    CombinePropertyWindow window = EditorWindow.GetWindow<CombinePropertyWindow>(Path.GetFileNameWithoutExtension(filePath), true);
    //    window.tables = data;
    //    window.scrollPos = Vector2.zero;
    //    window.selectedRow = 1;
    //}
    Vector2 scrollPos = Vector2.zero;
    int selectedRow = 1;

    public List<List<string>> tables;
    private void OnGUI()
    {
        if (tables == null || tables.Count == 0) return;

        int rowCount = tables.Count;
        int columnCount = tables[0].Count;

        NGUIEditorTools.DrawSeparator();
        GUILayout.BeginHorizontal();
        GUILayout.Label("1", "TextField", GUILayout.MaxWidth(40));
        for (int i = 0; i < columnCount; i++)
        {
            if (GUILayout.Button(tables[0][i], "TextField", GUILayout.MaxWidth(100)))
            {
                ShowContextMenu(i);
            }
        }
        GUILayout.EndHorizontal();
        NGUIEditorTools.DrawSeparator();

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        {
            for (int i = 1; i < rowCount; i++)
            {
                GUILayout.BeginHorizontal();
                GUI.backgroundColor = i == selectedRow ? Color.blue : Color.white;
                GUILayout.Label((i + 1).ToString(), "TextField", GUILayout.MaxWidth(35));
                for (int j = 0; j < columnCount; j++)
                {
                    if (GUILayout.Button(tables[i][j], "TextField", GUILayout.MaxWidth(100)))
                    {
                        selectedRow = i;
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
    }

    public HashSet<uint> hashs = new HashSet<uint>();
    void ShowContextMenu(int column)
    {
        hashs.Clear();
        for (int i = 1; i < tables.Count; i++)
        {
            hashs.Add(uint.Parse(tables[i][column]));
        }

        uint[] array = new uint[hashs.Count];
        hashs.CopyTo(array);
        Array.Sort(array);

        for (int i = 0; i < array.Length; i++)
        {
            NGUIContextMenu.AddItem(array[i].ToString(), false, null, null);
        }
        NGUIContextMenu.Show();
    }

    //public static List<List<string>> GetTableCombinePropertys(string filePath)
    //{
    //    if (!File.Exists(filePath))
    //    {
    //        Debug.LogError("找不到文件!");
    //        return null;
    //    }
    //    string protoFileName = Path.GetFileNameWithoutExtension(filePath);
    //    string tableNameUpper = protoFileName.Replace("c_table_", "").ToUpper();

    //    string tableTypeName = "TABLE." + tableNameUpper;

    //    Assembly assembly = Assembly.GetAssembly(typeof(CSGame));

    //    Type tableType = assembly.GetType(tableTypeName);

    //    if (tableType == null)
    //    {
    //        Debug.LogError("找不到表格类: " + protoFileName);
    //        return null;
    //    }

    //    List<string> propertyNames = new List<string>();
    //    StreamReader sr = new StreamReader(filePath);
    //    string parseStr = "";
    //    while (!sr.EndOfStream)
    //    {
    //        parseStr = sr.ReadLine();
    //        if (parseStr.Contains("@"))
    //        {
    //            parseStr = System.Text.RegularExpressions.Regex.Replace(parseStr, @"\s+", " ").Trim(' ');
    //            parseStr = parseStr.Remove(0, parseStr.IndexOf(' ') + 1);
    //            string[] tempPropertys = parseStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    //            for (int i = 0; i < tempPropertys.Length; i++)
    //            {
    //                string s = tempPropertys[i];
    //                propertyNames.Add(s.Remove(s.IndexOf('#')));
    //            }
    //        }
    //    }
    //    sr.Close();

    //    if (propertyNames.Count == 0)
    //    {
    //        Debug.LogError("没有合并字段!");
    //        return null;
    //    }

    //    TableLoader.TableDesc td = null;
    //    string tableName = "";
    //    for (int i = 0; i < TableLoader.Instance.PackTableList.Count; i++)
    //    {
    //        td = TableLoader.Instance.PackTableList[i];
    //        if (td.tableName.ToUpper() == tableNameUpper)
    //        {
    //            tableName = td.tableName;
    //            break;
    //        }
    //    }

    //    if (string.IsNullOrEmpty(tableName)) return null;

    //    Type tableManagerType = assembly.GetType(tableName + "TableManager");
    //    if (tableManagerType == null)
    //    {
    //        Debug.LogError("找不到" + tableName + "TableManager");
    //        return null;
    //    }

    //    //System.Object instObj = ReflectionObj(tableManagerType, "Instance", null, BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
    //    System.Object instObj = tableManagerType.InvokeMember("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.FlattenHierarchy, null, null, null);

    //    if (instObj == null) return null;

    //    System.Object dicObj = tableManagerType.InvokeMember("dic", BindingFlags.Public | BindingFlags.GetField, null, instObj, null);

    //    if (dicObj == null) return null;

    //    Type dicType = dicObj.GetType();
    //    MethodInfo beginM = dicType.GetMethod("Begin");
    //    if (beginM == null) return null;
    //    MethodInfo nextM = dicType.GetMethod("Next");
    //    if (nextM == null) return null;

    //    List<List<string>> result = new List<List<string>>();
    //    result.Add(propertyNames);

    //    for (beginM.Invoke(dicObj, null); (bool)nextM.Invoke(dicObj, null);)
    //    {
    //        System.Object valueObj = dicType.InvokeMember("Value", BindingFlags.Public | BindingFlags.GetProperty, null, dicObj, null);
    //        if (valueObj == null) continue;
    //        List<string> values = new List<string>();
    //        result.Add(values);
    //        for (int i = 0; i < propertyNames.Count; i++)
    //        {
    //            string pName = propertyNames[i];
    //            string value = valueObj.GetType().InvokeMember(pName, BindingFlags.Public | BindingFlags.GetProperty, null, valueObj, null).ToString();
    //            values.Add(value);
    //        }
    //    }

    //    return result;
    //}

    public static System.Object ReflectionObj(Type type, string propertyName, System.Object obj,
        BindingFlags bindingAttr = BindingFlags.Default, params System.Object[] parameters)
    {
        PropertyInfo instanceP = null;
        while (type != null)
        {
            instanceP = type.GetProperty(propertyName, bindingAttr);

            if (instanceP == null)
            {
                type = type.BaseType;
            }
            else
            {
                break;
            }
        }

        if (instanceP == null)
        {
            FNDebug.LogError("找不到PropertyInfo: " + propertyName);
            return null;
        }

        MethodInfo instGetM = instanceP.GetGetMethod();

        if (instGetM == null) return null;

        return instGetM.Invoke(obj, parameters);
    }
}

