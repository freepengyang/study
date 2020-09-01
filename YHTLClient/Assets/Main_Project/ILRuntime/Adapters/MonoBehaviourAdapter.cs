using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using UnityEngine;

public class MonoBehaviourAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(MonoBehaviour);
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
    //为了完整实现MonoBehaviour的所有特性，这个Adapter还得扩展，这里只抛砖引玉，只实现了最常用的Awake, Start和Update
    public class Adaptor : MonoBehaviour, CrossBindingAdaptorType
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

        public ILTypeInstance ILInstance { get { return instance; } set { instance = value; } }

        public ILRuntime.Runtime.Enviorment.AppDomain AppDomain { get { return appdomain; } set { appdomain = value; } }

        IMethod mAwakeMethod;
        bool mAwakeMethodGot;
        object[] mAwakeParams;
        public void Awake()
        {
            //Unity会在ILRuntime准备好这个实例前调用Awake，所以这里暂时先不掉用
            if (instance != null)
            {
                if (!mAwakeMethodGot)
                {
                    mAwakeMethod = instance.Type.GetMethod("Awake", 0);
                    mAwakeParams = new object[0];
                    mAwakeMethodGot = true;
                }

                if (mAwakeMethod != null)
                {
                    appdomain.Invoke(mAwakeMethod, instance, mAwakeParams);
                }
            }
        }

        IMethod mStartMethod;
        bool mStartMethodGot;
        object[] mStartParams;
        public void Start()
        {
            if (instance == null) return;
            if (!mStartMethodGot)
            {
                mStartMethod = instance.Type.GetMethod("Start", 0);
                mStartParams = new object[0];
                mStartMethodGot = true;
            }

            if (mStartMethod != null)
            {
                appdomain.Invoke(mStartMethod, instance, mStartParams);
            }
        }

        IMethod mUpdateMethod;
        bool mUpdateMethodGot;
        object[] mUpdateParams;
        public void Update()
        {
            if (instance == null) return;
            if (!mUpdateMethodGot)
            {
                mUpdateMethod = instance.Type.GetMethod("Update", 0);
                mUpdateParams = new object[0];
                mUpdateMethodGot = true;
            }

            if (mUpdateMethod != null)
            {
                appdomain.Invoke(mUpdateMethod, instance, mUpdateParams);
            }
        }

        IMethod mFixedUpdateMethod;
        bool mFixedUpdateMethodGot;
        object[] mFixedUpdateParams;
        public void FixedUpdate()
        {
            if (instance == null) return;
            if (!mFixedUpdateMethodGot)
            {
                mFixedUpdateMethod = instance.Type.GetMethod("FixedUpdate", 0);
                mFixedUpdateParams = new object[0];
                mFixedUpdateMethodGot = true;
            }

            if (mFixedUpdateMethod != null)
            {
                appdomain.Invoke(mFixedUpdateMethod, instance, mFixedUpdateParams);
            }
        }

        IMethod mLateUpdateMethod;
        bool mLateUpdateMethodGot;
        object[] mLateUpdateParams;
        public void LateUpdate()
        {
            if (appdomain == null)
            {
                return;
            }
            if (instance == null)
            {
                return;
            }

            if (!mLateUpdateMethodGot)
            {
                mLateUpdateMethod = instance.Type.GetMethod("LateUpdate", 0);
                mLateUpdateParams = new object[0];
                mLateUpdateMethodGot = true;
            }

            if (mLateUpdateMethod != null)
            {
                appdomain.Invoke(mFixedUpdateMethod, instance, mLateUpdateParams);
            }
        }

        IMethod mOnDestroyMethod;
        bool mOnDestroyMethodGot;
        object[] mOnDestroyParams;
        public void OnDestroy()
        {
            if (appdomain == null) return;

            if (!mOnDestroyMethodGot)
            {
                if (instance == null)
                {
                    return;
                }
                mOnDestroyMethod = instance.Type.GetMethod("OnDestroy", 0);
                mOnDestroyParams = new object[0];
                mOnDestroyMethodGot = true;
            }

            if (mOnDestroyMethod != null && instance != null)
            {
                mAwakeParams = null;
                mStartParams = null;
                mUpdateParams = null;
                mFixedUpdateParams = null;
                mLateUpdateParams = null;
                mOnToggleClickParams = null;
                mOnDragStartParams = null;
                mOnDragMethodParams = null;
                mOnDragEndParams = null;
                appdomain.Invoke(mOnDestroyMethod, instance, mOnDestroyParams);
                mOnDestroyParams = null;
            }
        }

        IMethod mOnToggleClickMethod;
        bool mOnToggleClickeMethodGot;
        object[] mOnToggleClickParams;
        public void OnToggleClick(UIToggle gameObject)
        {
            if (!mOnToggleClickeMethodGot && instance != null)
            {
                mOnToggleClickMethod = instance.Type.GetMethod("OnToggleClick", 1);
                mOnToggleClickParams = new object[1];
                mOnToggleClickeMethodGot = true;
            }

            if (mOnToggleClickMethod != null && instance != null)
            {
                mOnToggleClickParams[0] = gameObject;
                appdomain.Invoke(mOnToggleClickMethod, instance, mOnToggleClickParams);
            }
        }

        IMethod mOnDragStartMethod;
        bool mOnDragStartMethodGot;
        object[] mOnDragStartParams;
        public void OnDragStart()
        {
            if (instance == null)
            {
                return;
            }

            if (!mOnDragStartMethodGot && instance != null)
            {
                mOnDragStartMethod = instance.Type.GetMethod("OnDragStart", 0);
                mOnDragStartParams = new object[0];
                mOnDragStartMethodGot = true;
            }

            if (mOnDragStartMethod != null)
            {
                appdomain.Invoke(mOnDragStartMethod, instance, mOnDragStartParams);
            }
        }

        IMethod mOnDragMethod;
        bool mOnDragMethodGot;
        object[] mOnDragMethodParams;
        public void OnDrag(Vector2 delta)
        {
            if (instance == null)
            {
                return;
            }

            if (!mOnDragMethodGot && instance != null)
            {
                mOnDragMethod = instance.Type.GetMethod("OnDrag", 1);
                mOnDragMethodParams = new object[1];
                mOnDragMethodGot = true;
            }

            if (mOnDragMethod != null)
            {
                mOnDragMethodParams[0] = delta;
                appdomain.Invoke(mOnDragMethod, instance, mOnDragMethodParams);
            }
        }

        IMethod mOnDragEndMethod;
        bool mOnDragEndMethodGot;
        object[] mOnDragEndParams;
        public void OnDragEnd()
        {
            if (instance == null)
            {
                return;
            }

            if (!mOnDragEndMethodGot && instance != null)
            {
                mOnDragEndMethod = instance.Type.GetMethod("OnDragEnd", 0);
                mOnDragEndParams = new object[0];
                mOnDragEndMethodGot = true;
            }

            if (mOnDragEndMethod != null && instance != null)
            {
                appdomain.Invoke(mOnDragEndMethod, instance, mOnDragEndParams);
            }
        }
        IMethod mOnPressMethod;
        bool mOnPressMethodGot;
        public void OnPress(bool isPressed)
        {
            if (instance == null)
            {
                return;
            }

            if (!mOnPressMethodGot && instance != null)
            {
                mOnPressMethod = instance.Type.GetMethod("OnPress", 1);
                mOnPressMethodGot = true;
            }

            if (mOnPressMethod != null)
            {
                appdomain.Invoke(mOnPressMethod, instance, new object[] { isPressed });
            }
        }

        IMethod mOnHoverMethod;
        bool mOnHoverMethodGot;
        public void OnHover(bool isOver)
        {
            if (instance == null)
            {
                return;
            }

            if (!mOnHoverMethodGot && instance != null)
            {
                mOnHoverMethod = instance.Type.GetMethod("OnHover", 1);
                mOnHoverMethodGot = true;
            }

            if (mOnHoverMethod != null)
            {
                appdomain.Invoke(mOnHoverMethod, instance, new object[] { isOver });
            }
        }

        IMethod mOnClickMethod;
        bool mOnClickMethodGot;
        public void OnClick()
        {
            if (instance == null)
            {
                return;
            }

            if (!mOnClickMethodGot && instance != null)
            {
                mOnClickMethod = instance.Type.GetMethod("OnClick", 0);
                mOnClickMethodGot = true;
            }

            if (mOnClickMethod != null)
            {
                appdomain.Invoke(mOnClickMethod, instance, null);
            }
        }
        public override string ToString()
        {
            IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
            m = instance.Type.GetVirtualMethod(m);
            if (m == null || m is ILMethod)
            {
                return instance.ToString();
            }
            else
            {
                return instance.Type.FullName;
            }
        }
    }
}