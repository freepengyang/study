using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
namespace Smart.Editor
{
    class ExcelConfig
    {
        public static bool XLSX_NEED_ADAPT_OLD = false;
        public static string XLSX_PATH = "/../../table/workbook";//"/../../ProtoGen/UpgradeTable/";
        public static string PROTO_PATH = "/../../ProtoGen/protos/";
        public static string PROTO3_PATH = "/../../Proto3/proto/";
        public static string CMP_PROTO_PATH = "/../../orgTable/proto/";
		public static string TABLE_LIST_CS_PATH = "/Scripts/00Common/";
		public static string TXT_SAVE_PATH = "/UGame/LeSiMath/Data/Table/";
        public static string TXT_ASSET_PATH = "Assets/UGame/LeSiMath/Data/Table/";
        public static string PB_TXT_SRC_PATH = "/../../NetMsgProtocol/";
        public static string PB_ASSET_DST_PATH = "Assets/Resources/XLuaCode/protocol/";
        public static string SHELL_CMD_PATH = "/../../shell_cmd/";
        public static string TABLE_SCRIPTS_PATH = "/UGame/LeSiMath/01TableScripts/";
        public static string TABLE_SCRIPTS_CCODE_PATH = "/../CCode/";
    }

    enum FileExtensionType
    {
        FET_XLSX = 0,
        FET_PROTO,
    }

    class ExcelManager
    {
        static ExcelManager ms_handle = null;

        public static ExcelManager Instance()
        {
            if(null == ms_handle)
            {
                ms_handle = new ExcelManager();
            }
            return ms_handle;
        }

        //public bool SaveGlobalQueryTable(string applicationPath, string name, Dictionary<int, ProtoTable.GlobalResQueryTable> data)
        //{
        //    if(null == data)
        //    {
        //        return false;
        //    }

        //    string purName = Path.GetFileNameWithoutExtension(name);
        //    var excelPath = CombinePath(applicationPath, purName, FileExtensionType.FET_XLSX);
        //    if(string.IsNullOrEmpty(excelPath))
        //    {
        //        return false;
        //    }

        //    ExcelUnit unit = new ExcelUnit(excelPath, FileAccess.ReadWrite);
        //    if (!unit.Init())
        //    {
        //        unit.Close();
        //        return false;
        //    }

        //    if(!unit.LoadProtoBase())
        //    {
        //        unit.Close();
        //        return false;
        //    }

        //    if(!unit.SaveGlobalQueryTable(data))
        //    {
        //        unit.Close();
        //        return false;
        //    }

        //    unit.generateText(applicationPath);

        //    bool succeed = unit.succeed;
        //    if(succeed)
        //    {
        //        ExcelHelper.ConvertAsset(unit.SheetName + ".txt");
        //    }

        //    unit.Close();

        //    if(!succeed)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public class ColoumeData
        {
            public int ColumnIndex;
            public string name;
        }

        class MatchedItemData
        {
            public bool IsValid;
            public string required;
            public string type;
            public string name;
            public int filedNumer;
            public string Required
            {
                get
                {
                    if(string.Equals("optional", required))
                    {
                        if(type == "IntList")
                        {
                            return "repeated";
                        }
                        return "required";
                    }
                    return required;
                }
            }

            public string Type
            {
                get
                {
                    if(type == "IntList")
                        return "sint32";
                    if (type == "IntListList")
                        return "string";
                    return type;
                }
            }
        }

