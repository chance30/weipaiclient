using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using System;


/**申请解散房间投票框**/

public class VoteScript : MonoBehaviour {
	public Text sponsorNameText;
	public List<PlayerResult> playerList; 
	public Button okButton;
	public Button cancleButton;
	public Text timerText;
//	private List<int> uuids; 
	private string sponsorName;
	private int disagreeCount = 0;
	private string dissolveType;
	private List<string> playerNames;
	private float timer = GameConfig.GAME_DEFALUT_AGREE_TIME;
	private bool isStop = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (timer != 0) {

			timer -= Time.deltaTime;
			if (timer < 0)
			{
				timer = 0;
				clickOk ();
				isStop = true;
				//UpateTimeReStart();
			}
			timerText.text = Math.Floor(timer) + "";
		}



	}

	private void addListener(){
		
		SocketEventHandle.getInstance().dissoliveRoomResponse += dissoliveRoomResponse;

	}
	public void removeListener(){
		SocketEventHandle.getInstance().dissoliveRoomResponse -= dissoliveRoomResponse;
	}

	public  void iniUI( string sponsor,List<AvatarVO> avatarVOList){
		
		if (GlobalDataScript.loginResponseData.account.nickname == sponsor) {
			okButton.transform.gameObject.SetActive (false);
			cancleButton.transform.gameObject.SetActive (false);
		}

		sponsorName = sponsor;
		playerNames = new List<string> ();
		sponsorNameText.text = sponsorName;
		addListener ();
		for (int i = 0; i <avatarVOList.Count; i++) {
			string name = avatarVOList [i].account.nickname;
		
			if (name != sponsorName) {
				playerNames.Add (name);
			}

		}

		for (int i = 0; i < playerNames.Count; i++) {
			playerList [i].setInitVal (playerNames [i], "正在选择");
		}

	
	}

	private int getPlayerIndex(string name){
		for (int i = 0; i < playerNames.Count; i++) {
			if (name == playerNames[i]) {
				return i;
			}
		}	
		return 0;
	}

	private void dissoliveRoomResponse(ClientResponse response){
		DissoliveRoomResponseVo dissoliveRoomResponseVo = JsonMapper.ToObject<DissoliveRoomResponseVo> (response.message);
		string plyerName = dissoliveRoomResponseVo.accountName;
		if (dissoliveRoomResponseVo.type == "1") {
			playerList [getPlayerIndex (plyerName)].changeResult ("同意");
		} else if (dissoliveRoomResponseVo.type == "2") {
			GlobalDataScript.isonApplayExitRoomstatus = false;
			playerList [getPlayerIndex (plyerName)].changeResult ("拒绝");
			disagreeCount += 1;
			if (disagreeCount >= 2) {
				TipsManagerScript.getInstance ().setTips ("同意解散房间申请人数不够，本轮投票结束，继续游戏");
				removeListener ();
				Destroy (this);
				Destroy (gameObject);
			}
		} 
	}

	private void exitGamePlay(){
	//	CommonEvent.getInstance ().closeGamePanel ();
		removeListener ();
		Destroy (this);
		Destroy (gameObject);
	}

	private void  doDissoliveRoomRequest(){
		DissoliveRoomRequestVo dissoliveRoomRequestVo = new DissoliveRoomRequestVo ();
		dissoliveRoomRequestVo.roomId = GlobalDataScript.loginResponseData.roomId;
		dissoliveRoomRequestVo.type = dissolveType;
		string sendMsg = JsonMapper.ToJson (dissoliveRoomRequestVo);
		CustomSocket.getInstance().sendMsg(new DissoliveRoomRequest(sendMsg));

	}
	public void  clickOk(){
		dissolveType = "1";
		okButton.transform.gameObject.SetActive (false);
		cancleButton.transform.gameObject.SetActive (false);
		doDissoliveRoomRequest ();


	}

	public void clickCancle(){
		dissolveType = "2";
		doDissoliveRoomRequest ();
		okButton.transform.gameObject.SetActive (false);
		cancleButton.transform.gameObject.SetActive (false);
	}

}
