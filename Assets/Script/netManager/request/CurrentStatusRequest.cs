using System;

namespace AssemblyCSharp
{
	public class CurrentStatusRequest:ClientRequest
	{
		public CurrentStatusRequest ()
		{
			headCode = APIS.REQUEST_CURRENT_DATA;
			messageContent = "dd";
		}
	}
}

