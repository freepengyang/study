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
    unsafe class UITweener_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UITweener);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(global::TweenScale)};
            if (genericMethods.TryGetValue("Begin", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(global::TweenScale), typeof(UnityEngine.GameObject), typeof(System.Single)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Begin_0);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(global::TweenAlpha)};
            if (genericMethods.TryGetValue("Begin", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(global::TweenAlpha), typeof(UnityEngine.GameObject), typeof(System.Single)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Begin_1);

                        break;
                    }
                }
            }
            args = new Type[]{};
            method = type.GetMethod("PlayForward", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayForward_2);
            args = new Type[]{typeof(global::TweenPosition)};
            if (genericMethods.TryGetValue("Begin", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(global::TweenPosition), typeof(UnityEngine.GameObject), typeof(System.Single)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Begin_3);

                        break;
                    }
                }
            }
            args = new Type[]{};
            method = type.GetMethod("ResetToBeginning", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResetToBeginning_4);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("SetOnFinished", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetOnFinished_5);
            args = new Type[]{};
            method = type.GetMethod("get_tweenFactor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_tweenFactor_6);
            args = new Type[]{};
            method = type.GetMethod("PlayReverse", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayReverse_7);
            args = new Type[]{};
            method = type.GetMethod("ResetToFrom", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResetToFrom_8);
            args = new Type[]{};
            method = type.GetMethod("PlayTween", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayTween_9);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("Play", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Play_10);
            args = new Type[]{typeof(global::EventDelegate)};
            method = type.GetMethod("AddOnFinished", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddOnFinished_11);
            args = new Type[]{typeof(System.Single), typeof(System.Boolean)};
            method = type.GetMethod("Sample", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Sample_12);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("AddOnFinished", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddOnFinished_13);
            args = new Type[]{typeof(global::EventDelegate)};
            method = type.GetMethod("RemoveOnFinished", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveOnFinished_14);

            field = type.GetField("delay", flag);
            app.RegisterCLRFieldGetter(field, get_delay_0);
            app.RegisterCLRFieldSetter(field, set_delay_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_delay_0, AssignFromStack_delay_0);
            field = type.GetField("duration", flag);
            app.RegisterCLRFieldGetter(field, get_duration_1);
            app.RegisterCLRFieldSetter(field, set_duration_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_duration_1, AssignFromStack_duration_1);
            field = type.GetField("onFinished", flag);
            app.RegisterCLRFieldGetter(field, get_onFinished_2);
            app.RegisterCLRFieldSetter(field, set_onFinished_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_onFinished_2, AssignFromStack_onFinished_2);
            field = type.GetField("tweenGroup", flag);
            app.RegisterCLRFieldGetter(field, get_tweenGroup_3);
            app.RegisterCLRFieldSetter(field, set_tweenGroup_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_tweenGroup_3, AssignFromStack_tweenGroup_3);
            field = type.GetField("animationCurve", flag);
            app.RegisterCLRFieldGetter(field, get_animationCurve_4);
            app.RegisterCLRFieldSetter(field, set_animationCurve_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_animationCurve_4, AssignFromStack_animationCurve_4);


        }


        static StackObject* Begin_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::UITweener.Begin<global::TweenScale>(@go, @duration);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Begin_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::UITweener.Begin<global::TweenAlpha>(@go, @duration);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* PlayForward_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.PlayForward();

            return __ret;
        }

        static StackObject* Begin_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::UITweener.Begin<global::TweenPosition>(@go, @duration);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ResetToBeginning_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ResetToBeginning();

            return __ret;
        }

        static StackObject* SetOnFinished_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @del = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetOnFinished(@del);

            return __ret;
        }

        static StackObject* get_tweenFactor_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.tweenFactor;

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* PlayReverse_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.PlayReverse();

            return __ret;
        }

        static StackObject* ResetToFrom_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ResetToFrom();

            return __ret;
        }

        static StackObject* PlayTween_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.PlayTween();

            return __ret;
        }

        static StackObject* Play_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @forward = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Play(@forward);

            return __ret;
        }

        static StackObject* AddOnFinished_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::EventDelegate @del = (global::EventDelegate)typeof(global::EventDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddOnFinished(@del);

            return __ret;
        }

        static StackObject* Sample_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isFinished = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @factor = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Sample(@factor, @isFinished);

            return __ret;
        }

        static StackObject* AddOnFinished_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @del = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddOnFinished(@del);

            return __ret;
        }

        static StackObject* RemoveOnFinished_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::EventDelegate @del = (global::EventDelegate)typeof(global::EventDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UITweener instance_of_this_method = (global::UITweener)typeof(global::UITweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveOnFinished(@del);

            return __ret;
        }


        static object get_delay_0(ref object o)
        {
            return ((global::UITweener)o).delay;
        }

        static StackObject* CopyToStack_delay_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UITweener)o).delay;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_delay_0(ref object o, object v)
        {
            ((global::UITweener)o).delay = (System.Single)v;
        }

        static StackObject* AssignFromStack_delay_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @delay = *(float*)&ptr_of_this_method->Value;
            ((global::UITweener)o).delay = @delay;
            return ptr_of_this_method;
        }

        static object get_duration_1(ref object o)
        {
            return ((global::UITweener)o).duration;
        }

        static StackObject* CopyToStack_duration_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UITweener)o).duration;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_duration_1(ref object o, object v)
        {
            ((global::UITweener)o).duration = (System.Single)v;
        }

        static StackObject* AssignFromStack_duration_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @duration = *(float*)&ptr_of_this_method->Value;
            ((global::UITweener)o).duration = @duration;
            return ptr_of_this_method;
        }

        static object get_onFinished_2(ref object o)
        {
            return ((global::UITweener)o).onFinished;
        }

        static StackObject* CopyToStack_onFinished_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UITweener)o).onFinished;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onFinished_2(ref object o, object v)
        {
            ((global::UITweener)o).onFinished = (System.Collections.Generic.List<global::EventDelegate>)v;
        }

        static StackObject* AssignFromStack_onFinished_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<global::EventDelegate> @onFinished = (System.Collections.Generic.List<global::EventDelegate>)typeof(System.Collections.Generic.List<global::EventDelegate>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UITweener)o).onFinished = @onFinished;
            return ptr_of_this_method;
        }

        static object get_tweenGroup_3(ref object o)
        {
            return ((global::UITweener)o).tweenGroup;
        }

        static StackObject* CopyToStack_tweenGroup_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UITweener)o).tweenGroup;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_tweenGroup_3(ref object o, object v)
        {
            ((global::UITweener)o).tweenGroup = (System.Int32)v;
        }

        static StackObject* AssignFromStack_tweenGroup_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @tweenGroup = ptr_of_this_method->Value;
            ((global::UITweener)o).tweenGroup = @tweenGroup;
            return ptr_of_this_method;
        }

        static object get_animationCurve_4(ref object o)
        {
            return ((global::UITweener)o).animationCurve;
        }

        static StackObject* CopyToStack_animationCurve_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UITweener)o).animationCurve;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_animationCurve_4(ref object o, object v)
        {
            ((global::UITweener)o).animationCurve = (UnityEngine.AnimationCurve)v;
        }

        static StackObject* AssignFromStack_animationCurve_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.AnimationCurve @animationCurve = (UnityEngine.AnimationCurve)typeof(UnityEngine.AnimationCurve).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UITweener)o).animationCurve = @animationCurve;
            return ptr_of_this_method;
        }



    }
}
