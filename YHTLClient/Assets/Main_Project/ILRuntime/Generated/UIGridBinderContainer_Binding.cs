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
    unsafe class UIGridBinderContainer_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIGridBinderContainer);
            args = new Type[]{};
            method = type.GetMethod("get_MaxCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_MaxCount_0);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_MaxCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_MaxCount_1);

            field = type.GetField("controlList", flag);
            app.RegisterCLRFieldGetter(field, get_controlList_0);
            app.RegisterCLRFieldSetter(field, set_controlList_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_controlList_0, AssignFromStack_controlList_0);


        }


        static StackObject* get_MaxCount_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIGridBinderContainer instance_of_this_method = (global::UIGridBinderContainer)typeof(global::UIGridBinderContainer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.MaxCount;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* set_MaxCount_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIGridBinderContainer instance_of_this_method = (global::UIGridBinderContainer)typeof(global::UIGridBinderContainer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.MaxCount = value;

            return __ret;
        }


        static object get_controlList_0(ref object o)
        {
            return ((global::UIGridBinderContainer)o).controlList;
        }

        static StackObject* CopyToStack_controlList_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIGridBinderContainer)o).controlList;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_controlList_0(ref object o, object v)
        {
            ((global::UIGridBinderContainer)o).controlList = (System.Collections.Generic.List<global::ScriptBinder>)v;
        }

        static StackObject* AssignFromStack_controlList_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<global::ScriptBinder> @controlList = (System.Collections.Generic.List<global::ScriptBinder>)typeof(System.Collections.Generic.List<global::ScriptBinder>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIGridBinderContainer)o).controlList = @controlList;
            return ptr_of_this_method;
        }



    }
}
