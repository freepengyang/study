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
    unsafe class QuDaoConstant_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::QuDaoConstant);
            args = new Type[]{};
            method = type.GetMethod("get_OpenRecharge", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_OpenRecharge_0);
            args = new Type[]{};
            method = type.GetMethod("GetPlatformData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetPlatformData_1);
            args = new Type[]{};
            method = type.GetMethod("get_OpenVoice", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_OpenVoice_2);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_OpenFeedBack", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_OpenFeedBack_3);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_OpenVoice", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_OpenVoice_4);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_OpenRecharge", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_OpenRecharge_5);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_OpenTranslate", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_OpenTranslate_6);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_OpenPush", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_OpenPush_7);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_ChannelCloseRegister", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_ChannelCloseRegister_8);
            args = new Type[]{};
            method = type.GetMethod("get_OpenCheckVersion", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_OpenCheckVersion_9);
            args = new Type[]{};
            method = type.GetMethod("isEditorMode", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, isEditorMode_10);
            args = new Type[]{};
            method = type.GetMethod("get_OpenTranslate", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_OpenTranslate_11);
            args = new Type[]{};
            method = type.GetMethod("get_ChannelCloseRegister", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_ChannelCloseRegister_12);


        }


        static StackObject* get_OpenRecharge_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::QuDaoConstant.OpenRecharge;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* GetPlatformData_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::QuDaoConstant.GetPlatformData();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_OpenVoice_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::QuDaoConstant.OpenVoice;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* set_OpenFeedBack_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;


            global::QuDaoConstant.OpenFeedBack = value;

            return __ret;
        }

        static StackObject* set_OpenVoice_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;


            global::QuDaoConstant.OpenVoice = value;

            return __ret;
        }

        static StackObject* set_OpenRecharge_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;


            global::QuDaoConstant.OpenRecharge = value;

            return __ret;
        }

        static StackObject* set_OpenTranslate_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;


            global::QuDaoConstant.OpenTranslate = value;

            return __ret;
        }

        static StackObject* set_OpenPush_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;


            global::QuDaoConstant.OpenPush = value;

            return __ret;
        }

        static StackObject* set_ChannelCloseRegister_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;


            global::QuDaoConstant.ChannelCloseRegister = value;

            return __ret;
        }

        static StackObject* get_OpenCheckVersion_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::QuDaoConstant.OpenCheckVersion;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* isEditorMode_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::QuDaoConstant.isEditorMode();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_OpenTranslate_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::QuDaoConstant.OpenTranslate;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_ChannelCloseRegister_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::QuDaoConstant.ChannelCloseRegister;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }



    }
}
