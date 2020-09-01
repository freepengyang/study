using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
namespace ExtendEditor
{
    public static class ReflectionUtility
    {
        public static string GetStackTraceClassName(List<string> excludeClassNameList = null)
        {
            List<string> list = GetStackTraceClasssNameList(excludeClassNameList);
            string _fullName = "";
            for (int i = 0; i < list.Count; i++)
            {
                _fullName = _fullName + "->" + list[i];
            }
            return _fullName.TrimEnd('-', '>');
        }

        public static List<string> GetStackTraceClasssNameList(List<string> excludeClassNameList = null)
        {
            //当前堆栈信息
            List<string> list = new List<string>();
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame[] sfs = st.GetFrames();
            //过虑的方法名称,以下方法将不会出现在返回的方法调用列表中
            string _filterdName = "ResponseWrite,ResponseWriteError,";
            string _fullName = string.Empty, _methodName = string.Empty;
            for (int i = 0; i < sfs.Length; ++i)
            {
                //非用户代码,系统方法及后面的都是系统调用，不获取用户代码调用结束
                if (System.Diagnostics.StackFrame.OFFSET_UNKNOWN == sfs[i].GetILOffset()) break;
                _methodName = sfs[i].GetMethod().ReflectedType.FullName;//方法名称
                if (excludeClassNameList != null && excludeClassNameList.Contains(_methodName)) continue;
                if (_methodName == "ReflectionUtility") continue;
                //sfs[i].GetFileLineNumber();//没有PDB文件的情况下将始终返回0
                list.Add(_methodName);
            }
            st = null;
            sfs = null;
            _filterdName = _methodName = null;
            list.Reverse();
            return list;
        }

        /// <summary>
        /// 扩展 获取变量名称(字符串)
        ///         bool test_name = true; //变量类型可随意
        ///string tips = test_name.GetVarName(it => test_name);   打印"test_name";
        /// </summary>
        /// <param name="var_name"></param>
        /// <param name="exp"></param>
        /// <returns>return string</returns>
        public static string GetVarName<T>(this T var_name, System.Linq.Expressions.Expression<Func<T, T>> exp)
        {
            return ((System.Linq.Expressions.MemberExpression)exp.Body).Member.Name;
        }
    }
}
