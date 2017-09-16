using System;

namespace AssemblyCSharp
{
	public class GetGiftRequest:ClientRequest
	{
		public GetGiftRequest (string msg)
		{
			headCode = APIS.GET_PRIZE;
			messageContent = msg;
		}
	}
}

