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
    unsafe class CSConstant_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSConstant);

            field = type.GetField("loginName", flag);
            app.RegisterCLRFieldGetter(field, get_loginName_0);
            app.RegisterCLRFieldSetter(field, set_loginName_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_loginName_0, AssignFromStack_loginName_0);
            field = type.GetField("mSeverId", flag);
            app.RegisterCLRFieldGetter(field, get_mSeverId_1);
            app.RegisterCLRFieldSetter(field, set_mSeverId_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_mSeverId_1, AssignFromStack_mSeverId_1);
            field = type.GetField("lastStandTimeUnloadAsset", flag);
            app.RegisterCLRFieldGetter(field, get_lastStandTimeUnloadAsset_2);
            app.RegisterCLRFieldSetter(field, set_lastStandTimeUnloadAsset_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_lastStandTimeUnloadAsset_2, AssignFromStack_lastStandTimeUnloadAsset_2);
            field = type.GetField("IsLanuchMainPlayer", flag);
            app.RegisterCLRFieldGetter(field, get_IsLanuchMainPlayer_3);
            app.RegisterCLRFieldSetter(field, set_IsLanuchMainPlayer_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_IsLanuchMainPlayer_3, AssignFromStack_IsLanuchMainPlayer_3);
            field = type.GetField("MainPlayerName", flag);
            app.RegisterCLRFieldGetter(field, get_MainPlayerName_4);
            app.RegisterCLRFieldSetter(field, set_MainPlayerName_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_MainPlayerName_4, AssignFromStack_MainPlayerName_4);
            field = type.GetField("isMainPlayerMoving", flag);
            app.RegisterCLRFieldGetter(field, get_isMainPlayerMoving_5);
            app.RegisterCLRFieldSetter(field, set_isMainPlayerMoving_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_isMainPlayerMoving_5, AssignFromStack_isMainPlayerMoving_5);
            field = type.GetField("PixelRatio", flag);
            app.RegisterCLRFieldGetter(field, get_PixelRatio_6);
            app.RegisterCLRFieldSetter(field, set_PixelRatio_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_PixelRatio_6, AssignFromStack_PixelRatio_6);
            field = type.GetField("mOnlyServerId", flag);
            app.RegisterCLRFieldGetter(field, get_mOnlyServerId_7);
            app.RegisterCLRFieldSetter(field, set_mOnlyServerId_7);
            app.RegisterCLRFieldBinding(field, CopyToStack_mOnlyServerId_7, AssignFromStack_mOnlyServerId_7);
            field = type.GetField("RoleCount", flag);
            app.RegisterCLRFieldGetter(field, get_RoleCount_8);
            app.RegisterCLRFieldSetter(field, set_RoleCount_8);
            app.RegisterCLRFieldBinding(field, CopyToStack_RoleCount_8, AssignFromStack_RoleCount_8);
            field = type.GetField("plantformBigId", flag);
            app.RegisterCLRFieldGetter(field, get_plantformBigId_9);
            app.RegisterCLRFieldSetter(field, set_plantformBigId_9);
            app.RegisterCLRFieldBinding(field, CopyToStack_plantformBigId_9, AssignFromStack_plantformBigId_9);
            field = type.GetField("ServerType", flag);
            app.RegisterCLRFieldGetter(field, get_ServerType_10);
            app.RegisterCLRFieldSetter(field, set_ServerType_10);
            app.RegisterCLRFieldBinding(field, CopyToStack_ServerType_10, AssignFromStack_ServerType_10);
            field = type.GetField("LastServerType", flag);
            app.RegisterCLRFieldGetter(field, get_LastServerType_11);
            app.RegisterCLRFieldSetter(field, set_LastServerType_11);
            app.RegisterCLRFieldBinding(field, CopyToStack_LastServerType_11, AssignFromStack_LastServerType_11);
            field = type.GetField("isWhitePaper", flag);
            app.RegisterCLRFieldGetter(field, get_isWhitePaper_12);
            app.RegisterCLRFieldSetter(field, set_isWhitePaper_12);
            app.RegisterCLRFieldBinding(field, CopyToStack_isWhitePaper_12, AssignFromStack_isWhitePaper_12);


        }



        static object get_loginName_0(ref object o)
        {
            return global::CSConstant.loginName;
        }

        static StackObject* CopyToStack_loginName_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.loginName;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_loginName_0(ref object o, object v)
        {
            global::CSConstant.loginName = (System.String)v;
        }

        static StackObject* AssignFromStack_loginName_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @loginName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::CSConstant.loginName = @loginName;
            return ptr_of_this_method;
        }

        static object get_mSeverId_1(ref object o)
        {
            return global::CSConstant.mSeverId;
        }

        static StackObject* CopyToStack_mSeverId_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.mSeverId;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_mSeverId_1(ref object o, object v)
        {
            global::CSConstant.mSeverId = (System.Int32)v;
        }

        static StackObject* AssignFromStack_mSeverId_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @mSeverId = ptr_of_this_method->Value;
            global::CSConstant.mSeverId = @mSeverId;
            return ptr_of_this_method;
        }

        static object get_lastStandTimeUnloadAsset_2(ref object o)
        {
            return global::CSConstant.lastStandTimeUnloadAsset;
        }

        static StackObject* CopyToStack_lastStandTimeUnloadAsset_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.lastStandTimeUnloadAsset;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_lastStandTimeUnloadAsset_2(ref object o, object v)
        {
            global::CSConstant.lastStandTimeUnloadAsset = (System.Single)v;
        }

        static StackObject* AssignFromStack_lastStandTimeUnloadAsset_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @lastStandTimeUnloadAsset = *(float*)&ptr_of_this_method->Value;
            global::CSConstant.lastStandTimeUnloadAsset = @lastStandTimeUnloadAsset;
            return ptr_of_this_method;
        }

        static object get_IsLanuchMainPlayer_3(ref object o)
        {
            return global::CSConstant.IsLanuchMainPlayer;
        }

        static StackObject* CopyToStack_IsLanuchMainPlayer_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.IsLanuchMainPlayer;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_IsLanuchMainPlayer_3(ref object o, object v)
        {
            global::CSConstant.IsLanuchMainPlayer = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_IsLanuchMainPlayer_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @IsLanuchMainPlayer = ptr_of_this_method->Value == 1;
            global::CSConstant.IsLanuchMainPlayer = @IsLanuchMainPlayer;
            return ptr_of_this_method;
        }

        static object get_MainPlayerName_4(ref object o)
        {
            return global::CSConstant.MainPlayerName;
        }

        static StackObject* CopyToStack_MainPlayerName_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.MainPlayerName;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_MainPlayerName_4(ref object o, object v)
        {
            global::CSConstant.MainPlayerName = (System.String)v;
        }

        static StackObject* AssignFromStack_MainPlayerName_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @MainPlayerName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::CSConstant.MainPlayerName = @MainPlayerName;
            return ptr_of_this_method;
        }

        static object get_isMainPlayerMoving_5(ref object o)
        {
            return global::CSConstant.isMainPlayerMoving;
        }

        static StackObject* CopyToStack_isMainPlayerMoving_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.isMainPlayerMoving;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isMainPlayerMoving_5(ref object o, object v)
        {
            global::CSConstant.isMainPlayerMoving = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isMainPlayerMoving_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isMainPlayerMoving = ptr_of_this_method->Value == 1;
            global::CSConstant.isMainPlayerMoving = @isMainPlayerMoving;
            return ptr_of_this_method;
        }

        static object get_PixelRatio_6(ref object o)
        {
            return global::CSConstant.PixelRatio;
        }

        static StackObject* CopyToStack_PixelRatio_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.PixelRatio;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_PixelRatio_6(ref object o, object v)
        {
            global::CSConstant.PixelRatio = (System.Single)v;
        }

        static StackObject* AssignFromStack_PixelRatio_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @PixelRatio = *(float*)&ptr_of_this_method->Value;
            global::CSConstant.PixelRatio = @PixelRatio;
            return ptr_of_this_method;
        }

        static object get_mOnlyServerId_7(ref object o)
        {
            return global::CSConstant.mOnlyServerId;
        }

        static StackObject* CopyToStack_mOnlyServerId_7(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.mOnlyServerId;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_mOnlyServerId_7(ref object o, object v)
        {
            global::CSConstant.mOnlyServerId = (System.Int32)v;
        }

        static StackObject* AssignFromStack_mOnlyServerId_7(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @mOnlyServerId = ptr_of_this_method->Value;
            global::CSConstant.mOnlyServerId = @mOnlyServerId;
            return ptr_of_this_method;
        }

        static object get_RoleCount_8(ref object o)
        {
            return global::CSConstant.RoleCount;
        }

        static StackObject* CopyToStack_RoleCount_8(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.RoleCount;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_RoleCount_8(ref object o, object v)
        {
            global::CSConstant.RoleCount = (System.Int32)v;
        }

        static StackObject* AssignFromStack_RoleCount_8(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @RoleCount = ptr_of_this_method->Value;
            global::CSConstant.RoleCount = @RoleCount;
            return ptr_of_this_method;
        }

        static object get_plantformBigId_9(ref object o)
        {
            return global::CSConstant.plantformBigId;
        }

        static StackObject* CopyToStack_plantformBigId_9(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.plantformBigId;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_plantformBigId_9(ref object o, object v)
        {
            global::CSConstant.plantformBigId = (System.Int32)v;
        }

        static StackObject* AssignFromStack_plantformBigId_9(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @plantformBigId = ptr_of_this_method->Value;
            global::CSConstant.plantformBigId = @plantformBigId;
            return ptr_of_this_method;
        }

        static object get_ServerType_10(ref object o)
        {
            return global::CSConstant.ServerType;
        }

        static StackObject* CopyToStack_ServerType_10(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.ServerType;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ServerType_10(ref object o, object v)
        {
            global::CSConstant.ServerType = (System.String)v;
        }

        static StackObject* AssignFromStack_ServerType_10(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @ServerType = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::CSConstant.ServerType = @ServerType;
            return ptr_of_this_method;
        }

        static object get_LastServerType_11(ref object o)
        {
            return global::CSConstant.LastServerType;
        }

        static StackObject* CopyToStack_LastServerType_11(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.LastServerType;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_LastServerType_11(ref object o, object v)
        {
            global::CSConstant.LastServerType = (System.String)v;
        }

        static StackObject* AssignFromStack_LastServerType_11(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @LastServerType = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::CSConstant.LastServerType = @LastServerType;
            return ptr_of_this_method;
        }

        static object get_isWhitePaper_12(ref object o)
        {
            return global::CSConstant.isWhitePaper;
        }

        static StackObject* CopyToStack_isWhitePaper_12(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSConstant.isWhitePaper;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isWhitePaper_12(ref object o, object v)
        {
            global::CSConstant.isWhitePaper = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isWhitePaper_12(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isWhitePaper = ptr_of_this_method->Value == 1;
            global::CSConstant.isWhitePaper = @isWhitePaper;
            return ptr_of_this_method;
        }



    }
}
