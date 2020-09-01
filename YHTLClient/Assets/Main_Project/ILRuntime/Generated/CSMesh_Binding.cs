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
    unsafe class CSMesh_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSMesh);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("getCell", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getCell_0);
            args = new Type[]{typeof(global::CSMisc.Dot2)};
            method = type.GetMethod("getNode", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getNode_1);
            args = new Type[]{typeof(global::CSMisc.Dot2)};
            method = type.GetMethod("getCell", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getCell_2);
            args = new Type[]{};
            method = type.GetMethod("Destroy", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Destroy_3);
            args = new Type[]{};
            method = type.GetMethod("get_PadVisionCountX", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_PadVisionCountX_4);
            args = new Type[]{};
            method = type.GetMethod("get_PadVisionCountY", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_PadVisionCountY_5);
            args = new Type[]{};
            method = type.GetMethod("get_XRange", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_XRange_6);
            args = new Type[]{};
            method = type.GetMethod("get_YRange", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_YRange_7);

            field = type.GetField("VisionCount", flag);
            app.RegisterCLRFieldGetter(field, get_VisionCount_0);
            app.RegisterCLRFieldSetter(field, set_VisionCount_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_VisionCount_0, AssignFromStack_VisionCount_0);


        }


        static StackObject* getCell_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @x = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::CSMesh instance_of_this_method = (global::CSMesh)typeof(global::CSMesh).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getCell(@x, @y);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getNode_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMisc.Dot2 @coord = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @coord, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @coord = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSMesh instance_of_this_method = (global::CSMesh)typeof(global::CSMesh).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getNode(@coord);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getCell_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMisc.Dot2 @dot = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @dot, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @dot = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSMesh instance_of_this_method = (global::CSMesh)typeof(global::CSMesh).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getCell(@dot);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Destroy_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMesh instance_of_this_method = (global::CSMesh)typeof(global::CSMesh).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Destroy();

            return __ret;
        }

        static StackObject* get_PadVisionCountX_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::CSMesh.PadVisionCountX;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* get_PadVisionCountY_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::CSMesh.PadVisionCountY;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* get_XRange_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMesh instance_of_this_method = (global::CSMesh)typeof(global::CSMesh).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.XRange;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* get_YRange_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMesh instance_of_this_method = (global::CSMesh)typeof(global::CSMesh).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.YRange;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }


        static object get_VisionCount_0(ref object o)
        {
            return ((global::CSMesh)o).VisionCount;
        }

        static StackObject* CopyToStack_VisionCount_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSMesh)o).VisionCount;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_VisionCount_0(ref object o, object v)
        {
            ((global::CSMesh)o).VisionCount = (System.Int32)v;
        }

        static StackObject* AssignFromStack_VisionCount_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @VisionCount = ptr_of_this_method->Value;
            ((global::CSMesh)o).VisionCount = @VisionCount;
            return ptr_of_this_method;
        }



    }
}
