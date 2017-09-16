using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System;


public class ShowUserInfoScript : MonoBehaviour {
	public Image headIcon;
	public Text Ip;
	public Text ID;
	public Text name;

	private Texture2D texture2D;
	private string headIconPath;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setUIData(AvatarVO  userInfo){
		headIconPath = userInfo.account.headicon;
		Ip.text = "IP:"+ userInfo.IP;
		ID.text = "ID:"+  userInfo.account.uuid +"";
		name.text ="昵称:"+ userInfo.account.nickname;
		StartCoroutine (LoadImg());

	}



	private IEnumerator LoadImg() { 
		//开始下载图片
		if (headIconPath != null && headIconPath != "") {
			WWW www = new WWW(headIconPath);
			yield return www;
			//下载完成，保存图片到路径filePath
			try {
				texture2D = www.texture;
				byte[] bytes = texture2D.EncodeToPNG();
				//将图片赋给场景上的Sprite
				Sprite tempSp = Sprite.Create(texture2D, new Rect(0,0,texture2D.width,texture2D.height),new Vector2(0,0));
				headIcon.sprite = tempSp;

			} catch (Exception e){

				MyDebug.Log ("LoadImg"+e.Message);
			}
		}
	}

	public void closeUserInfoPanel(){
		Destroy (this);
		Destroy (gameObject);
	}
}
