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
    unsafe class GuideLife_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::GuideLife);
            args = new Type[]{};
            method = type.GetMethod("Remove", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Remove_0);

            field = type.GetField("onDeath", flag);
            app.RegisterCLRFieldGetter(field, get_onDeath_0);
            app.RegisterCLRFieldSetter(field, set_onDeath_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDeath_0, AssignFromStack_onDeath_0);
            field = type.GetField("onStart", flag);
            app.RegisterCLRFieldGetter(field, get_onStart_1);
            app.RegisterCLRFieldSetter(field, set_onStart_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onStart_1, AssignFromStack_onStart_1);


        }


        static StackObject* Remove_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::GuideLife instance_of_this_method = (global::GuideLife)typeof(global::GuideLife).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Remove();

            return __ret;
        }


        static object get_onDeath_0(ref object o)
        {
            return ((global::GuideLife)o).onDeath;
        }

        static StackObject* CopyToStack_onDeath_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::GuideLife)o).onDeath;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDeath_0(ref object o, object v)
        {
            ((global::GuideLife)o).onDeath = (System.Action<System.Boolean>)v;
        }

        static StackObject* AssignFromStack_onDeath_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Boolean> @onDeath = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::GuideLife)o).onDeath = @onDeath;
            return ptr_of_this_method;
        }

        static object get_onStart_1(ref object o)
        {
            return ((global::GuideLife)o).onStart;
        }

        static StackObject* CopyToStack_onStart_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::GuideLife)o).onStart;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onStart_1(ref object o, object v)
        {
            ((global::GuideLife)o).onStart = (System.Action)v;
        }

        static StackObject* AssignFromStack_onStart_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @onStart = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::GuideLife)o).onStart = @onStart;
            return ptr_of_this_method;
        }



    }
}
