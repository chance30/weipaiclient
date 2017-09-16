using System;
using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

public class GlobalDataScript
{

    public static bool isDrag = false;
	/**登陆返回数据**/
	public static AvatarVO loginResponseData;
	/**加入房间返回数据**/
	public static RoomJoinResponseVo roomJoinResponseData;
	/**房间游戏规则信息**/
	public static RoomCreateVo roomVo=new RoomCreateVo(); 
	/**单局游戏结束服务器返回数据**/
	public static HupaiResponseVo hupaiResponseVo;
	/**全局游戏结束服务器返回数据**/
	public static FinalGameEndVo finalGameEndVo;

	public static int mainUuid;
	/**房间成员信息**/
	public static List<AvatarVO> roomAvatarVoList;

//	public static Dictionary<int, Account > palyerBaseInfo = new Dictionary<int, Account> (); 

	public static GameObject homePanel;//主界面
	public static GameObject gamePlayPanel;//游戏界面

	/**麻将剩余局数**/
	public static int surplusTimes ;
	/**总局数**/
	public static int totalTimes;
	/// <summary>
	/// 最顶层的容器
	/// </summary>
	public Transform canvsTransfrom;
	/**重新加入房间的数据**/
	public static RoomJoinResponseVo reEnterRoomData;

	public WechatOperateScript wechatOperate;
	/// <summary>
	/// 声音开关
	/// </summary>
	public static bool soundToggle = true;

	public static List<String> messageBoxContents = new List<string>();
	/// <summary>
	/// 单局结算面板
	/// </summary>
	public static List<GameObject> singalGameOverList = new List<GameObject>();


	public static bool isonLoginPage ;//是否在登陆页面

	//public SocketEventHandle socketEventHandle;
	/// <summary>
	/// 抽奖数据
	/// </summary>
	public static List<LotteryData> lotteryDatas;
	public static bool isonApplayExitRoomstatus = false;//是否处于申请解散房间状态
	public static bool isOverByPlayer = false;//是否由用用户选择退出而退出的游戏
	public static LoginVo loginVo;//登录数据
	public static List<String> noticeMegs = new List<string>();


	/**
	 * 重新初始化数据
	*/
	public static void reinitData(){
		isDrag = false;
		loginResponseData = null;
		roomJoinResponseData = null;
		roomVo=new RoomCreateVo(); 
		hupaiResponseVo = null;
		finalGameEndVo = null;
		roomAvatarVoList = null;
		surplusTimes = 0;
		totalTimes = 0;
		reEnterRoomData = null;
		singalGameOverList =   new List<GameObject>();
		lotteryDatas = null;
		isonApplayExitRoomstatus = false;
		isOverByPlayer = false;
		loginVo = null;
	}


	public void init(){
		//socketEventHandle = GameObject.Find ("Canvas").transform.GetComponent<SocketEventHandle> ();
		canvsTransfrom = GameObject.Find ("container").transform;
		TipsManagerScript.getInstance ().parent = GameObject.Find ("Canvas").transform;
		wechatOperate = GameObject.Find ("Canvas").GetComponent<WechatOperateScript>();
		initMessageBox ();
	}

	void initMessageBox(){
		messageBoxContents.Add ("不要吵了，专心玩游戏！");
		messageBoxContents.Add ("不要走，决战到天亮");
		messageBoxContents.Add ("大家好，很高兴见到各位");
		messageBoxContents.Add ("和你合作真是太愉快了");
		messageBoxContents.Add ("快点啊，等得你花儿都谢了!");
		messageBoxContents.Add ("你的牌打得也太好了");
		messageBoxContents.Add ("交个朋友吧");
		messageBoxContents.Add ("下次再玩吧，我要走了");
	}

	private static GlobalDataScript _instance;
	public static GlobalDataScript getInstance(){
		if (_instance == null) {
			_instance = new GlobalDataScript ();
		}
		return _instance;
	}

	public GlobalDataScript(){
		init ();
	}

	public string getIpAddress()
	{
		string tempip = "";
		try
		{
			WebRequest wr = WebRequest.Create("http://1212.ip138.com/ic.asp");
			Stream s = wr.GetResponse().GetResponseStream();
			StreamReader sr = new StreamReader(s, Encoding.Default);
			string all = sr.ReadToEnd(); //读取网站的数据

			int start = all.IndexOf("[")+1;
		    int end = all.IndexOf("]");
		    int count = end-start;
			tempip = all.Substring(start,count);
			sr.Close();
			s.Close();
		}
		catch
		{
		}
		return tempip;
	}



}
	
