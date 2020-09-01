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
    unsafe class SFOut_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::SFOut);

            field = type.GetField("IsLoadLocalRes", flag);
            app.RegisterCLRFieldGetter(field, get_IsLoadLocalRes_0);
            app.RegisterCLRFieldSetter(field, set_IsLoadLocalRes_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_IsLoadLocalRes_0, AssignFromStack_IsLoadLocalRes_0);


        }



        static object get_IsLoadLocalRes_0(ref object o)
        {
            return global::SFOut.IsLoadLocalRes;
        }

        static StackObject* CopyToStack_IsLoadLocalRes_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::SFOut.IsLoadLocalRes;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_IsLoadLocalRes_0(ref object o, object v)
        {
            global::SFOut.IsLoadLocalRes = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_IsLoadLocalRes_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @IsLoadLocalRes = ptr_of_this_method->Value == 1;
            global::SFOut.IsLoadLocalRes = @IsLoadLocalRes;
            return ptr_of_this_method;
        }



    }
}
