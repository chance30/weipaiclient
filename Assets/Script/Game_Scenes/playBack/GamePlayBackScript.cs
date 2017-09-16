using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;
using LitJson;

public class GamePlayBackScript : MonoBehaviour {

	public double lastTime;
	public Text Number;
	public Text roomRemark;
	public GameObject liujuEffectGame;
	public int otherPengCard;
	public int otherGangCard;
	public List<Transform> parentList;
	public List<Transform> outparentList;
	public List<GameObject> dirGameList;
	public List<PlayerItemBackScript> playerItems;
	public Text LeavedCastNumText;//剩余牌的张数
	public Text LeavedRoundNumText;//剩余局数
	public GameObject playBtn;
	public GameObject stopBtn;
	public Text processText;
	//public int StartRoundNum;
	public List<Transform> pengGangParentList;
	public GameObject genZhuang;
	public GameObject exitPanel;
	//======================================
	private float timer = 0;
	private int LeavedCardsNum;
	private int MoPaiCardPoint;
	private int cardCount;
	private GameObject zhuamaPanel;
	private List<List<List<GameObject>>> pengGangLists;
	private List<List<int>> indexList;
	/// <summary>
	/// 手牌数组，0自己，1-右边。2-上边。3-左边
	/// </summary>
	public List<List<GameObject>> handerCardList;
	/// <summary>
	/// 打在桌子上的牌
	/// </summary>
	public List<List<GameObject>> tableCardList;

	private int timeNum = 0;
	private int stepNum = 0;
	private int count= 0;
	private bool play = false;
	private GamePlayResponseVo aa = new GamePlayResponseVo ();
	/// <summary>
	/// 使用过的行为数组
	/// </summary>
	private List<GameBehaviourVO> usedList;
	private string[] pathDirString = new string[]{"B","R","T","L" };
	private int myIndex;

	private int roomType;//房间类型

	// Use this for initialization
	void Start () {
		addListener ();
		initArrayList ();
	}

	private void addListener(){
		SocketEventHandle.getInstance ().gameBackPlayResponse += gameBackPlayResponse;
	}

	private  void gameBackPlayResponse(ClientResponse response){
		aa = JsonMapper.ToObject<GamePlayResponseVo> (response.message);
		setRoomRemark (aa.roomvo);
		getMyIndex ();
		chongZu ();
		initall ();
		initCard ();
	}
	void initall(){
		for (int i = 0; i < aa.playerItems.Count; i++) {
			PlayerBackVO player = aa.playerItems [i];
			playerItems [i].setAvatarVo (player);
		}
		int indexCount = 0;
	}
	private void initCard(){
		for (int i = 0; i <aa.playerItems.Count; i++) {
			List<GameObject> cards = new List<GameObject> ();
			int[] tempPai = aa.playerItems [i].getPaiArray ();
			int aaa = 0;
			for (int a = 0; a <tempPai.Length; a++) {
				if (tempPai [a] > 0) {
					GameObject temp = null;
					for (int b = 0; b < tempPai [a]; b++) {
						temp = createGameObjectAndReturn ("Prefab/playBack/HandCard_"+pathDirString[i], parentList [i], Vector3.one);
						if (pathDirString [i] == DirectionEnum.Left || pathDirString [i] == DirectionEnum.Right) {
							temp.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (a);
						} else {
							temp.GetComponent<TopAndBottomCardScript> ().setPoint (a);
						}

						if (i == 1) {
							temp.transform.SetSiblingIndex (0);
						}
						aaa++;
						cards.Add (temp);
					}
				}
			}
			handerCardList.Add (cards);
		}
		setPosition ();
		play = true;
	}


	private void initArrayList(){
		usedList = new List<GameBehaviourVO> ();
		handerCardList = new List<List<GameObject>> ();
		tableCardList = new List<List<GameObject>> ();
		pengGangLists = new List<List<List<GameObject>>> ();
		indexList = new List<List<int>> ();
		for (int i = 0; i < 4; i++) {
			List<int> temp = new List<int> ();
			for(int k=0;k<4;k++){
				int n = i + k;
				if (n > 3) {
					n -= 4;
				}
				temp.Add (n);
			}
			if(i==0){
				indexList.Add (temp);
			}else{
				indexList.Insert (1,temp);
			}
		}
		for (int i = 0; i < 4; i++) {
			tableCardList.Add (new List<GameObject>());
			pengGangLists.Add (new List<List<GameObject>>());
		}
	}

