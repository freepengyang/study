using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System;
using System.Diagnostics;
using System.Reflection;

//-------------------------------------------------------------------------
//资源打包
//Author jiabao
//Time 2016.2.4
//-------------------------------------------------------------------------

public class TablePacker
{
    struct TableProperty
    {
        public string Name;
        public int Column;
    }

    /// <summary>
    /// 呼叫流程
    /// </summary>
    /// <param name="processName"></param>
    /// <param name="param"></param>
    /// <returns>成功true</returns>
    static bool CallProcess(string processName, string param)
    {
        ProcessStartInfo process = new ProcessStartInfo
        {
            CreateNoWindow = false,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            FileName = processName,
            Arguments = param,
        };

        Process p = Process.Start(process);
        p.WaitForExit();

        string error = p.StandardError.ReadToEnd();
        if (!string.IsNullOrEmpty(error))
        {
            UnityEngine.Debug.LogError(processName + " " + param + "  ERROR! " + "\n" + error);

            string output = p.StandardOutput.ReadToEnd();
            if (!string.IsNullOrEmpty(output))
            {
                UnityEngine.Debug.Log(output);
            }

            return false;
        }

        return true;
    }

    // no postfix (.cs or .proto)
    static bool ProcessTableProto(string name)
    {
        string param = string.Format("-i:{0}.proto -o:{0}.cs -p:detectMissing", name);
        if (CallProcess("protogen.exe", param))
        {
            File.Copy(@".\" + name + ".cs", tableClientPath + name + ".cs", true);
            File.Delete(@".\" + name + ".cs");
            return true;
        }

        return false;
    }

    static void ProcessMsgProto3(string name, bool isHot = true)
    {
        string serPaht = isHot ? serverTableHotPath : serverTablePath;
        string param = $"--csharp_out={serPaht}  {name}.proto";
        string extPath = $"{ServerProtoPath}/protoc3.exe".Replace("/", @"\");
        if (CallProcess(extPath, param))
        {
        }
    }

    static void GeneratePythonFile(string name)
    {
        string param = string.Format("-I. --python_out=. {0}.proto", name);
        CallProcess("protoc.exe", param);
    }


	# region Path

    public static string protoPath
    {
        get
        {
            string curPath = Application.dataPath;

            string str1 = "YHTLClient/Assets";

            if (curPath.Contains(str1))
            {
                return curPath.Replace(str1, "proto");
            }

            return string.Empty;
        }
    } //= @"......\Data\Branch\MapUseData\proto";

    public static string ServerProtoPath
    {
        get
        {
            string curPath = Application.dataPath;

            string str1 = "YHTLClient/Assets";

            if (curPath.Contains(str1))
            {
                return curPath.Replace(str1, "xml/proto");
            }

            return string.Empty;
        }
    } //= @"......\Data\Branch\MapUseData\proto";

    private static string clientPath
    {
        get
        {
            string curPath = Application.dataPath;
            return curPath.Replace("/Assets", "");
        }
    } //= Application.dataPath;

    private static string tablePath
    {
        get
        {
            string curPath = Application.dataPath;

            string str1 = "YHTLClient/Assets";

            if (curPath.Contains(str1))
            {
                return curPath.Replace(str1, "table");
            }

            FNDebug.Log("get proto  path  faile");
            return string.Empty;
        }
    } //= @"......


    private static string tableClientPath = Application.dataPath + "/Main_Project/Table/";
    private static string serverTablePath = Application.dataPath + "/Main_Project/ServerTable/";
    private static string serverTableHotPath = Application.dataPath + "/HotFix_Project/Hotfix/ServerTable/";

    private static string tableBytePath
    {
        get
        {
            string curPath = Application.dataPath;

            string path = string.Empty;

            string str1 = "YHTLClient/Assets";

            if (curPath.Contains(str1))
            {
                path = str1;

                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                {
                    return curPath.Replace(path, "Normal/zt_android/Table/");
                }
                else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
                {
                    return curPath.Replace(path, "Data/Branch/CurrentUseData/wzcq_ios/Table/");
                }
                else
                {
                    return curPath.Replace(path, "Data/Branch/CurrentUseData/wzcq_android/Table/");
                }
            }

            return string.Empty;
        }
    }

	#endregion


    /*[MenuItem("Tools/打前端【Proto】文件")]
    public static void ComplieTableProtoFile()
    {
        Directory.SetCurrentDirectory(protoPath);

        try
        {
            Process p = Process.Start("TortoiseProc.exe", @"/command:update /path:"".\"" /closeonend:1");
            p.WaitForExit();

            ProcessTableProto("table_common");

            string[] fileNames = Directory.GetFiles(@".\");
            foreach (string fileName in fileNames)
            {
                if (fileName.Contains("c_table_") && fileName.Contains(".proto"))
                {
                    string name = fileName.Substring(0, fileName.LastIndexOf('.'));
                    name = name.Replace(".\\", "");
                    if (ProcessTableProto(name) == false)
                    {
                        Directory.SetCurrentDirectory(clientPath);
                        return;
                    }
                    else
                    {
                        AddPropertyToTableCS(fileName);
                    }
                }
            }


            Directory.SetCurrentDirectory(clientPath);

            UnityEngine.Debug.Log("卐  打前端【Proto】文件 Success   卍");
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError(ex);
            Directory.SetCurrentDirectory(clientPath);
        }
    }*/

    public static void AddPropertyToTableCS(string protoFilePath)
    {
        string protoFileName = Path.GetFileName(protoFilePath);
        string className = protoFileName.Replace("c_table_", "").Replace(".proto", "");
        string csFileName = protoFileName.Replace("proto", "cs");
        string csFilePath = tableClientPath + csFileName;

        string replaceStr = "";

        //读取proto文件，解析要添加的属性
        using (StreamReader protoReader = new StreamReader(protoFilePath))
        {
            string saveProperty = "";
            string propertyType = "";
            while (!protoReader.EndOfStream)
            {
                string parseStr = protoReader.ReadLine();
                if (!parseStr.Contains("@")) continue;

                parseStr = System.Text.RegularExpressions.Regex.Replace(parseStr, @"\s+", " ").Trim(' ');
                parseStr = parseStr.Remove(0, parseStr.IndexOf(' ') + 1);
                FNDebug.Log(parseStr);
                if (!string.IsNullOrEmpty(parseStr))
                {
                    saveProperty = "_" + parseStr.Replace("#", "").Replace(" ", "_");
                    propertyType = "uint";

                    string[] newPropertys = parseStr.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    int beginPlace = 0;
                    string newProperty = "";
                    for (int i = newPropertys.Length - 1; i >= 0; i--)
                    {
                        newProperty = newPropertys[i];

                        string[] newPropertyParams =
                            newProperty.Split(new char[] {'#'}, StringSplitOptions.RemoveEmptyEntries);
                        if (newPropertyParams.Length == 2)
                        {
                            replaceStr += string.Format("    public {0} {1} {{ {2} }}\n", propertyType,
                                newPropertyParams[0],
                                GetGetMethodStr2(saveProperty, newPropertyParams[1], ref beginPlace));
                        }
                        else
                        {
                            FNDebug.LogError(string.Format("{0} {0}, 书写错误", protoFilePath, newProperty));
                            continue;
                        }
                    }
                }
            }

            protoReader.Close();
        }

        if (string.IsNullOrEmpty(replaceStr))
            return;

        //读取cs文件
        using (StreamReader csReader = new StreamReader(csFilePath))
        {
            string csText = csReader.ReadToEnd();
            csReader.Close();

            //添加新属性
            string constructorMethod = string.Format("public {0}() {{}}", className.ToUpper());

            if (csText.Contains(constructorMethod))
            {
                csText = csText.Replace(constructorMethod, constructorMethod + "\n" + replaceStr.TrimEnd('\n'));
            }

            using (StreamWriter csWriter = new StreamWriter(csFilePath))
            {
                csWriter.Write(csText);
                csWriter.Flush();
                csWriter.Close();
            }
        }
    }

    static string GetGetMethodStr2(string saveProperty, string placeCount, ref int beginPlace)
    {
        int count = 0;
        if (beginPlace >= 0 && int.TryParse(placeCount, out count) && count > 0)
        {
            string result = string.Format("get {{ return {0}{1}{2}; }}", saveProperty,
                beginPlace > 0 ? string.Format(" >> {0}", beginPlace) : "",
                string.Format(" & {0}", (int) Mathf.Pow(2, count) - 1));
            beginPlace = beginPlace + count;
            return result;
        }

        return "";
    }

    static string GetGetMethodStr(string saveProperty, string placeCount, ref int beginPlace)
    {
        int count = 0;
        if (beginPlace >= 0 && int.TryParse(placeCount, out count) && count > 0)
        {
            int endPlace = beginPlace + count;
            string result = string.Format("get {{ return {0}{1}{2}; }}", saveProperty,
                endPlace < 10 ? string.Format(" % {0}", TenPow(endPlace)) : "",
                beginPlace > 0 ? string.Format(" / {0}", TenPow(beginPlace)) : "");
            beginPlace = endPlace;
            return result;
        }

        return "";
    }

    static string GetPowerTen(int value)
    {
        string result = "1";
        for (int i = 0; i < value; i++)
        {
            result += "0";
        }

        return result;
    }

    static int TenPow(int value)
    {
        return (int) Mathf.Pow(10, value);
    }

    //    [MenuItem("Tools/打【Excel】数据表")]
    //    public static void PackTables()
    //    {
    //        TableLoader.PrivateInstance = new ExtendTableLoader();
    //
    //        Directory.SetCurrentDirectory(protoPath);
    //        Process pp = Process.Start("TortoiseProc.exe", @"/command:update /path:"".\"" /closeonend:1");
    //        pp.WaitForExit();
    //        Directory.SetCurrentDirectory(protoPath);
    //
    //        try
    //        {
    //            GeneratePythonFile("c_table_*");
    //
    //            GeneratePythonFile("table_common");
    //
    //            Directory.SetCurrentDirectory(tablePath);
    //
    //            Process p = Process.Start("TortoiseProc.exe", @"/command:update /path:"".\"" /closeonend:1");
    //            p.WaitForExit();
    //
    //
    //            System.Environment.ExpandEnvironmentVariables(protoPath);
    //
    //            foreach (TableLoader.TableDesc desc in TableLoader.Instance.PackTableList)
    //            {
    //                string param = "";
    //                string bytesName = "";
    //                if (string.IsNullOrEmpty(desc.param_0))
    //                {
    //                    param = string.Format(@"table_writer.py -c ""{0}""", desc.tableName);
    //                    bytesName = desc.tableName.ToLower();
    //                }
    //                else
    //                {
    //                    param = string.Format(@"table_writer.py -c ""{0}"" ""{1}"" ""{2}""", desc.tableName, desc.param_0, desc.param_1);
    //                    bytesName = desc.tableName.ToLower() + "_" + desc.param_0.ToLower();
    //
    //                }
    //
    //                if (CallProcess("python.exe", param))
    //                {
    //                    File.Copy(@".\" + bytesName + ".bytes", tableBytePath + bytesName + ".bytes", true);
    //                    File.Delete(@".\" + bytesName + ".bytes");
    //                }
    //                else
    //                {
    //                    Directory.SetCurrentDirectory(clientPath);
    //                    return;
    //                }
    //
    //                CheckCombineProperties(bytesName, desc.tableName);
    //            }
    //
    //            Directory.SetCurrentDirectory(clientPath);
    //
    //            UnityEngine.Debug.Log("卐  打【Excel】数据表 Success   卍");
    //        }
    //        catch (System.Exception ex)
    //        {
    //            UnityEngine.Debug.LogError(ex);
    //            Directory.SetCurrentDirectory(clientPath);
    //        }
    //    }

    /*public static bool CheckCombineProperties(string bytesName, string excelName)
    {
        string protoFileName = "c_table_" + bytesName + ".proto";
        string protoFilePath = protoPath + "/" + protoFileName;

        List<TableProperty> tableProperties = GetTablePropeties(protoFilePath);

        if (tableProperties == null)
            return false;

        if (tableProperties.Count == 0)
        {
            return true;
        }

        string bytesFilePath = tableBytePath + bytesName + ".bytes";
        string tableName = "TABLE." + bytesName.ToUpper();
        string arrayName = tableName + "ARRAY";
        Assembly a = Assembly.GetAssembly(typeof(CSGame));
        var arrayType = a.GetType(arrayName);
        IEnumerator tableRows = null;
        using (FileStream fs = new FileStream(bytesFilePath, FileMode.Open, FileAccess.Read))
        {
            System.Object arrayData = ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(fs, null, arrayType);
            fs.Close();
            if (arrayData == null)
            {
                Debug.LogError(bytesFilePath + " 反序列化出错！");
                return false;
            }

            var rowsData = arrayType.InvokeMember("rows", BindingFlags.Public | BindingFlags.GetProperty, null,
                arrayData, null);
            tableRows = (rowsData as ICollection).GetEnumerator();
        }

        string excelFilePath = tablePath + "/workbook/" + excelName + ".xls";
        using (System.IO.FileStream fs = System.IO.File.OpenRead(excelFilePath))
        {
            NPOI.HSSF.UserModel.HSSFWorkbook workBook = new NPOI.HSSF.UserModel.HSSFWorkbook(fs);
            fs.Close();
            NPOI.SS.UserModel.ISheet sheet = workBook.GetSheetAt(0);
            System.Object propertyValue = null;
            Type tableType = a.GetType(tableName);
            int rowIndex = 0;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                rowIndex = i + 1;
                NPOI.SS.UserModel.IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    Debug.LogError(string.Format("表格{0}  第{1}行为空！", excelFilePath, rowIndex));
                    continue;
                }

                if (!tableRows.MoveNext())
                {
                    Debug.LogError(string.Format("{0}  读取第{1}行出错！", bytesFilePath, rowIndex));
                    return false;
                }

                for (int j = tableProperties.Count - 1; j >= 0; j--)
                {
                    propertyValue = tableType.InvokeMember(tableProperties[j].Name,
                        BindingFlags.Public | BindingFlags.GetProperty, null, tableRows.Current, null);
                    if (propertyValue == null)
                    {
                        Debug.LogError(string.Format("{0}  读取第{1}行{2}列 {3} 出错！", bytesFilePath, rowIndex,
                            tableProperties[j].Column, tableProperties[j].Name));
                        return false;
                    }

                    NPOI.SS.UserModel.ICell cell = row.GetCell(tableProperties[j].Column - 1);
                    if (cell == null || string.IsNullOrEmpty(cell.ToString()))
                    {
                        if (propertyValue.ToString() == "0")
                            continue;
                        else
                        {
                            Debug.LogError(string.Format("{0}  第{1}行{2}列 {3} != 0 数据不匹配！", excelFilePath, rowIndex,
                                tableProperties[j].Column, propertyValue));
                            return false;
                        }
                    }
                    else
                    {
                        if (propertyValue.ToString() == cell.ToString())
                            continue;
                        else
                        {
                            Debug.LogError(string.Format("{0}  第{1}行{2}列 {3} != {4} 数据不匹配！", excelFilePath, rowIndex,
                                tableProperties[j].Column, propertyValue, cell));
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }*/

    static List<TableProperty> GetTablePropeties(string protoFilePath)
    {
        List<TableProperty> tableProperties = new List<TableProperty>();
        CSStringBuilder.Clear();
        using (StreamReader sr = new StreamReader(protoFilePath))
        {
            string parseStr = "";
            while (!sr.EndOfStream)
            {
                parseStr = sr.ReadLine();
                if (!parseStr.Contains("@")) continue;

                parseStr = System.Text.RegularExpressions.Regex.Replace(parseStr, @"\s+", " ");
                parseStr = parseStr.Remove(0, parseStr.IndexOf('@') + 1);

                if (string.IsNullOrEmpty(parseStr))
                {
                    FNDebug.LogError(protoFilePath + " 解析字符串为空！");
                    return null;
                }

                string[] parseSplits = parseStr.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                if (parseSplits.Length < 3)
                {
                    FNDebug.LogError(protoFilePath + " 合并的字段少于2个！");
                    return null;
                }

                string[] columns = parseSplits[0].Split('#');
                if (columns.Length < 2)
                {
                    FNDebug.LogError(protoFilePath + " 字段的列数错误！");
                    return null;
                }

                if (columns.Length != parseSplits.Length - 1)
                {
                    FNDebug.LogError(protoFilePath + " 字段数和列数不匹配！");
                    return null;
                }


                int column = 0;
                string propertyName = "";
                for (int i = 0; i < columns.Length; i++)
                {
                    propertyName = parseSplits[i + 1];
                    propertyName = propertyName.Remove(propertyName.IndexOf('#'));
                    if (int.TryParse(columns[i], out column) && !string.IsNullOrEmpty(propertyName))
                    {
                        tableProperties.Add(new TableProperty {Name = propertyName, Column = column});
                        CSStringBuilder.Append(propertyName, " ", column.ToString(), ",");
                    }
                    else
                    {
                        FNDebug.LogError(protoFilePath + " 字段数和列数不匹配！");
                        return null;
                    }
                }
            }

            sr.Close();
        }

        if (tableProperties.Count != 0)
        {
            FNDebug.Log(CSStringBuilder.ToString());
        }
        else
        {
            FNDebug.Log(protoFilePath + " 没有合并字段！");
        }

        return tableProperties;
    }

    /*[MenuItem("Tools/打后端【Proto】文件")]
    public static void ComplieServerTableProtoFile()
    {
        Directory.SetCurrentDirectory(ServerProtoPath);

        try
        {
            Process p = Process.Start("TortoiseProc.exe", @"/command:update /path:"".\"" /closeonend:1");
            p.WaitForExit();

            string[] fileNames = Directory.GetFiles(@".\");

            foreach (string fileName in fileNames)
            {
                if (!fileName.Contains("c_table_") && fileName.Contains(".proto")
                    && !fileName.Contains("table_common"))
                {
                    string name = fileName.Substring(0, fileName.LastIndexOf('.'));
                    name = name.Replace(".\\", "");
                    ProcessMsgProto3(name);
                }
            }

            Directory.SetCurrentDirectory(clientPath);

            UnityEngine.Debug.Log("卐  打后端【Proto】文件 Success   卍");
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError(ex);
            Directory.SetCurrentDirectory(clientPath);
        }
    }*/

    //[MenuItem("Tools/检查Tips", false, 2000)]
    //public static void CheckTips()
    //{
    //CheckItemTips();
    //	CheckSkillTips();
    //}

    //    public static void CheckItemTips()
    //    {
    //        string itemPath = tablePath + "/workbook/" + "Item.xls";
    //        NPOI.HSSF.UserModel.HSSFWorkbook workBook = GetWorkbook(itemPath);
    //        NPOI.SS.UserModel.ISheet sheet = workBook.GetSheetAt(0);
    //        NPOI.SS.UserModel.IRow curRow = null;
    //        NPOI.SS.UserModel.ICell curCell = null;
    //
    //        Map<uint, TABLE.ITEM> itemMaps = ItemTableManager.Instance.dic;
    //        int r = 1;
    //        int rowNum = 1;
    //        string cellStr = "";
    //        TABLE.ITEM tableItem = null;
    //        int ErrorCount = 0;
    //        for (itemMaps.Begin(); itemMaps.Next(); r++)
    //        {
    //            rowNum = r + 1;
    //            curRow = sheet.GetRow(r);
    //
    //            if (curRow == null)
    //            {
    //                Debug.LogError(string.Format("读取Item.xls表格的第{0}行为空", rowNum));
    //                return;
    //            }
    //
    //            curCell = curRow.GetCell(8);
    //            cellStr = curCell == null ? "" : curCell.ToString().Replace("\\n", "\n");
    //            tableItem = itemMaps.Value;
    //
    //            if (curCell == null && tableItem.tips() != "" || curCell != null && tableItem.tips() != cellStr)
    //            {
    //                Debug.LogError(string.Format("第{0}行{1} {2} {3}\n{4}\n{5}", rowNum, 9, tableItem.name, tableItem.id, tableItem.tips(), cellStr));
    //                ErrorCount++;
    //            }
    //
    //            curCell = curRow.GetCell(41);
    //            cellStr = curCell == null ? "" : curCell.ToString().Replace("\\n", "\n");
    //
    //            if (curCell == null && tableItem.tips2() != "" || curCell != null && tableItem.tips2() != cellStr)
    //            {
    //                Debug.LogError(string.Format("第{0}行{1} {2} {3}\n{4}\n{5}", rowNum, 42, tableItem.name, tableItem.id, tableItem.tips2(), cellStr));
    //                ErrorCount++;
    //            }
    //        }
    //
    //        if (ErrorCount > 0)
    //        {
    //            Debug.Log(string.Format("<color=red>检查Item.Tips发现{0}个错误!</color>", ErrorCount));
    //        }
    //        else
    //        {
    //            Debug.Log("<color=green>检查Item.Tips无错误!</color>");
    //        }
    //    }

    //public static void CheckSkillTips()
    //{
    //TODO:ddn
    //string itemPath = tablePath + "/workbook/" + "Skill.xls";
    //NPOI.HSSF.UserModel.HSSFWorkbook workBook = GetWorkbook(itemPath);
    //NPOI.SS.UserModel.ISheet sheet = workBook.GetSheetAt(0);
    //NPOI.SS.UserModel.IRow curRow = null;
    //NPOI.SS.UserModel.ICell curCell = null;

    //Map<uint, TABLE.SKILL> skillMaps = SkillTableManager.Instance.dic;
    //int r = 1;
    //int rowNum = 1;
    //string cellStr = "";
    //TABLE.SKILL tableSkill = null;
    //int ErrorCount = 0;
    //for (skillMaps.Begin(); skillMaps.Next(); r++)
    //{
    //    rowNum = r + 1;
    //    curRow = sheet.GetRow(r);

    //    if (curRow == null)
    //    {
    //        Debug.LogError(string.Format("读取Skill.xls表格的第{0}行为空", rowNum));
    //        return;
    //    }

    //    curCell = curRow.GetCell(23);
    //    cellStr = curCell == null ? "" : curCell.ToString().Replace("\\n", "\n");
    //    tableSkill = skillMaps.Value;

    //    try
    //    {
    //        if (curCell == null && tableSkill.description() != "" || curCell != null && tableSkill.description() != cellStr)
    //        {
    //            Debug.LogError(string.Format("第{0}行{1} {2} {3}\n{4}\n{5}", rowNum, 23, tableSkill.name, tableSkill.sid, tableSkill.description(), cellStr));
    //            ErrorCount++;
    //        }
    //    }
    //    catch(Exception e)
    //    {
    //        Debug.LogException(e);
    //        Debug.LogError(string.Format("第{0}行{1} {2} {3}\n{4}", rowNum, 23, tableSkill.name, tableSkill.sid, e));
    //    }
    //}

    //if (ErrorCount > 0)
    //{
    //    Debug.Log(string.Format("<color=red>检查Skill.Tips发现{0}个错误!</color>", ErrorCount));
    //}
    //else
    //{
    //    Debug.Log("<color=green>检查Skill.Tips无错误!</color>");
    //}
    //}

    public static NPOI.HSSF.UserModel.HSSFWorkbook GetWorkbook(string filePath)
    {
        using (FileStream fs = File.OpenRead(filePath))
        {
            NPOI.HSSF.UserModel.HSSFWorkbook workBook = new NPOI.HSSF.UserModel.HSSFWorkbook(fs);
            fs.Close();
            return workBook;
        }
    }

    /*
    /// <summary>编译选中的proto文件</summary>
    [MenuItem("Tools/Protogen Selected Proto File")]
    static void ProtogenSelectedProtoFile()
    {
        //protoPath 表示打开的路径   "proto"表示打开的路径下需要显示的文件的类型  fileName 表示选中的文件的路径
        string fileName = EditorUtility.OpenFilePanel("Select Proto File", protoPath, "proto");

        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogWarning("Please select one proto file");
            return;
        }

        Process.Start("TortoiseProc.exe", string.Format(@"/command:update /path:""{0}"" /closeonend:1", fileName))
            .WaitForExit();
        //获取指定目录中子目录的名称。
        string fileDirectory = Path.GetDirectoryName(fileName);
        //用于获得应用程序当前工作目录
        string defaultDirectory = Directory.GetCurrentDirectory();

        try
        {
            //改变进程的当前目录。
            Directory.SetCurrentDirectory(fileDirectory);

            bool isTable = fileName.Contains("c_table_") || fileName.Contains("table_common"); //是否包含
            string outputPath =
                Application.dataPath + (isTable ? "/Main_Project/Table" : "/ServerTable"); //生成的cs文件的存放路径
            string name = Path.GetFileNameWithoutExtension(fileName); //获取当前选中文件的名字

            CallProcess("protogen", string.Format("-i:{0}.proto -o:{1}/{0}.cs -p:detectMissing", name, outputPath));
            if (isTable) CallProcess("protoc", string.Format("--python_out=./ {0}.proto", name));

            Directory.SetCurrentDirectory(defaultDirectory);
            if (fileName.Contains("c_table_"))
                TablePacker.AddPropertyToTableCS(fileName);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            Directory.SetCurrentDirectory(defaultDirectory);
        }
    }

    /// <summary>读选中的表格</summary>
    [MenuItem("Tools/Read Selected Table")]
    static void ReadSelectedTable()
    {
        //string excelPath = EditorUtility.OpenFilePanel("Select Proto File", Path.GetFullPath("../table/workbook"), "xls");
        string excelPath = EditorUtility.OpenFilePanel("Select Table File", tablePath + "/workbook", "xls");

        if (string.IsNullOrEmpty(excelPath))
        {
            Debug.LogWarning("Please select one xls file");
            return;
        }

        Process.Start("TortoiseProc.exe", string.Format(@"/command:update /path:""{0}"" /closeonend:1", excelPath))
            .WaitForExit();

        //string excelDirectory = Path.GetDirectoryName(excelPath);
        string projectDirectory = Directory.GetCurrentDirectory();
        string excelName = Path.GetFileNameWithoutExtension(excelPath);
        string bytesName = excelName.ToLower();

        try
        {
            //Directory.SetCurrentDirectory("../table");
            Directory.SetCurrentDirectory(tablePath);

            if (CallProcess("python", string.Format(@"table_writer.py -c ""{0}""", excelName)))
            {
                //File.Copy(@".\" + bytesName + ".bytes", @"..\Client\Assets\StreamingAssets\Table\" + bytesName + ".bytes", true);
                File.Copy(@".\" + bytesName + ".bytes", tableBytePath + bytesName + ".bytes", true);
                File.Delete(@".\" + bytesName + ".bytes");
            }

            Directory.SetCurrentDirectory(projectDirectory);

            TablePacker.CheckCombineProperties(bytesName, excelName);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            Directory.SetCurrentDirectory(projectDirectory);
        }
    }
    */

    [MenuItem("Tools/selected【后端】proto  主工程")]
    static void SelectedSeverProtoFileMain()
    {
            SelectedSeverProtoFile(false);
    }

    static void SelectedSeverProtoFile(bool isHot = false)
    {
        string fileName = EditorUtility.OpenFilePanel("Select Server Proto File", ServerProtoPath, "proto");

        if (string.IsNullOrEmpty(fileName))
        {
            FNDebug.LogWarning("Please select one proto file");
            return;
        }

        string fileDirectory = Path.GetDirectoryName(fileName);
        string defaultDirectory = Directory.GetCurrentDirectory();

        try
        {
            Directory.SetCurrentDirectory(fileDirectory);

            Process p = Process.Start("TortoiseProc.exe", @"/command:update /path:"".\"" /closeonend:1");
            p.WaitForExit();

            string name = Path.GetFileNameWithoutExtension(fileName);
            ProcessMsgProto3(name, isHot);

            Directory.SetCurrentDirectory(defaultDirectory);

            UnityEngine.Debug.Log("卐  打后端【Proto】文件 Success   卍");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(ex);
            Directory.SetCurrentDirectory(defaultDirectory);
        }
    }

    [MenuItem("Tools/selected【后端】proto  热更工程")]
    static void SelectedSeverProtoFileHot()
    {
        SelectedSeverProtoFile(true);
    }
}