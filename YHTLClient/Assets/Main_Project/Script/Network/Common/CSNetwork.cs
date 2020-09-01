
// Network连接器
// author : jiabao
// 2016.2.17
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Threading;
using Google.Protobuf;
using heart;

public struct NetworkMsg
{
    public int length; //长度
    public int msgId;//id
    public byte[] data;//正文

    public NetworkMsg(int _length, int _msgId, byte[] _data)
    {
        this.length = _length;
        this.msgId = _msgId;
        this.data = _data;
    }
}

public struct NetInfo
{
    public int msgId;//id
    public object obj;
    public NetInfo(int _msgid, object _obj)
    {
        this.msgId = _msgid;
        this.obj = _obj;
    }

}

public class CSNetwork : Singleton2<CSNetwork>
{
    /********************** Members *************************/
    private const int READ_BUFFER_SIZE = 8192 * 2;
    private byte[] readBuf = new byte[READ_BUFFER_SIZE];
    private CircularBuffer<byte> ringBuf = new CircularBuffer<byte>(READ_BUFFER_SIZE, true);
    private static Queue<NetworkMsg> mMsgEvents = new Queue<NetworkMsg>();
    private static Queue<NetInfo> mMsgCollectionEvents = new Queue<NetInfo>();
    private static readonly object objThread = new object();

    private byte[] packageBytes = null;
    private MemoryStream mStream = null;
    private NetworkStream _networkStream;
    private long time = 0;
    private heart.Heartbeat data;
    private float heartBeatInterval = 15.0f;
    private float lastBeatTime = 0.0f;
    private int checkTheTime = 0;
    private Thread thread;
    private byte[] lengthByte = new byte[sizeof(uint)];
    public CSNetwork()
    {
        mStream = new MemoryStream();
    }


    /// <summary>
    /// 主机端口
    /// </summary>
    private int mport = 0;
    public int port
    {
        get { return mport; }
        set
        {
            mport = value;
        }
    }
    /// <summary>
    /// 主机ID地址
    /// </summary>
    private string mhost = "";
    public string host
    {
        get
        {
            return mhost;
        }
        set
        {
            mhost = value;
        }
    }
    public string Host { get { return host; } }

    /// <summary>
    /// Tcp对象
    /// </summary>
    private TcpClient client = null;
    public System.Net.Sockets.TcpClient Client
    {
        get { return client; }
        set { client = value; }
    }


    /// <summary>
    /// 每帧处理最大消息包个数
    /// </summary>
    public const int MaxDealPackPerFrame = 100;

    /// <summary>
    /// 空数组
    /// </summary>
    private static readonly byte[] emptyArray = new byte[0];


    /*************************** Function **************************/

    /// <summary>
    /// 初始解包线程
    /// </summary>
    public void InIt()
    {
        CloseThread();
        thread = new Thread(AnalyticalData);
        thread.IsBackground = true;
        thread.Start();
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="_ip">地址</param>
    /// <param name="_port">端口</param>
    public void Connect(string _host, int _port, bool isEnterShare = false)
    {

#if UNITY_EDITOR
        UnityEngine.Debug.LogError("host == " + _host + " port== " + _port);
#endif
        host = _host;
        port = _port;

        if (!IsAlive) InIt();
        Close();
#if UNITY_EDITOR || UNITY_ANDROID
        client = new TcpClient();
#else
        String newServerIp = "";
		AddressFamily newAddressFamily = AddressFamily.InterNetwork;
		NetworkeUtil.GetIPType(_host, port.ToString(), out newServerIp, out newAddressFamily);
		if (string.IsNullOrEmpty(newServerIp)) return;
		host = newServerIp;
		client = new TcpClient(newAddressFamily);
#endif
        heartBeatInterval = 15.0f;
        checkTheTime = 0;
        CSConstant.mHeartbeatNum = 0;
        client.SendTimeout = 1000;
        client.ReceiveTimeout = 1000;
        client.NoDelay = true;
        try
        {
            client.BeginConnect(host, port, OnConnect, client);
        }
        catch (System.Exception ex)
        {
            OnDisconnected(ECM2.ConnectFail, ex.Message);
            if (isEnterShare)
            {
                UnityEngine.Debug.LogError("进入共享服务器 = " + " host = " + host + " _port=" + _port + " " + ex);
            }
            else
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError(ex);
#endif
            }
        }
    }

