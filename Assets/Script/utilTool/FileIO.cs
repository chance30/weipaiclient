using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

public class FileIO
{


    public static string LOCAL_RES_PATH = "";//本地资源加载地址
    public static string DEBUG_FILE = "Debug.txt";//本地资源加载地址
    public static string Game_File = "File.txt";//本地资源加载地址

    static void SetPath()
    {
        if (LOCAL_RES_PATH.Equals(""))
        {
            LOCAL_RES_PATH =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
 Application.dataPath + "/";
#elif UNITY_ANDROID
            Application.persistentDataPath + "/";
#elif UNITY_IPHONE
            Application.persistentDataPath + "/";
#endif
        }
    }
    public static void Debug_Write(string str)
    {
        //return;
        SetPath();
        str = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond + "-" + str + "\n";
        FileStream stream = new FileStream(LOCAL_RES_PATH + DEBUG_FILE, FileMode.OpenOrCreate);
        byte[] data = Encoding.UTF8.GetBytes(str);
        stream.Position = stream.Length;
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
    }

    public static string usID = "null";
    public static string Token = "null";

    public static void ReadFileOption()
    {
        if (LOCAL_RES_PATH.Equals(""))
            LOCAL_RES_PATH =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
 Application.dataPath + "/";
#elif UNITY_ANDROID
    Application.persistentDataPath + "/";
#elif UNITY_IPHONE
    Application.persistentDataPath + "/";
#endif

        //string str = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond + "-" + "\n";
        //FileStream stream = new FileStream(LOCAL_RES_PATH + DEBUG_FILE, FileMode.OpenOrCreate);
        //byte[] data = Encoding.UTF8.GetBytes(str);
        //stream.Position = stream.Length;
        //stream.Write(data, 0, data.Length);
        //stream.Flush();
        //stream.Close();


        FileStream stream = new FileStream(LOCAL_RES_PATH + Game_File, FileMode.OpenOrCreate);

        if (stream.Length < 1)
        {
            byte[] data = Encoding.UTF8.GetBytes("null\nnull\n1\n1\n1\n1\n");
            stream.Write(data, 0, data.Length);
            usID = Token = "null";
        }
        else
        {
            byte[] data = new byte[1024];
            stream.Read(data, 0, (int)stream.Length);
            string str = Encoding.UTF8.GetString(data);

            string[] strS = str.Split('\n');
            usID = strS[0].Trim();
            Token = strS[1].Trim();

        }
        stream.Flush();
        stream.Close();
    }

    public static Dictionary<string, Sprite> wwwSpriteImage = new Dictionary<string, Sprite>();
    public static void SaveLogin(string usid, string token)
    {

        usID = usid;
        Token = token;
        SaveFile();
    }

    public static void SaveVolume()
    {
        SaveFile();
    }

    static void SaveFile()
    {
        FileStream stream = new FileStream(LOCAL_RES_PATH + Game_File, FileMode.Create);

        byte[] data = Encoding.UTF8.GetBytes(
            usID + "\n" +
            Token + "\n"

            );
        stream.Position = 0;
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
    }
}
