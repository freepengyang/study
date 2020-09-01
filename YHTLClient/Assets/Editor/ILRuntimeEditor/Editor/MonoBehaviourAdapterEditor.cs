using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.Utils;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Runtime.Enviorment;

[CustomEditor(typeof(MonoBehaviourAdapter.Adaptor), true)]
public class MonoBehaviourAdapterEditor : UnityEditor.UI.GraphicEditor
{
    public override void OnInspectorGUI()
    {
        return;

        // serializedObject.Update();
        // MonoBehaviourAdapter.Adaptor clr = target as MonoBehaviourAdapter.Adaptor;
        // var instance = clr.ILInstance;
        // if (instance != null)
        // {
        //     EditorGUILayout.LabelField("Script", clr.ILInstance.Type.FullName);
        //     int tempLength = instance.Type.FieldTypes.Length;
        //     foreach (var i in instance.Type.FieldMapping)
        //     {
        //         if (i.Value >= tempLength)
        //             continue;
        //         //这里是取的所有字段，没有处理不是public的
        //
        //         if (instance.Type.FieldTypes.GetValue(i.Value) == null)
        //         {
        //             continue;
        //         }
        //
        //         var name = i.Key;
        //         var type = instance.Type.FieldTypes[i.Value];
        //         var cType = type.TypeForCLR;
        //         if (cType.IsPrimitive)//如果是基础类型
        //         {
        //             if (cType.IsPublic)
        //             {
        //                 if (cType == typeof(float))
        //                 {
        //                     instance[i.Value] = EditorGUILayout.FloatField(name, (float)instance[i.Value]);
        //                 }
        //                 else if (cType == typeof(int))
        //                 {
        //                     instance[i.Value] = EditorGUILayout.IntField(name, (int)instance[i.Value]);
        //                 }
        //                 else if (cType == typeof(bool))
        //                 {
        //                     //instance[i.Value] = EditorGUILayout.Toggle(name, (bool)instance[i.Value]);
        //                 }
        //                 else
        //                 {
        //                     FNDebug.Log("字段显示区域还没适配：" + name + " " + cType);
        //                 }
        //             }
        //
        //             /*
        //             if (cType == typeof(float))
        //             {
        //                 instance[i.Value] = EditorGUILayout.FloatField(name, (float)instance[i.Value]);
        //             }
        //             else
        //             {
        //                 throw new System.NotImplementedException();//剩下的大家自己补吧
        //             }
        //             */
        //         }
        //         else
        //         {
        //             object obj = instance[i.Value];
        //             if (typeof(UnityEngine.Object).IsAssignableFrom(cType))
        //             {
        //                 //处理Unity类型
        //                 var res = EditorGUILayout.ObjectField(name, obj as UnityEngine.Object, cType, true);
        //                 instance[i.Value] = res;
        //             }
        //             else
        //             {
        //                 //其他类型现在没法处理
        //                 if (obj != null)
        //                 {
        //                     EditorGUILayout.LabelField(name, obj.ToString());
        //                 }
        //                 else
        //                 {
        //                     EditorGUILayout.LabelField(name, "(null)");
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}
