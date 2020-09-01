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
    unsafe class Main_Project_SDKScript_SDK_QuDaoPayParams_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Main_Project.SDKScript.SDK.QuDaoPayParams);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_amount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_amount_0);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_sid", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_sid_1);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_serverName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_serverName_2);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_vipLevel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_vipLevel_3);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_roleLevel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_roleLevel_4);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_UnionName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_UnionName_5);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("set_CreateTime", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_CreateTime_6);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_balance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_balance_7);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_sign", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_sign_8);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_remainder", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_remainder_9);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* set_amount_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.QuDaoPayParams instance_of_this_method = (Main_Project.SDKScript.SDK.QuDaoPayParams)typeof(Main_Project.SDKScript.SDK.QuDaoPayParams).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.amount = value;

            return __ret;
        }

        static StackObject* set_sid_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.QuDaoPayParams instance_of_this_method = (Main_Project.SDKScript.SDK.QuDaoPayParams)typeof(Main_Project.SDKScript.SDK.QuDaoPayParams).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.sid = value;

            return __ret;
        }

        static StackObject* set_serverName_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.QuDaoPayParams instance_of_this_method = (Main_Project.SDKScript.SDK.QuDaoPayParams)typeof(Main_Project.SDKScript.SDK.QuDaoPayParams).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.serverName = value;

            return __ret;
        }

        static StackObject* set_vipLevel_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.QuDaoPayParams instance_of_this_method = (Main_Project.SDKScript.SDK.QuDaoPayParams)typeof(Main_Project.SDKScript.SDK.QuDaoPayParams).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.vipLevel = value;

            return __ret;
        }

        static StackObject* set_roleLevel_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.QuDaoPayParams instance_of_this_method = (Main_Project.SDKScript.SDK.QuDaoPayParams)typeof(Main_Project.SDKScript.SDK.QuDaoPayParams).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.roleLevel = value;

            return __ret;
        }

        static StackObject* set_UnionName_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.QuDaoPayParams instance_of_this_method = (Main_Project.SDKScript.SDK.QuDaoPayParams)typeof(Main_Project.SDKScript.SDK.QuDaoPayParams).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UnionName = value;

            return __ret;
        }

        static StackObject* set_CreateTime_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @value = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.QuDaoPayParams instance_of_this_method = (Main_Project.SDKScript.SDK.QuDaoPayParams)typeof(Main_Project.SDKScript.SDK.QuDaoPayParams).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CreateTime = value;

            return __ret;
        }

        static StackObject* set_balance_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.QuDaoPayParams instance_of_this_method = (Main_Project.SDKScript.SDK.QuDaoPayParams)typeof(Main_Project.SDKScript.SDK.QuDaoPayParams).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.balance = value;

            return __ret;
        }

        static StackObject* set_sign_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.QuDaoPayParams instance_of_this_method = (Main_Project.SDKScript.SDK.QuDaoPayParams)typeof(Main_Project.SDKScript.SDK.QuDaoPayParams).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.sign = value;

            return __ret;
        }

        static StackObject* set_remainder_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.QuDaoPayParams instance_of_this_method = (Main_Project.SDKScript.SDK.QuDaoPayParams)typeof(Main_Project.SDKScript.SDK.QuDaoPayParams).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.remainder = value;

            return __ret;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Main_Project.SDKScript.SDK.QuDaoPayParams();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
