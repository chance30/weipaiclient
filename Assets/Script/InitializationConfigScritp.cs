using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;


public class InitializationConfigScritp : MonoBehaviour {
	
	int num = 0;
	bool hasPaused   = false;
	void Start () {
		
		MicroPhoneInput.getInstance ();
		GlobalDataScript.getInstance ();
		//CustomSocket.getInstance().Connect();
		//ChatSocket.getInstance ();
		TipsManagerScript.getInstance ().parent = gameObject.transform;
		SoundCtrl.getInstance ();

		//UpdateScript update = new UpdateScript ();
		//StartCoroutine (update.updateCheck ());
		ServiceErrorListener seriveError = new ServiceErrorListener();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		//heartbeatTimer ();
		heartbeatThread();
	}

   void	Awake(){
		SocketEventHandle.getInstance().disConnetNotice += disConnetNotice;
		SocketEventHandle.getInstance ().hostUpdateDrawResponse += hostUpdateDrawResponse;
		SocketEventHandle.getInstance ().otherTeleLogin += otherTeleLogin;
	}




	private void  disConnetNotice(){
		if (GlobalDataScript.isonLoginPage) {
			//CustomSocket.getInstance ().Connect();
			//ChatSocket.getInstance ().Connect ();
		} else {
			cleaListener ();
			PrefabManage.loadPerfab ("Prefab/Panel_Start");

		}
	}

	private void otherTeleLogin(ClientResponse response){
	//	TipsManagerScript.getInstance ().setTips ("你的账号在其他设备登录");
		disConnetNotice ();
	}



	private void cleaListener(){
		/*
		if (SocketEventHandle.getInstance ().LoginCallBack != null) {
			SocketEventHandle.getInstance ().LoginCallBack = null;
		}
*/
		if (SocketEventHandle.getInstance ().CreateRoomCallBack != null) {
			SocketEventHandle.getInstance ().CreateRoomCallBack = null;
		}

		if (SocketEventHandle.getInstance ().JoinRoomCallBack != null) {
			SocketEventHandle.getInstance ().JoinRoomCallBack = null;
		}

		if (SocketEventHandle.getInstance ().StartGameNotice != null) {
			SocketEventHandle.getInstance ().StartGameNotice = null;
		}

		if (SocketEventHandle.getInstance ().pickCardCallBack != null) {
			SocketEventHandle.getInstance ().pickCardCallBack = null;
		}

		if (SocketEventHandle.getInstance ().otherPickCardCallBack != null) {
			SocketEventHandle.getInstance ().otherPickCardCallBack = null;
		}

		if (SocketEventHandle.getInstance ().putOutCardCallBack != null) {
			SocketEventHandle.getInstance ().putOutCardCallBack = null;
		}

		if (SocketEventHandle.getInstance ().PengCardCallBack != null) {
			SocketEventHandle.getInstance ().PengCardCallBack = null;
		}

		if (SocketEventHandle.getInstance ().GangCardCallBack != null) {
			SocketEventHandle.getInstance ().GangCardCallBack = null;
		}

		if (SocketEventHandle.getInstance ().HupaiCallBack != null) {
			SocketEventHandle.getInstance ().HupaiCallBack = null;
		}

	
		if (SocketEventHandle.getInstance ().gangCardNotice != null) {
			SocketEventHandle.getInstance ().gangCardNotice = null;
		}



		if (SocketEventHandle.getInstance ().btnActionShow != null) {
			SocketEventHandle.getInstance ().btnActionShow = null;
		}

		if (SocketEventHandle.getInstance ().outRoomCallback != null) {
			SocketEventHandle.getInstance ().outRoomCallback = null;
		}

		if (SocketEventHandle.getInstance ().dissoliveRoomResponse != null) {
			SocketEventHandle.getInstance ().dissoliveRoomResponse = null;
		}

		if (SocketEventHandle.getInstance ().gameReadyNotice != null) {
			SocketEventHandle.getInstance ().gameReadyNotice = null;
		}

	

		if (SocketEventHandle.getInstance ().messageBoxNotice != null) {
			SocketEventHandle.getInstance ().messageBoxNotice = null;
		}



		if (SocketEventHandle.getInstance ().backLoginNotice != null) {
			SocketEventHandle.getInstance ().backLoginNotice = null;
		}
		/*
		if (SocketEventHandle.getInstance ().RoomBackResponse != null) {
			SocketEventHandle.getInstance ().RoomBackResponse = null;
		}
		*/

		if (SocketEventHandle.getInstance ().cardChangeNotice != null) {
			SocketEventHandle.getInstance ().cardChangeNotice = null;
		}



		if (SocketEventHandle.getInstance ().offlineNotice != null) {
			SocketEventHandle.getInstance ().offlineNotice = null;
		}

		if (SocketEventHandle.getInstance ().onlineNotice != null) {
			SocketEventHandle.getInstance ().onlineNotice = null;
		}

		if (SocketEventHandle.getInstance ().giftResponse != null) {
			SocketEventHandle.getInstance ().giftResponse = null;
		}

		if (SocketEventHandle.getInstance ().returnGameResponse != null) {
			SocketEventHandle.getInstance ().returnGameResponse = null;
		}

		if (SocketEventHandle.getInstance ().gameFollowBanderNotice != null) {
			SocketEventHandle.getInstance ().gameFollowBanderNotice = null;
		}

		if (SocketEventHandle.getInstance ().contactInfoResponse != null) {
			SocketEventHandle.getInstance ().contactInfoResponse = null;
		}

		if (SocketEventHandle.getInstance ().zhanjiResponse != null) {
			SocketEventHandle.getInstance ().zhanjiResponse = null;
		}



		if (SocketEventHandle.getInstance ().zhanjiDetailResponse != null) {
			SocketEventHandle.getInstance ().zhanjiDetailResponse = null;
		}

		if (SocketEventHandle.getInstance ().gameBackPlayResponse != null) {
			SocketEventHandle.getInstance ().gameBackPlayResponse = null;
		}

	}

	void FixedUpdate(){
		/**
		num++;
		if (num == 150) {
			num = 0;

			CustomSocket.getInstance ().sendHeadData ();
		}
		**/
	}

	System.Timers.Timer t;
	private  void heartbeatTimer(){
		t = new System.Timers.Timer(1000);   //实例化Timer类，设置间隔时间为10000毫秒；   
		t.Elapsed += new System.Timers.ElapsedEventHandler(doSendHeartbeat); //到达时间的时候执行事件；   
		t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
		t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   

	}

	private void heartbeatThread(){
		Thread thread = new Thread (sendHeartbeat);
		thread.IsBackground = true;
		thread.Start();
	}


	private static void sendHeartbeat(){
		CustomSocket.getInstance ().sendHeadData ();
		Thread.Sleep (20000);
		sendHeartbeat ();
	}

	public  void doSendHeartbeat( object source, System.Timers.ElapsedEventArgs e){
		CustomSocket.getInstance ().sendHeadData ();
		/*
		bool flag = 
		if (!flag) {
			if (t != null) {
				t.Stop ();
			}
		}
		*/
	}


	private void hostUpdateDrawResponse(ClientResponse response){
		int giftTimes =int.Parse(response.message);
		GlobalDataScript.loginResponseData.account.prizecount = giftTimes;
		if(CommonEvent.getInstance().prizeCountChange !=null){
			CommonEvent.getInstance ().prizeCountChange();
		}
	}

}
