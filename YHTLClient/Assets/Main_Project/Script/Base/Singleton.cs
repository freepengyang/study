using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlyBirds.Model
{
    public abstract class Singleton<T> where T : Singleton<T>,new()
    {
        static T ms_instance;
        public static T Instance
        {
            get
            {
                if (null == ms_instance)
                    ms_instance = new T();

                return ms_instance;
            }
        }

        public void Dispose()
        {
            if(ms_instance == this)
            {
                ms_instance = null;
            }
        }

        public abstract void OnDispose();
    }
}