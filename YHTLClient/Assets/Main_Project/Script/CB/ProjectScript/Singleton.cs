using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Singleton2<T>
{
    private static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = (T)Activator.CreateInstance(typeof(T));

            return m_instance;
        }
        set
        {
            if (m_instance.Equals(value))
                return;

            m_instance = value;
        }
    }
}