    public void ReqConnect(string _host, int _port)
    {
        Close();

        try
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError("host == " + _host + " port== " + _port);
#endif
            host = _host;
            port = _port;

            client = new TcpClient();
            heartBeatInterval = 15.0f;
            checkTheTime = 0;
            CSConstant.mHeartbeatNum = 0;
            client.SendTimeout = 1000;
            client.ReceiveTimeout = 1000;
            client.NoDelay = true;
            client.BeginConnect(host, port, OnConnect, client);
        }
        catch (System.Exception ex)
        {
            OnDisconnected(ECM2.ConnectFail, ex.Message);
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(ex);
#endif
        }
    }

    /// <summary>
    /// 重新连接服务器
    /// </summary>
    public void Reconection()
    {
        if (client != null && client.Connected) return;

        Close();
        Connect(host, port);
    }

    /// <summary>
    /// 切换线路
    /// </summary>
    public void SwitchLine()
    {
        Connect(host, port);
    }

    /// <summary>
    /// 连接上服务器
    /// </summary>
    /// <param name="ar"></param>
    private void OnConnect(IAsyncResult ar)
    {
        try
        {
            var c = (TcpClient)ar.AsyncState;
            c.EndConnect(ar);

            if (client != null && client.Connected)
            {
                _networkStream = client.GetStream();
                
                lock (objThread)
                {
                    mMsgCollectionEvents.Enqueue(new NetInfo((int)ECM2.Connect, null));
                }
                _networkStream.BeginRead(readBuf, 0, READ_BUFFER_SIZE, DoRead, null);
            }
        }
        catch (Exception e)
        {
            OnDisconnected(ECM2.ConnectFail, e.Message);
        }
    }

    public void GetCurClientIpAndPort(ref string ip, ref int port)
    {
        if (client == null || client.Client == null || client.Client.RemoteEndPoint == null) return;
        string p = client.Client.RemoteEndPoint.ToString();
        string[] strs = p.Split(':');
        if (strs.Length == 2)
        {
            ip = strs[0];
            int.TryParse(strs[1], out port);
        }
    }

    /// <summary>
    /// 接受主机数据
    /// </summary>
    /// <param name="ar"></param>
    private void DoRead(IAsyncResult ar)
    {
        try
        {
            int sizeRead = 0;

            lock (_networkStream)
            {
                //读取字节流到缓冲区
                sizeRead = _networkStream.EndRead(ar);
            }
            if (sizeRead < 1)
            {
                //包尺寸有问题，断线处理
                OnDisconnected(ECM2.Disconnect, "sizeRead < 1");
                return;
            }
            ringBuf.Put(readBuf, 0, sizeRead);
            Receive();
            lock (_networkStream)
            {
                Array.Clear(readBuf, 0, readBuf.Length);   //清空数组
                _networkStream.BeginRead(readBuf, 0, READ_BUFFER_SIZE, DoRead, null);
            }
        }
        catch (Exception ex)
        {
            OnDisconnected(ECM2.Disconnect, ex.Message);
        }
    }

    private void OnDisconnected(ECM2 state, string msg)
    {
        lock (objThread)
        {
            if (mMsgCollectionEvents != null) mMsgCollectionEvents.Enqueue(new NetInfo((int)state, null));
        }
#if UNITY_EDITOR
        UnityEngine.Debug.LogError("Connection was closed by the server:>" + msg + " NetworkState:>" + state);
#endif

    }

    public void Close()
    {
        if (client != null)
        {
            try
            {
                if (_networkStream != null)
                    _networkStream.Close();
                client.Close();
                
            }
            catch (IOException e)
            {
                UnityEngine.Debug.LogError(e);
            }
            catch (ArgumentNullException e)
            {
                UnityEngine.Debug.LogError(e);
            }
            catch (SocketException e)
            {
                UnityEngine.Debug.LogError(e);
            }
        }
        client = null;
        _networkStream = null;
    }

    public void CloseThread()
    {
        if (thread != null)
        {
            thread.Abort();
            thread = null;
        }
    }

    public void Update()
    {
        //心跳包
        HeartBeat();

        if (mMsgCollectionEvents.Count > 0)
        {
            int maxDeal = MaxDealPackPerFrame;
            while (mMsgCollectionEvents.Count > 0 && maxDeal > 0)
            {
                NetInfo info;
                lock (objThread)
                {
                    info = mMsgCollectionEvents.Dequeue();

                    maxDeal--;
                    if (info.msgId != 0)
                    {
                        if (NetMsgMain.Instance.IsHaveMsgID(info.msgId))
                        {
                            MonitorEvent((ECM2)info.msgId, info);
                        }
                        else
                        {
                            CSGame.Sington.ProcessNetwork?.Run(info);
                        }
                    }
                }
            }
        }
    }

    public void SendMsg(ECM2 id, IMessage msg)
    {
        SendMsg(id, msg);
    }
    
    public void SendMsg(int id, Google.Protobuf.IMessage msg)
    {
        if (client == null || !client.Connected) return;

        byte[] msgBytes;
        uint orderLength = 0;
        uint orderId = 0;

        msgBytes = msg == null ? emptyArray : msg.ToByteArray();

        orderLength = (uint)IPAddress.HostToNetworkOrder(sizeof(uint) * 2 + msgBytes.Length);
        orderId = (uint)IPAddress.HostToNetworkOrder(id);

        if (msgBytes == null) return;

        try
        { 
            BinaryWriter writer = new BinaryWriter(_networkStream);
            writer.Write(orderLength);
            writer.Write(orderId);
            writer.Write(msgBytes);
            writer.Flush();
        }
        catch (Exception e) { }
    }

    private void Receive()
    {
        while (ringBuf.Size >= sizeof(uint))
        {
            ringBuf.CopyTo(0, lengthByte, 0, lengthByte.Length);

            uint msgLength = BitConverter.ToUInt32(lengthByte, 0);

            msgLength = NetworkToHostOrder(msgLength);

            if (ringBuf.Size >= msgLength)
            {
                byte[] msgdata = new byte[msgLength];

                ringBuf.Get(msgdata);

                if (msgLength == 0)
                {
                    ringBuf.Clear();
                    return;
                }

                using (MemoryStream stream = new MemoryStream(msgdata))
                {
                    BinaryReader br = new BinaryReader(stream);

                    NetworkMsg Nmsg = new NetworkMsg();

                    try
                    {
                        Nmsg.length = NetworkToHostOrder(br.ReadInt32());
                        Nmsg.msgId = NetworkToHostOrder(br.ReadInt32());
                    }
                    catch (Exception)
                    {
                        if (FNDebug.developerConsoleVisible) FNDebug.Log(" Nmsg.length ==null|| Nmsg.msgId == null");
                        throw;
                    }
                    
                    // 8 is header length
                    byte[] data = br.ReadBytes((int)msgLength - 8);

                    Nmsg.data = data;

                    lock (objThread)
                    {
                        mMsgEvents.Enqueue(Nmsg);
                    }
                }
            }
            else
            {
                break;
            }
        }
    }

    private uint NetworkToHostOrder(uint val)
    {
        byte[] array = BitConverter.GetBytes(val);
        Array.Reverse(array);
        return BitConverter.ToUInt32(array, 0);
    }

    private int NetworkToHostOrder(int val)
    {
        byte[] array = BitConverter.GetBytes(val);
        Array.Reverse(array);
        return BitConverter.ToInt32(array, 0);
    }

    #region Connect,IsAlive,analyticPackageCount，distributionCount
    public bool IsConnect
    {
        get
        {
            if (client != null) return client.Connected; else return false;

        }
    }

    public bool IsAlive
    {
        get { return thread != null && thread.IsAlive; }
    }

    public ThreadState threadState
    {
        get { return thread != null ? thread.ThreadState : ThreadState.Aborted; }
    }

    public int analyticPackageCount
    {
        get { return mMsgCollectionEvents.Count; }
    }

    public int distributionCount
    {
        get { return mMsgEvents.Count; }
    }
    #endregion
    public void HeartBeat(bool isForce = false)
    {
        if (client == null || (CSGame.Sington.mCurState != GameState.MainScene)) return;

        float t = Time.realtimeSinceStartup - lastBeatTime;

        if (lastBeatTime == 0 || t >= heartBeatInterval)
        {
            if (checkTheTime < 3)
            {
                lastBeatTime = Time.realtimeSinceStartup;
                time = (long)Time.realtimeSinceStartup;
                ReqHeartbeatMessage(time);
                checkTheTime++;
            }
            else
            {
#if !UNITY_EDITOR
                Close();
#endif
                checkTheTime = 0;
            }
        }
    }


    public void ReqHeartbeatMessage(long time = 0)
    {
        if(data == null) data= new heart.Heartbeat();
        data.clientTime = time;
        CSNetwork.Instance.SendMsg((int)ECM2.ReqHeartbeatMessage, data);
    }

    public void AnalyticalData()
    {
        while (true)
        {
            if (mMsgEvents.Count > 0)
            {
                NetworkMsg msg;
                lock (objThread)
                {
                    msg = mMsgEvents.Dequeue();

                    if (msg.msgId != 0)
                    {
                        MessageParser parser = null;
                        object obj = null;
                        try
                        {
                            if (NetMsgMain.Instance.IsHaveMsgID(msg.msgId))
                            {
                                 parser = NetMsgMain.Instance.GetMsgType(msg.msgId);
                            }
                            else
                            {
                                parser = NetMsgMain.Instance.GetMsgHotType(msg.msgId);
                            }
                            obj = parser == null ? null : parser.ParseFrom(msg.data);
                            
                            lock (objThread)
                            {
                                mMsgCollectionEvents.Enqueue(new NetInfo(msg.msgId, obj));
                            }
                        }
                        catch (Exception e)
                        {
                            UnityEngine.Debug.LogError(" 解析异常  " + msg.msgId);
                        }
                    }
                }
            }
            Thread.Sleep(1);
        }
    }

    private void MonitorEvent(ECM2 _type, NetInfo obj)
    {
        switch (_type)
        {
            case ECM2.ResHeartbeatMessage://心跳响应
                heart.Heartbeat data = Network.Deserialize<heart.Heartbeat>(obj);
                checkTheTime = 0;
                if (CSConstant.mHeartbeatNum == 0) HotFix_Invoke.Instance.RefreshTime(data);
                CSConstant.mHeartbeatNum++;
                if (CSConstant.mHeartbeatNum >= 2) CSConstant.mHeartbeatNum = 0;
                break;
            default: break;
        }
    }


}