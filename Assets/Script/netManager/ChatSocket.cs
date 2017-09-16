using System;
using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using AssemblyCSharp;
using System.IO;

public class ChatSocket{
	//reset对象
	//public ManualResetEvent connectDone = new ManualResetEvent(false);
	//tcp客户端,即是与服务端通讯的组件
	TcpClient tcpclient = new TcpClient();
	//网络流
	NetworkStream stream;
	//接受到的数据
	//public byte[] databuffer = new byte[StateObject.BufferSize];//数据缓冲区;
	public int offset = 0;//处理的位置
	public int end = 0;//数据缓冲区的长度

	byte[] headBytes;
	byte[] sources;
	int waitLen = 0;
	bool isWait = false;

	private static ChatSocket _instance;
	public static ChatSocket getInstance(){
		if (_instance == null) {
			_instance = new ChatSocket ();
			//_instance.Connect ();
		}
		return _instance;
	}

	/// <summary>
	/// 连接到服务器
	/// </summary>
	/// <param name="ip">服务器IP</param>
	/// <returns></returns>
	public void Connect()
	{

		try
		{
			tcpclient = new TcpClient();
			//防止延迟,即时发送!
			tcpclient.NoDelay = true;
			tcpclient.BeginConnect(APIS.chatSocketUrl, 10112, new AsyncCallback(ConnectCallback), tcpclient);
		}
		catch(Exception ex)
		{
			//设置标志,连接服务端失败!
			Connect();
			Debug.Log(ex.ToString());
		}
	}

	/// <summary>
	/// 关闭网络流
	/// </summary>
	private void DisConnect()
	{
		if (tcpclient != null)
		{
			tcpclient.Close();
			tcpclient = null;
		}
		if (stream != null)
		{
			stream.Close();
			stream = null;
		}
	}

	public void sendMsg(ChatRequest client){
		if (tcpclient != null) {
			if (tcpclient.Connected) {
				SendData (client.ToBytes ());
			}
		}else{
		//	showMessageTip ("聊天服务器断开，正在重连。。。");
			Connect ();
		}
	}

	/// <summary>
	/// 发送数据
	/// </summary>
	private void SendData(byte[] data)
	{
		try
		{
			if (stream != null)
			{
				stream.Write(data, 0, data.Length);
			}else{
				//TipsManagerScript.getInstance ().setTips ("聊天服务器断开，正在重连。。。");
				Connect ();
			}
		}
		catch(Exception ex)
		{
			Debug.Log(ex.ToString());
		}

	}

	/// <summary>
	/// 异步连接的回调函数
	/// </summary>
	/// <param name="ar"></param>
	private void ConnectCallback(IAsyncResult ar)
	{
		//connectDone.Set();
		if ((tcpclient != null) && (tcpclient.Connected)) {
			stream = tcpclient.GetStream ();
			asyncread (tcpclient);
			MyDebug.Log ("聊天服务器已经连接!");
		//	showMessageTip ("聊天服务器已经连接!");
		} else {
			closeSocket ();
			return;
		}
		TcpClient t = (TcpClient)ar.AsyncState;
		try
		{
			t.EndConnect(ar);
		}
		catch(Exception ex)
		{
			//设置标志,连接服务端失败!
			Debug.Log(ex.ToString());
			//	tcpclient.BeginConnect(APIS.socketUrl, 1101, new AsyncCallback(ConnectCallback), tcpclient);
		}
	}
	/// <summary>
	/// 异步读TCP数据
	/// </summary>
	/// <param name="sock"></param>
	private void asyncread(TcpClient sock)
	{
		StateObject state = new StateObject();
		state.client = sock;
		NetworkStream stream;
		try
		{
			stream = sock.GetStream();
			if (stream.CanRead)
			{
				try
				{
					IAsyncResult ar = stream.BeginRead(state.buffer, 0, StateObject.BufferSize,
						new AsyncCallback(TCPReadCallBack), state);

				}
				catch(Exception ex)
				{
					//设置标志,连接服务端失败!

					Debug.Log(ex.ToString());
				}
			}
		}
		catch(Exception ex)
		{
			//设置标志,连接服务端失败!
			// NetManaged.isConnectServer = false;
			// NetManaged.surcessstate = 0;
			Debug.Log(ex.ToString());
		}

	}

