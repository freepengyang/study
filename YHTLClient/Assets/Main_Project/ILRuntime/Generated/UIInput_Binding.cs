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
    unsafe class UIInput_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIInput);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_value", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_value_0);
            args = new Type[]{};
            method = type.GetMethod("get_value", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_value_1);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_SetValue", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_SetValue_2);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("InsertText", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InsertText_3);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_isSelected", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_isSelected_4);

            field = type.GetField("onChange", flag);
            app.RegisterCLRFieldGetter(field, get_onChange_0);
            app.RegisterCLRFieldSetter(field, set_onChange_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onChange_0, AssignFromStack_onChange_0);
            field = type.GetField("onValidate", flag);
            app.RegisterCLRFieldGetter(field, get_onValidate_1);
            app.RegisterCLRFieldSetter(field, set_onValidate_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onValidate_1, AssignFromStack_onValidate_1);
            field = type.GetField("activeTextColor", flag);
            app.RegisterCLRFieldGetter(field, get_activeTextColor_2);
            app.RegisterCLRFieldSetter(field, set_activeTextColor_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_activeTextColor_2, AssignFromStack_activeTextColor_2);
            field = type.GetField("label", flag);
            app.RegisterCLRFieldGetter(field, get_label_3);
            app.RegisterCLRFieldSetter(field, set_label_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_label_3, AssignFromStack_label_3);
            field = type.GetField("OnReachLimit", flag);
            app.RegisterCLRFieldGetter(field, get_OnReachLimit_4);
            app.RegisterCLRFieldSetter(field, set_OnReachLimit_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnReachLimit_4, AssignFromStack_OnReachLimit_4);
            field = type.GetField("selectAllTextOnFocus", flag);
            app.RegisterCLRFieldGetter(field, get_selectAllTextOnFocus_5);
            app.RegisterCLRFieldSetter(field, set_selectAllTextOnFocus_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_selectAllTextOnFocus_5, AssignFromStack_selectAllTextOnFocus_5);


        }


        static StackObject* set_value_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIInput instance_of_this_method = (global::UIInput)typeof(global::UIInput).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
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
            global::UIInput instance_of_this_method = (global::UIInput)typeof(global::UIInput).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.value;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* set_SetValue_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIInput instance_of_this_method = (global::UIInput)typeof(global::UIInput).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetValue = value;

            return __ret;
        }

        static StackObject* InsertText_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @str = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIInput instance_of_this_method = (global::UIInput)typeof(global::UIInput).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InsertText(@str);

            return __ret;
        }

        static StackObject* set_isSelected_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIInput instance_of_this_method = (global::UIInput)typeof(global::UIInput).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.isSelected = value;

            return __ret;
        }


        static object get_onChange_0(ref object o)
        {
            return ((global::UIInput)o).onChange;
        }

        static StackObject* CopyToStack_onChange_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIInput)o).onChange;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onChange_0(ref object o, object v)
        {
            ((global::UIInput)o).onChange = (System.Collections.Generic.List<global::EventDelegate>)v;
        }

        static StackObject* AssignFromStack_onChange_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<global::EventDelegate> @onChange = (System.Collections.Generic.List<global::EventDelegate>)typeof(System.Collections.Generic.List<global::EventDelegate>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIInput)o).onChange = @onChange;
            return ptr_of_this_method;
        }

        static object get_onValidate_1(ref object o)
        {
            return ((global::UIInput)o).onValidate;
        }

        static StackObject* CopyToStack_onValidate_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIInput)o).onValidate;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onValidate_1(ref object o, object v)
        {
            ((global::UIInput)o).onValidate = (global::UIInput.OnValidate)v;
        }

        static StackObject* AssignFromStack_onValidate_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UIInput.OnValidate @onValidate = (global::UIInput.OnValidate)typeof(global::UIInput.OnValidate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIInput)o).onValidate = @onValidate;
            return ptr_of_this_method;
        }

        static object get_activeTextColor_2(ref object o)
        {
            return ((global::UIInput)o).activeTextColor;
        }

        static StackObject* CopyToStack_activeTextColor_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIInput)o).activeTextColor;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_activeTextColor_2(ref object o, object v)
        {
            ((global::UIInput)o).activeTextColor = (UnityEngine.Color)v;
        }

        static StackObject* AssignFromStack_activeTextColor_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Color @activeTextColor = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIInput)o).activeTextColor = @activeTextColor;
            return ptr_of_this_method;
        }

        static object get_label_3(ref object o)
        {
            return ((global::UIInput)o).label;
        }

        static StackObject* CopyToStack_label_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIInput)o).label;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_label_3(ref object o, object v)
        {
            ((global::UIInput)o).label = (global::UILabel)v;
        }

        static StackObject* AssignFromStack_label_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UILabel @label = (global::UILabel)typeof(global::UILabel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIInput)o).label = @label;
            return ptr_of_this_method;
        }

        static object get_OnReachLimit_4(ref object o)
        {
            return ((global::UIInput)o).OnReachLimit;
        }

        static StackObject* CopyToStack_OnReachLimit_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIInput)o).OnReachLimit;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnReachLimit_4(ref object o, object v)
        {
            ((global::UIInput)o).OnReachLimit = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnReachLimit_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnReachLimit = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIInput)o).OnReachLimit = @OnReachLimit;
            return ptr_of_this_method;
        }

        static object get_selectAllTextOnFocus_5(ref object o)
        {
            return ((global::UIInput)o).selectAllTextOnFocus;
        }

        static StackObject* CopyToStack_selectAllTextOnFocus_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIInput)o).selectAllTextOnFocus;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_selectAllTextOnFocus_5(ref object o, object v)
        {
            ((global::UIInput)o).selectAllTextOnFocus = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_selectAllTextOnFocus_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @selectAllTextOnFocus = ptr_of_this_method->Value == 1;
            ((global::UIInput)o).selectAllTextOnFocus = @selectAllTextOnFocus;
            return ptr_of_this_method;
        }



    }
}
