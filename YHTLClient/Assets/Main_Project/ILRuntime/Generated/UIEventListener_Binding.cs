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
    unsafe class UIEventListener_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIEventListener);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Object)};
            method = type.GetMethod("Get", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Get_0);
            args = new Type[]{typeof(UnityEngine.GameObject)};
            method = type.GetMethod("Get", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Get_1);

            field = type.GetField("onClick", flag);
            app.RegisterCLRFieldGetter(field, get_onClick_0);
            app.RegisterCLRFieldSetter(field, set_onClick_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onClick_0, AssignFromStack_onClick_0);
            field = type.GetField("parameter", flag);
            app.RegisterCLRFieldGetter(field, get_parameter_1);
            app.RegisterCLRFieldSetter(field, set_parameter_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_parameter_1, AssignFromStack_parameter_1);
            field = type.GetField("onDragStart", flag);
            app.RegisterCLRFieldGetter(field, get_onDragStart_2);
            app.RegisterCLRFieldSetter(field, set_onDragStart_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDragStart_2, AssignFromStack_onDragStart_2);
            field = type.GetField("onDrag", flag);
            app.RegisterCLRFieldGetter(field, get_onDrag_3);
            app.RegisterCLRFieldSetter(field, set_onDrag_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDrag_3, AssignFromStack_onDrag_3);
            field = type.GetField("onDragEnd", flag);
            app.RegisterCLRFieldGetter(field, get_onDragEnd_4);
            app.RegisterCLRFieldSetter(field, set_onDragEnd_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDragEnd_4, AssignFromStack_onDragEnd_4);
            field = type.GetField("onKeepPress", flag);
            app.RegisterCLRFieldGetter(field, get_onKeepPress_5);
            app.RegisterCLRFieldSetter(field, set_onKeepPress_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_onKeepPress_5, AssignFromStack_onKeepPress_5);
            field = type.GetField("onPress", flag);
            app.RegisterCLRFieldGetter(field, get_onPress_6);
            app.RegisterCLRFieldSetter(field, set_onPress_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_onPress_6, AssignFromStack_onPress_6);
            field = type.GetField("onSelect", flag);
            app.RegisterCLRFieldGetter(field, get_onSelect_7);
            app.RegisterCLRFieldSetter(field, set_onSelect_7);
            app.RegisterCLRFieldBinding(field, CopyToStack_onSelect_7, AssignFromStack_onSelect_7);
            field = type.GetField("ClickIntervalTime", flag);
            app.RegisterCLRFieldGetter(field, get_ClickIntervalTime_8);
            app.RegisterCLRFieldSetter(field, set_ClickIntervalTime_8);
            app.RegisterCLRFieldBinding(field, CopyToStack_ClickIntervalTime_8, AssignFromStack_ClickIntervalTime_8);
            field = type.GetField("onDoubleClick", flag);
            app.RegisterCLRFieldGetter(field, get_onDoubleClick_9);
            app.RegisterCLRFieldSetter(field, set_onDoubleClick_9);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDoubleClick_9, AssignFromStack_onDoubleClick_9);
            field = type.GetField("onDrop", flag);
            app.RegisterCLRFieldGetter(field, get_onDrop_10);
            app.RegisterCLRFieldSetter(field, set_onDrop_10);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDrop_10, AssignFromStack_onDrop_10);

            app.RegisterCLRCreateArrayInstance(type, s => new global::UIEventListener[s]);


        }


        static StackObject* Get_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @obj = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::UIEventListener.Get(@go, @obj);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Get_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::UIEventListener.Get(@go);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_onClick_0(ref object o)
        {
            return ((global::UIEventListener)o).onClick;
        }

        static StackObject* CopyToStack_onClick_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIEventListener)o).onClick;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onClick_0(ref object o, object v)
        {
            ((global::UIEventListener)o).onClick = (System.Action<UnityEngine.GameObject>)v;
        }

        static StackObject* AssignFromStack_onClick_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject> @onClick = (System.Action<UnityEngine.GameObject>)typeof(System.Action<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIEventListener)o).onClick = @onClick;
            return ptr_of_this_method;
        }

        static object get_parameter_1(ref object o)
        {
            return ((global::UIEventListener)o).parameter;
        }

        static StackObject* CopyToStack_parameter_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIEventListener)o).parameter;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static void set_parameter_1(ref object o, object v)
        {
            ((global::UIEventListener)o).parameter = (System.Object)v;
        }

        static StackObject* AssignFromStack_parameter_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Object @parameter = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIEventListener)o).parameter = @parameter;
            return ptr_of_this_method;
        }

        static object get_onDragStart_2(ref object o)
        {
            return ((global::UIEventListener)o).onDragStart;
        }

        static StackObject* CopyToStack_onDragStart_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIEventListener)o).onDragStart;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDragStart_2(ref object o, object v)
        {
            ((global::UIEventListener)o).onDragStart = (System.Action<UnityEngine.GameObject>)v;
        }

        static StackObject* AssignFromStack_onDragStart_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject> @onDragStart = (System.Action<UnityEngine.GameObject>)typeof(System.Action<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIEventListener)o).onDragStart = @onDragStart;
            return ptr_of_this_method;
        }

        static object get_onDrag_3(ref object o)
        {
            return ((global::UIEventListener)o).onDrag;
        }

        static StackObject* CopyToStack_onDrag_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIEventListener)o).onDrag;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDrag_3(ref object o, object v)
        {
            ((global::UIEventListener)o).onDrag = (System.Action<UnityEngine.GameObject, UnityEngine.Vector2>)v;
        }

        static StackObject* AssignFromStack_onDrag_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject, UnityEngine.Vector2> @onDrag = (System.Action<UnityEngine.GameObject, UnityEngine.Vector2>)typeof(System.Action<UnityEngine.GameObject, UnityEngine.Vector2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIEventListener)o).onDrag = @onDrag;
            return ptr_of_this_method;
        }

        static object get_onDragEnd_4(ref object o)
        {
            return ((global::UIEventListener)o).onDragEnd;
        }

        static StackObject* CopyToStack_onDragEnd_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIEventListener)o).onDragEnd;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDragEnd_4(ref object o, object v)
        {
            ((global::UIEventListener)o).onDragEnd = (System.Action<UnityEngine.GameObject>)v;
        }

        static StackObject* AssignFromStack_onDragEnd_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject> @onDragEnd = (System.Action<UnityEngine.GameObject>)typeof(System.Action<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIEventListener)o).onDragEnd = @onDragEnd;
            return ptr_of_this_method;
        }

        static object get_onKeepPress_5(ref object o)
        {
            return ((global::UIEventListener)o).onKeepPress;
        }

        static StackObject* CopyToStack_onKeepPress_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIEventListener)o).onKeepPress;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onKeepPress_5(ref object o, object v)
        {
            ((global::UIEventListener)o).onKeepPress = (System.Action<UnityEngine.GameObject>)v;
        }

        static StackObject* AssignFromStack_onKeepPress_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject> @onKeepPress = (System.Action<UnityEngine.GameObject>)typeof(System.Action<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIEventListener)o).onKeepPress = @onKeepPress;
            return ptr_of_this_method;
        }

        static object get_onPress_6(ref object o)
        {
            return ((global::UIEventListener)o).onPress;
        }

        static StackObject* CopyToStack_onPress_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIEventListener)o).onPress;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onPress_6(ref object o, object v)
        {
            ((global::UIEventListener)o).onPress = (System.Action<UnityEngine.GameObject, System.Boolean>)v;
        }

        static StackObject* AssignFromStack_onPress_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject, System.Boolean> @onPress = (System.Action<UnityEngine.GameObject, System.Boolean>)typeof(System.Action<UnityEngine.GameObject, System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIEventListener)o).onPress = @onPress;
            return ptr_of_this_method;
        }

        static object get_onSelect_7(ref object o)
        {
            return ((global::UIEventListener)o).onSelect;
        }

        static StackObject* CopyToStack_onSelect_7(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIEventListener)o).onSelect;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onSelect_7(ref object o, object v)
        {
            ((global::UIEventListener)o).onSelect = (System.Action<UnityEngine.GameObject, System.Boolean>)v;
        }

        static StackObject* AssignFromStack_onSelect_7(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject, System.Boolean> @onSelect = (System.Action<UnityEngine.GameObject, System.Boolean>)typeof(System.Action<UnityEngine.GameObject, System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIEventListener)o).onSelect = @onSelect;
            return ptr_of_this_method;
        }

        static object get_ClickIntervalTime_8(ref object o)
        {
            return ((global::UIEventListener)o).ClickIntervalTime;
        }

        static StackObject* CopyToStack_ClickIntervalTime_8(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIEventListener)o).ClickIntervalTime;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_ClickIntervalTime_8(ref object o, object v)
        {
            ((global::UIEventListener)o).ClickIntervalTime = (System.Single)v;
        }

        static StackObject* AssignFromStack_ClickIntervalTime_8(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @ClickIntervalTime = *(float*)&ptr_of_this_method->Value;
            ((global::UIEventListener)o).ClickIntervalTime = @ClickIntervalTime;
            return ptr_of_this_method;
        }

        static object get_onDoubleClick_9(ref object o)
        {
            return ((global::UIEventListener)o).onDoubleClick;
        }

        static StackObject* CopyToStack_onDoubleClick_9(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIEventListener)o).onDoubleClick;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDoubleClick_9(ref object o, object v)
        {
            ((global::UIEventListener)o).onDoubleClick = (System.Action<UnityEngine.GameObject>)v;
        }

        static StackObject* AssignFromStack_onDoubleClick_9(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject> @onDoubleClick = (System.Action<UnityEngine.GameObject>)typeof(System.Action<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIEventListener)o).onDoubleClick = @onDoubleClick;
            return ptr_of_this_method;
        }

        static object get_onDrop_10(ref object o)
        {
            return ((global::UIEventListener)o).onDrop;
        }

        static StackObject* CopyToStack_onDrop_10(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIEventListener)o).onDrop;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDrop_10(ref object o, object v)
        {
            ((global::UIEventListener)o).onDrop = (System.Action<UnityEngine.GameObject, UnityEngine.GameObject>)v;
        }

        static StackObject* AssignFromStack_onDrop_10(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.GameObject, UnityEngine.GameObject> @onDrop = (System.Action<UnityEngine.GameObject, UnityEngine.GameObject>)typeof(System.Action<UnityEngine.GameObject, UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIEventListener)o).onDrop = @onDrop;
            return ptr_of_this_method;
        }



    }
}
