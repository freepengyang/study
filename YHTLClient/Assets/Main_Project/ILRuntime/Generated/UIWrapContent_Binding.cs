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
    unsafe class UIWrapContent_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIWrapContent);
            args = new Type[]{};
            method = type.GetMethod("ResetChildPositions", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResetChildPositions_0);
            args = new Type[]{};
            method = type.GetMethod("SortBasedOnScrollMovement", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SortBasedOnScrollMovement_1);

            field = type.GetField("onInitializeItem", flag);
            app.RegisterCLRFieldGetter(field, get_onInitializeItem_0);
            app.RegisterCLRFieldSetter(field, set_onInitializeItem_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onInitializeItem_0, AssignFromStack_onInitializeItem_0);
            field = type.GetField("maxIndex", flag);
            app.RegisterCLRFieldGetter(field, get_maxIndex_1);
            app.RegisterCLRFieldSetter(field, set_maxIndex_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_maxIndex_1, AssignFromStack_maxIndex_1);
            field = type.GetField("minIndex", flag);
            app.RegisterCLRFieldGetter(field, get_minIndex_2);
            app.RegisterCLRFieldSetter(field, set_minIndex_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_minIndex_2, AssignFromStack_minIndex_2);
            field = type.GetField("itemSize", flag);
            app.RegisterCLRFieldGetter(field, get_itemSize_3);
            app.RegisterCLRFieldSetter(field, set_itemSize_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_itemSize_3, AssignFromStack_itemSize_3);
            field = type.GetField("cullContent", flag);
            app.RegisterCLRFieldGetter(field, get_cullContent_4);
            app.RegisterCLRFieldSetter(field, set_cullContent_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_cullContent_4, AssignFromStack_cullContent_4);


        }


        static StackObject* ResetChildPositions_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIWrapContent instance_of_this_method = (global::UIWrapContent)typeof(global::UIWrapContent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ResetChildPositions();

            return __ret;
        }

        static StackObject* SortBasedOnScrollMovement_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIWrapContent instance_of_this_method = (global::UIWrapContent)typeof(global::UIWrapContent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SortBasedOnScrollMovement();

            return __ret;
        }


        static object get_onInitializeItem_0(ref object o)
        {
            return ((global::UIWrapContent)o).onInitializeItem;
        }

        static StackObject* CopyToStack_onInitializeItem_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIWrapContent)o).onInitializeItem;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onInitializeItem_0(ref object o, object v)
        {
            ((global::UIWrapContent)o).onInitializeItem = (global::UIWrapContent.OnInitializeItem)v;
        }

        static StackObject* AssignFromStack_onInitializeItem_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UIWrapContent.OnInitializeItem @onInitializeItem = (global::UIWrapContent.OnInitializeItem)typeof(global::UIWrapContent.OnInitializeItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIWrapContent)o).onInitializeItem = @onInitializeItem;
            return ptr_of_this_method;
        }

        static object get_maxIndex_1(ref object o)
        {
            return ((global::UIWrapContent)o).maxIndex;
        }

        static StackObject* CopyToStack_maxIndex_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIWrapContent)o).maxIndex;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_maxIndex_1(ref object o, object v)
        {
            ((global::UIWrapContent)o).maxIndex = (System.Int32)v;
        }

        static StackObject* AssignFromStack_maxIndex_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @maxIndex = ptr_of_this_method->Value;
            ((global::UIWrapContent)o).maxIndex = @maxIndex;
            return ptr_of_this_method;
        }

        static object get_minIndex_2(ref object o)
        {
            return ((global::UIWrapContent)o).minIndex;
        }

        static StackObject* CopyToStack_minIndex_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIWrapContent)o).minIndex;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_minIndex_2(ref object o, object v)
        {
            ((global::UIWrapContent)o).minIndex = (System.Int32)v;
        }

        static StackObject* AssignFromStack_minIndex_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @minIndex = ptr_of_this_method->Value;
            ((global::UIWrapContent)o).minIndex = @minIndex;
            return ptr_of_this_method;
        }

        static object get_itemSize_3(ref object o)
        {
            return ((global::UIWrapContent)o).itemSize;
        }

        static StackObject* CopyToStack_itemSize_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIWrapContent)o).itemSize;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_itemSize_3(ref object o, object v)
        {
            ((global::UIWrapContent)o).itemSize = (System.Int32)v;
        }

        static StackObject* AssignFromStack_itemSize_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @itemSize = ptr_of_this_method->Value;
            ((global::UIWrapContent)o).itemSize = @itemSize;
            return ptr_of_this_method;
        }

        static object get_cullContent_4(ref object o)
        {
            return ((global::UIWrapContent)o).cullContent;
        }

        static StackObject* CopyToStack_cullContent_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIWrapContent)o).cullContent;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_cullContent_4(ref object o, object v)
        {
            ((global::UIWrapContent)o).cullContent = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_cullContent_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @cullContent = ptr_of_this_method->Value == 1;
            ((global::UIWrapContent)o).cullContent = @cullContent;
            return ptr_of_this_method;
        }



    }
}
