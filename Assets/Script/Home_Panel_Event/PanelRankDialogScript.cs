using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelRankDialogScript : MonoBehaviour {
	public Transform itemParent;
	public Text selfRank;
	public Image selfHeaderIcon;
	public Text selfRankDes;
	public Text selfScore;
	public Button giftButton;
	// Use this for initialization
	void Start () {
		setRankData ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/**设置排行榜中的数据
	 * 
	 */ 
	public void setRankData(){
		for (int i = 0; i < 20; i++) {
			GameObject itemTemp = Instantiate (Resources.Load("Prefab/Rank_List_Item")) as GameObject;
			itemTemp.transform.parent = itemParent;
			itemTemp.transform.localScale = Vector3.one;
		}
	}

	/**
	*关闭对话框
	*/
	public void closeDialog(){
		Destroy(this.gameObject );
	}



}
