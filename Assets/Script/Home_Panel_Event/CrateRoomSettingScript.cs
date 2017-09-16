using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using System;
using LitJson;
using UnityEngine.SceneManagement;


public class CrateRoomSettingScript : MonoBehaviour {
	
	public GameObject panelZhuanzhuanSetting;
	public GameObject panelChangshaSetting;
	public GameObject panelHuashuiSetting;
	public GameObject panelDevoloping;
    public GameObject knobzhuanzhuan;
    public GameObject knobhuashui;
	public List<Toggle> zhuanzhuanRoomCards;//转转麻将房卡数
	public List<Toggle> changshaRoomCards;//长沙麻将房卡数
	public List<Toggle> huashuiRoomCards;//划水麻将房卡数

	public List<Toggle> zhuanzhuanGameRule;//转转麻将玩法
	public List<Toggle> changshaGameRule;//长沙麻将玩法
	public List<Toggle> huashuiGameRule;//划水麻将玩法

	public List<Toggle> zhuanzhuanZhuama;//转转麻将抓码个数
	public List<Toggle> changshaZhuama;//长沙麻将抓码个数
	public List<Toggle> huashuixiayu;//划水麻将下鱼条数


	private int roomCardCount;//房卡数
	private GameObject gameSence;
	private RoomCreateVo sendVo;//创建房间的信息
	void Start () {
		panelZhuanzhuanSetting.SetActive (true);
		panelChangshaSetting.SetActive (false);
		panelHuashuiSetting.SetActive (false);
		panelDevoloping.SetActive (false);
        knobhuashui.SetActive(false);
		SocketEventHandle.getInstance ().CreateRoomCallBack += onCreateRoomCallback;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/***
	 * 打开转转麻将设置面板
	 */ 
	public void openZhuanzhuanSeetingPanel(){

		panelZhuanzhuanSetting.SetActive (true);
		panelChangshaSetting.SetActive (false);
		panelHuashuiSetting.SetActive (false);
		panelDevoloping.SetActive (false);
        knobzhuanzhuan.SetActive(true);
        knobhuashui.SetActive(false);
	
	}

	/***
	 * 打开长沙麻将设置面板
	 */ 
	public void openChangshaSeetingPanel(){
		panelZhuanzhuanSetting.SetActive (false);
		panelChangshaSetting.SetActive (false);
		panelHuashuiSetting.SetActive (false);
		panelDevoloping.SetActive (true);
	}

	/***
	 * 打开划水麻将设置面板
	 */ 
	public void openHuashuiSeetingPanel(){
		panelZhuanzhuanSetting.SetActive (false);
		panelChangshaSetting.SetActive (false);
		panelHuashuiSetting.SetActive (true);
		panelDevoloping.SetActive (false);
        knobzhuanzhuan.SetActive(false);
        knobhuashui.SetActive(true);
	}

	public void openDevloping(){
		panelZhuanzhuanSetting.SetActive (false);
		panelChangshaSetting.SetActive (false);
		panelHuashuiSetting.SetActive (false);
		panelDevoloping.SetActive (true);
	}

	public void closeDialog(){
		MyDebug.Log ("closeDialog");
		SocketEventHandle.getInstance ().CreateRoomCallBack -= onCreateRoomCallback;
		Destroy (this);
		Destroy (gameObject);
	}

	/**
	 * 创建转转麻将房间
	 */ 
	public void createZhuanzhuanRoom(){
		
		int roundNumber = 4;//房卡数量
		bool isZimo=false;//自摸
		bool hasHong=false;//红中赖子
		bool isSevenDoube =false;//七小对
		//bool isGang = false;
		int maCount = 0;
		for (int i = 0; i < zhuanzhuanRoomCards.Count; i++) {
			Toggle item = zhuanzhuanRoomCards [i];
			if (item.isOn) {
				if (i == 0) {
					roundNumber = 8;
				} else if (i == 1) {
					roundNumber = 16;
				} 
				break;
			}
		}

		if (zhuanzhuanGameRule [0].isOn) {
			isZimo = true;
		}

		//if (zhuanzhuanGameRule [1].isOn) {
		//	isGang = true;
		//}

		if (zhuanzhuanGameRule [2].isOn) {
			hasHong = true;
		}

		if (zhuanzhuanGameRule [3].isOn) {
			isSevenDoube = true;
		}


		for (int i = 0; i < zhuanzhuanZhuama.Count; i++) {
			if (zhuanzhuanZhuama [i].isOn) {
				maCount = 2 * (i + 1);
				break;
			}
		}
		sendVo = new RoomCreateVo ();
		sendVo.ma = maCount;
		sendVo.roundNumber = roundNumber;
		sendVo.ziMo = isZimo?1:0;
		sendVo.hong = hasHong;
		sendVo.sevenDouble = isSevenDoube;
		sendVo.roomType = GameConfig.GAME_TYPE_ZHUANZHUAN;
		string sendmsgstr = JsonMapper.ToJson (sendVo);
		if (GlobalDataScript.loginResponseData.account.roomcard > 0) {
			CustomSocket.getInstance ().sendMsg (new CreateRoomRequest (sendmsgstr));
		} else {
			TipsManagerScript.getInstance ().setTips ("你的房卡数量不足，不能创建房间");
		}


	}

	/**
	 * 创建长沙麻将房间
	 */
	public void createChangshaRoom(){
		int roundNumber = 4;//房卡数量
		bool isZimo=false;//自摸
		int maCount = 0;
		for (int i = 0; i < changshaRoomCards.Count; i++) {
			Toggle item = changshaRoomCards [i];
			if (item.isOn) {
				if (i == 0) {
					roundNumber = 8;
				} else if (i == 1) {
					roundNumber = 16;
				} 			
				break;
			}
		}
		/*
		if (changshaGameRule [0].isOn) {
			isZimo = true;
		}
		*/
	//	isZimo = true;
		for (int i = 0; i <changshaZhuama.Count; i++) {
			if (changshaZhuama [i].isOn) {
				maCount = (int)Math.Pow(2,i);
				break;
			}
		}

		sendVo = new RoomCreateVo ();
		sendVo.magnification = maCount;
		sendVo.roundNumber = roundNumber;
		//sendVo.ziMo = isZimo?1:0;
		sendVo.roomType = GameConfig.GAME_TYPE_CHANGSHA;
		string sendmsgstr = JsonMapper.ToJson (sendVo);
		if (GlobalDataScript.loginResponseData.account.roomcard > 0) {
			CustomSocket.getInstance ().sendMsg (new CreateRoomRequest (sendmsgstr));
		} else {
			TipsManagerScript.getInstance ().setTips ("你的房卡数量不足，不能创建房间");
		}

	}

	/**
	 * 创建划水麻将房间
	 */
	public void createHuashuiRoom(){
		int roundNumber = 4;//房卡数量
		bool isZimo=false;//自摸
		bool isFengpai =false;//七小对
		int maCount = 0;
		for (int i = 0; i < huashuiRoomCards.Count; i++) {
			Toggle item = huashuiRoomCards [i];
			if (item.isOn) {
				if (i == 0) {
					roundNumber = 8;
				} else if (i == 1) {
					roundNumber = 16;
				} 
				break;
			}
		}
		if (huashuiGameRule [0].isOn) {
			isFengpai = true;
		}
		if (huashuiGameRule [1].isOn) {
			isZimo = true;
		}
	

		for (int i = 0; i <huashuixiayu.Count; i++) {
			if (huashuixiayu [i].isOn) {
				maCount = 2 * (i + 1);
				break;
			}
		}

		sendVo = new RoomCreateVo ();
		sendVo.xiaYu = maCount;
		sendVo.roundNumber = roundNumber;
		sendVo.ziMo = isZimo?1:0;
		sendVo.roomType = GameConfig.GAME_TYPE_HUASHUI;
		sendVo.addWordCard = isFengpai;
		sendVo.sevenDouble = true;
		string sendmsgstr = JsonMapper.ToJson (sendVo);
		if (GlobalDataScript.loginResponseData.account.roomcard > 0) {
			CustomSocket.getInstance ().sendMsg (new CreateRoomRequest (sendmsgstr));
		} else {
			TipsManagerScript.getInstance ().setTips ("你的房卡数量不足，不能创建房间");
		}

	}

//	public void toggleHongClick(){
//
//		if (zhuanzhuanGameRule [2].isOn) {
//			zhuanzhuanGameRule [0].isOn = true;
//		}
//	}
//
//	public void toggleQiangGangHuClick(){
//		if (zhuanzhuanGameRule [1].isOn) {
//			zhuanzhuanGameRule [2].isOn = false;
//		}
//	}

	public void onCreateRoomCallback(ClientResponse response){
		MyDebug.Log (response.message);
		if (response.status == 1) {
			
			//RoomCreateResponseVo responseVO = JsonMapper.ToObject<RoomCreateResponseVo> (response.message);
			int roomid = Int32.Parse(response.message);
			sendVo.roomId = roomid;
			GlobalDataScript.roomVo = sendVo;
			GlobalDataScript.loginResponseData.roomId = roomid;
			//GlobalDataScript.loginResponseData.isReady = true;
			GlobalDataScript.loginResponseData.main = true;
			GlobalDataScript.loginResponseData.isOnLine = true;

			//SceneManager.LoadSceneAsync(1);
			/**
			if (gameSence == null) {
				gameSence = Instantiate (Resources.Load ("Prefab/Panel_GamePlay")) as GameObject;
				gameSence.transform.parent = GlobalDataScript.getInstance ().canvsTransfrom;
				gameSence.transform.localScale = Vector3.one;
				gameSence.GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
				gameSence.GetComponent<RectTransform> ().offsetMin = new Vector2 (0f, 0f);
				gameSence.GetComponent<MyMahjongScript> ().createRoomAddAvatarVO (GlobalDataScript.loginResponseData);
			}*/
			GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GamePlay");

			GlobalDataScript.gamePlayPanel.GetComponent<MyMahjongScript> ().createRoomAddAvatarVO (GlobalDataScript.loginResponseData);
		
			closeDialog ();

		} else {
			TipsManagerScript.getInstance ().setTips (response.message);
		}
	}

}
