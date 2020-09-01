using System;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using ILRuntime.CLR.TypeSystem;
using System.Collections.Generic;

public class Adapt_ITable : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get { return typeof(ITable); }
    }

    public override Type AdaptorType
    {
        get { return typeof(Adaptor); }
    }

    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor : MyAdaptor, ITable
    {
        static Dictionary<ILType, AdaptHelper.AdaptMethod[]> ms_iltype_to_adaptmethods = new Dictionary<ILType, AdaptHelper.AdaptMethod[]>(512);

        public Adaptor(AppDomain appdomain, ILTypeInstance instance) : base(appdomain,instance)
        {

        }

        protected override AdaptHelper.AdaptMethod[] GetAdaptMethods()
        {
            var type = _instance.Type;
            AdaptHelper.AdaptMethod[] adaptMethods = null;
            if (!ms_iltype_to_adaptmethods.TryGetValue(type,out adaptMethods))
            {
                adaptMethods = new AdaptHelper.AdaptMethod[]
                {
                };
                ms_iltype_to_adaptmethods.Add(type, adaptMethods);
                //Debug.LogFormat("<color=#00ff00>[ILType]:[{0}]:[{1}]</color>", ms_iltype_to_adaptmethods.Count - 1,type.FullName);
            }
            return adaptMethods;
        }
    }
}
