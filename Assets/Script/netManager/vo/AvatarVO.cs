using System;
/**
 * 登录接口返回数据封装
 * 
 */ 
namespace AssemblyCSharp
{
	public class AvatarVO
	{
		public Account account;

		//public int cardIndex; 
		public bool isOnLine;
		public bool isReady;
		public bool main;
		public int roomId;
		public int[] chupais;//出牌
		public int commonCards;//剩余牌数
		public int[][] paiArray;
		public HupaiResponseItem huReturnObjectVO;//胡牌才有数据，登录过程不管
		public  int scores;//分数
		public string IP;
		public AvatarVO ()
		{
		}

		public void resetData(){
		//	cardIndex = 0;
			isOnLine = false;
			isReady = false;
			main = false;
			roomId = 0;
		}
	}
}

