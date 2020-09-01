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
    unsafe class CSAction_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSAction);

            field = type.GetField("Direction", flag);
            app.RegisterCLRFieldGetter(field, get_Direction_0);
            app.RegisterCLRFieldSetter(field, set_Direction_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Direction_0, AssignFromStack_Direction_0);


        }



        static object get_Direction_0(ref object o)
        {
            return ((global::CSAction)o).Direction;
        }

        static StackObject* CopyToStack_Direction_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSAction)o).Direction;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_Direction_0(ref object o, object v)
        {
            ((global::CSAction)o).Direction = (System.Int32)v;
        }

        static StackObject* AssignFromStack_Direction_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @Direction = ptr_of_this_method->Value;
            ((global::CSAction)o).Direction = @Direction;
            return ptr_of_this_method;
        }



    }
}
