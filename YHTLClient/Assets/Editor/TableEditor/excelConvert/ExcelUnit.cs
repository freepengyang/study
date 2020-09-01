using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using System.Text.RegularExpressions;
using System.Text;

using System.Reflection;
using System;
using System.Threading;

using UnityEngine.Events;
using UnityEditor;

using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.XR;

namespace Smart.Editor
{
    enum ErrCode
    {
        EC_SUCCEED = 0,
        EC_NO_FILE,
        EC_NO_SHEET,
        EC_SHEET_NAME_INVALID,
        EC_NO_PROTO_HEAD_ROW,
        EC_PROTO_HEAD_MATCH_FAILED,
        EC_NO_PROTO_VAR_TYPE_ROW,
        EC_PROTO_VAR_TYPE_MATCH_FAILED,
        EC_NO_PROTO_VAR_NAME_ROW,
        EC_PROTO_VAR_NAME_MATCH_FAILED,
        EC_NO_SERVER_CLIENT_FLAG_ROW,
        EC_NO_ANNOTATION_ROW,
        EC_CONVERT_PROTO_FAILED,
        EC_WRITE_PROTO_FAILED,
        EC_ENUM_DECLARE_CONVERT_IS_EMPTY,
		EC_NO_INITIALIZE,
		EC_VAR_TYPE_CONVERT_FAILED,
		EC_VAR_VALUE_CONVERT_FAILED,
		EC_ASSEMBLY_CLASS_CREATE_INSTANCE_FAILED,
		EC_HAS_NO_COLOUM,
		EC_PROTO_NOT_MATCH_CS,
        EC_CONVERT_CS_FAILED,
		EC_VAR_NAME_REPEATED,
		EC_PARSE_BIT_COMBINE_ERROR,
		EC_PARSE_PRIMARY_KEY_ERROR,
		EC_PARIMARY_KEY_HAS_NOT_ATTACHED_FILED,
		EC_PARIMARY_KEY_EMPTY,
		EC_HAS_NO_PARIMARY_KEY,
		EC_HAS_MORE_THAN_ONE_PARIMARY_KEY,
		EC_PARSE_PRIMARY_KEY_NAME_ERROR,
		EC_PRIMARY_KEY_TYPE_ERROR,
		EC_FIRST_COLOUM_ERROR,
		EC_COUNT,
    }

	enum VarType
	{
		VT_SINT32 = 0,
		VT_UINT32,
		VT_INT64,
		VT_FLOAT,
		VT_STRING,
		VT_UNION,
		VT_BOOL,
		VT_ENUM,
		VT_PAIR,
		VT_COUNT,
	}

	public class BitVarInfo
	{
		public string key;
		public int value;
		public string definitions;
		public string typeString;
		public string makeKeyFunction;
	}

    class ExcelColoumValue
    {
        public int iIndex;
        public string head;
        public string type;
        public string name;
		public string propertyName;
        public bool isValidToSearver = false;
		public bool isPrimaryKey = false;
		public List<BitVarInfo> bits = null;
		public string make_id_function = string.Empty;
        public string declare;
		public VarType eVarType = VarType.VT_COUNT;
		public bool bUnionFloat = false;
		public List<int> enumValues = null;
		public List<string> keys = null;
		public bool isList = false;
		public bool hasEnumValue(int iValue)
		{
			return null != enumValues && enumValues.Contains (iValue);
		}

		public object toValue(string value)
		{
			if (eVarType != VarType.VT_COUNT) 
			{
				if (eVarType == VarType.VT_STRING) 
				{
					return value;
				}

				if (eVarType == VarType.VT_BOOL) 
				{
					value = value.Trim ();
					int iValue = 0;
					if (int.TryParse (value, out iValue)) 
					{
						return (bool)(iValue != 0);
					}
				}

				if (eVarType == VarType.VT_FLOAT) 
				{
					value = value.Trim ();
					int iValue = 0;
					float fValue = 0.0f;
					if (float.TryParse (value, out fValue)) 
					{
						iValue = (int)(fValue * 1000.0f + 0.50f);
						return (int)iValue;
					}
				}

				if (eVarType == VarType.VT_SINT32) 
				{
					value = value.Trim();
					int iValue = 0;
					int.TryParse(value, out iValue);
					return iValue;
				}

                if (eVarType == VarType.VT_INT64)
                {
                    value = value.Trim();
                    long iValue = 0;
					long.TryParse(value, out iValue);
					return iValue;
                }

                if (eVarType == VarType.VT_UINT32)
                {
                    value = value.Trim();
                    UInt32 iValue = 0;
					UInt32.TryParse(value, out iValue);
					return iValue;
                }

                if (eVarType == VarType.VT_ENUM) 
				{
					value = value.Trim ();
					int iValue = 0;
					if (int.TryParse (value, out iValue)) 
					{
						if (hasEnumValue (iValue)) 
						{
							return iValue;
						}
					}
				}

				if (eVarType == VarType.VT_UNION) 
				{
					var retValue = _constructUnionCell (value, bUnionFloat);
					return retValue;
				}

				if (eVarType == VarType.VT_PAIR)
				{
					var tokens = value.Split('#');
					int k = 0;
					long v = 0;
					if(tokens.Length != 2 || !int.TryParse(tokens[0],out k) ||!long.TryParse(tokens[1],out v))
					{
						return null;
					}

					return new TABLE.KEYVALUE
					{
						key = k,
						value = v,
					};
				}
			}
			return null;
		}

		private int _convertToInt(string value, bool isFloat = false)
		{
			int iValue = 0;
			try
			{
				iValue = isFloat ? (int)(Convert.ToDouble(value) * 1000) : Convert.ToInt32(value);
			}
			catch(Exception e) 
			{
				Debug.LogErrorFormat ("value = {0} isFloat = {1}", value,isFloat);
				Debug.LogErrorFormat (e.ToString ());
			}
			return iValue;
		}

		private object _constructUnionCell(string content, bool isFloat = false)
		{
			//const string FIX_EVERY_SPLIT = ",";
			//const string FIX_GROW_SPLIT = ";";
			if (string.IsNullOrEmpty (content) || content.Equals ("-")) {
				content = 0.ToString ();
			}

            //ProtoTable.UnionCell unionCell = new ProtoTable.UnionCell();

            //if (content.Contains(FIX_EVERY_SPLIT))
            //{
            //	unionCell.valueType = ProtoTable.UnionCellType.union_everyvalue;
            //	ProtoTable.EveryValue values = new ProtoTable.EveryValue();

            //	var stringValues = content.Split(FIX_EVERY_SPLIT[0]);
            //	foreach(var sv in stringValues)
            //	{
            //		values.everyValues.Add(_convertToInt(sv, isFloat));
            //	}

            //	unionCell.eValues = values;
            //}
            //else if (content.Contains(FIX_GROW_SPLIT))
            //{
            //	unionCell.valueType = ProtoTable.UnionCellType.union_fixGrow;

            //	var stringValues = content.Split(FIX_GROW_SPLIT[0]);
            //	if (stringValues.Length != 2)
            //	{
            //		Debug.LogErrorFormat("[GenerateText] union format error {0}", content);
            //		return null;
            //	}

            //	unionCell.fixInitValue = _convertToInt(stringValues[0], isFloat);
            //	unionCell.fixLevelGrow = _convertToInt(stringValues[1], isFloat);
            //}
            //else
            //{
            //	unionCell.valueType = ProtoTable.UnionCellType.union_fix;
            //	unionCell.fixValue = _convertToInt(content, isFloat);
            //}

            //return (object)(unionCell);
            return (object)(null);
        }

		public void loadVarType()
		{
			switch (type) 
			{
			case "sint32":
				{
					eVarType = VarType.VT_SINT32;
				}
				break;
				case "int64":
                {
                    eVarType = VarType.VT_INT64;
                }
                    break;
            case "uint32":
                {
                    eVarType = VarType.VT_UINT32;
                }
                break;
                case "float":
				{
					eVarType = VarType.VT_FLOAT;
				}
				break;
			case "string":
				{
					eVarType = VarType.VT_STRING;
				}
				break;
			case "enum":
				{
					eVarType = VarType.VT_ENUM;
				}
				break;
			case "bool":
				{
					eVarType = VarType.VT_BOOL;
				}
				break;
			case "union":
				{
					eVarType = VarType.VT_UNION;
				}
				break;
				case "pair":
                {
                    eVarType = VarType.VT_PAIR;
				}
                break;
                default:
				{
					eVarType = VarType.VT_COUNT;
				}
				break;
			}
		}
    }

    class ExcelUnit
    {
        static string[] m_errMsg = new string[(int)ErrCode.EC_COUNT]
        {
            string.Empty,
            @"file not exist !",
            @"m_workbook is empty , has no sheet !",
            @"sheet name is invalid, is null or empty !",
            @"proto head line not exist !",
            @"proto head just support {0}",
            @"proto var type line not exist !",
            @"proto var type just support {0}",
            @"proto var name line not exist !",
            @"proto var name just support {0}",
            @"no server_client_flag row !",
            @"no annotation row !",
            @"convert proto content failed !",
            @"write proto {0} failed !",
            @"enum declare convert failed !",
			@"initialize has not been called !",
			@"var type={0} int table [{1}] convert failed !",
			@"var type={0} value={1} convert failed !",
			@"assembly create instance cls={0} failed!",
			@"has no coloum {0} {1}:{2}!",
			@"proto no matched cs {0} !",
            @"convert .cs file failed for {0} ! reason:{1}",
            @"var name repeated {0} col {1} with col {2}",
			@"try parse bit combaine error",
			@"try parse primary key error",
			@"primary key has no combined key named {0}",
			@"has no primary key is empty",
			@"has no primary key",
			@"has more than one primary key",
			@"primary key's name must be [PrimaryKey]",
			@"主键与他的组成部分必须是sint32或uint32 [{0}] [{1}]",
			@"主键Id列定义错误 格式:required sint32 id 1,SheetName = {0}",
		};

        void _PrintErrMsg(ErrCode eErrCode,params object[] param)
        {
            this.m_eErrCode = eErrCode;
			if (eErrCode > ErrCode.EC_SUCCEED && eErrCode < ErrCode.EC_COUNT)
            {
				Debug.LogErrorFormat(m_errMsg[(int)eErrCode],param);
            }
        }

        static int PROTO_HEAD_ROW = 0;
        static int PROTO_VAR_TYPE_ROW = 1;
        static int PROTO_VAR_NAME_ROW = 2;
        static int SERVER_CLIENT_FLAG_ROW = 3;
        static int ANNOTATION_ROW = 4;
        static int CONTENT_START_ROW = 9;
        static Regex ms_proto_head_reg = new Regex(@"(^\s*required\s*$|^\s*repeated\s*$)",RegexOptions.Singleline);
        static Regex ms_proto_var_type_reg = new Regex(@"(uint32|sint32|string|enum|bool|float|pair|union|union\(float\))", RegexOptions.Singleline);
        static Regex ms_proto_var_name_reg = new Regex(@"(^\s*[a-zA-Z_][a-zA-Z_0-9]*\s*$|^\s*[a-zA-Z_][a-zA-Z_0-9]*:[0-9]*\s*$)", RegexOptions.Singleline);
        //static Regex ms_server_client_reg = new Regex(@"^\s*([1])\s*$",RegexOptions.Singleline);
        static Regex ms_enum_prop_reg = new Regex(@"([a-zA-Z_][0-9a-zA-Z_]*):(\-*\d+):\s*([^\s]+)\s*", RegexOptions.Singleline);
		static Regex ms_proto_merge_var_reg = new Regex(@"([a-zA-Z]+)(\d+)");
		static Regex ms_special_annotation_content_reg = new Regex(@"^(bit)+:{(.+)}$", RegexOptions.Singleline);

		public string ProtoNameWithExtend
		{
			get
			{
				if(null == m_sheet)
				{
					return string.Empty;
				}
				return string.Format("c_table_{0}.proto", SheetName.ToLower());
			}
		}

        public string ProtoNameWithOutExtend
        {
            get
            {
                if (null == m_sheet)
                {
                    return string.Empty;
                }
                return string.Format("c_table_{0}", SheetName.ToLower());
            }
        }

        FileStream m_fileStream;
        IWorkbook m_workbook;
        ISheet m_sheet;
        IRow headLineRow = null;
        IRow protoVarTypeRow = null;
        IRow protoVarNameRow = null;
        IRow serverClientFlagRow = null;
        IRow annotationRow = null;
        bool m_bHasUnion = false;
		Dictionary<string,int> mVarName = new Dictionary<string,int>();
        ErrCode m_eErrCode = ErrCode.EC_SUCCEED;
        ExcelColoumValue[] coloumValues = null;
		int m_iValidColoumCounts = 0;
		int m_iConvertedRows = 0;
		object[] mDatas = null;
		string name = string.Empty;
        string path = string.Empty;
		bool m_bInitialized = false;

