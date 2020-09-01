using UnityEngine;
using System.Collections.Generic;
using ILRuntime.Other;
using System;
using System.Collections;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Method;

public class ISFResUpdateMgrAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return null;
        }
    }

    public override Type[] BaseCLRTypes
    {
        get
        {
            return new Type[] { typeof(ISFResUpdateMgr) };
        }
    }

    public override Type AdaptorType
    {
        get
        {
            return typeof(Adaptor);
        }
    }

    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }
    
    internal class Adaptor : ISFResUpdateMgr, CrossBindingAdaptorType
    {
        ILTypeInstance instance;
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;

        public Adaptor()
        {

        }

        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }

        public ILTypeInstance ILInstance { get { return instance; } }

        public void AddToCompleteQueue(Resource res)
        {
            throw new NotImplementedException();
        }

        public void AddToDownloadQueue(Resource res)
        {
            throw new NotImplementedException();
        }

        public bool CheckIsNeedDownload(string relatePath)
        {
            throw new NotImplementedException();
        }

        public int GetResourceType(string relatePath)
        {
            throw new NotImplementedException();
        }
    }
}