	/// <summary>
	/// TCP读数据的回调函数
	/// </summary>
	/// <param name="ar"></param>
	private void TCPReadCallBack(IAsyncResult ar)
	{
		StateObject state = (StateObject)ar.AsyncState;
		//主动断开时
		if ((state.client == null) || (!state.client.Connected))
		{
			closeSocket ();
			return;
		}
		int numberOfBytesRead;
		NetworkStream mas = state.client.GetStream();
		numberOfBytesRead = mas.EndRead(ar);
		state.totalBytesRead += numberOfBytesRead;
		if (numberOfBytesRead > 0)
		{
			byte[] dd = new byte[numberOfBytesRead];
			Array.Copy(state.buffer,0,dd,0,numberOfBytesRead);
			if (isWait) {
				byte[] temp = new byte[sources.Length+dd.Length];
				sources.CopyTo (temp,0);
				dd.CopyTo (temp,sources.Length);
				sources = temp;
				if (sources.Length >= waitLen) {
					ReceiveCallBack (sources.Clone() as byte[]);
					isWait = false;
					waitLen = 0;
				}
			} else {
				sources = null;
				ReceiveCallBack (dd);
			}
			mas.BeginRead(state.buffer, 0, StateObject.BufferSize,
				new AsyncCallback(TCPReadCallBack), state);
		}
		else
		{
			//被动断开时 
			mas.Close();
			state.client.Close();
			mas = null;
			state = null;
			//设置标志,连接服务端失败!
			showMessageTip("客户端被动断开聊天服务器");
		}
	}

	private void showMessageTip(string message){
		ClientResponse temp = new ClientResponse ();
		temp.headCode = APIS.TIP_MESSAGE;
		temp.message = message;
		SocketEventHandle.getInstance ().addResponse (temp);
	}
	/// <summary>
	/// 读取大端序的int
	/// </summary>
	/// <param name="value"></param>
	public int ReadInt(byte[] intbytes)
	{
		Array.Reverse(intbytes);
		return BitConverter.ToInt32(intbytes, 0);
	}

	public short ReadShort(byte[] intbytes){
		Array.Reverse(intbytes);
		return BitConverter.ToInt16(intbytes, 0);
	}

	private void ReceiveCallBack(byte[] m_receiveBuffer)
	{
		//通知调用端接收完毕
		try
		{
			MemoryStream ms = new MemoryStream(m_receiveBuffer);
			BinaryReader buffers = new BinaryReader(ms, UTF8Encoding.Default);
			readBuffer(buffers);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void readBuffer(BinaryReader buffers){
		byte flag = buffers.ReadByte();
		int lens = ReadInt(buffers.ReadBytes(4));
		if (lens > buffers.BaseStream.Length) {
			waitLen = lens;
			isWait = true;
			buffers.BaseStream.Position = 0;
			byte[] dd = new byte[buffers.BaseStream.Length];
			byte[] temp =  buffers.ReadBytes ((int)buffers.BaseStream.Length);
			Array.Copy (temp, 0, dd, 0, (int)buffers.BaseStream.Length);
			if (sources == null) {
				sources = dd;
			} 
			return;
		}
		int headcode = ReadInt(buffers.ReadBytes(4));
		int sendUuid = ReadInt (buffers.ReadBytes(4));
		int soundLen = ReadInt(buffers.ReadBytes(4));
		if(flag == 1){
			byte[] sound = buffers.ReadBytes (soundLen);
			ClientResponse response = new ClientResponse();
			response.headCode = headcode;
			response.bytes = sound;
			response.message = sendUuid.ToString ();
			MyDebug.Log("chat.headCode = "+response.headCode+"  sound.lenght =   "+soundLen);
			SocketEventHandle.getInstance().addResponse(response);
		}
		if (buffers.BaseStream.Position < buffers.BaseStream.Length) {
			readBuffer (buffers);
		}
	}

	public void closeSocket(){
		DisConnect ();
	}

}


internal class StateObject
{
	public TcpClient client = null;
	public int totalBytesRead = 0;
	public const int BufferSize = 1024*1024*2;
	public string readType = null;
	public byte[] buffer = new byte[BufferSize];
}
