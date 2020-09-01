﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
public class NetworkeUtil
{
#if UNITY_IPHONE || UNITY_IOS || UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern string getIPv6(string mHost, string mPort);
    //"192.168.1.1&&ipv4"
    public static string GetIPv6(string mHost, string mPort)
    {
#if UNITY_IPHONE && !UNITY_EDITOR
string mIPv6=getIPv6(mHost,mPort);
return mIPv6;
#else
        return mHost+ "&&ipv4";
#endif
    }


    public static void GetIPType(String serverIp, String serverPorts, out String newServerIp, out AddressFamily mIPType)
    {
        mIPType = AddressFamily.InterNetwork;
        newServerIp = serverIp;
        try
        {
            string mIPv6 = GetIPv6(serverIp, serverPorts);

            if (!string.IsNullOrEmpty(mIPv6))
            {
                string[] m_StrTemp = System.Text.RegularExpressions.Regex.Split(mIPv6, "&&");
                if (m_StrTemp != null && m_StrTemp.Length >= 2)
                {
                    string IPType = m_StrTemp[1];
                    if (IPType == "ipv6")
                    {
                        newServerIp = m_StrTemp[0];
                        mIPType = AddressFamily.InterNetworkV6;
                    }
                }
            }
        }
        catch (Exception e)
        {
            FNDebug.LogError(e.Message);
        }
    }
#endif
}
