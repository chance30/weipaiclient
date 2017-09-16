using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class RoomJoinResponseVo
	{
		public bool addWordCard;
		public bool hong;
		public int ma;
		public string name;
		public int roomId;
		public int roomType;
		public int roundNumber;
		public bool sevenDouble;
		public int xiaYu;
		public int ziMo;
		public int magnification;
		public List<AvatarVO> playerList;
		//public LastOperationVo lastOperationVo;
		public RoomJoinResponseVo ()
		{
		}
	}
}