		public int ConvertedRows 
		{
			get 
			{
				return m_iConvertedRows;
			}
		}

		public string FileName 
		{
			get { return name; }
		}

		public string SheetName
		{
			get 
			{
				if (null == m_sheet) 
				{
					return string.Empty;
				}
				return m_sheet.SheetName;
			}
		}

        public ExcelUnit(string path,FileAccess accessMode = FileAccess.Read)
        {
            if(File.Exists(path))
            {
                m_fileStream = new FileStream(path, FileMode.Open, accessMode, FileShare.ReadWrite);
            }

            if(null == m_fileStream)
            {
                _PrintErrMsg(ErrCode.EC_NO_FILE);
				return;
            }

			name = Path.GetFileNameWithoutExtension (path);
            this.path = path;
        }

        public bool succeed
        {
            get
            {
                return m_eErrCode == ErrCode.EC_SUCCEED;
            }
        }

		public string ApplicationDataPath
		{
			get;private set;
		}

		public ExcelHelper.ConvertMode ConvertMode
		{
			get;private set;
		}

		public bool isModel
		{
			get;private set;
		}

        public bool Init(string dataPath, ExcelHelper.ConvertMode convertMode, bool isModel = false)
        {
			ConvertMode = convertMode;
			this.isModel = isModel;
			ApplicationDataPath = dataPath;
			if (m_bInitialized) 
			{
				return succeed;
			}
			m_bInitialized = true;

            if (null != m_fileStream)
            {
				if (path.EndsWith(".xls"))
					m_workbook = new HSSFWorkbook(m_fileStream);
				else
				{
					XSSFWorkbook wb2007 = null;
					wb2007 = new XSSFWorkbook(path);
					m_workbook = wb2007;
                    //m_fileStream.Close();
                    //m_fileStream = null;
                }
				m_sheet = m_workbook.GetSheetAt(0);
                m_bHasUnion = false;

                if (null == m_sheet)
                {
                    _PrintErrMsg(ErrCode.EC_NO_SHEET);
                    return false;
                }

                if (string.IsNullOrEmpty(m_sheet.SheetName))
                {
                    _PrintErrMsg(ErrCode.EC_SHEET_NAME_INVALID);
                    return false;
                }

                if(!succeed)
                {
                    Debug.LogErrorFormat("[npoi] loadsheet=<color=#ff0000>[{0}]</color> falied !", m_sheet.SheetName);
                    return false;
                }
				//Debug.LogFormat ("sheet = {0}", m_sheet.SheetName);
            }
            return succeed;
        }

