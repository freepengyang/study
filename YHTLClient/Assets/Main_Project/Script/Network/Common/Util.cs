// 对network meassage的一些实用函数

using System;
using System.IO;
using Google.Protobuf;

public partial class Network
{
    private static Network m_instance = new Network();

    public static Network Instance
    {
        get { return m_instance; }
    }

    public void Update()
    {
        CSNetwork.Instance.Update();
    }

    public void Derstroy()
    {
        m_instance = null;
    }

    public static T Deserialize<T>(object msg, bool islimit = false, bool isForceTest = false)
    {
        NetInfo info = (NetInfo) msg;
        return (T) info.obj;
    }

    public static T ReadOneDataConfig<T>(Type type, CSResourceWWW res)
    {
        T t = default(T);
        byte[] data = res.MirroyBytes;
        MessageParser parser = TableUtility.Instance.GetMsgHotType(type);
        object obj = null;
        if (data == null || parser == null) return default(T);
        obj =  parser.ParseFrom(data);
        return (T)obj;
    }
}