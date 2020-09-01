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
    unsafe class NetMsgMain_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::NetMsgMain);
            args = new Type[]{};
            method = type.GetMethod("get_Instance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Instance_0);

            field = type.GetField("mNetInfoDicHot", flag);
            app.RegisterCLRFieldGetter(field, get_mNetInfoDicHot_0);
            app.RegisterCLRFieldSetter(field, set_mNetInfoDicHot_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_mNetInfoDicHot_0, AssignFromStack_mNetInfoDicHot_0);


        }


        static StackObject* get_Instance_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::NetMsgMain.Instance;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_mNetInfoDicHot_0(ref object o)
        {
            return ((global::NetMsgMain)o).mNetInfoDicHot;
        }

        static StackObject* CopyToStack_mNetInfoDicHot_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::NetMsgMain)o).mNetInfoDicHot;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mNetInfoDicHot_0(ref object o, object v)
        {
            ((global::NetMsgMain)o).mNetInfoDicHot = (System.Collections.Generic.Dictionary<System.Int32, Google.Protobuf.MessageParser>)v;
        }

        static StackObject* AssignFromStack_mNetInfoDicHot_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.Dictionary<System.Int32, Google.Protobuf.MessageParser> @mNetInfoDicHot = (System.Collections.Generic.Dictionary<System.Int32, Google.Protobuf.MessageParser>)typeof(System.Collections.Generic.Dictionary<System.Int32, Google.Protobuf.MessageParser>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::NetMsgMain)o).mNetInfoDicHot = @mNetInfoDicHot;
            return ptr_of_this_method;
        }



    }
}
