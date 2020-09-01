using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Google.Protobuf.Collections;
using TABLE;

namespace Main_Project.Script.Update
{
    public class CSResUpdateManager : CSGameMgrBase2<CSResUpdateManager>, ISFResUpdateMgr
    {
        public event Action<int, int> onDownloadProgress;

        //完整资源列表
        private Dictionary<string, RESOURCELIST> fullResourceLists;

        private Dictionary<string, RESOURCELIST> preDownloadResDic;
        private Dictionary<string, RESOURCELIST> GameDownloadResDic;
        private List<string> GameDownLoadReslist = new List<string>();
        private Dictionary<string, Resource> failedResDic = new Dictionary<string, Resource>(12);

        private Stack<Resource> ResourceQueue = new Stack<Resource>(12);

        private object objLock = new object();
        private object objLock1 = new object();

        public int curPreDownloadByteNum = 0;
        public int preDownloadByteNum = 0;
        private bool preDownloading = false;
        private bool backDownloading = false;
        private int timeScale = 0;
        public int limit = 10;
        private int downloadLimit = 5;


        private int curBackDownloadByteNum = 0;

        public int CurBackDownloadByteNum
        {
            get { return curBackDownloadByteNum; }
        }

        public bool NeedUpdate
        {
            get { return preDownloadResDic != null && preDownloadResDic.Count > 0; }
        }

        private List<Resource> waitingDownloadQueue = new List<Resource>(12);
        private Dictionary<string, Resource> downloadingDic = new Dictionary<string, Resource>(12);
        private Queue<Resource> downloadedQueue = new Queue<Resource>(12);

        /*后台下载总资源大小*/
        private int backDownloadByteNum = -1;

        public int BackDownloadByteNum
        {
            get
            {
                if (backDownloadByteNum < 0 && GameDownloadResDic != null)
                {
                    int totalByteNum = 0;
                    var dic = GameDownloadResDic.GetEnumerator();
                    while (dic.MoveNext())
                    {
                        totalByteNum += dic.Current.Value.length;
                    }

                    dic.Dispose();
                    backDownloadByteNum = totalByteNum;
                    return totalByteNum;
                }

                return backDownloadByteNum;
            }
        }

        public override void Start()
        {
            base.Start();
            OpenDownloadThread();
        }

        private void Update()
        {
            HandleDownloadedRes();

            PreDownload();

            BackDownload();
        }

    #region DownloadThread

        private Thread downloadThread;

        /// <summary>
        /// 开启下载线程
        /// </summary>
        private void OpenDownloadThread()
        {
            downloadThread = new Thread(DownloadAsync);
            downloadThread.IsBackground = true;
            downloadThread.Start();
        }

        /// <summary>
        /// 关闭下载线程
        /// </summary>
        public void CloseDownloadThread()
        {
            if (downloadThread != null)
            {
                try
                {
                    downloadThread.Abort();
                }
                catch (ThreadAbortException e)
                {
                    UnityEngine.Debug.LogError($"CSResUpdateManager CloseDownloadThread error  {e.StackTrace} ");
                }

                downloadThread = null;
            }

            objLock = null;
            objLock1 = null;
        }

