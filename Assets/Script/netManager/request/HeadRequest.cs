using System;

namespace AssemblyCSharp
{
	public class HeadRequest : ClientRequest
	{
		public HeadRequest ()
		{
			headCode = APIS.head;
			messageContent = "";

		}
			
	}
}

