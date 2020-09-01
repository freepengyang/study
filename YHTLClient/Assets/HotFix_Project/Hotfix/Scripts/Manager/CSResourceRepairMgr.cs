using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Main_Project.Script.Update;
using UnityEngine.Networking;

public class CSResourceRepairMgr : Singleton<CSResourceRepairMgr> {

    public enum RepairType
    {
        All,
        PreDown,
    }

    public float resourceProgress;
    public bool isDown;
    private System.Action errorAct;
    private RepairType repairType;
    private Dictionary<string, ResourceRepairData> resourceDic = new Dictionary<string, ResourceRepairData>();
    private List<string> errorFiles = new List<string>();
    private Thread thread;
    private string resourceURL;
    private string mClientResURL;
    private EventHanlderManager mClientEvent = new EventHanlderManager(EventHanlderManager.DispatchType.Event);

    public void StartRepairResource(RepairType repairType, System.Action errorAct)
    {
        this.errorAct = errorAct;
        mClientResURL = AppUrlMain.mClientResPath;
        Reset();
        if(!GetResourceURL()) return;
        bool isPreDown = repairType == RepairType.PreDown ? true : false;
        CSGame.Sington.StartCoroutine(ImportResourceList(isPreDown));

    }

    public void DownLoadRepairComplete(uint id, object data)
    {
        mClientResURL = AppUrlMain.mClientResPath;
        string file = mClientResURL + AppUrlMain.mRepairListFileName;
        try
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
        catch (IOException e)
        {
            FNDebug.Log(e);
        }
    }

    public void DownLoadPreDownComplete()
    {
        mClientResURL = AppUrlMain.mClientResPath;
        string file = mClientResURL + AppUrlMain.mRepairListFileName;
        try
        {
            if (File.Exists(file))
            {
                string resourceList = File.ReadAllText(file);

                if (!string.IsNullOrEmpty(resourceList))
                {
                    errorFiles.Clear();
                    string[] resList = resourceList.Split('\n');
                    for (int i = 0; i < resList.Length; i++)
                    {
                        string[] res = resList[i].Split('#');
                        if (res.Length < 6) continue;
                        if (res[4] == "1") continue;
                        errorFiles.Add(resList[i]);
                    }

                    if (errorFiles.Count == 0)
                        File.Delete(file);
                    else
                        SaveErrorResource();
                }
            }
        }
        catch (IOException e)
        {
            FNDebug.LogError(e);
        }
    }

    public void CheckRepairResList()
    {
        mClientResURL = AppUrlMain.mClientResPath;
        string file = mClientResURL + AppUrlMain.mRepairListFileName;
        if (File.Exists(file))
        {
            //ClentResourceList resList = new ClentResourceList(ResourceListType.Normal, AppUrl.mRepairListFileName, 0);
            //CSResUpdateManager.Instance.AddToResListDic(resList);
            //mClientEvent.AddEvent(CEvent.DownloadFinish, DownLoadRepairComplete);
        }
    }

    private bool GetResourceURL()
    {
        if (CSVersionManager.Instance.ServerVersion == null)
        {
            Error();
            return false;
        }

        resourceURL = AppUrlMain.mServerResURL + CSCheckResourceManager.RESOURCELISTNAME + AppUrlMain.cdnVersion;
        if (string.IsNullOrEmpty(resourceURL))
        {
            Error();
            return false;
        }
        return true;
    }

  
    private void OnStartThread()
    {
        if (thread != null)
        {
            thread.Abort();
            thread = null;
        }

        thread = new Thread(CaculaterFileMd5);
        thread.IsBackground = true;
        thread.Start();
    }

    private void CaculaterFileMd5()
    {
        try
        {
            float allCount = resourceDic.Count;
            int curCount = 0;
            var EnumeratorDic = resourceDic.GetEnumerator();
            while (EnumeratorDic.MoveNext())
            {
                var dic = EnumeratorDic.Current;
                string file = mClientResURL + dic.Key;
                string md5 = md5file(file);
                if (!string.IsNullOrEmpty(md5) && md5 != dic.Value.md5)
                {
                    errorFiles.Add(dic.Value.resourceData);
                }
                resourceProgress = ++curCount / allCount;
                Thread.Sleep(1);
            }
            SaveErrorResource();
            isDown = true;
        }
        catch(ThreadAbortException e)
        {
            FNDebug.LogError("MD5 校验失败  ==   " + e.Message);
        }
    }

    private void SaveErrorResource()
    {
        if (errorFiles == null || errorFiles.Count == 0) return;
        CSStringBuilder.Clear();
        for (int i = 0; i < errorFiles.Count; i++)
        {
            CSStringBuilder.Append(errorFiles[i]);
            if (i != errorFiles.Count - 1)
                CSStringBuilder.Append("\n");
        }
        byte[] bytes = Encoding.UTF8.GetBytes(CSStringBuilder.ToString());

        string file = mClientResURL + AppUrlMain.mRepairListFileName;
        try
        {
            if (!Directory.Exists(mClientResURL))
            {
                Directory.CreateDirectory(mClientResURL);
            }

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (FileStream stream = new FileStream(file, FileMode.Create))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();
            }
        }
        catch (IOException e)
        {
            FNDebug.LogError("保存资源修复列表错误 ：  " + e.Message);
        }
    }

   
    
    private void Error()
    {
        if (errorAct != null)
            errorAct();
        if (thread != null && thread.IsAlive)
        {
            thread.Abort();
            thread = null;
        }
    }

    public static string md5file(string file)
    {
        try
        {
            if (!File.Exists(file)) return "";
            FileStream fs = new FileStream(file, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            CSStringBuilder.Clear();
            for (int i = 0; i < retVal.Length; i++)
            {
                CSStringBuilder.Append(retVal[i].ToString("x2"));
            }
            return CSStringBuilder.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
  
  
    IEnumerator ImportResourceList(bool isPreDown)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(resourceURL))
        {
            yield return www.SendWebRequest();
            if (string.IsNullOrEmpty(www.error))
            {
                if (string.IsNullOrEmpty(www.downloadHandler.text))
                {
                    Error();
                }else
                {
                    Recursive(www.downloadHandler.text, isPreDown);
                    OnStartThread();
                }
            }
            else
            {
                Error();
            }
        }
    }
    private void Recursive(string resourceList, bool isPreDown)
    {
        string[] resList = resourceList.Split('\n');
        for (int i = 0; i < resList.Length; i++)
        {
            string[] res = resList[i].Split('#');
            if (res.Length < 6) continue;
            if (resourceDic.ContainsKey(res[0])) continue;
            if (isPreDown && res[4] == "0") continue;
            res[5] = res[5].Replace("\r","");
            resourceDic.Add(res[0], new ResourceRepairData(res[5], resList[i]));
        }
    }

    private void Reset()
    {
        resourceDic.Clear();
        errorFiles.Clear();
        isDown = false;
    }

    public void Destroy()
    {
        Reset();
        mClientEvent.UnRegAll();
        mClientEvent = null;
        errorAct = null;
        if (thread != null)
        {
            thread.Abort();
            thread = null;
        }
    }
}


public class ResourceRepairData
{
    public string md5;
    public string resourceData;

    public ResourceRepairData(string md5, string resourceData)
    {
        this.md5 = md5;
        this.resourceData = resourceData;
    }
}