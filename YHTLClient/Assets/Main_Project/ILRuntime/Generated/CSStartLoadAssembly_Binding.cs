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
    unsafe class CSStartLoadAssembly_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSStartLoadAssembly);
            args = new Type[]{};
            method = type.GetMethod("get_Sington", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Sington_0);

            field = type.GetField("IsMonsterHeadShowConfigID", flag);
            app.RegisterCLRFieldGetter(field, get_IsMonsterHeadShowConfigID_0);
            app.RegisterCLRFieldSetter(field, set_IsMonsterHeadShowConfigID_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_IsMonsterHeadShowConfigID_0, AssignFromStack_IsMonsterHeadShowConfigID_0);


        }


        static StackObject* get_Sington_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::CSStartLoadAssembly.Sington;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_IsMonsterHeadShowConfigID_0(ref object o)
        {
            return ((global::CSStartLoadAssembly)o).IsMonsterHeadShowConfigID;
        }

        static StackObject* CopyToStack_IsMonsterHeadShowConfigID_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSStartLoadAssembly)o).IsMonsterHeadShowConfigID;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_IsMonsterHeadShowConfigID_0(ref object o, object v)
        {
            ((global::CSStartLoadAssembly)o).IsMonsterHeadShowConfigID = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_IsMonsterHeadShowConfigID_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @IsMonsterHeadShowConfigID = ptr_of_this_method->Value == 1;
            ((global::CSStartLoadAssembly)o).IsMonsterHeadShowConfigID = @IsMonsterHeadShowConfigID;
            return ptr_of_this_method;
        }



    }
}
