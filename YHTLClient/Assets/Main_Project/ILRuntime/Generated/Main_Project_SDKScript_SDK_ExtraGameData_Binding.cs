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
    unsafe class Main_Project_SDKScript_SDK_ExtraGameData_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Main_Project.SDKScript.SDK.ExtraGameData);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_dataType", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_dataType_0);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_userID", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_userID_1);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_guildName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_guildName_2);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_moneyNum", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_moneyNum_3);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_roleID", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_roleID_4);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_roleName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_roleName_5);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_serverID", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_serverID_6);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_roleLevel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_roleLevel_7);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_serverName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_serverName_8);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_vipLevel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_vipLevel_9);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_vipExp", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_vipExp_10);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("set_updateLevelTime", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_updateLevelTime_11);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_guildLevel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_guildLevel_12);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_guildID", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_guildID_13);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_guildLeaderName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_guildLeaderName_14);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_rolePower", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_rolePower_15);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_professionID", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_professionID_16);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_profession", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_profession_17);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_professionRoleName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_professionRoleName_18);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_sex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_sex_19);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("set_createRoleTime", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_createRoleTime_20);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* set_dataType_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.dataType = value;

            return __ret;
        }

        static StackObject* set_userID_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.userID = value;

            return __ret;
        }

        static StackObject* set_guildName_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.guildName = value;

            return __ret;
        }

        static StackObject* set_moneyNum_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.moneyNum = value;

            return __ret;
        }

        static StackObject* set_roleID_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.roleID = value;

            return __ret;
        }

        static StackObject* set_roleName_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.roleName = value;

            return __ret;
        }

        static StackObject* set_serverID_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.serverID = value;

            return __ret;
        }

        static StackObject* set_roleLevel_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.roleLevel = value;

            return __ret;
        }

        static StackObject* set_serverName_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.serverName = value;

            return __ret;
        }

        static StackObject* set_vipLevel_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.vipLevel = value;

            return __ret;
        }

        static StackObject* set_vipExp_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.vipExp = value;

            return __ret;
        }

        static StackObject* set_updateLevelTime_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @value = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.updateLevelTime = value;

            return __ret;
        }

        static StackObject* set_guildLevel_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.guildLevel = value;

            return __ret;
        }

        static StackObject* set_guildID_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.guildID = value;

            return __ret;
        }

        static StackObject* set_guildLeaderName_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.guildLeaderName = value;

            return __ret;
        }

        static StackObject* set_rolePower_15(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.rolePower = value;

            return __ret;
        }

        static StackObject* set_professionID_16(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.professionID = value;

            return __ret;
        }

        static StackObject* set_profession_17(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.profession = value;

            return __ret;
        }

        static StackObject* set_professionRoleName_18(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.professionRoleName = value;

            return __ret;
        }

        static StackObject* set_sex_19(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.sex = value;

            return __ret;
        }

        static StackObject* set_createRoleTime_20(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @value = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.SDKScript.SDK.ExtraGameData instance_of_this_method = (Main_Project.SDKScript.SDK.ExtraGameData)typeof(Main_Project.SDKScript.SDK.ExtraGameData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.createRoleTime = value;

            return __ret;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Main_Project.SDKScript.SDK.ExtraGameData();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
