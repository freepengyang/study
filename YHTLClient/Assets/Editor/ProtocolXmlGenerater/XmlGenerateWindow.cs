using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;

namespace ExtendEditor
{
    public static class XmlGenerate
    {
        [MenuItem("Tools/传奇/网络消息一键转换")]
        public static void ProtocolGenerate()
        {
            GenerateMsgAction();
            GenerateMsgId();
            GenerateMsgParser();
            GenerateMsgHandler();
            AssetDatabase.Refresh();
        }

        const string mFmtString = "\t{0} = {1},";
        const int mTabCount = 6;
        static void Generate(XmlFileData data, out string results)
        {
            results = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(data.filePath);
            var mainNode = xmlDoc.SelectSingleNode("messages") as XmlElement;
            var keyId = mainNode.Attributes.GetNamedItem("id").Value;
            keyId.Trim();
            //Debug.LogFormat("<color=#00ffff>keyId={0}</color>", keyId);
            var childNodes = mainNode.ChildNodes;
            StringBuilder builder = new StringBuilder(1024);
            builder.AppendLine($"\t#region {data.fileName}");
            foreach (XmlElement child in childNodes)
            {
                var childKeyId = child.Attributes.GetNamedItem("id").Value;
                childKeyId.Trim();
                //Debug.LogFormat("<color=#00ffff>childKeyId={0}</color>", childKeyId);
                var className = child.Attributes.GetNamedItem("class").Value.Trim();
                var desc = child.Attributes.GetNamedItem("desc").Value.Trim();
                var lineContent = string.Format(mFmtString, className, keyId + childKeyId);
                builder.Append(lineContent);
                for (int i = 0; i < mTabCount; ++i)
                {
                    builder.Append("\t");
                }
                builder.Append("//");
                builder.AppendLine(desc);
            }
            builder.AppendLine($"\t#endregion");
            GUIUtility.systemCopyBuffer = builder.ToString();
            results = builder.ToString();
            FNDebug.LogFormat("<color=#00ffff>copyed succeed</color>");
        }

        const string mFmtNetMsgString = "\t\thotDic.Add((int)ECM.{0},{1}.{2}.Parser);";
        static System.Text.RegularExpressions.Regex netmsgReg = new System.Text.RegularExpressions.Regex(@"com.sh.game.proto.([A-Za-z]+)Protos.(\w+)");
        static void GenerateNetMsg(XmlFileData data, StringBuilder builder)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(data.filePath);
            var mainNode = xmlDoc.SelectSingleNode("messages") as XmlElement;
            var childNodes = mainNode.ChildNodes;
            //StringBuilder builder = new StringBuilder(1024);
            builder.AppendLine($"\t\t/*----------------{data.fileName}----------------*/");