		protected bool _checkIdColoums(ISheet sheet)
		{
			int rowNum = 0;
			var row = m_sheet.GetRow(rowNum);
			if(null == row || null == row.GetCell(0) || row.GetCell(0).CellType != CellType.String || row.GetCell(0).StringCellValue != "required")
			{
				_PrintErrMsg(ErrCode.EC_FIRST_COLOUM_ERROR,sheet.SheetName);
				Debug.LogErrorFormat("第{0}行第1列必须是 required",rowNum + 1);
				return false;
			}

            rowNum = 1;
            row = m_sheet.GetRow(rowNum);
            if (null == row || null == row.GetCell(0) || row.GetCell(0).CellType != CellType.String || row.GetCell(0).StringCellValue != "sint32")
            {
				_PrintErrMsg(ErrCode.EC_FIRST_COLOUM_ERROR, sheet.SheetName);
				Debug.LogErrorFormat("第{0}行第1列必须是 sint32", rowNum + 1);
                return false;
            }

            rowNum = 2;
            row = m_sheet.GetRow(rowNum);
            if (null == row || null == row.GetCell(0) || row.GetCell(0).CellType != CellType.String || row.GetCell(0).StringCellValue != "id")
            {
				_PrintErrMsg(ErrCode.EC_FIRST_COLOUM_ERROR, sheet.SheetName);
				Debug.LogErrorFormat("第{0}行第1列必须是 id", rowNum + 1);
                return false;
            }

            rowNum = 3;
            row = m_sheet.GetRow(rowNum);
            if (null == row || null == row.GetCell(0))
            {
                _PrintErrMsg(ErrCode.EC_FIRST_COLOUM_ERROR, sheet.SheetName);
				Debug.LogErrorFormat("第{0}行第1列必须是 1", rowNum + 1);
				return false;
            }
            var cell = row.GetCell(0);

            try
            {
				if(cell.CellType == CellType.String && cell.StringCellValue.Trim() != "1")
                {
                    _PrintErrMsg(ErrCode.EC_FIRST_COLOUM_ERROR, sheet.SheetName);
					Debug.LogErrorFormat("第{0}行第1列必须是 1", rowNum + 1);
                    return false;
                }
                if (cell.CellType == CellType.Numeric && cell.NumericCellValue.ToString().Trim() != "1")
                {
                    _PrintErrMsg(ErrCode.EC_FIRST_COLOUM_ERROR, sheet.SheetName);
					Debug.LogErrorFormat("第{0}行第1列必须是 1", rowNum + 1);
                    return false;
                }
				if(cell.CellType == CellType.Boolean && !cell.BooleanCellValue)
				{
                    _PrintErrMsg(ErrCode.EC_FIRST_COLOUM_ERROR, sheet.SheetName);
					Debug.LogErrorFormat("第{0}行第1列必须是 1", rowNum + 1);
                    return false;
                }
				if(cell.CellType == CellType.Formula)
				{
                    if (cell.CachedFormulaResultType == CellType.String && cell.StringCellValue.Trim() != "1")
                    {
                        _PrintErrMsg(ErrCode.EC_FIRST_COLOUM_ERROR, sheet.SheetName);
						Debug.LogErrorFormat("第{0}行第1列必须是 1", rowNum + 1);
                        return false;
                    }
                    if (cell.CachedFormulaResultType == CellType.Numeric && cell.NumericCellValue.ToString().Trim() != "1")
                    {
                        _PrintErrMsg(ErrCode.EC_FIRST_COLOUM_ERROR, sheet.SheetName);
						Debug.LogErrorFormat("第{0}行第1列必须是 1", rowNum + 1);
                        return false;
                    }
                    if (cell.CachedFormulaResultType == CellType.Boolean && !cell.BooleanCellValue)
                    {
                        _PrintErrMsg(ErrCode.EC_FIRST_COLOUM_ERROR, sheet.SheetName);
						Debug.LogErrorFormat("第{0}行第1列必须是 1", rowNum + 1);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                _PrintErrMsg(ErrCode.EC_FIRST_COLOUM_ERROR, sheet.SheetName); ;
                Debug.LogErrorFormat("第{0}行第1列必须是 1", rowNum + 1);
                Debug.LogError(e.Message);
                return false;
            }

            return true;
		}

		public bool LoadProtoBase()
		{
			mVarName.Clear();

			_checkIdColoums(m_sheet);
			if (!succeed)
            {
                Debug.LogErrorFormat("[npoi] check first coloum falied !");
                return false;
            }

            _loadProtoHeadLine(m_sheet);
			if (!succeed)
			{
				Debug.LogErrorFormat("[npoi] loadProtoHeadLine falied !");
				return false;
			}

			_loadProtoVarTypeLine(m_sheet);
			if (!succeed)
			{
				Debug.LogErrorFormat("[npoi] loadProtoVarTypeLine falied !");
				return false;
			}

			_loadProtoVarNameLine(m_sheet);
			if (!succeed)
			{
				Debug.LogErrorFormat("[npoi] loadProtoVarNameLine falied !");
				return false;
			}

			_loadServerClientFlag(m_sheet);
			if(!succeed)
			{
				Debug.LogErrorFormat("[npoi] _loadServerClientFlag falied !");
				return false;
			}

			_loadAnnontation(m_sheet);
			if(!succeed)
			{
				Debug.LogErrorFormat("[npoi] _loadAnnontation falied !");
				return false;
			}

			if(null != coloumValues)
			{
				intValueLength = 0;
				stringValueLength = 0;
				for (int i = 0; i < coloumValues.Length; ++i)
				{
					var coloum = coloumValues[i];
					if (null == coloum)
						continue;

					if(!coloum.isValidToSearver)
					{
						continue;
					}

					if(coloum.isList)
					{
						++intValueLength;
					}
					else
					{
                        if (coloum.eVarType == VarType.VT_STRING)
                        {
                            ++stringValueLength;
                        }
                        else
                        {
                            ++intValueLength;
                        }
                    }
				}
			}

			return true;
		}

		int intValueLength;
		int stringValueLength;

		int _GetMaxColoumValues(IRow row)
		{
			int iRet = 0;
			if (null != row) 
			{
				for (int j = 0; j < row.Cells.Count; ++j) 
				{
					if (row.Cells[j].CellType == CellType.String && !string.IsNullOrEmpty (row.Cells[j].StringCellValue)) 
					{
						iRet = Math.Max (iRet, row.Cells [j].ColumnIndex + 1);
					}
				}
			}
			return iRet;
		}

        void _loadProtoHeadLine(ISheet sheet)
        {
            if(null != sheet)
            {
                headLineRow = m_sheet.GetRow(PROTO_HEAD_ROW);
                if (null == headLineRow)
                {
                    _PrintErrMsg(ErrCode.EC_NO_PROTO_HEAD_ROW);
                    return;
                }
					
				coloumValues = new ExcelColoumValue[_GetMaxColoumValues(headLineRow)];
				m_iValidColoumCounts = 0;

				//Debug.LogErrorFormat ("sheetName={0} HeadLines = {1}", sheet.SheetName, coloumValues.Length);

                for (int i = 0; i < headLineRow.Cells.Count; ++i)
                {
					int iColoumIndex = headLineRow.Cells [i].ColumnIndex;

					if(_IsHeadLineValid(i))
                    {
                        var match = ms_proto_head_reg.Match(headLineRow.Cells[i].StringCellValue);
                        if (!match.Success)
                        {
                            Debug.LogErrorFormat("ROW={0},COL={1},VALUE={2}", PROTO_HEAD_ROW, i + 1,headLineRow.Cells[i].StringCellValue);
                            _PrintErrMsg(ErrCode.EC_PROTO_HEAD_MATCH_FAILED, ms_proto_head_reg);
                            break;
                        }

						coloumValues[iColoumIndex] = new ExcelColoumValue
                        {
							iIndex = iColoumIndex,
                            head = headLineRow.Cells[i].StringCellValue.Trim(),
							isList = headLineRow.Cells[i].StringCellValue.Trim() == "repeated",
						};

						m_iValidColoumCounts += 1;
                    }
                }
            }
        }

        void _loadProtoVarTypeLine(ISheet sheet)
        {
            if(null != sheet)
            {
                protoVarTypeRow = m_sheet.GetRow(PROTO_VAR_TYPE_ROW);
                if(null == protoVarTypeRow)
                {
                    _PrintErrMsg(ErrCode.EC_NO_PROTO_VAR_TYPE_ROW);
                    return;
                }

                for (int i = 0; i < protoVarTypeRow.Cells.Count; ++i)
                {
					int iColoumIndex = protoVarTypeRow.Cells [i].ColumnIndex;

					if(!_IsColoumValid(iColoumIndex))
                    {
                        continue;
                    }

					var coloumValue = coloumValues [iColoumIndex];

                    var match = ms_proto_var_type_reg.Match(protoVarTypeRow.Cells[i].StringCellValue);
                    if (!match.Success)
                    {
                        Debug.LogErrorFormat(protoVarTypeRow.Cells[i].StringCellValue);
                        _PrintErrMsg(ErrCode.EC_PROTO_VAR_TYPE_MATCH_FAILED,ms_proto_var_type_reg);
                        return;
                    }

					coloumValue.type = match.Groups [1].Value;
					coloumValue.loadVarType ();
					coloumValue.type = _generateVarType(match.Groups[1].Value);
					if (coloumValue.eVarType == VarType.VT_UNION)
					{
						coloumValue.bUnionFloat = protoVarTypeRow.Cells[i].StringCellValue.EndsWith ("(float)");
					} 
					else 
					{
						coloumValue.bUnionFloat = false;
					}

					if (coloumValue.eVarType == VarType.VT_COUNT) 
					{
						_PrintErrMsg (ErrCode.EC_VAR_TYPE_CONVERT_FAILED, coloumValue.type,sheet.SheetName);
						return;
					}

                    if (!m_bHasUnion)
                    {
						if (coloumValue.eVarType == VarType.VT_PAIR) 
						{
							m_bHasUnion = true;
						}
                    }
                }
            }
        }

        void _loadProtoVarNameLine(ISheet sheet)
        {
            if(null != sheet)
            {
                protoVarNameRow = m_sheet.GetRow(PROTO_VAR_NAME_ROW);
                if (null == protoVarNameRow)
                {
                    _PrintErrMsg(ErrCode.EC_NO_PROTO_VAR_NAME_ROW);
                    return;
                }

                for (int i = 0; i < protoVarNameRow.Cells.Count; ++i)
                {
					int iColoumIndex = protoVarNameRow.Cells [i].ColumnIndex;

					if (!_IsColoumValid(iColoumIndex))
                    {
                        continue;
                    }

					var coloumValue = coloumValues [iColoumIndex];

                    var match = ms_proto_var_name_reg.Match(protoVarNameRow.Cells[i].StringCellValue);
                    if (!match.Success)
                    {
                        Debug.LogErrorFormat(protoVarNameRow.Cells[i].StringCellValue);
                        _PrintErrMsg(ErrCode.EC_PROTO_VAR_NAME_MATCH_FAILED,ms_proto_var_name_reg);
                        break;
                    }

					coloumValue.name = _generateName (protoVarNameRow.Cells [i].StringCellValue);

					if(mVarName.ContainsKey(coloumValue.name))
					{
                        Debug.LogErrorFormat(protoVarNameRow.Cells[i].StringCellValue);
                        _PrintErrMsg(ErrCode.EC_VAR_NAME_REPEATED,coloumValue.name, iColoumIndex + 1, mVarName[coloumValue.name]);
						break;
                    }
					mVarName.Add(coloumValue.name,iColoumIndex + 1);

					if (coloumValue.eVarType == VarType.VT_ENUM) 
					{
						coloumValue.propertyName = _generateEnumVarType (coloumValue.name);
					} 
					else 
					{
						coloumValue.propertyName = _generateName (coloumValue.name);
					}
                }
            }
        }

        void _loadServerClientFlag(ISheet sheet)
        {
            if(null != sheet)
            {
                serverClientFlagRow = m_sheet.GetRow(SERVER_CLIENT_FLAG_ROW);
                if(null == serverClientFlagRow)
                {
                    return;
                }

                for(int i = 0; i < serverClientFlagRow.Cells.Count; ++i)
                {
					int iColoumIndex = serverClientFlagRow.Cells [i].ColumnIndex;

					if(!_IsColoumValid(iColoumIndex))
                    {
                        continue;
                    }

					var coloumValue = coloumValues[iColoumIndex];

                    coloumValue.isValidToSearver = _IsValidToServer(i);
                }
            }
        }

		private object _getCellValueObject(ICell cell, string defaultStr)
		{
			object obj = defaultStr;

			if (cell != null)
			{
				switch (cell.CellType)
				{
				case CellType.Numeric:
					obj = cell.NumericCellValue;
					break;
				case CellType.String:
					obj = cell.StringCellValue;
					break;
				case CellType.Blank:
					obj = defaultStr;
					break;
				case CellType.Formula:
					try
					{
						if(cell.CachedFormulaResultType == CellType.Numeric)
						{
							obj = (int)cell.NumericCellValue;
						}
						else if(cell.CachedFormulaResultType == CellType.String)
						{
							obj = cell.StringCellValue;
						}
					}
					catch 
					{
						Debug.LogErrorFormat("[GenerateText] 公式转换出错");
						Debug.LogErrorFormat("[GenerateText] cell type is not valid {0}, {1}, [{2}行, {3}列]", m_sheet.SheetName, cell.CellType.ToString(), cell.RowIndex + 1, cell.ColumnIndex);
					}
					break;
				default:
					Debug.LogErrorFormat("[GenerateText] 数据非法 {0}, {1}, [{2}行, {3}列]", m_sheet.SheetName, cell.CellType.ToString(), cell.RowIndex + 1, cell.ColumnIndex);

					break;
				}
			}
			return obj;
		}

		bool _IsValidRow(IRow row)
		{
			if (null == row || row.Cells.Count <= 0) {
				return false;
			}

			if (row.Cells [0].ColumnIndex != 0) {
				return false;
			}

			var rowObject = _getCellValueObject (row.Cells [0], string.Empty);
			if (string.IsNullOrEmpty (rowObject.ToString ())) 
			{
				return false;
			}

			return true;
		}

		StringBuilder sb = new StringBuilder(256);
		static int[] varIntValues = new int[655360];
		static int varIntPos = 0;
		static string[] varStringValues = new string[65536];
		static int varStringPos = 0;
		static int varLongPos = 0;
		static long[] varLongValues = new long[65536];

        static int[] fixedIntValues = new int[655360];
        static int fixedIntPos = 0;
        static string[] fixedStringValues = new string[655360];
        //static int fixedStringPos = 0;
        //static int fixedLongPos = 0;
        static long[] fixedLongValues = new long[65536];
		ILFastMode tableArrayUnit = null;
		List<TableHandle> mHandles = new List<TableHandle>(4096);
		object _loadContentRow(ISheet sheet,IRow row,int validConvertedCnt)
		{
			if (!_IsValidRow (row)) 
			{
				return null;
			}

			var classname = sheet.SheetName.ToUpper();
			Assembly assembly = this.isModel ? typeof(CSLine).Assembly : typeof(HotMain).Assembly;
			object tableunit = assembly.CreateInstance (string.Format("TABLE.{0}",classname));
			if (null == tableunit) 
			{
				_PrintErrMsg (ErrCode.EC_ASSEMBLY_CLASS_CREATE_INSTANCE_FAILED, classname);
				return null;
			}

			TableHandle handle = null;
			if (this.ConvertMode == ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
			{
                handle = new TableHandle();
                handle.key = 0;
                handle.intOffset = validConvertedCnt * tableArrayUnit.IntValueFixedLength;
                handle.stringOffset = validConvertedCnt * tableArrayUnit.StringValueFixedLength;
                handle.longOffset = validConvertedCnt * tableArrayUnit.LongValueFixedLength;
                handle.data = tableArrayUnit.gItem;
                handle.Value = tableunit;
                tableunit.SetFieldValue("handle", handle);
                tableunit.SetFieldValue("__data", tableArrayUnit.gItem);
                tableunit.SetFieldValue("Array", tableArrayUnit);
            }

			int fixedStartPos = fixedIntPos;

			Type type = tableunit.GetType();
			object obj = new object();

			for (int j = 0,max = row.Cells.Count; j < max; ++j)
			{
				ICell cell = row.Cells[j];
				int iColoumIndex = cell.ColumnIndex;

				if(!_IsColoumValid(iColoumIndex))
				{
					continue;	
				}

				var coloumValue = coloumValues [iColoumIndex];
				if(!coloumValue.isValidToSearver)
				{
					continue;
				}

				obj = _getCellValueObject(cell, string.Empty);
				if(null == obj)
				{
					Debug.LogErrorFormat("obj is null");
					continue;
				}

				if (coloumValue.head.Equals ("repeated"))
				{
					var fieldName = string.Empty;
					FieldInfo fieldInfo = null;
					PropertyInfo propertyInfo = null;
                    if (this.ConvertMode == ExcelHelper.ConvertMode.CM_PROTO3)
                    {
                        if (!coloumValue.name.Contains("_"))
                        {
                            fieldName = coloumValue.name[0].ToString().ToLower() + coloumValue.name.Substring(1) + "_";
                        }
                        else
                        {
                            sb.Clear();
                            sb.Append(coloumValue.name[0]);
                            bool Up = false;
                            for (int i = 1; i < coloumValue.name.Length; ++i)
                            {
                                if (coloumValue.name[i] == '_')
                                {
                                    Up = true;
                                    continue;
                                }

                                if (Up)
                                {
                                    sb.Append(coloumValue.name[i].ToString().ToUpper());
                                    Up = false;
                                }
                                else
                                {
                                    sb.Append(coloumValue.name[i]);
                                }
                            }
                            sb.Append('_');
                            fieldName = sb.ToString();
                        }
                        fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                    }
					else if(this.ConvertMode == ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
					{
                        if (!coloumValue.name.Contains("_"))
                        {
							//fieldName = coloumValue.name[0].ToString().ToLower() + coloumValue.name.Substring(1) + "Ptr";
							fieldName = coloumValue.name + "Ptr";
						}
                        else
                        {
                            sb.Clear();
                            sb.Append(coloumValue.name[0]);
                            bool Up = false;
                            for (int i = 1; i < coloumValue.name.Length; ++i)
                            {
                                if (coloumValue.name[i] == '_')
                                {
                                    Up = true;
                                    continue;
                                }

                                if (Up)
                                {
                                    sb.Append(coloumValue.name[i].ToString().ToUpper());
                                    Up = false;
                                }
                                else
                                {
                                    sb.Append(coloumValue.name[i]);
                                }
                            }
                            sb.Append("Ptr");
                            fieldName = sb.ToString();
                        }
						propertyInfo = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
					}
                    else
                    {
						fieldName = $"_{coloumValue.name}";
						fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                    }

					if(this.ConvertMode !=  ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
					{
                        if (null == fieldInfo)
                        {
                            Debug.LogErrorFormat("GetField failed filedName = {0}", fieldName);
                            return null;
                        }
                    }
					else
					{
                        if (null == propertyInfo)
                        {
                            Debug.LogErrorFormat("GetProperty failed filedName = {0}", fieldName);
                            return null;
                        }
                    }

					if (this.ConvertMode == ExcelHelper.ConvertMode.CM_PROTO3)
                    {
                        var filedType = fieldInfo.FieldType;
                        string[] items = null;
                        if (coloumValue.eVarType != VarType.VT_PAIR)
                            items = obj.ToString().Split('#');
                        else
                            items = obj.ToString().Split('&');

                        var fieldHandle = fieldInfo.GetValue(tableunit);
                        for (int i = 0; i < items.Length; ++i)
                        {
                            try
                            {
                                var value = coloumValue.toValue(items[i]);
								if(null != value)
								{
                                    var methodInfo = filedType.GetMethod("Add", new Type[] { value.GetType() });
                                    methodInfo.Invoke(fieldHandle, new object[] { value });
                                }
                            }
                            catch (Exception)
                            {
                                Debug.LogErrorFormat("[GenerateText] 转表失败，请保证协议(*.cs)和表格数据(*.xls)相对应");
                                Debug.LogErrorFormat("[GenerateText] {0} : {1}, {2}, {3}", coloumValue.head, items[i], coloumValue.type, coloumValue.eVarType);
                                Debug.LogErrorFormat("[GenerateText] {0} {1}:{2} key: {3}({4}) (type:{5}) ERROR DATA", sheet.SheetName, row.RowNum + 1, cell.ColumnIndex + 1, coloumValue.type, coloumValue.name, coloumValue.type);
                                _PrintErrMsg(ErrCode.EC_PROTO_NOT_MATCH_CS, sheet.SheetName);
                                return null;
                            }
                        }
                    }
					else if(this.ConvertMode == ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
					{
                        var propertyType = propertyInfo.PropertyType;
                        string[] items = null;
                        if (coloumValue.eVarType != VarType.VT_PAIR)
                            items = obj.ToString().Split('#');
                        else
                            items = obj.ToString().Split('&');
                        int startPos = varIntPos;
						if(coloumValue.eVarType == VarType.VT_STRING)
						{
							startPos = varStringPos;
						}
						else if(coloumValue.eVarType == VarType.VT_PAIR)
						{
							startPos = varLongPos;
						}
						else
						{
							startPos = varIntPos;
						}

                        for (int i = 0; i < items.Length; ++i)
                        {
                            try
                            {
                                var value = coloumValue.toValue(items[i]);
								if(null == value)
								{

									continue;
								}
								if(value.GetType() == typeof(String))
								{
									varStringValues[varStringPos++] = value as string;
								}
								else if(value.GetType() == typeof(TABLE.KEYVALUE))
								{
									TABLE.KEYVALUE kv = value as TABLE.KEYVALUE;
									long val = ((kv.value & 0xFFFFFFFF) << 32) | ((long)kv.key);
									varLongValues[varLongPos++] = val;
									int key = val.key();
									int vv = val.value();

									//Debug.LogFormat("key = {0} vv={1}", key, vv);
								}
								else
								{
									varIntValues[varIntPos++] = (int)value;
								}
                            }
                            catch (Exception)
                            {
                                Debug.LogErrorFormat("[GenerateText] 转表失败，请保证协议(*.cs)和表格数据(*.xls)相对应");
                                Debug.LogErrorFormat("[GenerateText] {0} : {1}, {2}, {3}", coloumValue.head, items[i], coloumValue.type, coloumValue.eVarType);
                                Debug.LogErrorFormat("[GenerateText] {0} {1}:{2} key: {3}({4}) (type:{5}) ERROR DATA", sheet.SheetName, row.RowNum + 1, cell.ColumnIndex + 1, coloumValue.type, coloumValue.name, coloumValue.type);
                                _PrintErrMsg(ErrCode.EC_PROTO_NOT_MATCH_CS, sheet.SheetName);
                                return null;
                            }
                        }
						int endPos = varIntPos;
                        if (coloumValue.eVarType == VarType.VT_STRING)
                        {
							endPos = varStringPos;
                        }
                        else if (coloumValue.eVarType == VarType.VT_PAIR)
                        {
							endPos = varLongPos;
                        }
                        else
                        {
							endPos = varIntPos;
                        }
						/*int tag = 1;
                        if (coloumValue.eVarType == VarType.VT_STRING)
                        {
							tag = 2;
                        }
                        else if (coloumValue.eVarType == VarType.VT_PAIR)
                        {
							tag = 3;
                        }
                        else
                        {
							tag = 1;
                        }*/

                        int length = endPos - startPos;
                        int v = (length << 20) | (startPos & 0xFFFFF);
						if (startPos != (startPos & 0xFFFFF) || length != (length & 0xFFF))
						{
							Debug.LogFormat("<color=#ff0000>[id:{4}]:[s:{0} => s:{1}]:[e:{2} => e:{3}]</color>", startPos, startPos & 0xFFFFF, length, length & 0xFFF, fixedIntValues[fixedStartPos]);
						}
						//else
						//{
						//	Debug.LogFormat("<color=#00ff0c>[id:{2}]:[s:{0}]:[e:{1}]</color>", startPos, endPos,fastModeItem.id);
						//}
						//fixedIntValues[fixedIntPos++] = v;
						propertyInfo.SetValue(tableunit,v);
					}
					else
					{
                        var filedType = fieldInfo.FieldType;
                        string[] items = null;
                        if (coloumValue.eVarType != VarType.VT_PAIR)
                            items = obj.ToString().Split('#');
                        else
                            items = obj.ToString().Split('&');

                        var listObj = fieldInfo.GetValue(tableunit);
                        if (null == listObj)
                        {
                            Debug.LogErrorFormat("GetList Handle failed filedName = {0}", fieldName);
                            return null;
                        }

                        for (int i = 0; i < items.Length; ++i)
                        {
                            try
                            {
                                var value = coloumValue.toValue(items[i]);
                                listObj.Invoke("Add", value);
                            }
                            catch (Exception)
                            {
                                Debug.LogErrorFormat("[GenerateText] 转表失败，请保证协议(*.cs)和表格数据(*.xls)相对应");
                                Debug.LogErrorFormat("[GenerateText] {0} : {1}, {2}, {3}", coloumValue.head, items[i], coloumValue.type, coloumValue.eVarType);
                                Debug.LogErrorFormat("[GenerateText] {0} {1}:{2} key: {3}({4}) (type:{5}) ERROR DATA", sheet.SheetName, row.RowNum + 1, cell.ColumnIndex + 1, coloumValue.type, coloumValue.name, coloumValue.type);
                                _PrintErrMsg(ErrCode.EC_PROTO_NOT_MATCH_CS, sheet.SheetName);
                                return null;
                            }
                        }
                    }
				}
				else if (coloumValue.head.Equals ("required")) 
				{
					try
					{
						if(this.ConvertMode == ExcelHelper.ConvertMode.CM_PROTO3)
						{
                            FieldInfo fieldInfo = null;
                            var fieldName = coloumValue.name;
                            if (!fieldName.Contains("_"))
                            {
                                fieldName = fieldName[0].ToString().ToLower() + fieldName.Substring(1) + "_";
                            }
                            else
                            {
                                sb.Clear();
                                sb.Append(coloumValue.name[0]);
                                bool Up = false;
                                for (int i = 1; i < coloumValue.name.Length; ++i)
                                {
                                    if (coloumValue.name[i] == '_')
                                    {
                                        Up = true;
                                        continue;
                                    }
                                    if (Up)
                                    {
                                        Up = false;
                                        sb.Append(coloumValue.name[i].ToString().ToUpper());
                                    }
                                    else
                                    {
                                        sb.Append(coloumValue.name[i]);
                                    }
                                }
                                sb.Append('_');
                                fieldName = sb.ToString();
                            }
                            fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                            obj = coloumValue.toValue(obj.ToString());
                            fieldInfo.SetValue(tableunit, obj);
                        }
                        else if (this.ConvertMode == ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
                        {
                            PropertyInfo propertyInfo = type.GetProperty(coloumValue.name, BindingFlags.Public | BindingFlags.Instance);
                            obj = coloumValue.toValue(obj.ToString());
							propertyInfo.SetValue(tableunit,obj);
							//if (coloumValue.eVarType == VarType.VT_STRING)
							//{
							//	fixedStringValues[fixedStringPos++] = obj as string;
							//}
							//else if(coloumValue.eVarType == VarType.VT_INT64)
							//{
							//	fixedLongValues[fixedLongPos++] = (long)obj;
							//}
							//else
							//{
							//	fixedIntValues[fixedIntPos++] = (int)obj;
							//}
						}
                        else
						{
                            PropertyInfo propertyInfo = type.GetProperty(coloumValue.name, BindingFlags.Public | BindingFlags.Instance);
                            obj = coloumValue.toValue(obj.ToString());
                            propertyInfo.SetValue(tableunit, obj);
                        }
						//Debug.LogFormat("<color=#00ff00>NAME={0} TYPE={1} VALUE={2}</color>",coloumValue.propertyName,coloumValue.type,obj.ToString());
					}
					catch (Exception e)
					{
						Debug.LogErrorFormat(e.ToString());
						Debug.LogErrorFormat("{0}", coloumValue.propertyName);
						_PrintErrMsg(ErrCode.EC_PROTO_NOT_MATCH_CS, sheet.SheetName);
						return null;
					}
				}
				else 
				{
					Debug.LogErrorFormat ("unknown proto head = {0} {1}!!!", coloumValue.head,coloumValue.type);
					return null;
				}
			}

			if(null != tableunit && ConvertMode == ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
			{
                int id = (int)tableunit.GetPropertyValue("id");
                handle.key = id;
                mHandles.Add(handle);
            }

			return tableunit;
		}

		class loadContenRowParam
		{
			public delegate void OnTaskOver(int iCount,string txtPath, loadContenRowParam param);
			public ExcelUnit excelUnit;
			public ISheet sheet;
			public int iStartRowIndex;
			public int iEndRowIndex;
			public object[] mDatas;
            public string textPath;
			public object loadLock;
			public OnTaskOver callback;

			public static void _ConvertRow(object param)
			{
				var curParam = param as loadContenRowParam;
				if (null != curParam) 
				{
					try
					{
						var beginTime = System.DateTime.Now.Ticks;

						int iValidCount = 0;

						for (int i = curParam.iStartRowIndex; i < curParam.iEndRowIndex; ++i) 
						{
							int fixedStartPos = fixedIntPos;
							var tableUnit = curParam.excelUnit._loadContentRow (curParam.sheet, curParam.sheet.GetRow(i),iValidCount);
							curParam.mDatas[i] = tableUnit;
							if(null != tableUnit)
							{
								++iValidCount;
							}
						}

						var deltaTime = System.DateTime.Now.Ticks - beginTime;
						beginTime = System.DateTime.Now.Ticks;
						Debug.LogFormat("[convert row contents] <color=#00ff00>[{0}]</color>[{1}-{2}][{3}/{4}]<color=#00ff00>[time = {5}ms]</color>",curParam.sheet.SheetName,
							curParam.iStartRowIndex,curParam.iEndRowIndex - 1,
							iValidCount,curParam.iEndRowIndex - curParam.iStartRowIndex,
							deltaTime/10000);

						deltaTime = System.DateTime.Now.Ticks - beginTime;
						Debug.LogFormat("[push row contents] <color=#00ff00>[{0}] [{1}-{2}]</color>",curParam.sheet.SheetName,curParam.iStartRowIndex,curParam.iEndRowIndex);

						lock(curParam.loadLock)
						{
							if(null != curParam.callback)
							{
								curParam.callback.Invoke(curParam.iEndRowIndex - curParam.iStartRowIndex,curParam.textPath,curParam);
							}
						}
					}
					catch(Exception e) 
					{
						Debug.LogErrorFormat (e.Message);
						Debug.LogFormat("<color=#ff0000>[{0}] [{1}|{2}]</color>",curParam.sheet.SheetName,curParam.iStartRowIndex,curParam.iEndRowIndex);
						lock(curParam.loadLock)
						{
							if(null != curParam.callback)
							{
								curParam.callback.Invoke(curParam.iEndRowIndex - curParam.iStartRowIndex,curParam.textPath, curParam);
							}
						}
					}
				}
			}
		}

		private void WriteFile(object data,string path)
		{
			try
			{
				FileStream file = null;
				BinaryFormatter bf = new BinaryFormatter();
				file = File.Open(path, FileMode.Create);
				bf.Serialize(file, data);
				file.Close();
				Debug.Log("成功存储");
			}
			catch (System.Exception ex)
			{
				Debug.Log("存储失败----" + ex.Message);
			}
			//string.IsNullOrEmpty(path):基本等价与path==null||path.length==0::没用到，但想到。

		}

		void _OnTaskOver(int iCount,string txtPath,loadContenRowParam param)
		{
            m_iTotalTaskCount -= iCount;
            if (0 == m_iTotalTaskCount)
            {
                var beginTime = System.DateTime.Now.Ticks;
				var tableArrayName = (param.sheet.SheetName).ToUpper() + "ARRAY";
				var className = $"TABLE.{tableArrayName}";
				string tableAssetStorePath = GetTableAssetVerifyPath(param.sheet.SheetName);
				Assembly assembly = this.isModel ? typeof(CSLine).Assembly : typeof(HotMain).Assembly;
				var type = assembly.GetType(className);

				var tableAsset = assembly.CreateInstance(className);

				var assetType = tableAsset.GetType();

				var fieldName = string.Empty;
				
				if(this.ConvertMode == ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
				{
					fieldName = "Rows";
				}
				else if(this.ConvertMode == ExcelHelper.ConvertMode.CM_PROTO3)
				{
					fieldName = "rows_";
				}
				else
				{
					fieldName = "_rows";
				}

				ILFastModeItem[] fastModeRows = new ILFastModeItem[0];

				FieldInfo fieldInfo = null;
				PropertyInfo propertyInfo = null;
				if(this.ConvertMode != ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
				{
					fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
				}
				else
				{
					propertyInfo = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
				}

				var filed = tableAsset.GetFieldValue(fieldName);

				int firstContentIndex = 9;

				if (this.ConvertMode == ExcelHelper.ConvertMode.CM_PROTO3)
				{
					var list = filed as IList;
                    for (int i = firstContentIndex; i < mDatas.Length; ++i)
                    {
                        if (null != mDatas[i])
                        {
							list.Add(mDatas[i]);
                        }
                    }
                }
				else if (this.ConvertMode == ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
				{
					if(tableAsset is ILFastMode ilFastObject)
					{
						//List<ILFastModeItem> validItems = new List<ILFastModeItem>(mDatas.Length - firstContentIndex);
      //                  for (int i = firstContentIndex; i < mDatas.Length; ++i)
      //                  {
      //                      if (mDatas[i] is ILFastModeItem modeItem)
      //                      {
						//		validItems.Add(modeItem);
      //                      }
      //                  }
						//fastModeRows = new ILFastModeItem[validItems.Count];
						//for(int i = 0; i < fastModeRows.Length;++i)
						//{
						//	fastModeRows[i] = validItems[i];
						//}
						//计算可变int长度
					}
                }
				else
				{
                    var method = fieldInfo.FieldType.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
                    for (int i = firstContentIndex; i < mDatas.Length; ++i)
                    {
                        if (null != mDatas[i])
                        {
                            method.Invoke(filed, new object[] { mDatas[i] });
                        }
                    }
                }

				if (System.IO.File.Exists(tableAssetStorePath))
				{
					System.IO.File.Delete(tableAssetStorePath);
				}

				if(this.ConvertMode == ExcelHelper.ConvertMode.CM_PROTO3)
                {
                    var tableMessage = tableAsset as Google.Protobuf.IMessage;
                    byte[] bytes = new byte[tableMessage.CalculateSize()];
                    using (Google.Protobuf.CodedOutputStream output = new Google.Protobuf.CodedOutputStream(bytes))
                    {
                        tableMessage.WriteTo(output);
                    }
                    System.IO.File.WriteAllBytes(tableAssetStorePath,bytes);
				}
				else if (this.ConvertMode == ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
				{
                    if (tableAsset is ILFastMode fastMode)
                    {
						var rows = fastModeRows;
						var bytesFileName = param.sheet.SheetName.ToLower();

						string fileName = GetTableAssetVerifyPath(bytesFileName);
                        using (FileStream fileStream = File.Create(fileName))
                        {
                            //压入变长IntArray
                            fileStream.Encode(varIntPos);
                            for (int i = 0; i < varIntPos; ++i)
                            {
                                fileStream.Encode(varIntValues[i]);
                            }
                            //压入变长StringArray
                            fileStream.Encode(varStringPos);
                            for (int i = 0; i < varStringPos; ++i)
                            {
                                fileStream.Encode(varStringValues[i]);
                            }
							//压入变长LongArray
							fileStream.Encode(varLongPos);
                            for (int i = 0; i < varLongPos; ++i)
                            {
                                fileStream.Encode(varLongValues[i]);
                            }
                            //压入元素个数
                            fileStream.Encode(mHandles.Count);
							//压入定长IntArray个数
							fileStream.Encode(fastMode.IntValueFixedLength);
							//压入定长SringArray个数
							fileStream.Encode(fastMode.StringValueFixedLength);
                            //压入定长LongArray个数
                            fileStream.Encode(fastMode.LongValueFixedLength);

							//获取帮助器
							var helperName = $"TABLE.{(param.sheet.SheetName).ToUpper()}Helper";
							var gtype = assembly.GetType(helperName);
							var gmethod = gtype.GetMethod("Encode", BindingFlags.Static | BindingFlags.Public);
							var classname = $"TABLE.{(param.sheet.SheetName).ToUpper()}";

							//压入元素数据
							for (int i = 0; i < mHandles.Count; ++i)
                            {
                                ExcelLogicHelper.ModifyRowContent(mHandles[i].Value);
								gmethod.Invoke(null, new object[] { fileStream, mHandles[i].Value });
							}
                        }
                    }
                }
				else
				{
                    //using (FileStream fileStream = File.Create(tableAssetStorePath))
                    //{
                    //    ProtoBuf.Serializer.Serialize(fileStream, tableAsset);
                    //}
                }
				//var tableAsset = ScriptableObject.CreateInstance("Smart.Model.BinaryAsset");
				//AssetDatabase.CreateAsset(tableAsset, tableAssetStorePath);
				////(tableAsset as BinaryAsset).datas = JsonHelper.ToJson(dataClass);
				//EditorUtility.SetDirty(tableAsset);
				//AssetDatabase.Refresh();
				//AssetDatabase.SaveAssets();
				var deltaTime = System.DateTime.Now.Ticks - beginTime;
				var name = Path.GetFileNameWithoutExtension(txtPath);
				//bool verifyOK = HasTheSameContent(param.sheet.SheetName);
				//var result = verifyOK ? "<color=#00ff00>校验成功!</color>" : "<color=#ff0000>校验失败!</color>";
				Debug.LogFormat("<color=#00ff00>[write file][{0}][time={1}ms]</color>", name, deltaTime / 10000);
			}
        }
			
		int m_iTotalTaskCount = 0;
		void _loadContent(ISheet sheet,string path)
		{
			m_iConvertedRows = 0;
			m_iTotalTaskCount = 0;
			varIntPos = 0;
			varStringPos = 0;
			varLongPos = 0;
			//fixedLongPos = 0;
			fixedIntPos = 0;
			//fixedStringPos = 0;
            System.Array.Clear(fixedIntValues, 0, fixedIntValues.Length);
            System.Array.Clear(fixedLongValues, 0, fixedLongValues.Length);
            System.Array.Clear(fixedStringValues, 0, fixedStringValues.Length);
            System.Array.Clear(varIntValues, 0, varIntValues.Length);
            System.Array.Clear(varStringValues, 0, varStringValues.Length);
            System.Array.Clear(varLongValues, 0, varLongValues.Length);
            mDatas = null;
			if (succeed) 
			{
				if (null == m_fileStream && null == m_workbook) 
				{
					_PrintErrMsg (ErrCode.EC_NO_INITIALIZE);
					return;
				}

				if (null == sheet) 
				{
					_PrintErrMsg (ErrCode.EC_NO_SHEET);
					return;
				}

				int iTotal = (sheet.LastRowNum - CONTENT_START_ROW + 1);
				int iThreadCount = 0;// iTotal / 300;
				int iMaxThread = 1;
				iThreadCount = Math.Min (iThreadCount, iMaxThread);
				object loadLock = new object ();
				m_iTotalTaskCount = sheet.LastRowNum - CONTENT_START_ROW + 1;
				var beginTime = System.DateTime.Now.Ticks;
				mDatas = new object[sheet.LastRowNum + 1];
				var deltaTime = System.DateTime.Now.Ticks - beginTime;
				Debug.LogFormat ("[allocate memory] <color=#00ff00>[{0}]</color>[counts = {1}] <color=#ffff00>[time = {2}ms]</color>", sheet.SheetName, m_iTotalTaskCount, deltaTime/10000);

                if (this.ConvertMode == ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
                {
                    tableArrayUnit = (typeof(HotMain)).Assembly.CreateInstance(string.Format("TABLE.{0}ARRAY", sheet.SheetName.ToUpper())) as ILFastMode;
                    if (null != tableArrayUnit)
					{
                        tableArrayUnit.gItem = new TableData();
                        tableArrayUnit.gItem.intValues = fixedIntValues;
                        tableArrayUnit.gItem.longValues = fixedLongValues;
                        tableArrayUnit.gItem.stringValues = fixedStringValues;
                        tableArrayUnit.VarIntValues = varIntValues;
                        tableArrayUnit.VarLongValues = varLongValues;
                        tableArrayUnit.VarStringValues = varStringValues;
					}
                }
				mHandles.Clear();

				if (iThreadCount <= 0) 
				{
					loadContenRowParam._ConvertRow (new loadContenRowParam {
						excelUnit = this,
						sheet = sheet,
						iStartRowIndex = CONTENT_START_ROW,
						iEndRowIndex = sheet.LastRowNum + 1,
						mDatas = mDatas,
						textPath = path,
						loadLock = loadLock,
						callback = _OnTaskOver,
					});
				} 
				else 
				{
					int iAve = iTotal / (iThreadCount + 1);

					Thread[] threads = new Thread[iThreadCount];
					for (int i = 0; i < iThreadCount; ++i) 
					{
						threads[i] = new Thread(loadContenRowParam._ConvertRow);
						threads[i].Priority = System.Threading.ThreadPriority.Highest;
						threads[i].Start (new loadContenRowParam {
							excelUnit = this,
							sheet = sheet,
							iStartRowIndex = CONTENT_START_ROW + iAve * i,
							iEndRowIndex = CONTENT_START_ROW + iAve * (i + 1),
							mDatas = mDatas,
							textPath = path,
							loadLock = loadLock,
							callback = _OnTaskOver,
						});
					}
						
					loadContenRowParam._ConvertRow (new loadContenRowParam {
						excelUnit = this,
						sheet = sheet,
						iStartRowIndex = CONTENT_START_ROW + iAve * iThreadCount,
						iEndRowIndex = sheet.LastRowNum + 1,
						mDatas = mDatas,
						textPath = path,
						loadLock = loadLock,
						callback = _OnTaskOver,
					});

					for (int i = 0; i < threads.Length; ++i) 
					{
						threads [i].Join ();
					}
				}
			}
		}

		bool TryParseBitCombine(ExcelColoumValue coloumValue,ICell cell,string capturedValue)
		{
			capturedValue = capturedValue.Trim();
			if (null == coloumValue || null == cell || string.IsNullOrEmpty(capturedValue) || !capturedValue.Contains("_"))
            {
                _PrintErrMsg(ErrCode.EC_PARSE_BIT_COMBINE_ERROR);
                return false;
            }

            coloumValue.bits = new List<BitVarInfo>();
            var tokens = capturedValue.Split('_');
            for (int k = 0; k < tokens.Length && succeed; ++k)
            {
				tokens[k] = tokens[k].Trim();
				var mergeMatch = ms_proto_merge_var_reg.Match(tokens[k]);
                int bitCount = 0;
				string varName = mergeMatch.Groups[1].Value;

				if (!mergeMatch.Success)
				{
                    coloumValue.bits = null;
                    _PrintErrMsg(ErrCode.EC_PARSE_BIT_COMBINE_ERROR);
                    return false;
                }

				var length = mergeMatch.Groups[2].Value.Length - 1;
				for (int i = length; i >= 0; --i)
				{
					var key = mergeMatch.Groups[1].Value + mergeMatch.Groups[2].Value.Substring(0, i);
					if(!mVarName.ContainsKey(key))
					{
						continue;
					}

					if(mergeMatch.Groups[2].Value[i] == '0')
					{
						continue;
					}

					var num = mergeMatch.Groups[2].Value.Substring(i, mergeMatch.Groups[2].Value.Length - i);
					int count = 0;
					if(!int.TryParse(num,out count) || count <= 0)
					{
						continue;
					}

					varName = key;
					bitCount = count;
					break;
				}

                coloumValue.bits.Add(
                    new BitVarInfo
                    {
                        key = varName,
						value = bitCount,
                        definitions = string.Empty,
                    });
            }

            var datas = coloumValue.bits;
			var sb = new StringBuilder();
			sb.Clear();
			sb.Append($"\t\tpublic int make_{coloumValue.propertyName}");
			sb.Append("(");
			for (int j = datas.Count - 1; j >= 0; --j)
			{
				BitVarInfo info = datas[j];
				var propertyColoume = coloumValues[mVarName[info.key] - 1];
				sb.Append($"{CONST_VAR_TYPE_TO_STRING[(int)propertyColoume.eVarType]} {info.key}");
				if(j != 0)
				{
					sb.Append(",");
				}
			}
			sb.AppendLine(")");
			sb.AppendLine("\t\t{");
			var idKey = "____id";
			sb.AppendLine($"\t\t\tint {idKey} = 0;");
			int add = 0;
			for (int j = datas.Count - 1; j >= 0; --j)
			{
				BitVarInfo info = datas[j];
				sb.AppendFormat($"\t\t\t{idKey} |= (int)({info.key} << {add});\n");
				add += info.value;
			}
			sb.AppendLine($"\t\t\treturn {idKey};");
			sb.AppendLine("\t\t}");
			sb.AppendLine();

			coloumValue.make_id_function = sb.ToString();

            add = 0;
            for (int j = datas.Count - 1; j >= 0; --j)
            {
                BitVarInfo info = datas[j];
                var propertyColoume = coloumValues[mVarName[info.key] - 1];
				var varType = getVarType(propertyColoume);
				var attrName = info.key[0].ToString().ToUpper() + (info.key.Length > 1 ? info.key.Substring(1) : string.Empty);
				info.definitions = $"\t\tpublic {varType} {attrName}\n";
                info.definitions += $"\t\t{{\n";
                info.definitions += $"\t\t\tget{{ return ({varType})(({coloumValue.propertyName} << {32 - (add + info.value)}) >> {(32 - (add + info.value)) + add}); }}\n";
                info.definitions += $"\t\t}}\n";
                if (j != datas.Count - 1)
                    info.definitions += "\n";
                add += info.value;
            }

            return true;
        }

		public string getVarType(ExcelColoumValue coloum)
		{
			if(coloum.eVarType != VarType.VT_ENUM)
			{
				if (ConvertMode != ExcelHelper.ConvertMode.CM_IL_FAST_MODE)
					return CONST_VAR_TYPE_TO_STRING[(int)coloum.eVarType];
				return CONST_VAR_TYPE_TO_STRING_IL[(int)coloum.eVarType];
			}
			return coloum.propertyName;
		}

        void _loadAnnontation(ISheet sheet)
        {
            if(null != sheet)
            {
                annotationRow = sheet.GetRow(ANNOTATION_ROW);
                if (null == annotationRow)
                {
                    _PrintErrMsg(ErrCode.EC_NO_ANNOTATION_ROW);
                    return;
                }

                for(int i = 0; i < annotationRow.Cells.Count; ++i)
                {
					int iColoumIndex = annotationRow.Cells[i].ColumnIndex;

					if(!_IsColoumValid(iColoumIndex))
                    {
                        continue;
                    }

					var coloumValue = coloumValues [iColoumIndex];

                    if(coloumValue.type.Equals("enum"))
                    {
                        coloumValue.declare = _generateEnumDeclare(coloumValue, annotationRow.Cells[i].StringCellValue,2);
                        if(string.IsNullOrEmpty(coloumValue.declare))
                        {
                            _PrintErrMsg(ErrCode.EC_ENUM_DECLARE_CONVERT_IS_EMPTY);
                            return;
                        }
                    }
					else
					{
						var match = ms_special_annotation_content_reg.Match(annotationRow.Cells[i].StringCellValue);
						if(match.Success)
						{
							if (string.Equals("bit", match.Groups[1].Value))
                            {
								if(!TryParseBitCombine(coloumValue, annotationRow.Cells[i], match.Groups[2].Value))
								{
									return;
								}
                            }
                        }
					}
                }
            }
        }

        bool _IsColoumValid(int iIndex)
        {
			if (null != coloumValues && iIndex >= 0 && iIndex < coloumValues.Length)
            {
				return null != coloumValues[iIndex];
            }
            return false;
        }

		bool _IsHeadLineValid(int iIndex)
		{
			if (null != headLineRow && iIndex >= 0 && iIndex < headLineRow.Cells.Count)
			{
				return headLineRow.Cells[iIndex].CellType == CellType.String && !string.IsNullOrEmpty(headLineRow.Cells[iIndex].StringCellValue);
			}
			return false;
		}

        ExcelColoumValue _GetColoumValue(int iIndex)
        {
            if(null != coloumValues)
            {
                if(iIndex >= 0 && iIndex < coloumValues.Length)
                {
                    return coloumValues[iIndex];
                }
            }
            return null;
        }

        bool _IsValidToServer(int iIndex)
        {
            if(null != serverClientFlagRow && iIndex >= 0 && iIndex < serverClientFlagRow.Cells.Count)
            {
				var cellObject = _getCellValueObject(serverClientFlagRow.Cells[iIndex],string.Empty);
				int iRes = 0;
				if (int.TryParse (cellObject.ToString(), out iRes)) 
				{
					return iRes == 1;
				}
            }
            return false;
        }

        bool _IsEnumValue(int iIndex)
        {
            if(null != protoVarTypeRow && iIndex >= 0 && iIndex < protoVarTypeRow.Cells.Count)
            {
                var match = ms_proto_var_type_reg.Match(protoVarTypeRow.Cells[iIndex].StringCellValue);
                if(match.Success && match.Groups[1].Value.Equals("enum"))
                {
                    return true;
                }
            }
            return false;
        }

        string m_proto_content = string.Empty;
        public void CreateProto(string applicationPath)
        {
            if(succeed)
            {
				bool useProto3 = this.ConvertMode == ExcelHelper.ConvertMode.CM_PROTO3;
				var path = System.IO.Path.GetFullPath(applicationPath + (useProto3 ? ExcelConfig.PROTO3_PATH : ExcelConfig.PROTO_PATH));
				var protoFilePath = Path.GetFullPath(path + ProtoNameWithExtend);
                try
                {
					StringBuilder kString = new StringBuilder(256);
					if (useProto3)
					{
						kString.AppendLine("syntax = \"proto3\";");
					}
                    if (m_bHasUnion)
                    {
                        kString.Append("import \"c_table_common.proto\";\n\n");
                    }
                    kString.Append("package TABLE;\n\n");
                    kString.AppendFormat("message {0}\n", m_sheet.SheetName.ToUpper());
                    kString.Append("{\n");
                    int iCnt = 0;
                    for(int i = 0; i < coloumValues.Length; ++i)
                    {
						var coloumValue = coloumValues[i];
						if(null == coloumValue)
						{
							continue;
						}

						if (null != coloumValue.bits && coloumValue.bits.Count > 0)
						{
							kString.AppendFormat("\t//@");
							for (int j = 0; j < coloumValue.bits.Count; ++j)
							{
								if(j != coloumValue.bits.Count - 1)
									kString.AppendFormat("{0}#",mVarName[coloumValue.bits[j].key]);
								else
									kString.AppendFormat("{0}",mVarName[coloumValue.bits[j].key]);
							}
                            for (int j = 0; j < coloumValue.bits.Count; ++j)
                            {
								kString.AppendFormat(" {0}#{1}", coloumValue.bits[j].key, coloumValue.bits[j].value);
							}
							kString.AppendLine();
						}

						if (!coloumValue.isValidToSearver)
						{
							kString.Append("\t//");
						}
						else
						{
							kString.Append("\t");
						}

						var head = useProto3 ? (coloumValue.head == "required" ? string.Empty : coloumValue.head) : coloumValue.head;
						if (coloumValue.type.Equals("enum"))
                        {
                            kString.Append(coloumValue.declare);
                            kString.AppendFormat("{0} {1} {2} = {3};\n", head, _generateEnumVarType(coloumValue.name), coloumValue.name, ++iCnt);
                        }
						else if(coloumValue.type.Equals("float"))
						{
							kString.AppendFormat("{0} {1} {2} = {3};\n", head, useProto3? "sint32" : "int32", coloumValue.name, ++iCnt);
						}
                        else
                        {
                            kString.AppendFormat("{0} {1} {2} = {3};\n", head, coloumValue.type, coloumValue.name, ++iCnt);
                        }
                    }
					kString.Append("};\n\n");

					kString.AppendFormat("message {0}ARRAY\n", m_sheet.SheetName.ToUpper());
					kString.Append("{\n");
					kString.AppendFormat("\trepeated {0} rows\t=1;\n",m_sheet.SheetName.ToUpper());
					kString.Append("};\n");

					m_proto_content = kString.ToString();
					File.WriteAllText(protoFilePath, m_proto_content, Encoding.ASCII);
                }
                catch (System.Exception ex)
                {
                    Debug.LogErrorFormat(ex.ToString());
                    _PrintErrMsg(ErrCode.EC_WRITE_PROTO_FAILED, protoFilePath);
                }
            }
        }

        string _generateVarType(string content)
        {
            if (content.StartsWith("union"))
            {
                return "UnionCell";
            }

			if (content.Trim() == "pair")
				return "KEYVALUE";

			return content;
        }

        string _generateName(string content)
        {
            var tokens = content.Split(':');
            if(tokens.Length > 0)
            {
				tokens [0] = tokens [0].Trim ();
                return tokens[0];
            }
            return content;
        }

        string _generateEnumVarType(string varName)
        {
            return string.Format("Enum{0}",varName);
        }

        void _insertTab(StringBuilder stringBuilder,int n = 2)
        {
            while (n-- > 0) stringBuilder.Append("\t");
        }

        string _generateEnumDeclare(ExcelColoumValue value,string content,int nPreTabs)
        {
            var tokens = content.Split(new char[] { '\r','\n'});
            List<Match> matches = new List<Match>();
            for(int i = 0; i < tokens.Length; ++i)
            {
                var match = ms_enum_prop_reg.Match(tokens[i]);
                if(match.Success)
                {
                    matches.Add(match);
                }
            }
			if (matches.Count > 0) 
			{
				if (null == value.enumValues) 
				{
					value.enumValues = new List<int> ();
				}
				value.enumValues.Clear ();
				for (int j = 0; j < matches.Count; ++j) 
				{
					int iRes = 0;
					if (int.TryParse (matches [j].Groups [2].Value, out iRes)) 
					{
						if (!value.enumValues.Contains (iRes)) 
						{
							value.enumValues.Add (iRes);
						}
					}
				}
			}
            if(matches.Count > 0)
            {
				StringBuilder kString = new StringBuilder(256);
                var ret = kString.ToString();
                _insertTab(kString, 1); kString.AppendFormat("enum {0}\n", value.propertyName);
                _insertTab(kString, 1); kString.Append("{\n");
                for(int i = 0; i < matches.Count; ++i)
                {
                    _insertTab(kString, 2); kString.AppendFormat("\t{0} = {1};\n", matches[i].Groups[1].Value, matches[i].Groups[2].Value/*, matches[i].Groups[3].Value*/);
                }
                _insertTab(kString, 1); kString.Append("}\n");
                ret = kString.ToString();
                return ret;
            }
            return string.Empty;
        }

        protected const string CONST_ARRAY_KEY = @"repeated";
        protected string[] CONST_VAR_TYPE_TO_STRING = new string[(int)VarType.VT_COUNT]
        {
            @"int",
			@"uint",
			@"long",
            @"float",
            @"string",
            @"union",
            @"bool",
            @"enum",
			@"TABLE.KEYVALUE",
		};
        protected string[] CONST_VAR_TYPE_TO_STRING_IL = new string[(int)VarType.VT_COUNT]
        {
            @"int",
            @"uint",
            @"long",
            @"float",
            @"string",
            @"union",
            @"bool",
            @"enum",
			@"long",
        };

        public string GetCSType(VarType eType)
		{
			return CONST_VAR_TYPE_TO_STRING[(int)eType];
		}

        public void generateCSharpCode(string application,string dir, ExcelUnit unit)
        {
			if (succeed)
            {
                try
                {
					StringBuilder stringBuilder = new StringBuilder(2048);
					stringBuilder.AppendLine($"//------------------------------------------------------------------------------");
					stringBuilder.AppendLine($"// <auto-generated>");
					stringBuilder.AppendLine($"//     This code was generated by a tool.");
					stringBuilder.AppendLine($"//");
					stringBuilder.AppendLine($"//     Changes to this file may cause incorrect behavior and will be lost if");
					stringBuilder.AppendLine($"//     the code is regenerated.");
					stringBuilder.AppendLine($"// <auto-generated>");
					stringBuilder.AppendLine($"//------------------------------------------------------------------------------");
					stringBuilder.AppendLine();
					stringBuilder.AppendFormat($"// Generated from: protos/{ProtoNameWithExtend}");
					stringBuilder.AppendLine();

					if (this.ConvertMode == ExcelHelper.ConvertMode.CM_PROTO3)
						stringBuilder.AppendLine($"using Google.Protobuf.Collections;");

					stringBuilder.AppendLine($"namespace TABLE");
					stringBuilder.AppendLine("{");
					stringBuilder.AppendLine($"\tpublic partial class {unit.SheetName.ToUpper()}");
					stringBuilder.AppendLine("\t{");

					for (int i = 0; i < coloumValues.Length; ++i)
					{
						var coloumItem = coloumValues[i];
						if(null == coloumItem || null == coloumItem.bits)
						{
							continue;
                        }

                        for (int j = 0; j < coloumItem.bits.Count; ++j)
						{
							stringBuilder.Append(coloumItem.bits[j].definitions);
						}
					}

                    stringBuilder.AppendLine($"\t}}");
					stringBuilder.Append($"}}");

					if (!System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }
                    var path = $"{dir}/c_table_{unit.SheetName.ToLower()}_extend.cs";
                    System.IO.File.WriteAllText(path, stringBuilder.ToString());

					stringBuilder.Clear();
                    stringBuilder.AppendLine($"//------------------------------------------------------------------------------");
                    stringBuilder.AppendLine($"// <auto-generated>");
                    stringBuilder.AppendLine($"//     This code was generated by a tool.");
                    stringBuilder.AppendLine($"//");
                    stringBuilder.AppendLine($"//     Changes to this file may cause incorrect behavior and will be lost if");
                    stringBuilder.AppendLine($"//     the code is regenerated.");
                    stringBuilder.AppendLine($"// <auto-generated>");
                    stringBuilder.AppendLine($"//------------------------------------------------------------------------------");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat($"// Generated from: protos/{ProtoNameWithExtend}");
                    stringBuilder.AppendLine();
					stringBuilder.AppendLine("using System.Collections.Generic;");
					if (this.ConvertMode == ExcelHelper.ConvertMode.CM_PROTO3)
						stringBuilder.AppendLine($"using Google.Protobuf.Collections;");

                    var tblClassName = unit.SheetName[0].ToString().ToUpper() + unit.SheetName.Substring(1, unit.SheetName.Length - 1) + "TableManager";

					stringBuilder.AppendFormat("public partial class {0} : TableManager<TABLE.{1}ARRAY, TABLE.{1},{2},{0}>", tblClassName, unit.SheetName.ToUpper(), "int");
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("{");
					stringBuilder.AppendLine("\tprotected override void OnResourceLoaded(CSResourceWWW res)");
					stringBuilder.AppendLine("\t{");
					stringBuilder.AppendLine("\t\tbase.OnResourceLoaded(res);");
					stringBuilder.AppendLine($"\t\tDic = new Dictionary<int, TABLE.{unit.SheetName.ToUpper()}>(array.rows.Count);");
					stringBuilder.AppendLine("\t\tif (array != null)");
					stringBuilder.AppendLine("\t\t{");
					stringBuilder.AppendLine("\t\t\tfor (int i = 0; i < array.rows.Count; i++)");
					stringBuilder.AppendLine("\t\t\t{");

					stringBuilder.AppendFormat("\t\t\t\tint id = array.rows[i].id;\n");
					stringBuilder.AppendLine("\t\t\t\tAddTables(id, array.rows[i]);");

					stringBuilder.AppendLine("\t\t\t}");
					stringBuilder.AppendLine("\t\t}");
					stringBuilder.AppendLine("\t\tbase.OnDealOver();");
					stringBuilder.AppendLine("\t}");

                    for (int i = 0; i < coloumValues.Length; ++i)
                    {
                        var coloumItem = coloumValues[i];
                        if (null == coloumItem || null == coloumItem.bits)
                        {
                            continue;
                        }

                        stringBuilder.Append(coloumItem.make_id_function);
                    }

                    var convernedSheetName = unit.SheetName[0].ToString().ToUpper() + unit.SheetName.Substring(1);

					for (int i = 0; i < coloumValues.Length; ++i)
                    {
                        var coloumItem = coloumValues[i];
                        if (null == coloumItem || !coloumItem.isValidToSearver)
                        {
                            continue;
                        }
						string convernedKeyName = string.Empty;
						convernedKeyName = coloumItem.name[0].ToString().ToUpper() + coloumItem.name.Substring(1);

						if(!coloumItem.isList)
						{
                            if (coloumItem.eVarType != VarType.VT_ENUM)
                            {
                                stringBuilder.AppendFormat("\tpublic {0} Get{1}{2}(int id,{0} defaultValue = default({0}))\n", getVarType(coloumItem), convernedSheetName, convernedKeyName);
                            }
                            else
                            {
								//RepeatedField
								if (this.ConvertMode != ExcelHelper.ConvertMode.CM_PROTO3)
									stringBuilder.AppendFormat("\tpublic TABLE.{0}.{1} Get{2}{3}(int id,TABLE.{0}.{1} defaultValue = default(TABLE.{0}.{1}))\n", unit.SheetName.ToUpper(), getVarType(coloumItem), convernedSheetName, convernedKeyName);
								else
									stringBuilder.AppendFormat("\tpublic TABLE.{0}.Types.{1} Get{2}{3}(int id,TABLE.{0}.Types.{1} defaultValue = default(TABLE.{0}.Types.{1}))\n", unit.SheetName.ToUpper(), getVarType(coloumItem), convernedSheetName, convernedKeyName);
							}
                        }
						else
						{
							if (this.ConvertMode != ExcelHelper.ConvertMode.CM_PROTO3)
							{
                                if (coloumItem.eVarType != VarType.VT_ENUM)
                                {
                                    stringBuilder.AppendFormat("\tpublic List<{0}> Get{1}{2}(int id,List<{0}> defaultValue = default(List<{0}>))\n", getVarType(coloumItem), convernedSheetName, convernedKeyName);
                                }
                                else
                                {
                                    stringBuilder.AppendFormat("\tpublic List<TABLE.{0}.{1}> Get{2}{3}(int id,List<TABLE.{0}.{1}> defaultValue = default(List<TABLE.{0}.{1}>))\n", unit.SheetName.ToUpper(), getVarType(coloumItem), convernedSheetName, convernedKeyName);
                                }
                            }
							else
							{
                                if (coloumItem.eVarType != VarType.VT_ENUM)
                                {
                                    stringBuilder.AppendFormat("\tpublic RepeatedField<{0}> Get{1}{2}(int id,RepeatedField<{0}> defaultValue = default(RepeatedField<{0}>))\n", getVarType(coloumItem), convernedSheetName, convernedKeyName);
                                }
                                else
                                {
                                    stringBuilder.AppendFormat("\tpublic RepeatedField<TABLE.{0}.Types.{1}> Get{2}{3}(int id,RepeatedField<TABLE.{0}.Types.{1}> defaultValue = default(RepeatedField<TABLE.{0}.Types.{1}>))\n", unit.SheetName.ToUpper(), getVarType(coloumItem), convernedSheetName, convernedKeyName);
                                }
                            }
                        }
                        stringBuilder.AppendLine("\t{");
                        stringBuilder.AppendFormat("\t\tTABLE.{0} cfg = null;\n", unit.SheetName.ToUpper());
                        //stringBuilder.AppendFormat("\t\tvar cfg = Get(id);\n");
                        stringBuilder.AppendFormat("\t\tif(TryGetValue(id,out cfg))\n");
                        stringBuilder.AppendFormat("\t\t{{\n");
                        stringBuilder.AppendFormat("\t\t\treturn cfg.{0};\n", coloumItem.name);
                        stringBuilder.AppendFormat("\t\t}}\n");
                        stringBuilder.AppendFormat("\t\telse\n");
                        stringBuilder.AppendFormat("\t\t{{\n");
                        stringBuilder.AppendFormat("\t\t\treturn defaultValue;\n");
                        stringBuilder.AppendFormat("\t\t}}\n");
                        stringBuilder.AppendLine("\t}");
                    }

					stringBuilder.Append("}");//end class

					var path2 = $"{dir}/{tblClassName}.cs";
					System.IO.File.WriteAllText(path2, stringBuilder.ToString());

					//var extendFilePath = $"{dir}/{tblClassName}Extend.cs";
					//stringBuilder.Clear();
					//stringBuilder.AppendFormat("public partial class {0} : TableManager<TABLE.{1}ARRAY, TABLE.{1},{2},{0}>", tblClassName, unit.SheetName.ToUpper(), KeyType);
				}
                catch(Exception e)
                {
                    _PrintErrMsg(ErrCode.EC_CONVERT_CS_FAILED, unit.SheetName, e.Message);
                }
            }
        }

        public void generateCSharpCodeForILFastMode(string application, string dir, ExcelUnit unit)
        {
            if (succeed)
            {
                try
                {
                    StringBuilder stringBuilder = new StringBuilder(2048);
                    stringBuilder.AppendLine($"//------------------------------------------------------------------------------");
                    stringBuilder.AppendLine($"// <auto-generated>");
                    stringBuilder.AppendLine($"//     This code was generated by a tool.");
                    stringBuilder.AppendLine($"//");
                    stringBuilder.AppendLine($"//     Changes to this file may cause incorrect behavior and will be lost if");
                    stringBuilder.AppendLine($"//     the code is regenerated.");
                    stringBuilder.AppendLine($"// <auto-generated>");
                    stringBuilder.AppendLine($"//------------------------------------------------------------------------------");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat($"// Generated from: protos/{ProtoNameWithExtend}");
                    stringBuilder.AppendLine();

                    stringBuilder.AppendLine($"namespace TABLE");
                    stringBuilder.AppendLine("{");
                    stringBuilder.AppendLine($"\tpublic partial class {unit.SheetName.ToUpper()}");
                    stringBuilder.AppendLine("\t{");

                    for (int i = 0; i < coloumValues.Length; ++i)
                    {
                        var coloumItem = coloumValues[i];
                        if (null == coloumItem || null == coloumItem.bits)
                        {
                            continue;
                        }

                        for (int j = 0; j < coloumItem.bits.Count; ++j)
                        {
                            stringBuilder.Append(coloumItem.bits[j].definitions);
                        }
                    }

                    stringBuilder.AppendLine($"\t}}");
                    stringBuilder.Append($"}}");

                    if (!System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }
                    var path = $"{dir}/c_table_{unit.SheetName.ToLower()}_extend.cs";
                    System.IO.File.WriteAllText(path, stringBuilder.ToString());

                    stringBuilder.Clear();
                    stringBuilder.AppendLine($"//------------------------------------------------------------------------------");
                    stringBuilder.AppendLine($"// <auto-generated>");
                    stringBuilder.AppendLine($"//     This code was generated by a tool.");
                    stringBuilder.AppendLine($"//");
                    stringBuilder.AppendLine($"//     Changes to this file may cause incorrect behavior and will be lost if");
                    stringBuilder.AppendLine($"//     the code is regenerated.");
                    stringBuilder.AppendLine($"// <auto-generated>");
                    stringBuilder.AppendLine($"//------------------------------------------------------------------------------");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendFormat($"// Generated from: protos/{ProtoNameWithExtend}");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("using System.Collections.Generic;");

                    var tblClassName = unit.SheetName[0].ToString().ToUpper() + unit.SheetName.Substring(1, unit.SheetName.Length - 1) + "TableManager";

                    stringBuilder.AppendFormat("public partial class {0} : TableManager<TABLE.{1}ARRAY, TABLE.{1},{2},{0}>", tblClassName, unit.SheetName.ToUpper(), "int");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("{");

                    stringBuilder.AppendLine($"\tpublic override bool TryGetValue(int key, out TABLE.{unit.SheetName.ToUpper()} tbl)");
                    stringBuilder.AppendLine("\t{");
                    stringBuilder.AppendLine("\t\ttbl = null;");
                    stringBuilder.AppendLine("\t\tif (array == null || array.gItem == null)");
					stringBuilder.AppendLine("\t\t\treturn false;");
					stringBuilder.AppendLine("\t\tTableHandle handle = null;");
					stringBuilder.AppendLine("\t\tif (array.gItem.id2offset.TryGetValue(key, out handle))");
					stringBuilder.AppendLine("\t\t{");
					stringBuilder.AppendLine($"\t\t\ttbl = handle.Value as TABLE.{unit.SheetName.ToUpper()};");
					stringBuilder.AppendLine("\t\t}");
                    stringBuilder.AppendLine("\t\treturn null != tbl;");
                    stringBuilder.AppendLine("\t}");

                    for (int i = 0; i < coloumValues.Length; ++i)
                    {
                        var coloumItem = coloumValues[i];
                        if (null == coloumItem || null == coloumItem.bits)
                        {
                            continue;
                        }

                        stringBuilder.Append(coloumItem.make_id_function);
                    }

                    var convernedSheetName = unit.SheetName[0].ToString().ToUpper() + unit.SheetName.Substring(1);

                    for (int i = 0; i < coloumValues.Length; ++i)
                    {
                        var coloumItem = coloumValues[i];
                        if (null == coloumItem || !coloumItem.isValidToSearver)
                        {
                            continue;
                        }
                        string convernedKeyName = string.Empty;
                        convernedKeyName = coloumItem.name[0].ToString().ToUpper() + coloumItem.name.Substring(1);

                        if (!coloumItem.isList)
                        {
                            if (coloumItem.eVarType != VarType.VT_ENUM)
                            {
                                stringBuilder.AppendFormat("\tpublic {0} Get{1}{2}(int id,{0} defaultValue = default({0}))\n", getVarType(coloumItem), convernedSheetName, convernedKeyName);
                            }
                            else
                            {
								stringBuilder.AppendFormat("\tpublic TABLE.{0}.Types.{1} Get{2}{3}(int id,TABLE.{0}.Types.{1} defaultValue = default(TABLE.{0}.Types.{1}))\n", unit.SheetName.ToUpper(), getVarType(coloumItem), convernedSheetName, convernedKeyName);
                            }
                        }
                        else
                        {
							if(coloumItem.eVarType == VarType.VT_STRING)
							{
								stringBuilder.AppendFormat("\tpublic StringArray Get{0}{1}(int id,StringArray defaultValue = default(StringArray))\n", convernedSheetName, convernedKeyName);
							}
							else if(coloumItem.eVarType == VarType.VT_PAIR || coloumItem.eVarType == VarType.VT_INT64)
							{
								stringBuilder.AppendFormat("\tpublic LongArray Get{0}{1}(int id,LongArray defaultValue = default(LongArray))\n", convernedSheetName, convernedKeyName);
							}
							else if (coloumItem.eVarType != VarType.VT_ENUM)
                            {
                                stringBuilder.AppendFormat("\tpublic IntArray Get{0}{1}(int id,IntArray defaultValue = default(IntArray))\n",convernedSheetName, convernedKeyName);
                            }
                            else
                            {
                                stringBuilder.AppendFormat("\tpublic TABLE.{0}.Types.{1}[] Get{2}{3}(int id,TABLE.{0}.Types.{1}[] defaultValue = default(TABLE.{0}.Types.{1}[]))\n", unit.SheetName.ToUpper(), getVarType(coloumItem), convernedSheetName, convernedKeyName);
                            }
                        }

                        stringBuilder.AppendLine("\t{");
                        stringBuilder.AppendFormat("\t\tTABLE.{0} cfg = null;\n", unit.SheetName.ToUpper());
                        //stringBuilder.AppendFormat("\t\tvar cfg = Get(id);\n");
                        stringBuilder.AppendFormat("\t\tif(TryGetValue(id,out cfg))\n");
                        stringBuilder.AppendFormat("\t\t{{\n");
						stringBuilder.AppendFormat("\t\t\treturn cfg.{0};\n", coloumItem.name);
						stringBuilder.AppendFormat("\t\t}}\n");
                        stringBuilder.AppendFormat("\t\telse\n");
                        stringBuilder.AppendFormat("\t\t{{\n");
                        stringBuilder.AppendFormat("\t\t\treturn defaultValue;\n");
                        stringBuilder.AppendFormat("\t\t}}\n");
                        stringBuilder.AppendLine("\t}");
                    }

                    stringBuilder.Append("}");//end class

                    var path2 = $"{dir}/{tblClassName}.cs";
                    System.IO.File.WriteAllText(path2, stringBuilder.ToString());

                    //var extendFilePath = $"{dir}/{tblClassName}Extend.cs";
                    //stringBuilder.Clear();
                    //stringBuilder.AppendFormat("public partial class {0} : TableManager<TABLE.{1}ARRAY, TABLE.{1},{2},{0}>", tblClassName, unit.SheetName.ToUpper(), KeyType);
                }
                catch (Exception e)
                {
                    _PrintErrMsg(ErrCode.EC_CONVERT_CS_FAILED, unit.SheetName, e.Message);
                }
            }
        }

        public string GetTableAssetPath(string sheetName)
		{
			var tableAssetStorePath = System.IO.Path.GetFullPath($"{ApplicationDataPath}/../../ProtoGen/assets/{sheetName.ToLower()}.bytes");
			return tableAssetStorePath;
		}

		public string GetTableAssetVerifyPath(string sheetName)
		{
            var tableAssetStorePath = System.IO.Path.GetFullPath($"{ApplicationDataPath}/../../Normal/zt_android/Table/{sheetName.ToLower()}.bytes");
            return tableAssetStorePath;
		}

		public bool HasTheSameContent(string sheetName)
		{
			var srcBytes = System.IO.File.ReadAllBytes(GetTableAssetPath(sheetName));
			var dstBytes = System.IO.File.ReadAllBytes(GetTableAssetVerifyPath(sheetName));
			if(srcBytes.Length != dstBytes.Length)
			{
				return false;
			}
            for (int i = 0; i < srcBytes.Length; ++i)
            {
                if (srcBytes[i] != dstBytes[i])
                    return false;
            }

            return true;
		}

        public void generateAsset(string path)
		{
			if (succeed)
			{
				var tableAssetStorePath = GetTableAssetPath(SheetName);
				if (System.IO.File.Exists(tableAssetStorePath))
				{
					System.IO.File.Delete(tableAssetStorePath);
				}
				_loadContent(m_sheet,path);
				if (!succeed)
				{
					Debug.LogErrorFormat("generate text failed ! [{0}]", m_sheet.SheetName);
				}
			}
		}

		public void CreateTableStruct(ISheet sheet)
		{

		}

		static Dictionary<string, string> type2CSTypeName = new Dictionary<string, string>
		{
			{ "sint32","int"},
			{ "uint32","uint"},
			{ "string","string"},
			{ "KEYVALUE","long"},
		};
		public void generateILFastMode()
		{
            if (succeed)
            {
				StringBuilder stringBuilder = new StringBuilder(1024);
				string className = $"{m_sheet.SheetName.ToUpper()}";
				stringBuilder.AppendLine($"namespace TABLE");
				stringBuilder.AppendLine($"{{");
				stringBuilder.AppendLine($"\tpublic partial class {className}");
				stringBuilder.AppendLine("\t{");

				stringBuilder.AppendLine("\t\tpublic TableHandle handle;");
				stringBuilder.AppendLine("\t\tpublic TableData __data;");
				stringBuilder.AppendLine("\t\tpublic ILFastMode Array;");

				int idxInt = 0;
				int idxString = 0;
				int idxLong = 0;

				//先处理定长(对于定长写得是立即数，变长写的是pos和长度和给)
				for(int i = 0; i < coloumValues.Length; ++i)
				{
					if (null == coloumValues[i])
						continue;

					if(!coloumValues[i].isValidToSearver)
					{
						continue;
					}

					string varType = type2CSTypeName[coloumValues[i].type];
					string varName = coloumValues[i].name;
					bool isList = coloumValues[i].isList;

					if (coloumValues[i].eVarType == VarType.VT_STRING)
					{
						if(!isList)
                        {
                            stringBuilder.AppendLine($"\t\tpublic {varType} {varName}");
                            stringBuilder.AppendLine($"\t\t{{");
                            stringBuilder.AppendLine($"\t\t\tget");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\treturn __data.stringValues[{idxString} + handle.stringOffset];");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t\tset");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\t__data.stringValues[{idxString} + handle.stringOffset] = value;");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t}}");
                            ++idxString;
                        }
						else
						{
                            stringBuilder.AppendLine($"\t\tpublic int {varName}Ptr");
                            stringBuilder.AppendLine($"\t\t{{");
                            stringBuilder.AppendLine($"\t\t\tget");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\treturn __data.intValues[{idxInt} + handle.intOffset];");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t\tset");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\t__data.intValues[{idxInt} + handle.intOffset] = value;");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t}}");
                            ++idxInt;

                            stringBuilder.AppendLine($"\t\tpublic StringArray {varName}");
                            stringBuilder.AppendLine($"\t\t{{");
                            stringBuilder.AppendLine($"\t\t\tget");
                            stringBuilder.AppendLine($"\t\t\t{{");
							stringBuilder.AppendLine($"\t\t\t\tStringArray array;");
							stringBuilder.AppendLine($"\t\t\t\tarray.__fastMode = this.Array;");
							stringBuilder.AppendLine($"\t\t\t\tarray.__start = {varName}Ptr & 0xFFFFF;");
							stringBuilder.AppendLine($"\t\t\t\tarray.__length = ({varName}Ptr >> 20) & 0xFFF;");
							stringBuilder.AppendLine($"\t\t\t\tarray.__end = array.__length + array.__start;");
							stringBuilder.AppendLine($"\t\t\t\treturn array;");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t}}");
                        }
					}
					else if (coloumValues[i].eVarType == VarType.VT_PAIR)
					{
                        if (!isList)
                        {
                            stringBuilder.AppendLine($"\t\tpublic long {varName}");
                            stringBuilder.AppendLine($"\t\t{{");
                            stringBuilder.AppendLine($"\t\t\tget");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\treturn __data.longValues[{idxLong} + handle.longOffset];");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t\tset");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\t__data.longValues[{idxLong} + handle.longOffset] = value;");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t}}");
                            ++idxLong;
                        }
                        else
                        {
                            stringBuilder.AppendLine($"\t\tpublic int {varName}Ptr");
                            stringBuilder.AppendLine($"\t\t{{");
                            stringBuilder.AppendLine($"\t\t\tget");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\treturn __data.intValues[{idxInt} + handle.intOffset];");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t\tset");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\t__data.intValues[{idxInt} + handle.intOffset] = value;");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t}}");
                            ++idxInt;

                            stringBuilder.AppendLine($"\t\tpublic LongArray {varName}");
                            stringBuilder.AppendLine($"\t\t{{");
                            stringBuilder.AppendLine($"\t\t\tget");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\tLongArray array;");
                            stringBuilder.AppendLine($"\t\t\t\tarray.__fastMode = this.Array;");
                            stringBuilder.AppendLine($"\t\t\t\tarray.__start = {varName}Ptr & 0xFFFFF;");
                            stringBuilder.AppendLine($"\t\t\t\tarray.__length = ({varName}Ptr >> 20) & 0xFFF;");
                            stringBuilder.AppendLine($"\t\t\t\tarray.__end = array.__length + array.__start;");
                            stringBuilder.AppendLine($"\t\t\t\treturn array;");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t}}");
                        }
                    }
					else if (coloumValues[i].eVarType != VarType.VT_ENUM)
					{
						if(!isList)
						{
							stringBuilder.AppendLine($"\t\tpublic {varType} {varName}");
                            stringBuilder.AppendLine($"\t\t{{");
                            stringBuilder.AppendLine($"\t\t\tget");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\t return ({varType})__data.intValues[{idxInt} + handle.intOffset];");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t\tset");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\t__data.intValues[{idxInt} + handle.intOffset] = (int)value;");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t}}");
                            ++idxInt;
                        }
						else
						{
                            stringBuilder.AppendLine($"\t\tpublic int {varName}Ptr");
                            stringBuilder.AppendLine($"\t\t{{");
                            stringBuilder.AppendLine($"\t\t\tget");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\t return (int)__data.intValues[{idxInt} + handle.intOffset];");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t\tset");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\t__data.intValues[{idxInt} + handle.intOffset] = (int)value;");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t}}");
                            ++idxInt;

                            stringBuilder.AppendLine($"\t\tpublic IntArray {varName}");
                            stringBuilder.AppendLine($"\t\t{{");
                            stringBuilder.AppendLine($"\t\t\tget");
                            stringBuilder.AppendLine($"\t\t\t{{");
                            stringBuilder.AppendLine($"\t\t\t\tIntArray array;");
                            stringBuilder.AppendLine($"\t\t\t\tarray.__fastMode = this.Array;");
                            stringBuilder.AppendLine($"\t\t\t\tarray.__start = {varName}Ptr & 0xFFFFF;");
                            stringBuilder.AppendLine($"\t\t\t\tarray.__length = ({varName}Ptr >> 20) & 0xFFF;");
                            stringBuilder.AppendLine($"\t\t\t\tarray.__end = array.__length + array.__start;");
                            stringBuilder.AppendLine($"\t\t\t\treturn array;");
                            stringBuilder.AppendLine($"\t\t\t}}");
                            stringBuilder.AppendLine($"\t\t}}");
                        }
                    }
					else
					{
						Debug.LogError("EnumWaitDeal ...");
					}
				}

