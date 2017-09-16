using System;
using System.Collections;

namespace AssemblyCSharp
{
	public class CreateRoomRequest:ClientRequest
	{
		public CreateRoomRequest (string sendMsg)
		{
			headCode = APIS.CREATEROOM_REQUEST;
			messageContent = sendMsg;
		}

	}
}

