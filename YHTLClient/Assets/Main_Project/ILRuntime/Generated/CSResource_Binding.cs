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
    unsafe class CSResource_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CSResource);
            args = new Type[]{};
            method = type.GetMethod("GetObjInst", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetObjInst_0);
            args = new Type[]{};
            method = type.GetMethod("ReleaseCallBack", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ReleaseCallBack_1);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("set_FileName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_FileName_2);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_IsCanBeDelete", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_IsCanBeDelete_3);
            args = new Type[]{};
            method = type.GetMethod("get_FileName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_FileName_4);
            args = new Type[]{};
            method = type.GetMethod("get_Path", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Path_5);
            args = new Type[]{};
            method = type.GetMethod("get_mCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_mCount_6);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_mCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_mCount_7);

            field = type.GetField("MirrorObj", flag);
            app.RegisterCLRFieldGetter(field, get_MirrorObj_0);
            app.RegisterCLRFieldSetter(field, set_MirrorObj_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_MirrorObj_0, AssignFromStack_MirrorObj_0);
            field = type.GetField("MirroyBytes", flag);
            app.RegisterCLRFieldGetter(field, get_MirroyBytes_1);
            app.RegisterCLRFieldSetter(field, set_MirroyBytes_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_MirroyBytes_1, AssignFromStack_MirroyBytes_1);
            field = type.GetField("IsDone", flag);
            app.RegisterCLRFieldGetter(field, get_IsDone_2);
            app.RegisterCLRFieldSetter(field, set_IsDone_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_IsDone_2, AssignFromStack_IsDone_2);


        }


        static StackObject* GetObjInst_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResource instance_of_this_method = (global::CSResource)typeof(global::CSResource).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetObjInst();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ReleaseCallBack_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResource instance_of_this_method = (global::CSResource)typeof(global::CSResource).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ReleaseCallBack();

            return __ret;
        }

        static StackObject* set_FileName_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSResource instance_of_this_method = (global::CSResource)typeof(global::CSResource).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.FileName = value;

            return __ret;
        }

        static StackObject* set_IsCanBeDelete_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSResource instance_of_this_method = (global::CSResource)typeof(global::CSResource).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.IsCanBeDelete = value;

            return __ret;
        }

        static StackObject* get_FileName_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResource instance_of_this_method = (global::CSResource)typeof(global::CSResource).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.FileName;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_Path_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResource instance_of_this_method = (global::CSResource)typeof(global::CSResource).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Path;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_mCount_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::CSResource instance_of_this_method = (global::CSResource)typeof(global::CSResource).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.mCount;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* set_mCount_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::CSResource instance_of_this_method = (global::CSResource)typeof(global::CSResource).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.mCount = value;

            return __ret;
        }


        static object get_MirrorObj_0(ref object o)
        {
            return ((global::CSResource)o).MirrorObj;
        }

        static StackObject* CopyToStack_MirrorObj_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSResource)o).MirrorObj;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_MirrorObj_0(ref object o, object v)
        {
            ((global::CSResource)o).MirrorObj = (UnityEngine.Object)v;
        }

        static StackObject* AssignFromStack_MirrorObj_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Object @MirrorObj = (UnityEngine.Object)typeof(UnityEngine.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSResource)o).MirrorObj = @MirrorObj;
            return ptr_of_this_method;
        }

        static object get_MirroyBytes_1(ref object o)
        {
            return ((global::CSResource)o).MirroyBytes;
        }

        static StackObject* CopyToStack_MirroyBytes_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSResource)o).MirroyBytes;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_MirroyBytes_1(ref object o, object v)
        {
            ((global::CSResource)o).MirroyBytes = (System.Byte[])v;
        }

        static StackObject* AssignFromStack_MirroyBytes_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Byte[] @MirroyBytes = (System.Byte[])typeof(System.Byte[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::CSResource)o).MirroyBytes = @MirroyBytes;
            return ptr_of_this_method;
        }

        static object get_IsDone_2(ref object o)
        {
            return ((global::CSResource)o).IsDone;
        }

        static StackObject* CopyToStack_IsDone_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CSResource)o).IsDone;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_IsDone_2(ref object o, object v)
        {
            ((global::CSResource)o).IsDone = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_IsDone_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @IsDone = ptr_of_this_method->Value == 1;
            ((global::CSResource)o).IsDone = @IsDone;
            return ptr_of_this_method;
        }



    }
}