                stringBuilder.AppendLine("\t}");
				stringBuilder.AppendLine($"\tpublic static class {className}Helper");
				stringBuilder.AppendLine("\t{");
				stringBuilder.AppendLine($"\t\tpublic static void Encode(this System.IO.Stream stream,{className} item)");
				stringBuilder.AppendLine("\t\t{");
                for (int i = 0; i < coloumValues.Length; ++i)
                {
					if (null == coloumValues[i])
						continue;
                    if (!coloumValues[i].isValidToSearver)
                    {
                        continue;
                    }
                    string varName = coloumValues[i].name;
					if(!coloumValues[i].isList)
						stringBuilder.AppendLine($"\t\t\tstream.Encode(item.{varName});");
					else
						stringBuilder.AppendLine($"\t\t\tstream.Encode(item.{varName}Ptr);");
				}
                stringBuilder.AppendLine("\t\t}");
                stringBuilder.AppendLine("\t}");

				stringBuilder.AppendLine($"\tpublic class {className}ARRAY  : ILFastMode");
				stringBuilder.AppendLine("\t{");
				stringBuilder.AppendLine($"\t\tpublic {className}ARRAY()");
				stringBuilder.AppendLine("\t\t{");
				stringBuilder.AppendLine($"\t\t\tLongValueFixedLength = {idxLong};");
				stringBuilder.AppendLine($"\t\t\tIntValueFixedLength = {idxInt};");
				stringBuilder.AppendLine($"\t\t\tStringValueFixedLength = {idxString};");
				stringBuilder.Append($"\t\t\tRules = new byte[]{{");

