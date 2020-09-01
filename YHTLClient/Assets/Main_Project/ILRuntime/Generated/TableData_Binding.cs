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
    unsafe class TableData_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::TableData);

            field = type.GetField("handles", flag);
            app.RegisterCLRFieldGetter(field, get_handles_0);
            app.RegisterCLRFieldSetter(field, set_handles_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_handles_0, AssignFromStack_handles_0);
            field = type.GetField("id2offset", flag);
            app.RegisterCLRFieldGetter(field, get_id2offset_1);
            app.RegisterCLRFieldSetter(field, set_id2offset_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_id2offset_1, AssignFromStack_id2offset_1);
            field = type.GetField("intValues", flag);
            app.RegisterCLRFieldGetter(field, get_intValues_2);
            app.RegisterCLRFieldSetter(field, set_intValues_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_intValues_2, AssignFromStack_intValues_2);
            field = type.GetField("stringValues", flag);
            app.RegisterCLRFieldGetter(field, get_stringValues_3);
            app.RegisterCLRFieldSetter(field, set_stringValues_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_stringValues_3, AssignFromStack_stringValues_3);


        }



        static object get_handles_0(ref object o)
        {
            return ((global::TableData)o).handles;
        }

        static StackObject* CopyToStack_handles_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TableData)o).handles;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_handles_0(ref object o, object v)
        {
            ((global::TableData)o).handles = (global::TableHandle[])v;
        }

        static StackObject* AssignFromStack_handles_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::TableHandle[] @handles = (global::TableHandle[])typeof(global::TableHandle[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::TableData)o).handles = @handles;
            return ptr_of_this_method;
        }

        static object get_id2offset_1(ref object o)
        {
            return ((global::TableData)o).id2offset;
        }

        static StackObject* CopyToStack_id2offset_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TableData)o).id2offset;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_id2offset_1(ref object o, object v)
        {
            ((global::TableData)o).id2offset = (System.Collections.Generic.Dictionary<System.Int32, global::TableHandle>)v;
        }

        static StackObject* AssignFromStack_id2offset_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.Dictionary<System.Int32, global::TableHandle> @id2offset = (System.Collections.Generic.Dictionary<System.Int32, global::TableHandle>)typeof(System.Collections.Generic.Dictionary<System.Int32, global::TableHandle>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::TableData)o).id2offset = @id2offset;
            return ptr_of_this_method;
        }

        static object get_intValues_2(ref object o)
        {
            return ((global::TableData)o).intValues;
        }

        static StackObject* CopyToStack_intValues_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TableData)o).intValues;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_intValues_2(ref object o, object v)
        {
            ((global::TableData)o).intValues = (System.Int32[])v;
        }

        static StackObject* AssignFromStack_intValues_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32[] @intValues = (System.Int32[])typeof(System.Int32[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::TableData)o).intValues = @intValues;
            return ptr_of_this_method;
        }

        static object get_stringValues_3(ref object o)
        {
            return ((global::TableData)o).stringValues;
        }

        static StackObject* CopyToStack_stringValues_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TableData)o).stringValues;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_stringValues_3(ref object o, object v)
        {
            ((global::TableData)o).stringValues = (System.String[])v;
        }

        static StackObject* AssignFromStack_stringValues_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String[] @stringValues = (System.String[])typeof(System.String[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::TableData)o).stringValues = @stringValues;
            return ptr_of_this_method;
        }



    }
}
