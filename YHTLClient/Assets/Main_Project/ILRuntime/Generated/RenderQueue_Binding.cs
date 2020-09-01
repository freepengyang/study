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
    unsafe class RenderQueue_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::RenderQueue);

            field = type.GetField("sortingStage", flag);
            app.RegisterCLRFieldGetter(field, get_sortingStage_0);
            app.RegisterCLRFieldSetter(field, set_sortingStage_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_sortingStage_0, AssignFromStack_sortingStage_0);


        }



        static object get_sortingStage_0(ref object o)
        {
            return ((global::RenderQueue)o).sortingStage;
        }

        static StackObject* CopyToStack_sortingStage_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::RenderQueue)o).sortingStage;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_sortingStage_0(ref object o, object v)
        {
            ((global::RenderQueue)o).sortingStage = (System.Int32)v;
        }

        static StackObject* AssignFromStack_sortingStage_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @sortingStage = ptr_of_this_method->Value;
            ((global::RenderQueue)o).sortingStage = @sortingStage;
            return ptr_of_this_method;
        }



    }
}
