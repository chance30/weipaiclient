using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;
using LitJson;

public class DebugPanelScript : MonoBehaviour {
	public Text roomInputText;                                              
	public Text remarkText;
	public Text loginText;
    public Text cardPointInput;
    public Text CardPointInput2;
	// Use this for initialization
	private List<SocketForDebugPanel> socketList;                   
	private List<AvatarVO> avatarList;
	public List<int> userList;
	private Hashtable ava_socket; 
	void Start () {
		socketList = new List<SocketForDebugPanel> ();
		avatarList = new List<AvatarVO> ();
		ava_socket = new Hashtable ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loginNewUse(){
		if (socketList.Count >= 4) {
			remarkText.text = "已经有4个连接了";
		} else {
			SocketForDebugPanel tempsocket = new SocketForDebugPanel ();
			tempsocket.LoginCallBack_debug += loginCallBack;
			socketList.Add (tempsocket);
			tempsocket.sendMsg (new LoginRequest(null));
		}
	}

	public void jointRoom(){
		int RoomId = int.Parse( roomInputText.text);
		if (RoomId != 0) {
			RoomJoinVo roomJoinVo = new  RoomJoinVo ();
			roomJoinVo.roomId = RoomId;
			for (int i = 0; i < socketList.Count; i++) {
				socketList [i].sendMsg (new JoinRoomRequest (JsonMapper.ToJson (roomJoinVo)));
			}
		} else {
			remarkText.text = "请输入正确的房间号";
			roomInputText.text = "";
		}
	}

	public void loginCallBack(ClientResponse response){
		AvatarVO temp = JsonMapper.ToObject<AvatarVO> (response.message);
		avatarList.Add (temp);
		string str = "";
		for(int i = 0;i<avatarList.Count;i++){
			str += "用户名：" + avatarList [i].account.nickname + "\n";
		}
		loginText.text = str;
	}

	public void show(){
		gameObject.SetActive( true);
	}

	public void close(){
		gameObject.SetActive ( false);
	}

    public void checkOnClick()
    {
        int cardPoint = Int32.Parse( cardPointInput.text);
    }

    public void checkGangClick2()
    {
        int cardPoint = Int32.Parse(CardPointInput2.text);
    }

	public void StartRecord()  {
		MicroPhoneInput.getInstance ().StartRecord (userList);
	}

	public void StopRecord(){
		MicroPhoneInput.getInstance ().StopRecord ();
	}
}
