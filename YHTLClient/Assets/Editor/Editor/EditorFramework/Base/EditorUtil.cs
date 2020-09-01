using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Reflection;
using Object = UnityEngine.Object;
namespace ExtendEditor
{
    public class EditorUtil
    {
        private static bool mIsDealingTexFormat = false;
        public static bool IsDealingTexFormat
        {
            get { return mIsDealingTexFormat; }
            set { mIsDealingTexFormat = value; }
        }

        public static void SetTexReadable(Texture2D tex, bool isReadable)
        {
            if (tex == null) return;
            TextureImporter importer = null;
            string texPath = "";
            texPath = AssetDatabase.GetAssetPath(tex);
            importer = TextureImporter.GetAtPath(texPath) as TextureImporter;
            if (importer != null && importer.isReadable != isReadable)
            {
                importer.isReadable = isReadable;
                EditorUtil.IsDealingTexFormat = isReadable;
                AssetDatabase.ImportAsset(texPath);
            }
            EditorUtil.IsDealingTexFormat = isReadable;
        }

        public static void PrintLogPath(GameObject go,string pre = "",string post = "")
        {
            string str = GetParentPath(go);
            FNDebug.LogError(pre+str+post);
        }

        public static string GetParentPath(GameObject go)
        {
            if (go == null) return "";
            List<string> list = new List<string>();
            Transform trans = go.transform;
            while (trans != null)
            {
                list.Add(trans.name);
                trans = trans.parent;
            }
            list.Reverse();
            string str = "";
            for (int i = 0; i < list.Count; i++)
            {
                str += list[i];
                if (i != list.Count - 1)
                {
                    str += "->";
                }
            }
            return str;
        }

        public static List<Object> GetDependent(Object obj)
        {
            List<Object> list = new List<Object>();
            Object[] dependObjs = EditorUtility.CollectDependencies(new Object[1] { obj });
            list.Add(obj);
            foreach (Object ob in dependObjs)
            {
                if (list.Contains(ob)) continue;
                list.Add(ob);
            }
            return list;
        }

        /// <summary>
        /// 单位是MB
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float GetTotalMemoryNum(Object obj)
        {
            List<Object> list = GetDependent(obj);
            float num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null) continue;
                num += UnityEngine.Profiling.Profiler.GetRuntimeMemorySize(list[i]);
            }
            num = num / 1024 / 1024;
            return num;
        }

        public static List<Type> GetExecutingAssemblyAllTypes(bool isSort = true)
        {
            List<Type> list = new List<Type>();
            list.AddRange(Assembly.GetExecutingAssembly().GetTypes());
            if (isSort)
            {
                list.Sort(CompareSort_AllTypes);
            }
            return list;
        }

        public static List<Type> GetCustomSciptAllTypes()
        {
            Assembly a = Assembly.GetAssembly(typeof(CSGame));
            return GetAssetmblyAllTypes(a.Location.Replace("\\","/"));
        }

        //E:\GameProject\Client\Branch\Client\Library\UnityAssemblies\UnityEngine.dll
        public static List<Type> GetAssetmblyAllTypes(string fileName,bool isSort = true)
        {
            Assembly assembly = Assembly.LoadFrom(fileName);
            Type[] types = assembly.GetTypes();
            List<Type> list = new List<Type>();
            list.AddRange(types);
            if (isSort)
            {
                list.Sort(CompareSort_AllTypes);
            }
            return list;
        }

        public static List<Type> GetUnityEngineAssemblyAllTypes(bool isSort = true)
        {
            string filaName = Application.dataPath+"/../Library/UnityAssemblies/UnityEngine.dll";
            return GetAssetmblyAllTypes(filaName,isSort);
        }

        static int CompareSort_AllTypes(Type f, Type s)
        {
            string fstr = f.ToString().ToLower();
            string sstr = s.ToString().ToLower();
            return string.Compare(fstr, sstr);
        }
    }

}
