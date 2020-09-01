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
    unsafe class AvatarUnit_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::AvatarUnit);
            args = new Type[]{};
            method = type.GetMethod("get_IsBeControl", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_IsBeControl_0);
            args = new Type[]{};
            method = type.GetMethod("get_AvatarType", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_AvatarType_1);
            args = new Type[]{};
            method = type.GetMethod("get_OldCell", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_OldCell_2);
            args = new Type[]{};
            method = type.GetMethod("get_ServerCell", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_ServerCell_3);
            args = new Type[]{};
            method = type.GetMethod("get_IsMoving", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_IsMoving_4);
            args = new Type[]{};
            method = type.GetMethod("get_NewCell", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_NewCell_5);
            args = new Type[]{};
            method = type.GetMethod("IsAttackPlaying", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsAttackPlaying_6);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("set_UnitID", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_UnitID_7);
            args = new Type[]{};
            method = type.GetMethod("get_Model", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Model_8);
            args = new Type[]{};
            method = type.GetMethod("get_IsCanSetAction", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_IsCanSetAction_9);
            args = new Type[]{};
            method = type.GetMethod("UpdateAdjusetClientAndServerPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateAdjusetClientAndServerPos_10);
            args = new Type[]{};
            method = type.GetMethod("UpdateShaderName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateShaderName_11);
            args = new Type[]{};
            method = type.GetMethod("UpdatePosition1", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdatePosition1_12);
            args = new Type[]{};
            method = type.GetMethod("get_CacheRootTransform", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_CacheRootTransform_13);
            args = new Type[]{typeof(UnityEngine.Vector2)};
            method = type.GetMethod("SetPosition", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetPosition_14);
            args = new Type[]{};
            method = type.GetMethod("OnOldCellChange", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OnOldCellChange_15);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_IsMoving", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_IsMoving_16);
            args = new Type[]{};
            method = type.GetMethod("get_IsDead", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_IsDead_17);
            args = new Type[]{};
            method = type.GetMethod("MoveInitBase", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, MoveInitBase_18);
            args = new Type[]{};
            method = type.GetMethod("RefreshDepth", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RefreshDepth_19);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetIsDead", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetIsDead_20);
            args = new Type[]{typeof(global::CSCell)};
            method = type.GetMethod("set_OldCell", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_OldCell_21);
            args = new Type[]{typeof(global::CSCell)};
            method = type.GetMethod("set_NewCell", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_NewCell_22);
            args = new Type[]{};
            method = type.GetMethod("get_CacheTransform", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_CacheTransform_23);
            args = new Type[]{typeof(global::CSMisc.Dot2)};
            method = type.GetMethod("set_touchhCoord", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_touchhCoord_24);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_IsLoad", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_IsLoad_25);
            args = new Type[]{};
            method = type.GetMethod("RemovePoolItem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemovePoolItem_26);
            args = new Type[]{typeof(global::CSObjectPoolItem)};
            method = type.GetMethod("set_PoolItem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_PoolItem_27);
            args = new Type[]{};
            method = type.GetMethod("get_ID", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_ID_28);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_AvatarType", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_AvatarType_29);
            args = new Type[]{};
            method = type.GetMethod("InitModel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitModel_30);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("SetStepTime", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetStepTime_31);
            args = new Type[]{typeof(System.Boolean), typeof(System.Int32), typeof(System.Boolean), typeof(System.Boolean)};
            method = type.GetMethod("Initialize", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Initialize_32);
            args = new Type[]{};
            method = type.GetMethod("get_IsDataSplit", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_IsDataSplit_33);
            args = new Type[]{typeof(global::CSModel)};
            method = type.GetMethod("set_Model", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_Model_34);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("ResetServerCell", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResetServerCell_35);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("ResetOldCell", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResetOldCell_36);
            args = new Type[]{};
            method = type.GetMethod("SetShaderName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetShaderName_37);
            args = new Type[]{};
            method = type.GetMethod("get_Paths", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Paths_38);
            args = new Type[]{};
            method = type.GetMethod("get_StepTime", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_StepTime_39);
            args = new Type[]{};
            method = type.GetMethod("NextTarget", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, NextTarget_40);
            args = new Type[]{};
            method = type.GetMethod("get_ModelModule", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_ModelModule_41);
            args = new Type[]{};
            method = type.GetMethod("InitBox", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitBox_42);
            args = new Type[]{};
            method = type.GetMethod("get_IsLoad", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_IsLoad_43);
            args = new Type[]{};
            method = type.GetMethod("MoaveInItBase1", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, MoaveInItBase1_44);
            args = new Type[]{};
            method = type.GetMethod("UpdatePosition2", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdatePosition2_45);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("ResetDepth", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResetDepth_46);
            args = new Type[]{};
            method = type.GetMethod("GetAvatarTypeInt", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetAvatarTypeInt_47);
            args = new Type[]{};
            method = type.GetMethod("GetRealPosition2", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetRealPosition2_48);
            args = new Type[]{};
            method = type.GetMethod("GetDirection", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetDirection_49);

            field = type.GetField("actState", flag);
            app.RegisterCLRFieldGetter(field, get_actState_0);
            app.RegisterCLRFieldSetter(field, set_actState_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_actState_0, AssignFromStack_actState_0);
            field = type.GetField("onTowardsTarget", flag);
            app.RegisterCLRFieldGetter(field, get_onTowardsTarget_1);
            app.RegisterCLRFieldSetter(field, set_onTowardsTarget_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onTowardsTarget_1, AssignFromStack_onTowardsTarget_1);
            field = type.GetField("Go", flag);
            app.RegisterCLRFieldGetter(field, get_Go_2);
            app.RegisterCLRFieldSetter(field, set_Go_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_Go_2, AssignFromStack_Go_2);
            field = type.GetField("PathData", flag);
            app.RegisterCLRFieldGetter(field, get_PathData_3);
            app.RegisterCLRFieldSetter(field, set_PathData_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_PathData_3, AssignFromStack_PathData_3);
            field = type.GetField("box", flag);
            app.RegisterCLRFieldGetter(field, get_box_4);
            app.RegisterCLRFieldSetter(field, set_box_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_box_4, AssignFromStack_box_4);


        }


        static StackObject* get_IsBeControl_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsBeControl;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_AvatarType_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.AvatarType;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* get_OldCell_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.OldCell;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_ServerCell_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.ServerCell;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_IsMoving_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsMoving;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_NewCell_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.NewCell;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* IsAttackPlaying_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsAttackPlaying();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* set_UnitID_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @value = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UnitID = value;

            return __ret;
        }

        static StackObject* get_Model_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Model;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_IsCanSetAction_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsCanSetAction;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* UpdateAdjusetClientAndServerPos_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateAdjusetClientAndServerPos();

            return __ret;
        }

        static StackObject* UpdateShaderName_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateShaderName();

            return __ret;
        }

        static StackObject* UpdatePosition1_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdatePosition1();

            return __ret;
        }

        static StackObject* get_CacheRootTransform_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.CacheRootTransform;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetPosition_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector2 @worldPosition = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @worldPosition, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @worldPosition = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetPosition(@worldPosition);

            return __ret;
        }

        static StackObject* OnOldCellChange_15(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OnOldCellChange();

            return __ret;
        }

        static StackObject* set_IsMoving_16(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.IsMoving = value;

            return __ret;
        }

        static StackObject* get_IsDead_17(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsDead;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* MoveInitBase_18(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.MoveInitBase();

            return __ret;
        }

        static StackObject* RefreshDepth_19(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RefreshDepth();

            return __ret;
        }

        static StackObject* SetIsDead_20(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isDead = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetIsDead(@isDead);

            return __ret;
        }

        static StackObject* set_OldCell_21(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSCell @value = (global::CSCell)typeof(global::CSCell).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OldCell = value;

            return __ret;
        }

        static StackObject* set_NewCell_22(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSCell @value = (global::CSCell)typeof(global::CSCell).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.NewCell = value;

            return __ret;
        }

        static StackObject* get_CacheTransform_23(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.CacheTransform;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* set_touchhCoord_24(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMisc.Dot2 @value = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @value, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @value = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.touchhCoord = value;

            return __ret;
        }

        static StackObject* set_IsLoad_25(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.IsLoad = value;

            return __ret;
        }

        static StackObject* RemovePoolItem_26(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemovePoolItem();

            return __ret;
        }

        static StackObject* set_PoolItem_27(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSObjectPoolItem @value = (global::CSObjectPoolItem)typeof(global::CSObjectPoolItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.PoolItem = value;

            return __ret;
        }

        static StackObject* get_ID_28(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.ID;

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* set_AvatarType_29(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AvatarType = value;

            return __ret;
        }

        static StackObject* InitModel_30(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitModel();

            return __ret;
        }

        static StackObject* SetStepTime_31(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @speed = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetStepTime(@speed);

            return __ret;
        }

        static StackObject* Initialize_32(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isMasterSelf = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @isDead = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @modelHeight = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Boolean @isDataSplit = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Initialize(@isDataSplit, @modelHeight, @isDead, @isMasterSelf);

            return __ret;
        }

        static StackObject* get_IsDataSplit_33(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsDataSplit;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* set_Model_34(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSModel @value = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Model = value;

            return __ret;
        }

        static StackObject* ResetServerCell_35(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @x = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ResetServerCell(@x, @y);

            return __ret;
        }

        static StackObject* ResetOldCell_36(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @x = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ResetOldCell(@x, @y);

            return __ret;
        }

        static StackObject* SetShaderName_37(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetShaderName();

            return __ret;
        }

        static StackObject* get_Paths_38(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Paths;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_StepTime_39(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.StepTime;

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* NextTarget_40(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.NextTarget();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_ModelModule_41(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.ModelModule;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* InitBox_42(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitBox();

            return __ret;
        }

        static StackObject* get_IsLoad_43(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsLoad;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* MoaveInItBase1_44(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.MoaveInItBase1();

            return __ret;
        }

        static StackObject* UpdatePosition2_45(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdatePosition2();

            return __ret;
        }

        static StackObject* ResetDepth_46(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @cooordY = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @coordX = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ResetDepth(@coordX, @cooordY);

            return __ret;
        }

        static StackObject* GetAvatarTypeInt_47(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetAvatarTypeInt();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetRealPosition2_48(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetRealPosition2();

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* GetDirection_49(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AvatarUnit instance_of_this_method = (global::AvatarUnit)typeof(global::AvatarUnit).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetDirection();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }


        static object get_actState_0(ref object o)
        {
            return ((global::AvatarUnit)o).actState;
        }

        static StackObject* CopyToStack_actState_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::AvatarUnit)o).actState;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_actState_0(ref object o, object v)
        {
            ((global::AvatarUnit)o).actState = (global::CSAvatarState)v;
        }

        static StackObject* AssignFromStack_actState_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::CSAvatarState @actState = (global::CSAvatarState)typeof(global::CSAvatarState).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::AvatarUnit)o).actState = @actState;
            return ptr_of_this_method;
        }

        static object get_onTowardsTarget_1(ref object o)
        {
            return ((global::AvatarUnit)o).onTowardsTarget;
        }

        static StackObject* CopyToStack_onTowardsTarget_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::AvatarUnit)o).onTowardsTarget;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onTowardsTarget_1(ref object o, object v)
        {
            ((global::AvatarUnit)o).onTowardsTarget = (System.Action<global::CSMisc.Dot2>)v;
        }

        static StackObject* AssignFromStack_onTowardsTarget_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<global::CSMisc.Dot2> @onTowardsTarget = (System.Action<global::CSMisc.Dot2>)typeof(System.Action<global::CSMisc.Dot2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::AvatarUnit)o).onTowardsTarget = @onTowardsTarget;
            return ptr_of_this_method;
        }

        static object get_Go_2(ref object o)
        {
            return ((global::AvatarUnit)o).Go;
        }

        static StackObject* CopyToStack_Go_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::AvatarUnit)o).Go;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Go_2(ref object o, object v)
        {
            ((global::AvatarUnit)o).Go = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_Go_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @Go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::AvatarUnit)o).Go = @Go;
            return ptr_of_this_method;
        }

        static object get_PathData_3(ref object o)
        {
            return ((global::AvatarUnit)o).PathData;
        }

        static StackObject* CopyToStack_PathData_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::AvatarUnit)o).PathData;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_PathData_3(ref object o, object v)
        {
            ((global::AvatarUnit)o).PathData = (global::CSPathData)v;
        }

        static StackObject* AssignFromStack_PathData_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::CSPathData @PathData = (global::CSPathData)typeof(global::CSPathData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::AvatarUnit)o).PathData = @PathData;
            return ptr_of_this_method;
        }

        static object get_box_4(ref object o)
        {
            return ((global::AvatarUnit)o).box;
        }

        static StackObject* CopyToStack_box_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::AvatarUnit)o).box;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_box_4(ref object o, object v)
        {
            ((global::AvatarUnit)o).box = (UnityEngine.BoxCollider)v;
        }

        static StackObject* AssignFromStack_box_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.BoxCollider @box = (UnityEngine.BoxCollider)typeof(UnityEngine.BoxCollider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::AvatarUnit)o).box = @box;
            return ptr_of_this_method;
        }



    }
}
