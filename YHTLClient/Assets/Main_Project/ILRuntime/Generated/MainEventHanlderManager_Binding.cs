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
    unsafe class MainEventHanlderManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::MainEventHanlderManager);
            args = new Type[]{typeof(global::MainEvent), typeof(global::MainBaseEvent.Callback)};
            method = type.GetMethod("AddEvent", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddEvent_0);
            args = new Type[]{};
            method = type.GetMethod("UnRegAll", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UnRegAll_1);
            args = new Type[]{typeof(global::MainEvent), typeof(System.Object)};
            method = type.GetMethod("SendEvent", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SendEvent_2);
            args = new Type[]{typeof(System.UInt32), typeof(global::MainBaseEvent.Callback)};
            method = type.GetMethod("UnReg", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UnReg_3);

            args = new Type[]{typeof(global::MainEventHanlderManager.MainDispatchType)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* AddEvent_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::MainBaseEvent.Callback @callback = (global::MainBaseEvent.Callback)typeof(global::MainBaseEvent.Callback).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::MainEvent @uiEvtID = (global::MainEvent)typeof(global::MainEvent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::MainEventHanlderManager instance_of_this_method = (global::MainEventHanlderManager)typeof(global::MainEventHanlderManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddEvent(@uiEvtID, @callback);

            return __ret;
        }

        static StackObject* UnRegAll_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::MainEventHanlderManager instance_of_this_method = (global::MainEventHanlderManager)typeof(global::MainEventHanlderManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UnRegAll();

            return __ret;
        }

        static StackObject* SendEvent_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @objData = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::MainEvent @uiEvtID = (global::MainEvent)typeof(global::MainEvent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::MainEventHanlderManager instance_of_this_method = (global::MainEventHanlderManager)typeof(global::MainEventHanlderManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SendEvent(@uiEvtID, @objData);

            return __ret;
        }

        static StackObject* UnReg_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::MainBaseEvent.Callback @cb = (global::MainBaseEvent.Callback)typeof(global::MainBaseEvent.Callback).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.UInt32 @msgId = (uint)ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::MainEventHanlderManager instance_of_this_method = (global::MainEventHanlderManager)typeof(global::MainEventHanlderManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UnReg(@msgId, @cb);

            return __ret;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::MainEventHanlderManager.MainDispatchType @dt = (global::MainEventHanlderManager.MainDispatchType)typeof(global::MainEventHanlderManager.MainDispatchType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = new global::MainEventHanlderManager(@dt);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
