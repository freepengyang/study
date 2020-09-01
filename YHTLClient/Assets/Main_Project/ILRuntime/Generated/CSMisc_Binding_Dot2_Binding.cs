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
    unsafe class CSMisc_Binding_Dot2_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSMisc.Dot2);
            args = new Type[]{typeof(global::CSMisc.Dot2)};
            method = type.GetMethod("Equal", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Equal_0);
            args = new Type[]{};
            method = type.GetMethod("get_Zero", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Zero_1);
            args = new Type[]{typeof(global::CSMisc.Dot2), typeof(global::CSMisc.Dot2)};
            method = type.GetMethod("op_Subtraction", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_Subtraction_2);
            args = new Type[]{};
            method = type.GetMethod("Normal", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Normal_3);
            args = new Type[]{};
            method = type.GetMethod("Abs", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Abs_4);
            args = new Type[]{typeof(global::CSMisc.Dot2), typeof(global::CSMisc.Dot2)};
            method = type.GetMethod("op_Addition", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_Addition_5);
            args = new Type[]{};
            method = type.GetMethod("Clear", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Clear_6);
            args = new Type[]{typeof(global::CSMisc.Dot2), typeof(System.Int32)};
            method = type.GetMethod("op_Multiply", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_Multiply_7);

            field = type.GetField("x", flag);
            app.RegisterCLRFieldGetter(field, get_x_0);
            app.RegisterCLRFieldSetter(field, set_x_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_x_0, AssignFromStack_x_0);
            field = type.GetField("y", flag);
            app.RegisterCLRFieldGetter(field, get_y_1);
            app.RegisterCLRFieldSetter(field, set_y_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_y_1, AssignFromStack_y_1);

            app.RegisterCLRMemberwiseClone(type, PerformMemberwiseClone);

            app.RegisterCLRCreateDefaultInstance(type, () => new global::CSMisc.Dot2());

            args = new Type[]{typeof(System.Int32), typeof(System.Int32)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }

        static void WriteBackInstance(ILRuntime.Runtime.Enviorment.AppDomain __domain, StackObject* ptr_of_this_method, IList<object> __mStack, ref global::CSMisc.Dot2 instance_of_this_method)
        {
            ptr_of_this_method = ILIntepreter.GetObjectAndResolveReference(ptr_of_this_method);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.Object:
                    {
                        __mStack[ptr_of_this_method->Value] = instance_of_this_method;
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = instance_of_this_method;
                        }
                        else
                        {
                            var t = __domain.GetType(___obj.GetType()) as CLRType;
                            t.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, instance_of_this_method);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var t = __domain.GetType(ptr_of_this_method->Value);
                        if(t is ILType)
                        {
                            ((ILType)t).StaticInstance[ptr_of_this_method->ValueLow] = instance_of_this_method;
                        }
                        else
                        {
                            ((CLRType)t).SetStaticFieldValue(ptr_of_this_method->ValueLow, instance_of_this_method);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as global::CSMisc.Dot2[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = instance_of_this_method;
                    }
                    break;
            }
        }

        static StackObject* Equal_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMisc.Dot2 @dot = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @dot, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @dot = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSMisc.Dot2 instance_of_this_method = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref instance_of_this_method, __intp, ptr_of_this_method, __mStack, false);
            } else {
                ptr_of_this_method = ILIntepreter.GetObjectAndResolveReference(ptr_of_this_method);
                instance_of_this_method = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }

            var result_of_this_method = instance_of_this_method.Equal(@dot);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.WriteBackValue(__domain, ptr_of_this_method, __mStack, ref instance_of_this_method);
            } else {
                WriteBackInstance(__domain, ptr_of_this_method, __mStack, ref instance_of_this_method);
            }

            __intp.Free(ptr_of_this_method);
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_Zero_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::CSMisc.Dot2.Zero;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* op_Subtraction_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMisc.Dot2 @s = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @s, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @s = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSMisc.Dot2 @f = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @f, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @f = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }


            var result_of_this_method = f - s;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* Normal_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMisc.Dot2 instance_of_this_method = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref instance_of_this_method, __intp, ptr_of_this_method, __mStack, false);
            } else {
                ptr_of_this_method = ILIntepreter.GetObjectAndResolveReference(ptr_of_this_method);
                instance_of_this_method = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }

            var result_of_this_method = instance_of_this_method.Normal();

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.WriteBackValue(__domain, ptr_of_this_method, __mStack, ref instance_of_this_method);
            } else {
                WriteBackInstance(__domain, ptr_of_this_method, __mStack, ref instance_of_this_method);
            }

            __intp.Free(ptr_of_this_method);
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* Abs_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMisc.Dot2 instance_of_this_method = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref instance_of_this_method, __intp, ptr_of_this_method, __mStack, false);
            } else {
                ptr_of_this_method = ILIntepreter.GetObjectAndResolveReference(ptr_of_this_method);
                instance_of_this_method = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }

            var result_of_this_method = instance_of_this_method.Abs();

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.WriteBackValue(__domain, ptr_of_this_method, __mStack, ref instance_of_this_method);
            } else {
                WriteBackInstance(__domain, ptr_of_this_method, __mStack, ref instance_of_this_method);
            }

            __intp.Free(ptr_of_this_method);
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* op_Addition_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMisc.Dot2 @s = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @s, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @s = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSMisc.Dot2 @f = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @f, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @f = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }


            var result_of_this_method = f + s;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* Clear_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSMisc.Dot2 instance_of_this_method = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref instance_of_this_method, __intp, ptr_of_this_method, __mStack, false);
            } else {
                ptr_of_this_method = ILIntepreter.GetObjectAndResolveReference(ptr_of_this_method);
                instance_of_this_method = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }

            instance_of_this_method.Clear();

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.WriteBackValue(__domain, ptr_of_this_method, __mStack, ref instance_of_this_method);
            } else {
                WriteBackInstance(__domain, ptr_of_this_method, __mStack, ref instance_of_this_method);
            }

            __intp.Free(ptr_of_this_method);
            return __ret;
        }

        static StackObject* op_Multiply_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @i = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSMisc.Dot2 @f = new global::CSMisc.Dot2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.ParseValue(ref @f, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @f = (global::CSMisc.Dot2)typeof(global::CSMisc.Dot2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }


            var result_of_this_method = f * i;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }


        static object get_x_0(ref object o)
        {
            return ((global::CSMisc.Dot2)o).x;
        }

        static StackObject* CopyToStack_x_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSMisc.Dot2)o).x;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_x_0(ref object o, object v)
        {
            global::CSMisc.Dot2 ins =(global::CSMisc.Dot2)o;
            ins.x = (System.Int32)v;
            o = ins;
        }

        static StackObject* AssignFromStack_x_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @x = ptr_of_this_method->Value;
            global::CSMisc.Dot2 ins =(global::CSMisc.Dot2)o;
            ins.x = @x;
            o = ins;
            return ptr_of_this_method;
        }

        static object get_y_1(ref object o)
        {
            return ((global::CSMisc.Dot2)o).y;
        }

        static StackObject* CopyToStack_y_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSMisc.Dot2)o).y;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_y_1(ref object o, object v)
        {
            global::CSMisc.Dot2 ins =(global::CSMisc.Dot2)o;
            ins.y = (System.Int32)v;
            o = ins;
        }

        static StackObject* AssignFromStack_y_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @y = ptr_of_this_method->Value;
            global::CSMisc.Dot2 ins =(global::CSMisc.Dot2)o;
            ins.y = @y;
            o = ins;
            return ptr_of_this_method;
        }


        static object PerformMemberwiseClone(ref object o)
        {
            var ins = new global::CSMisc.Dot2();
            ins = (global::CSMisc.Dot2)o;
            return ins;
        }

        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @_y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @_x = ptr_of_this_method->Value;


            var result_of_this_method = new global::CSMisc.Dot2(@_x, @_y);

            if(!isNewObj)
            {
                __ret--;
                if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                    ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.WriteBackValue(__domain, __ret, __mStack, ref result_of_this_method);
                } else {
                    WriteBackInstance(__domain, __ret, __mStack, ref result_of_this_method);
                }
                return __ret;
            }

            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
                return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }


    }
}
