using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class MicInputRequest:ChatRequest
	{
		public MicInputRequest (List<int> _userList, byte[] sound)
		{
			headCode = APIS.MicInput_Request;
			myUUid = GlobalDataScript.loginResponseData.account.uuid;
			userList = _userList;
			ChatSound = sound;
		}
	}
}

