using System;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace AssemblyCSharp
{
	public class SocketForDebugPanel
	{

		private Socket socket;
		private byte[] m_receiveBuffer = new byte[1024*1024];
		public  delegate void ServerCallBackEvent (ClientResponse response);
		public ServerCallBackEvent LoginCallBack_debug;//登录回调

		public SocketForDebugPanel ()
		{
			try {
				socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.Connect(APIS.socketUrl,1101);
				if (socket.Connected)
				{
					socket.BeginReceive(m_receiveBuffer, 0, m_receiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);
				}
			} catch (ArgumentNullException e) {
			} catch (SocketException e) {

			}
		}

		/// <summary>
		/// Sends the message.
		/// </summary>
		/// <param name="msg">Message.</param>
		public void sendMsg(ClientRequest msg){
			if (socket != null)
			{
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
			try
			{
				MemoryStream ms = new MemoryStream(m_receiveBuffer);
				BinaryReader buffers = new BinaryReader(ms, UTF8Encoding.Default);
				byte flag = buffers.ReadByte();
				int lens = ReadInt(buffers.ReadBytes(4));
				int headcode = ReadInt(buffers.ReadBytes(4));
				int status = ReadInt(buffers.ReadBytes(4));
				short messageLen = ReadShort(buffers.ReadBytes(2));
				if(flag == 1){
					string message = Encoding.UTF8.GetString(buffers.ReadBytes(messageLen));
					ClientResponse response = new ClientResponse();
					response.status = status;
					response.message = message;
					response.headCode = headcode;
					MyDebug.Log("DebugForSocket response.headCode = "+response.headCode+"  response.message =   "+message);
					switch(response.headCode){
					case APIS.LOGIN_RESPONSE:
						MyDebug.Log("测试用户登录成功");
						if(LoginCallBack_debug != null){
							LoginCallBack_debug(response);
						}
						break;
					case APIS.CREATEROOM_RESPONSE:
						break;
					case APIS.JOIN_ROOM_RESPONSE:
						MyDebug.Log("测试用户加入房间成功");
						break;
					case APIS.STARTGAME_RESPONSE_NOTICE:
						break;
					case APIS.PICKCARD_RESPONSE:
						break;
					case APIS.OTHER_PICKCARD_RESPONSE_NOTICE:
						break;
					case APIS.CHUPAI_RESPONSE:
						break;
					}
				}
				socket.EndReceive(ar);
				socket.BeginReceive(m_receiveBuffer, 0, m_receiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}

