using System;
using UnityEngine;

namespace cn.sharesdk.unity3d
{
	#if UNITY_ANDROID
	public class WxPayImpl
	{
		private AndroidJavaObject wxPaySdk;

		public WxPayImpl (GameObject go) 
		{
			Debug.Log("WxPayImpl  ===>>>  WxPayImpl" );

			try{
				

				wxPaySdk = new AndroidJavaObject("com.weipai.hu.WxPay", go.name, "_Callback");
				Debug.Log("wxPaySdk  ===>>>  "+wxPaySdk );
			} catch(Exception e) {

				Console.WriteLine("{0} Exception caught.", e);
			}
		}

		public void callTest (string msg) {
			if (wxPaySdk != null) {
				wxPaySdk.Call("wxpayTest", msg);
			}
		}
	}
	#endif
}