        public void InsertNewRows(string excelPath,string storePath)
        {
            var tableName = System.IO.Path.GetFileName(excelPath);

            using (var m_fileStream = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var m_workbook = new HSSFWorkbook(m_fileStream);
                var m_sheet = m_workbook.GetSheetAt(0);
                var sheetName = m_sheet.SheetName;

                int lastRowCount = m_sheet.LastRowNum;
                IRow copyedRow = null;
                int cellLength = 0;
                int keyCell = -1;
                Dictionary<int, int> v2cnt = new Dictionary<int, int>();
                for (int i = 0; i < m_sheet.LastRowNum; ++i)
                {
                    var row = m_sheet.GetRow(i);
                    if (null != row && row.LastCellNum >= 0)
                    {
                        if (!v2cnt.ContainsKey(row.LastCellNum))
                        {
                            v2cnt.Add(row.LastCellNum, 0);
                        }
                        v2cnt[row.LastCellNum] += 1;
                        var v = v2cnt[row.LastCellNum];
                        if (-1 == keyCell || cellLength < v)
                        {
                            keyCell = row.LastCellNum;
                            cellLength = v;
                            copyedRow = row;
                        }
                    }
                }

                for (int i = lastRowCount; i >= 0; --i)
                {
                    var row = m_sheet.GetRow(i);
                    if (null == row)
                    {
                        --lastRowCount;
                        continue;
                    }

                    if (row.LastCellNum < 0)
                    {
                        m_sheet.RemoveRow(row);
                        --lastRowCount;
                        continue;
                    }

                    break;
                }
                int targetRow = lastRowCount + 5;

                for (int i = 0; i < 4; ++i)
                {
                    var row = m_sheet.GetRow(i);
                    if (null == row)
                    {
                        row = m_sheet.CreateRow(i);
                        for (int j = 0; j < keyCell; ++j)
                        {
                            var cell = row.CreateCell(j);
                            cell.SetCellType(CellType.String);
                            cell.SetCellValue(string.Empty);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < keyCell; ++j)
                        {
                            var cell = row.GetCell(j);
                            if (null == cell)
                            {
                                cell = row.CreateCell(j);
                                cell.SetCellType(CellType.String);
                                cell.SetCellValue(string.Empty);
                            }
                        }
                    }
                }

                if (null == copyedRow)
                {
                    FNDebug.LogErrorFormat("Copyed Row Not Exist For:{0}", excelPath);
                    return;
                }

                while (m_sheet.LastRowNum < targetRow)
                {
                    var row = m_sheet.CopyRow(copyedRow.RowNum, 0);
                    for (int j = 0; j < row.LastCellNum; ++j)
                    {
                        var cell = row.GetCell(j);
                        if (null == cell)
                        {
                            cell = row.CreateCell(j);
                        }
                        row.Cells[j].SetCellType(CellType.String);
                        row.Cells[j].SetCellValue(string.Empty);
                    }
                }

                using (var fs = new FileStream(storePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    m_workbook.Write(fs);
                }
            }
        }

        public int GetMaxColoumValues(IRow row)
        {
            int iRet = 0;
            if (null != row)
            {
                for (int j = 0; j < row.Cells.Count; ++j)
                {
                    if (row.Cells[j].CellType == CellType.String && !string.IsNullOrEmpty(row.Cells[j].StringCellValue))
                    {
                        iRet = System.Math.Max(iRet, row.Cells[j].ColumnIndex + 1);
                    }
                }
            }
            return iRet;
        }

        System.Text.RegularExpressions.Regex reg_kv = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z]+[0-9]+$");
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"([/]{0,2})\s*(required|repeated|optional)\s*(sint32|uint32|string|IntList|IntListList)\s*(\w+)\s*=\s*(\d+)\s*;");
        public void UpgradeTable(string applicationPath,string excelPath)
        {
            var tableName = System.IO.Path.GetFileName(excelPath);
            excelPath = Application.dataPath + $"/../../table/workbook/{tableName}";
            if (!File.Exists(excelPath))
            {
                FNDebug.LogErrorFormat("File Not Exist For Excel:{0}", excelPath);
                return;
            }

            var tableNameWithOutExtend = System.IO.Path.GetFileNameWithoutExtension(excelPath);
            var protoPath = Application.dataPath + $"/../../proto/c_table_{tableNameWithOutExtend.ToLower()}.proto";

            var storePath = System.IO.Path.GetFullPath(Application.dataPath + $"/../../ProtoGen/UpgradeTable/{tableName[0].ToString().ToUpper() + tableName.Substring(1)}");
            if (System.IO.File.Exists(storePath))
            {
                System.IO.File.Delete(storePath);
            }

            InsertNewRows(excelPath,storePath);

            using (var m_fileStream = new FileStream(storePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var m_workbook = new HSSFWorkbook(m_fileStream);
                var m_sheet = m_workbook.GetSheetAt(0);
                var sheetName = m_sheet.SheetName;
                if(sheetName != tableNameWithOutExtend)
                {
                    var oldName = sheetName;
                    sheetName = tableNameWithOutExtend[0].ToString().ToUpper() + tableNameWithOutExtend.Substring(1);
                    m_workbook.SetSheetName(0, sheetName);
                    FNDebug.LogWarningFormat("<color=#00ffff>sheetName HasBeenModified From {0} To {1} </color>", oldName, m_sheet.SheetName);
                }

                const int TitleLine = 8;
                var firstRow = m_sheet.GetRow(TitleLine);
                int coloumLength = GetMaxColoumValues(firstRow);
                ColoumeData[] coloumes = new ColoumeData[coloumLength];
                Dictionary<string, ColoumeData> mColoumDic = new Dictionary<string, ColoumeData>(coloumLength);
                for (int i = 0; i < coloumes.Length; ++i)
                {
                    var cell = firstRow.Cells[i];
                    if (null == cell || cell.CellType != CellType.String)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(cell.StringCellValue))
                    {
                        continue;
                    }

                    coloumes[i] = new ColoumeData
                    {
                        ColumnIndex = cell.ColumnIndex,
                        name = cell.StringCellValue.Trim(),
                    };
                    mColoumDic.Add(coloumes[i].name, coloumes[i]);
                }

                Dictionary<string, MatchedItemData> mDicKey2MatchItems = new Dictionary<string, MatchedItemData>(coloumLength);
                var protoContent = string.Empty;
                bool hasProto = false;
                if (File.Exists(protoPath))
                {
                    protoContent = System.IO.File.ReadAllText(protoPath);
                    hasProto = true;
                }
                bool hasId = false;
                
                var match = reg.Match(protoContent);
                while(match.Success)
                {
                    var key = match.Groups[4].Value.ToLower();
                    mDicKey2MatchItems.Add(key, new MatchedItemData
                    {
                        IsValid = string.IsNullOrEmpty(match.Groups[1].Value),
                        required = match.Groups[2].Value,
                        type = match.Groups[3].Value,
                        name = key,
                        filedNumer = int.Parse(match.Groups[5].Value),
                    });
                    match = match.NextMatch();
                }

                var contentRow = m_sheet.GetRow(9);
                string PrimaryKeyName = string.Empty;
                bool hasSpecialPrimaryKey = false;
                int PrimaryKeyIdx = -1;

                for (int i = 0; i < 5; ++i)
                {
                    var row = m_sheet.GetRow(i);
                    for (int j = 0; j < coloumes.Length; ++j)
                    {
                        if(null != coloumes[j])
                        {
                            ICell cell = row.CreateCell(coloumes[j].ColumnIndex);
                            var matchKey = coloumes[j].name.ToLower();

                            if (mDicKey2MatchItems.ContainsKey(matchKey))
                            {
                                var matchItem = mDicKey2MatchItems[matchKey];
                                if (i == 0)
                                {
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue(matchItem.Required);
                                }
                                else if(i == 1)
                                {
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue(matchItem.Type);
                                }
                                else if (i == 2)
                                {
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue(coloumes[j].name);
                                }
                                else if (i == 3)
                                {
                                    cell.SetCellType(CellType.Numeric);
                                    cell.SetCellValue(matchItem.IsValid ? 1 : 0);
                                    if (matchItem.IsValid)
                                    {
                                        var contentCell = contentRow.GetCell(cell.ColumnIndex);
                                        if(null != contentCell && contentCell.CellType == CellType.Numeric)
                                        {
                                            if(!hasSpecialPrimaryKey)
                                            {
                                                if (!hasId)
                                                {
                                                    if (coloumes[j].name.ToLower().Contains("id"))
                                                    {
                                                        PrimaryKeyName = coloumes[j].name;
                                                        hasId = true;
                                                        PrimaryKeyIdx = j;
                                                    }
                                                }
                                            }
                                        }
                                        else if(null != contentCell && contentCell.CellType == CellType.Formula)
                                        {
                                            if(contentCell.CachedFormulaResultType == CellType.Numeric && !hasSpecialPrimaryKey)
                                            {
                                                if (coloumes[j].name.Contains("_"))
                                                {
                                                    var tokens = coloumes[j].name.Trim().Split('_');
                                                    bool ok = true;
                                                    for (int k = 0; k < tokens.Length; ++k)
                                                    {
                                                        if (!reg_kv.IsMatch(tokens[k]))
                                                        {
                                                            ok = false;
                                                            break;
                                                        }
                                                    }
                                                    if (ok)
                                                    {
                                                        PrimaryKeyName = coloumes[j].name;
                                                        hasSpecialPrimaryKey = true;
                                                        hasId = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    cell.SetCellType(CellType.String);
                                    var NameRow = m_sheet.GetRow(2);
                                    var NameCell = NameRow.GetCell(cell.ColumnIndex);
                                    if(NameCell.StringCellValue.Contains("_"))
                                    {
                                        var tokens = NameCell.StringCellValue.Trim().Split('_');
                                        bool ok = true;
                                        for(int k = 0; k < tokens.Length; ++k)
                                        {
                                            if(!reg_kv.IsMatch(tokens[k]))
                                            {
                                                ok = false;
                                                break;
                                            }
                                        }
                                        if(ok)
                                            cell.SetCellValue($"bit:{{{coloumes[j].name}}}");
                                        else
                                            cell.SetCellValue(coloumes[j].name);
                                    }
                                    else
                                    {
                                        cell.SetCellValue(coloumes[j].name);
                                    }
                                }
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue("required");
                                }
                                else if (i == 1)
                                {
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue("string");
                                    var contentCell = contentRow.GetCell(cell.ColumnIndex);
                                    if (null != contentRow && null != contentCell)
                                    {
                                        if(contentCell.CellType == CellType.String)
                                        {
                                            cell.SetCellValue("string");
                                        }
                                        else if(contentCell.CellType == CellType.Numeric)
                                        {
                                            cell.SetCellValue("sint32");
                                        }
                                        else if(contentCell.CellType == CellType.Boolean)
                                        {
                                            cell.SetCellValue("sint32");
                                        }
                                        else if (contentCell.CellType == CellType.Formula)
                                        {
                                            if (contentCell.CachedFormulaResultType == CellType.Numeric)
                                            {
                                                cell.SetCellValue("sint32");
                                            }
                                            else if (contentCell.CachedFormulaResultType == CellType.String)
                                            {
                                                cell.SetCellValue("string");
                                            }
                                        }
                                    }
                                }
                                else if (i == 2)
                                {
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue(coloumes[j].name);
                                }
                                else if (i == 3)
                                {
                                    cell.SetCellType(CellType.Numeric);
                                    cell.SetCellValue(0);

                                    if(!hasProto)
                                    {
                                        var contentCell = contentRow.GetCell(cell.ColumnIndex);
                                        if (null != contentCell && contentCell.CellType == CellType.Numeric)
                                        {
                                            if (!hasSpecialPrimaryKey)
                                            {
                                                if (!hasId)
                                                {
                                                    if (coloumes[j].name.ToLower().Contains("id"))
                                                    {
                                                        PrimaryKeyName = coloumes[j].name;
                                                        hasId = true;
                                                        PrimaryKeyIdx = j;
                                                    }
                                                }
                                            }
                                        }
                                        else if (null != contentCell && contentCell.CellType == CellType.Formula)
                                        {
                                            if (contentCell.CachedFormulaResultType == CellType.Numeric && !hasSpecialPrimaryKey)
                                            {
                                                if (coloumes[j].name.Contains("_"))
                                                {
                                                    var tokens = coloumes[j].name.Trim().Split('_');
                                                    bool ok = true;
                                                    for (int k = 0; k < tokens.Length; ++k)
                                                    {
                                                        if (!reg_kv.IsMatch(tokens[k]))
                                                        {
                                                            ok = false;
                                                            break;
                                                        }
                                                    }
                                                    if (ok)
                                                    {
                                                        PrimaryKeyName = coloumes[j].name;
                                                        hasSpecialPrimaryKey = true;
                                                        hasId = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue(coloumes[j].name);
                                }
                            }
                        }
                    }
                }

                if (!hasProto && PrimaryKeyIdx >= 0)
                {
                    var row = m_sheet.GetRow(3);
                    row.GetCell(PrimaryKeyIdx).SetCellValue(1);
                }

                if(string.IsNullOrEmpty(PrimaryKeyName) && null != contentRow)
                {
                    var nameRow = m_sheet.GetRow(2);
                    for (int i = 0; null != nameRow && i < contentRow.Cells.Count;++i)
                    {
                        var cell = contentRow.Cells[i];
                        var nameCell = nameRow.GetCell(cell.ColumnIndex);
                        if (null != nameCell && nameCell.CellType == CellType.String && null != cell && cell.CellType == CellType.Numeric)
                        {
                            PrimaryKeyName = nameCell.StringCellValue;
                            hasId = true;
                        }
                    }
                }
                
                if (!string.IsNullOrEmpty(PrimaryKeyName))
                {
                    if(true)
                    {
                        var row = m_sheet.GetRow(0);
                        ICell cell = row.GetCell(coloumLength);
                        if (null == cell)
                        {
                            cell = row.CreateCell(coloumLength);
                        }
                        cell.SetCellType(CellType.String);
                        cell.SetCellValue("required");
                    }
                    if (true)
                    {
                        var row = m_sheet.GetRow(1);
                        ICell cell = row.GetCell(coloumLength);
                        if (null == cell)
                        {
                            cell = row.CreateCell(coloumLength);
                        }
                        cell.SetCellType(CellType.String);
                        cell.SetCellValue("sint32");
                    }
                    if (true)
                    {
                        var row = m_sheet.GetRow(2);
                        ICell cell = row.GetCell(coloumLength);
                        if (null == cell)
                        {
                            cell = row.CreateCell(coloumLength);
                        }
                        cell.SetCellType(CellType.String);
                        cell.SetCellValue("PrimaryKey");
                    }
                    if (true)
                    {
                        var row = m_sheet.GetRow(3);
                        ICell cell = row.GetCell(coloumLength);
                        if (null == cell)
                        {
                            cell = row.CreateCell(coloumLength);
                        }
                        if(cell.CellType != CellType.Numeric)
                        {
                            row.RemoveCell(cell);
                            cell = row.CreateCell(coloumLength);
                        }
                        cell.SetCellType(CellType.Numeric);
                        cell.SetCellValue(0);
                    }
                    if (true)
                    {
                        var row = m_sheet.GetRow(4);
                        ICell cell = row.GetCell(coloumLength);
                        if (null == cell)
                        {
                            cell = row.CreateCell(coloumLength);
                        }
                        cell.SetCellType(CellType.String);
                        cell.SetCellValue($"key:{{{PrimaryKeyName}}}");
                    }

                    using (var fs = new FileStream(storePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        m_workbook.Write(fs);
                    }
                }
                else
                {
                    FNDebug.LogErrorFormat("Has No PrimaryKey !!! {0}",m_sheet.SheetName);
                }
            }
        }

		public bool Convert(string applicationPath,string excelPath, ExcelHelper.ConvertType eConvertType,out string sheetName, ExcelHelper.ConvertMode convertMode = ExcelHelper.ConvertMode.CM_PROTO3,bool model = false)
        {
            sheetName = string.Empty;
            if (ExcelConfig.XLSX_NEED_ADAPT_OLD)
            {
                var fileName = System.IO.Path.GetFileName(excelPath);
                excelPath = applicationPath + $"/../../ProtoGen/UpgradeTable/{fileName}";
            }

            if(!string.IsNullOrEmpty(excelPath))
            {
                ExcelUnit unit = new ExcelUnit(excelPath);
                unit.Init(Application.dataPath, convertMode, model);
                unit.LoadProtoBase();
                if (unit.succeed)
                {
                    sheetName = unit.SheetName;
                    if (eConvertType == ExcelHelper.ConvertType.CT_PROTO)
                    {
                        unit.CreateProto(applicationPath);
                    }
                    else if (eConvertType == ExcelHelper.ConvertType.CT_CSHARP)
                    {
                        if(convertMode != ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
                        {
                            unit.generateCSharpCode(applicationPath, applicationPath + "/../../ProtoGen/table/", unit);
                        }
                        else
                        {
                            unit.generateCSharpCodeForILFastMode(applicationPath, applicationPath + "/../../ProtoGen/table/", unit);
                        }
                    }
                    else
                    {
                        unit.generateAsset(excelPath);
                    }
                }
                unit.Close();
                return unit.succeed;
            }
            return false;
        }

        string CombinePath(string applicationPath,string name, FileExtensionType eFileExtensionType)
        {
            switch(eFileExtensionType)
            {
                case FileExtensionType.FET_PROTO:
                    {
                        return Path.GetFullPath(applicationPath + ExcelConfig.PROTO_PATH + name + ".proto");
                    }
                case FileExtensionType.FET_XLSX:
                    {
                        return Path.GetFullPath(applicationPath + ExcelConfig.XLSX_PATH + name + ".xls");
                    }
            }
            return string.Empty;
        }
    }
}