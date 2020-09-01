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
    unsafe class UIDragScrollView_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIDragScrollView);

            field = type.GetField("scrollView", flag);
            app.RegisterCLRFieldGetter(field, get_scrollView_0);
            app.RegisterCLRFieldSetter(field, set_scrollView_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_scrollView_0, AssignFromStack_scrollView_0);


        }



        static object get_scrollView_0(ref object o)
        {
            return ((global::UIDragScrollView)o).scrollView;
        }

        static StackObject* CopyToStack_scrollView_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIDragScrollView)o).scrollView;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_scrollView_0(ref object o, object v)
        {
            ((global::UIDragScrollView)o).scrollView = (global::UIScrollView)v;
        }

        static StackObject* AssignFromStack_scrollView_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UIScrollView @scrollView = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIDragScrollView)o).scrollView = @scrollView;
            return ptr_of_this_method;
        }



    }
}
