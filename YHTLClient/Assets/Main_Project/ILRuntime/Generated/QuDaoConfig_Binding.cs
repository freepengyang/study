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
    unsafe class QuDaoConfig_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::QuDaoConfig);

            field = type.GetField("requestCode", flag);
            app.RegisterCLRFieldGetter(field, get_requestCode_0);
            app.RegisterCLRFieldSetter(field, set_requestCode_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_requestCode_0, AssignFromStack_requestCode_0);
            field = type.GetField("platformID", flag);
            app.RegisterCLRFieldGetter(field, get_platformID_1);
            app.RegisterCLRFieldSetter(field, set_platformID_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_platformID_1, AssignFromStack_platformID_1);
            field = type.GetField("submitData", flag);
            app.RegisterCLRFieldGetter(field, get_submitData_2);
            app.RegisterCLRFieldSetter(field, set_submitData_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_submitData_2, AssignFromStack_submitData_2);
            field = type.GetField("exitAccount", flag);
            app.RegisterCLRFieldGetter(field, get_exitAccount_3);
            app.RegisterCLRFieldSetter(field, set_exitAccount_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_exitAccount_3, AssignFromStack_exitAccount_3);
            field = type.GetField("payCode", flag);
            app.RegisterCLRFieldGetter(field, get_payCode_4);
            app.RegisterCLRFieldSetter(field, set_payCode_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_payCode_4, AssignFromStack_payCode_4);
            field = type.GetField("switchAccount", flag);
            app.RegisterCLRFieldGetter(field, get_switchAccount_5);
            app.RegisterCLRFieldSetter(field, set_switchAccount_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_switchAccount_5, AssignFromStack_switchAccount_5);
            field = type.GetField("loginCode", flag);
            app.RegisterCLRFieldGetter(field, get_loginCode_6);
            app.RegisterCLRFieldSetter(field, set_loginCode_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_loginCode_6, AssignFromStack_loginCode_6);


        }



        static object get_requestCode_0(ref object o)
        {
            return ((global::QuDaoConfig)o).requestCode;
        }

        static StackObject* CopyToStack_requestCode_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::QuDaoConfig)o).requestCode;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_requestCode_0(ref object o, object v)
        {
            ((global::QuDaoConfig)o).requestCode = (global::RequestCode)v;
        }

        static StackObject* AssignFromStack_requestCode_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::RequestCode @requestCode = (global::RequestCode)typeof(global::RequestCode).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::QuDaoConfig)o).requestCode = @requestCode;
            return ptr_of_this_method;
        }

        static object get_platformID_1(ref object o)
        {
            return ((global::QuDaoConfig)o).platformID;
        }

        static StackObject* CopyToStack_platformID_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::QuDaoConfig)o).platformID;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_platformID_1(ref object o, object v)
        {
            ((global::QuDaoConfig)o).platformID = (System.Int32)v;
        }

        static StackObject* AssignFromStack_platformID_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @platformID = ptr_of_this_method->Value;
            ((global::QuDaoConfig)o).platformID = @platformID;
            return ptr_of_this_method;
        }

        static object get_submitData_2(ref object o)
        {
            return ((global::QuDaoConfig)o).submitData;
        }

        static StackObject* CopyToStack_submitData_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::QuDaoConfig)o).submitData;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_submitData_2(ref object o, object v)
        {
            ((global::QuDaoConfig)o).submitData = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_submitData_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @submitData = ptr_of_this_method->Value == 1;
            ((global::QuDaoConfig)o).submitData = @submitData;
            return ptr_of_this_method;
        }

        static object get_exitAccount_3(ref object o)
        {
            return ((global::QuDaoConfig)o).exitAccount;
        }

        static StackObject* CopyToStack_exitAccount_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::QuDaoConfig)o).exitAccount;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_exitAccount_3(ref object o, object v)
        {
            ((global::QuDaoConfig)o).exitAccount = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_exitAccount_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @exitAccount = ptr_of_this_method->Value == 1;
            ((global::QuDaoConfig)o).exitAccount = @exitAccount;
            return ptr_of_this_method;
        }

        static object get_payCode_4(ref object o)
        {
            return ((global::QuDaoConfig)o).payCode;
        }

        static StackObject* CopyToStack_payCode_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::QuDaoConfig)o).payCode;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_payCode_4(ref object o, object v)
        {
            ((global::QuDaoConfig)o).payCode = (global::PayCode)v;
        }

        static StackObject* AssignFromStack_payCode_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::PayCode @payCode = (global::PayCode)typeof(global::PayCode).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::QuDaoConfig)o).payCode = @payCode;
            return ptr_of_this_method;
        }

        static object get_switchAccount_5(ref object o)
        {
            return ((global::QuDaoConfig)o).switchAccount;
        }

        static StackObject* CopyToStack_switchAccount_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::QuDaoConfig)o).switchAccount;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_switchAccount_5(ref object o, object v)
        {
            ((global::QuDaoConfig)o).switchAccount = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_switchAccount_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @switchAccount = ptr_of_this_method->Value == 1;
            ((global::QuDaoConfig)o).switchAccount = @switchAccount;
            return ptr_of_this_method;
        }

        static object get_loginCode_6(ref object o)
        {
            return ((global::QuDaoConfig)o).loginCode;
        }

        static StackObject* CopyToStack_loginCode_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::QuDaoConfig)o).loginCode;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_loginCode_6(ref object o, object v)
        {
            ((global::QuDaoConfig)o).loginCode = (global::LoginCode)v;
        }

        static StackObject* AssignFromStack_loginCode_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::LoginCode @loginCode = (global::LoginCode)typeof(global::LoginCode).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::QuDaoConfig)o).loginCode = @loginCode;
            return ptr_of_this_method;
        }



    }
}
