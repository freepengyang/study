using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

public class AvatarUnitAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(AvatarUnit);
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

    public class Adaptor : AvatarUnit, CrossBindingAdaptorType
    {
        ILTypeInstance instance;
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;

        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }

        public ILTypeInstance ILInstance { get { return instance; } }

        private object[] mParams = new object[0];

        //private IMethod mUpdateMethod;
        //private bool mUpdateMethodGot;
       
        //private bool isUpdateInvoking = false;
        //public override void Update()
        //{
        //    if (instance == null) return;

        //    if (!mUpdateMethodGot)
        //    {
        //        mUpdateMethod = instance.Type.GetMethod("Update", 0);
        //        mUpdateMethodGot = true;
        //    }

        //    if(mUpdateMethod != null && !isUpdateInvoking)
        //    {
        //        isUpdateInvoking = true;
        //        appdomain.Invoke(mUpdateMethod, instance, mParams);
        //        isUpdateInvoking = false;
        //    }
        //    //else
        //    //{
        //    //    base.Update();
        //    //}
        //}

        //private IMethod mUpdatePositionMethod;
        //private bool mUpdatePositionMethodGot;
        //private bool isUpdatePositionInvoking = false;
        //protected override void UpdatePosition()
        //{
        //    if (instance == null) return;
        //    if (!mUpdatePositionMethodGot)
        //    {
        //        mUpdatePositionMethod = instance.Type.GetMethod("UpdatePosition", 0);
        //        mUpdatePositionMethodGot = true;
        //    }
        //    if (mUpdatePositionMethod != null && !isUpdatePositionInvoking)
        //    {
        //        isUpdatePositionInvoking = true;
        //        appdomain.Invoke(mUpdatePositionMethod, instance, mParams);
        //        isUpdatePositionInvoking = false;
        //    }
        //    else
        //    {
        //        base.UpdatePosition();
        //    }
        //}

        private IMethod mOnOldCellChangeMethod;
        private bool mOnOldCellChangeMethodGot;
        private bool isOnOldCellChangeInvoking = false;
        public override void OnOldCellChange()
        {
            if (instance == null) return;
            if (!mOnOldCellChangeMethodGot)
            {
                mOnOldCellChangeMethod = instance.Type.GetMethod("OnOldCellChange", 0);
                mOnOldCellChangeMethodGot = true;
            }
            if (mOnOldCellChangeMethod != null && !isOnOldCellChangeInvoking)
            {
                isOnOldCellChangeInvoking = true;
                appdomain.Invoke(mOnOldCellChangeMethod, instance, mParams);
                isOnOldCellChangeInvoking = false;
            }
            else
            {
                base.OnOldCellChange();
            }
        }


        private IMethod mOnInitModelMethod;
        private bool mOnInitModelMethodGot;
        private bool isOnInitModeInvoking = false;

        public override void InitModel()
        {
            if (instance == null) return;
            if (!mOnInitModelMethodGot)
            {
                mOnInitModelMethod = instance.Type.GetMethod("InitModel", 0);
                mOnInitModelMethodGot = true;
            }
            if (mOnInitModelMethod != null && !isOnInitModeInvoking)
            {
                isOnInitModeInvoking = true;
                appdomain.Invoke(mOnInitModelMethod, instance, mParams);
                isOnInitModeInvoking = false;
            }
            else
            {
                base.InitModel();
            }
        }

        //private IMethod mMoveInitMethod;
        //private bool mOnMoveInitMethodGot;
        //private bool isMoveInitInvoking = false;
        //public override void MoveInit()
        //{
        //    if (instance == null) return;

        //    if (!mOnMoveInitMethodGot)
        //    {
        //        mMoveInitMethod = instance.Type.GetMethod("MoveInit", 0);
        //        mOnMoveInitMethodGot = true;
        //    }
        //    if (mMoveInitMethod != null && !isMoveInitInvoking)
        //    {
        //        isMoveInitInvoking = true;
        //        appdomain.Invoke(mMoveInitMethod, instance, mParams);
        //        isMoveInitInvoking = false;
        //    }
        //    else
        //    {
        //        base.MoveInit();
        //    }
        //}

        private IMethod mNextTargetMethod;
        private bool mNextTargetGot;
        private bool isNextTargetInvoking = false;
        public override Node NextTarget()
        {
            if (instance == null) return null;

            if (!mNextTargetGot)
            {
                mNextTargetMethod = instance.Type.GetMethod("NextTarget", 0);
                mNextTargetGot = true;
            }
            if (mNextTargetMethod != null && !isNextTargetInvoking)
            {
                isNextTargetInvoking = true;
                Node node =  (Node)appdomain.Invoke(mNextTargetMethod, instance, mParams);
                isNextTargetInvoking = false;
                return node;
            }
            else
            {
                return base.NextTarget();
            }
        }

    }
}
