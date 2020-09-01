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
    unsafe class UIDebugScrollBase_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIDebugScrollBase);
            args = new Type[]{};
            method = type.GetMethod("ClearData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClearData_0);
            args = new Type[]{};
            method = type.GetMethod("Clear", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Clear_1);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_IsNeedUpdateData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_IsNeedUpdateData_2);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetBoxColliderEnable", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetBoxColliderEnable_3);
            args = new Type[]{};
            method = type.GetMethod("UpdateData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateData_4);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("RefreshScroll", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RefreshScroll_5);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("Search", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Search_6);

            field = type.GetField("togType", flag);
            app.RegisterCLRFieldGetter(field, get_togType_0);
            app.RegisterCLRFieldSetter(field, set_togType_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_togType_0, AssignFromStack_togType_0);
            field = type.GetField("mark", flag);
            app.RegisterCLRFieldGetter(field, get_mark_1);
            app.RegisterCLRFieldSetter(field, set_mark_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_mark_1, AssignFromStack_mark_1);


        }


        static StackObject* ClearData_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIDebugScrollBase instance_of_this_method = (global::UIDebugScrollBase)typeof(global::UIDebugScrollBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ClearData();

            return __ret;
        }

        static StackObject* Clear_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIDebugScrollBase instance_of_this_method = (global::UIDebugScrollBase)typeof(global::UIDebugScrollBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Clear();

            return __ret;
        }

        static StackObject* set_IsNeedUpdateData_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIDebugScrollBase instance_of_this_method = (global::UIDebugScrollBase)typeof(global::UIDebugScrollBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.IsNeedUpdateData = value;

            return __ret;
        }

        static StackObject* SetBoxColliderEnable_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isEnable = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIDebugScrollBase instance_of_this_method = (global::UIDebugScrollBase)typeof(global::UIDebugScrollBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetBoxColliderEnable(@isEnable);

            return __ret;
        }

        static StackObject* UpdateData_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIDebugScrollBase instance_of_this_method = (global::UIDebugScrollBase)typeof(global::UIDebugScrollBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateData();

            return __ret;
        }

        static StackObject* RefreshScroll_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isUpdateData = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIDebugScrollBase instance_of_this_method = (global::UIDebugScrollBase)typeof(global::UIDebugScrollBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RefreshScroll(@isUpdateData);

            return __ret;
        }

        static StackObject* Search_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @str = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIDebugScrollBase instance_of_this_method = (global::UIDebugScrollBase)typeof(global::UIDebugScrollBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Search(@str);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }


        static object get_togType_0(ref object o)
        {
            return ((global::UIDebugScrollBase)o).togType;
        }

        static StackObject* CopyToStack_togType_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIDebugScrollBase)o).togType;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_togType_0(ref object o, object v)
        {
            ((global::UIDebugScrollBase)o).togType = (global::ELogToggleType)v;
        }

        static StackObject* AssignFromStack_togType_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::ELogToggleType @togType = (global::ELogToggleType)typeof(global::ELogToggleType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIDebugScrollBase)o).togType = @togType;
            return ptr_of_this_method;
        }

        static object get_mark_1(ref object o)
        {
            return ((global::UIDebugScrollBase)o).mark;
        }

        static StackObject* CopyToStack_mark_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIDebugScrollBase)o).mark;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mark_1(ref object o, object v)
        {
            ((global::UIDebugScrollBase)o).mark = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_mark_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @mark = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIDebugScrollBase)o).mark = @mark;
            return ptr_of_this_method;
        }



    }
}
