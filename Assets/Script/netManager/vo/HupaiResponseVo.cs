using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class HupaiResponseVo
	{
		public string type;//结束类型0是正常结束，1流局 2是中途解散
		public string allMas;
		public string currentScore;
		public List<int> validMas;
		public List<HupaiResponseItem> avatarList;
		public HupaiResponseVo ()
		{
		}
	}
}

