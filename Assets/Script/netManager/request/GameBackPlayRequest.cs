using System;

namespace AssemblyCSharp
{
	public class GameBackPlayRequest : ClientRequest
	{
		
		public GameBackPlayRequest (string id)
		{
			headCode = APIS.GAME_BACK_PLAY_REQUEST;
			messageContent = id;
		}
	}
}

