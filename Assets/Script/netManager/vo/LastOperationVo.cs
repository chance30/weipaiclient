using System;

namespace AssemblyCSharp
{
	[Serializable]
	public class LastOperationVo
	{

		/** 1:碰  2 杠  3 吃   4 胡     5出牌   6 摸牌  断线进行的操作**/
		public int id;
		public object objectCC;//数据不需要处理
		public  int uuid;
		/**牌的点数**/
		public int cardIndex;
		public LastOperationVo ()
		{
			
		}
	}
}