            foreach (XmlElement child in childNodes)
            {
                bool responseMsg = child.Attributes.GetNamedItem("type").Value.Trim() == "toClient";
                if (!responseMsg)
                {
                    continue;
                }

                var className = child.Attributes.GetNamedItem("class").Value.Trim();
                var protoNode = child.SelectSingleNode("proto") as XmlElement;
                if (null == protoNode)
                {
                    continue;
                }

                var protoName = protoNode.Attributes.GetNamedItem("class").Value;
                var match = netmsgReg.Match(protoName);
                if (!match.Success)
                {
                    FNDebug.LogErrorFormat("matched failed for {0}", protoName);
                    continue;
                }
                protoName = match.Groups[2].Value;
                var ns = match.Groups[1].Value.ToLower();
                builder.AppendFormat(mFmtNetMsgString, className, ns, protoName);
                builder.AppendLine();
            }
            //GUIUtility.systemCopyBuffer = builder.ToString();
            //Debug.LogFormat("<color=#00ffff>generate {0} parser succeed</color>",data.fileName);
        }
        const string mFmtNetMsgActionString = "\t\t{{(int)ECM.{0},typeof({1})}},";
        static void GenerateNetMsgAction(XmlFileData data, StringBuilder builder)
        {
            if (data.fileName.Equals("heart"))
                return;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(data.filePath);
            var mainNode = xmlDoc.SelectSingleNode("messages") as XmlElement;
            var childNodes = mainNode.ChildNodes;
            builder.AppendLine($"\t\t/*----------------{data.fileName}----------------*/");
            var csClassName = "CSNet" + data.fileName[0].ToString().ToUpper() + data.fileName.Substring(1);

            if(data.fileName.Equals("union"))
            {
                int v = 12121;
                v += 1111;
                v += 13123;
            }

            if (data.fileName.Equals("user"))
            {
                builder.AppendLine("\t\t{(int)ECM.Connect,typeof(CSNetUser)},");
                builder.AppendLine("\t\t{(int)ECM.ConnectFail,typeof(CSNetUser)},");
                builder.AppendLine("\t\t{(int)ECM.Disconnect,typeof(CSNetUser)},");
            }

            foreach (XmlElement child in childNodes)
            {
                bool responseMsg = child.Attributes.GetNamedItem("type").Value.Trim() == "toClient";
                if (!responseMsg)
                {
                    continue;
                }

                var className = child.Attributes.GetNamedItem("class").Value.Trim();

                builder.AppendFormat(mFmtNetMsgActionString, className, csClassName);
                builder.AppendLine();
            }
            //GUIUtility.systemCopyBuffer = builder.ToString();
            FNDebug.LogFormat("<color=#00ffff>generate {0} msgAction succeed</color>", data.fileName);
        }

        static System.Text.RegularExpressions.Regex reg_system_var = new System.Text.RegularExpressions.Regex(@"System\.([^(,| )]+)");
        static System.Text.RegularExpressions.Regex reg_repeated_var = new System.Text.RegularExpressions.Regex(@"Google.Protobuf.Collections.RepeatedField`1\[\[([^,]+)");

