using System;
using LitJson;
namespace AssemblyCSharp
{
	public class PutOutCardRequest:ClientRequest
	{
		public PutOutCardRequest (CardVO cardvo)
		{
			headCode = APIS.CHUPAI_REQUEST;
			messageContent = JsonMapper.ToJson (cardvo);;
		}
	}
}

