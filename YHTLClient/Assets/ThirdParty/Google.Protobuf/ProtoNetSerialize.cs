using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.XGame.NetWork
{
    internal class ProtoNetSerialize
    {
        [ThreadStatic]
        private static MemoryStream s_Stream = null;
        [ThreadStatic]
       
        private static Dictionary<int, Type> s_DicIDMsg = new Dictionary<int, Type>();
        //private static Dictionary<Type, int> s_DicIDName = new Dictionary<Type, int>();
#pragma warning disable CS0414 // The field 'ProtoNetSerialize.s_Inited' is assigned but its value is never used
        private static bool s_Inited = false;
#pragma warning restore CS0414 // The field 'ProtoNetSerialize.s_Inited' is assigned but its value is never used

        public static void register(int id,Type type)
        {
            s_DicIDMsg.Add(id, type);
        }

        public static byte[] Encode(int socketId, Google.Protobuf.IMessage msg)
        {
            // if (s_DicIDName.ContainsKey(msg.GetType()))
            // {
            // MemoryStream ms = new MemoryStream();
            //ProtoBuf.Serializer.Serialize(ms, msg);

            //byte[] data = ms.ToArray();
            //int len = data.Length + 8;
            //Int32 id = 5;
            //byte[] lens = System.BitConverter.GetBytes(len);
            //byte[] ids = System.BitConverter.GetBytes(id);
            //byte[] sendData = new byte[len];

            //lens.CopyTo(sendData, 0);
            //ids.CopyTo(sendData, lens.Length);
            //data.CopyTo(sendData, lens.Length + ids.Length);

            //return sendData;
            int msgLen = msg.CalculateSize();

            int len = 4 + msgLen;
            byte[] ret = new byte[len];

            ProtoPreconditions.CheckNotNull(msg, "msg");
            byte[] result = new byte[msgLen];
            CodedOutputStream output = new CodedOutputStream(result);
            msg.WriteTo(output);
            output.CheckNoSpaceLeft(); 

            //int id = s_DicIDName[msg.GetType()];
            int id = socketId;
            ret[0] = (byte)(len >> 8);
            ret[1] = (byte)(len);
            ret[2] = (byte)(id >> 8);
            ret[3] = (byte)(id);

            result.CopyTo(ret, 4);

            return ret;

        }

        public static int readBodyLength = 0;
        public static int readId = 0;

        public static object Decode(int id,byte[] msgbuf)
        {
            if (id < 0)
            {
                UnityEngine.Debug.LogError("NetDataMgr.Instance.mMrgs  not have:" + id);
                return null;
            }
            readId = id;

            //if (s_DicIDMsg.ContainsKey(id))
            //{
                int sid = id;

            NetParam netPar;
            if(!NetDataMgr.Instance.Mrgs.TryGetValue(readId, out netPar))
            {
                UnityEngine.Debug.LogError("NetDataMgr.Instance.mMrgs  not have:" + sid);
                return null;
            }

                try
                {
                //ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(m_protoms, null, n.mType);
                //object msg = ProtoBuf.Serializer.Deserialize<Type>(DataStream);
                //int bodyLen = length - 4;
                object msg = netPar.Parser.ParseFrom(msgbuf);
              
                    if (msg == null)
                    {
                        return null;
                    }
                    //LogSystem.Info("decode message:id {0} len({1})[{2}]", id, msgbuf.Length - 2, msg.GetType().Name);
                    return msg;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
           // }
          //  return null;
        }
        private static MemoryStream DataStream
        {
            get
            {
                if (null == s_Stream)
                    s_Stream = new MemoryStream(4096);
                return s_Stream;
            }
        }
    }

}
