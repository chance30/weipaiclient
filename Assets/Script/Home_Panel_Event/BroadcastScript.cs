using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class BroadcastScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		addListener ();
	}


	
	// Update is called once per frame
	void Update () {
	
	}


	private void addListener(){
		SocketEventHandle.getInstance ().gameBroadcastNotice += gameBroadcastNotice;
	}

	private void removeListener(){
		SocketEventHandle.getInstance ().gameBroadcastNotice -= gameBroadcastNotice;
	}

	private void gameBroadcastNotice(ClientResponse response){
		string noticeString = response.message;
		string[] noticeList = noticeString.Split (new char[1]{ '*' });
	//	List<string> notices = new List<string> ();
		if (noticeList != null)
		{
			GlobalDataScript.noticeMegs = new List<string> ();
			for (int i=0 ;i<noticeList.Length ;i++){
				GlobalDataScript.noticeMegs .Add (noticeList[i]);
			}
			if (CommonEvent.getInstance ().DisplayBroadcast != null) {
				CommonEvent.getInstance ().DisplayBroadcast ();
			}
		}
	
	}
}
