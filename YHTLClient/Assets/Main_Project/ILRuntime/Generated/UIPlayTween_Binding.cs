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
    unsafe class UIPlayTween_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIPlayTween);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("Play", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Play_0);

            field = type.GetField("resetOnPlay", flag);
            app.RegisterCLRFieldGetter(field, get_resetOnPlay_0);
            app.RegisterCLRFieldSetter(field, set_resetOnPlay_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_resetOnPlay_0, AssignFromStack_resetOnPlay_0);


        }


        static StackObject* Play_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @forward = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIPlayTween instance_of_this_method = (global::UIPlayTween)typeof(global::UIPlayTween).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Play(@forward);

            return __ret;
        }


        static object get_resetOnPlay_0(ref object o)
        {
            return ((global::UIPlayTween)o).resetOnPlay;
        }

        static StackObject* CopyToStack_resetOnPlay_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIPlayTween)o).resetOnPlay;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_resetOnPlay_0(ref object o, object v)
        {
            ((global::UIPlayTween)o).resetOnPlay = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_resetOnPlay_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @resetOnPlay = ptr_of_this_method->Value == 1;
            ((global::UIPlayTween)o).resetOnPlay = @resetOnPlay;
            return ptr_of_this_method;
        }



    }
}
