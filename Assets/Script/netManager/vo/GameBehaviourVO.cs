using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class GameBehaviourVO
	{
		public int id;//	int(11)	主键	
		public int type;//	         char(1)	1出牌，2摸牌，3吃，4碰，5杠，6胡(自摸/点炮).......
		public string cardIndex;//	varchar(11)	进行type操作时的牌(当为吃的时候存cardIndex1,cardIndex2,cardIndex3)
		public int accountindex_id;//	int(11)	索引
		public int recordindex;//	int(11)	记录序号
		public long currentTime;//	datestamp	当前操作时间，存时间戳（long）	
		public int status;//	char(1)	0: 正常  1:删除/注销 	
		public int gangType;//杠的类型，1-别人点杠，2-自己暗杠，3-自己摸起来杠
		public string ma;
		public List<int> valideMa;
		public GameBehaviourVO ()
		{
		}
	}
}


