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
    unsafe class CSSubmitDataManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::CSSubmitDataManager);
            args = new Type[]{};
            method = type.GetMethod("GetQueryloginnameForm", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetQueryloginnameForm_0);
            args = new Type[]{typeof(global::SubmitDataType)};
            method = type.GetMethod("SendSubmitData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SendSubmitData_1);
            args = new Type[]{};
            method = type.GetMethod("SendLoginData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SendLoginData_2);
            args = new Type[]{};
            method = type.GetMethod("StartWrite", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StartWrite_3);


        }


        static StackObject* GetQueryloginnameForm_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::CSSubmitDataManager.GetQueryloginnameForm();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SendSubmitData_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::SubmitDataType @type = (global::SubmitDataType)typeof(global::SubmitDataType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSSubmitDataManager instance_of_this_method = (global::CSSubmitDataManager)typeof(global::CSSubmitDataManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SendSubmitData(@type);

            return __ret;
        }

        static StackObject* SendLoginData_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSSubmitDataManager instance_of_this_method = (global::CSSubmitDataManager)typeof(global::CSSubmitDataManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SendLoginData();

            return __ret;
        }

        static StackObject* StartWrite_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSSubmitDataManager instance_of_this_method = (global::CSSubmitDataManager)typeof(global::CSSubmitDataManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.StartWrite();

            return __ret;
        }



    }
}
