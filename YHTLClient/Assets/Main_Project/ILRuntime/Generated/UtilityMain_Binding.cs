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
    unsafe class UtilityMain_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::UtilityMain);
            args = new Type[]{typeof(global::CSMisc.Dot2), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("IsInViewRange", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsInViewRange_0);


        }


        static StackObject* IsInViewRange_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @deltaY = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @deltaX = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::CSMisc.Dot2 @v = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @v, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @v = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }


            var result_of_this_method = global::UtilityMain.IsInViewRange(@v, @deltaX, @deltaY);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }



    }
}
