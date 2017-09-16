using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AssemblyCSharp
{
	public class finalOverItem :MonoBehaviour
	{
		public Text nickName;//昵称
		public Text ID;//id
		public Image icon;//头像
		public GameObject winer;//打赢家
		public GameObject paoshou;//点炮手
		public Text zimoCount;//自摸次数
		public Text jiepaoCount;//接炮次数
		public Text dianpaoCount;//点炮次数
		public Text angangCount;//暗杠次数
		public Text minggangCount;//明杠次数
		public Text finalScore;//总成绩
		public GameObject mainImgFlag;
		private string headIcon;
		private Texture2D texture2D;         //下载的图片
		public finalOverItem ()
		{
		}

		public void setUI(FinalGameEndItemVo itemData){
			string owerFlag = (itemData.getIsMain ()) ? "(房主)" : "";
			if (itemData.getIsMain ()) {
				mainImgFlag.SetActive (true);
			} else {
				mainImgFlag.SetActive (false);
			}
			nickName.text =itemData.getNickname ();
			ID.text ="ID:" +itemData.uuid + "";
			if (itemData.getIsWiner() && itemData.scores>0) {
				winer.SetActive (true);
			}
			if (itemData.getIsPaoshou() &&  itemData.dianpao>0) {
				paoshou.SetActive (true);
			}

			zimoCount.text = itemData.zimo + "";
			jiepaoCount.text = itemData.jiepao + "";
			dianpaoCount.text = itemData.dianpao + "";
			angangCount.text = itemData.angang + "";
			minggangCount.text = itemData.minggang + "";
			finalScore.text = itemData.scores + "";
			headIcon = itemData.getIcon ();
			StartCoroutine (LoadImg());

		}


		private IEnumerator LoadImg() { 
			//开始下载图片
			WWW www = new WWW(headIcon);
			yield return www;
			//下载完成，保存图片到路径filePath
			texture2D = www.texture;
			byte[] bytes = texture2D.EncodeToPNG();

			//将图片赋给场景上的Sprite
			Sprite tempSp = Sprite.Create(texture2D, new Rect(0,0,texture2D.width,texture2D.height),new Vector2(0,0));
			icon.sprite = tempSp;


		}

	}
}

