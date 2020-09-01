using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace FBPkgGen
{
    public class BuildPackage
    {
        const string OUTPUT_FILE = "zt.spkg";
        const string RES_FILE_NAME = "ResListInApk.txt";
        const string Err_1 = "资源目录不能为null!";
        const string Err_2 = "资源目录不存在！";
        const string key = "flybrid";

        static readonly List<string> dst_keys = new List<string>();

        private static readonly string RESOURCEFILEDIR =
            Path.GetDirectoryName(Application.dataPath + "/../../Normal/zt_android/");
        
        private static readonly string OUT_RESOURCE_FILE =
            Application.dataPath + "/../../Normal/Resource/" + OUTPUT_FILE;

        private static readonly string RES_FILE =
            Application.dataPath + "/../../Normal/resourceTool/" + RES_FILE_NAME;
        
        
        //[MenuItem("Tools/Build spkg")]
        public static void BuildSPKG()
        {
            try
            {
                dst_keys.Clear();
                var lines = File.ReadAllLines(RES_FILE);

                string res_path = RESOURCEFILEDIR;

                if (string.IsNullOrEmpty(res_path))
                {
                    UnityEngine.Debug.LogError(Err_1);
                    return;
                }

                if (!Directory.Exists(res_path))
                {
                    UnityEngine.Debug.LogError(Err_2);
                    return;
                }

                if (File.Exists(OUT_RESOURCE_FILE))
                {
                    File.Delete(OUT_RESOURCE_FILE);
                }

                int err_count = 0;

                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    string[] res = line.Split('#');
                    if (res.Length == 0) return;
                    string file = res[0];

                    var path = Path.Combine(res_path, file);

                    if (!File.Exists(path))
                    {
                        ++err_count;
                        UnityEngine.Debug.LogError(string.Format("文件不存在: {0}\n", file));
                        continue;
                    }

                    dst_keys.Add(file);
                }

                if (err_count > 0)
                {
                    UnityEngine.Debug.LogError("存在无效文件");
                    //return;
                }

                SHPackage package = new SHPackage();

                var ms = package.Save(res_path, dst_keys);

                File.WriteAllBytes(OUT_RESOURCE_FILE, ms.ToArray());

                UnityEngine.Debug.Log("DONE ");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError(ex.Message);
            }
        }

    }
}