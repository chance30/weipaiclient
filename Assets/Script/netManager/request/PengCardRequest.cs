using System;
using LitJson;
namespace AssemblyCSharp
{
	public class PengCardRequest : ClientRequest
	{
		public PengCardRequest (CardVO cardvo)
		{
			headCode = APIS.PENGPAI_REQUEST;
			messageContent = JsonMapper.ToJson (cardvo);;
		}
	}
}

