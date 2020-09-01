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
    unsafe class HotFix_InvokeThird_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::HotFix_InvokeThird);

            field = type.GetField("UIPlaySoundInCSAudioAction", flag);
            app.RegisterCLRFieldGetter(field, get_UIPlaySoundInCSAudioAction_0);
            app.RegisterCLRFieldSetter(field, set_UIPlaySoundInCSAudioAction_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_UIPlaySoundInCSAudioAction_0, AssignFromStack_UIPlaySoundInCSAudioAction_0);
            field = type.GetField("UIPlayChatVoiceAction", flag);
            app.RegisterCLRFieldGetter(field, get_UIPlayChatVoiceAction_1);
            app.RegisterCLRFieldSetter(field, set_UIPlayChatVoiceAction_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_UIPlayChatVoiceAction_1, AssignFromStack_UIPlayChatVoiceAction_1);
            field = type.GetField("UIStopChatVoiceAction", flag);
            app.RegisterCLRFieldGetter(field, get_UIStopChatVoiceAction_2);
            app.RegisterCLRFieldSetter(field, set_UIStopChatVoiceAction_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_UIStopChatVoiceAction_2, AssignFromStack_UIStopChatVoiceAction_2);


        }



        static object get_UIPlaySoundInCSAudioAction_0(ref object o)
        {
            return global::HotFix_InvokeThird.UIPlaySoundInCSAudioAction;
        }

        static StackObject* CopyToStack_UIPlaySoundInCSAudioAction_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::HotFix_InvokeThird.UIPlaySoundInCSAudioAction;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_UIPlaySoundInCSAudioAction_0(ref object o, object v)
        {
            global::HotFix_InvokeThird.UIPlaySoundInCSAudioAction = (System.Action<System.Int32>)v;
        }

        static StackObject* AssignFromStack_UIPlaySoundInCSAudioAction_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Int32> @UIPlaySoundInCSAudioAction = (System.Action<System.Int32>)typeof(System.Action<System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::HotFix_InvokeThird.UIPlaySoundInCSAudioAction = @UIPlaySoundInCSAudioAction;
            return ptr_of_this_method;
        }

        static object get_UIPlayChatVoiceAction_1(ref object o)
        {
            return global::HotFix_InvokeThird.UIPlayChatVoiceAction;
        }

        static StackObject* CopyToStack_UIPlayChatVoiceAction_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::HotFix_InvokeThird.UIPlayChatVoiceAction;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_UIPlayChatVoiceAction_1(ref object o, object v)
        {
            global::HotFix_InvokeThird.UIPlayChatVoiceAction = (System.Func<System.String, System.Action, System.Boolean>)v;
        }

        static StackObject* AssignFromStack_UIPlayChatVoiceAction_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Func<System.String, System.Action, System.Boolean> @UIPlayChatVoiceAction = (System.Func<System.String, System.Action, System.Boolean>)typeof(System.Func<System.String, System.Action, System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::HotFix_InvokeThird.UIPlayChatVoiceAction = @UIPlayChatVoiceAction;
            return ptr_of_this_method;
        }

        static object get_UIStopChatVoiceAction_2(ref object o)
        {
            return global::HotFix_InvokeThird.UIStopChatVoiceAction;
        }

        static StackObject* CopyToStack_UIStopChatVoiceAction_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::HotFix_InvokeThird.UIStopChatVoiceAction;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_UIStopChatVoiceAction_2(ref object o, object v)
        {
            global::HotFix_InvokeThird.UIStopChatVoiceAction = (System.Action)v;
        }

        static StackObject* AssignFromStack_UIStopChatVoiceAction_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @UIStopChatVoiceAction = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::HotFix_InvokeThird.UIStopChatVoiceAction = @UIStopChatVoiceAction;
            return ptr_of_this_method;
        }



    }
}
