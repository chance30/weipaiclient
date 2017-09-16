using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class MyDebug
	{
		private static bool flag = true;

		public MyDebug ()
		{
		}

		public static void Log(object message){
			if (flag) {
				Debug.Log (message);
			}
		}

		public static void LogError(object message){
			if (flag) {
				Debug.LogError (message);
			}
		}
	}
}

