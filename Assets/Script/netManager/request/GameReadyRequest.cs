using System;

namespace AssemblyCSharp
{
	public class GameReadyRequest:ClientRequest
	{
		public GameReadyRequest ()
		{
			headCode = APIS.PrepareGame_MSG_REQUEST;
			messageContent = "ss";
		}
	}
}

