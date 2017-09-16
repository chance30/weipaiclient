using UnityEngine;
using System.Collections;
using DG.Tweening;
using AssemblyCSharp;

public class MessageBoxScript : MonoBehaviour {
	MyMahjongScript myMaj;
	// Use this for initialization
	void Start () {
		SocketEventHandle.getInstance ().messageBoxNotice += messageBoxNotice;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void btnClick(int index){
		SoundCtrl.getInstance ().playMessageBoxSound (index);
		CustomSocket.getInstance ().sendMsg (new MessageBoxRequest(index,GlobalDataScript.loginResponseData.account.uuid));
		if (myMaj == null) {
			myMaj = GameObject.Find ("Panel_GamePlay(Clone)").GetComponent<MyMahjongScript>();
		}
		if (myMaj != null) {
			myMaj.playerItems [0].showChatMessage (index);
		}
		hidePanel ();
	}

	public void showPanel(){
		gameObject.transform.DOLocalMove (new Vector3(472,113), 0.4f);
	}

	public void hidePanel(){
		gameObject.transform.DOLocalMove (new Vector3(472,567), 0.4f);
	}

	public void messageBoxNotice(ClientResponse response){
		string[] arr = response.message.Split (new char[1]{ '|' });
		int code = int.Parse(arr[0]);
		SoundCtrl.getInstance ().playMessageBoxSound (code);
	}

	public void Destroy(){
		SocketEventHandle.getInstance ().messageBoxNotice -= messageBoxNotice;
	}
}
