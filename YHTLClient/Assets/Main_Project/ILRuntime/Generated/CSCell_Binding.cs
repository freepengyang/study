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
    unsafe class CSCell_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSCell);
            args = new Type[]{};
            method = type.GetMethod("get_Coord", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Coord_0);
            args = new Type[]{};
            method = type.GetMethod("get_LocalPosition2", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_LocalPosition2_1);
            args = new Type[]{};
            method = type.GetMethod("get_WorldPosition", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_WorldPosition_2);
            args = new Type[]{typeof(MapEditor.CellType)};
            method = type.GetMethod("isAttributes", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, isAttributes_3);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("isAttributes", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, isAttributes_4);
            args = new Type[]{};
            method = type.GetMethod("get_WorldPosition3", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_WorldPosition3_5);
            args = new Type[]{};
            method = type.GetMethod("get_width", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_width_6);

            field = type.GetField("node", flag);
            app.RegisterCLRFieldGetter(field, get_node_0);
            app.RegisterCLRFieldSetter(field, set_node_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_node_0, AssignFromStack_node_0);
            field = type.GetField("Size", flag);
            app.RegisterCLRFieldGetter(field, get_Size_1);
            app.RegisterCLRFieldSetter(field, set_Size_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_Size_1, AssignFromStack_Size_1);
            field = type.GetField("mCell_x", flag);
            app.RegisterCLRFieldGetter(field, get_mCell_x_2);
            app.RegisterCLRFieldSetter(field, set_mCell_x_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_mCell_x_2, AssignFromStack_mCell_x_2);
            field = type.GetField("mCell_y", flag);
            app.RegisterCLRFieldGetter(field, get_mCell_y_3);
            app.RegisterCLRFieldSetter(field, set_mCell_y_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_mCell_y_3, AssignFromStack_mCell_y_3);


        }


        static StackObject* get_Coord_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSCell instance_of_this_method = (global::CSCell)typeof(global::CSCell).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Coord;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* get_LocalPosition2_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSCell instance_of_this_method = (global::CSCell)typeof(global::CSCell).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.LocalPosition2;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* get_WorldPosition_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSCell instance_of_this_method = (global::CSCell)typeof(global::CSCell).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.WorldPosition;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* isAttributes_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            MapEditor.CellType @type = (MapEditor.CellType)typeof(MapEditor.CellType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSCell instance_of_this_method = (global::CSCell)typeof(global::CSCell).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isAttributes(@type);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* isAttributes_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSCell instance_of_this_method = (global::CSCell)typeof(global::CSCell).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isAttributes(@type);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_WorldPosition3_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSCell instance_of_this_method = (global::CSCell)typeof(global::CSCell).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.WorldPosition3;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* get_width_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::CSCell.width;

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }


        static object get_node_0(ref object o)
        {
            return ((global::CSCell)o).node;
        }

        static StackObject* CopyToStack_node_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSCell)o).node;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_node_0(ref object o, object v)
        {
            ((global::CSCell)o).node = (global::Node)v;
        }

        static StackObject* AssignFromStack_node_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::Node @node = (global::Node)typeof(global::Node).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSCell)o).node = @node;
            return ptr_of_this_method;
        }

        static object get_Size_1(ref object o)
        {
            return global::CSCell.Size;
        }

        static StackObject* CopyToStack_Size_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSCell.Size;
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static void set_Size_1(ref object o, object v)
        {
            global::CSCell.Size = (global::CSMisc.Dot2)v;
        }

        static StackObject* AssignFromStack_Size_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::CSMisc.Dot2 @Size = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @Size, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @Size = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }
            global::CSCell.Size = @Size;
            return ptr_of_this_method;
        }

        static object get_mCell_x_2(ref object o)
        {
            return ((global::CSCell)o).mCell_x;
        }

        static StackObject* CopyToStack_mCell_x_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSCell)o).mCell_x;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_mCell_x_2(ref object o, object v)
        {
            ((global::CSCell)o).mCell_x = (System.Int32)v;
        }

        static StackObject* AssignFromStack_mCell_x_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @mCell_x = ptr_of_this_method->Value;
            ((global::CSCell)o).mCell_x = @mCell_x;
            return ptr_of_this_method;
        }

        static object get_mCell_y_3(ref object o)
        {
            return ((global::CSCell)o).mCell_y;
        }

        static StackObject* CopyToStack_mCell_y_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSCell)o).mCell_y;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_mCell_y_3(ref object o, object v)
        {
            ((global::CSCell)o).mCell_y = (System.Int32)v;
        }

        static StackObject* AssignFromStack_mCell_y_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @mCell_y = ptr_of_this_method->Value;
            ((global::CSCell)o).mCell_y = @mCell_y;
            return ptr_of_this_method;
        }



    }
}
