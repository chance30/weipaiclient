using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class PlayerItemBackScript : MonoBehaviour {
	public Image headerIcon;
	public Image bankerImg;
	public Text nameText;
	public Image readyImg;
	public Text scoreText;
	public string dir;
	public GameObject HuFlag;
	public GameObject pengEffect;
	public GameObject gangEffect;
	public GameObject huEffect;
	// Use this for initialization
	private PlayerBackVO avatarvo;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setAvatarVo(PlayerBackVO value){
		if (value != null) {
			avatarvo = value;
			nameText.text = avatarvo.accountName;
			scoreText.text = avatarvo.socre+"";
			StartCoroutine (LoadImg());

		} else {
			nameText.text = "";
			readyImg.enabled = false;
			bankerImg.enabled = false;
			headerIcon.sprite = Resources.Load("Image/morentouxiang",typeof(Sprite)) as Sprite;
		}
	}

	/// <summary>
	/// 加载头像
	/// </summary>
	/// <returns>The image.</returns>
	private IEnumerator LoadImg() { 
		//开始下载图片
		WWW www = new WWW(avatarvo.headIcon);
		yield return www;
		if (www != null) {
			Texture2D texture2D = www.texture;
			byte[] bytes = texture2D.EncodeToPNG ();
			//将图片赋给场景上的Sprite
			Sprite tempSp = Sprite.Create (texture2D, new Rect (0, 0, texture2D.width, texture2D.height), new Vector2 (0, 0));
			headerIcon.sprite = tempSp;
		} else {
			MyDebug.Log ("没有加载到图片");
		}
	}
	/// <summary>
	/// Shows the hu effect.
	/// </summary>
	public void showHuEffect(){
		huEffect.SetActive (true);
		HuFlag.SetActive (true);
	}

	public void hideHuImage(){
		HuFlag.SetActive (false);
	}
	/// <summary>
	/// Shows the peng effect.
	/// </summary>
	public void showPengEffect(){
		pengEffect.SetActive (true);
	}
	/// <summary>
	/// Shows the gang effect.
	/// </summary>
	public void showGangEffect(){
		gangEffect.SetActive (true);
	}

	public int getSex(){
		return avatarvo.sex;
	}
}
