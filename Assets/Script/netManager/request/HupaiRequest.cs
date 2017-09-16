using System;

namespace AssemblyCSharp
{
	public class HupaiRequest:ClientRequest
	{
		
		public HupaiRequest (string sendMsg)
		{
			headCode = APIS.HUPAI_REQUEST;
			messageContent = sendMsg;
		}
	}
}

