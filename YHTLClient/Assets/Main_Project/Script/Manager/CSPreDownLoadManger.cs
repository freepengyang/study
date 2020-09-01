using System;
using System.IO;
using System.Text;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Security.Cryptography;
using Main_Project.Script.Update;
using TABLE;
using UnityEngine;

public class CSPreDownLoadManger : Singleton2<CSPreDownLoadManger>
{
    /// <summary>
    /// 重复更细次数
    /// </summary>
    private int repetitionUpdateTime = 0;

    /// <summary>
    /// 下载完成
    /// </summary>
    public Action<bool> onDownloadFinish= null;
    /// <summary>
    /// 下载失败
    /// </summary>
    public Action<bool> onDownloadError = null;

    /// <summary>
    /// 下载进度
    /// </summary>
    public Action<int, int> onDownloadProgress = null;

    private List<RESOURCELIST> mList = new List<RESOURCELIST>();

    private Dictionary<string, RESOURCELIST> mErrDic = new Dictionary<string, RESOURCELIST>();

    private CSGame mGame;

    public void StartLoad(Dictionary<string, RESOURCELIST> mDic)
    {
        if (mDic == null || mDic.Count == 0)
        {
            onDownloadFinish(true);
            return;
        }

        repetitionUpdateTime++;

        mList.Clear();

        var dic = mDic.GetEnumerator();

        while (dic.MoveNext())
        {
            mList.Add(dic.Current.Value);
        }
        dic.Dispose();

        mGame = CSGame.Sington;
        mGame.StopCoroutine(LoadAll());
        mGame.StartCoroutine(LoadAll());
    }

    private IEnumerator LoadAll()
    {
        for (int i = 0; i < mList.Count; i++)
        {
            if(mList[i] == null)
            {
                Debug.LogError("CSPreDownloadManager   LoadAll   mList  is  Null  :  index is : " + i);
                continue;
            }
            Resource resource = CSResUpdateManager.Instance.PopResourceQueue(mList[i]);
            yield return mGame.StartCoroutine(Load(resource, mList[i]));
            CSResUpdateManager.Instance.PushResourceQueue(resource);
        }

        if (mErrDic.Count <= 0)
        {
            CSVersionManager.Instance.ClientVersion.SavePreClientVersion(CSCheckResourceManager.Instance.ServerResourceVersion);
            CSCheckResourceManager.Instance.SaveDownResListText();
            repetitionUpdateTime = 0;
            yield return null;
            if (onDownloadFinish != null)
            {
                onDownloadFinish(true);
            }
        }
        else
        {
            if (repetitionUpdateTime >= 3)
            {
                if (onDownloadError != null)
                {
                    onDownloadError(true);
                }
                mGame.StartCoroutine(StartPostLog());
            }
            else
            {
                StartLoad(mErrDic);
            }
        }
    }

    /// <summary>
    /// 单个下载资源
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    private IEnumerator Load(Resource res, RESOURCELIST resourcelist)
    {
        using (UnityWebRequest web = UnityWebRequest.Get(res.url))
        {
            yield return web.SendWebRequest();

            if (string.IsNullOrEmpty(web.error) && web.responseCode == 200)
            {
                try
                {
                    if (mErrDic.ContainsKey(res.RelatePath))
                    {
                        mErrDic.Remove(res.RelatePath);
                    }

                    SaveBytes(res.localPath, web.downloadHandler.data);

                    if (!string.IsNullOrEmpty(res.mMd5))
                    {
                        string str = GetMD5HashFromFile(res.localPath);

                        if (str.Equals(res.mMd5))
                        {
                            CSResUpdateManager.Instance.curPreDownloadByteNum += res.ByteNum;
                            int curByteNum = CSResUpdateManager.Instance.curPreDownloadByteNum;
                            int totalByteNum = CSResUpdateManager.Instance.preDownloadByteNum;
                            CSCheckResourceManager.Instance.UpdateClientDownRes(res);

                            if (onDownloadProgress != null)
                            {
                                onDownloadProgress(curByteNum, totalByteNum);
                            }
                        }
                        else
                        {
                            FNDebug.LogError($"资源下载错误:1： {res.RelatePath}");
                            if (!mErrDic.ContainsKey(res.RelatePath))
                            {
                                mErrDic.Add(res.RelatePath, resourcelist);
                            }
                        }
                    }
                }
                catch
                {
                    FNDebug.LogError($"资源下载错误:2： {res.RelatePath}");
                    if (!mErrDic.ContainsKey(res.RelatePath))
                    {
                        mErrDic.Add(res.RelatePath, resourcelist);
                    }
                }
            }
            else
            {
                FNDebug.LogError($"资源下载错误:3： {res.RelatePath}");
                if (!mErrDic.ContainsKey(res.RelatePath))
                {
                    mErrDic.Add(res.RelatePath, resourcelist);
                }
            }
        }
    }

    /// <summary>
    /// 生成MD5值
    /// </summary>
    /// <param name="filePath">资源文本路径</param>
    /// <returns></returns>
    private string GetMD5HashFromFile(string filePath)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();

                byte[] retVal = md5.ComputeHash(file);

                file.Close();

                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
            }
           
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("GetMD5HashFromFile() fail,error:" + filePath + " " + ex.Message);
        }
        return sb.ToString();
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="fileFullName"></param>
    /// <param name="data"></param>
    public void SaveBytes(string fileFullName, byte[] data)
    {
        if (File.Exists(fileFullName))
        {
            File.Delete(fileFullName);
        }
        string dName = Path.GetDirectoryName(fileFullName);

        if (!Directory.Exists(dName))  //判断目录是否存在
        {
            Directory.CreateDirectory(dName);
        }
        using (FileStream stream = new FileStream(fileFullName, FileMode.Create))
        {
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
        }
    }


    IEnumerator StartPostLog()
    {
        RESOURCELIST res = null;
        foreach(var v in mErrDic)
        {
            res = v.Value;
            break;
        }
        WWWForm sum = new WWWForm();
        if (res != null)
        {
            sum.AddField("name == ", res.name);
        }
        WWW ww2 = new WWW(AppUrlMain.errorUrl3, sum);
        yield return ww2;
    }

    public IEnumerator LoadSingeRes(string assetBundleName, string url,Action<string,bool> action = null)
    {
        using (UnityWebRequest web = UnityWebRequest.Get(url))
        {
            yield return web.SendWebRequest();

            if (string.IsNullOrEmpty(web.error) && web.responseCode == 200)
            {
                try
                {
                    CSStringBuilder.Clear();
                    CSStringBuilder.Append(AppUrlMain.mClientResPath,"Android/",assetBundleName);
                    SaveBytes(CSStringBuilder.ToString(), web.downloadHandler.data);

                    if(action != null) action(assetBundleName,true);
                }
                catch
                {
                    if(action != null) action(assetBundleName,false);
                }
            }
            else
            {
                FNDebug.LogError($"{url} is http 404");
                if(action != null) action(assetBundleName,false);
            }
        }
    }
}
