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
    unsafe class UIToggle_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIToggle);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_value", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_value_0);
            args = new Type[]{};
            method = type.GetMethod("get_value", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_value_1);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("Set", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Set_2);

            field = type.GetField("onChange", flag);
            app.RegisterCLRFieldGetter(field, get_onChange_0);
            app.RegisterCLRFieldSetter(field, set_onChange_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onChange_0, AssignFromStack_onChange_0);
            field = type.GetField("current", flag);
            app.RegisterCLRFieldGetter(field, get_current_1);
            app.RegisterCLRFieldSetter(field, set_current_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_current_1, AssignFromStack_current_1);
            field = type.GetField("instantTween", flag);
            app.RegisterCLRFieldGetter(field, get_instantTween_2);
            app.RegisterCLRFieldSetter(field, set_instantTween_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_instantTween_2, AssignFromStack_instantTween_2);
            field = type.GetField("activeSprite", flag);
            app.RegisterCLRFieldGetter(field, get_activeSprite_3);
            app.RegisterCLRFieldSetter(field, set_activeSprite_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_activeSprite_3, AssignFromStack_activeSprite_3);
            field = type.GetField("optionCanBeNone", flag);
            app.RegisterCLRFieldGetter(field, get_optionCanBeNone_4);
            app.RegisterCLRFieldSetter(field, set_optionCanBeNone_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_optionCanBeNone_4, AssignFromStack_optionCanBeNone_4);


        }


        static StackObject* set_value_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIToggle instance_of_this_method = (global::UIToggle)typeof(global::UIToggle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.value = value;

            return __ret;
        }

        static StackObject* get_value_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIToggle instance_of_this_method = (global::UIToggle)typeof(global::UIToggle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.value;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* Set_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @state = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIToggle instance_of_this_method = (global::UIToggle)typeof(global::UIToggle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Set(@state);

            return __ret;
        }


        static object get_onChange_0(ref object o)
        {
            return ((global::UIToggle)o).onChange;
        }

        static StackObject* CopyToStack_onChange_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIToggle)o).onChange;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onChange_0(ref object o, object v)
        {
            ((global::UIToggle)o).onChange = (System.Collections.Generic.List<global::EventDelegate>)v;
        }

        static StackObject* AssignFromStack_onChange_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<global::EventDelegate> @onChange = (System.Collections.Generic.List<global::EventDelegate>)typeof(System.Collections.Generic.List<global::EventDelegate>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIToggle)o).onChange = @onChange;
            return ptr_of_this_method;
        }

        static object get_current_1(ref object o)
        {
            return global::UIToggle.current;
        }

        static StackObject* CopyToStack_current_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::UIToggle.current;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_current_1(ref object o, object v)
        {
            global::UIToggle.current = (global::UIToggle)v;
        }

        static StackObject* AssignFromStack_current_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UIToggle @current = (global::UIToggle)typeof(global::UIToggle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::UIToggle.current = @current;
            return ptr_of_this_method;
        }

        static object get_instantTween_2(ref object o)
        {
            return ((global::UIToggle)o).instantTween;
        }

        static StackObject* CopyToStack_instantTween_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIToggle)o).instantTween;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_instantTween_2(ref object o, object v)
        {
            ((global::UIToggle)o).instantTween = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_instantTween_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @instantTween = ptr_of_this_method->Value == 1;
            ((global::UIToggle)o).instantTween = @instantTween;
            return ptr_of_this_method;
        }

        static object get_activeSprite_3(ref object o)
        {
            return ((global::UIToggle)o).activeSprite;
        }

        static StackObject* CopyToStack_activeSprite_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIToggle)o).activeSprite;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_activeSprite_3(ref object o, object v)
        {
            ((global::UIToggle)o).activeSprite = (global::UIWidget)v;
        }

        static StackObject* AssignFromStack_activeSprite_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UIWidget @activeSprite = (global::UIWidget)typeof(global::UIWidget).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIToggle)o).activeSprite = @activeSprite;
            return ptr_of_this_method;
        }

        static object get_optionCanBeNone_4(ref object o)
        {
            return ((global::UIToggle)o).optionCanBeNone;
        }

        static StackObject* CopyToStack_optionCanBeNone_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIToggle)o).optionCanBeNone;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_optionCanBeNone_4(ref object o, object v)
        {
            ((global::UIToggle)o).optionCanBeNone = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_optionCanBeNone_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @optionCanBeNone = ptr_of_this_method->Value == 1;
            ((global::UIToggle)o).optionCanBeNone = @optionCanBeNone;
            return ptr_of_this_method;
        }



    }
}