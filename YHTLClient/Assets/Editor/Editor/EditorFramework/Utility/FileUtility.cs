using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Object = UnityEngine.Object;
namespace ExtendEditor
{
    public static class FileUtility
    {
        /// <summary>
        /// path can be a object,as xxx/x/x/x.jpg,relatively Application.dataPath
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void GetDeepAssetPaths(string path, List<string> list, string extension = "")
        {
            if (Directory.Exists(path))
            {
                if (path.Contains(@".svn")) return;
                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] files = new FileInfo[0];
                if (string.IsNullOrEmpty(extension))
                    files = dir.GetFiles();
                else
                    files = dir.GetFiles("*" + extension);
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = files[i];
                    if (file.Extension == ".meta") continue;
                    string filePath = GetAssetPath(file.FullName);
                    //Debug.LogError(filePath);
                    if (!list.Contains(filePath))
                    list.Add(filePath);
                }
                DirectoryInfo[] dirChild = dir.GetDirectories();
                for (int i = 0; i < dirChild.Length; i++)
                {
                    GetDeepAssetPaths(dirChild[i].FullName, list, extension);
                }
            }

            if (File.Exists(path))
            {
                string filePath = GetAssetPath(path);
                if(!list.Contains(filePath))
                list.Add(filePath);
            }
        }

        /// <summary>
        /// get all directories by the path,relatively Application.dataPath
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dirList"></param>
        /// <param name="extension"></param>
        public static void GetDeepAssetDirs(string path, List<string> dirList)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                string dirPathRelatively = GetAssetPath(path);
                if (!dirList.Contains(dirPathRelatively))
                    dirList.Add(dirPathRelatively);
                DirectoryInfo[] dirChild = dir.GetDirectories();
                for (int i = 0; i < dirChild.Length; i++)
                {
                    GetDeepAssetDirs(dirChild[i].FullName, dirList);
                }
            }
            if (File.Exists(path))
            {
                GetDeepAssetDirs(path + "/../", dirList);
            }
        }

        public static void GetSelectAssetPaths(List<string> list)
        {
            Object[] mSelectObjs = FileUtility.GetFiltered();
            for (int i = 0; i < mSelectObjs.Length; i++)
            {
                string path = AssetDatabase.GetAssetPath(mSelectObjs[i]);
                UnityEngine.Debug.Log(path);
                list.Add(path);
            }
        }


        public static bool IsFileExist(string filePath, bool isIgnoreExtension = false)
        {
            if (!isIgnoreExtension)
            {
                return File.Exists(filePath);
            }
            string dirPath = filePath + "/../";
            int starIndex = filePath.LastIndexOf("/")+1;
            int length = filePath.LastIndexOf(".") - starIndex;
            string fileName = filePath.Substring(starIndex, length);
            if (Directory.Exists(dirPath))
            {
                DirectoryInfo info = new DirectoryInfo(dirPath);
                foreach (FileInfo fileInfo in info.GetFiles())
                {
                    if (fileInfo.Extension.Contains("meta")) continue;
                    string fName = fileInfo.Name.Substring(0,fileInfo.Name.LastIndexOf("."));
                    if (fName==fileName)
                    {
                        //Debug.Log("fileInfo = "+fileInfo.FullName);
                        return true;
                    }
                    //Debug.Log(fileInfo.Name);
                    //if(fileInfo.Name)
                }
            }
            return false;
        }
        /// <summary>
        /// Application.dataPath
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static string GetAssetPath(string fileFullPath)
        {
            //Debug.LogError(fileFullPath);

            return "Assets" + fileFullPath.Replace("\\", "/").Replace(Application.dataPath, "");
        }

        public static string GetAssetFullPath(string assetPath)
        {
            return Application.dataPath.Replace("Assets","") + assetPath;
        }

        public static Object GetObject(string filePath)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
            return obj;
        }

        /// <summary>
        /// cur select object's directory
        /// </summary>
        /// <returns></returns>
        public static string GetSelectDirectory()
        {
            string path = "Assets";
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }
                break;
            }
            return path;
        }

        public static string GetDirectory(UnityEngine.Object obj)
        {
            string path = "Assets";
            path = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }
            return path;
        }

        public static string GetDirectory(string path)
        {
            int lastIndex = path.LastIndexOf("/");
            if (lastIndex == -1) return "";

            path = path.Substring(0, lastIndex);
            return path;
        }

        public static string GetFileName(string path)
        {
            string dir = GetDirectory(path);
            path = path.Replace(dir+"/", "");
            path = path.Substring(0, path.LastIndexOf("."));
            return path;
        }

        public static string GetFileWithoutExtension(string path)
        {
            string dir = GetDirectory(path);
            path = path.Substring(0, path.LastIndexOf("."));
            return path;
        }

        public static void Rename(string path, string newPath)
        {
            if (File.Exists(path))
            {
                File.Move(path, newPath);
            }
            else
            {
                UnityEngine.Debug.LogError(path + " is Not exist");
            }
        }

        /// <summary>
        /// File + Directory(not deep asset)
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static Object[] GetFiltered(SelectionMode mode = SelectionMode.Assets)
        {
            return Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        }

        public static bool FileIsChange(string path,string md5Path)
        {
            string saveContent = ReadToEnd(md5Path);
            UnityEngine.Debug.Log("saveContent = " + saveContent);
            string Md5 = MD5Utility.GetMD5HashFromFile(path);
            UnityEngine.Debug.Log("Md5 = " + Md5);
            return saveContent != Md5;
        }

        public static void SaveMD5ToFile(string path,string md5Path)
        {
            string Md5 = MD5Utility.GetMD5HashFromFile(path);
            Write(md5Path, Md5, false);
        }

        public static void DetectCreateDirectory(string path)
        {
            int index = path.LastIndexOf(".");
            if (index == -1||path.Substring(index).Contains("/"))
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            else
            {
                if (!Directory.Exists(path + "/../"))
                    Directory.CreateDirectory(path + "/../");
            }
        }

        public static string ReadToEnd(string path)
        {
            string content = "";
            if (!File.Exists(path)) return content;
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate,FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fileStream, System.Text.Encoding.UTF8))
                {
                    content = sr.ReadToEnd();
                }
            }
            return content;
        }

        public static void Write(string path, string content, bool append = true)
        {
            string old = ReadToEnd(path);
            DetectCreateDirectory(path);
            using (StreamWriter sw = new StreamWriter(path, append, System.Text.Encoding.UTF8))
            {
                sw.Write(content);
            }
        }

        public static void UseCommandUploadFileToSVN(string dir)
        {
            // 路径不存在直接返回
            if (!Directory.Exists(dir))
            {
                UnityEngine.Debug.LogError("路径不存在直接返回 = " + dir);
                return;
            }
            Process pp = Process.Start("TortoiseProc.exe", @"/command:commit /path:" + dir + " /closeonend:1");
            pp.WaitForExit();
        }

        /// <summary>
        /// 相对路径
        /// </summary>
        /// <param name="dirs"></param>
        public static void CommitToSVN(List<string> dirs)
        {
            ProcSVNCmd(GetPath(dirs), "commit");
        }

        public static void LogSVN(List<string> dirs)
        {
            ProcSVNCmd(GetPath(dirs), "log");
        }


        public static void UpdateSVN(List<string> dirs,string attachPath = "")
        {
            if (string.IsNullOrEmpty(attachPath))
            {
                ProcSVNCmd(GetPath(dirs), "update");
            }
            else
            {
                ProcSVNCmd(GetPath(dirs)+"*"+attachPath, "update");
            }
        }

        public static void RevertSVN(List<string> dirs)
        {
            ProcSVNCmd(GetPath(dirs), "revert");
        }

        public static void ResolveSVN(List<string> dirs)
        {
            ProcSVNCmd(GetPath(dirs), "resolve");
        }


        public static void BlameSVN(List<string> dirs)
        {
            ProcSVNCmd(GetPath(dirs), "blame");
        }

        static string GetPath(List<string> dirs)
        {
            string path = "";
            string temp = Application.dataPath.Replace("Assets","");
            for (int i = 0; i < dirs.Count; i++)
            {
                if (i == 0)
                {
                    path += temp + dirs[i];
                }
                else
                {
                    path += "*" + temp + dirs[i];
                }
            }
            return path;
        }

        public static void UpdateProtoAndTable()
        {
            string protoPath = Application.dataPath+"/../../proto";
            string tablePath = Application.dataPath+"/../../table/workbook";
            string path = protoPath+"*"+tablePath;
            ProcSVNCmd(path, "update");
        }

        public static void CommitProtoAndTable()
        {
            string protoPath = Application.dataPath + "/../../proto";
            string tablePath = Application.dataPath + "/../../table/workbook";
            string path = protoPath + "*" + tablePath;
            ProcSVNCmd(path, "commit");
        }

        static void ProcSVNCmd(string path, string cmd)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Process pp = Process.Start("TortoiseProc.exe", @"/command:" + cmd + " /path:" + path + " /closeonend:0");
                pp.WaitForExit();
            }
            else
            {
                UnityEngine.Debug.LogError("路径不对");
            }
        }



        public static void CopyText(string s)
        {
            TextEditor te = new TextEditor();
            te.content = new GUIContent(s);
            te.OnFocus();
            te.Copy();
        }

        public static string PasteText()
        {
            TextEditor te = new TextEditor();
            te.OnFocus();
            te.Paste();
            UnityEngine.Debug.Log(te.content.text);
            return te.content.text.ToString();
        }

        public static void CopyFile()
        {
            UnityEngine.Object[] objs = GetFiltered();
            if (objs.Length > 0)
            {
                string path = AssetDatabase.GetAssetPath(objs[0]);
                CopyText(path);
                UnityEngine.Debug.Log("复制" + path);
            }
        }

        public static void PasteFile()
        {
            UnityEngine.Object[] objs = GetFiltered();
            if (objs.Length > 0)
            {
                string path = PasteText();
                path = Application.dataPath + "/../" + path;
                string destPath = AssetDatabase.GetAssetPath(objs[0]);
                destPath = Application.dataPath+"/../"+destPath;
                if (File.Exists(path)&&Directory.Exists(destPath))
                {
                    FileInfo fileInfo = new FileInfo(path);
                    //UnityEngine.Debug.Log(path+" "+destPath);
                    File.Copy(path, destPath + "/" + fileInfo.Name, true);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                }
                else if(!File.Exists(path))
                {
                    UnityEngine.Debug.Log("文件不存在" + path);
                }
                else
                {
                    UnityEngine.Debug.Log("目标目录不存在" + destPath);
                }
            }
        }

        public static void Open(string path)
        {
            System.Diagnostics.Process pp = System.Diagnostics.Process.Start(path);
        }

        public static void UpdateToClassLibrary(List<string> list)
        {
            string p = Application.dataPath + "/../../ChuanQi/ClassLibrary/ClassLibrary/ClassLibrary/";
            for (int i = 0; i < list.Count; i++)
            {
                string path = p + list[i];
                //Debug.LogError(path);
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(list[i],typeof(UnityEngine.Object));
                if (obj != null)
                {
                    FileUtility.DetectCreateDirectory(path);
                    string srcPath = Application.dataPath+"/../"+list[i];
                    //Debug.LogError(srcPath);
                    //if(!File.Exists())
                    File.Copy(srcPath, path, true);
                }
            }
        }

        public static void UpdateFromClassLibrary()
        {
            string p = Application.dataPath + "/../../ChuanQi/ClassLibrary/ClassLibrary/ClassLibrary/bin/Debug/ClassLibrary.dll";
            if (File.Exists(p))
            {
                string destPath = Application.dataPath+"/Plugins/ClassLibrary.dll";
                File.Copy(p, destPath, true);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }

        /// <summary>
        /// 相对Assets/
        /// </summary>
        /// <param name="path"></param>
        /// <param name="?"></param>
        /// <returns></returns>lastModityTime
        public static bool IsFileChange(string path, bool isDetectDependenice)
        {
            string txtPath = Application.dataPath.Replace("Assets", "") + path.Replace("Assets", "AssetsChangeDetect");
            txtPath = txtPath.Substring(0, txtPath.LastIndexOf(".")) + ".txt";
            string txt = ReadToEnd(txtPath);
            if (string.IsNullOrEmpty(txt)) return true;
            Dictionary<string, DateTime> dataDic = new Dictionary<string, DateTime>();
            string[] ts = txt.Split('\n', '\r');
            for (int i = 0; i < ts.Length; i++)
            {
                string t = ts[i];
                string[] data = t.Split('#');
                if (data.Length != 2) continue;
                string assetPath = data[0];
                string lastModityTime = data[1];
                FileInfo fileInfo = new FileInfo(Application.dataPath.Replace("Assets", "") + assetPath);
                DateTime lastDt = DateTime.Parse(lastModityTime);
                DateTime dt = fileInfo.LastWriteTime;

                if (assetPath == path)
                {
                    TimeSpan tts = dt - lastDt;
                    long delta = (long)tts.TotalSeconds;
                    if (delta >= 1) return true;
                }
                dataDic.Add(assetPath, lastDt);
            }

            if (isDetectDependenice)
            {
                List<UnityEngine.Object> list = new List<UnityEngine.Object>();
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
                UnityEngine.Object[] dependObjs = EditorUtility.CollectDependencies(new UnityEngine.Object[1] { obj });
                if (dataDic.Count != dependObjs.Length*2) return true;//meta文件
                for (int i = 0; i < dependObjs.Length; i++)
                {
                    UnityEngine.Object o = dependObjs[i];
                    string oPath = AssetDatabase.GetAssetPath(o);
                    if (!dataDic.ContainsKey(oPath)) return true;
                    DateTime lastDt = dataDic[oPath];
                    FileInfo fileInfo = new FileInfo(Application.dataPath.Replace("Assets", "") + oPath);
                    DateTime dt = fileInfo.LastWriteTime;
                    TimeSpan tts = dt - lastDt;
                    long delta = (long)tts.TotalSeconds;
                    if (delta >=1) return true;

                    string metaPath = oPath + ".meta";
                    if (!dataDic.ContainsKey(metaPath)) return true;
                    lastDt = dataDic[metaPath];
                    fileInfo = new FileInfo(Application.dataPath.Replace("Assets", "") + metaPath);
                    dt = fileInfo.LastWriteTime;
                    tts = dt - lastDt;
                    delta = (long)tts.TotalSeconds;
                    if (delta >= 1) return true;
                }
            }
            return false;
        }

        public static void WriteFileChange(string path)
        {
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
            UnityEngine.Object[] dependObjs = EditorUtility.CollectDependencies(new UnityEngine.Object[1] { obj });
            List<UnityEngine.Object> list = new List<UnityEngine.Object>();
            list.AddRange(dependObjs);
            string txt = "";
            for (int i = 0; i < list.Count; i++)
            {
                string p = AssetDatabase.GetAssetPath(list[0]);
                string fullP = Application.dataPath.Replace("Assets", "") + p;
                FileInfo f = new FileInfo(fullP);
                txt += p + "#" + f.LastWriteTime.ToString()+"\r\n";

                p = p+".meta";
                fullP = Application.dataPath.Replace("Assets", "") + p;
                f = new FileInfo(fullP);
                txt += p + "#" + f.LastWriteTime.ToString()+"\r\n";
            }
            string txtPath = Application.dataPath.Replace("Assets", "") + path.Replace("Assets", "AssetsChangeDetect");
            txtPath = txtPath.Substring(0, txtPath.LastIndexOf(".")) + ".txt";
            //UnityEngine.Debug.LogError(txtPath);
            Write(txtPath, txt, false);
        }

		public static Int64 GetFileSize(string path)
		{
			try
			{
				FileStream file = new FileStream(path, FileMode.Open);
				return file.Length;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(string.Format("GetFileSize error {0}", ex.Message));
			}
			
			return 0;
		}
    }
}