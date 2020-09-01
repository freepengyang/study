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
    unsafe class Main_Project_Script_Update_CSCheckResourceManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Main_Project.Script.Update.CSCheckResourceManager);
            args = new Type[]{};
            method = type.GetMethod("OnDispose", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OnDispose_0);

            field = type.GetField("RESOURCELISTNAME", flag);
            app.RegisterCLRFieldGetter(field, get_RESOURCELISTNAME_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_RESOURCELISTNAME_0, null);


        }


        static StackObject* OnDispose_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Main_Project.Script.Update.CSCheckResourceManager instance_of_this_method = (Main_Project.Script.Update.CSCheckResourceManager)typeof(Main_Project.Script.Update.CSCheckResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OnDispose();

            return __ret;
        }


        static object get_RESOURCELISTNAME_0(ref object o)
        {
            return Main_Project.Script.Update.CSCheckResourceManager.RESOURCELISTNAME;
        }

        static StackObject* CopyToStack_RESOURCELISTNAME_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = Main_Project.Script.Update.CSCheckResourceManager.RESOURCELISTNAME;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
