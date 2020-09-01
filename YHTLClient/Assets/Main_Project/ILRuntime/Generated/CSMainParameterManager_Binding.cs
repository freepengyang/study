using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class CSMainParameterManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSMainParameterManager);

            field = type.GetField("tableMapInfo", flag);
            app.RegisterCLRFieldGetter(field, get_tableMapInfo_0);
            app.RegisterCLRFieldSetter(field, set_tableMapInfo_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_tableMapInfo_0, AssignFromStack_tableMapInfo_0);
            field = type.GetField("StartEnterSceneComplete", flag);
            app.RegisterCLRFieldGetter(field, get_StartEnterSceneComplete_1);
            app.RegisterCLRFieldSetter(field, set_StartEnterSceneComplete_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_StartEnterSceneComplete_1, AssignFromStack_StartEnterSceneComplete_1);
            field = type.GetField("LoadingComplete", flag);
            app.RegisterCLRFieldGetter(field, get_LoadingComplete_2);
            app.RegisterCLRFieldSetter(field, set_LoadingComplete_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_LoadingComplete_2, AssignFromStack_LoadingComplete_2);


        }



        static object get_tableMapInfo_0(ref object o)
        {
            return global::CSMainParameterManager.tableMapInfo;
        }

        static StackObject* CopyToStack_tableMapInfo_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSMainParameterManager.tableMapInfo;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_tableMapInfo_0(ref object o, object v)
        {
            global::CSMainParameterManager.tableMapInfo = (global::TableMapInfo)v;
        }

        static StackObject* AssignFromStack_tableMapInfo_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::TableMapInfo @tableMapInfo = (global::TableMapInfo)typeof(global::TableMapInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::CSMainParameterManager.tableMapInfo = @tableMapInfo;
            return ptr_of_this_method;
        }

        static object get_StartEnterSceneComplete_1(ref object o)
        {
            return global::CSMainParameterManager.StartEnterSceneComplete;
        }

        static StackObject* CopyToStack_StartEnterSceneComplete_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSMainParameterManager.StartEnterSceneComplete;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_StartEnterSceneComplete_1(ref object o, object v)
        {
            global::CSMainParameterManager.StartEnterSceneComplete = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_StartEnterSceneComplete_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @StartEnterSceneComplete = ptr_of_this_method->Value == 1;
            global::CSMainParameterManager.StartEnterSceneComplete = @StartEnterSceneComplete;
            return ptr_of_this_method;
        }

        static object get_LoadingComplete_2(ref object o)
        {
            return global::CSMainParameterManager.LoadingComplete;
        }

        static StackObject* CopyToStack_LoadingComplete_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSMainParameterManager.LoadingComplete;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_LoadingComplete_2(ref object o, object v)
        {
            global::CSMainParameterManager.LoadingComplete = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_LoadingComplete_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @LoadingComplete = ptr_of_this_method->Value == 1;
            global::CSMainParameterManager.LoadingComplete = @LoadingComplete;
            return ptr_of_this_method;
        }



    }
}
