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
    unsafe class UIGridContainer_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIGridContainer);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_MaxCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_MaxCount_0);
            args = new Type[]{};
            method = type.GetMethod("get_MaxCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_MaxCount_1);
            args = new Type[]{};
            method = type.GetMethod("get_CellHeight", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_CellHeight_2);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_MaxPerLine", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_MaxPerLine_3);
            args = new Type[]{};
            method = type.GetMethod("get_CellWidth", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_CellWidth_4);
            args = new Type[]{};
            method = type.GetMethod("get_MaxPerLine", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_MaxPerLine_5);
            args = new Type[]{typeof(System.Int32), typeof(System.Action<UnityEngine.GameObject, System.Int32>)};
            method = type.GetMethod("BindAsync", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, BindAsync_6);
            args = new Type[]{};
            method = type.GetMethod("get_arrangement", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_arrangement_7);
            args = new Type[]{};
            method = type.GetMethod("get_RestoreList", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_RestoreList_8);

            field = type.GetField("controlList", flag);
            app.RegisterCLRFieldGetter(field, get_controlList_0);
            app.RegisterCLRFieldSetter(field, set_controlList_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_controlList_0, AssignFromStack_controlList_0);
            field = type.GetField("controlTemplate", flag);
            app.RegisterCLRFieldGetter(field, get_controlTemplate_1);
            app.RegisterCLRFieldSetter(field, set_controlTemplate_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_controlTemplate_1, AssignFromStack_controlTemplate_1);


        }


        static StackObject* set_MaxCount_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIGridContainer instance_of_this_method = (global::UIGridContainer)typeof(global::UIGridContainer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.MaxCount = value;

            return __ret;
        }

        static StackObject* get_MaxCount_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIGridContainer instance_of_this_method = (global::UIGridContainer)typeof(global::UIGridContainer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.MaxCount;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* get_CellHeight_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIGridContainer instance_of_this_method = (global::UIGridContainer)typeof(global::UIGridContainer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.CellHeight;

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* set_MaxPerLine_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIGridContainer instance_of_this_method = (global::UIGridContainer)typeof(global::UIGridContainer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.MaxPerLine = value;

            return __ret;
        }

        static StackObject* get_CellWidth_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIGridContainer instance_of_this_method = (global::UIGridContainer)typeof(global::UIGridContainer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.CellWidth;

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* get_MaxPerLine_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIGridContainer instance_of_this_method = (global::UIGridContainer)typeof(global::UIGridContainer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.MaxPerLine;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* BindAsync_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<UnityEngine.GameObject, System.Int32> @onItemVisible = (System.Action<UnityEngine.GameObject, System.Int32>)typeof(System.Action<UnityEngine.GameObject, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @count = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::UIGridContainer instance_of_this_method = (global::UIGridContainer)typeof(global::UIGridContainer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.BindAsync(@count, @onItemVisible);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_arrangement_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIGridContainer instance_of_this_method = (global::UIGridContainer)typeof(global::UIGridContainer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.arrangement;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_RestoreList_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIGridContainer instance_of_this_method = (global::UIGridContainer)typeof(global::UIGridContainer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.RestoreList;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_controlList_0(ref object o)
        {
            return ((global::UIGridContainer)o).controlList;
        }

        static StackObject* CopyToStack_controlList_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIGridContainer)o).controlList;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_controlList_0(ref object o, object v)
        {
            ((global::UIGridContainer)o).controlList = (System.Collections.Generic.List<UnityEngine.GameObject>)v;
        }

        static StackObject* AssignFromStack_controlList_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<UnityEngine.GameObject> @controlList = (System.Collections.Generic.List<UnityEngine.GameObject>)typeof(System.Collections.Generic.List<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIGridContainer)o).controlList = @controlList;
            return ptr_of_this_method;
        }

        static object get_controlTemplate_1(ref object o)
        {
            return ((global::UIGridContainer)o).controlTemplate;
        }

        static StackObject* CopyToStack_controlTemplate_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIGridContainer)o).controlTemplate;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_controlTemplate_1(ref object o, object v)
        {
            ((global::UIGridContainer)o).controlTemplate = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_controlTemplate_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @controlTemplate = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIGridContainer)o).controlTemplate = @controlTemplate;
            return ptr_of_this_method;
        }



    }
}
