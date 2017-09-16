using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class GiftList
	{
		public List<GiftItemVo> data;
		public string type;

		public GiftList ()
		{
		}
	}
}

