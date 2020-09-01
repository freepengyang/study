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
    unsafe class BehaviourProvider_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::BehaviourProvider);
            args = new Type[]{typeof(global::FSMState)};
            method = type.GetMethod("InitializeFSM", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitializeFSM_0);
            args = new Type[]{};
            method = type.GetMethod("Reset", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Reset_1);

            field = type.GetField("onRunOverDoSmoethingStart", flag);
            app.RegisterCLRFieldGetter(field, get_onRunOverDoSmoethingStart_0);
            app.RegisterCLRFieldSetter(field, set_onRunOverDoSmoethingStart_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onRunOverDoSmoethingStart_0, AssignFromStack_onRunOverDoSmoethingStart_0);
            field = type.GetField("onRunOverDoSmoethingEnd", flag);
            app.RegisterCLRFieldGetter(field, get_onRunOverDoSmoethingEnd_1);
            app.RegisterCLRFieldSetter(field, set_onRunOverDoSmoethingEnd_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onRunOverDoSmoethingEnd_1, AssignFromStack_onRunOverDoSmoethingEnd_1);


        }


        static StackObject* InitializeFSM_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::FSMState @fsm = (global::FSMState)typeof(global::FSMState).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::BehaviourProvider instance_of_this_method = (global::BehaviourProvider)typeof(global::BehaviourProvider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.InitializeFSM(@fsm);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* Reset_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::BehaviourProvider instance_of_this_method = (global::BehaviourProvider)typeof(global::BehaviourProvider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Reset();

            return __ret;
        }


        static object get_onRunOverDoSmoethingStart_0(ref object o)
        {
            return ((global::BehaviourProvider)o).onRunOverDoSmoethingStart;
        }

        static StackObject* CopyToStack_onRunOverDoSmoethingStart_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::BehaviourProvider)o).onRunOverDoSmoethingStart;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onRunOverDoSmoethingStart_0(ref object o, object v)
        {
            ((global::BehaviourProvider)o).onRunOverDoSmoethingStart = (global::BehaviourProvider.OnRunOverDoSmoethingStart)v;
        }

        static StackObject* AssignFromStack_onRunOverDoSmoethingStart_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::BehaviourProvider.OnRunOverDoSmoethingStart @onRunOverDoSmoethingStart = (global::BehaviourProvider.OnRunOverDoSmoethingStart)typeof(global::BehaviourProvider.OnRunOverDoSmoethingStart).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::BehaviourProvider)o).onRunOverDoSmoethingStart = @onRunOverDoSmoethingStart;
            return ptr_of_this_method;
        }

        static object get_onRunOverDoSmoethingEnd_1(ref object o)
        {
            return ((global::BehaviourProvider)o).onRunOverDoSmoethingEnd;
        }

        static StackObject* CopyToStack_onRunOverDoSmoethingEnd_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::BehaviourProvider)o).onRunOverDoSmoethingEnd;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onRunOverDoSmoethingEnd_1(ref object o, object v)
        {
            ((global::BehaviourProvider)o).onRunOverDoSmoethingEnd = (global::BehaviourProvider.OnRunOverDoSmoethingEnd)v;
        }

        static StackObject* AssignFromStack_onRunOverDoSmoethingEnd_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::BehaviourProvider.OnRunOverDoSmoethingEnd @onRunOverDoSmoethingEnd = (global::BehaviourProvider.OnRunOverDoSmoethingEnd)typeof(global::BehaviourProvider.OnRunOverDoSmoethingEnd).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::BehaviourProvider)o).onRunOverDoSmoethingEnd = @onRunOverDoSmoethingEnd;
            return ptr_of_this_method;
        }



    }
}
