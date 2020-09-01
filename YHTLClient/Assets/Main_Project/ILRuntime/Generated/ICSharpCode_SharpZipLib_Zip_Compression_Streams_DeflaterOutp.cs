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
    unsafe class ICSharpCode_SharpZipLib_Zip_Compression_Streams_DeflaterOutputStream_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream);
            args = new Type[]{};
            method = type.GetMethod("Finish", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Finish_0);


        }


        static StackObject* Finish_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream instance_of_this_method = (ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream)typeof(ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Finish();

            return __ret;
        }



    }
}
