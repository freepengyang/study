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
    unsafe class NGUIText_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::NGUIText);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("StripSymbols", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StripSymbols_0);
            args = new Type[]{typeof(System.String), typeof(System.Int32)};
            method = type.GetMethod("CalculateCharacterSize", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CalculateCharacterSize_1);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("Update", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Update_2);

            field = type.GetField("fontSize", flag);
            app.RegisterCLRFieldGetter(field, get_fontSize_0);
            app.RegisterCLRFieldSetter(field, set_fontSize_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_fontSize_0, AssignFromStack_fontSize_0);
            field = type.GetField("rectWidth", flag);
            app.RegisterCLRFieldGetter(field, get_rectWidth_1);
            app.RegisterCLRFieldSetter(field, set_rectWidth_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_rectWidth_1, AssignFromStack_rectWidth_1);
            field = type.GetField("dynamicFont", flag);
            app.RegisterCLRFieldGetter(field, get_dynamicFont_2);
            app.RegisterCLRFieldSetter(field, set_dynamicFont_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_dynamicFont_2, AssignFromStack_dynamicFont_2);
            field = type.GetField("fontScale", flag);
            app.RegisterCLRFieldGetter(field, get_fontScale_3);
            app.RegisterCLRFieldSetter(field, set_fontScale_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_fontScale_3, AssignFromStack_fontScale_3);
            field = type.GetField("encoding", flag);
            app.RegisterCLRFieldGetter(field, get_encoding_4);
            app.RegisterCLRFieldSetter(field, set_encoding_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_encoding_4, AssignFromStack_encoding_4);
            field = type.GetField("spacingX", flag);
            app.RegisterCLRFieldGetter(field, get_spacingX_5);
            app.RegisterCLRFieldSetter(field, set_spacingX_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_spacingX_5, AssignFromStack_spacingX_5);
            field = type.GetField("spacingY", flag);
            app.RegisterCLRFieldGetter(field, get_spacingY_6);
            app.RegisterCLRFieldSetter(field, set_spacingY_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_spacingY_6, AssignFromStack_spacingY_6);


        }


        static StackObject* StripSymbols_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @text = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::NGUIText.StripSymbols(@text);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* CalculateCharacterSize_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @maxWidth = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @text = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::NGUIText.CalculateCharacterSize(@text, @maxWidth);

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* Update_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @request = ptr_of_this_method->Value == 1;


            global::NGUIText.Update(@request);

            return __ret;
        }


        static object get_fontSize_0(ref object o)
        {
            return global::NGUIText.fontSize;
        }

        static StackObject* CopyToStack_fontSize_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::NGUIText.fontSize;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_fontSize_0(ref object o, object v)
        {
            global::NGUIText.fontSize = (System.Int32)v;
        }

        static StackObject* AssignFromStack_fontSize_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @fontSize = ptr_of_this_method->Value;
            global::NGUIText.fontSize = @fontSize;
            return ptr_of_this_method;
        }

        static object get_rectWidth_1(ref object o)
        {
            return global::NGUIText.rectWidth;
        }

        static StackObject* CopyToStack_rectWidth_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::NGUIText.rectWidth;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_rectWidth_1(ref object o, object v)
        {
            global::NGUIText.rectWidth = (System.Int32)v;
        }

        static StackObject* AssignFromStack_rectWidth_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @rectWidth = ptr_of_this_method->Value;
            global::NGUIText.rectWidth = @rectWidth;
            return ptr_of_this_method;
        }

        static object get_dynamicFont_2(ref object o)
        {
            return global::NGUIText.dynamicFont;
        }

        static StackObject* CopyToStack_dynamicFont_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::NGUIText.dynamicFont;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_dynamicFont_2(ref object o, object v)
        {
            global::NGUIText.dynamicFont = (UnityEngine.Font)v;
        }

        static StackObject* AssignFromStack_dynamicFont_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Font @dynamicFont = (UnityEngine.Font)typeof(UnityEngine.Font).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::NGUIText.dynamicFont = @dynamicFont;
            return ptr_of_this_method;
        }

        static object get_fontScale_3(ref object o)
        {
            return global::NGUIText.fontScale;
        }

        static StackObject* CopyToStack_fontScale_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::NGUIText.fontScale;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_fontScale_3(ref object o, object v)
        {
            global::NGUIText.fontScale = (System.Single)v;
        }

        static StackObject* AssignFromStack_fontScale_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @fontScale = *(float*)&ptr_of_this_method->Value;
            global::NGUIText.fontScale = @fontScale;
            return ptr_of_this_method;
        }

        static object get_encoding_4(ref object o)
        {
            return global::NGUIText.encoding;
        }

        static StackObject* CopyToStack_encoding_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::NGUIText.encoding;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_encoding_4(ref object o, object v)
        {
            global::NGUIText.encoding = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_encoding_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @encoding = ptr_of_this_method->Value == 1;
            global::NGUIText.encoding = @encoding;
            return ptr_of_this_method;
        }

        static object get_spacingX_5(ref object o)
        {
            return global::NGUIText.spacingX;
        }

        static StackObject* CopyToStack_spacingX_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::NGUIText.spacingX;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_spacingX_5(ref object o, object v)
        {
            global::NGUIText.spacingX = (System.Single)v;
        }

        static StackObject* AssignFromStack_spacingX_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @spacingX = *(float*)&ptr_of_this_method->Value;
            global::NGUIText.spacingX = @spacingX;
            return ptr_of_this_method;
        }

        static object get_spacingY_6(ref object o)
        {
            return global::NGUIText.spacingY;
        }

        static StackObject* CopyToStack_spacingY_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::NGUIText.spacingY;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_spacingY_6(ref object o, object v)
        {
            global::NGUIText.spacingY = (System.Single)v;
        }

        static StackObject* AssignFromStack_spacingY_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @spacingY = *(float*)&ptr_of_this_method->Value;
            global::NGUIText.spacingY = @spacingY;
            return ptr_of_this_method;
        }



    }
}
