public enum ECM2
{
    BEGIN = 0x100,//用户相关的包
    Connect = 101,        //连接服务器
    Disconnect,     //正常断线   
    ConnectFail,        //连接失败
    ReqHeartbeatMessage = 100101,//心跳请求
    ResHeartbeatMessage,         //心跳响应
    ReqRechargeMessage,
}

