using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_MicPhoneScript : MonoBehaviour
{
    public float WholeTime=10f;
    public GameObject InputGameObject;
    private Boolean btnDown = false;
    public GameObject circle;
	public MyMahjongScript myScript;
	void Start ()
	{
	}
		
    // Update is called once per frame
    void Update () {
	   
	}

	void FixedUpdate(){
		if (btnDown)
		{
			WholeTime -= Time.deltaTime;
			circle.GetComponent<Slider>().value = WholeTime;
			if (WholeTime <= 0)
			{
				OnPointerUp ();
			}
		}
	}

    public void OnPointerDown()
    {
        
		if (myScript.avatarList != null && myScript.avatarList.Count >1) {
			btnDown = true;
			InputGameObject.SetActive(true);
			MicroPhoneInput.getInstance ().StartRecord (getUserList ());
		}else{
			TipsManagerScript.getInstance ().setTips ("房间里只有你一个人，不能发送语音");
		}
    }

    public void OnPointerUp()
    {
		if (btnDown) {
			btnDown = false;
			InputGameObject.SetActive (false);
			WholeTime = 10;
			if (myScript.avatarList != null && myScript.avatarList.Count > 1) {
				MicroPhoneInput.getInstance ().StopRecord ();
				myScript.myselfSoundActionPlay ();
			} else {
				
			}
		}
    }

	private List<int> getUserList(){
		List<int> userList = new List<int> ();
		for(int i=0;i<myScript.avatarList.Count;i++){
			if (myScript.avatarList [i].account.uuid != GlobalDataScript.loginResponseData.account.uuid) {
				userList.Add (myScript.avatarList[i].account.uuid);
			}
		}
		return userList;
	}
}
