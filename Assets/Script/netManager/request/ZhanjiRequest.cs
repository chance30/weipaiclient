using System;

namespace AssemblyCSharp
{
	public class ZhanjiRequest :ClientRequest
	{
		public ZhanjiRequest (string Msg)
		{
			headCode = APIS.ZHANJI_REPOTER_REQUEST;
			messageContent=Msg;
		}
	}
}

