using System;
using System.IO;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

namespace AssemblyCSharp
{

	public class ChatRequest
	{
		private byte Flag = 1;
		protected int Len = 0;
		public int headCode;
		public byte[] ChatSound;
		public int totelLenght;
		public List<int> userList;
		public int myUUid;

		public ChatRequest ()
		{
		}

		/// <summary>
		/// 写入大端序的int
		/// </summary>
		/// <param name="value"></param>
		public byte[] WriterInt(int value)
		{
			byte[] bs = BitConverter.GetBytes(value);
			Array.Reverse(bs);
			return bs;
		}
		/// <summary>
		/// Writes the short.
		/// </summary>
		/// <returns>The short.</returns>
		/// <param name="value">Value.</param>
		public byte[] WriteShort(short value){
			byte[] bs = BitConverter.GetBytes(value);
			Array.Reverse(bs);
			return bs;
		}

		/// <summary>
		///  转换为 byte[]
		/// </summary>
		/// <returns></returns>
		public byte[] ToBytes()
		{
			byte[] _bytes; //自定义字节数组，用以装载消息协议
			using (MemoryStream memoryStream = new MemoryStream()) //创建内存流
			{
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream,UTF8Encoding.Default); //以二进制写入器往这个流里写内容
				Len =4;
				if (userList != null && userList.Count > 0) {
					Len += (userList.Count + 1) * 4;
				}
				if (ChatSound != null) {
					Len += ChatSound.Length+4;
				}
				if (myUUid != null && myUUid != 0) {
					Len += 4;
				}
				binaryWriter.Write(Flag); //写入协议一级标志，占1个字节
				binaryWriter.Write(WriterInt(Len));//占4个字节
				binaryWriter.Write(WriterInt(headCode)); //写入实际消息长度，占4个字节
				if (userList != null && userList.Count > 0) {
					binaryWriter.Write (WriterInt(userList.Count));
					for(int i=0;i<userList.Count;i++){
						binaryWriter.Write (WriterInt(userList[i]));
					}

				}
				if(myUUid != null && myUUid != 0){
					binaryWriter.Write (WriterInt(myUUid));
				}
				if(ChatSound != null && ChatSound.Length >0){
					binaryWriter.Write(WriterInt(ChatSound.Length));
					binaryWriter.Write(ChatSound); //写入实际消息内容
					MyDebug.Log("chatSound length == >> "+ChatSound.Length);

				}
				_bytes = memoryStream.ToArray(); //将流内容写入自定义字节数组
				MyDebug.Log("data.Length   ==== >"+_bytes.Length);

				binaryWriter.Close(); //关闭写入器释放资源
			}
			totelLenght = _bytes.Length;
			return _bytes; //返回填充好消息协议对象的自定义字节数组
		}
	}
}

