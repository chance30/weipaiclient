using UnityEngine;
using System.Collections;
using System.Net;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

public class IOSIpv6
{
    [DllImport("__Internal")]
    private static extern string getIPv6(string mHost, string mPort);

    //"192.168.1.1&&ipv4"
    static string GetIPv6(string mHost, string mPort)
    {
#if UNITY_IPHONE && !UNITY_EDITOR
		string mIPv6 = getIPv6(mHost, mPort);
		return mIPv6;
#else
        return mHost + "&&ipv4";
#endif
    }

    static void getIPType(string serverIp, string serverPorts, out string newServerIp, out AddressFamily mIPType)
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
            Debug.Log("GetIPv6 error:" + e);
        }

    }

    public static string GetIPv6Str(string serverIp, string serverPorts, out AddressFamily family)
    {
        //String newServerIp = "";
        //AddressFamily newAddressFamily = AddressFamily.InterNetwork;
        //getIPType(serverIp, serverPorts, out newServerIp, out newAddressFamily);
        //if (!string.IsNullOrEmpty(newServerIp)) { serverIp = newServerIp; }
        //socketClient = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
        //ClientLog.Instance.Log("Socket AddressFamily :" + newAddressFamily.ToString() + "ServerIp:" + serverIp);


        String newServerIp = "";
        family = AddressFamily.InterNetwork;
        getIPType(serverIp, serverPorts, out newServerIp, out family);
        if (!string.IsNullOrEmpty(newServerIp)) 
        { 
            serverIp = newServerIp; 
        }
        Debug.Log(serverIp);
        return serverIp;
        //Socket socketClient = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
        //Debug.Log("Socket AddressFamily :" + newAddressFamily.ToString() + "ServerIp:" + serverIp);
    }
}