        static void GenerateCSReqClass(XmlFileData data)
        {
            string fileName = data.fileName[0].ToString().ToUpper() + data.fileName.Substring(1);
            string fileNameSpace = data.fileName.ToLower();
            var savePath = $"{Application.dataPath}/HotFix_Project/Hotfix/Scripts/Network/Request/Req_{fileName}.cs";

            var orgContent = string.Empty;
            if (System.IO.File.Exists(savePath))
            {
                orgContent = System.IO.File.ReadAllText(savePath);
            }
            bool contentHasExited = !string.IsNullOrEmpty(orgContent);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(data.filePath);
            var mainNode = xmlDoc.SelectSingleNode("messages") as XmlElement;
            var childNodes = mainNode.ChildNodes;
            StringBuilder builder = new StringBuilder(1024);
            StringBuilder builderFunction = new StringBuilder(1024);

            if (!orgContent.Contains("using System;"))
                builder.AppendLine($"using System;");
            if (!orgContent.Contains("using Google.Protobuf.Collections;"))
                builder.AppendLine($"using Google.Protobuf.Collections;");
            builder.Append(orgContent);
            if (!contentHasExited)
            {
                builder.AppendLine($"public partial class Net");
                builder.AppendLine($"{{");
            }

            string className = string.Empty;
            foreach (XmlElement child in childNodes)
            {
                bool requestMsg = child.Attributes.GetNamedItem("type").Value.Trim() == "toServer";
                if (!requestMsg)
                {
                    continue;
                }

                className = child.Attributes.GetNamedItem("class").Value.Trim();

                var protoNode = child.SelectSingleNode("proto") as XmlElement;

                var protoName = string.Empty;
                if (null != protoNode)
                {
                    protoName = protoNode.Attributes.GetNamedItem("class").Value;
                    protoName = protoName.Substring(protoName.LastIndexOf('.') + 1);
                }

                //如果内容已经存在 DONOTHING !
                if (contentHasExited && orgContent.Contains(className))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(protoName))
                {
                    builderFunction.AppendLine($"\tpublic static void {className}()");
                }
                else
                {
                    var fields = GetArgumentsList($"{fileNameSpace}.{protoName}");
                    builderFunction.Append($"\tpublic static void {className}(");
                    for (int i = 0; i < fields.Length; ++i)
                    {
                        var fieldTypeName = fields[i].FieldType.FullName;
                        if (reg_repeated_var.IsMatch(fieldTypeName))
                        {
                            var match = reg_repeated_var.Match(fieldTypeName);
                            var innerName = match.Groups[1].Value;
                            if (reg_system_var.IsMatch(innerName))
                            {
                                innerName = reg_system_var.Match(innerName).Groups[1].Value;
                            }
                            fieldTypeName = $"RepeatedField<{innerName}>";
                        }
                        else if (reg_system_var.IsMatch(fieldTypeName))
                        {
                            var match = reg_system_var.Match(fieldTypeName);
                            fieldTypeName = match.Groups[1].Value;
                        }

                        builderFunction.Append($"{fieldTypeName} {fields[i].Name.Trim('_')}");
                        if (i + 1 != fields.Length && fields.Length > 1)
                            builderFunction.Append($",");
                    }
                    builderFunction.AppendLine($")");
                }
                builderFunction.AppendLine($"\t{{");
                if (string.IsNullOrEmpty(protoName))
                {
                    builderFunction.AppendLine($"\t\tCSHotNetWork.Instance.SendMsg((int)ECM.{className},null);");
                }
                else
                {
                    builderFunction.AppendLine($"\t\t{fileNameSpace}.{protoName} req = CSProtoManager.Get<{fileNameSpace}.{protoName}>();");
                    if (true)
                    {
                        var fields = GetArgumentsList($"{fileNameSpace}.{protoName}");
                        for (int i = 0; i < fields.Length; ++i)
                        {
                            var memerName = fields[i].Name.Trim('_');
                            var fieldTypeName = fields[i].FieldType.FullName;
                            bool isRepeatedFiled = false;
                            if (reg_repeated_var.IsMatch(fieldTypeName))
                            {
                                var match = reg_repeated_var.Match(fieldTypeName);
                                var innerName = match.Groups[1].Value;
                                if (reg_system_var.IsMatch(innerName))
                                {
                                    innerName = reg_system_var.Match(innerName).Groups[1].Value;
                                }
                                fieldTypeName = $"RepeatedField<{innerName}>";
                                isRepeatedFiled = true;
                            }
                            else if (reg_system_var.IsMatch(fieldTypeName))
                            {
                                var match = reg_system_var.Match(fieldTypeName);
                                fieldTypeName = match.Groups[1].Value;
                            }

                            if (!isRepeatedFiled)
                            {
                                builderFunction.AppendLine($"\t\treq.{memerName} = {memerName};");
                            }
                            else
                            {
                                builderFunction.AppendLine($"\t\treq.{memerName}.Clear();");
                                builderFunction.AppendLine($"\t\treq.{memerName}.AddRange({memerName});");
                                builderFunction.AppendLine($"\t\t{memerName}.Clear();");
                                builderFunction.AppendLine($"\t\tCSNetRepeatedFieldPool.Put({memerName});");
                            }
                        }
                    }
                    builderFunction.AppendLine($"\t\tCSHotNetWork.Instance.SendMsg((int)ECM.{className},req);");
                    //builder.AppendLine($"\t\tCSProtoManager.Recycle(req);");
                }
                builderFunction.AppendLine($"\t}}");
            }

