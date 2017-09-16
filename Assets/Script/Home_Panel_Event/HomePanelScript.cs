using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;

public class HomePanelScript : MonoBehaviour {
    public Image headIconImg;//头像路径
	//public Image tipHeadIcon;
	public Text noticeText;
	//public Text tipNameText;
//	public Text tipIdText;
	//public Text tipIpText;
    public Text nickNameText;//昵称
	public Text cardCountText;//房卡剩余数量
	public Text IpText;

	public Text contactInfoContent;

	//public GameObject userInfoPanel;
	public GameObject roomCardPanel;
	WWW www;                     //请求
	string filePath;             //保存的文件路径
	Texture2D texture2D;         //下载的图片
	private string headIcon;
	private GameObject panelCreateDialog;//界面上打开的dialog
	private GameObject panelExitDialog;
	/// <summary>
	/// 这个字段是作为消息显示的列表 ，如果要想通过管理后台随时修改通知信息，
	/// 请接收服务器的数据，并重新赋值给这个字段就行了。
	/// </summary>
	private bool startFlag = false;
	public  float waiteTime = 1;
	private float TimeNum = 0;
	private int showNum = 0;
    private int i;
    private int a=0;
	// Use this for initialization
	void Start () {
		initUI();
		GlobalDataScript.isonLoginPage = false;
		checkEnterInRoom ();
		addListener ();
	}
		

	void setNoticeTextMessage(){
		
		if (GlobalDataScript.noticeMegs != null && GlobalDataScript.noticeMegs.Count != 0) {
			noticeText.transform.localPosition = new Vector3 (500,noticeText.transform.localPosition.y);
			noticeText.text = GlobalDataScript.noticeMegs [showNum];
			float time = noticeText.text.Length*0.5f+422f/56f;

			Tweener tweener=noticeText.transform.DOLocalMove(
				new Vector3(-noticeText.text.Length*28, noticeText.transform.localPosition.y), time)
				.OnComplete(moveCompleted);
			tweener.SetEase (Ease.Linear);
			//tweener.SetLoops(-1);
		}

	}

	void moveCompleted(){
		showNum++;
		if (showNum == GlobalDataScript.noticeMegs.Count) {
			showNum = 0;
		}
		setNoticeTextMessage ();
	}
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Escape)){ //Android系统监听返回键，由于只有Android和ios系统所以无需对系统做判断
			MyDebug.Log ("Input.GetKey(KeyCode.Escape)");
			if(panelCreateDialog!=null){
				Destroy (panelCreateDialog);
				return;
			}else if (panelExitDialog == null) {
				panelExitDialog = Instantiate (Resources.Load("Prefab/Panel_Exit")) as GameObject;
				panelExitDialog.transform.parent = gameObject.transform;
				panelExitDialog.transform.localScale = Vector3.one;
				//panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
				panelExitDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
				panelExitDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
			}
		}

