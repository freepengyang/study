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
    unsafe class CSOrgan_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSOrgan);

            field = type.GetField("GoTrans", flag);
            app.RegisterCLRFieldGetter(field, get_GoTrans_0);
            app.RegisterCLRFieldSetter(field, set_GoTrans_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_GoTrans_0, AssignFromStack_GoTrans_0);
            field = type.GetField("Go", flag);
            app.RegisterCLRFieldGetter(field, get_Go_1);
            app.RegisterCLRFieldSetter(field, set_Go_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_Go_1, AssignFromStack_Go_1);


        }



        static object get_GoTrans_0(ref object o)
        {
            return ((global::CSOrgan)o).GoTrans;
        }

        static StackObject* CopyToStack_GoTrans_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSOrgan)o).GoTrans;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_GoTrans_0(ref object o, object v)
        {
            ((global::CSOrgan)o).GoTrans = (UnityEngine.Transform)v;
        }

        static StackObject* AssignFromStack_GoTrans_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Transform @GoTrans = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSOrgan)o).GoTrans = @GoTrans;
            return ptr_of_this_method;
        }

        static object get_Go_1(ref object o)
        {
            return ((global::CSOrgan)o).Go;
        }

        static StackObject* CopyToStack_Go_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSOrgan)o).Go;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Go_1(ref object o, object v)
        {
            ((global::CSOrgan)o).Go = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_Go_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @Go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSOrgan)o).Go = @Go;
            return ptr_of_this_method;
        }



    }
}
