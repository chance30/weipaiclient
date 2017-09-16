using System;

namespace AssemblyCSharp
{
	public class JoinRoomRequest : ClientRequest
	{
		public JoinRoomRequest (string sendMsg)
		{
			headCode = APIS.JOIN_ROOM_REQUEST;
			messageContent = sendMsg;
		}
	}
}

