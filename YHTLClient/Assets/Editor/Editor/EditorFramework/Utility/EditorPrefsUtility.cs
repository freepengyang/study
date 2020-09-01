using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
namespace ExtendEditor
{
    public static class EditorPrefsUtility
    {
        public static string StackClassName
        {
            get
            {
                List<string> list = ReflectionUtility.GetStackTraceClasssNameList(new List<string>() { "EditorPrefsUtility" });
                return list[list.Count - 1];
            }
        }

        public static bool GetBool(string paramKey)
        {
            return EditorPrefs.GetBool(StackClassName + "=>" + paramKey);
        }

        public static float GetFloat(string paramKey, int defaultValue = 0)
        {
            return EditorPrefs.GetFloat(StackClassName + "=>" + paramKey, defaultValue);
        }


        public static int GetInt(string paramKey, int defaultValue = 0)
        {
            string key = StackClassName + "_" + paramKey;
            int value = EditorPrefs.GetInt(key);
            return value;
        }

        public static string GetString(string paramKey, string defaultValue = "")
        {
            return EditorPrefs.GetString(StackClassName + "=>" + paramKey, defaultValue);
        }


        public static void SetBool(string paramKey, bool value)
        {
            EditorPrefs.SetBool(StackClassName + "=>" + paramKey, value);
        }

        public static void SetFloat(string paramKey, float value = 0)
        {
            EditorPrefs.SetFloat(StackClassName + "=>" + paramKey, value);
        }

        public static void SetInt(string paramKey, int value = 0)
        {
            string key = StackClassName + "_" + paramKey;
            EditorPrefs.SetInt(key, value);
        }

        public static void SetString(string paramKey, string value = "")
        {
            EditorPrefs.SetString(StackClassName + "=>" + paramKey, value);
        }


    }
}