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
    unsafe class CSSkillTools_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::CSSkillTools);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32), typeof(System.Int32), typeof(global::CSMisc.Dot2), typeof(global::CSMisc.Dot2)};
            method = type.GetMethod("GetBestLaunchCoord", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetBestLaunchCoord_0);
            args = new Type[]{typeof(global::CSMisc.Dot2), typeof(global::CSMisc.Dot2), typeof(System.Int32)};
            method = type.GetMethod("IsCellDistance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsCellDistance_1);


        }


        static StackObject* GetBestLaunchCoord_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMisc.Dot2 @targetPos = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @targetPos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @targetPos = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSMisc.Dot2 @curPos = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @curPos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @curPos = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @effectRange = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int32 @range = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.Int32 @effectArea = ptr_of_this_method->Value;


            var result_of_this_method = global::CSSkillTools.GetBestLaunchCoord(@effectArea, @range, @effectRange, @curPos, @targetPos);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsCellDistance_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @range = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSMisc.Dot2 @targetPos = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @targetPos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @targetPos = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::CSMisc.Dot2 @curPos = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @curPos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @curPos = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }


            var result_of_this_method = global::CSSkillTools.IsCellDistance(@curPos, @targetPos, @range);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }



    }
}
