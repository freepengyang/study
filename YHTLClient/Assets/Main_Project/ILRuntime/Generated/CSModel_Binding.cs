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
    unsafe class CSModel_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSModel);
            args = new Type[]{};
            method = type.GetMethod("DestroyBottom", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DestroyBottom_0);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("Show", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Show_1);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("SetAction", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetAction_2);
            args = new Type[]{typeof(UnityEngine.Vector3), typeof(global::CSCell)};
            method = type.GetMethod("SetDirection", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetDirection_3);
            args = new Type[]{};
            method = type.GetMethod("EndOfAction", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, EndOfAction_4);
            args = new Type[]{};
            method = type.GetMethod("IsAttackMotion", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsAttackMotion_5);
            args = new Type[]{};
            method = type.GetMethod("Destroy", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Destroy_6);
            args = new Type[]{};
            method = type.GetMethod("Release", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Release_7);
            args = new Type[]{typeof(System.Int32), typeof(System.Boolean)};
            method = type.GetMethod("SetDirection", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetDirection_8);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("InitAnimationFPS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitAnimationFPS_9);
            args = new Type[]{};
            method = type.GetMethod("get_Action", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Action_10);
            args = new Type[]{typeof(global::CSBottom)};
            method = type.GetMethod("AttachBottom", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AttachBottom_11);
            args = new Type[]{typeof(global::CSBottomNPC)};
            method = type.GetMethod("AttachBottomNPC", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AttachBottomNPC_12);
            args = new Type[]{typeof(global::CSModelModule)};
            method = type.GetMethod("InitPart", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitPart_13);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("ShowSelectAndHideOtherBottom", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ShowSelectAndHideOtherBottom_14);

            field = type.GetField("Effect", flag);
            app.RegisterCLRFieldGetter(field, get_Effect_0);
            app.RegisterCLRFieldSetter(field, set_Effect_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Effect_0, AssignFromStack_Effect_0);
            field = type.GetField("Bottom", flag);
            app.RegisterCLRFieldGetter(field, get_Bottom_1);
            app.RegisterCLRFieldSetter(field, set_Bottom_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_Bottom_1, AssignFromStack_Bottom_1);
            field = type.GetField("BottomNPC", flag);
            app.RegisterCLRFieldGetter(field, get_BottomNPC_2);
            app.RegisterCLRFieldSetter(field, set_BottomNPC_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_BottomNPC_2, AssignFromStack_BottomNPC_2);
            field = type.GetField("Body", flag);
            app.RegisterCLRFieldGetter(field, get_Body_3);
            app.RegisterCLRFieldSetter(field, set_Body_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_Body_3, AssignFromStack_Body_3);
            field = type.GetField("Weapon", flag);
            app.RegisterCLRFieldGetter(field, get_Weapon_4);
            app.RegisterCLRFieldSetter(field, set_Weapon_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_Weapon_4, AssignFromStack_Weapon_4);
            field = type.GetField("Wing", flag);
            app.RegisterCLRFieldGetter(field, get_Wing_5);
            app.RegisterCLRFieldSetter(field, set_Wing_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_Wing_5, AssignFromStack_Wing_5);

            args = new Type[]{typeof(UnityEngine.Transform), typeof(UnityEngine.BoxCollider), typeof(System.Int32), typeof(System.Boolean), typeof(global::EShareMatType), typeof(System.Action<System.Boolean>)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* DestroyBottom_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.DestroyBottom();

            return __ret;
        }

        static StackObject* Show_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Show(@value);

            return __ret;
        }

        static StackObject* SetAction_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @stopType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @motion = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetAction(@motion, @stopType);

            return __ret;
        }

        static StackObject* SetDirection_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSCell @oldCell = (global::CSCell)typeof(global::CSCell).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Vector3 @position = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @position, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @position = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetDirection(@position, @oldCell);

            return __ret;
        }

        static StackObject* EndOfAction_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.EndOfAction();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsAttackMotion_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsAttackMotion();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* Destroy_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Destroy();

            return __ret;
        }

        static StackObject* Release_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Release();

            return __ret;
        }

        static StackObject* SetDirection_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isForceSet = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @direction = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetDirection(@direction, @isForceSet);

            return __ret;
        }

        static StackObject* InitAnimationFPS_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @modelId = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitAnimationFPS(@modelId);

            return __ret;
        }

        static StackObject* get_Action_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Action;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AttachBottom_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSBottom @bottom = (global::CSBottom)typeof(global::CSBottom).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AttachBottom(@bottom);

            return __ret;
        }

        static StackObject* AttachBottomNPC_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSBottomNPC @bottom = (global::CSBottomNPC)typeof(global::CSBottomNPC).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AttachBottomNPC(@bottom);

            return __ret;
        }

        static StackObject* InitPart_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSModelModule @module = (global::CSModelModule)typeof(global::CSModelModule).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitPart(@module);

            return __ret;
        }

        static StackObject* ShowSelectAndHideOtherBottom_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSModel instance_of_this_method = (global::CSModel)typeof(global::CSModel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ShowSelectAndHideOtherBottom(@type);

            return __ret;
        }


        static object get_Effect_0(ref object o)
        {
            return ((global::CSModel)o).Effect;
        }

        static StackObject* CopyToStack_Effect_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSModel)o).Effect;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Effect_0(ref object o, object v)
        {
            ((global::CSModel)o).Effect = (global::CSEffect)v;
        }

        static StackObject* AssignFromStack_Effect_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::CSEffect @Effect = (global::CSEffect)typeof(global::CSEffect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSModel)o).Effect = @Effect;
            return ptr_of_this_method;
        }

        static object get_Bottom_1(ref object o)
        {
            return ((global::CSModel)o).Bottom;
        }

        static StackObject* CopyToStack_Bottom_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSModel)o).Bottom;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Bottom_1(ref object o, object v)
        {
            ((global::CSModel)o).Bottom = (global::CSBottom)v;
        }

        static StackObject* AssignFromStack_Bottom_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::CSBottom @Bottom = (global::CSBottom)typeof(global::CSBottom).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSModel)o).Bottom = @Bottom;
            return ptr_of_this_method;
        }

        static object get_BottomNPC_2(ref object o)
        {
            return ((global::CSModel)o).BottomNPC;
        }

        static StackObject* CopyToStack_BottomNPC_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSModel)o).BottomNPC;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_BottomNPC_2(ref object o, object v)
        {
            ((global::CSModel)o).BottomNPC = (global::CSBottomNPC)v;
        }

        static StackObject* AssignFromStack_BottomNPC_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::CSBottomNPC @BottomNPC = (global::CSBottomNPC)typeof(global::CSBottomNPC).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSModel)o).BottomNPC = @BottomNPC;
            return ptr_of_this_method;
        }

        static object get_Body_3(ref object o)
        {
            return ((global::CSModel)o).Body;
        }

        static StackObject* CopyToStack_Body_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSModel)o).Body;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Body_3(ref object o, object v)
        {
            ((global::CSModel)o).Body = (global::CSBody)v;
        }

        static StackObject* AssignFromStack_Body_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::CSBody @Body = (global::CSBody)typeof(global::CSBody).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSModel)o).Body = @Body;
            return ptr_of_this_method;
        }

        static object get_Weapon_4(ref object o)
        {
            return ((global::CSModel)o).Weapon;
        }

        static StackObject* CopyToStack_Weapon_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSModel)o).Weapon;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Weapon_4(ref object o, object v)
        {
            ((global::CSModel)o).Weapon = (global::CSWeapon)v;
        }

        static StackObject* AssignFromStack_Weapon_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::CSWeapon @Weapon = (global::CSWeapon)typeof(global::CSWeapon).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSModel)o).Weapon = @Weapon;
            return ptr_of_this_method;
        }

        static object get_Wing_5(ref object o)
        {
            return ((global::CSModel)o).Wing;
        }

        static StackObject* CopyToStack_Wing_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSModel)o).Wing;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Wing_5(ref object o, object v)
        {
            ((global::CSModel)o).Wing = (global::CSWing)v;
        }

        static StackObject* AssignFromStack_Wing_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::CSWing @Wing = (global::CSWing)typeof(global::CSWing).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSModel)o).Wing = @Wing;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 6);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Boolean> @_replaceEquip = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::EShareMatType @_matType = (global::EShareMatType)typeof(global::EShareMatType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Boolean @isDataSplit = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int32 @avatarType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.BoxCollider @_box = (UnityEngine.BoxCollider)typeof(UnityEngine.BoxCollider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            UnityEngine.Transform @tr = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = new global::CSModel(@tr, @_box, @avatarType, @isDataSplit, @_matType, @_replaceEquip);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