            string content = string.Empty;
            if (!contentHasExited)
            {
                builder.Append(builderFunction);
                builder.AppendLine($"}}");
                content = builder.ToString();
            }
            else
            {
                content = builder.ToString();
                var pos = content.LastIndexOf('}');
                content = content.Insert(pos, builderFunction.ToString());
            }
            System.IO.File.WriteAllText(savePath, content);
        }
        static void GenerateCSNetClass(XmlFileData data)
        {
            string fileName = data.fileName[0].ToString().ToUpper() + data.fileName.Substring(1);
            var savePath = $"{Application.dataPath}/HotFix_Project/Hotfix/Scripts/Network/Receive/CSNet{fileName}.cs";

            var content = string.Empty;
            if (System.IO.File.Exists(savePath))
            {
                content = System.IO.File.ReadAllText(savePath);
            }
            bool contentHasExited = !string.IsNullOrEmpty(content);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(data.filePath);
            var mainNode = xmlDoc.SelectSingleNode("messages") as XmlElement;
            var childNodes = mainNode.ChildNodes;
            StringBuilder builder = new StringBuilder(1024);
            StringBuilder builderFunction = new StringBuilder(1024);
            builder.Clear();
            builderFunction.Clear();
            builder.AppendLine($"public partial class CSNet{fileName} : CSNetBase");
            if (!contentHasExited)
                builderFunction.AppendLine($"public partial class CSNet{fileName} : CSNetBase");
            builder.AppendLine("{");
            if (!contentHasExited)
                builderFunction.AppendLine("{");
            builder.AppendLine("\tpublic override void NetCallback(ECM _type, NetInfo obj)");
            builder.AppendLine("\t{");
            builder.AppendLine("\t\tswitch (_type)");
            builder.AppendLine("\t\t{");
            string className = string.Empty;
            foreach (XmlElement child in childNodes)
            {
                bool responseMsg = child.Attributes.GetNamedItem("type").Value.Trim() == "toClient";
                if (!responseMsg)
                {
                    continue;
                }

                className = child.Attributes.GetNamedItem("class").Value.Trim();

                var protoNode = child.SelectSingleNode("proto") as XmlElement;
                if (null != protoNode)
                {
                    var protoName = protoNode.Attributes.GetNamedItem("class").Value;
                    protoName = protoName.Substring(protoName.LastIndexOf('.') + 1);
                    builder.AppendLine($"\t\t\tcase ECM.{className}:");
                    builder.AppendLine($"\t\t\t\tECM_{className}(obj);");
                    var functionName = $"ECM_{className}";
                    if (!contentHasExited || !content.Contains(functionName))
                    {
                        builderFunction.AppendLine($"\tvoid ECM_{className}(NetInfo info)");
                        builderFunction.AppendLine($"\t{{");
                        builderFunction.AppendLine($"\t\t{data.fileName}.{protoName} msg = Network.Deserialize<{data.fileName}.{protoName}>(info);");
                        builderFunction.AppendLine($"\t\tif(null == msg)");
                        builderFunction.AppendLine($"\t\t{{");
                        builderFunction.AppendLine($"\t\t\tFNDebug.LogError(\"Deserialize Msg Failed For{data.fileName}.{protoName}\");");
                        builderFunction.AppendLine($"\t\t\treturn;");
                        builderFunction.AppendLine($"\t\t}}");
                        builderFunction.AppendLine($"\t}}");
                    }
                    builder.AppendLine($"\t\t\tbreak;");
                }
                else
                {
                    builder.AppendLine($"\t\t\tcase ECM.{className}:");
                    builder.AppendLine($"\t\t\t\tECM_{className}(obj);");
                    var functionName = $"ECM_{className}";
                    if (!contentHasExited || !content.Contains(functionName))
                    {
                        builderFunction.AppendLine($"\tvoid ECM_{className}(NetInfo info)");
                        builderFunction.AppendLine($"\t{{");
                        builderFunction.AppendLine($"\t\t");
                        builderFunction.AppendLine($"\t}}");
                    }
                    builder.AppendLine($"\t\t\tbreak;");
                }
            }
            builder.AppendLine("\t\t\tdefault:");
            builder.AppendLine("\t\t\t\tHandByNetCallback(_type, obj);");
            builder.AppendLine("\t\t\tbreak;");
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t}");

            builder.AppendLine();
            if (!contentHasExited)
            {
                builderFunction.AppendLine("}");
                content = builderFunction.ToString();
            }
            else
            {
                var pos = content.LastIndexOf('}');
                content = content.Insert(pos, builderFunction.ToString());
            }
            builder.AppendLine("}");

            var saveExtendPath = $"{Application.dataPath}/HotFix_Project/Hotfix/Scripts/Network/Receive/CSNet{fileName}Extend.cs";
            System.IO.File.WriteAllText(saveExtendPath, builder.ToString());
            System.IO.File.WriteAllText(savePath, content);
        }

        static System.Reflection.FieldInfo[] GetArgumentsList(string clsName)
        {
            System.Reflection.FieldInfo[] fileds = new System.Reflection.FieldInfo[0];
            var instance = typeof(HotMain).Assembly.CreateInstance(clsName);
            if (null != instance)
            {
                fileds = instance.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            }
            return fileds;
        }
        public class XmlFileData
        {
            public string fileName;
            public string filePath;
        };

        static void GenerateMsgId()
        {
            var xmlCollectionPath = System.IO.Path.GetFullPath(Application.dataPath + "/../../xml/game_used_protocol_name.txt");
            var files = System.IO.File.ReadAllLines(xmlCollectionPath);
            HashSet<string> fileSet = new HashSet<string>();
            for (int i = 0; i < files.Length; ++i)
            {
                var file = System.IO.Path.GetFullPath(Application.dataPath + $"/../../xml/{files[i]}.xml");
                if (!System.IO.File.Exists(file))
                {
                    continue;
                }
                fileSet.Add(file);
            }

            List<XmlFileData> xmlDatas = new List<XmlFileData>(fileSet.Count);
            foreach (var file in fileSet)
            {
                xmlDatas.Add(new XmlFileData
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(file),
                    filePath = file,
                });
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("//# generated by tools , Do Not Edit It ...");
            builder.AppendLine("public enum ECM");
            builder.AppendLine("{");
            builder.AppendLine("\tBEGIN = 0x100,//用户相关的包");
            builder.AppendLine("\tConnect = 101,        //连接服务器");
            builder.AppendLine("\tDisconnect,     //正常断线");
            builder.AppendLine("\tConnectFail,        //连接失败");
            builder.AppendLine();
            //消息枚举
            for (int i = 0; i < xmlDatas.Count; ++i)
            {
                var xmlData = xmlDatas[i];
                string result = string.Empty;
                if (null != xmlData)
                    Generate(xmlData, out result);
                builder.Append(result);
            }
            builder.AppendLine("}");
            var sorePath = Application.dataPath + "/HotFix_Project/Hotfix/Scripts/Network/Common/MsgEnum.cs";
            System.IO.File.WriteAllText(sorePath, builder.ToString());
        }
        static void GenerateMsgParser()
        {
            var xmlCollectionPath = System.IO.Path.GetFullPath(Application.dataPath + "/../../xml/game_used_protocol_name.txt");
            var files = System.IO.File.ReadAllLines(xmlCollectionPath);
            HashSet<string> fileSet = new HashSet<string>();
            for (int i = 0; i < files.Length; ++i)
            {
                var file = System.IO.Path.GetFullPath(Application.dataPath + $"/../../xml/{files[i]}.xml");
                if (!System.IO.File.Exists(file))
                {
                    continue;
                }
                fileSet.Add(file);
            }

            List<XmlFileData> xmlDatas = new List<XmlFileData>(fileSet.Count);
            foreach (var file in fileSet)
            {
                xmlDatas.Add(new XmlFileData
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(file),
                    filePath = file,
                });
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("//# generated by tools , Do Not Edit It ...");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System;");
            builder.AppendLine("using Google.Protobuf;");
            builder.AppendLine();
            builder.AppendLine("public static class NetMsgParser");
            builder.AppendLine("{");
            builder.AppendLine("\tpublic static void InitNetMsg()");
            builder.AppendLine("\t{");
            builder.AppendLine("\t\tDictionary<int, MessageParser> hotDic = NetMsgMain.Instance.mNetInfoDicHot;");
            //消息枚举
            for (int i = 0; i < xmlDatas.Count; ++i)
            {
                var xmlData = xmlDatas[i];
                if (null != xmlData)
                    GenerateNetMsg(xmlData, builder);
            }
            builder.AppendLine("\t}");

            builder.AppendLine("\tprivate static CSNetBase cSNetBase;");
            builder.AppendLine("\tpublic static void ProcessNetwork(NetInfo netinfo)");
            builder.AppendLine("\t{");
            builder.AppendLine("\t\tcSNetBase = CSNetFactory.Get(netinfo.msgId);");
            builder.AppendLine("\t\tif (cSNetBase != null)");
            builder.AppendLine("\t\t\tcSNetBase.NetCallback((ECM)netinfo.msgId, netinfo);");
            builder.AppendLine("\t}");
            builder.AppendLine("}");
            builder.AppendLine();
            var sorePath = Application.dataPath + "/HotFix_Project/Hotfix/Scripts/Network/Common/NetMsgParser.cs";
            System.IO.File.WriteAllText(sorePath, builder.ToString());
        }
        static void GenerateMsgHandler()
        {
            var xmlCollectionPath = System.IO.Path.GetFullPath(Application.dataPath + "/../../xml/game_used_protocol_name.txt");
            var files = System.IO.File.ReadAllLines(xmlCollectionPath);
            HashSet<string> fileSet = new HashSet<string>();
            for (int i = 0; i < files.Length; ++i)
            {
                var file = System.IO.Path.GetFullPath(Application.dataPath + $"/../../xml/{files[i]}.xml");
                if (!System.IO.File.Exists(file))
                {
                    continue;
                }
                fileSet.Add(file);
            }

            List<XmlFileData> xmlDatas = new List<XmlFileData>(fileSet.Count);
            foreach (var file in fileSet)
            {
                xmlDatas.Add(new XmlFileData
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(file),
                    filePath = file,
                });
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("//# generated by tools , Do Not Edit It ...");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System;");
            builder.AppendLine("using Google.Protobuf;");
            builder.AppendLine();
            builder.AppendLine("public static class NetCallabck");
            builder.AppendLine("{");

            builder.AppendLine("\tpublic static Type GetCallbackType(int id)");
            builder.AppendLine("\t{");
            builder.AppendLine("\t\tif (mNetCallbackDic.ContainsKey(id))");
            builder.AppendLine("\t\t\treturn mNetCallbackDic[id];");
            builder.AppendLine("\t\t");
            builder.AppendLine("\t\treturn null;");
            builder.AppendLine("\t}");

            builder.AppendLine("\tpublic static Dictionary<int, Type> mNetCallbackDic = new Dictionary<int, Type>");
            builder.AppendLine("\t{");

            //消息枚举
            for (int i = 0; i < xmlDatas.Count; ++i)
            {
                var xmlData = xmlDatas[i];
                if (null != xmlData)
                    GenerateNetMsgAction(xmlData, builder);
            }
            builder.AppendLine("\t};");

            builder.AppendLine("}");
            builder.AppendLine();

            var sorePath = Application.dataPath + "/HotFix_Project/Hotfix/Scripts/Network/Common/NetMsg.cs";
            System.IO.File.WriteAllText(sorePath, builder.ToString());
        }
        static void GenerateMsgAction()
        {
            var xmlCollectionPath = System.IO.Path.GetFullPath(Application.dataPath + "/../../xml/game_used_protocol_name.txt");
            var files = System.IO.File.ReadAllLines(xmlCollectionPath);
            HashSet<string> fileSet = new HashSet<string>();
            for (int i = 0; i < files.Length; ++i)
            {
                var file = System.IO.Path.GetFullPath(Application.dataPath + $"/../../xml/{files[i]}.xml");
                if (!System.IO.File.Exists(file))
                {
                    continue;
                }
                fileSet.Add(file);
            }

            List<XmlFileData> xmlDatas = new List<XmlFileData>(fileSet.Count);
            foreach (var file in fileSet)
            {
                xmlDatas.Add(new XmlFileData
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(file),
                    filePath = file,
                });
            }

            //消息枚举
            for (int i = 0; i < xmlDatas.Count; ++i)
            {
                var xmlData = xmlDatas[i];
                if (null != xmlData)
                    GenerateCSReqClass(xmlData);
                if (null != xmlData)
                    GenerateCSNetClass(xmlData);
            }
        }
    }
}