//		TimeNum += Time.deltaTime;
//		if(TimeNum >= waiteTime){
//			TimeNum = 0;
//			setNoticeTextMessage ();
//		}

	}

	//增加服务器返沪数据监听
	public void  addListener(){
		SocketEventHandle.getInstance ().cardChangeNotice += cardChangeNotice;
		SocketEventHandle.getInstance ().contactInfoResponse += contactInfoResponse;
			
	//	SocketEventHandle.getInstance ().gameBroadcastNotice += gameBroadcastNotice;
		CommonEvent.getInstance ().DisplayBroadcast += gameBroadcastNotice;
	}

	public void removeListener(){
		SocketEventHandle.getInstance ().cardChangeNotice -= cardChangeNotice;
		CommonEvent.getInstance ().DisplayBroadcast -= gameBroadcastNotice;
		SocketEventHandle.getInstance ().contactInfoResponse -= contactInfoResponse;
	//	SocketEventHandle.getInstance ().gameBroadcastNotice -= gameBroadcastNotice;
	}



	//房卡变化处理
	private void cardChangeNotice(ClientResponse response){
		cardCountText.text = response.message;
		GlobalDataScript.loginResponseData.account.roomcard =int.Parse(response.message);
	}

	private void gameBroadcastNotice(){
		showNum = 0;
		if(!startFlag){
			startFlag = true;
			setNoticeTextMessage ();
		}
	}

  
	private void contactInfoResponse(ClientResponse response){
		contactInfoContent.text = response.message;
		roomCardPanel.SetActive (true);
	}
	/***
	 *初始化显示界面 
	 */
	private void initUI(){
		if (GlobalDataScript.loginResponseData != null) {
			headIcon = GlobalDataScript.loginResponseData.account.headicon;
			string nickName = GlobalDataScript.loginResponseData.account.nickname;
			int roomCardcount = GlobalDataScript.loginResponseData.account.roomcard;
			cardCountText.text = roomCardcount+"";
			nickNameText.text = nickName;
			IpText.text = "ID:" + GlobalDataScript.loginResponseData.account.uuid;
			GlobalDataScript.loginResponseData.account.roomcard = roomCardcount;
		}
        StartCoroutine (LoadImg());
	//	CustomSocket.getInstance ().sendMsg (new GetContactInfoRequest ());

	}

	public void showUserInfoPanel(){
		//userInfoPanel.SetActive (true);
		GameObject obj= PrefabManage.loadPerfab("Prefab/userInfo");
		obj.GetComponent<ShowUserInfoScript> ().setUIData (GlobalDataScript.loginResponseData);
	}

	/**
	public void closeUserInfoPanel (){
		userInfoPanel.SetActive (false);
	}
*/
	public void showRoomCardPanel(){
		CustomSocket.getInstance ().sendMsg (new GetContactInfoRequest ());

	}

	public void closeRoomCardPanel (){
		roomCardPanel.SetActive (false);
	}

	/****
	 * 判断进入房间
	 */ 
	private void checkEnterInRoom(){
		if (GlobalDataScript.roomVo!= null && GlobalDataScript.roomVo.roomId != 0) {
			//loadPerfab ("Prefab/Panel_GamePlay");
			GlobalDataScript.gamePlayPanel = PrefabManage.loadPerfab ("Prefab/Panel_GamePlay");
		}

	}


	/***
	 * 打开创建房间的对话框
	 * 
	 */ 
	public void openCreateRoomDialog(){
		if (GlobalDataScript.loginResponseData == null || GlobalDataScript.loginResponseData.roomId == 0) {
			loadPerfab ("Prefab/Panel_Create_Dialog");
		} else {
		
			TipsManagerScript.getInstance ().setTips("当前正在房间状态，无法创建房间");
		}




		//Application.LoadLevel ("Play_Scene");
	}

	/***
	 * 打开进入房间的对话框
	 * 
	 */ 
	public void openEnterRoomDialog(){
		
		if (GlobalDataScript.roomVo == null || GlobalDataScript.roomVo.roomId == 0) {
			loadPerfab ("Prefab/Panel_Enter_Room");

		} else {
			TipsManagerScript.getInstance ().setTips("当前正在房间状态，无法加入新的房间");
		}
	}

	/**
	 * 打开游戏规则对话框
	 */ 
	public void openGameRuleDialog(){
		
		loadPerfab ("Prefab/Panel_Game_Rule_Dialog");
	}

	/**
	 * 打开游戏规则对话框
	 */ 
	public void openGameRankDialog(){
		loadPerfab ("Prefab/Panel_Rank_Dialog");
	}


	/**
	 * 打开抽奖对话框
	 * 
	*/
	public void LotteryBtnClick()
	{
		loadPerfab ("Prefab/Panel_Lottery");
	}

    public void ZhanjiBtnClick()
    {
        loadPerfab("Prefab/Panel_Report");
    }

	private void  loadPerfab(string perfabName){
		panelCreateDialog = Instantiate (Resources.Load(perfabName)) as GameObject;
		panelCreateDialog.transform.parent = gameObject.transform;
		panelCreateDialog.transform.localScale = Vector3.one;
		//panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
		panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
	}


	private IEnumerator LoadImg() { 
		//开始下载图片
		if (headIcon != null && headIcon != "") {
			WWW www = new WWW(headIcon);
			yield return www;
			//下载完成，保存图片到路径filePath
			try {
				texture2D = www.texture;
				byte[] bytes = texture2D.EncodeToPNG();
				//将图片赋给场景上的Sprite
				Sprite tempSp = Sprite.Create(texture2D, new Rect(0,0,texture2D.width,texture2D.height),new Vector2(0,0));
				headIconImg.sprite = tempSp;

			} catch (Exception e){
				
				MyDebug.Log ("LoadImg"+e.Message);
			}
		}
	}



	public void exitApp(){
		if (panelExitDialog == null) {
			panelExitDialog = Instantiate (Resources.Load("Prefab/Panel_Exit")) as GameObject;
			panelExitDialog.transform.parent = gameObject.transform;
			panelExitDialog.transform.localScale = Vector3.one;
			//panelCreateDialog.transform.localPosition = new Vector3 (200f,150f);
			panelExitDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
			panelExitDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
		}
	}

}
