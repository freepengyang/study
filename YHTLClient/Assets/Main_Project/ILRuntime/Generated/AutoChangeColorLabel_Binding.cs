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
    unsafe class AutoChangeColorLabel_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::AutoChangeColorLabel);

            field = type.GetField("StartColor", flag);
            app.RegisterCLRFieldGetter(field, get_StartColor_0);
            app.RegisterCLRFieldSetter(field, set_StartColor_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_StartColor_0, AssignFromStack_StartColor_0);
            field = type.GetField("MiddleColor", flag);
            app.RegisterCLRFieldGetter(field, get_MiddleColor_1);
            app.RegisterCLRFieldSetter(field, set_MiddleColor_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_MiddleColor_1, AssignFromStack_MiddleColor_1);
            field = type.GetField("EndColor", flag);
            app.RegisterCLRFieldGetter(field, get_EndColor_2);
            app.RegisterCLRFieldSetter(field, set_EndColor_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_EndColor_2, AssignFromStack_EndColor_2);


        }



        static object get_StartColor_0(ref object o)
        {
            return ((global::AutoChangeColorLabel)o).StartColor;
        }

        static StackObject* CopyToStack_StartColor_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::AutoChangeColorLabel)o).StartColor;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_StartColor_0(ref object o, object v)
        {
            ((global::AutoChangeColorLabel)o).StartColor = (UnityEngine.Color)v;
        }

        static StackObject* AssignFromStack_StartColor_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Color @StartColor = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::AutoChangeColorLabel)o).StartColor = @StartColor;
            return ptr_of_this_method;
        }

        static object get_MiddleColor_1(ref object o)
        {
            return ((global::AutoChangeColorLabel)o).MiddleColor;
        }

        static StackObject* CopyToStack_MiddleColor_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::AutoChangeColorLabel)o).MiddleColor;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_MiddleColor_1(ref object o, object v)
        {
            ((global::AutoChangeColorLabel)o).MiddleColor = (UnityEngine.Color)v;
        }

        static StackObject* AssignFromStack_MiddleColor_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Color @MiddleColor = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::AutoChangeColorLabel)o).MiddleColor = @MiddleColor;
            return ptr_of_this_method;
        }

        static object get_EndColor_2(ref object o)
        {
            return ((global::AutoChangeColorLabel)o).EndColor;
        }

        static StackObject* CopyToStack_EndColor_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::AutoChangeColorLabel)o).EndColor;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_EndColor_2(ref object o, object v)
        {
            ((global::AutoChangeColorLabel)o).EndColor = (UnityEngine.Color)v;
        }

        static StackObject* AssignFromStack_EndColor_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Color @EndColor = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::AutoChangeColorLabel)o).EndColor = @EndColor;
            return ptr_of_this_method;
        }



    }
}
