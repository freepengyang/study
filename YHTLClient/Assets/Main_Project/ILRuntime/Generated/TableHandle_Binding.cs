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
    unsafe class TableHandle_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::TableHandle);

            field = type.GetField("Value", flag);
            app.RegisterCLRFieldGetter(field, get_Value_0);
            app.RegisterCLRFieldSetter(field, set_Value_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Value_0, AssignFromStack_Value_0);
            field = type.GetField("key", flag);
            app.RegisterCLRFieldGetter(field, get_key_1);
            app.RegisterCLRFieldSetter(field, set_key_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_key_1, AssignFromStack_key_1);
            field = type.GetField("intOffset", flag);
            app.RegisterCLRFieldGetter(field, get_intOffset_2);
            app.RegisterCLRFieldSetter(field, set_intOffset_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_intOffset_2, AssignFromStack_intOffset_2);
            field = type.GetField("stringOffset", flag);
            app.RegisterCLRFieldGetter(field, get_stringOffset_3);
            app.RegisterCLRFieldSetter(field, set_stringOffset_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_stringOffset_3, AssignFromStack_stringOffset_3);
            field = type.GetField("data", flag);
            app.RegisterCLRFieldGetter(field, get_data_4);
            app.RegisterCLRFieldSetter(field, set_data_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_data_4, AssignFromStack_data_4);

            app.RegisterCLRCreateArrayInstance(type, s => new global::TableHandle[s]);


        }



        static object get_Value_0(ref object o)
        {
            return ((global::TableHandle)o).Value;
        }

        static StackObject* CopyToStack_Value_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TableHandle)o).Value;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static void set_Value_0(ref object o, object v)
        {
            ((global::TableHandle)o).Value = (System.Object)v;
        }

        static StackObject* AssignFromStack_Value_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Object @Value = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::TableHandle)o).Value = @Value;
            return ptr_of_this_method;
        }

        static object get_key_1(ref object o)
        {
            return ((global::TableHandle)o).key;
        }

        static StackObject* CopyToStack_key_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TableHandle)o).key;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_key_1(ref object o, object v)
        {
            ((global::TableHandle)o).key = (System.Int32)v;
        }

        static StackObject* AssignFromStack_key_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @key = ptr_of_this_method->Value;
            ((global::TableHandle)o).key = @key;
            return ptr_of_this_method;
        }

        static object get_intOffset_2(ref object o)
        {
            return ((global::TableHandle)o).intOffset;
        }

        static StackObject* CopyToStack_intOffset_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TableHandle)o).intOffset;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_intOffset_2(ref object o, object v)
        {
            ((global::TableHandle)o).intOffset = (System.Int32)v;
        }

        static StackObject* AssignFromStack_intOffset_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @intOffset = ptr_of_this_method->Value;
            ((global::TableHandle)o).intOffset = @intOffset;
            return ptr_of_this_method;
        }

        static object get_stringOffset_3(ref object o)
        {
            return ((global::TableHandle)o).stringOffset;
        }

        static StackObject* CopyToStack_stringOffset_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TableHandle)o).stringOffset;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_stringOffset_3(ref object o, object v)
        {
            ((global::TableHandle)o).stringOffset = (System.Int32)v;
        }

        static StackObject* AssignFromStack_stringOffset_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @stringOffset = ptr_of_this_method->Value;
            ((global::TableHandle)o).stringOffset = @stringOffset;
            return ptr_of_this_method;
        }

        static object get_data_4(ref object o)
        {
            return ((global::TableHandle)o).data;
        }

        static StackObject* CopyToStack_data_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TableHandle)o).data;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_data_4(ref object o, object v)
        {
            ((global::TableHandle)o).data = (global::TableData)v;
        }

        static StackObject* AssignFromStack_data_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::TableData @data = (global::TableData)typeof(global::TableData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::TableHandle)o).data = @data;
            return ptr_of_this_method;
        }



    }
}
