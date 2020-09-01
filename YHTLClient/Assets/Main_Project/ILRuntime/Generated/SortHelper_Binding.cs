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
    unsafe class SortHelper_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::SortHelper);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("GetComparersList", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetComparersList_0);
            args = new Type[]{typeof(System.Collections.Generic.List<global::SortHelper.FixedCompare>), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("AddCompare", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddCompare_1);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("GetSortHandle", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetSortHandle_2);
            args = new Type[]{typeof(System.Collections.Generic.List<global::SortHelper.SortHandle>), typeof(System.Collections.Generic.List<global::SortHelper.FixedCompare>)};
            method = type.GetMethod("Sort", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Sort_3);
            args = new Type[]{typeof(System.Collections.Generic.List<global::SortHelper.SortHandle>)};
            method = type.GetMethod("OnRecycle", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OnRecycle_4);
            args = new Type[]{typeof(System.Collections.Generic.List<global::SortHelper.FixedCompare>)};
            method = type.GetMethod("OnRecycle", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OnRecycle_5);


        }


        static StackObject* GetComparersList_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @capacity = ptr_of_this_method->Value;


            var result_of_this_method = global::SortHelper.GetComparersList(@capacity);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AddCompare_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @idx = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @compareType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Collections.Generic.List<global::SortHelper.FixedCompare> @fixedComparers = (System.Collections.Generic.List<global::SortHelper.FixedCompare>)typeof(System.Collections.Generic.List<global::SortHelper.FixedCompare>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::SortHelper.AddCompare(@fixedComparers, @compareType, @idx);

            return __ret;
        }

        static StackObject* GetSortHandle_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @count = ptr_of_this_method->Value;


            var result_of_this_method = global::SortHelper.GetSortHandle(@count);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Sort_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<global::SortHelper.FixedCompare> @comparers = (System.Collections.Generic.List<global::SortHelper.FixedCompare>)typeof(System.Collections.Generic.List<global::SortHelper.FixedCompare>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<global::SortHelper.SortHandle> @mBuffer = (System.Collections.Generic.List<global::SortHelper.SortHandle>)typeof(System.Collections.Generic.List<global::SortHelper.SortHandle>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::SortHelper.Sort(@mBuffer, @comparers);

            return __ret;
        }

        static StackObject* OnRecycle_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<global::SortHelper.SortHandle> @handleList = (System.Collections.Generic.List<global::SortHelper.SortHandle>)typeof(System.Collections.Generic.List<global::SortHelper.SortHandle>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::SortHelper.OnRecycle(@handleList);

            return __ret;
        }

        static StackObject* OnRecycle_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<global::SortHelper.FixedCompare> @fixedComparers = (System.Collections.Generic.List<global::SortHelper.FixedCompare>)typeof(System.Collections.Generic.List<global::SortHelper.FixedCompare>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::SortHelper.OnRecycle(@fixedComparers);

            return __ret;
        }



    }
}
