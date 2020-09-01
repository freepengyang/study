using System.IO;
using System.Net;
using System.Threading;

public enum DownloadType
{
    PreDownload = 0,
    BackDownload = 1,
    GamingDownload = 2,
    FailedDownload = 3,
}


public class HttpDownload
{
    public DownloadType type;
    public HttpWebRequest req;
    public HttpWebResponse rsp;
    protected Stream stream;
    protected FileStream fileStream;
    protected byte[] buffer;
    public bool isSucceed = false;
    public int requestTimes = 0;

    private long totalSize = 0;
    private long downloadSize = 0;

    public virtual string url
    {
        get { return ""; }
    }

    public virtual string localPath
    {
        get { return ""; }
    }

    public void Download()
    {
        try
        {
            req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "GET";
            req.Timeout = 120000;
            req.ReadWriteTimeout = 120000;
            req.Proxy = null;
        }
        catch (System.Exception ex)
        {
        }

        GetResponse();
    }

    protected void GetResponse()
    {
        try
        {
			string tempLocalPath = $"{localPath.Remove(localPath.LastIndexOf('.'))}_temp";
			
            if (File.Exists(tempLocalPath))
            {
                File.Delete(tempLocalPath);
            }

            rsp = req.GetResponse() as HttpWebResponse;
            totalSize = rsp.ContentLength;
            downloadSize = 0;

            CheckDirExist();

            if (File.Exists(localPath))
            {
                File.Delete(localPath);
            }

            if (buffer == null)
            {
                if (type == DownloadType.BackDownload ||
                    type == DownloadType.FailedDownload)
                {
                    buffer = new byte[1024];
                }
                else
                {
                    buffer = new byte[2048];
                }
            }

            using (fileStream = new FileStream(tempLocalPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                using (stream = rsp.GetResponseStream())
                {
                    int len = stream.Read(buffer, 0, buffer.Length);
                    downloadSize += len;
                    while (len > 0)
                    {
                        fileStream.Write(buffer, 0, len);
                        fileStream.Flush();
                        len = stream.Read(buffer, 0, buffer.Length);
                        downloadSize += len;
                    }
                }

                if (totalSize == downloadSize)
                {
                    isSucceed = true;
                    FinishDownload();
                    File.Move(tempLocalPath, localPath);
                }
                else
                {
                    CloseHttp();
                    NeedDownloadAgain();
                    UnityEngine.Debug.LogError($"HttpDownload  down resource error : totalSize:{totalSize}   downloadSize:{downloadSize} ");
                }
            }
        }
        catch (WebException webEx)
        {
            CloseHttp();
            NeedDownloadAgain();
        }
        catch (IOException ioEx)
        {
            CloseHttp();
            NeedDownloadAgain();
            UnityEngine.Debug.LogError($"HttpDownload  down resource error : totalSize:{totalSize}   downloadSize:{downloadSize}   exception: {ioEx.StackTrace}");
        }
    }

    protected virtual void NeedDownloadAgain()
    {
        if (requestTimes < 3 || type == DownloadType.PreDownload)
        {
            requestTimes++;
            Download();
        }
        else
        {
            FinishDownload();
        }
    }

    protected virtual void CheckDirExist()
    {
        if (!string.IsNullOrEmpty(localPath))
        {
            string dirPath = localPath.Remove(localPath.LastIndexOf('/'));

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
    }

    protected virtual void FinishDownload()
    {
        CloseHttp();
        Destroy();
    }

    protected virtual void CloseHttp()
    {
        downloadSize = 0;
        if (fileStream != null)
        {
            fileStream.Close();
            fileStream = null;
        }

        if (stream != null)
        {
            stream.Close();
            stream = null;
        }

        if (req != null)
        {
            try
            {
                req.Abort();
            }
            catch (ThreadAbortException e)
            {
                UnityEngine.Debug.LogError($"HttpDownload CloseDownloadThread error  {e.StackTrace} ");
            }
            req.KeepAlive = false;
            req = null;
        }

        if (rsp != null)
        {
            rsp.Close();
            rsp = null;
        }

        if (buffer != null)
        {
            buffer = null;
        }
    }

    protected virtual void Destroy()
    {
    }
}


