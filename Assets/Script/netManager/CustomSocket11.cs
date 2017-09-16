using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using UnityEngine;
using System.Runtime.Remoting.Messaging;

namespace AssemblyCSharp
{
	public class CustomSocket11
	{
		private Socket socket;
		private byte[] m_receiveBuffer = new byte[32*1024];

		private static CustomSocket11 _instance;

		public static CustomSocket11 getInstance(){
			if (_instance == null) {
				_instance = new CustomSocket11 ();
			}
			return _instance;
		}

		private CustomSocket11 ()
		{
			try {
				socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				//socket.Connect (new IPEndPoint (IPAddress.Parse ("192.168.0.107"), 1101));
				socket.Connect(APIS.socketUrl,1101);
				if (socket.Connected)
				{
					socket.BeginReceive(m_receiveBuffer, 0, m_receiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
				}
			} catch (ArgumentNullException e) {
				MyDebug.Log ("argumentNullException: = " + e);
				//Console.WriteLine (" {0}", e);
			} catch (SocketException e) {
				MyDebug.Log ("SocketException  == "+ e);
				//Console.WriteLine ("SocketException:{0}", e);
			}
		}

		/// <summary>
		/// Sends the message.
		/// </summary>
		/// <param name="msg">Message.</param>
		public void sendMsg(ClientRequest msg){
			if (socket != null)
			{
				MyDebug.Log ("sendMsg  ==>>  "+ msg.headCode+"  content ==> "+msg.messageContent);
				socket.Send(msg.ToBytes(),msg.totelLenght, SocketFlags.None);
			}
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
		/// <summary>
		/// Receives the call back.
		/// </summary>
		/// <param name="ar">Ar.</param>
		private void ReceiveCallBack(IAsyncResult ar)
		{
			//通知调用端接收完毕
			try
			{

				//int REnd = socket.EndReceive(ar);
				//BinaryReader br = new BinaryReader(m_receiveBuffer);
				MyDebug.Log("socket.Available === >>>  "+socket.Available);
				if(socket.Available <= 0){
					MemoryStream ms = new MemoryStream(m_receiveBuffer);
					BinaryReader buffers = new BinaryReader(ms, UTF8Encoding.Default);
					byte flag = buffers.ReadByte();
					int lens = ReadInt(buffers.ReadBytes(4));
					int headcode = ReadInt(buffers.ReadBytes(4));
					int status = ReadInt(buffers.ReadBytes(4));
					short messageLen = ReadShort(buffers.ReadBytes(2));
					MyDebug.Log("response.flag = "+flag);
					socket.EndReceive(ar);
					if(flag == 1){
						string message = Encoding.UTF8.GetString(buffers.ReadBytes(messageLen));
						ClientResponse response = new ClientResponse();
						response.status = status;
						response.message = message;
						response.headCode = headcode;
						MyDebug.Log("response.headCode = "+response.headCode+"  response.message =   "+message);
						SocketEventHandle.getInstance().addResponse(response);
					}
				}
				socket.BeginReceive(m_receiveBuffer, 0, m_receiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);

				//接收下一个消息(因为这是一个递归的调用，所以这样就可以一直接收消息了）
			}
			catch (Exception ex)
			{
				MyDebug.Log ("socket exception");
				socket.Close ();
				throw new Exception(ex.Message);
			}
		}


		public void closeSocket(){
			if (socket != null) {
				socket.Close ();
			}
		}
	}
}

