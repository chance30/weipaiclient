using System;

namespace AssemblyCSharp
{
	public class GaveUpRequest : ClientRequest
	{
		public GaveUpRequest ()
		{
			headCode = APIS.GAVEUP_REQUEST;
			messageContent = "gaveup";
		}
	}
}

