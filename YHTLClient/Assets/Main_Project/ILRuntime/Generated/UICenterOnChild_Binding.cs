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
    unsafe class UICenterOnChild_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UICenterOnChild);
            args = new Type[]{typeof(UnityEngine.Transform)};
            method = type.GetMethod("CenterOn", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CenterOn_0);
            args = new Type[]{};
            method = type.GetMethod("get_centeredObject", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_centeredObject_1);

            field = type.GetField("nextPageThreshold", flag);
            app.RegisterCLRFieldGetter(field, get_nextPageThreshold_0);
            app.RegisterCLRFieldSetter(field, set_nextPageThreshold_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_nextPageThreshold_0, AssignFromStack_nextPageThreshold_0);
            field = type.GetField("onCenter", flag);
            app.RegisterCLRFieldGetter(field, get_onCenter_1);
            app.RegisterCLRFieldSetter(field, set_onCenter_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onCenter_1, AssignFromStack_onCenter_1);


        }


        static StackObject* CenterOn_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Transform @target = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UICenterOnChild instance_of_this_method = (global::UICenterOnChild)typeof(global::UICenterOnChild).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CenterOn(@target);

            return __ret;
        }

        static StackObject* get_centeredObject_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UICenterOnChild instance_of_this_method = (global::UICenterOnChild)typeof(global::UICenterOnChild).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.centeredObject;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_nextPageThreshold_0(ref object o)
        {
            return ((global::UICenterOnChild)o).nextPageThreshold;
        }

        static StackObject* CopyToStack_nextPageThreshold_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UICenterOnChild)o).nextPageThreshold;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_nextPageThreshold_0(ref object o, object v)
        {
            ((global::UICenterOnChild)o).nextPageThreshold = (System.Single)v;
        }

        static StackObject* AssignFromStack_nextPageThreshold_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @nextPageThreshold = *(float*)&ptr_of_this_method->Value;
            ((global::UICenterOnChild)o).nextPageThreshold = @nextPageThreshold;
            return ptr_of_this_method;
        }

        static object get_onCenter_1(ref object o)
        {
            return ((global::UICenterOnChild)o).onCenter;
        }

        static StackObject* CopyToStack_onCenter_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UICenterOnChild)o).onCenter;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onCenter_1(ref object o, object v)
        {
            ((global::UICenterOnChild)o).onCenter = (global::UICenterOnChild.OnCenterCallback)v;
        }

        static StackObject* AssignFromStack_onCenter_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UICenterOnChild.OnCenterCallback @onCenter = (global::UICenterOnChild.OnCenterCallback)typeof(global::UICenterOnChild.OnCenterCallback).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UICenterOnChild)o).onCenter = @onCenter;
            return ptr_of_this_method;
        }



    }
}
