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
    unsafe class CSGame_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSGame);
            args = new Type[]{};
            method = type.GetMethod("get_Sington", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Sington_0);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("ChangeStateBackFromGame", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ChangeStateBackFromGame_1);
            args = new Type[]{};
            method = type.GetMethod("InitTable", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitTable_2);
            args = new Type[]{};
            method = type.GetMethod("get_IsLoadLocalRes", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_IsLoadLocalRes_3);

            field = type.GetField("isLoadLocalRes", flag);
            app.RegisterCLRFieldGetter(field, get_isLoadLocalRes_0);
            app.RegisterCLRFieldSetter(field, set_isLoadLocalRes_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_isLoadLocalRes_0, AssignFromStack_isLoadLocalRes_0);
            field = type.GetField("IsFirstTo", flag);
            app.RegisterCLRFieldGetter(field, get_IsFirstTo_1);
            app.RegisterCLRFieldSetter(field, set_IsFirstTo_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_IsFirstTo_1, AssignFromStack_IsFirstTo_1);
            field = type.GetField("IsToRoleListPanel", flag);
            app.RegisterCLRFieldGetter(field, get_IsToRoleListPanel_2);
            app.RegisterCLRFieldSetter(field, set_IsToRoleListPanel_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_IsToRoleListPanel_2, AssignFromStack_IsToRoleListPanel_2);
            field = type.GetField("mCurState", flag);
            app.RegisterCLRFieldGetter(field, get_mCurState_3);
            app.RegisterCLRFieldSetter(field, set_mCurState_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_mCurState_3, AssignFromStack_mCurState_3);
            field = type.GetField("IsMiniApp", flag);
            app.RegisterCLRFieldGetter(field, get_IsMiniApp_4);
            app.RegisterCLRFieldSetter(field, set_IsMiniApp_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_IsMiniApp_4, AssignFromStack_IsMiniApp_4);


        }


        static StackObject* get_Sington_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::CSGame.Sington;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ChangeStateBackFromGame_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isToRoleListPanel = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSGame instance_of_this_method = (global::CSGame)typeof(global::CSGame).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ChangeStateBackFromGame(@isToRoleListPanel);

            return __ret;
        }

        static StackObject* InitTable_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSGame instance_of_this_method = (global::CSGame)typeof(global::CSGame).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitTable();

            return __ret;
        }

        static StackObject* get_IsLoadLocalRes_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSGame instance_of_this_method = (global::CSGame)typeof(global::CSGame).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsLoadLocalRes;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }


        static object get_isLoadLocalRes_0(ref object o)
        {
            return ((global::CSGame)o).isLoadLocalRes;
        }

        static StackObject* CopyToStack_isLoadLocalRes_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSGame)o).isLoadLocalRes;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isLoadLocalRes_0(ref object o, object v)
        {
            ((global::CSGame)o).isLoadLocalRes = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isLoadLocalRes_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isLoadLocalRes = ptr_of_this_method->Value == 1;
            ((global::CSGame)o).isLoadLocalRes = @isLoadLocalRes;
            return ptr_of_this_method;
        }

        static object get_IsFirstTo_1(ref object o)
        {
            return ((global::CSGame)o).IsFirstTo;
        }

        static StackObject* CopyToStack_IsFirstTo_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSGame)o).IsFirstTo;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_IsFirstTo_1(ref object o, object v)
        {
            ((global::CSGame)o).IsFirstTo = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_IsFirstTo_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @IsFirstTo = ptr_of_this_method->Value == 1;
            ((global::CSGame)o).IsFirstTo = @IsFirstTo;
            return ptr_of_this_method;
        }

        static object get_IsToRoleListPanel_2(ref object o)
        {
            return ((global::CSGame)o).IsToRoleListPanel;
        }

        static StackObject* CopyToStack_IsToRoleListPanel_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSGame)o).IsToRoleListPanel;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_IsToRoleListPanel_2(ref object o, object v)
        {
            ((global::CSGame)o).IsToRoleListPanel = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_IsToRoleListPanel_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @IsToRoleListPanel = ptr_of_this_method->Value == 1;
            ((global::CSGame)o).IsToRoleListPanel = @IsToRoleListPanel;
            return ptr_of_this_method;
        }

        static object get_mCurState_3(ref object o)
        {
            return ((global::CSGame)o).mCurState;
        }

        static StackObject* CopyToStack_mCurState_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSGame)o).mCurState;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mCurState_3(ref object o, object v)
        {
            ((global::CSGame)o).mCurState = (global::GameState)v;
        }

        static StackObject* AssignFromStack_mCurState_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::GameState @mCurState = (global::GameState)typeof(global::GameState).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSGame)o).mCurState = @mCurState;
            return ptr_of_this_method;
        }

        static object get_IsMiniApp_4(ref object o)
        {
            return ((global::CSGame)o).IsMiniApp;
        }

        static StackObject* CopyToStack_IsMiniApp_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSGame)o).IsMiniApp;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_IsMiniApp_4(ref object o, object v)
        {
            ((global::CSGame)o).IsMiniApp = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_IsMiniApp_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @IsMiniApp = ptr_of_this_method->Value == 1;
            ((global::CSGame)o).IsMiniApp = @IsMiniApp;
            return ptr_of_this_method;
        }



    }
}
