/****************************************************************************
 * Author : jiabao
 * Time:    2019/8/19
 * Describe: 用于区分平台
 ****************************************************************************/

public class Platform
{
    /// <summary>
    /// 平台类型~~~(在这里用宏区分)
    /// </summary>
    public static PlatformType mPlatformType
    {
        get
        {
#if UNITY_EDITOR
            return PlatformType.EDITOR;
#elif UNITY_ANDROID
            return PlatformType.ANDROID;
#elif UNITY_IOS
           return PlatformType.IOS;
#endif
        }
    }

    public static bool IsAndroid
    {
        get
        {
            bool mValue = false;
#if UNITY_ANDROID
            mValue = true;
#endif
            return mValue;
        }
    }

    public static bool IsEditor
    {
        get
        {
            bool mValue = false;
#if UNITY_EDITOR
            mValue = true;
#endif
            return mValue;
        }
    }

    public static bool IsIOS
    {
        get
        {
            bool mValue = false;
#if UNITY_IOS
				mValue = true;    
#endif
            return mValue;
        }
    }
}