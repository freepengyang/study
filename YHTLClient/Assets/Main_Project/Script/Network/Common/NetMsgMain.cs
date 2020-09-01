using System.Collections.Generic;
using Google.Protobuf;
using heart;

public class NetMsgMain
{
    private static NetMsgMain mInstance;

    public static NetMsgMain Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new NetMsgMain();
            }
            return mInstance;
        }
        set { mInstance = value; }
    }

    //主工程注册的消息
    public Dictionary<int, Google.Protobuf.MessageParser> mNetInfoDic = new Dictionary<int, MessageParser>();
    //热更工程注册的消息
    public Dictionary<int, Google.Protobuf.MessageParser> mNetInfoDicHot = new Dictionary<int, MessageParser>(512);

    public NetMsgMain()
    {
        //解析服务器proto
        mNetInfoDic.Add((int)ECM2.ResHeartbeatMessage,Heartbeat.Parser);


    }

    public bool IsHaveMsgID(int id)
    {
        return mNetInfoDic.ContainsKey(id);
    }

    public Google.Protobuf.MessageParser GetMsgType(int id)
    {
        if (mNetInfoDic.ContainsKey(id))
        {
            return mNetInfoDic[id];
        }
        return null;
    }

    public Google.Protobuf.MessageParser GetMsgHotType(int id)
    {
        if (mNetInfoDicHot.ContainsKey(id))
        {
            return mNetInfoDicHot[id];
        }
        return null;
    }
}