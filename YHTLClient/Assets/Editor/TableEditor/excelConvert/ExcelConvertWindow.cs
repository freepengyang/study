using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

namespace Smart.Editor
{
	class ExcelConvertEditorWindow : EditorWindow 
	{
		[MenuItem ("Tools/传奇/Excel转表")]
		static void AddWindow ()
		{       
			//创建窗口
			Rect  wr = new Rect (0,0,316,420);
			ExcelConvertEditorWindow window = (ExcelConvertEditorWindow)EditorWindow.GetWindowWithRect (typeof (ExcelConvertEditorWindow),wr,true,"客户端-转表");	
			window.Show();
		}

        [Serializable]
		public class ConvertInfo
		{
			public bool bSelected = false;
			public string name = string.Empty;
			public string lowerName = string.Empty;
			public string path = string.Empty;
			public bool HasProto = false;
			public bool HasUpgraded = false;
		}

		protected string filter = string.Empty;
		protected List<ConvertInfo> fileNames = new List<ConvertInfo>();

		Vector2 _scrollPos = Vector2.zero;

		HashSet<string> mIgnoreList = new HashSet<string>();
        protected void LoadIgnoreList()
        {
            mIgnoreList.Clear();
            var path = System.IO.Path.GetFullPath($"{Application.dataPath}/../../table/ignore_table.txt");
            var lines = System.IO.File.ReadAllLines(path);
            foreach (var name in lines)
            {
                if (!string.IsNullOrEmpty(name))
                    mIgnoreList.Add(name);
            }
        }

        HashSet<string> mModelList = new HashSet<string>();
        protected void LoadModelList()
        {
            mModelList.Clear();
            var path = System.IO.Path.GetFullPath($"{Application.dataPath}/../../table/model_table.txt");
            var lines = System.IO.File.ReadAllLines(path);
            foreach (var name in lines)
            {
                if (!string.IsNullOrEmpty(name))
                    mModelList.Add(name);
            }
        }

        protected void OnEnable()
		{
            LoadIgnoreList();
            LoadModelList();
            _ListAllExcelFiles();
		}

		void _ListAllExcelFiles()
		{
			fileNames.Clear ();
			var dir = Path.GetFullPath (Application.dataPath + ExcelConfig.XLSX_PATH);
			var newDir = Path.GetFullPath(Application.dataPath + "/../../ProtoGen/UpgradeTable/");
			if (System.IO.Directory.Exists(dir))
			{
                string[] strList = Directory.GetFiles(dir, "*.xls*", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < strList.Length; ++i)
                {
					var filePath = strList[i];
					var name = Path.GetFileNameWithoutExtension(filePath);
					if(mIgnoreList.Contains(name))
					{
						continue;
					}
					fileNames.Add(new ConvertInfo
                    {
                        bSelected = false,
                        name = name,
						lowerName = name.ToLower(),
						path = filePath,
					});
                }
            }
			else
			{
				UnityEngine.Debug.Log($"dir for {dir} not exist,created");
				System.IO.Directory.CreateDirectory(dir);
			}
		}

		protected void _LoadShellCmd(string shell,string argv)
		{
			Process process = new Process();
			process.StartInfo.FileName = shell;
			process.StartInfo.Arguments = argv;
			process.StartInfo.CreateNoWindow = false;
			process.StartInfo.ErrorDialog = true;
			process.StartInfo.UseShellExecute = false;

			//重新定向标准输入，输入，错误输出
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.Start ();

			string strRst = process.StandardOutput.ReadToEnd();
			if(!string.IsNullOrEmpty(strRst))
			{
				UnityEngine.Debug.LogFormat ("<color=#00ff00>{0}</color>",strRst);
			}
			strRst = process.StandardError.ReadToEnd();
			if (!string.IsNullOrEmpty (strRst)) 
			{
				UnityEngine.Debug.LogFormat ("<color=#ff0000>{0}</color>",strRst);
			}
            process.WaitForExit();
            process.Close();
        }

		public static void ProtoToCSharp(string name)
		{
			string protoc = "";
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				protoc = "protoc.exe";
			}
			else
			{
				protoc = "protoc";
			}

			var workDirectory = System.IO.Path.GetFullPath(Application.dataPath + "/../../Proto3/");
			if (!System.IO.Directory.Exists(workDirectory))
			{
				UnityEngine.Debug.LogError($"directory not exist for {workDirectory}");
				return;
			}

