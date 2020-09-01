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
    unsafe class Main_Project_Script_Update_CSVersionManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Main_Project.Script.Update.CSVersionManager);
            args = new Type[]{};
            method = type.GetMethod("get_ServerVersion", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_ServerVersion_0);
            args = new Type[]{};
            method = type.GetMethod("get_Version", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Version_1);
            args = new Type[]{typeof(System.Action<System.Boolean>)};
            method = type.GetMethod("Init", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Init_2);
            args = new Type[]{typeof(System.String), typeof(System.Action<Main_Project.Script.Update.UpdateCheckCode>)};
            method = type.GetMethod("CheckVersion", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CheckVersion_3);


        }


        static StackObject* get_ServerVersion_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Main_Project.Script.Update.CSVersionManager instance_of_this_method = (Main_Project.Script.Update.CSVersionManager)typeof(Main_Project.Script.Update.CSVersionManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.ServerVersion;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_Version_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Main_Project.Script.Update.CSVersionManager instance_of_this_method = (Main_Project.Script.Update.CSVersionManager)typeof(Main_Project.Script.Update.CSVersionManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Version;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Init_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Boolean> @onInitFinish = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.Script.Update.CSVersionManager instance_of_this_method = (Main_Project.Script.Update.CSVersionManager)typeof(Main_Project.Script.Update.CSVersionManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Init(@onInitFinish);

            return __ret;
        }

        static StackObject* CheckVersion_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<Main_Project.Script.Update.UpdateCheckCode> @onCheckFinish = (System.Action<Main_Project.Script.Update.UpdateCheckCode>)typeof(System.Action<Main_Project.Script.Update.UpdateCheckCode>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @serverVersion = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Main_Project.Script.Update.CSVersionManager instance_of_this_method = (Main_Project.Script.Update.CSVersionManager)typeof(Main_Project.Script.Update.CSVersionManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CheckVersion(@serverVersion, @onCheckFinish);

            return __ret;
        }



    }
}