        private void DownloadAsync()
        {
            while (true)
            {
                if (waitingDownloadQueue.Count > 0 && downloadingDic.Count < 10)
                {
#if UNITY_ANDROID
                    if (string.IsNullOrEmpty(SFOut.URL_mServerResURL))
                    {
                        Thread.Sleep(1);
                        continue;
                    }
#endif
                    try
                    {
                        Resource res;
                        lock (objLock)
                        {
                            res = waitingDownloadQueue[0];
                            waitingDownloadQueue.RemoveAt(0);
                        }

                        if (res == null)
                        {
                            ReduceBackDownloadCount();
                        }
                        else
                        {
                            lock (objLock1)
                            {
                                if (!downloadingDic.ContainsKey(res.RelatePath))
                                {
                                    downloadingDic.Add(res.RelatePath, res);
                                    res.Download();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }

                Thread.Sleep(1);
            }
        }

        private int backDownloadCount = 0;

        private void ReduceBackDownloadCount()
        {
            if (backDownloadCount > 0)
            {
                backDownloadCount--;
            }
            else
            {
                backDownloadCount = 0;
            }
        }

    #endregion

    #region Update

        private void HandleDownloadedRes()
        {
            if (downloadedQueue == null || downloadedQueue.Count == 0) return;
            Resource res = downloadedQueue.Dequeue();
            if (res == null)
            {
                ReduceBackDownloadCount();
                return;
            }

            if (res.type == DownloadType.BackDownload) ReduceBackDownloadCount();

            if (GameDownloadResDic != null)
            {
                if (GameDownloadResDic.ContainsKey(res.RelatePath))
                {
                    GameDownloadResDic.Remove(res.RelatePath);
                    CSCheckResourceManager.Instance.UpdateClientDownRes(res);
                }

                if (GameDownloadResDic.Count == 0)
                    CSVersionManager.Instance.ClientVersion.SaveGameClientVersion(CSCheckResourceManager.Instance
                        .ServerResourceVersion);
            }

            if (!res.isSucceed)
            {
                if (!failedResDic.ContainsKey(res.RelatePath))
                    failedResDic.Add(res.RelatePath, res);
            }
            else
                HandleDownloadedRes(res);
        }

        private void PreDownload()
        {
            if (!preDownloading || preDownloadResDic == null) return;
            preDownloading = false;

            CSPreDownLoadManger.Instance.StartLoad(preDownloadResDic);
            preDownloadResDic.Clear();
        }

        private void BackDownload()
        {
            if (!backDownloading) return;
            if(GameDownLoadReslist == null) return;
            if (limit != 0)
            {
                timeScale++;
                if (timeScale < limit) return;
                timeScale = 0;
            }

            if (backDownloadCount >= downloadLimit) return;
            if (GameDownLoadReslist.Count > 0)
            {
                if (GameDownloadResDic != null && GameDownloadResDic.TryGetValue(GameDownLoadReslist[0], out RESOURCELIST res))
                {
                    curBackDownloadByteNum += res.length;
                    AddToDownloadQueue(res, DownloadType.BackDownload);
                    backDownloadCount++;
                }

                GameDownLoadReslist.RemoveAt(0);
            }
        }

    #endregion


        private void HandleDownloadedRes(Resource res)
        {
            if (res == null) return;
            switch (res.type)
            {
                case DownloadType.GamingDownload:
                    CSResourceManager.Instance.ResHotUpdateCallBack_HttpLoad(
                        AppUrlMain.mClientResURL + res.LocalRelatePath,
                        res.isSucceed);
                    PushResourceQueue(res);
                    break;
                case DownloadType.BackDownload:
                    curBackDownloadByteNum += res.ByteNum;
                    if (onDownloadProgress != null)
                    {
                        onDownloadProgress(curBackDownloadByteNum, BackDownloadByteNum);
                    }

                    PushResourceQueue(res);
                    break;
                case DownloadType.FailedDownload:
                    if (failedResDic.ContainsKey(res.RelatePath))
                    {
                        failedResDic.Remove(res.RelatePath);
                        if (!res.NeedPreDownload)
                            CSCheckResourceManager.Instance.UpdateClientDownRes(res);
                        else
                            PushResourceQueue(res);
                    }

                    break;
            }
        }


    #region Base

        public void AddFullResourceList(RepeatedField<RESOURCELIST> rows)
        {
            if(fullResourceLists == null)
                fullResourceLists = new Dictionary<string, RESOURCELIST>(25000);
            if (rows != null)
            {
                RESOURCELIST resourcelist;
                for (int i = 0; i < rows.Count; i++)
                {
                    resourcelist = rows[i];
                    if (!fullResourceLists.ContainsKey(resourcelist.name))
                        fullResourceLists.Add(resourcelist.name, resourcelist);
                }
            }
        }

        public void CompareResUpdate(Dictionary<string, RESLISTANDROID> clientDown)
        {
            if (clientDown.Count <= 0 || fullResourceLists == null) return;
            var resDic = fullResourceLists.GetEnumerator();
            RESLISTANDROID res;
            RESOURCELIST resourcelist;
            while (resDic.MoveNext())
            {
                resourcelist = resDic.Current.Value;
                if (resourcelist == null) continue;
                if (!clientDown.TryGetValue(resDic.Current.Key, out res) || !resourcelist.md5.Equals(res.md5))
                {
                    if (resourcelist.resType.Equals("1"))
                    {
                        AddToPreDownload(resourcelist);
                    }
                    else
                    {
                        AddToGameDownload(resourcelist);
                    }
                }
            }

            //CSCheckResourceManager.Instance.SaveResListNewTextTest(GameDownloadResDic, AppUrlMain.mClientGameDownResourceFile);
            res = null;
            resourcelist = null;
            resDic.Dispose();
        }

        public bool CheckIsNeedDownload(string relatePath)
        {
            if (string.IsNullOrEmpty(relatePath) || GameDownloadResDic == null)
            {
                return false;
            }
            else
            {
                return GameDownloadResDic.ContainsKey(relatePath) || failedResDic.ContainsKey(relatePath);
            }
        }

        public RESOURCELIST GetResource(string relatePath)
        {
            if (string.IsNullOrEmpty(relatePath) ||  fullResourceLists == null || !fullResourceLists.ContainsKey(relatePath))
            {
                return null;
            }

            return fullResourceLists[relatePath];
        }

        public void AddToDownloadQueue(RESOURCELIST res, DownloadType type)
        {
            if (res == null) return;
            switch (type)
            {
                case DownloadType.GamingDownload:
                    lock (objLock)
                    {
                        if (!IsExistSameResInWaitingQueue(res))
                        {
                            Resource resource = PopResourceQueue(res);
                            resource.type = type;
                            waitingDownloadQueue.Insert(0, resource);
                        }
                    }

                    break;
                default:
                    lock (objLock)
                    {
                        Resource resource = PopResourceQueue(res);
                        resource.type = type;
                        waitingDownloadQueue.Add(resource);
                    }

                    break;
            }
        }


        public void AddToPreDownload(RESOURCELIST res)
        {
            if (preDownloadResDic == null)
            {
                preDownloadResDic = new Dictionary<string, RESOURCELIST>(1024);
            }
            if (!preDownloadResDic.ContainsKey(res.name))
            {
                preDownloadResDic.Add(res.name, res);
                preDownloadByteNum += res.length;
            }
        }

        public void AddToGameDownload(RESOURCELIST res)
        {
            if (GameDownloadResDic == null)
            {
                GameDownloadResDic = new Dictionary<string, RESOURCELIST>(25000);
            }
            if (!GameDownloadResDic.ContainsKey(res.name))
            {
                GameDownloadResDic.Add(res.name, res);
                GameDownLoadReslist.Add(res.name);
            }
        }

        public void AddToCompleteQueue(Resource res)
        {
            if (res != null)
            {
                lock (objLock1)
                {
                    if (downloadingDic.ContainsKey(res.RelatePath))
                    {
                        downloadingDic.Remove(res.RelatePath);
                    }
                }

                downloadedQueue.Enqueue(res);
            }
            else
            {
                ReduceBackDownloadCount();
            }
        }

        public void StartPreDownload(Action<bool> onComplete)
        {
            CSPreDownLoadManger.Instance.onDownloadFinish = onComplete;
            preDownloading = true;
        }

        public void StartBackDownload()
        {
            backDownloading = true;
        }

        public bool IsDownloading(string relatePath)
        {
            if (string.IsNullOrEmpty(relatePath)) return false;

            for (int i = 0; i < waitingDownloadQueue.Count; i++)
            {
                if (waitingDownloadQueue[i].RelatePath == relatePath)
                    return true;
            }

            return downloadingDic.ContainsKey(relatePath);
        }

        public void SaveFailedResList()
        {
            var dic = failedResDic.GetEnumerator();

            StringBuilder str = new StringBuilder();

            while (dic.MoveNext())
            {
                string relatePath = dic.Current.Key;
                if (!string.IsNullOrEmpty(relatePath))
                {
                    str.Append(relatePath).Append("\r\n");
                }
            }

            byte[] bytes = Encoding.UTF8.GetBytes(str.ToString());

            SaveData(bytes, AppUrlMain.mClientResPath + AppUrlMain.mFailedListFileName);
        }

    #endregion

        private bool IsExistSameResInWaitingQueue(RESOURCELIST res)
        {
            for (int i = 0; i < waitingDownloadQueue.Count; i++)
            {
                if (i < waitingDownloadQueue.Count)
                {
                    if (waitingDownloadQueue[i].RelatePath == res.name)
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        private void SaveData(byte[] bytes, string path)
        {
            if (string.IsNullOrEmpty(path) || bytes == null) return;

            string dirPath = path.Remove(path.LastIndexOf('/'));

            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        public Resource PopResourceQueue(RESOURCELIST res)
        {
            Resource resource = null;
            if (ResourceQueue.Count > 0)
                resource = ResourceQueue.Pop();
            if(resource == null)
                resource = new Resource();
            resource.Init(res);
            return resource;
        }

        public void PushResourceQueue(Resource resource)
        {
            resource.Dispose();
            ResourceQueue.Push(resource);
        }


        public void RegUpdateAction(Action<int, int> callBack)
        {
            onDownloadProgress -= callBack;
            onDownloadProgress += callBack;
        }

        public void UnRegUpdateAction(Action<int, int> callBack)
        {
            onDownloadProgress -= callBack;
        }

        public override void OnDestroy()
        {
            OnDispose();
            base.OnDestroy();
        }
        
        public void OnDispose()
        {
            CloseDownloadThread();
            fullResourceLists = null;
            preDownloadResDic = null;
            GameDownloadResDic = null;
            GameDownLoadReslist = null;
            failedResDic = null;
            ResourceQueue = null;
            objLock = null;
            objLock1 = null;
            waitingDownloadQueue = null;
            downloadingDic = null;
            downloadedQueue = null;
        }
    }
}