			string argv = "--csharp_out=\"./../ProtoGen/Table/\" --proto_path=\"./proto/\" " + name;

			ProcessHelper.Run(workDirectory + protoc, argv, workDirectory, true);
		}

		protected void OnProto2GUI()
		{
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("转表-Proto", GUILayout.Width(100)))
            {
                string sheetName = string.Empty;
                for (int i = 0; i < fileNames.Count; ++i)
                {
                    if (fileNames[i].bSelected)
                    {
                        if (ExcelManager.Instance().Convert(Application.dataPath, fileNames[i].path, ExcelHelper.ConvertType.CT_PROTO, out sheetName,ExcelHelper.ConvertMode.CM_PROTO2))
                        {
                            UnityEngine.Debug.LogFormat("<color=#00ff00>convert <color=#ffff00>{0}.cs</color> succeed !!</color>", fileNames[i].name);
                        }
                        else
                        {
                            UnityEngine.Debug.LogFormat("<color=#ff0000>convert <color=#ffff00>{0}.cs</color> failed !!</color>", fileNames[i].name);
                        }
                    }
                }

                AssetDatabase.Refresh();
            }
            if (GUILayout.Button("转表-CS", GUILayout.Width(100)))
            {
                List<string> selectedFileNames = new List<string>(0);
                string sheetName = string.Empty;
                for (int i = 0; i < fileNames.Count; ++i)
                {
                    if (fileNames[i].bSelected)
                    {
                        if (ExcelManager.Instance().Convert(Application.dataPath, fileNames[i].path, ExcelHelper.ConvertType.CT_CSHARP, out sheetName, ExcelHelper.ConvertMode.CM_PROTO2,true))
                        {
                            selectedFileNames.Add($"c_table_{sheetName.ToLower()}");
                            UnityEngine.Debug.LogFormat("<color=#00ff00>convert <color=#ffff00>{0}.cs</color> succeed !!</color>", fileNames[i].name);
                        }
                        else
                        {
                            UnityEngine.Debug.LogFormat("<color=#ff0000>convert <color=#ffff00>{0}.cs</color> failed !!</color>", fileNames[i].name);
                        }
                    }
                }

                if (selectedFileNames.Count > 0)
                {
                    try
                    {
                        ProcessHelper.ProtoToCS(selectedFileNames.ToArray());
                        UnityEngine.Debug.LogFormat("<color=#00ff00>convert <color=#ffff00>all cs file</color> succeed !!</color>");

                        for (int i = 0; i < fileNames.Count; ++i)
                        {
                            if (fileNames[i].bSelected)
                            {
                                try
                                {
                                    var src = System.IO.Path.GetFullPath(Application.dataPath + "/../../ProtoGen/table");
                                    var dst = System.IO.Path.GetFullPath(Application.dataPath + "/../../YHTLClient/Assets/Main_Project/Table");
                                    var dst_tbl_mgr = System.IO.Path.GetFullPath(Application.dataPath + "/../../YHTLClient/Assets/HotFix_Project/Hotfix/Scripts/TableManager/Generated");
                                    var table_name = $"c_table_{fileNames[i].name.ToLower()}.cs";
                                    var table_name_extend = $"c_table_{fileNames[i].name.ToLower()}_extend.cs";
                                    var table_mgr_name = fileNames[i].name[0].ToString().ToUpper() + fileNames[i].name.Substring(1, fileNames[i].name.Length - 1) + "TableManager.cs";
                                    System.IO.File.Copy($"{src}/{table_name}", $"{dst}/{table_name}", true);
                                    UnityEngine.Debug.LogFormat("<color=#00ff00>move<color=#ffff00>{0}</color> succeed !!</color>", table_name);
                                    System.IO.File.Copy($"{src}/{table_name_extend}", $"{dst}/{table_name_extend}", true);
                                    UnityEngine.Debug.LogFormat("<color=#00ff00>move<color=#ffff00>{0}</color> succeed !!</color>", table_name_extend);
                                    System.IO.File.Copy($"{src}/{table_mgr_name}", $"{dst_tbl_mgr}/{table_mgr_name}", true);
                                    UnityEngine.Debug.LogFormat("<color=#00ff00>move<color=#ffff00>{0}</color> succeed !!</color>", table_mgr_name);
                                    //var guid = AssetDatabase.AssetPathToGUID($"Assets/Main_Project/Table/{table_name}");
                                }
                                catch (Exception e)
                                {
                                    UnityEngine.Debug.LogFormat("<color=#ff0000>移动CS文件到工程 <color=#ffff00>{0}</color> failed !!</color>", fileNames[i].name);
                                    UnityEngine.Debug.LogError(e.Message);
                                }
                            }
                        }

                        AssetDatabase.Refresh();
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogFormat("<color=#ff0000>convert <color=#ffff00>all cs file</color> failed !!</color>");
                        UnityEngine.Debug.LogError(e.Message);
                    }
                }
            }
            if (GUILayout.Button("转表-Asset", GUILayout.Width(100)))
            {
                for (int i = 0; i < fileNames.Count; ++i)
                {
                    if (fileNames[i].bSelected)
                    {
                        var dir = Path.GetFullPath(fileNames[i].path);
                        if (ExcelConfig.XLSX_NEED_ADAPT_OLD)
                        {
                            dir = Path.GetFullPath(Application.dataPath + $"/../../ProtoGen/UpgradeTable/{Path.GetFileName(fileNames[i].path)}");
                        }
                        var excelUnit = new ExcelUnit(dir);
                        excelUnit.Init(Application.dataPath, ExcelHelper.ConvertMode.CM_PROTO2,true);
                        excelUnit.LoadProtoBase();
                        excelUnit.generateAsset(fileNames[i].path);
                        excelUnit.Close();
                    }
                }

                //AssetDatabase.Refresh();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("策划在此处转表↑↑↑↑↑↑↑"))
            {

            }
        }
        protected void OnConvertP3GUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("转表-Proto3", GUILayout.Width(100)))
            {
                string sheetName = string.Empty;
                for (int i = 0; i < fileNames.Count; ++i)
                {
                    if (fileNames[i].bSelected)
                    {
                        if (ExcelManager.Instance().Convert(Application.dataPath, fileNames[i].path, ExcelHelper.ConvertType.CT_PROTO, out sheetName, ExcelHelper.ConvertMode.CM_PROTO3))
                        {
                            UnityEngine.Debug.LogFormat("<color=#00ff00>convert <color=#ffff00>{0}.proto</color> succeed !!</color>", fileNames[i].name);
                        }
                        else
                        {
                            UnityEngine.Debug.LogFormat("<color=#ff0000>convert <color=#ffff00>{0}.proto</color> failed !!</color>", fileNames[i].name);
                        }
                    }
                }
            }
            if (GUILayout.Button("转表-CS-P3", GUILayout.Width(100)))
            {
                List<string> selectedFileNames = new List<string>(0);
                string sheetName = string.Empty;
                for (int i = 0; i < fileNames.Count; ++i)
                {
                    if (fileNames[i].bSelected)
                    {
                        try
                        {
                            if (ExcelManager.Instance().Convert(Application.dataPath, fileNames[i].path, ExcelHelper.ConvertType.CT_CSHARP, out sheetName, ExcelHelper.ConvertMode.CM_PROTO3, mModelList.Contains(fileNames[i].name)))
                            {
                                selectedFileNames.Add($"c_table_{sheetName.ToLower()}");
                                UnityEngine.Debug.LogFormat("<color=#00ff00>convert <color=#ffff00>{0}.cs</color> succeed !!</color>", fileNames[i].name);
                            }
                            else
                            {
                                UnityEngine.Debug.LogFormat("<color=#ff0000>convert <color=#ffff00>{0}.cs</color> failed !!</color>", fileNames[i].name);
                                break;
                            }

                            var excelUnit = new ExcelUnit(fileNames[i].path);
                            excelUnit.Init(Application.dataPath, ExcelHelper.ConvertMode.CM_PROTO3, mModelList.Contains(fileNames[i].name));
                            sheetName = excelUnit.SheetName;
                            excelUnit.Close();
                            //ProtoToCSharp("c_table_common.proto");
                            var name = $"c_table_{sheetName.ToLower()}.proto";
                            ProtoToCSharp(name);
                            UnityEngine.Debug.LogFormat("<color=#00ff00>convert <color=#ffff00>{0}.cs</color> succeed !!</color>", fileNames[i].name);
                        }
                        catch (Exception e)
                        {
                            UnityEngine.Debug.LogFormat("<color=#ff0000>convert <color=#ffff00>{0}.cs</color> failed !!</color>", fileNames[i].name);
                            UnityEngine.Debug.LogErrorFormat("error={0}", e.Message);
                        }
                    }
                }

                if (selectedFileNames.Count > 0)
                {
                    try
                    {
                        ProcessHelper.ProtoToCS(selectedFileNames.ToArray());
                        UnityEngine.Debug.LogFormat("<color=#00ff00>convert <color=#ffff00>all cs file</color> succeed !!</color>");

                        for (int i = 0; i < fileNames.Count; ++i)
                        {
                            if (fileNames[i].bSelected)
                            {
                                try
                                {
                                    var src = System.IO.Path.GetFullPath(Application.dataPath + "/../../ProtoGen/table");
                                    var dst = string.Empty;
                                    if(mModelList.Contains(fileNames[i].name))
                                        dst = System.IO.Path.GetFullPath(Application.dataPath + "/../../YHTLClient/Assets/Main_Project/Table");
                                    else
                                        dst = System.IO.Path.GetFullPath(Application.dataPath + "/../../YHTLClient/Assets/HotFix_Project/Hotfix/Table");
                                    var dst_tbl_mgr = System.IO.Path.GetFullPath(Application.dataPath + "/../../YHTLClient/Assets/HotFix_Project/Hotfix/Scripts/TableManager/Generated");

                                    var table_name = $"CTable{fileNames[i].name[0].ToString().ToUpper()}{fileNames[i].name.Substring(1).ToLower()}.cs";
                                    var dst_table_name = $"c_table_{fileNames[i].name.ToLower()}.cs";
                                    var table_name_extend = $"c_table_{fileNames[i].name.ToLower()}_extend.cs";
                                    var table_mgr_name = fileNames[i].name[0].ToString().ToUpper() + fileNames[i].name.Substring(1, fileNames[i].name.Length - 1) + "TableManager.cs";
                                    //var table_common_name = "CTableCommon.cs";
                                    //var dst_common_table_name = "c_table_common.cs";

                                    var gcontent = System.IO.File.ReadAllText($"{src}/{table_name}");
                                    gcontent = gcontent.Replace("public sealed class", "public partial class");
                                    //gcontent = ImessageNoAttr.ReplaceContent(gcontent);

                                    System.IO.File.WriteAllText($"{src}/{table_name}", gcontent);

                                    //System.IO.File.Copy($"{src}/{table_common_name}", $"{dst}/{dst_common_table_name}", true);
                                    //UnityEngine.Debug.LogFormat("<color=#00ff00>move<color=#ffff00>{0}</color> succeed !!</color>", table_common_name);
                                    System.IO.File.Copy($"{src}/{table_name}", $"{dst}/{dst_table_name}", true);
                                    UnityEngine.Debug.LogFormat("<color=#00ff00>move<color=#ffff00>{0}</color> succeed !!</color>", table_name);
                                    System.IO.File.Copy($"{src}/{table_name_extend}", $"{dst}/{table_name_extend}", true);
                                    UnityEngine.Debug.LogFormat("<color=#00ff00>move<color=#ffff00>{0}</color> succeed !!</color>", table_name_extend);
                                    System.IO.File.Copy($"{src}/{table_mgr_name}", $"{dst_tbl_mgr}/{table_mgr_name}", true);
                                    UnityEngine.Debug.LogFormat("<color=#00ff00>move<color=#ffff00>{0}</color> succeed !!</color>", table_mgr_name);
                                    //var guid = AssetDatabase.AssetPathToGUID($"Assets/Main_Project/Table/{table_name}");
                                }
                                catch (Exception e)
                                {
                                    UnityEngine.Debug.LogFormat("<color=#ff0000>移动CS文件到工程 <color=#ffff00>{0}</color> failed !!</color>", fileNames[i].name);
                                    UnityEngine.Debug.LogError(e.Message);
                                }
                            }
                        }

                        AssetDatabase.Refresh();
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogFormat("<color=#ff0000>convert <color=#ffff00>all cs file</color> failed !!</color>");
                        UnityEngine.Debug.LogError(e.Message);
                    }
                }
            }
            if (GUILayout.Button("转表-Asset-P3", GUILayout.Width(100)))
            {
                for (int i = 0; i < fileNames.Count; ++i)
                {
                    if (fileNames[i].bSelected)
                    {
                        var dir = Path.GetFullPath(fileNames[i].path);
                        if (ExcelConfig.XLSX_NEED_ADAPT_OLD)
                        {
                            dir = Path.GetFullPath(Application.dataPath + $"/../../ProtoGen/UpgradeTable/{Path.GetFileName(fileNames[i].path)}");
                        }
                        var excelUnit = new ExcelUnit(dir);
                        excelUnit.Init(Application.dataPath, ExcelHelper.ConvertMode.CM_PROTO3,mModelList.Contains(fileNames[i].name));
                        excelUnit.LoadProtoBase();
                        excelUnit.generateAsset(fileNames[i].path);
                        excelUnit.Close();
                    }
                }

                //AssetDatabase.Refresh();
            }
            EditorGUILayout.EndHorizontal();
        }
        protected void OnConvertILFastMode()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("转表-CS-ILMode", GUILayout.Width(152)))
            {
                List<string> selectedFileNames = new List<string>(0);
                string sheetName = string.Empty;
                for (int i = 0; i < fileNames.Count; ++i)
                {
                    if (fileNames[i].bSelected)
                    {
                        try
                        {
                            if (ExcelManager.Instance().Convert(Application.dataPath, fileNames[i].path, ExcelHelper.ConvertType.CT_CSHARP, out sheetName,ExcelHelper.ConvertMode.CM_IL_FAST_MODE, mModelList.Contains(fileNames[i].name)))
                            {
                                selectedFileNames.Add($"c_table_{sheetName.ToLower()}");
                                UnityEngine.Debug.LogFormat("<color=#00ff00>convert <color=#ffff00>{0}.cs</color> succeed !!</color>", fileNames[i].name);
                            }
                            else
                            {
                                UnityEngine.Debug.LogFormat("<color=#ff0000>convert <color=#ffff00>{0}.cs</color> failed !!</color>", fileNames[i].name);
                                break;
                            }

                            var excelUnit = new ExcelUnit(fileNames[i].path);
                            excelUnit.Init(Application.dataPath, ExcelHelper.ConvertMode.CM_IL_FAST_MODE, mModelList.Contains(fileNames[i].name));
                            excelUnit.LoadProtoBase();
                            sheetName = excelUnit.SheetName;
                            excelUnit.generateILFastMode();
                            excelUnit.Close();
                            //ProtoToCSharp("c_table_common.proto");
                            //var name = $"c_table_{sheetName.ToLower()}.proto";
                            //ProtoToCSharp(name);
                            UnityEngine.Debug.LogFormat("<color=#00ff00>convert <color=#ffff00>{0}.cs</color> succeed !!</color>", fileNames[i].name);
                        }
                        catch (Exception e)
                        {
                            UnityEngine.Debug.LogFormat("<color=#ff0000>convert <color=#ffff00>{0}.cs</color> failed !!</color>", fileNames[i].name);
                            UnityEngine.Debug.LogErrorFormat("error={0}", e.Message);
                        }
                    }
                }
                UnityEngine.Debug.LogFormat("<color=#00ff00>convert <color=#ffff00>all cs file</color> succeed !!</color>");

                if (selectedFileNames.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < fileNames.Count; ++i)
                        {
                            if (fileNames[i].bSelected)
                            {
                                try
                                {
                                    var src = System.IO.Path.GetFullPath(Application.dataPath + "/../../ProtoGen/table");
                                    var dst = string.Empty;
                                    if (mModelList.Contains(fileNames[i].name))
                                        dst = System.IO.Path.GetFullPath(Application.dataPath + "/../../YHTLClient/Assets/Main_Project/Table");
                                    else
                                        dst = System.IO.Path.GetFullPath(Application.dataPath + "/../../YHTLClient/Assets/HotFix_Project/Hotfix/Table");
                                    var dst_tbl_mgr = System.IO.Path.GetFullPath(Application.dataPath + "/../../YHTLClient/Assets/HotFix_Project/Hotfix/Scripts/TableManager/Generated");

                                    var table_name_extend = $"c_table_{fileNames[i].name.ToLower()}_extend.cs";
                                    var table_mgr_name = fileNames[i].name[0].ToString().ToUpper() + fileNames[i].name.Substring(1, fileNames[i].name.Length - 1) + "TableManager.cs";

                                    System.IO.File.Copy($"{src}/{table_name_extend}", $"{dst}/{table_name_extend}", true);
                                    UnityEngine.Debug.LogFormat("<color=#00ff00>move<color=#ffff00>{0}</color> succeed !!</color>", table_name_extend);
                                    System.IO.File.Copy($"{src}/{table_mgr_name}", $"{dst_tbl_mgr}/{table_mgr_name}", true);
                                    UnityEngine.Debug.LogFormat("<color=#00ff00>move<color=#ffff00>{0}</color> succeed !!</color>", table_mgr_name);
                                    //var guid = AssetDatabase.AssetPathToGUID($"Assets/Main_Project/Table/{table_name}");
                                }
                                catch (Exception e)
                                {
                                    UnityEngine.Debug.LogFormat("<color=#ff0000>移动CS文件到工程 <color=#ffff00>{0}</color> failed !!</color>", fileNames[i].name);
                                    UnityEngine.Debug.LogError(e.Message);
                                }
                            }
                        }

                        AssetDatabase.Refresh();
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogFormat("<color=#ff0000>convert <color=#ffff00>all cs file</color> failed !!</color>");
                        UnityEngine.Debug.LogError(e.Message);
                    }
                }
            }
            if (GUILayout.Button("转表-Asset-ILFastMode", GUILayout.Width(152)))
            {
                for (int i = 0; i < fileNames.Count; ++i)
                {
                    if (fileNames[i].bSelected)
                    {
                        var dir = Path.GetFullPath(fileNames[i].path);
                        if (ExcelConfig.XLSX_NEED_ADAPT_OLD)
                        {
                            dir = Path.GetFullPath(Application.dataPath + $"/../../ProtoGen/UpgradeTable/{Path.GetFileName(fileNames[i].path)}");
                        }
                        var excelUnit = new ExcelUnit(dir);
                        excelUnit.Init(Application.dataPath, ExcelHelper.ConvertMode.CM_IL_FAST_MODE, mModelList.Contains(fileNames[i].name));
                        excelUnit.LoadProtoBase();
                        excelUnit.generateAsset(fileNames[i].path);
                        excelUnit.Close();
                    }
                }

                //AssetDatabase.Refresh();
            }
            EditorGUILayout.EndHorizontal();
        }

        bool IgnoreUpgraded = false;
        protected void OnGUI()
		{
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("过滤器",GUILayout.Width(60));
			filter = EditorGUILayout.TextField(filter, GUILayout.Width(100)).ToLower();
			if(ExcelConfig.XLSX_NEED_ADAPT_OLD)
			{
                EditorGUILayout.LabelField("过滤已经升级", GUILayout.Width(80));
                IgnoreUpgraded = EditorGUILayout.Toggle(IgnoreUpgraded, GUILayout.Width(60));
            }
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			if(GUILayout.Button("刷新",GUILayout.Width(100)))
			{
				_ListAllExcelFiles ();
			}
			if (GUILayout.Button ("全选", GUILayout.Width (100))) 
			{
				for (int i = 0; i < fileNames.Count; ++i) 
				{
					fileNames [i].bSelected = true;
				}
			}
			if (GUILayout.Button ("反选", GUILayout.Width (100))) 
			{
				for (int i = 0; i < fileNames.Count; ++i) 
				{
					fileNames [i].bSelected = !fileNames[i].bSelected;
				}
			}
			EditorGUILayout.EndHorizontal ();

            OnConvertILFastMode();
            OnConvertP3GUI();
            //OnProto2GUI();

            //excel-table-name
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
			EditorGUILayout.BeginVertical ();
			var defColor = GUI.color;
			for (int i = 0; i < fileNames.Count; ++i) 
			{
				if (!fileNames [i].lowerName.Contains(filter)) 
				{
					continue;
				}

                EditorGUILayout.BeginHorizontal();
                if (i % 2 == 0)
                {
                    GUI.color = Color.yellow;
                }
                else
                {
                    GUI.color = Color.cyan;
                }
                EditorGUILayout.LabelField(fileNames[i].name);
                fileNames[i].bSelected = EditorGUILayout.Toggle(fileNames[i].bSelected);

				if(GUILayout.Button("前往>>"))
				{
					System.Diagnostics.Process.Start("Explorer", "/select,"+ fileNames[i].path);
				}

                EditorGUILayout.EndHorizontal();
            }
			GUI.color = defColor;
			EditorGUILayout.EndVertical ();
			EditorGUILayout.EndScrollView ();
		}
	}
}