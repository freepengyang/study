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
    unsafe class SpringPanel_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::SpringPanel);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(UnityEngine.Vector3), typeof(System.Single)};
            method = type.GetMethod("Begin", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Begin_0);

            field = type.GetField("onFinished", flag);
            app.RegisterCLRFieldGetter(field, get_onFinished_0);
            app.RegisterCLRFieldSetter(field, set_onFinished_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onFinished_0, AssignFromStack_onFinished_0);
            field = type.GetField("onMovement", flag);
            app.RegisterCLRFieldGetter(field, get_onMovement_1);
            app.RegisterCLRFieldSetter(field, set_onMovement_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onMovement_1, AssignFromStack_onMovement_1);


        }


        static StackObject* Begin_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @strength = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Vector3 @pos = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @pos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::SpringPanel.Begin(@go, @pos, @strength);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_onFinished_0(ref object o)
        {
            return ((global::SpringPanel)o).onFinished;
        }

        static StackObject* CopyToStack_onFinished_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::SpringPanel)o).onFinished;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onFinished_0(ref object o, object v)
        {
            ((global::SpringPanel)o).onFinished = (System.Action)v;
        }

        static StackObject* AssignFromStack_onFinished_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @onFinished = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::SpringPanel)o).onFinished = @onFinished;
            return ptr_of_this_method;
        }

        static object get_onMovement_1(ref object o)
        {
            return ((global::SpringPanel)o).onMovement;
        }

        static StackObject* CopyToStack_onMovement_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::SpringPanel)o).onMovement;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onMovement_1(ref object o, object v)
        {
            ((global::SpringPanel)o).onMovement = (System.Action<UnityEngine.Vector3>)v;
        }

        static StackObject* AssignFromStack_onMovement_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.Vector3> @onMovement = (System.Action<UnityEngine.Vector3>)typeof(System.Action<UnityEngine.Vector3>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::SpringPanel)o).onMovement = @onMovement;
            return ptr_of_this_method;
        }



    }
}