	// Update is called once per frame
	void Update () {
		if (play) {
			timeNum--;
			if (timeNum <= 0) {
				timeNum = 70;
				if (stepNum < aa.behavieList.Count) {
					processText.text = "播放进度：" + (int)(stepNum*100/aa.behavieList.Count) +"%";
					stepAction ();
				} else {
					processText.text = "播放进度：100%";
					play = false;
				}
			}
		}
	}

	private void stepAction(){
		GameBehaviourVO temp = aa.behavieList[stepNum];
		usedList.Insert (0,temp);
		int tempcurIndex = getCurIndex (temp.accountindex_id);
		if (temp.type == 1) {
			outCard (tempcurIndex, int.Parse (temp.cardIndex));
		} else if (temp.type == 2) {
			pickCard (tempcurIndex, int.Parse (temp.cardIndex));
		} else if (temp.type == 4) {
			pengCard (tempcurIndex, int.Parse (temp.cardIndex));
		} else if (temp.type == 5) {
			gangCard (tempcurIndex, int.Parse (temp.cardIndex), temp.gangType);
		} else if (temp.type == 6) {
			huCard (tempcurIndex, int.Parse (temp.cardIndex));

		} else if(temp.type == 7){
			qiangGangHu (tempcurIndex, int.Parse (temp.cardIndex));
		}else if (temp.type == 8) {
			if (roomType == GameConfig.GAME_TYPE_HUASHUI) {//划水麻将不显示码框
			} else {
				zhuama (temp.ma,temp.valideMa);

			}

		}else if(temp.type == 9){
			liuju ();
		}
		SetDirGameObjectAction (tempcurIndex);
		stepNum++;
	}
	/// <summary>
	/// 摸牌
	/// </summary>
	/// <param name="avaIndex">Ava index.</param>
	/// <param name="cardPoint">Card point.</param>
	private void pickCard(int avaIndex,int cardPoint){
		GameObject tempObj = null;
		cardCount--;
		showCardNumber ();
		switch (avaIndex) {
		case 0:
			tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_B", parentList [avaIndex], new Vector3(520,0));
			tempObj.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);
			break;
		case 1:
			tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_R", parentList [avaIndex], new Vector3(0,250));
			tempObj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardPoint);
			break;
		case 2:
			tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_T", parentList [avaIndex], new Vector3(-260,0));
			tempObj.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);
			break;
		case 3:
			tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_L", parentList [avaIndex], new Vector3(0,-190));
			tempObj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardPoint);
			break;
		}

		handerCardList [avaIndex].Add (tempObj);
	}

	private void qiangGangHu(int avaIndex,int cardPoint){
		GameObject tempObj = null;
		switch (avaIndex) {
		case 0:
			tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_B", parentList [avaIndex], new Vector3(520,0));
			tempObj.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);
			break;
		case 1:
			tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_R", parentList [avaIndex], new Vector3(0,250));
			tempObj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardPoint);
			break;
		case 2:
			tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_T", parentList [avaIndex], new Vector3(-260,0));
			tempObj.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);
			break;
		case 3:
			tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_L", parentList [avaIndex], new Vector3(0,-190));
			tempObj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardPoint);
			break;
		}

		handerCardList [avaIndex].Add (tempObj);
		playerItems [avaIndex].showHuEffect ();
		SoundCtrl.getInstance ().playSoundByAction ("hu",playerItems[avaIndex].getSex());

	}
	/// <summary>
	/// 出牌
	/// </summary>
	/// <param name="avaIndex">Ava index.</param>
	/// <param name="cardPoint">Card point.</param>
	private void outCard(int avaIndex,int cardPoint){
		List<GameObject> tempCardObjs = handerCardList[avaIndex];
		GameObject lastCard = tempCardObjs [tempCardObjs.Count - 1];
		int lastPoint = lastCard.GetComponent<TopAndBottomCardScript> ().getPoint ();
		if (cardPoint == lastPoint) {
			tempCardObjs.Remove (lastCard);
			Destroy (lastCard);
		} else {
			for(int i=0;i<tempCardObjs.Count;i++){
				int tempPoint = tempCardObjs [i].GetComponent<TopAndBottomCardScript> ().getPoint ();
				if (tempPoint == cardPoint) {
					GameObject removeItem = tempCardObjs [i];
					tempCardObjs.RemoveAt (i);
					GameObject.Destroy (removeItem);
					break;
				}
			}
			paiSort (tempCardObjs,avaIndex);
			setPosition ();
		}
		createPutOutCardAndPlayAction (cardPoint,avaIndex);
	}

	/// <summary>
	/// 创建打来的的牌对象，并且开始播放动画
	/// </summary>
	/// <param name="cardPoint">Card point.</param>
	/// <param name="curAvatarIndex">Current avatar index.</param>
	private void createPutOutCardAndPlayAction(int cardPoint, int curAvatarIndex)
	{
		SoundCtrl.getInstance ().playSound (cardPoint,playerItems[curAvatarIndex].getSex());
		Vector3 tempVector3 = new Vector3(0, 0);

		switch (curAvatarIndex)
		{
			case 0:
				tempVector3 = new Vector3(0, 180);
				break;
			case 1: //右
				tempVector3 = new Vector3(-100, 0f);
				break;
			case 2: //上
				tempVector3 = new Vector3(0, -180f);
				break;
			case 3: //左
				tempVector3 = new Vector3(100, 0f);
				break;
		}

		GameObject tempGameObject = createGameObjectAndReturn ("Prefab/card/PutOutCard",parentList[curAvatarIndex],tempVector3);
		tempGameObject.name = "putOutCard";
		tempGameObject.transform.localScale = Vector3.one;

		tempGameObject.GetComponent<TopAndBottomCardScript>().setPoint(cardPoint);
		ThrowBottom (cardPoint,curAvatarIndex);
		Destroy(tempGameObject, 1f);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="lists">Lists.</param>
	private void paiSort(List<GameObject> lists,int avaIndex){
		GameObject tempObject = lists[lists.Count-1];
		lists.Remove (tempObject);
		int lastPoint = tempObject.GetComponent<TopAndBottomCardScript> ().getPoint ();
		for(int i=0;i<lists.Count;i++){
			int curPoint = lists [i].GetComponent<TopAndBottomCardScript> ().getPoint ();
			if (lastPoint <= curPoint) {
				lists.Insert (i,tempObject);
				return;
			}
		}
		lists.Add (tempObject);
	}

	void setPosition(){
		for (int i = 0; i < handerCardList.Count; i++) {
			int len = handerCardList [i].Count;
			for (int a = 0; a <len; a++) {
				int tempNum = len - a-1;
				switch (i) {
				case 0:
					handerCardList [i] [a].transform.localPosition = new Vector3 (410 - tempNum * 79, 0);
					break;
				case 1:
					handerCardList [i] [a].transform.localPosition = new Vector3 (0, 200 - tempNum * 32);
					handerCardList [i] [a].transform.SetSiblingIndex (0);
					break;
				case 2:
					handerCardList [i] [a].transform.localPosition = new Vector3 (-190 + tempNum * 55, 0);
					break;
				case 3:
					handerCardList [i] [a].transform.localPosition = new Vector3 (0,-140+tempNum*32);
					handerCardList [i] [a].transform.SetSiblingIndex (a);
					break;
				}
			}
		}
	}

	public void setRoomRemark(RoomCreateVo roomvo){
		roomType = roomvo.roomType;
		string str = "房间号：\n"+roomvo.roomId+"\n";
		str += "圈数："+roomvo.roundNumber+"\n";
		if (roomvo.hong) {
			cardCount = 59;
			str += "红中麻将\n";
		} else {
			cardCount = 55;
			if (roomvo.roomType == 1) {
				str += "转转麻将\n";
			} else if (roomvo.roomType == 2){
				str += "划水麻将\n";
			}else if (roomvo.roomType == 3){
				str += "长沙麻将\n";
			}
		}
		if (roomvo.ziMo == 1) {
			str += "只能自摸\n";
		} else {
			str += "可抢杠胡\n";
		}
		if(roomvo.sevenDouble){
			str += "可胡七对\n";
		}

		if (roomvo.addWordCard) {
			str += "有风牌\n";
			cardCount = 83;
		}
		if (roomvo.xiaYu > 0) {
			str += "下鱼数：" + roomvo.xiaYu+"\n";
		}

		if (roomvo.ma > 0) {
			str += "抓码数：" + roomvo.ma+"\n";
		}
		roomRemark.text = str;
		showCardNumber ();
	}

	public void play_Click(){
		play = true;
		playBtn.SetActive (false);
		stopBtn.SetActive (true);
	}

	public void stop_Click(){
		play = false;
		playBtn.SetActive (true);
		stopBtn.SetActive (false);
	}

	public void front_Click(){
		frontAction ();
	}

	private void frontAction(){
		if (usedList.Count > 0) {
			//play = false;
			GameBehaviourVO temp = usedList [0];
			usedList.RemoveAt (0);
			int tempcurIndex = getCurIndex (temp.accountindex_id);
			if (temp.type == 1) {
				frontIncard (tempcurIndex, int.Parse (temp.cardIndex));
			} else if (temp.type == 2) {
				stepNum--;
				frontOutcard (tempcurIndex, int.Parse (temp.cardIndex));
				cardCount++;
				showCardNumber ();
				frontAction ();
				return;
			} else if (temp.type == 4) {
				frontPengCard (tempcurIndex, int.Parse (temp.cardIndex));
			} else if (temp.type == 5) {
				frontGangCard (tempcurIndex, int.Parse (temp.cardIndex), temp.gangType);
			} else if (temp.type == 6) {
				frontHuCard (tempcurIndex, int.Parse (temp.cardIndex));
			} else if (temp.type == 7) {
				frontOutcard (tempcurIndex, int.Parse (temp.cardIndex));
			}else if(temp.type == 8){
				//frontZhuaMa ();
			}else if(temp.type == 9){
				frontLiuju();
			}
			SetDirGameObjectAction (tempcurIndex);
			stepNum--;
			processText.text = "播放进度：" + (int)(stepNum*100/aa.behavieList.Count) +"%";
			//play = true;
		}
	}


	private void frontIncard(int avaIndex,int cardPoint){
		GameObject tempObj = null;
		tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_"+pathDirString[avaIndex], parentList [avaIndex], Vector3.one);
		if (pathDirString [avaIndex] == DirectionEnum.Left || pathDirString [avaIndex] == DirectionEnum.Right) {
			tempObj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardPoint);

		} else {
			tempObj.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);

		}

		List<GameObject> tempTables = tableCardList[avaIndex];
		if (tempTables.Count > 0) {
			GameObject temptablCard = tempTables [tempTables.Count - 1];
			tempTables.Remove (temptablCard);
			GameObject.Destroy (temptablCard);
		}
		handerCardList [avaIndex].Add (tempObj);
		paiSort (handerCardList [avaIndex],avaIndex);
		setPosition ();
	}
	private void frontHuCard(int avaIndex,int cardPoint){
		playerItems [avaIndex].hideHuImage ();
	}
		
	private void frontOutcard(int avaIndex,int cardPoint){
		removeFromList (handerCardList[avaIndex],cardPoint);
	}
	/// <summary>
	/// 把牌的对象从数组 中移除并销毁
	/// </summary>
	/// <param name="lists">Lists.</param>
	/// <param name="cardPoint">Card point.</param>
	private void removeFromList(List<GameObject> lists,int cardPoint){
		for (int i = 0; i < lists.Count; i++) {
			int indexPoint = lists [i].GetComponent<TopAndBottomCardScript> ().getPoint ();
			if (indexPoint == cardPoint) {
				GameObject temp = lists[i];
				lists.RemoveAt (i);
				GameObject.Destroy (temp);
				break;
			}
		}
		setPosition ();
	}

	public void next_Click(){
		timeNum = 1;
	}

	public void exit_Click(){
		exitPanel.SetActive (true);
	}

	public void exitSure_Click(){
		SocketEventHandle.getInstance ().gameBackPlayResponse -= gameBackPlayResponse;
		Destroy (this);
		Destroy (gameObject);
	}

	public void exitCancel_Click(){
		exitPanel.SetActive (false);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="path"></param>
	/// <param name="parent"></param>
	/// <param name="position"></param>
	/// <returns></returns>
	private GameObject createGameObjectAndReturn(string path,Transform parent,Vector3 position)
	{
		GameObject obj = Instantiate(Resources.Load(path)) as GameObject;
		obj.transform.SetParent (parent);
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = position;
		return obj;
	}
	/// <summary>
	/// Throws the bottom.
	/// </summary>
	/// <param name="index">Index.</param>
	public void ThrowBottom(int index,int avaIndex)//
	{
		GameObject temp = null;
		string path = "";
		Vector3 poisVector3 = Vector3.one;
		switch(avaIndex){
			case 0:
				path = "Prefab/ThrowCard/TopAndBottomCard";
				poisVector3 = new Vector3(-261 + tableCardList[0].Count%14*37, (int)(tableCardList[0].Count/14)*67f);
				break;
			case 1:
				path = "Prefab/ThrowCard/ThrowCard_R";
				poisVector3 = new Vector3((int)(-tableCardList[1].Count/13*46f), -180f + tableCardList[1].Count%13*28);
				break;
			case 2:
				path = "Prefab/ThrowCard/TopAndBottomCard";
				poisVector3 = new Vector3(289f - tableCardList[2].Count%14*37, -(int)(tableCardList[2].Count/14)*67f);
				break;
			case 3:
				path = "Prefab/ThrowCard/ThrowCard_L";
				poisVector3 = new Vector3(tableCardList[3].Count/13*46f, 152f - tableCardList[3].Count%13*28);
				break;
		}

		temp = createGameObjectAndReturn (path,outparentList[avaIndex],poisVector3);
		temp.transform.localScale = Vector3.one;
		if (avaIndex == 1 || avaIndex == 3) {
			temp.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (index);
		} else {
			temp.GetComponent<TopAndBottomCardScript>().setPoint(index);
		}

		//temp.transform.SetAsLastSibling();
		tableCardList[avaIndex].Add(temp);
		if (avaIndex == 1) {
			temp.transform.SetSiblingIndex(0);
		}
	}

	public void pengCard(int avaIndex,int cardPoint)//其他人碰牌
	{
		playerItems [avaIndex].showPengEffect ();
		SoundCtrl.getInstance ().playSoundByAction ("peng",playerItems[avaIndex].getSex());
		List<GameObject> tempCardList = handerCardList[avaIndex];
		string path="Prefab/PengGangCard/PengGangCard_"+pathDirString[avaIndex];
		Vector3 tempvector3 = new Vector3(0, 0, 0);
		for (int i = 0; i < tableCardList.Count; i++) {
			List<GameObject> temptableList = tableCardList[i];
			if (temptableList.Count > 0) {
				GameObject temp = temptableList[temptableList.Count - 1];
				int tempPoint = temp.GetComponent<TopAndBottomCardScript> ().getPoint ();
				if (tempPoint == cardPoint) {
					temptableList.Remove (temp);
					GameObject.Destroy (temp);
					break;
				}
			}
		}
		int num = 0;
		if (tempCardList != null) {
			for (int i = 0; i < tempCardList.Count; i++)//消除其他的人牌碰牌长度
			{
				GameObject temp = tempCardList [i];
				if(cardPoint == temp.GetComponent<TopAndBottomCardScript>().getPoint()){
					tempCardList.Remove(temp);
					Destroy(temp);
					num++;
					if (num == 2) {
						break;
					} else {
						i--;
					}
				}
			}
			setPosition ();
		}
		List<GameObject> tempList = new List<GameObject> ();
		if (path != null)
		{
			for (int i = 0; i < 3; i++) //实例化其他人碰牌
			{
				GameObject obj = Instantiate(Resources.Load(path)) as GameObject;

				switch (avaIndex)
				{
					case 0:
						obj.GetComponent<TopAndBottomCardScript>().setPoint(cardPoint);
						tempvector3 = new Vector3(-380 + pengGangLists[avaIndex].Count*200 + i*60,0);
						obj.transform.parent = pengGangParentList[avaIndex];
						break;
					case 1:
					obj.GetComponent<TopAndBottomCardScript>().setLefAndRightPoint(cardPoint);
						tempvector3 = new Vector3(0, -116 + pengGangLists[avaIndex].Count*90 + i*26f);
						obj.transform.parent = pengGangParentList[avaIndex];
						obj.transform.SetSiblingIndex(0);
						break;
					case 2:
						obj.GetComponent<TopAndBottomCardScript>().setPoint(cardPoint);
						tempvector3 = new Vector3(231 - pengGangLists[avaIndex].Count*120f + i*37, 0, 0);
						obj.transform.parent = pengGangParentList[avaIndex];
						break;
					case 3:
						obj.GetComponent<TopAndBottomCardScript>().setLefAndRightPoint(cardPoint);
						tempvector3 = new Vector3(0, 142 - pengGangLists[avaIndex].Count*90f - i*26f, 0);
						obj.transform.parent = pengGangParentList[avaIndex];
						break;
				}
				obj.transform.localScale = Vector3.one;
				obj.transform.localPosition = tempvector3;
				tempList.Add(obj);
			}
			pengGangLists [avaIndex].Add (tempList);
		}

	}

	private void huCard(int avaIndex,int cardPoint){
		playerItems [avaIndex].showHuEffect ();
		SoundCtrl.getInstance ().playSoundByAction ("hu",playerItems[avaIndex].getSex());
	}

	/// <summary>
	/// Gangs the card.
	/// </summary>
	/// <param name="avaIndex">Ava index.</param>
	/// <param name="cardPoint">Card point.</param>
	/// <param name="gangType">Gang type.</param>
	private void gangCard(int avaIndex,int cardPoint,int gangType){
		playerItems [avaIndex].showGangEffect ();
		SoundCtrl.getInstance ().playSoundByAction ("gang",playerItems[avaIndex].getSex());
		List<GameObject> tempCardList = handerCardList[avaIndex];
		string path="Prefab/PengGangCard/PengGangCard_"+pathDirString[avaIndex];
		Vector3 tempvector3 = new Vector3(0, 0, 0);
		if (gangType != 3) {
			if(gangType == 4){
				if (tempCardList != null) {
					for (int i = 0; i < tempCardList.Count; i++)//消除其他的人牌碰牌长度
					{
						GameObject temp = tempCardList [i];
						if(cardPoint == temp.GetComponent<TopAndBottomCardScript>().getPoint()){
							tempCardList.Remove(temp);
							Destroy(temp);
						}
					}
					setPosition ();
				}
				return;
			}
			if (tempCardList != null) {
				int num = 0;
				for (int i = 0; i < tempCardList.Count; i++)//消除其他的人牌碰牌长度
				{
					GameObject temp = tempCardList [i];
					if(cardPoint == temp.GetComponent<TopAndBottomCardScript>().getPoint()){
						tempCardList.Remove(temp);
						Destroy(temp);
						num++;
						if (num == 3) {
							break;
						} else {
							i--;
						}
					}
				}
				setPosition ();
			}
			List<GameObject> tempList = new List<GameObject> ();
			if (path != null) {
				for (int i = 0; i < 4; i++) { //实例化其他人碰牌
					switch (avaIndex) {
					case 0:
						tempvector3 = new Vector3 (-380 + pengGangLists [avaIndex].Count * 200 + i * 60, 0);
						break;
					case 1:
						tempvector3 = new Vector3 (0, -116 + pengGangLists [avaIndex].Count * 90 + i * 26f);
						break;
					case 2:
						tempvector3 = new Vector3 (231 - pengGangLists [avaIndex].Count * 120f + i * 37, 0, 0);
						break;
					case 3:
						tempvector3 = new Vector3 (0, 142 - pengGangLists [avaIndex].Count * 90f - i * 26f, 0);
						break;
					}
					GameObject obj = createGameObjectAndReturn (path, pengGangParentList [avaIndex], tempvector3);
					if (avaIndex == 1 || avaIndex == 3) {
						obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardPoint);
					} else {
						obj.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);
					}

					obj.transform.localScale = Vector3.one;
					if (avaIndex == 1) {
						obj.transform.SetSiblingIndex (0);
					}
					if(i == 3){
						switch (avaIndex) {
						case 0:
							obj.transform.localPosition = new Vector3 (-380 + pengGangLists [avaIndex].Count * 200 + 60,28);
							break;
						case 1:
							obj.transform.localPosition = new Vector3 (0, -116 + pengGangLists [avaIndex].Count * 90 + 36f);
							obj.transform.SetSiblingIndex (tempList.Count);
							break;
						case 2:
							obj.transform.localPosition = new Vector3 (231 - pengGangLists [avaIndex].Count * 120f +  37, 17, 0);
							break;
						case 3:
							obj.transform.localPosition = new Vector3 (0, 142 - pengGangLists [avaIndex].Count * 90f -  16f, 0);
							break;
						}
					}
					tempList.Add (obj);
				}
				pengGangLists [avaIndex].Add (tempList);
				if (gangType == 1) {
					for (int i = 0; i < tableCardList.Count; i++) {
						List<GameObject> temptableList = tableCardList[i];
						if (temptableList.Count > 0) {
							GameObject temp = temptableList[temptableList.Count - 1];
							int tempPoint = temp.GetComponent<TopAndBottomCardScript> ().getPoint ();
							if (tempPoint == cardPoint) {
								temptableList.Remove (temp);
								GameObject.Destroy (temp);
								break;
							}
						}
					}
				} else {
					removeFromList (handerCardList[avaIndex],cardPoint);
				}
			}
		} else {
			List<List<GameObject>> curList = pengGangLists[avaIndex];
			for (int i = 0; i < curList.Count; i++) {
				GameObject tempobj = curList [i] [0];
				int tempPoint = tempobj.GetComponent<TopAndBottomCardScript> ().getPoint ();
				if (tempPoint == cardPoint) {
					GameObject obj = createGameObjectAndReturn (path, pengGangParentList [avaIndex], tempvector3);

					obj.transform.localScale = Vector3.one;
					switch (avaIndex) {
					case 0:
						obj.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);
						obj.transform.localPosition = new Vector3 (tempobj.transform.localPosition.x+60,24);
						break;
					case 1:
						obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardPoint);
						obj.transform.localPosition = new Vector3 (0, tempobj.transform.localPosition.y + 37);
						break;
					case 2:
						obj.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);
						obj.transform.localPosition = new Vector3 (tempobj.transform.localPosition.x+37, 17);
						break;
					case 3:
						obj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardPoint);
						obj.transform.localPosition = new Vector3 (0,tempobj.transform.localPosition.y - 17, 0);
						break;
					}
					curList [i].Add (obj);
					break;
				}
			}
			removeFromList (handerCardList[avaIndex],cardPoint);
		}
	}
		
	private void frontPengCard(int avaIndex,int cardPoint){
		List<List<GameObject>> tempList = pengGangLists [avaIndex];
		if (tempList.Count > 0) {
			List<GameObject> list = tempList[tempList.Count -1];
			tempList.Remove (list);
			if (list.Count > 0) {
				while (list.Count > 0) {
					GameObject obj = list [0];
					list.Remove (obj);
					GameObject.Destroy (obj);
				}
			}
		}
		for(int i=0;i<2;i++){
			GameObject tempObj = null;
			tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_"+pathDirString[avaIndex], parentList [avaIndex], Vector3.one);
			if (avaIndex == 1 || avaIndex == 3) {
				tempObj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardPoint);
			} else {
				tempObj.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);
			}
		
			handerCardList [avaIndex].Add (tempObj);
			paiSort (handerCardList [avaIndex],avaIndex);
		}
		setPosition ();
	}

	private void frontGangCard(int avaIndex,int cardPoint,int gangType){
		List<List<GameObject>> tempList = pengGangLists [avaIndex];

		if (gangType != 3) {
			if (gangType == 4) {
				GameObject tempObj = null;
				tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_" + pathDirString [avaIndex], parentList [avaIndex], Vector3.one);
				if (avaIndex == 1 || avaIndex == 3) {
					tempObj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardPoint);
				} else {
					tempObj.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);
				}
				handerCardList [avaIndex].Add (tempObj);
				paiSort (handerCardList [avaIndex], avaIndex);
				setPosition ();
			} else {
				if (tempList.Count > 0) {
					List<GameObject> list = tempList [tempList.Count - 1];
					tempList.Remove (list);
					if (list.Count > 0) {
						while (list.Count > 0) {
							GameObject obj = list [0];
							list.Remove (obj);
							GameObject.Destroy (obj);
						}
					}
				}
				for (int i = 0; i < 3; i++) {
					GameObject tempObj = null;
					tempObj = createGameObjectAndReturn ("Prefab/playBack/HandCard_" + pathDirString [avaIndex], parentList [avaIndex], Vector3.one);
					if (avaIndex == 1 || avaIndex == 3) {
						tempObj.GetComponent<TopAndBottomCardScript> ().setLefAndRightPoint (cardPoint);
					} else {
						tempObj.GetComponent<TopAndBottomCardScript> ().setPoint (cardPoint);
					}

					handerCardList [avaIndex].Add (tempObj);
					paiSort (handerCardList [avaIndex], avaIndex);
				}
				setPosition ();
				if (gangType == 2) {
					pickCard (avaIndex, cardPoint);
				}
			}
		} else {
			if (tempList.Count > 0) {
				List<GameObject> list = tempList [tempList.Count - 1];
				if (list.Count > 0) {
					GameObject gob = list [list.Count - 1];
					list.Remove (gob);
					GameObject.Destroy (gob);
				}
				pickCard (avaIndex, cardPoint);
			}
		}
	}

	/// <summary>
	/// 设置红色箭头的显示方向
	/// </summary>
	public void SetDirGameObjectAction(int avaIndex) //设置方向
	{
		//UpateTimeReStart();
		for (int i = 0; i < dirGameList.Count; i++) {
			dirGameList [i].SetActive (false);
		}
		dirGameList[avaIndex].SetActive(true);
	}

	private void showCardNumber(){
		LeavedCastNumText.text = cardCount + "";
	}

	private void zhuama(string allMas,List<int> vailedMa){
		if (allMas == "" || allMas == null) {
			return;
		}
		if (zhuamaPanel == null) {
			List<AvatarVO> avatarList = new List<AvatarVO> ();
			int tempCount = 0;
			for (int i = 0; i < 4; i++) {
				AvatarVO avo = new AvatarVO ();
				avo.account = new Account ();
				avo.account.uuid = aa.playerItems [i].uuid;
				avatarList.Add (avo);
			}
			zhuamaPanel = PrefabManage.loadPerfab ("prefab/Panel_ZhuaMa");
			zhuamaPanel.GetComponent<ZhuMaScript> ().arrageMas (allMas, avatarList, vailedMa);
		}
		Invoke ("frontZhuaMa",7);
	}

	private void chongZu(){
		List<PlayerBackVO> templist = new List<PlayerBackVO> ();
		for (int i = 0; i < 4; i++) {
			int n = myIndex + i;
			if (n > 3) {
				n -= 4;
			}
			templist.Add (aa.playerItems[n]);
		}
		aa.playerItems = templist;
	}

	private void getMyIndex(){
		for(int i=0;i<aa.playerItems.Count;i++){
			if(aa.playerItems [i].uuid == GlobalDataScript.loginResponseData.account.uuid){
				myIndex =  i;
			}
		}
	}

	private void frontZhuaMa(){
		if (zhuamaPanel != null) {
			GameObject.Destroy (zhuamaPanel);
			zhuamaPanel = null;
		}
	}

	private void liuju(){
		liujuEffectGame.SetActive (true);
	}

	private void frontLiuju(){
		liujuEffectGame.SetActive (false);
	}

	private int getCurIndex(int index){
		return indexList[myIndex][index];
	}
}
