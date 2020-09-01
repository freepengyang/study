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
    unsafe class CSPreDownLoadManger_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSPreDownLoadManger);

            field = type.GetField("onDownloadProgress", flag);
            app.RegisterCLRFieldGetter(field, get_onDownloadProgress_0);
            app.RegisterCLRFieldSetter(field, set_onDownloadProgress_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDownloadProgress_0, AssignFromStack_onDownloadProgress_0);
            field = type.GetField("onDownloadError", flag);
            app.RegisterCLRFieldGetter(field, get_onDownloadError_1);
            app.RegisterCLRFieldSetter(field, set_onDownloadError_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDownloadError_1, AssignFromStack_onDownloadError_1);


        }



        static object get_onDownloadProgress_0(ref object o)
        {
            return ((global::CSPreDownLoadManger)o).onDownloadProgress;
        }

        static StackObject* CopyToStack_onDownloadProgress_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSPreDownLoadManger)o).onDownloadProgress;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDownloadProgress_0(ref object o, object v)
        {
            ((global::CSPreDownLoadManger)o).onDownloadProgress = (System.Action<System.Int32, System.Int32>)v;
        }

        static StackObject* AssignFromStack_onDownloadProgress_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Int32, System.Int32> @onDownloadProgress = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSPreDownLoadManger)o).onDownloadProgress = @onDownloadProgress;
            return ptr_of_this_method;
        }

        static object get_onDownloadError_1(ref object o)
        {
            return ((global::CSPreDownLoadManger)o).onDownloadError;
        }

        static StackObject* CopyToStack_onDownloadError_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSPreDownLoadManger)o).onDownloadError;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDownloadError_1(ref object o, object v)
        {
            ((global::CSPreDownLoadManger)o).onDownloadError = (System.Action<System.Boolean>)v;
        }

        static StackObject* AssignFromStack_onDownloadError_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Boolean> @onDownloadError = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSPreDownLoadManger)o).onDownloadError = @onDownloadError;
            return ptr_of_this_method;
        }



    }
}
