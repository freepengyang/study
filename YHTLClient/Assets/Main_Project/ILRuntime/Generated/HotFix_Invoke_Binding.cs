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
    unsafe class HotFix_Invoke_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::HotFix_Invoke);
            args = new Type[]{};
            method = type.GetMethod("get_Instance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Instance_0);

            field = type.GetField("mOnLogoutAccount", flag);
            app.RegisterCLRFieldGetter(field, get_mOnLogoutAccount_0);
            app.RegisterCLRFieldSetter(field, set_mOnLogoutAccount_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_mOnLogoutAccount_0, AssignFromStack_mOnLogoutAccount_0);
            field = type.GetField("mReqPay", flag);
            app.RegisterCLRFieldGetter(field, get_mReqPay_1);
            app.RegisterCLRFieldSetter(field, set_mReqPay_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_mReqPay_1, AssignFromStack_mReqPay_1);
            field = type.GetField("mACTION_BATTERY_CHANGED", flag);
            app.RegisterCLRFieldGetter(field, get_mACTION_BATTERY_CHANGED_2);
            app.RegisterCLRFieldSetter(field, set_mACTION_BATTERY_CHANGED_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_mACTION_BATTERY_CHANGED_2, AssignFromStack_mACTION_BATTERY_CHANGED_2);
            field = type.GetField("mOnQuitGameTips", flag);
            app.RegisterCLRFieldGetter(field, get_mOnQuitGameTips_3);
            app.RegisterCLRFieldSetter(field, set_mOnQuitGameTips_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_mOnQuitGameTips_3, AssignFromStack_mOnQuitGameTips_3);
            field = type.GetField("mRefreshTime", flag);
            app.RegisterCLRFieldGetter(field, get_mRefreshTime_4);
            app.RegisterCLRFieldSetter(field, set_mRefreshTime_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_mRefreshTime_4, AssignFromStack_mRefreshTime_4);
            field = type.GetField("EventHandler", flag);
            app.RegisterCLRFieldGetter(field, get_EventHandler_5);
            app.RegisterCLRFieldSetter(field, set_EventHandler_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_EventHandler_5, AssignFromStack_EventHandler_5);


        }


        static StackObject* get_Instance_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::HotFix_Invoke.Instance;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_mOnLogoutAccount_0(ref object o)
        {
            return ((global::HotFix_Invoke)o).mOnLogoutAccount;
        }

        static StackObject* CopyToStack_mOnLogoutAccount_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::HotFix_Invoke)o).mOnLogoutAccount;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mOnLogoutAccount_0(ref object o, object v)
        {
            ((global::HotFix_Invoke)o).mOnLogoutAccount = (System.Action)v;
        }

        static StackObject* AssignFromStack_mOnLogoutAccount_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @mOnLogoutAccount = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::HotFix_Invoke)o).mOnLogoutAccount = @mOnLogoutAccount;
            return ptr_of_this_method;
        }

        static object get_mReqPay_1(ref object o)
        {
            return ((global::HotFix_Invoke)o).mReqPay;
        }

        static StackObject* CopyToStack_mReqPay_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::HotFix_Invoke)o).mReqPay;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mReqPay_1(ref object o, object v)
        {
            ((global::HotFix_Invoke)o).mReqPay = (System.Action<System.String>)v;
        }

        static StackObject* AssignFromStack_mReqPay_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.String> @mReqPay = (System.Action<System.String>)typeof(System.Action<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::HotFix_Invoke)o).mReqPay = @mReqPay;
            return ptr_of_this_method;
        }

        static object get_mACTION_BATTERY_CHANGED_2(ref object o)
        {
            return ((global::HotFix_Invoke)o).mACTION_BATTERY_CHANGED;
        }

        static StackObject* CopyToStack_mACTION_BATTERY_CHANGED_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::HotFix_Invoke)o).mACTION_BATTERY_CHANGED;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mACTION_BATTERY_CHANGED_2(ref object o, object v)
        {
            ((global::HotFix_Invoke)o).mACTION_BATTERY_CHANGED = (System.Action<System.Int32>)v;
        }

        static StackObject* AssignFromStack_mACTION_BATTERY_CHANGED_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Int32> @mACTION_BATTERY_CHANGED = (System.Action<System.Int32>)typeof(System.Action<System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::HotFix_Invoke)o).mACTION_BATTERY_CHANGED = @mACTION_BATTERY_CHANGED;
            return ptr_of_this_method;
        }

        static object get_mOnQuitGameTips_3(ref object o)
        {
            return ((global::HotFix_Invoke)o).mOnQuitGameTips;
        }

        static StackObject* CopyToStack_mOnQuitGameTips_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::HotFix_Invoke)o).mOnQuitGameTips;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mOnQuitGameTips_3(ref object o, object v)
        {
            ((global::HotFix_Invoke)o).mOnQuitGameTips = (System.Action)v;
        }

        static StackObject* AssignFromStack_mOnQuitGameTips_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @mOnQuitGameTips = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::HotFix_Invoke)o).mOnQuitGameTips = @mOnQuitGameTips;
            return ptr_of_this_method;
        }

        static object get_mRefreshTime_4(ref object o)
        {
            return ((global::HotFix_Invoke)o).mRefreshTime;
        }

        static StackObject* CopyToStack_mRefreshTime_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::HotFix_Invoke)o).mRefreshTime;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mRefreshTime_4(ref object o, object v)
        {
            ((global::HotFix_Invoke)o).mRefreshTime = (System.Action<heart.Heartbeat>)v;
        }

        static StackObject* AssignFromStack_mRefreshTime_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<heart.Heartbeat> @mRefreshTime = (System.Action<heart.Heartbeat>)typeof(System.Action<heart.Heartbeat>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::HotFix_Invoke)o).mRefreshTime = @mRefreshTime;
            return ptr_of_this_method;
        }

        static object get_EventHandler_5(ref object o)
        {
            return global::HotFix_Invoke.EventHandler;
        }

        static StackObject* CopyToStack_EventHandler_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::HotFix_Invoke.EventHandler;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_EventHandler_5(ref object o, object v)
        {
            global::HotFix_Invoke.EventHandler = (global::MainEventHanlderManager)v;
        }

        static StackObject* AssignFromStack_EventHandler_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::MainEventHanlderManager @EventHandler = (global::MainEventHanlderManager)typeof(global::MainEventHanlderManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::HotFix_Invoke.EventHandler = @EventHandler;
            return ptr_of_this_method;
        }



    }
}
