using System;

namespace AssemblyCSharp
{
	public class LoginChatRequest:ChatRequest
	{
		public LoginChatRequest (int userId)
		{
			headCode = APIS.LoginChat_Request;
			userList = new System.Collections.Generic.List<int> ();
			userList.Add (userId);
		}
	}
}

