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
    unsafe class CoroutineManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::CoroutineManager);
            args = new Type[]{typeof(System.Collections.IEnumerator)};
            method = type.GetMethod("DoCoroutine", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoCoroutine_0);
            args = new Type[]{};
            method = type.GetMethod("StopAllCoroutine", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopAllCoroutine_1);
            args = new Type[]{typeof(UnityEngine.Coroutine)};
            method = type.GetMethod("StopCoroutine", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopCoroutine_2);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("StopCoroutine", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopCoroutine_3);
            args = new Type[]{typeof(System.String), typeof(System.Object)};
            method = type.GetMethod("DoCoroutine", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DoCoroutine_4);


        }


        static StackObject* DoCoroutine_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.IEnumerator @routine = (System.Collections.IEnumerator)typeof(System.Collections.IEnumerator).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::CoroutineManager.DoCoroutine(@routine);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* StopAllCoroutine_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            global::CoroutineManager.StopAllCoroutine();

            return __ret;
        }

        static StackObject* StopCoroutine_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Coroutine @coroutine = (UnityEngine.Coroutine)typeof(UnityEngine.Coroutine).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::CoroutineManager.StopCoroutine(@coroutine);

            return __ret;
        }

        static StackObject* StopCoroutine_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @methodName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::CoroutineManager.StopCoroutine(@methodName);

            return __ret;
        }

        static StackObject* DoCoroutine_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @value = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @enumerator = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::CoroutineManager.DoCoroutine(@enumerator, @value);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
