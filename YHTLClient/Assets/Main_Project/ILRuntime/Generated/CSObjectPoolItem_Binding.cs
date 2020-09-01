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
    unsafe class CSObjectPoolItem_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSObjectPoolItem);

            field = type.GetField("objParam", flag);
            app.RegisterCLRFieldGetter(field, get_objParam_0);
            app.RegisterCLRFieldSetter(field, set_objParam_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_objParam_0, AssignFromStack_objParam_0);
            field = type.GetField("go", flag);
            app.RegisterCLRFieldGetter(field, get_go_1);
            app.RegisterCLRFieldSetter(field, set_go_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_go_1, AssignFromStack_go_1);


        }



        static object get_objParam_0(ref object o)
        {
            return ((global::CSObjectPoolItem)o).objParam;
        }

        static StackObject* CopyToStack_objParam_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSObjectPoolItem)o).objParam;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static void set_objParam_0(ref object o, object v)
        {
            ((global::CSObjectPoolItem)o).objParam = (System.Object)v;
        }

        static StackObject* AssignFromStack_objParam_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Object @objParam = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSObjectPoolItem)o).objParam = @objParam;
            return ptr_of_this_method;
        }

        static object get_go_1(ref object o)
        {
            return ((global::CSObjectPoolItem)o).go;
        }

        static StackObject* CopyToStack_go_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSObjectPoolItem)o).go;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_go_1(ref object o, object v)
        {
            ((global::CSObjectPoolItem)o).go = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_go_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSObjectPoolItem)o).go = @go;
            return ptr_of_this_method;
        }



    }
}
