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
    unsafe class CSMisc_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSMisc);
            args = new Type[]{typeof(global::CSCell), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("GetDepth", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetDepth_0);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32), typeof(System.Int32), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("GetNearPoint", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetNearPoint_1);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("GetCombineModel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetCombineModel_2);

            field = type.GetField("dirList", flag);
            app.RegisterCLRFieldGetter(field, get_dirList_0);
            app.RegisterCLRFieldSetter(field, set_dirList_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_dirList_0, AssignFromStack_dirList_0);
            field = type.GetField("ItemQulityColorDic", flag);
            app.RegisterCLRFieldGetter(field, get_ItemQulityColorDic_1);
            app.RegisterCLRFieldSetter(field, set_ItemQulityColorDic_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_ItemQulityColorDic_1, AssignFromStack_ItemQulityColorDic_1);
            field = type.GetField("dirMove", flag);
            app.RegisterCLRFieldGetter(field, get_dirMove_2);
            app.RegisterCLRFieldSetter(field, set_dirMove_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_dirMove_2, AssignFromStack_dirMove_2);


        }


        static StackObject* GetDepth_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @avatarType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @depthType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::CSCell @cell = (global::CSCell)typeof(global::CSCell).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::CSMisc.GetDepth(@cell, @depthType, @avatarType);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetNearPoint_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @radius = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @sY = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @sX = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.Int32 @x = ptr_of_this_method->Value;


            var result_of_this_method = global::CSMisc.GetNearPoint(@x, @y, @sX, @sY, @radius);

            if (ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_CSMisc_Binding_Dot2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* GetCombineModel_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @avaterDirection = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @avaterMotion = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @model = ptr_of_this_method->Value;


            var result_of_this_method = global::CSMisc.GetCombineModel(@model, @avaterMotion, @avaterDirection);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_dirList_0(ref object o)
        {
            return global::CSMisc.dirList;
        }

        static StackObject* CopyToStack_dirList_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSMisc.dirList;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_dirList_0(ref object o, object v)
        {
            global::CSMisc.dirList = (System.Collections.Generic.List<global::CSMisc.Dot2>)v;
        }

        static StackObject* AssignFromStack_dirList_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<global::CSMisc.Dot2> @dirList = (System.Collections.Generic.List<global::CSMisc.Dot2>)typeof(System.Collections.Generic.List<global::CSMisc.Dot2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::CSMisc.dirList = @dirList;
            return ptr_of_this_method;
        }

        static object get_ItemQulityColorDic_1(ref object o)
        {
            return global::CSMisc.ItemQulityColorDic;
        }

        static StackObject* CopyToStack_ItemQulityColorDic_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSMisc.ItemQulityColorDic;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ItemQulityColorDic_1(ref object o, object v)
        {
            global::CSMisc.ItemQulityColorDic = (System.Collections.Generic.Dictionary<System.UInt32, UnityEngine.Color>)v;
        }

        static StackObject* AssignFromStack_ItemQulityColorDic_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.Dictionary<System.UInt32, UnityEngine.Color> @ItemQulityColorDic = (System.Collections.Generic.Dictionary<System.UInt32, UnityEngine.Color>)typeof(System.Collections.Generic.Dictionary<System.UInt32, UnityEngine.Color>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::CSMisc.ItemQulityColorDic = @ItemQulityColorDic;
            return ptr_of_this_method;
        }

        static object get_dirMove_2(ref object o)
        {
            return global::CSMisc.dirMove;
        }

        static StackObject* CopyToStack_dirMove_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::CSMisc.dirMove;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_dirMove_2(ref object o, object v)
        {
            global::CSMisc.dirMove = (System.Collections.Generic.Dictionary<System.Int32, global::CSMisc.Dot2>)v;
        }

        static StackObject* AssignFromStack_dirMove_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.Dictionary<System.Int32, global::CSMisc.Dot2> @dirMove = (System.Collections.Generic.Dictionary<System.Int32, global::CSMisc.Dot2>)typeof(System.Collections.Generic.Dictionary<System.Int32, global::CSMisc.Dot2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::CSMisc.dirMove = @dirMove;
            return ptr_of_this_method;
        }



    }
}
