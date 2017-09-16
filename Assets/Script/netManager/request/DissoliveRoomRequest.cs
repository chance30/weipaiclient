using System;

namespace AssemblyCSharp
{
	public class DissoliveRoomRequest:ClientRequest
	{
		public DissoliveRoomRequest (string msg)
		{
			headCode = APIS.DISSOLIVE_ROOM_REQUEST;
			messageContent = msg;
		}
	}
}

