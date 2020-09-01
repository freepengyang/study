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
    unsafe class CSResourceManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSResourceManager);
            args = new Type[]{};
            method = type.GetMethod("get_Singleton", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Singleton_0);
            args = new Type[]{};
            method = type.GetMethod("BeginQueueDeal", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, BeginQueueDeal_1);
            args = new Type[]{};
            method = type.GetMethod("EndQueueDeal", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, EndQueueDeal_2);
            args = new Type[]{typeof(System.String), typeof(System.Int32), typeof(System.Action<global::CSResource>), typeof(System.Int32), typeof(System.Boolean), typeof(System.Int64)};
            method = type.GetMethod("AddQueue", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddQueue_3);
            args = new Type[]{};
            method = type.GetMethod("get_IsChangingScene", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_IsChangingScene_4);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_IsChangingScene", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_IsChangingScene_5);
            args = new Type[]{};
            method = type.GetMethod("InitMaxAtlas", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitMaxAtlas_6);
            args = new Type[]{};
            method = type.GetMethod("get_InitMaxPlayerNum", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_InitMaxPlayerNum_7);
            args = new Type[]{};
            method = type.GetMethod("get_InitMaxPlayerAtlasNum", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_InitMaxPlayerAtlasNum_8);
            args = new Type[]{};
            method = type.GetMethod("get_InitMaxWeaponAtlasNum", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_InitMaxWeaponAtlasNum_9);
            args = new Type[]{};
            method = type.GetMethod("get_InitMaxMountAtlasNum", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_InitMaxMountAtlasNum_10);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("UpdateMaxAtlas", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateMaxAtlas_11);
            args = new Type[]{typeof(System.String), typeof(System.Boolean), typeof(System.Boolean), typeof(System.Boolean)};
            method = type.GetMethod("DestroyResource", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DestroyResource_12);

            field = type.GetField("mIsChangingScene", flag);
            app.RegisterCLRFieldGetter(field, get_mIsChangingScene_0);
            app.RegisterCLRFieldSetter(field, set_mIsChangingScene_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_mIsChangingScene_0, AssignFromStack_mIsChangingScene_0);
            field = type.GetField("maxPlayerNum", flag);
            app.RegisterCLRFieldGetter(field, get_maxPlayerNum_1);
            app.RegisterCLRFieldSetter(field, set_maxPlayerNum_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_maxPlayerNum_1, AssignFromStack_maxPlayerNum_1);


        }


        static StackObject* get_Singleton_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::CSResourceManager.Singleton;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* BeginQueueDeal_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.BeginQueueDeal();

            return __ret;
        }

        static StackObject* EndQueueDeal_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.EndQueueDeal();

            return __ret;
        }

        static StackObject* AddQueue_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 7);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @key = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @isPath = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @assistType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Action<global::CSResource> @onLoadCallBack = (System.Action<global::CSResource>)typeof(System.Action<global::CSResource>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.Int32 @type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            System.String @name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 7);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.AddQueue(@name, @type, @onLoadCallBack, @assistType, @isPath, @key);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_IsChangingScene_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsChangingScene;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* set_IsChangingScene_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.IsChangingScene = value;

            return __ret;
        }

        static StackObject* InitMaxAtlas_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitMaxAtlas();

            return __ret;
        }

        static StackObject* get_InitMaxPlayerNum_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.InitMaxPlayerNum;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* get_InitMaxPlayerAtlasNum_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.InitMaxPlayerAtlasNum;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* get_InitMaxWeaponAtlasNum_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.InitMaxWeaponAtlasNum;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* get_InitMaxMountAtlasNum_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.InitMaxMountAtlasNum;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* UpdateMaxAtlas_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @pNum = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @mountAtla = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @weaponAtlas = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int32 @pAtlas = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateMaxAtlas(@pAtlas, @weaponAtlas, @mountAtla, @pNum);

            return __ret;
        }

        static StackObject* DestroyResource_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isRemoveCallBack = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @isRemoveFromDic = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Boolean @isUnLoadUnUsedAssets = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @path = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            global::CSResourceManager instance_of_this_method = (global::CSResourceManager)typeof(global::CSResourceManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.DestroyResource(@path, @isUnLoadUnUsedAssets, @isRemoveFromDic, @isRemoveCallBack);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }


        static object get_mIsChangingScene_0(ref object o)
        {
            return ((global::CSResourceManager)o).mIsChangingScene;
        }

        static StackObject* CopyToStack_mIsChangingScene_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSResourceManager)o).mIsChangingScene;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_mIsChangingScene_0(ref object o, object v)
        {
            ((global::CSResourceManager)o).mIsChangingScene = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_mIsChangingScene_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @mIsChangingScene = ptr_of_this_method->Value == 1;
            ((global::CSResourceManager)o).mIsChangingScene = @mIsChangingScene;
            return ptr_of_this_method;
        }

        static object get_maxPlayerNum_1(ref object o)
        {
            return ((global::CSResourceManager)o).maxPlayerNum;
        }

        static StackObject* CopyToStack_maxPlayerNum_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSResourceManager)o).maxPlayerNum;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_maxPlayerNum_1(ref object o, object v)
        {
            ((global::CSResourceManager)o).maxPlayerNum = (System.Int32)v;
        }

        static StackObject* AssignFromStack_maxPlayerNum_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @maxPlayerNum = ptr_of_this_method->Value;
            ((global::CSResourceManager)o).maxPlayerNum = @maxPlayerNum;
            return ptr_of_this_method;
        }



    }
}
