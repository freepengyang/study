using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;
namespace ExtendEditor
{
    public class CSGetAllStaticVar : EditorWindowBase<CSGetAllStaticVar>
    {
        public class Data
        {
            public class SubData
            {
                public bool isDebug = false;
                public FieldInfo info;
                public object value;
            }
            public Type type;
            public List<SubData> subDataList = new List<SubData>();
        }
        public List<Type> typeList = new List<Type>();
        public string[] typeNames = null;
        public int mCurDealIndex = -1;
        int mCurSelectIndex = 0;
        private Dictionary<Type, Data> dic = new Dictionary<Type, Data>();
        Vector2 mScrollPos = Vector2.zero;
        [MenuItem("Tools/Miscellaneous/获得所有静态变量")]
        public static void CSGetAllStaticVarProc()
        {
            EditorWindow win = GetWindow(typeof(CSGetAllStaticVar));
            win.Show();
        }

        public override void OnGUI()
        {
            GetCurType();
            mCurSelectIndex = EditorGUILayout.Popup("选择类型", mCurSelectIndex, typeNames);
            if (GUILayout.Button("Deal"))
            {
                dic.Clear();
                mCurDealIndex = 0;
                //FindStaticVar(typeof(CSScene));
            }
            if (GUILayout.Button("UnloadUnusedAssets"))
            {
                EditorUtility.UnloadUnusedAssetsImmediate();
                System.GC.Collect();
            }

            if (mCurDealIndex != -1 && mCurDealIndex < typeList.Count)
            {
                int dealNum = 0;
                while (true)
                {
                    FindStaticVar(typeList[mCurDealIndex]);
                    mCurDealIndex++;
                    dealNum++;
                    if (mCurDealIndex >= typeList.Count)
                    {
                        mCurDealIndex = -1;
                        break;
                    }
                    if (dealNum >= 100)
                    {
                        break;
                    }
                }
            }

            
            DrawStaticType();
        }

        void GetCurType()
        {
            if (typeList.Count == 0)
            {
                typeList = EditorUtil.GetCustomSciptAllTypes();
                typeNames = new string[typeList.Count];
                for (int i = 0; i < typeList.Count; i++)
                {
                    typeNames[i] = typeList[i].ToString().Replace("UnityEngine.", "");
                }
            }
        }

        void FindStaticVar(Type type)
        {
            if (type.IsEnum) return;

            FieldInfo[] infos = type.GetFields(BindingFlags.Static | BindingFlags.Public|BindingFlags.NonPublic);
            if (infos != null && infos.Length != 0)
            {
                
                for (int i = 0; i < infos.Length; i++)
                {
                    FieldInfo info = infos[i];
                    Type t = info.FieldType;
                    if(!IsTypeValid(t))continue;
                    if(!dic.ContainsKey(type))
                        dic.Add(type, new Data());
                    Data.SubData subData = new Data.SubData();
                    subData.info = info;
                    try
                    {
                        subData.value = info.GetValue(null);
                    }
                    catch (System.Exception ex)
                    {
                        subData.value = null;
                    }

                        
                    dic[type].type = type;
                    dic[type].subDataList.Add(subData);
                }
            }
        }

        bool IsTypeValid(Type type)
        {
            if(type.IsPrimitive)return false;
            if (type == typeof(string)) return false;//string类型，自定义的struct，class不是原生类型,struct,class中有可能包含资源属性
            if (type.IsValueType) return false;

            if (type.IsArray)
            {
                Type arrayType = type.GetElementType();
                return IsTypeValid(arrayType);
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(List<>) ||
                    genericType == typeof(BetterList<>) ||
                    genericType == typeof(System.Predicate<>)||
                    genericType == typeof(System.Comparison<>))
                {
                    Type itemType = type.GetGenericArguments()[0];
                    return IsTypeValid(itemType);
                }
                else if (genericType == typeof(Dictionary<,>))
                {
                    Type itemType0 = type.GetGenericArguments()[0];
                    bool isValid = IsTypeValid(itemType0);
                    if (isValid) return true;
                    Type itemType1 = type.GetGenericArguments()[1];
                    return IsTypeValid(itemType1);
                }
                else
                {
                    //Debug.LogError(genericType.ToString());
                }
            }
            return true;
        }

        void DrawStaticType()
        {
            if (mCurDealIndex != -1) return;
            mScrollPos = EditorGUILayout.BeginScrollView(mScrollPos);
            int index = 0;
            foreach (var kvp in dic)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField(index + " " + kvp.Key.ToString());
                bool isClick = GUILayout.Button("Clear All Reference");
                if (isClick)
                {
                    for (int i = 0; i < kvp.Value.subDataList.Count; i++)
                    {
                        Data.SubData subData = kvp.Value.subDataList[i];
                        FieldInfo info = kvp.Value.subDataList[i].info;
                        info.SetValue(null, null);
                        subData.value = info.GetValue(null);
                    }
                }
                EditorGUILayout.EndHorizontal();
                for (int i = 0; i < kvp.Value.subDataList.Count; i++)
                {
                    Data.SubData subData = kvp.Value.subDataList[i];
                    FieldInfo info = subData.info;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.TextField("       (" + i + ")" + info.Name + " ---" + (subData.value == null?"Null":subData.value.ToString()) + "-------" + info.FieldType.ToString());
                    bool isClick2 = GUILayout.Button("Clear Reference");
                    subData.isDebug = EditorGUILayout.Toggle("Debug", subData.isDebug);
                    if (isClick2)
                    {
                        info.SetValue(null, null);
                        subData.value = info.GetValue(null);
                    }
                    if (subData.isDebug)
                    {
                        FNDebug.Log("Debug");
                    }
                    EditorGUILayout.EndHorizontal();
                }
                index++;
            }
            EditorGUILayout.EndScrollView();
        }

        void DebugInfo(FieldInfo info)
        {

        }
    }
}

