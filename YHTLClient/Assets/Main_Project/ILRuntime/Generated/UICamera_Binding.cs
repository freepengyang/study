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
    unsafe class UICamera_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UICamera);
            args = new Type[]{};
            method = type.GetMethod("get_selectedObject", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_selectedObject_0);
            args = new Type[]{};
            method = type.GetMethod("get_currentScheme", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_currentScheme_1);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("FindCameraForLayer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FindCameraForLayer_2);
            args = new Type[]{};
            method = type.GetMethod("get_cachedCamera", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_cachedCamera_3);
            args = new Type[]{typeof(UnityEngine.Vector3)};
            method = type.GetMethod("Raycast", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Raycast_4);

            field = type.GetField("currentTouch", flag);
            app.RegisterCLRFieldGetter(field, get_currentTouch_0);
            app.RegisterCLRFieldSetter(field, set_currentTouch_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_currentTouch_0, AssignFromStack_currentTouch_0);
            field = type.GetField("currentCamera", flag);
            app.RegisterCLRFieldGetter(field, get_currentCamera_1);
            app.RegisterCLRFieldSetter(field, set_currentCamera_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_currentCamera_1, AssignFromStack_currentCamera_1);
            field = type.GetField("lastWorldPosition", flag);
            app.RegisterCLRFieldGetter(field, get_lastWorldPosition_2);
            app.RegisterCLRFieldSetter(field, set_lastWorldPosition_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_lastWorldPosition_2, AssignFromStack_lastWorldPosition_2);
            field = type.GetField("lastHit", flag);
            app.RegisterCLRFieldGetter(field, get_lastHit_3);
            app.RegisterCLRFieldSetter(field, set_lastHit_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_lastHit_3, AssignFromStack_lastHit_3);


        }


        static StackObject* get_selectedObject_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::UICamera.selectedObject;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_currentScheme_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::UICamera.currentScheme;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* FindCameraForLayer_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @layer = ptr_of_this_method->Value;


            var result_of_this_method = global::UICamera.FindCameraForLayer(@layer);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_cachedCamera_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UICamera instance_of_this_method = (global::UICamera)typeof(global::UICamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.cachedCamera;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Raycast_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector3 @inPos = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @inPos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @inPos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }


            var result_of_this_method = global::UICamera.Raycast(@inPos);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }


        static object get_currentTouch_0(ref object o)
        {
            return global::UICamera.currentTouch;
        }

        static StackObject* CopyToStack_currentTouch_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::UICamera.currentTouch;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_currentTouch_0(ref object o, object v)
        {
            global::UICamera.currentTouch = (global::UICamera.MouseOrTouch)v;
        }

        static StackObject* AssignFromStack_currentTouch_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UICamera.MouseOrTouch @currentTouch = (global::UICamera.MouseOrTouch)typeof(global::UICamera.MouseOrTouch).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::UICamera.currentTouch = @currentTouch;
            return ptr_of_this_method;
        }

        static object get_currentCamera_1(ref object o)
        {
            return global::UICamera.currentCamera;
        }

        static StackObject* CopyToStack_currentCamera_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::UICamera.currentCamera;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_currentCamera_1(ref object o, object v)
        {
            global::UICamera.currentCamera = (UnityEngine.Camera)v;
        }

        static StackObject* AssignFromStack_currentCamera_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Camera @currentCamera = (UnityEngine.Camera)typeof(UnityEngine.Camera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::UICamera.currentCamera = @currentCamera;
            return ptr_of_this_method;
        }

        static object get_lastWorldPosition_2(ref object o)
        {
            return global::UICamera.lastWorldPosition;
        }

        static StackObject* CopyToStack_lastWorldPosition_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::UICamera.lastWorldPosition;
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static void set_lastWorldPosition_2(ref object o, object v)
        {
            global::UICamera.lastWorldPosition = (UnityEngine.Vector3)v;
        }

        static StackObject* AssignFromStack_lastWorldPosition_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Vector3 @lastWorldPosition = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @lastWorldPosition, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @lastWorldPosition = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }
            global::UICamera.lastWorldPosition = @lastWorldPosition;
            return ptr_of_this_method;
        }

        static object get_lastHit_3(ref object o)
        {
            return global::UICamera.lastHit;
        }

        static StackObject* CopyToStack_lastHit_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::UICamera.lastHit;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_lastHit_3(ref object o, object v)
        {
            global::UICamera.lastHit = (UnityEngine.RaycastHit)v;
        }

        static StackObject* AssignFromStack_lastHit_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.RaycastHit @lastHit = (UnityEngine.RaycastHit)typeof(UnityEngine.RaycastHit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::UICamera.lastHit = @lastHit;
            return ptr_of_this_method;
        }



    }
}
