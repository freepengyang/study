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
    unsafe class Main_Project_Script_Update_CSResUpdateManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Main_Project.Script.Update.CSResUpdateManager);
            args = new Type[]{};
            method = type.GetMethod("CloseDownloadThread", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CloseDownloadThread_0);
            args = new Type[]{};
            method = type.GetMethod("StartBackDownload", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StartBackDownload_1);
            args = new Type[]{};
            method = type.GetMethod("OnDispose", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OnDispose_2);
            args = new Type[]{typeof(System.Action<System.Int32, System.Int32>)};
            method = type.GetMethod("RegUpdateAction", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RegUpdateAction_3);
            args = new Type[]{};
            method = type.GetMethod("get_CurBackDownloadByteNum", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_CurBackDownloadByteNum_4);
            args = new Type[]{};
            method = type.GetMethod("get_BackDownloadByteNum", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_BackDownloadByteNum_5);
            args = new Type[]{typeof(System.Action<System.Int32, System.Int32>)};
            method = type.GetMethod("UnRegUpdateAction", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UnRegUpdateAction_6);
            args = new Type[]{typeof(System.Action<System.Boolean>)};
            method = type.GetMethod("StartPreDownload", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StartPreDownload_7);

            field = type.GetField("preDownloadByteNum", flag);
            app.RegisterCLRFieldGetter(field, get_preDownloadByteNum_0);
            app.RegisterCLRFieldSetter(field, set_preDownloadByteNum_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_preDownloadByteNum_0, AssignFromStack_preDownloadByteNum_0);


        }


        static StackObject* CloseDownloadThread_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Main_Project.Script.Update.CSResUpdateManager instance_of_this_method = (Main_Project.Script.Update.CSResUpdateManager)typeof(Main_Project.Script.Update.CSResUpdateManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CloseDownloadThread();

            return __ret;
        }

        static StackObject* StartBackDownload_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Main_Project.Script.Update.CSResUpdateManager instance_of_this_method = (Main_Project.Script.Update.CSResUpdateManager)typeof(Main_Project.Script.Update.CSResUpdateManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.StartBackDownload();

            return __ret;
        }

        static StackObject* OnDispose_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Main_Project.Script.Update.CSResUpdateManager instance_of_this_method = (Main_Project.Script.Update.CSResUpdateManager)typeof(Main_Project.Script.Update.CSResUpdateManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OnDispose();

            return __ret;
        }

        static StackObject* RegUpdateAction_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int32, System.Int32> @callBack = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.Script.Update.CSResUpdateManager instance_of_this_method = (Main_Project.Script.Update.CSResUpdateManager)typeof(Main_Project.Script.Update.CSResUpdateManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RegUpdateAction(@callBack);

            return __ret;
        }

        static StackObject* get_CurBackDownloadByteNum_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Main_Project.Script.Update.CSResUpdateManager instance_of_this_method = (Main_Project.Script.Update.CSResUpdateManager)typeof(Main_Project.Script.Update.CSResUpdateManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.CurBackDownloadByteNum;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* get_BackDownloadByteNum_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Main_Project.Script.Update.CSResUpdateManager instance_of_this_method = (Main_Project.Script.Update.CSResUpdateManager)typeof(Main_Project.Script.Update.CSResUpdateManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.BackDownloadByteNum;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* UnRegUpdateAction_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int32, System.Int32> @callBack = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.Script.Update.CSResUpdateManager instance_of_this_method = (Main_Project.Script.Update.CSResUpdateManager)typeof(Main_Project.Script.Update.CSResUpdateManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UnRegUpdateAction(@callBack);

            return __ret;
        }

        static StackObject* StartPreDownload_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Boolean> @onComplete = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Main_Project.Script.Update.CSResUpdateManager instance_of_this_method = (Main_Project.Script.Update.CSResUpdateManager)typeof(Main_Project.Script.Update.CSResUpdateManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.StartPreDownload(@onComplete);

            return __ret;
        }


        static object get_preDownloadByteNum_0(ref object o)
        {
            return ((Main_Project.Script.Update.CSResUpdateManager)o).preDownloadByteNum;
        }

        static StackObject* CopyToStack_preDownloadByteNum_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Main_Project.Script.Update.CSResUpdateManager)o).preDownloadByteNum;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_preDownloadByteNum_0(ref object o, object v)
        {
            ((Main_Project.Script.Update.CSResUpdateManager)o).preDownloadByteNum = (System.Int32)v;
        }

        static StackObject* AssignFromStack_preDownloadByteNum_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @preDownloadByteNum = ptr_of_this_method->Value;
            ((Main_Project.Script.Update.CSResUpdateManager)o).preDownloadByteNum = @preDownloadByteNum;
            return ptr_of_this_method;
        }



    }
}
