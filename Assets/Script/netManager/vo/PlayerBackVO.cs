using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class PlayerBackVO
	{
		public int id;
		public string accountName;//		玩家名
		public int accountIndex;//	int(1)	索引(游戏时对应的索引)	
		public string cardList;//	初始牌组成的字符串
		public string headIcon;//
		public int gameRound;
		public int socre;
		public int sex;//性别
		public string ma;
		public int uuid;
		public int missType;
		private int[] pai;
		public PlayerBackVO ()
		{
		}
		/// <summary>
		/// Gets the pai array.
		/// </summary>
		/// <returns>The pai array.</returns>
		public int[] getPaiArray(){
			if (pai == null) {
				string[] temp = cardList.Split(new char[1]{','});
				pai = new int[temp.Length];
				for (int i = 0; i < temp.Length; i++) {
					pai [i] = Int32.Parse (temp[i]);
				}
			}
			return pai;
		}

		public void setPaiArray(int[] value){
			pai = value;
		}
	}
}

