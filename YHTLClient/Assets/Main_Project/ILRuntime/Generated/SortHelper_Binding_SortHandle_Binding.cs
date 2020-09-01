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
    unsafe class SortHelper_Binding_SortHandle_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::SortHelper.SortHandle);

            field = type.GetField("handle", flag);
            app.RegisterCLRFieldGetter(field, get_handle_0);
            app.RegisterCLRFieldSetter(field, set_handle_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_handle_0, AssignFromStack_handle_0);
            field = type.GetField("longValue", flag);
            app.RegisterCLRFieldGetter(field, get_longValue_1);
            app.RegisterCLRFieldSetter(field, set_longValue_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_longValue_1, AssignFromStack_longValue_1);
            field = type.GetField("intValue", flag);
            app.RegisterCLRFieldGetter(field, get_intValue_2);
            app.RegisterCLRFieldSetter(field, set_intValue_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_intValue_2, AssignFromStack_intValue_2);


        }



        static object get_handle_0(ref object o)
        {
            return ((global::SortHelper.SortHandle)o).handle;
        }

        static StackObject* CopyToStack_handle_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::SortHelper.SortHandle)o).handle;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static void set_handle_0(ref object o, object v)
        {
            ((global::SortHelper.SortHandle)o).handle = (System.Object)v;
        }

        static StackObject* AssignFromStack_handle_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Object @handle = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::SortHelper.SortHandle)o).handle = @handle;
            return ptr_of_this_method;
        }

        static object get_longValue_1(ref object o)
        {
            return ((global::SortHelper.SortHandle)o).longValue;
        }

        static StackObject* CopyToStack_longValue_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::SortHelper.SortHandle)o).longValue;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_longValue_1(ref object o, object v)
        {
            ((global::SortHelper.SortHandle)o).longValue = (System.Int64[])v;
        }

        static StackObject* AssignFromStack_longValue_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int64[] @longValue = (System.Int64[])typeof(System.Int64[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::SortHelper.SortHandle)o).longValue = @longValue;
            return ptr_of_this_method;
        }

        static object get_intValue_2(ref object o)
        {
            return ((global::SortHelper.SortHandle)o).intValue;
        }

        static StackObject* CopyToStack_intValue_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::SortHelper.SortHandle)o).intValue;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_intValue_2(ref object o, object v)
        {
            ((global::SortHelper.SortHandle)o).intValue = (System.Int32[])v;
        }

        static StackObject* AssignFromStack_intValue_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32[] @intValue = (System.Int32[])typeof(System.Int32[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::SortHelper.SortHandle)o).intValue = @intValue;
            return ptr_of_this_method;
        }



    }
}
