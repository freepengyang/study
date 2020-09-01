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
    unsafe class QuDaoInterface_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::QuDaoInterface);
            args = new Type[]{};
            method = type.GetMethod("get_Instance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Instance_0);
            args = new Type[]{};
            method = type.GetMethod("FinishGame", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FinishGame_1);
            args = new Type[]{typeof(System.Int32), typeof(Main_Project.SDKScript.SDK.ExtraGameData)};
            method = type.GetMethod("SubmitGameData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SubmitGameData_2);
            args = new Type[]{typeof(Main_Project.SDKScript.SDK.QuDaoPayParams)};
            method = type.GetMethod("FuKuan", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FuKuan_3);
            args = new Type[]{};
            method = type.GetMethod("GetChannelId", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetChannelId_4);
            args = new Type[]{};
            method = type.GetMethod("GetVersion", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetVersion_5);
            args = new Type[]{};
            method = type.GetMethod("Logout", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Logout_6);
            args = new Type[]{};
            method = type.GetMethod("getMemoryLimitResidue", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getMemoryLimitResidue_7);
            args = new Type[]{};
            method = type.GetMethod("RestartGame", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RestartGame_8);
            args = new Type[]{};
            method = type.GetMethod("getSystemModel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getSystemModel_9);
            args = new Type[]{};
            method = type.GetMethod("ChangeAccount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ChangeAccount_10);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("Login", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Login_11);
            args = new Type[]{};
            method = type.GetMethod("Login", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Login_12);
            args = new Type[]{};
            method = type.GetMethod("GetBattery", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetBattery_13);

            field = type.GetField("OnMultiLoginSuc", flag);
            app.RegisterCLRFieldGetter(field, get_OnMultiLoginSuc_0);
            app.RegisterCLRFieldSetter(field, set_OnMultiLoginSuc_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnMultiLoginSuc_0, AssignFromStack_OnMultiLoginSuc_0);
            field = type.GetField("OnLoginSuc", flag);
            app.RegisterCLRFieldGetter(field, get_OnLoginSuc_1);
            app.RegisterCLRFieldSetter(field, set_OnLoginSuc_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnLoginSuc_1, AssignFromStack_OnLoginSuc_1);
            field = type.GetField("OnIOSSDKLoginSuc", flag);
            app.RegisterCLRFieldGetter(field, get_OnIOSSDKLoginSuc_2);
            app.RegisterCLRFieldSetter(field, set_OnIOSSDKLoginSuc_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnIOSSDKLoginSuc_2, AssignFromStack_OnIOSSDKLoginSuc_2);


        }


        static StackObject* get_Instance_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::QuDaoInterface.Instance;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* FinishGame_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.FinishGame();

            return __ret;
        }

        static StackObject* SubmitGameData_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Main_Project.SDKScript.SDK.ExtraGameData @gameData = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @extraType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SubmitGameData(@extraType, @gameData);

            return __ret;
        }

        static StackObject* FuKuan_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Main_Project.SDKScript.SDK.QuDaoPayParams @data = (Main_Project.SDKScript.SDK.QuDaoPayParams)typeof(Main_Project.SDKScript.SDK.QuDaoPayParams).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.FuKuan(@data);

            return __ret;
        }

        static StackObject* GetChannelId_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetChannelId();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetVersion_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetVersion();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Logout_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Logout();

            return __ret;
        }

        static StackObject* getMemoryLimitResidue_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getMemoryLimitResidue();

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* RestartGame_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RestartGame();

            return __ret;
        }

        static StackObject* getSystemModel_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getSystemModel();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ChangeAccount_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ChangeAccount();

            return __ret;
        }

        static StackObject* Login_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @info = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Login(@info);

            return __ret;
        }

        static StackObject* Login_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Login();

            return __ret;
        }

        static StackObject* GetBattery_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::QuDaoInterface instance_of_this_method = (global::QuDaoInterface)typeof(global::QuDaoInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetBattery();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }


        static object get_OnMultiLoginSuc_0(ref object o)
        {
            return ((global::QuDaoInterface)o).OnMultiLoginSuc;
        }

        static StackObject* CopyToStack_OnMultiLoginSuc_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::QuDaoInterface)o).OnMultiLoginSuc;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnMultiLoginSuc_0(ref object o, object v)
        {
            ((global::QuDaoInterface)o).OnMultiLoginSuc = (global::QuDaoInterface.LoginMultiSucHandler)v;
        }

        static StackObject* AssignFromStack_OnMultiLoginSuc_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::QuDaoInterface.LoginMultiSucHandler @OnMultiLoginSuc = (global::QuDaoInterface.LoginMultiSucHandler)typeof(global::QuDaoInterface.LoginMultiSucHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::QuDaoInterface)o).OnMultiLoginSuc = @OnMultiLoginSuc;
            return ptr_of_this_method;
        }

        static object get_OnLoginSuc_1(ref object o)
        {
            return ((global::QuDaoInterface)o).OnLoginSuc;
        }

        static StackObject* CopyToStack_OnLoginSuc_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::QuDaoInterface)o).OnLoginSuc;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnLoginSuc_1(ref object o, object v)
        {
            ((global::QuDaoInterface)o).OnLoginSuc = (global::QuDaoInterface.LoginSucHandler)v;
        }

        static StackObject* AssignFromStack_OnLoginSuc_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::QuDaoInterface.LoginSucHandler @OnLoginSuc = (global::QuDaoInterface.LoginSucHandler)typeof(global::QuDaoInterface.LoginSucHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::QuDaoInterface)o).OnLoginSuc = @OnLoginSuc;
            return ptr_of_this_method;
        }

        static object get_OnIOSSDKLoginSuc_2(ref object o)
        {
            return ((global::QuDaoInterface)o).OnIOSSDKLoginSuc;
        }

        static StackObject* CopyToStack_OnIOSSDKLoginSuc_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::QuDaoInterface)o).OnIOSSDKLoginSuc;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnIOSSDKLoginSuc_2(ref object o, object v)
        {
            ((global::QuDaoInterface)o).OnIOSSDKLoginSuc = (global::QuDaoInterface.LoginMultiSucHandler)v;
        }

        static StackObject* AssignFromStack_OnIOSSDKLoginSuc_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::QuDaoInterface.LoginMultiSucHandler @OnIOSSDKLoginSuc = (global::QuDaoInterface.LoginMultiSucHandler)typeof(global::QuDaoInterface.LoginMultiSucHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::QuDaoInterface)o).OnIOSSDKLoginSuc = @OnIOSSDKLoginSuc;
            return ptr_of_this_method;
        }



    }
}
