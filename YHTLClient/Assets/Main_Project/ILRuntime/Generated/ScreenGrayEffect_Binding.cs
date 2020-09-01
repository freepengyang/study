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
    unsafe class ScreenGrayEffect_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::ScreenGrayEffect);

            field = type.GetField("grayScaleAmount", flag);
            app.RegisterCLRFieldGetter(field, get_grayScaleAmount_0);
            app.RegisterCLRFieldSetter(field, set_grayScaleAmount_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_grayScaleAmount_0, AssignFromStack_grayScaleAmount_0);


        }



        static object get_grayScaleAmount_0(ref object o)
        {
            return ((global::ScreenGrayEffect)o).grayScaleAmount;
        }

        static StackObject* CopyToStack_grayScaleAmount_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::ScreenGrayEffect)o).grayScaleAmount;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_grayScaleAmount_0(ref object o, object v)
        {
            ((global::ScreenGrayEffect)o).grayScaleAmount = (System.Single)v;
        }

        static StackObject* AssignFromStack_grayScaleAmount_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @grayScaleAmount = *(float*)&ptr_of_this_method->Value;
            ((global::ScreenGrayEffect)o).grayScaleAmount = @grayScaleAmount;
            return ptr_of_this_method;
        }



    }
}
