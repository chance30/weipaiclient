using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class NaiziHuScript
	{
		public NaiziHuScript ()
		{
		}

		/**
     	* 得到胡牌需要的赖子数
    	 * type 将在哪里
    	 * @return
     	*/
		public  bool isHu(List<List<int>> value){
			List<int> paiList = value [0];
			int zhong = 0;
			if (value [0] [31] == 0) {
				zhong = paiList [31];
			}
			int[] wan_arr = new int[9];
			int[] tiao_arr = new int[9];
			int[] tong_arr = new int[9];
			int needNum = 0;
			int index = 0;
			for(int i=0;i<27;i++){
				if(i<9){
					wan_arr[index] = paiList[i];
					index++;
				}
				if(i>=9 && i<18){
					if(i == 9){
						index = 0;
					}
					tiao_arr[index] = paiList[i];
					index++;
				}
				if(i>=18){
					if(i == 18){
						index = 0;
					}
					tong_arr[index] = paiList[i];
					index++;
				}
			}

			needNum = getNumWithJiang(wan_arr.Clone()) + getNumber(tiao_arr.Clone()) + getNumber(tong_arr.Clone());
			if(needNum <= zhong){
				return true;
			}
			else {
				needNum = getNumber(wan_arr.Clone()) + getNumWithJiang(tiao_arr.Clone()) + getNumber(tong_arr.Clone());
				if(needNum <= zhong){
					return true;
				}
				else{
					needNum = getNumber(wan_arr.Clone()) + getNumber(tiao_arr.Clone()) + getNumWithJiang(tong_arr.Clone());
					if(needNum <= zhong){
						return true;
					}
					else{
						return false;
					}
				}
			}
		}

		private static int getNumWithJiang(Object arr){
			bool isjiang = false;
			int result = 0;
			int[] temp_arr = arr as int[];
			for(int i=0;i<9;i++){
				if(temp_arr[i]== 3) {
					temp_arr[i] = 0;//先去除掉成坎的牌组
				}
				if(temp_arr[i] == 4){
					temp_arr[i] = 1;//这4张牌还在手中的情况
				}else if(temp_arr[i] > 4) {
					temp_arr[i] = 0;//该牌被扛掉了
				}
			}
			for(int i=0;i<9;i++){
				if(temp_arr[i]>0){
					if (i < 7) {
						if(temp_arr[i] >= 2 && isjiang == false && temp_arr[i+1] == 0){//先判断有将牌没有，如果没有将牌，先将这两张牌作将
							temp_arr[i] -= 2;
							isjiang = true;
							i--;
						}else {
							// 如果 这张牌的下一张和再下一张都不为空的情况。可以组成一级牌
							if (temp_arr[i + 1] > 0 && temp_arr[i + 2] > 0) {
								temp_arr[i]--;
								temp_arr[i + 1]--;
								temp_arr[i + 2]--;
								i--;
							} else if (temp_arr[i + 1] > 0 && temp_arr[i + 2] == 0) {
								//如果这张牌的下一张不为空，再下一张为空，需要一张赖子
								temp_arr[i]--;
								temp_arr[i + 1]--;
								result++;
								i--;
							} else if (temp_arr[i + 1] == 0 && temp_arr[i + 2] > 0) {
								//如果下一张为空，再下一张不为空，先判断有将没有，如果没有将，并且这张牌只有一张，补一张赖子组成将
								//                                                             如果这张牌有两张以上，直接做将，不补赖子。
								//                                                如果有将的情况，为中间补一张赖子。
								if (isjiang == false) {
									if (temp_arr[i] == 1) {
										temp_arr[i] = 0;
										isjiang = true;
										result++;
										i--;
									} else {
										if (temp_arr[i] >= 2) {
											isjiang = true;
											temp_arr[i] -= 2;
											i--;
										}
									}
								} else {
									temp_arr[i]--;
									temp_arr[i + 2]--;
									result++;
									i--;
								}
							} else if (temp_arr[i + 1] == 0 && temp_arr[i + 2] == 0) {
								//如果下一张和再下一张牌都为空的情况，先判断有将没得，如果有将了，为下一张牌补一张赖子
								//                                                    如果还没有将，并且这张牌只有一张，补一张赖子组成将
								//                                                                  如果这张牌有两张以上，直接做将，不补赖子。
								if (isjiang == true) {
									temp_arr[i + 1]++;
									result++;
									i--;
								} else {
									if (temp_arr[i] == 1) {
										temp_arr[i] = 0;
										isjiang = true;
										result++;
										i--;
									} else {
										if (temp_arr[i] >= 2) {
											isjiang = true;
											temp_arr[i] -= 2;
											i--;
										}
									}
								}
							}
						}
					}else{
						//牌面点子大得7的时候，先判断有没有将牌，如果没有，并且这张牌只有一张，补一张赖子组成将
						//                                                 如果这张牌有两张以上，直接做将，不补赖子。
						//                                      如果有将牌，则判断当前牌和下一张牌是否为空，如果都不为空，在前面补一张赖子
						//                                                  如果下一张牌为空，则将当前牌补成坎，缺多少张补多少张赖子
						if(i == 7){
							if(isjiang == false){
								if(temp_arr[i] == 1){
									temp_arr[i] = 0;
									result++;
									isjiang = true;
								}else if (temp_arr[i] >= 2){
									temp_arr[i] -=  2;
									isjiang = true;
									i--;
								}
							}else {
								if (temp_arr[i] > 0 && temp_arr[i + 1] > 0) {
									temp_arr[i]--;
									temp_arr[i+1]--;
									result++;
									i--;
								} else if (temp_arr[i] > 0 && temp_arr[i + 1] == 0) {
									result = result + 3 - temp_arr[i];
									temp_arr[i] = 0;
								}
							}
						}else{
							if(isjiang == false) {
								if(temp_arr[i] == 1){
									temp_arr[i] = 0;
									result++;
									isjiang = true;
								}else if (temp_arr[i] >= 2){
									temp_arr[i] -=  2;
									isjiang = true;
									i--;
								}
							}else{
								//如果当前牌为9点时，直接补成坎
								result = result + 3 - temp_arr[i];
								temp_arr[i] = 0;
							}
						}
					}
				}
			}

			if(isjiang == false){
				result += 2;
			}
			return  result;
		}

		private static int getNumber(Object arr){
			int result = 0;
			int[] temp_arr = arr as int[];
			for(int i=0;i<9;i++) {
				if(temp_arr[i] > 0) {
					if (i < 7) {
						if (temp_arr[i + 1] > 0 && temp_arr[i + 2] > 0) {
							temp_arr[i]--;
							temp_arr[i + 1]--;
							temp_arr[i + 2]--;
							i--;
						} else if (temp_arr[i + 1] > 0 && temp_arr[i + 2] == 0) {
							temp_arr[i]--;
							temp_arr[i + 1]--;
							result++;
							i--;
						} else if (temp_arr[i + 1] == 0 && temp_arr[i + 2] > 0) {
							temp_arr[i]--;
							temp_arr[i + 2]--;
							result++;
							i--;
						} else if (temp_arr[i + 1] == 0 && temp_arr[i + 2] == 0) {
							if (temp_arr[i] == 2) {
								temp_arr[i] = 0;
								result++;
							} else {
								temp_arr[i] = 0;
							}
						}
					} else {
						if(i == 7) {
							if (temp_arr[i] > 0 && temp_arr[i + 1] > 0) {
								temp_arr[i]--;
								temp_arr[i+1]--;
								result++;
								i--;
							} else if (temp_arr[i] > 0 && temp_arr[i + 1] == 0) {
								result = result + 3 - temp_arr[i];
								temp_arr[i] = 0;
							}
						}else{
							result  = result + 3-temp_arr[i];
							temp_arr[i] = 0;
						}
					}
				}
			}

			return result;
		}
	}
}

