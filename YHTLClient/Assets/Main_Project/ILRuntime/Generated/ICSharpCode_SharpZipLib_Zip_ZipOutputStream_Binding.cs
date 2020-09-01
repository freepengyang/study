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
    unsafe class ICSharpCode_SharpZipLib_Zip_ZipOutputStream_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ICSharpCode.SharpZipLib.Zip.ZipOutputStream);
            args = new Type[]{typeof(ICSharpCode.SharpZipLib.Zip.ZipEntry)};
            method = type.GetMethod("PutNextEntry", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PutNextEntry_0);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("SetLevel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetLevel_1);

            args = new Type[]{typeof(System.IO.Stream)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* PutNextEntry_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ICSharpCode.SharpZipLib.Zip.ZipEntry @entry = (ICSharpCode.SharpZipLib.Zip.ZipEntry)typeof(ICSharpCode.SharpZipLib.Zip.ZipEntry).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ICSharpCode.SharpZipLib.Zip.ZipOutputStream instance_of_this_method = (ICSharpCode.SharpZipLib.Zip.ZipOutputStream)typeof(ICSharpCode.SharpZipLib.Zip.ZipOutputStream).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.PutNextEntry(@entry);

            return __ret;
        }

        static StackObject* SetLevel_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @level = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ICSharpCode.SharpZipLib.Zip.ZipOutputStream instance_of_this_method = (ICSharpCode.SharpZipLib.Zip.ZipOutputStream)typeof(ICSharpCode.SharpZipLib.Zip.ZipOutputStream).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetLevel(@level);

            return __ret;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.IO.Stream @baseOutputStream = (System.IO.Stream)typeof(System.IO.Stream).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(@baseOutputStream);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
