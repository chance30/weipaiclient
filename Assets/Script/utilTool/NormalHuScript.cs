using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class NormalHuScript
	{

		private int JIANG;
		public NormalHuScript ()
		{
		}

		/**
     	* 普通胡牌算法
     	* @param paiList
     	* @return
     	*/
		public bool isHuPai(List<int> paiList) {

			if (Remain(paiList) == 0) {
				return true;           //   递归退出条件：如果没有剩牌，则胡牌返回。
			}
			for (int i = 0;  i < paiList.Count; i++) {//   找到有牌的地方，i就是当前牌,   PAI[i]是个数
			//   跟踪信息
			//   4张组合(杠子)
			if(paiList[i] != 0){
				if (paiList[i] == 4)                               //   如果当前牌数等于4张
				{
					paiList[i] = 0;                                     //   除开全部4张牌
					if (isHuPai(paiList)) {
						return true;             //   如果剩余的牌组合成功，和牌
					}
					paiList[i] = 4;                                     //   否则，取消4张组合
				}
				//   3张组合(大对)
				if (paiList[i] >= 3)                               //   如果当前牌不少于3张
				{
					paiList[i] -= 3;                                   //   减去3张牌
					if (isHuPai(paiList)) {
						return true;             //   如果剩余的牌组合成功，胡牌
					}
					paiList[i] += 3;                                   //   取消3张组合
				}
				//   2张组合(将牌)
				if (JIANG ==0 && paiList[i] >= 2)           //   如果之前没有将牌，且当前牌不少于2张
				{	
					JIANG = 1;                                       //   设置将牌标志
					paiList[i] -= 2;                                   //   减去2张牌
						if (isHuPai (paiList)) {
							return true;  
						}
						//   如果剩余的牌组合成功，胡牌
						paiList[i] += 2;                                   //   取消2张组合
						JIANG = 0;                                       //   清除将牌标志
					}
					if   ( i> 27){
						return   false;               //   “东南西北中发白”没有顺牌组合，不胡
					}
					//   顺牌组合，注意是从前往后组合！
					//   排除数值为8和9的牌
					if (i %9!=7 && i%9 != 8 && paiList[i+1]!=0 && paiList[i+2]!=0)             //   如果后面有连续两张牌
					{
						paiList[i]--;
						paiList[i + 1]--;
						paiList[i + 2]--;                                     //   各牌数减1
						if (isHuPai(paiList)) {
							return true;             //   如果剩余的牌组合成功，胡牌
						}
						paiList[i]++;
						paiList[i + 1]++;
						paiList[i + 2]++;                                     //   恢复各牌数
					}
				}

			}
			//   无法全部组合，不胡！
			return false;
		}

		//   检查剩余牌数
		private int Remain(List<int> paiList) {
			int sum = 0;
			for (int i = 0; i < paiList.Count; i++) {
				sum += paiList[i];
			}
			return sum;
		}

		/**
     	* 检查是否七小对胡牌
     	* @param paiList
    	 * @return 0-没有胡牌。1-普通七小对，2-龙七对
    	 */
		public int checkSevenDouble(List<int> paiList){
			int result = 1;
			for(int i=0;i<paiList.Count;i++){
				if(paiList[i] != 0){
					if(paiList[i] != 2 && paiList[i] != 4){
						return 0;
					}else{
						if(paiList[i] == 4){
							result = 2;
						}
					}
				}
			}
			return result;
		}

		public bool changsha(List<int> paiList){
			if (Remain(paiList) == 0) {
				return true;           //   递归退出条件：如果没有剩牌，则胡牌返回。
			}
			for (int i = 0;  i < paiList.Count; i++) {//   找到有牌的地方，i就是当前牌,   PAI[i]是个数
				//   跟踪信息
				//   4张组合(杠子)
				if(paiList[i] != 0){
					if (paiList[i] == 4)                               //   如果当前牌数等于4张
					{
						paiList[i] = 0;                                     //   除开全部4张牌
						if (changsha(paiList)) {
							return true;             //   如果剩余的牌组合成功，和牌
						}
						paiList[i] = 4;                                     //   否则，取消4张组合
					}
					//   3张组合(大对)
					if (paiList[i] >= 3)                               //   如果当前牌不少于3张
					{
						paiList[i] -= 3;                                   //   减去3张牌
						if (changsha(paiList)) {
							return true;             //   如果剩余的牌组合成功，胡牌
						}
						paiList[i] += 3;                                   //   取消3张组合
					}
					//   2张组合(将牌)
					if (JIANG ==0 && paiList[i] >= 2)           //   如果之前没有将牌，且当前牌不少于2张
					{	
						if (i == 1 || i == 4 || i == 7 || i == 10 || i == 13 || i == 16 || i == 19 || i == 22 || i == 25) {//必须258做将
							JIANG = 1;                                       //   设置将牌标志
							paiList[i] -= 2;                                   //   减去2张牌
							if (changsha (paiList)) {
								return true;  
							}
							//   如果剩余的牌组合成功，胡牌
							paiList[i] += 2;                                   //   取消2张组合
							JIANG = 0;                                       //   清除将牌标志
						}
					}
					if   ( i> 27){
						return   false;               //   “东南西北中发白”没有顺牌组合，不胡
					}
					//   顺牌组合，注意是从前往后组合！
					//   排除数值为8和9的牌
					if (i %9!=7 && i%9 != 8 && paiList[i+1]!=0 && paiList[i+2]!=0)             //   如果后面有连续两张牌
					{
						paiList[i]--;
						paiList[i + 1]--;
						paiList[i + 2]--;                                     //   各牌数减1
						if (changsha(paiList)) {
							return true;             //   如果剩余的牌组合成功，胡牌
						}
						paiList[i]++;
						paiList[i + 1]++;
						paiList[i + 2]++;                                     //   恢复各牌数
					}
				}

			}
			//   无法全部组合，不胡！
			return false;
		}
		/// <summary>
		/// 判断是否清一色
		/// </summary>
		/// <param name="paiList">Pai list.</param>
		public bool checkIsSingleType(List<int> paiList,int type){
			int startIndex = 0;
			int endIndex = 0;
			int start2Index = 0;
			int end2Index = 0;
			switch(type){
				case 1:
					startIndex = 0;
					endIndex = 9;
					start2Index = 9;
					end2Index = 17;
					break;
				case 2:
					startIndex = 0;
					endIndex = 9;
					start2Index = 17;
					end2Index = 26;
					break;
				case 3:
					startIndex = 9;
					endIndex = 17;
					start2Index = 17;
					end2Index = 26;
					break;
			}
			for (int i = startIndex; i < endIndex; i++) {
				if (paiList [i] > 0) {
					return false;
				}
			}

			for (int i = start2Index; i < end2Index; i++) {
				if (paiList [i] > 0) {
					return false;
				}
			}
			return true;
		}

		public bool test(List<int> paiList){
			int xiaohu = 0;//小胡数
			int dahu = 0;//大胡数
			int result = checkSevenDouble (paiList);
			if (result == 0) {
				//不是七小对
			} else {
				if (result == 1) {
					//七小对
					dahu ++;
				} else if (result == 2) {
					//龙对
					dahu += 2;
				}
				return true;
			}

			if (pengPengHu(paiList)) {
				//碰碰胡
			}
			//如果是清一色，则用乱将的胡牌算法
			if (checkIsSingleType (paiList, 1) || checkIsSingleType (paiList, 2) || checkIsSingleType (paiList, 3)) {
				return isHuPai (paiList);
			} else {
				
			}
			return false;
		}
		/// <summary>
		/// 碰碰胡
		/// </summary>
		/// <returns><c>true</c>, if pneg hu was penged, <c>false</c> otherwise.</returns>
		/// <param name="paiList">Pai list.</param>
		public bool pengPengHu(List<int> paiList){
			int two = 0;
			for (int i = 0; i < paiList.Count; i++) {
				if (paiList [i] == 1) {
					return false;
				}
				if (paiList [i] == 2) {
					two++;
					if (two >= 2) {
						return false;
					}
				}
			}

			return true;
		}
		/// <summary>
		/// 全求人
		/// </summary>
		/// <returns><c>true</c>, if qui ren was quaned, <c>false</c> otherwise.</returns>
		/// <param name="paiList">Pai list.</param>
		public bool quanQuiRen(List<List<int>> paiList){
			int two = 0;
			for (int i = 0; i < paiList [0].Count; i++) {
				if (paiList [0] [i] == 2 && paiList [1] [i] == 0) {
					two++;
					if(two >= 2){
						return false; 
					}
				}
			}

			return true;
		}
	}
}