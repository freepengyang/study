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
    unsafe class UISpriteAnimation_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UISpriteAnimation);
            args = new Type[]{};
            method = type.GetMethod("RebuildSpriteList", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RebuildSpriteList_0);
            args = new Type[]{};
            method = type.GetMethod("ResetToBeginning", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResetToBeginning_1);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_framesPerSecond", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_framesPerSecond_2);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_loop", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_loop_3);
            args = new Type[]{};
            method = type.GetMethod("get_AnimSprite", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_AnimSprite_4);
            args = new Type[]{};
            method = type.GetMethod("Play", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Play_5);

            field = type.GetField("OnFinish", flag);
            app.RegisterCLRFieldGetter(field, get_OnFinish_0);
            app.RegisterCLRFieldSetter(field, set_OnFinish_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnFinish_0, AssignFromStack_OnFinish_0);

            app.RegisterCLRCreateArrayInstance(type, s => new global::UISpriteAnimation[s]);


        }


        static StackObject* RebuildSpriteList_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UISpriteAnimation instance_of_this_method = (global::UISpriteAnimation)typeof(global::UISpriteAnimation).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RebuildSpriteList();

            return __ret;
        }

        static StackObject* ResetToBeginning_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UISpriteAnimation instance_of_this_method = (global::UISpriteAnimation)typeof(global::UISpriteAnimation).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ResetToBeginning();

            return __ret;
        }

        static StackObject* set_framesPerSecond_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UISpriteAnimation instance_of_this_method = (global::UISpriteAnimation)typeof(global::UISpriteAnimation).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.framesPerSecond = value;

            return __ret;
        }

        static StackObject* set_loop_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UISpriteAnimation instance_of_this_method = (global::UISpriteAnimation)typeof(global::UISpriteAnimation).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.loop = value;

            return __ret;
        }

        static StackObject* get_AnimSprite_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UISpriteAnimation instance_of_this_method = (global::UISpriteAnimation)typeof(global::UISpriteAnimation).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.AnimSprite;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Play_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UISpriteAnimation instance_of_this_method = (global::UISpriteAnimation)typeof(global::UISpriteAnimation).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Play();

            return __ret;
        }


        static object get_OnFinish_0(ref object o)
        {
            return ((global::UISpriteAnimation)o).OnFinish;
        }

        static StackObject* CopyToStack_OnFinish_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UISpriteAnimation)o).OnFinish;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnFinish_0(ref object o, object v)
        {
            ((global::UISpriteAnimation)o).OnFinish = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnFinish_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnFinish = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UISpriteAnimation)o).OnFinish = @OnFinish;
            return ptr_of_this_method;
        }



    }
}
