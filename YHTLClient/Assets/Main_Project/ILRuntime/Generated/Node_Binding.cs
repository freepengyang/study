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
    unsafe class Node_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::Node);
            args = new Type[]{};
            method = type.GetMethod("get_isCanCrossNpc", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isCanCrossNpc_0);
            args = new Type[]{};
            method = type.GetMethod("get_isType18", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isType18_1);
            args = new Type[]{};
            method = type.GetMethod("get_isProtect", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isProtect_2);
            args = new Type[]{};
            method = type.GetMethod("get_avatarNum", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_avatarNum_3);
            args = new Type[]{};
            method = type.GetMethod("IsCanCrossWall", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsCanCrossWall_4);
            args = new Type[]{};
            method = type.GetMethod("get_AvatarIDDic", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_AvatarIDDic_5);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("AddItemID", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddItemID_6);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("RemoveItemID", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveItemID_7);

            field = type.GetField("coord", flag);
            app.RegisterCLRFieldGetter(field, get_coord_0);
            app.RegisterCLRFieldSetter(field, set_coord_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_coord_0, AssignFromStack_coord_0);
            field = type.GetField("position", flag);
            app.RegisterCLRFieldGetter(field, get_position_1);
            app.RegisterCLRFieldSetter(field, set_position_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_position_1, AssignFromStack_position_1);
            field = type.GetField("cell", flag);
            app.RegisterCLRFieldGetter(field, get_cell_2);
            app.RegisterCLRFieldSetter(field, set_cell_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_cell_2, AssignFromStack_cell_2);
            field = type.GetField("bObstacle", flag);
            app.RegisterCLRFieldGetter(field, get_bObstacle_3);
            app.RegisterCLRFieldSetter(field, set_bObstacle_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_bObstacle_3, AssignFromStack_bObstacle_3);
            field = type.GetField("isHaveNpcWall", flag);
            app.RegisterCLRFieldGetter(field, get_isHaveNpcWall_4);
            app.RegisterCLRFieldSetter(field, set_isHaveNpcWall_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_isHaveNpcWall_4, AssignFromStack_isHaveNpcWall_4);


        }


        static StackObject* get_isCanCrossNpc_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::Node instance_of_this_method = (global::Node)typeof(global::Node).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isCanCrossNpc;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_isType18_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::Node instance_of_this_method = (global::Node)typeof(global::Node).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isType18;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_isProtect_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::Node instance_of_this_method = (global::Node)typeof(global::Node).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isProtect;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_avatarNum_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::Node instance_of_this_method = (global::Node)typeof(global::Node).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.avatarNum;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* IsCanCrossWall_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::Node instance_of_this_method = (global::Node)typeof(global::Node).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsCanCrossWall();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_AvatarIDDic_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::Node instance_of_this_method = (global::Node)typeof(global::Node).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.AvatarIDDic;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AddItemID_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @itemId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::Node instance_of_this_method = (global::Node)typeof(global::Node).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddItemID(@itemId);

            return __ret;
        }

        static StackObject* RemoveItemID_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @itemId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::Node instance_of_this_method = (global::Node)typeof(global::Node).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveItemID(@itemId);

            return __ret;
        }


        static object get_coord_0(ref object o)
        {
            return ((global::Node)o).coord;
        }

        static StackObject* CopyToStack_coord_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::Node)o).coord;
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static void set_coord_0(ref object o, object v)
        {
            ((global::Node)o).coord = (global::CSMisc.Dot2)v;
        }

        static StackObject* AssignFromStack_coord_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::CSMisc.Dot2 @coord = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @coord, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @coord = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }
            ((global::Node)o).coord = @coord;
            return ptr_of_this_method;
        }

        static object get_position_1(ref object o)
        {
            return ((global::Node)o).position;
        }

        static StackObject* CopyToStack_position_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::Node)o).position;
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static void set_position_1(ref object o, object v)
        {
            ((global::Node)o).position = (UnityEngine.Vector3)v;
        }

        static StackObject* AssignFromStack_position_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Vector3 @position = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @position, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @position = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }
            ((global::Node)o).position = @position;
            return ptr_of_this_method;
        }

        static object get_cell_2(ref object o)
        {
            return ((global::Node)o).cell;
        }

        static StackObject* CopyToStack_cell_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::Node)o).cell;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_cell_2(ref object o, object v)
        {
            ((global::Node)o).cell = (global::CSCell)v;
        }

        static StackObject* AssignFromStack_cell_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::CSCell @cell = (global::CSCell)typeof(global::CSCell).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::Node)o).cell = @cell;
            return ptr_of_this_method;
        }

        static object get_bObstacle_3(ref object o)
        {
            return ((global::Node)o).bObstacle;
        }

        static StackObject* CopyToStack_bObstacle_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::Node)o).bObstacle;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_bObstacle_3(ref object o, object v)
        {
            ((global::Node)o).bObstacle = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_bObstacle_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @bObstacle = ptr_of_this_method->Value == 1;
            ((global::Node)o).bObstacle = @bObstacle;
            return ptr_of_this_method;
        }

        static object get_isHaveNpcWall_4(ref object o)
        {
            return global::Node.isHaveNpcWall;
        }

        static StackObject* CopyToStack_isHaveNpcWall_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::Node.isHaveNpcWall;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isHaveNpcWall_4(ref object o, object v)
        {
            global::Node.isHaveNpcWall = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isHaveNpcWall_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isHaveNpcWall = ptr_of_this_method->Value == 1;
            global::Node.isHaveNpcWall = @isHaveNpcWall;
            return ptr_of_this_method;
        }



    }
}
