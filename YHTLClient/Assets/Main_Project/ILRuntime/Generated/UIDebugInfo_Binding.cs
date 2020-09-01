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
    unsafe class UIDebugInfo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIDebugInfo);
            args = new Type[]{typeof(global::ELogToggleType), typeof(System.String), typeof(global::ELogColorType)};
            method = type.GetMethod("AddLog", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddLog_0);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("Clear", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Clear_1);

            field = type.GetField("isCheckDisableBoxCollider", flag);
            app.RegisterCLRFieldGetter(field, get_isCheckDisableBoxCollider_0);
            app.RegisterCLRFieldSetter(field, set_isCheckDisableBoxCollider_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_isCheckDisableBoxCollider_0, AssignFromStack_isCheckDisableBoxCollider_0);
            field = type.GetField("selectTogIndex", flag);
            app.RegisterCLRFieldGetter(field, get_selectTogIndex_1);
            app.RegisterCLRFieldSetter(field, set_selectTogIndex_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_selectTogIndex_1, AssignFromStack_selectTogIndex_1);
            field = type.GetField("togNameList", flag);
            app.RegisterCLRFieldGetter(field, get_togNameList_2);
            app.RegisterCLRFieldSetter(field, set_togNameList_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_togNameList_2, AssignFromStack_togNameList_2);
            field = type.GetField("popNameList", flag);
            app.RegisterCLRFieldGetter(field, get_popNameList_3);
            app.RegisterCLRFieldSetter(field, set_popNameList_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_popNameList_3, AssignFromStack_popNameList_3);
            field = type.GetField("selectPopIndex", flag);
            app.RegisterCLRFieldGetter(field, get_selectPopIndex_4);
            app.RegisterCLRFieldSetter(field, set_selectPopIndex_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_selectPopIndex_4, AssignFromStack_selectPopIndex_4);


        }


        static StackObject* AddLog_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::ELogColorType @colorType = (global::ELogColorType)typeof(global::ELogColorType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @log = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::ELogToggleType @type = (global::ELogToggleType)typeof(global::ELogToggleType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::UIDebugInfo.AddLog(@type, @log, @colorType);

            return __ret;
        }

        static StackObject* Clear_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @togType = ptr_of_this_method->Value;


            global::UIDebugInfo.Clear(@togType);

            return __ret;
        }


        static object get_isCheckDisableBoxCollider_0(ref object o)
        {
            return global::UIDebugInfo.isCheckDisableBoxCollider;
        }

        static StackObject* CopyToStack_isCheckDisableBoxCollider_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::UIDebugInfo.isCheckDisableBoxCollider;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isCheckDisableBoxCollider_0(ref object o, object v)
        {
            global::UIDebugInfo.isCheckDisableBoxCollider = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isCheckDisableBoxCollider_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isCheckDisableBoxCollider = ptr_of_this_method->Value == 1;
            global::UIDebugInfo.isCheckDisableBoxCollider = @isCheckDisableBoxCollider;
            return ptr_of_this_method;
        }

        static object get_selectTogIndex_1(ref object o)
        {
            return global::UIDebugInfo.selectTogIndex;
        }

        static StackObject* CopyToStack_selectTogIndex_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::UIDebugInfo.selectTogIndex;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_selectTogIndex_1(ref object o, object v)
        {
            global::UIDebugInfo.selectTogIndex = (System.Int32)v;
        }

        static StackObject* AssignFromStack_selectTogIndex_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @selectTogIndex = ptr_of_this_method->Value;
            global::UIDebugInfo.selectTogIndex = @selectTogIndex;
            return ptr_of_this_method;
        }

        static object get_togNameList_2(ref object o)
        {
            return global::UIDebugInfo.togNameList;
        }

        static StackObject* CopyToStack_togNameList_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::UIDebugInfo.togNameList;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_togNameList_2(ref object o, object v)
        {
            global::UIDebugInfo.togNameList = (System.Collections.Generic.List<System.String>)v;
        }

        static StackObject* AssignFromStack_togNameList_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<System.String> @togNameList = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::UIDebugInfo.togNameList = @togNameList;
            return ptr_of_this_method;
        }

        static object get_popNameList_3(ref object o)
        {
            return global::UIDebugInfo.popNameList;
        }

        static StackObject* CopyToStack_popNameList_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::UIDebugInfo.popNameList;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_popNameList_3(ref object o, object v)
        {
            global::UIDebugInfo.popNameList = (System.Collections.Generic.List<System.String>)v;
        }

        static StackObject* AssignFromStack_popNameList_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<System.String> @popNameList = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::UIDebugInfo.popNameList = @popNameList;
            return ptr_of_this_method;
        }

        static object get_selectPopIndex_4(ref object o)
        {
            return global::UIDebugInfo.selectPopIndex;
        }

        static StackObject* CopyToStack_selectPopIndex_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::UIDebugInfo.selectPopIndex;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_selectPopIndex_4(ref object o, object v)
        {
            global::UIDebugInfo.selectPopIndex = (System.Int32)v;
        }

        static StackObject* AssignFromStack_selectPopIndex_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @selectPopIndex = ptr_of_this_method->Value;
            global::UIDebugInfo.selectPopIndex = @selectPopIndex;
            return ptr_of_this_method;
        }



    }
}
