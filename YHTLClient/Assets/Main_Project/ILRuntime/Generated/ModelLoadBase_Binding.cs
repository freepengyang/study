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
    unsafe class ModelLoadBase_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::ModelLoadBase);
            args = new Type[]{};
            method = type.GetMethod("Destroy", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Destroy_0);
            args = new Type[]{};
            method = type.GetMethod("Release", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Release_1);
            args = new Type[]{typeof(global::CSAction), typeof(System.Int32), typeof(System.Int32), typeof(System.Int32), typeof(System.Action<global::ModelLoadBase>)};
            method = type.GetMethod("UpdateModel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateModel_2);
            args = new Type[]{typeof(global::CSAction), typeof(System.Int32), typeof(System.Action<global::ModelLoadBase>)};
            method = type.GetMethod("UpdateModel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateModel_3);
            args = new Type[]{typeof(global::CSAction), typeof(System.Int32), typeof(System.Int32), typeof(System.Action<global::ModelLoadBase>)};
            method = type.GetMethod("UpdateModel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateModel_4);
            args = new Type[]{};
            method = type.GetMethod("Clear", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Clear_5);

            field = type.GetField("eAvatarType", flag);
            app.RegisterCLRFieldGetter(field, get_eAvatarType_0);
            app.RegisterCLRFieldSetter(field, set_eAvatarType_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_eAvatarType_0, AssignFromStack_eAvatarType_0);


        }


        static StackObject* Destroy_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::ModelLoadBase instance_of_this_method = (global::ModelLoadBase)typeof(global::ModelLoadBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Destroy();

            return __ret;
        }

        static StackObject* Release_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::ModelLoadBase instance_of_this_method = (global::ModelLoadBase)typeof(global::ModelLoadBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Release();

            return __ret;
        }

        static StackObject* UpdateModel_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 6);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<global::ModelLoadBase> @onFinishCallBack = (System.Action<global::ModelLoadBase>)typeof(System.Action<global::ModelLoadBase>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @wingId = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @weaponId = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int32 @bodyId = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            global::CSAction @action = (global::CSAction)typeof(global::CSAction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            global::ModelLoadBase instance_of_this_method = (global::ModelLoadBase)typeof(global::ModelLoadBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateModel(@action, @bodyId, @weaponId, @wingId, @onFinishCallBack);

            return __ret;
        }

        static StackObject* UpdateModel_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<global::ModelLoadBase> @onFinishCallBack = (System.Action<global::ModelLoadBase>)typeof(System.Action<global::ModelLoadBase>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @bodyId = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::CSAction @action = (global::CSAction)typeof(global::CSAction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            global::ModelLoadBase instance_of_this_method = (global::ModelLoadBase)typeof(global::ModelLoadBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateModel(@action, @bodyId, @onFinishCallBack);

            return __ret;
        }

        static StackObject* UpdateModel_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<global::ModelLoadBase> @onFinishCallBack = (System.Action<global::ModelLoadBase>)typeof(System.Action<global::ModelLoadBase>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @weaponId = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @bodyId = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            global::CSAction @action = (global::CSAction)typeof(global::CSAction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            global::ModelLoadBase instance_of_this_method = (global::ModelLoadBase)typeof(global::ModelLoadBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateModel(@action, @bodyId, @weaponId, @onFinishCallBack);

            return __ret;
        }

        static StackObject* Clear_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            global::ModelLoadBase.Clear();

            return __ret;
        }


        static object get_eAvatarType_0(ref object o)
        {
            return ((global::ModelLoadBase)o).eAvatarType;
        }

        static StackObject* CopyToStack_eAvatarType_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::ModelLoadBase)o).eAvatarType;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_eAvatarType_0(ref object o, object v)
        {
            ((global::ModelLoadBase)o).eAvatarType = (System.Int32)v;
        }

        static StackObject* AssignFromStack_eAvatarType_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @eAvatarType = ptr_of_this_method->Value;
            ((global::ModelLoadBase)o).eAvatarType = @eAvatarType;
            return ptr_of_this_method;
        }



    }
}