                for (int i = 0; i < coloumValues.Length; ++i)
                {
                    if (null == coloumValues[i])
                        continue;
                    if (!coloumValues[i].isValidToSearver)
					{
						continue;
					}
					string varName = coloumValues[i].name;
					if(coloumValues[i].isList)
					{
                        if (i == 0)
                        {
                            stringBuilder.Append("1");
                        }
                        else
                        {
                            stringBuilder.Append(",1");
                        }
                    }
					else
					{
                        if (coloumValues[i].eVarType == VarType.VT_STRING)
                        {
                            if (i == 0)
                            {
                                stringBuilder.Append("2");
                            }
                            else
                            {
                                stringBuilder.Append(",2");
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                stringBuilder.Append("1");
                            }
                            else
                            {
                                stringBuilder.Append(",1");
                            }
                        }
                    }
                }
				stringBuilder.AppendLine("};");

				stringBuilder.AppendLine("\t\t}");

				stringBuilder.AppendLine($"\t\tpublic override void Decode(byte[] contents)");
                stringBuilder.AppendLine("\t\t{");

				stringBuilder.AppendLine($"\t\t\tgItem = GDecoder.LoadTable(contents, this.Rules);");
				stringBuilder.AppendLine($"\t\t\tthis.VarIntValues = GDecoder.varIntValues;");
				stringBuilder.AppendLine($"\t\t\tthis.VarStringValues = GDecoder.varStringValues;");
				stringBuilder.AppendLine($"\t\t\tthis.VarLongValues = GDecoder.varLongValues;");
                stringBuilder.AppendLine($"\t\t\tvar handles = gItem.handles;");
                stringBuilder.AppendLine($"\t\t\tTableHandle handle = null;");

