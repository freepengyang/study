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
    unsafe class CSModelModule_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSModelModule);

            field = type.GetField("Top", flag);
            app.RegisterCLRFieldGetter(field, get_Top_0);
            app.RegisterCLRFieldSetter(field, set_Top_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Top_0, AssignFromStack_Top_0);


        }



        static object get_Top_0(ref object o)
        {
            return ((global::CSModelModule)o).Top;
        }

        static StackObject* CopyToStack_Top_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSModelModule)o).Top;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Top_0(ref object o, object v)
        {
            ((global::CSModelModule)o).Top = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_Top_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @Top = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSModelModule)o).Top = @Top;
            return ptr_of_this_method;
        }



    }
}
