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
    unsafe class GDecoder_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::GDecoder);
            args = new Type[]{typeof(System.Byte[]), typeof(System.Byte[])};
            method = type.GetMethod("LoadTable", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, LoadTable_0);

            field = type.GetField("varIntValues", flag);
            app.RegisterCLRFieldGetter(field, get_varIntValues_0);
            app.RegisterCLRFieldSetter(field, set_varIntValues_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_varIntValues_0, AssignFromStack_varIntValues_0);
            field = type.GetField("varStringValues", flag);
            app.RegisterCLRFieldGetter(field, get_varStringValues_1);
            app.RegisterCLRFieldSetter(field, set_varStringValues_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_varStringValues_1, AssignFromStack_varStringValues_1);
            field = type.GetField("varLongValues", flag);
            app.RegisterCLRFieldGetter(field, get_varLongValues_2);
            app.RegisterCLRFieldSetter(field, set_varLongValues_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_varLongValues_2, AssignFromStack_varLongValues_2);


        }


        static StackObject* LoadTable_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Byte[] @rs = (System.Byte[])typeof(System.Byte[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Byte[] @cs = (System.Byte[])typeof(System.Byte[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::GDecoder.LoadTable(@cs, @rs);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_varIntValues_0(ref object o)
        {
            return global::GDecoder.varIntValues;
        }

        static StackObject* CopyToStack_varIntValues_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::GDecoder.varIntValues;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_varIntValues_0(ref object o, object v)
        {
            global::GDecoder.varIntValues = (System.Int32[])v;
        }

        static StackObject* AssignFromStack_varIntValues_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32[] @varIntValues = (System.Int32[])typeof(System.Int32[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::GDecoder.varIntValues = @varIntValues;
            return ptr_of_this_method;
        }

        static object get_varStringValues_1(ref object o)
        {
            return global::GDecoder.varStringValues;
        }

        static StackObject* CopyToStack_varStringValues_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::GDecoder.varStringValues;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_varStringValues_1(ref object o, object v)
        {
            global::GDecoder.varStringValues = (System.String[])v;
        }

        static StackObject* AssignFromStack_varStringValues_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String[] @varStringValues = (System.String[])typeof(System.String[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::GDecoder.varStringValues = @varStringValues;
            return ptr_of_this_method;
        }

        static object get_varLongValues_2(ref object o)
        {
            return global::GDecoder.varLongValues;
        }

        static StackObject* CopyToStack_varLongValues_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::GDecoder.varLongValues;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_varLongValues_2(ref object o, object v)
        {
            global::GDecoder.varLongValues = (System.Int64[])v;
        }

        static StackObject* AssignFromStack_varLongValues_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int64[] @varLongValues = (System.Int64[])typeof(System.Int64[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::GDecoder.varLongValues = @varLongValues;
            return ptr_of_this_method;
        }



    }
}
