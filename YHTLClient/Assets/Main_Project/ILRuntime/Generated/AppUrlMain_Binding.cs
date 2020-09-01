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
    unsafe class AppUrlMain_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::AppUrlMain);
            args = new Type[]{};
            method = type.GetMethod("get_rechargeUrl", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_rechargeUrl_0);
            args = new Type[]{};
            method = type.GetMethod("serverListUrl", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, serverListUrl_1);
            args = new Type[]{};
            method = type.GetMethod("get_centerUrl", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_centerUrl_2);

            field = type.GetField("mClientResPath", flag);
            app.RegisterCLRFieldGetter(field, get_mClientResPath_0);
            app.RegisterCLRFieldSetter(field, set_mClientResPath_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_mClientResPath_0, AssignFromStack_mClientResPath_0);
            field = type.GetField("mRepairListFileName", flag);
            app.RegisterCLRFieldGetter(field, get_mRepairListFileName_1);
            app.RegisterCLRFieldSetter(field, set_mRepairListFileName_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_mRepairListFileName_1, AssignFromStack_mRepairListFileName_1);
            field = type.GetField("mServerResURL", flag);
            app.RegisterCLRFieldGetter(field, get_mServerResURL_2);
            app.RegisterCLRFieldSetter(field, set_mServerResURL_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_mServerResURL_2, AssignFromStack_mServerResURL_2);
            field = type.GetField("cdnVersion", flag);
            app.RegisterCLRFieldGetter(field, get_cdnVersion_3);
            app.RegisterCLRFieldSetter(field, set_cdnVersion_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_cdnVersion_3, AssignFromStack_cdnVersion_3);
            field = type.GetField("updateVersion", flag);
            app.RegisterCLRFieldGetter(field, get_updateVersion_4);
            app.RegisterCLRFieldSetter(field, set_updateVersion_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_updateVersion_4, AssignFromStack_updateVersion_4);


        }


        static StackObject* get_rechargeUrl_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::AppUrlMain.rechargeUrl;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* serverListUrl_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::AppUrlMain.serverListUrl();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_centerUrl_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::AppUrlMain.centerUrl;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_mClientResPath_0(ref object o)
        {
            return global::AppUrlMain.mClientResPath;
        }

        static StackObject* CopyToStack_mClientResPath_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::AppUrlMain.mClientResPath;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mClientResPath_0(ref object o, object v)
        {
            global::AppUrlMain.mClientResPath = (System.String)v;
        }

        static StackObject* AssignFromStack_mClientResPath_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @mClientResPath = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::AppUrlMain.mClientResPath = @mClientResPath;
            return ptr_of_this_method;
        }

        static object get_mRepairListFileName_1(ref object o)
        {
            return global::AppUrlMain.mRepairListFileName;
        }

        static StackObject* CopyToStack_mRepairListFileName_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::AppUrlMain.mRepairListFileName;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mRepairListFileName_1(ref object o, object v)
        {
            global::AppUrlMain.mRepairListFileName = (System.String)v;
        }

        static StackObject* AssignFromStack_mRepairListFileName_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @mRepairListFileName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::AppUrlMain.mRepairListFileName = @mRepairListFileName;
            return ptr_of_this_method;
        }

        static object get_mServerResURL_2(ref object o)
        {
            return global::AppUrlMain.mServerResURL;
        }

        static StackObject* CopyToStack_mServerResURL_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::AppUrlMain.mServerResURL;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mServerResURL_2(ref object o, object v)
        {
            global::AppUrlMain.mServerResURL = (System.String)v;
        }

        static StackObject* AssignFromStack_mServerResURL_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @mServerResURL = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::AppUrlMain.mServerResURL = @mServerResURL;
            return ptr_of_this_method;
        }

        static object get_cdnVersion_3(ref object o)
        {
            return global::AppUrlMain.cdnVersion;
        }

        static StackObject* CopyToStack_cdnVersion_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::AppUrlMain.cdnVersion;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_cdnVersion_3(ref object o, object v)
        {
            global::AppUrlMain.cdnVersion = (System.String)v;
        }

        static StackObject* AssignFromStack_cdnVersion_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @cdnVersion = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::AppUrlMain.cdnVersion = @cdnVersion;
            return ptr_of_this_method;
        }

        static object get_updateVersion_4(ref object o)
        {
            return global::AppUrlMain.updateVersion;
        }

        static StackObject* CopyToStack_updateVersion_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::AppUrlMain.updateVersion;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_updateVersion_4(ref object o, object v)
        {
            global::AppUrlMain.updateVersion = (System.Version)v;
        }

        static StackObject* AssignFromStack_updateVersion_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Version @updateVersion = (System.Version)typeof(System.Version).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::AppUrlMain.updateVersion = @updateVersion;
            return ptr_of_this_method;
        }



    }
}
