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
    unsafe class UICamera_Binding_MouseOrTouch_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UICamera.MouseOrTouch);

            field = type.GetField("totalDelta", flag);
            app.RegisterCLRFieldGetter(field, get_totalDelta_0);
            app.RegisterCLRFieldSetter(field, set_totalDelta_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_totalDelta_0, AssignFromStack_totalDelta_0);
            field = type.GetField("pos", flag);
            app.RegisterCLRFieldGetter(field, get_pos_1);
            app.RegisterCLRFieldSetter(field, set_pos_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_pos_1, AssignFromStack_pos_1);


        }



        static object get_totalDelta_0(ref object o)
        {
            return ((global::UICamera.MouseOrTouch)o).totalDelta;
        }

        static StackObject* CopyToStack_totalDelta_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UICamera.MouseOrTouch)o).totalDelta;
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static void set_totalDelta_0(ref object o, object v)
        {
            ((global::UICamera.MouseOrTouch)o).totalDelta = (UnityEngine.Vector2)v;
        }

        static StackObject* AssignFromStack_totalDelta_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Vector2 @totalDelta = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @totalDelta, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @totalDelta = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }
            ((global::UICamera.MouseOrTouch)o).totalDelta = @totalDelta;
            return ptr_of_this_method;
        }

        static object get_pos_1(ref object o)
        {
            return ((global::UICamera.MouseOrTouch)o).pos;
        }

        static StackObject* CopyToStack_pos_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UICamera.MouseOrTouch)o).pos;
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static void set_pos_1(ref object o, object v)
        {
            ((global::UICamera.MouseOrTouch)o).pos = (UnityEngine.Vector2)v;
        }

        static StackObject* AssignFromStack_pos_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Vector2 @pos = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @pos = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }
            ((global::UICamera.MouseOrTouch)o).pos = @pos;
            return ptr_of_this_method;
        }



    }
}
