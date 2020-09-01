using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ScreenAdapterTools 
{
    public void Init()
    {
        if (Platform.mPlatformType == PlatformType.ANDROID)
        {
            int height = QuDaoInterface.Instance.GetScreenDisplayCutout();
            if (height > 0)
            {
                CSConstant.pixelzoom = height;
                ScreenAdapter();
            }
            else
            {            
                CSGame.Sington.StartCoroutine(loadAdapt());
            }
        }
        else
        {
            ScreenAdapter();
        }
    }
    
    
    /// 加载特殊机型进行适配
    /// </summary>
    /// <returns></returns>
    IEnumerator loadAdapt()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(AppUrlMain.getRatioAdapt))
        {
            yield return www.SendWebRequest();

            if (string.IsNullOrEmpty(www.error))
            {
                //"2208_1028_40#2208_1028_40";
                string[] resolutions = www.downloadHandler.text.Trim().Split('#');
                for (int i = 0; i < resolutions.Length; i++)
                {
                    //"2208_1028_40_oppo-x21";
                    string[] info = resolutions[i].Split('_');
                    bool CheckPhoneName = true;
                    if (info.Length > 3 && !string.IsNullOrEmpty(info[3]))
                    {
                        string deviceBrand = QuDaoInterface.Instance.getDeviceBrand();
                        string systemModel = QuDaoInterface.Instance.getSystemModel();
                        string curPhoneName = string.Format("{0}-{1}", deviceBrand, systemModel);
                        if (curPhoneName != info[3]) CheckPhoneName = false;
                    }
                    if (info.Length > 2)
                    {
                        if (Screen.width == int.Parse(info[0]) && Screen.height == int.Parse(info[1]) && CheckPhoneName)
                        {
                            CSConstant.pixelzoom = int.Parse(info[2]);
                        }
                    }
                }
            }else
            {
                FNDebug.LogErrorFormat("ScreenAdapter loadAdapt error  {0}  code: {1}    url:{2}", www.error,
                    www.responseCode, www.uri);
            }
            ScreenAdapter();
        }
        yield return null;
    }

    /// <summary>
    /// 屏幕分辨率适配
    /// </summary>
    private void ScreenAdapter()
    {
        double curRoata = (Screen.width * 1.0f) / (Screen.height * 1.0);
        double defaultRoata = 1.775f;
        float radio = Screen.width * 1.0f / Screen.height;
        int newWidth = 1136;
        int newHeight = 640;
        bool fullScreen = false;

        if (curRoata > defaultRoata)//以宽度高度做适配
        {
            fullScreen = true;
            newWidth = Mathf.CeilToInt(640 * 1.0f * radio);
            CSMesh.PadVisionCountX = (int)(newWidth / CSCell.Size.x * 0.5f);
            CSMesh.PadVisionCountY = (int)(newHeight / CSCell.Size.y * 0.5f) + 1;
            if (CSConstant.pixelzoom != 0)
            {
                CSConstant.SpecialAdaptRadio = (newWidth - CSConstant.pixelzoom) * 1.0f / newWidth;
                NGUIConstant.SpecialAdaptRadio = CSConstant.SpecialAdaptRadio;
                //屏幕初始默认左侧是刘海， 如果是右侧 unity会收到回调
                NGUIConstant.SpecialAdaptRadioLeft = CSConstant.SpecialAdaptRadio;
            }
#if UNITY_IPHONE
                //1200/1280(2160*1080分辨率的情况下,通过NGUI适配,将屏幕分辨率调整为1280*640,0.9375=1200/1280即左右各缩进40)
                if ((Screen.width.Equals(2436) && Screen.height.Equals(1125)) ||
                (Screen.width.Equals(2688) && Screen.height.Equals(1242)) ||
                (Screen.width.Equals(1792) && (Screen.height.Equals(828) || Screen.height.Equals(827)))
                )//当前测试iPhoneX 2436*1125
            {
                CSConstant.SpecialAdaptRadio = (1280 * 1.0f - 48 * 2.0f) / 1280;
            }
#endif
        }
        else if (curRoata < defaultRoata)//以宽度做适配
        {
            newHeight = Mathf.CeilToInt(1136 * 1.0f / radio);
            CSMesh.PadVisionCountX = (int)(newWidth / CSCell.Size.x * 0.5f);
            CSMesh.PadVisionCountY = (int)(newHeight / CSCell.Size.y * 0.5f) + 1;
            if (CSConstant.pixelzoom != 0)
            {
                CSConstant.SpecialAdaptRadio = (newWidth - CSConstant.pixelzoom) * 1.0f / newWidth;
                NGUIConstant.SpecialAdaptRadio = CSConstant.SpecialAdaptRadio;
                NGUIConstant.SpecialAdaptRadioLeft = CSConstant.SpecialAdaptRadio;
            }
        }
        Screen.SetResolution(newWidth, newHeight, fullScreen);
        CSConstant.ContentSize.x = newWidth;
        CSConstant.ContentSize.y = newHeight;
        UIRoot root = GameObject.Find("UI Root").gameObject.GetComponent<UIRoot>();
        if (root == null) return;
        root.manualWidth = newWidth;
        root.manualHeight = newHeight;
        UnityEngine.Debug.Log("2 Screen.width = " + Screen.width + " Screen.height=" + Screen.height + " root w h = " + root.manualWidth + " " + root.manualHeight);
    }
}