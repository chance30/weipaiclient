using UnityEngine;
using System.Collections;
using System;

namespace cn.sharesdk.unity3d 
{
	[Serializable]
	public class DevInfoSet

	{
		//public TencentWeiboDevInfo tencentweibo;
		//public QQ qq;
	//	public QZone qzone;
		public WeChat wechat;
		public WeChatMoments wechatMoments; 
		public WeChatFavorites wechatFavorites;

		#if UNITY_ANDROID
		#elif UNITY_IPHONE		
	//	public Copy copy;
		//public YixinFavorites yixinFavorites;					//易信收藏，仅iOS端支持							[仅支持iOS端]
		//public YixinSeries yixinSeries;							//iOS端易信系列, 可直接配置易信三个子平台			[仅支持iOS端]
		//public WechatSeries wechatSeries;						//iOS端微信系列, 可直接配置微信三个子平台 		[仅支持iOS端]
		//public QQSeries qqSeries;								//iOS端QQ系列,  可直接配置QQ系列两个子平台		[仅支持iOS端]
		//public KakaoSeries kakaoSeries;							//iOS端Kakao系列, 可直接配置Kakao系列两个子平台	[仅支持iOS端]
		//public EvernoteInternational evernoteInternational;		//iOS配置印象笔记国内版在Evernote中配置;国际版在EvernoteInternational中配置												 
		//安卓配置印象笔记国内与国际版直接在Evernote中配置														
		#endif

	}

	public class DevInfo 
	{	
		public bool Enable = true;
	}
		
	/**
	[Serializable]
	public class TencentWeiboDevInfo : DevInfo 
	{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.TencentWeibo;
		public string SortId = "3";
		public string AppKey = "801307650";
		public string AppSecret = "ae36f4ee3946e1cbb98d6965b0b2ff5c";
		public string RedirectUri = "http://sharesdk.cn";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.TencentWeibo;
		public string app_key = "801307650";
		public string app_secret = "ae36f4ee3946e1cbb98d6965b0b2ff5c";
		public string redirect_uri = "http://sharesdk.cn";
		#endif
	}

	[Serializable]
	public class QQ : DevInfo 
	{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.QQ;
		public string SortId = "2";
		public string AppId = "100371282";
		public string AppKey = "aed9b0303e3ed1e27bae87c33761161d";
		public bool ShareByAppClient = true;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QQ;
		public string app_id = "100371282";
		public string app_key = "aed9b0303e3ed1e27bae87c33761161d";
		public string auth_type = "both";  //can pass "both","sso",or "web" 
		#endif
	}

	[Serializable]
	public class QZone : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "1";
		public const int type = (int) PlatformType.QZone;
		public string AppId = "100371282";
		public string AppKey = "ae36f4ee3946e1cbb98d6965b0b2ff5c";
		public bool ShareByAppClient = true;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QZone;
		public string app_id = "100371282";
		public string app_key = "aed9b0303e3ed1e27bae87c33761161d";
		public string auth_type = "both";  //can pass "both","sso",or "web" 
		#endif
	}
	
**/
	
	[Serializable]
	public class WeChat : DevInfo 
	{	
		#if UNITY_ANDROID
		public string SortId = "5";
		public const int type = (int) PlatformType.WeChat;
		public string AppId = "wx682ecf0cea34ac77";
		public string AppSecret = "9cf2ef1d2d09ebc042f723cdb281a3c2";
		public bool BypassApproval = true;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChat;
		public string app_id = "wx682ecf0cea34ac77";
		public string app_secret = "9cf2ef1d2d09ebc042f723cdb281a3c2";
		#endif
	}

	[Serializable]
	public class WeChatMoments : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "6";
		public const int type = (int) PlatformType.WeChatMoments;
		public string AppId = "wx682ecf0cea34ac77";
		public string AppSecret = "9cf2ef1d2d09ebc042f723cdb281a3c2";
		public bool BypassApproval = false;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatMoments;
		public string app_id = "wx682ecf0cea34ac77";
		public string app_secret = "9cf2ef1d2d09ebc042f723cdb281a3c2";
		#endif
	}

	[Serializable]
	public class WeChatFavorites : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "7";
		public const int type = (int) PlatformType.WeChatFavorites;
		public string AppId = "wx682ecf0cea34ac77";
		public string AppSecret = "9cf2ef1d2d09ebc042f723cdb281a3c2";
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatFavorites;
		public string app_id = "wx682ecf0cea34ac77";
		public string app_secret = "9cf2ef1d2d09ebc042f723cdb281a3c2";
		#endif
	}
		


}
