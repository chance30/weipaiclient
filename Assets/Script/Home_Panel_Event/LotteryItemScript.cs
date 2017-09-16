using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using DG.Tweening;
using UnityEngine.UI;

public class LotteryItemScript : MonoBehaviour
{
    public Image img;
	public Text nameTxt;
    public int Num=1;
	Texture2D texture2D;         //下载的图片
	private string imgUrl;
    // Use this for initialization
    void Start () {
	
	}

	private IEnumerator loadImg(){
		if (imgUrl != null && imgUrl != "") {
			WWW www = new WWW(APIS.PIC_PATH+ imgUrl);

			yield return www;
			//下载完成，保存图片到路径filePath
			try {
				texture2D = www.texture;
				byte[] bytes = texture2D.EncodeToPNG();
				//将图片赋给场景上的Sprite
				Sprite tempSp = Sprite.Create(texture2D, new Rect(0,0,texture2D.width,texture2D.height),new Vector2(0,0));
				img.sprite = tempSp;
				img.SetNativeSize();
			} catch (Exception e){

				MyDebug.Log ("LoadImg"+e.Message);
			}
		}
	}

	public void setPic(string imgUrlparm){
		imgUrl = imgUrlparm;
		StartCoroutine (loadImg());
	}




  
}