                //stringBuilder.AppendLine($"\t\t\tDic = new System.Collections.Generic.Dictionary<int, ILFastModeItem>(cnt);");

                stringBuilder.AppendLine($"\t\t\tfor (int i = 0, max = handles.Length; i < max; ++i)");
				stringBuilder.AppendLine($"\t\t\t{{");
				stringBuilder.AppendLine($"\t\t\t\tTABLE.{className} randAttr = new TABLE.{className}();");
				stringBuilder.AppendLine($"\t\t\t\thandle = handles[i];");
				stringBuilder.AppendLine($"\t\t\t\trandAttr.__data = handle.data;");
				stringBuilder.AppendLine($"\t\t\t\trandAttr.Array = this;");
				stringBuilder.AppendLine($"\t\t\t\thandle.Value = randAttr;");
				stringBuilder.AppendLine($"\t\t\t\trandAttr.handle = handle;");
				stringBuilder.AppendLine($"\t\t\t}}");
				stringBuilder.AppendLine("\t\t}");

				stringBuilder.AppendLine("\t}");
				stringBuilder.AppendLine("}");

				var fileName = $"c_table_{className.ToLower()}";
				var savePath = $"{Application.dataPath}/HotFix_Project/Hotfix/Table/{fileName}.cs";
				System.IO.File.WriteAllText(savePath, stringBuilder.ToString());
				AssetDatabase.Refresh();
			}
        }

        public void Close()
        {
            if (null != m_fileStream)
            {
                m_fileStream.Close();
                m_fileStream = null;
            }

            try
			{
                if (null != m_workbook)
                {
					(m_workbook as NPOI.POIXMLDocument).Close();

					m_workbook.Close();
                    m_workbook = null;
                }
            }
			catch(Exception e)
			{
				Debug.LogFormat("close error , igonre it ...");
			}

            headLineRow = null;
            protoVarTypeRow = null;
            protoVarNameRow = null;
            serverClientFlagRow = null;
            m_sheet = null;
            name = string.Empty;
            m_bInitialized = false;
        }
    }
}