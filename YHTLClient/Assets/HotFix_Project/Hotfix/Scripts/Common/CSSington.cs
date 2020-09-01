using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 单利类，，，，如果是UI数据类，请勿继承此类，，继承UIInfo
/// </summary>
/// <typeparam name="T"></typeparam>
public class CSSingOn<T>
{
    protected static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = Activator.CreateInstance<T>();

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
