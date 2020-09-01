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
    unsafe class BagWrapContent_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::BagWrapContent);

            field = type.GetField("itemSize", flag);
            app.RegisterCLRFieldGetter(field, get_itemSize_0);
            app.RegisterCLRFieldSetter(field, set_itemSize_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_itemSize_0, AssignFromStack_itemSize_0);
            field = type.GetField("minIndex", flag);
            app.RegisterCLRFieldGetter(field, get_minIndex_1);
            app.RegisterCLRFieldSetter(field, set_minIndex_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_minIndex_1, AssignFromStack_minIndex_1);
            field = type.GetField("maxIndex", flag);
            app.RegisterCLRFieldGetter(field, get_maxIndex_2);
            app.RegisterCLRFieldSetter(field, set_maxIndex_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_maxIndex_2, AssignFromStack_maxIndex_2);
            field = type.GetField("onInitializeItem", flag);
            app.RegisterCLRFieldGetter(field, get_onInitializeItem_3);
            app.RegisterCLRFieldSetter(field, set_onInitializeItem_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_onInitializeItem_3, AssignFromStack_onInitializeItem_3);


        }



        static object get_itemSize_0(ref object o)
        {
            return ((global::BagWrapContent)o).itemSize;
        }

        static StackObject* CopyToStack_itemSize_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::BagWrapContent)o).itemSize;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_itemSize_0(ref object o, object v)
        {
            ((global::BagWrapContent)o).itemSize = (System.Int32)v;
        }

        static StackObject* AssignFromStack_itemSize_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @itemSize = ptr_of_this_method->Value;
            ((global::BagWrapContent)o).itemSize = @itemSize;
            return ptr_of_this_method;
        }

        static object get_minIndex_1(ref object o)
        {
            return ((global::BagWrapContent)o).minIndex;
        }

        static StackObject* CopyToStack_minIndex_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::BagWrapContent)o).minIndex;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_minIndex_1(ref object o, object v)
        {
            ((global::BagWrapContent)o).minIndex = (System.Int32)v;
        }

        static StackObject* AssignFromStack_minIndex_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @minIndex = ptr_of_this_method->Value;
            ((global::BagWrapContent)o).minIndex = @minIndex;
            return ptr_of_this_method;
        }

        static object get_maxIndex_2(ref object o)
        {
            return ((global::BagWrapContent)o).maxIndex;
        }

        static StackObject* CopyToStack_maxIndex_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::BagWrapContent)o).maxIndex;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_maxIndex_2(ref object o, object v)
        {
            ((global::BagWrapContent)o).maxIndex = (System.Int32)v;
        }

        static StackObject* AssignFromStack_maxIndex_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @maxIndex = ptr_of_this_method->Value;
            ((global::BagWrapContent)o).maxIndex = @maxIndex;
            return ptr_of_this_method;
        }

        static object get_onInitializeItem_3(ref object o)
        {
            return ((global::BagWrapContent)o).onInitializeItem;
        }

        static StackObject* CopyToStack_onInitializeItem_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::BagWrapContent)o).onInitializeItem;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onInitializeItem_3(ref object o, object v)
        {
            ((global::BagWrapContent)o).onInitializeItem = (global::BagWrapContent.OnInitializeItem)v;
        }

        static StackObject* AssignFromStack_onInitializeItem_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::BagWrapContent.OnInitializeItem @onInitializeItem = (global::BagWrapContent.OnInitializeItem)typeof(global::BagWrapContent.OnInitializeItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::BagWrapContent)o).onInitializeItem = @onInitializeItem;
            return ptr_of_this_method;
        }



    }
}
