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
    unsafe class UIChatSpringPanel_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIChatSpringPanel);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("UpdateSprite", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateSprite_0);

            field = type.GetField("topMessage", flag);
            app.RegisterCLRFieldGetter(field, get_topMessage_0);
            app.RegisterCLRFieldSetter(field, set_topMessage_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_topMessage_0, AssignFromStack_topMessage_0);
            field = type.GetField("bottomMesssage", flag);
            app.RegisterCLRFieldGetter(field, get_bottomMesssage_1);
            app.RegisterCLRFieldSetter(field, set_bottomMesssage_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_bottomMesssage_1, AssignFromStack_bottomMesssage_1);


        }


        static StackObject* UpdateSprite_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @delta = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIChatSpringPanel instance_of_this_method = (global::UIChatSpringPanel)typeof(global::UIChatSpringPanel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateSprite(@delta);

            return __ret;
        }


        static object get_topMessage_0(ref object o)
        {
            return ((global::UIChatSpringPanel)o).topMessage;
        }

        static StackObject* CopyToStack_topMessage_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIChatSpringPanel)o).topMessage;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_topMessage_0(ref object o, object v)
        {
            ((global::UIChatSpringPanel)o).topMessage = (global::UISprite)v;
        }

        static StackObject* AssignFromStack_topMessage_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UISprite @topMessage = (global::UISprite)typeof(global::UISprite).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIChatSpringPanel)o).topMessage = @topMessage;
            return ptr_of_this_method;
        }

        static object get_bottomMesssage_1(ref object o)
        {
            return ((global::UIChatSpringPanel)o).bottomMesssage;
        }

        static StackObject* CopyToStack_bottomMesssage_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIChatSpringPanel)o).bottomMesssage;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_bottomMesssage_1(ref object o, object v)
        {
            ((global::UIChatSpringPanel)o).bottomMesssage = (global::UISprite)v;
        }

        static StackObject* AssignFromStack_bottomMesssage_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UISprite @bottomMesssage = (global::UISprite)typeof(global::UISprite).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIChatSpringPanel)o).bottomMesssage = @bottomMesssage;
            return ptr_of_this_method;
        }



    }
}
