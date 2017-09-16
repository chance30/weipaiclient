using System;
/**
*项目内部的代理事件
*
*/
using UnityEngine;


namespace AssemblyCSharp
{
	public class CommonEvent :MonoBehaviour
	{
		private static CommonEvent _instance;
		public  delegate void  Command();//命令事件
		public Command readyGame;//自己准备游戏
		public Command closeGamePanel;//关闭游戏页
		public Command disConnectNotice;//断线通知
		public Command DisplayBroadcast;// 
		public Command prizeCountChange;//抽奖次数改变

		//public Command refreshRoomCard;//刷新房卡数据
		void Start(){
			SocketEventHandle.getInstance ();
		}

		public static CommonEvent getInstance(){
			if (_instance == null) {
				GameObject temp = new GameObject ();
				_instance = temp.AddComponent<CommonEvent> ();
			}
			return _instance;
		}


		public CommonEvent ()
		{ 
			
		}
	}
}

