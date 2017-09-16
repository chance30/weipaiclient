using System;
using LitJson;
namespace AssemblyCSharp
{
	public class GangCardRequest : ClientRequest
	{
		public GangCardRequest (int cardPoint,int gangType)
		{
			headCode = APIS.GANGPAI_REQUEST;
            GangRequestVO vo = new GangRequestVO();
		    vo.cardPoint = cardPoint;
		    vo.gangType = gangType;
		    messageContent = JsonMapper.ToJson(vo);
		}
	}
}

